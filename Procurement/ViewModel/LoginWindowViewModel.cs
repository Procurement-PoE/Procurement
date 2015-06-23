﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Security;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using POEApi.Infrastructure;
using POEApi.Infrastructure.Events;
using POEApi.Model;
using POEApi.Model.Events;
using Procurement.View;
using Procurement.Utility;
using System.Windows;

namespace Procurement.ViewModel
{
    public class LoginWindowViewModel : INotifyPropertyChanged
    {
        private static bool authOffLine;

        private LoginView view = null;
        private StatusController statusController;
        public event LoginCompleted OnLoginCompleted;
        public delegate void LoginCompleted();
        private bool formChanged = false;
        private bool useSession;
        
        public event PropertyChangedEventHandler PropertyChanged;

        private CharacterTabInjector characterInjector;

        protected void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        private string email;
        public string Email
        {
            get { return email; }
            set
            {
                if (value != email)
                {
                    email = value;
                    OnPropertyChanged("Email");
                }
            }
        }

        private static string serverType;
        public static string ServerType
        {
            get { return serverType; }
            set
            {
                if (value != serverType)
                {
                    serverType = value;
                }
            }
        }

        public bool UseSession
        {
            get { return useSession; }
            set
            {
                useSession = value;
                Settings.UserSettings["UseSessionID"] = value.ToString();
                updateButtonLabels(useSession);
            }
        }

        private void updateButtonLabels(bool useSession)
        {
            if (this.view == null)
                return;

            if (((ContentControl)this.view.cmbRealmType.SelectedValue).Content.ToString() == "International")
            {
                this.view.lblEmail.Content = useSession ? "Alias" : "Email";
                this.view.lblPassword.Content = useSession ? "Session ID" : "Password";
            }
        }

        public LoginWindowViewModel(UserControl view)
        {
            this.view = view as LoginView;

            UseSession = Settings.UserSettings.ContainsKey("UseSessionID") ? bool.Parse(Settings.UserSettings["UseSessionID"]) : false;

            Email = Settings.UserSettings["AccountLogin"];
            ServerType = ((ContentControl)this.view.cmbRealmType.SelectedValue).Content.ToString();
            this.formChanged = string.IsNullOrEmpty(Settings.UserSettings["AccountPassword"]);

            if (!this.formChanged)
                this.view.txtPassword.Password = string.Empty.PadLeft(8); //For the visuals

            this.view.txtPassword.PasswordChanged += new System.Windows.RoutedEventHandler(txtPassword_PasswordChanged);

            characterInjector = new CharacterTabInjector();

            statusController = new StatusController(this.view.StatusBox);

            ApplicationState.Model.Authenticating += model_Authenticating;
            ApplicationState.Model.Throttled += model_Throttled;
            ApplicationState.InitializeFont(Properties.Resources.fontin_regular_webfont);
            ApplicationState.InitializeFont(Properties.Resources.fontin_smallcaps_webfont);

            statusController.DisplayMessage(ApplicationState.Version + " Medved Edition Initialized.\r");

            VersionChecker.CheckForUpdates();
        }

        void txtPassword_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            this.formChanged = true;
        }

        public void Login(bool offline, string server_type)
        {
            toggleControls();

            if ((string.IsNullOrEmpty(Email) || Email.ToLower() == "noemail") && server_type == "International")
            {
                MessageBox.Show(string.Format("{0} is required!", useSession ? "Alias" : "Email"), Procurement.MessagesRes.ErrorLoggingIn, MessageBoxButton.OK, MessageBoxImage.Stop);
                toggleControls();
                return;
            }

            if (this.view.txtPassword.SecurePassword.Length<1 && !offline)
            {
                MessageBox.Show(string.Format(Procurement.MessagesRes.IsRequiredPassword, ServerType=="International" ? "Password" : "Session ID"), Procurement.MessagesRes.ErrorLoggingIn, MessageBoxButton.OK, MessageBoxImage.Stop);
                toggleControls();
                return;
            }

            if (!offline)
            {
                ApplicationState.Model.StashLoading += model_StashLoading;
                ApplicationState.Model.ImageLoading += model_ImageLoading;
            }


            Task.Factory.StartNew(() =>
            {
                SecureString password = formChanged ? this.view.txtPassword.SecurePassword : Settings.UserSettings["AccountPassword"].Decrypt();
                ApplicationState.Model.Authenticate(Email, password, offline, useSession, server_type);

                if (formChanged)
                    saveSettings(password);

                if (!offline)
                {
                    ApplicationState.Model.ForceRefresh();
                    statusController.DisplayMessage(Procurement.MessagesRes.LoadingCharacters);
                }
                else
                {
                    statusController.DisplayMessage(Procurement.MessagesRes.LoadingProcurementInOfflineMode);
                }

                List<Character> chars;
                try
                {
                    chars = ApplicationState.Model.GetCharacters();
                }
                catch (WebException wex)
                {
                    Logger.Log(wex);
                    statusController.NotOK();
                    throw new Exception(Procurement.MessagesRes.FailedToLoadCharacters, wex.InnerException);
                }
                statusController.Ok();

                updateCharactersByLeague(chars);

                var items = LoadItems(offline, chars).ToList();

                ApplicationState.Model.GetImages(items);

                ApplicationState.SetDefaults();

                if (!offline)
                {
                    statusController.DisplayMessage(Procurement.MessagesRes.DoneLoading);
                    PoeTradeOnlineHelper.Instance.Start();
                }

                ApplicationState.Model.Authenticating -= model_Authenticating;
                ApplicationState.Model.StashLoading -= model_StashLoading;
                ApplicationState.Model.ImageLoading -= model_ImageLoading;
                ApplicationState.Model.Throttled -= model_Throttled;
                OnLoginCompleted();
            }).ContinueWith(t => { Logger.Log(t.Exception.InnerException.ToString()); statusController.HandleError(t.Exception.InnerException.Message, toggleControls); }, TaskContinuationOptions.OnlyOnFaulted);
        }

        private IEnumerable<Item> LoadItems(bool offline, IEnumerable<Character> chars)
        {
            bool downloadOnlyMyLeagues = (Settings.UserSettings.ContainsKey("DownloadOnlyMyLeagues") &&
                                          bool.TryParse(Settings.UserSettings["DownloadOnlyMyLeagues"], out downloadOnlyMyLeagues) &&
                                          downloadOnlyMyLeagues &&
                                          Settings.Lists.ContainsKey("MyLeagues") &&
                                          Settings.Lists["MyLeagues"].Count > 0);

            foreach (var character in chars)
            {
                if (character.League == "Void")
                    continue;

                if (downloadOnlyMyLeagues && !Settings.Lists["MyLeagues"].Contains(character.League))
                    continue;

                foreach (var item in LoadStashItems(character))
                    yield return item;

                foreach (var item in LoadCharacterInventoryItems(character, offline).Where(i => i.InventoryId != "MainInventory"))
                    yield return item;
            }

            if (downloadOnlyMyLeagues && ApplicationState.Characters.Count == 0)
                throw new Exception(Procurement.MessagesRes.NoCharactersFoundInTheLeaguesSpecified);


            characterInjector.Inject();
        }

        private static void updateCharactersByLeague(List<Character> chars)
        {
            var allLeagues = chars.Select(c => c.League).Distinct();

            foreach (var league in allLeagues)
                ApplicationState.AllCharactersByLeague[league] = new List<string>();

            if (Settings.Lists.ContainsKey("MyLeagues"))
                foreach (var league in Settings.Lists["MyLeagues"])
                    if (!ApplicationState.AllCharactersByLeague.ContainsKey(league))
                        ApplicationState.AllCharactersByLeague[league] = new List<string>();

            foreach (var character in chars)
                ApplicationState.AllCharactersByLeague[character.League].Add(character.Name);
        }

        private void saveSettings(SecureString password)
        {
            if (!formChanged)
                return;

            Settings.UserSettings["AccountLogin"] = Email;
            Settings.UserSettings["AccountPassword"] = password.Encrypt();
            Settings.UserSettings["UseSessionID"] = useSession.ToString();
            Settings.Save();
        }

        private void toggleControls()
        {
            view.LoginButton.IsEnabled = !view.LoginButton.IsEnabled;
            view.OfflineButton.IsEnabled = !view.OfflineButton.IsEnabled;
            view.txtLogin.IsEnabled = !view.txtLogin.IsEnabled;
            view.txtPassword.IsEnabled = !view.txtPassword.IsEnabled;
            view.cmbRealmType.IsEnabled = !view.cmbRealmType.IsEnabled;
        }

        private IEnumerable<Item> LoadStashItems(Character character)
        {
            if (ApplicationState.Leagues.Contains(character.League))
                return Enumerable.Empty<Item>();

            ApplicationState.CurrentLeague = character.League;
            ApplicationState.Stash[character.League] = ApplicationState.Model.GetStash(character.League);
            ApplicationState.Leagues.Add(character.League);

            return ApplicationState.Stash[character.League].Get<Item>();
        }

        private IEnumerable<Item> LoadCharacterInventoryItems(Character character, bool offline)
        {
            bool success;

            if (!offline)
                statusController.DisplayMessage((string.Format(Procurement.MessagesRes.Loading0SInventory, character.Name)));

            List<Item> inventory;
            try
            {
                inventory = ApplicationState.Model.GetInventory(character.Name, false);
                success = true;
            }
            catch (WebException)
            {
                inventory = new List<Item>();
                success = false;
            }

            characterInjector.Add(character, inventory);
            updateStatus(success, offline);

            return inventory;
        }

        private void updateStatus(bool success, bool offline)
        {
            if (offline)
                return;

            if (success)
                statusController.Ok();
            else
                statusController.NotOK();
        }

        void model_StashLoading(POEModel sender, StashLoadedEventArgs e)
        {
            update(Procurement.MessagesRes.LoadingStashTab1 + ApplicationState.CurrentLeague + Procurement.MessagesRes.LoadingStashTab2 + (e.StashID + 1) + "...", e);
        }

        void model_ImageLoading(POEModel sender, ImageLoadedEventArgs e)
        {
            update(Procurement.MessagesRes.LoadingImageFor + e.URL, e);
        }

        void model_Authenticating(POEModel sender, AuthenticateEventArgs e)
        {
            update(Procurement.MessagesRes.Authenticating + e.Email, e);
            if (e.State == POEEventState.AfterEvent) update(Procurement.MessagesRes.LoggedAsAuth + e.AccName, new POEEventArgs(POEEventState.BeforeEvent));
        }

        void model_Throttled(object sender, ThottledEventArgs e)
        {
            if (e.WaitTime.TotalSeconds > 4)
                update(string.Format(Procurement.MessagesRes.GGGServerRequestLimitHitThrottlingActivated, e.WaitTime.Seconds), new POEEventArgs(POEEventState.BeforeEvent));
        }

        private void update(string message, POEEventArgs e)
        {
            if (e.State == POEEventState.BeforeEvent)
            {
                statusController.DisplayMessage(message);
                return;
            }

            statusController.Ok();
        }
    }
}