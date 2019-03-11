using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Security;
using System.Threading.Tasks;
using System.Windows.Controls;
using POEApi.Infrastructure;
using POEApi.Infrastructure.Events;
using POEApi.Model;
using POEApi.Model.Events;
using Procurement.View;
using Procurement.Utility;
using System.Windows;
using POEApi.Transport;

namespace Procurement.ViewModel
{
    public class LoginWindowViewModel : ObservableBase
    {
        private LoginView _view = null;
        private StatusController _statusController;
        public event LoginCompleted OnLoginCompleted;
        public delegate void LoginCompleted();
        private bool _formChanged = false;
        private bool _passwordChanged = false;
        private bool _useSession = true;

        private CharacterTabInjector _characterInjector;

        private string _email;
        public string Email
        {
            get { return _email; }
            set
            {
                if (value != _email)
                {
                    _email = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _forceRefresh;
        private string selectedRealm;

        public bool ForceRefresh
        {
            get { return _forceRefresh; }
            set
            {
                if (value != _forceRefresh)
                {
                    _forceRefresh = value;
                    Settings.UserSettings["ForceRefresh"] = value.ToString();
                    OnPropertyChanged();
                }
            }
        }

        private void UpdateButtonLabels(bool useSession)
        {
            if (_view == null)
                return;

            _view.lblEmail.Content = useSession ? "Alias" : "Email";
            _view.lblPassword.Content = useSession ? "Session ID" : "Password";
        }

        public LoginWindowViewModel(UserControl view)
        {
            _view = view as LoginView;

            ForceRefresh = Settings.UserSettings.ContainsKey("ForceRefresh") ?
                bool.Parse(Settings.UserSettings["ForceRefresh"]) : true;

            Email = Settings.UserSettings["AccountLogin"];

            if (!string.IsNullOrEmpty(Settings.UserSettings["AccountPassword"]))
                _view.txtPassword.Password = string.Empty.PadLeft(8); //For the visuals

            _view.txtPassword.PasswordChanged += new RoutedEventHandler(TxtPassword_PasswordChanged);
            PropertyChanged += LoginWindow_PropertyChanged;

            _characterInjector = new CharacterTabInjector();

            _statusController = new StatusController(_view.StatusBox);

            ApplicationState.Model.Authenticating += Model_Authenticating;
            ApplicationState.Model.Throttled += Model_Throttled;
            ApplicationState.InitializeFont(Properties.Resources.fontin_regular_webfont);
            ApplicationState.InitializeFont(Properties.Resources.fontin_smallcaps_webfont);

            _statusController.DisplayMessage(ApplicationState.Version + " Initialized.\r");

            VersionChecker.CheckForUpdates();

            //Todo: Feed this in from a setting so that console players will have their preference remembered
            SelectedRealm = AvailableRealms.First();
        }

        void TxtPassword_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            _formChanged = true;
            _passwordChanged = true;
        }

        void LoginWindow_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            _formChanged = true;
        }

        public void Login(bool offline)
        {
            ToggleControls();

            if (string.IsNullOrEmpty(Email))
            {
                MessageBox.Show(string.Format("{0} is required!", _useSession ? "Alias" : "Email"), "Error logging in",
                    MessageBoxButton.OK, MessageBoxImage.Stop);
                ToggleControls();
                return;
            }

            if (!offline)
            {
                // Prevents the event from being registered doubly if the login fails prematurely. 
                ApplicationState.Model.StashLoading -= Model_StashLoading;
                ApplicationState.Model.ImageLoading -= Model_ImageLoading;

                ApplicationState.Model.StashLoading += Model_StashLoading;
                ApplicationState.Model.ImageLoading += Model_ImageLoading;
            }


            Task.Factory.StartNew(() =>
            {
                // If offline == true, the password is never checked to know if it is correct, but if
                // passwordChanged == true, the new value is saved to the settings file.  This might not be the
                // behavior the user expects.
                SecureString password = _passwordChanged ? this._view.txtPassword.SecurePassword :
                    Settings.UserSettings["AccountPassword"].Decrypt();
                ApplicationState.CurrentRealm = SelectedRealm;
                ApplicationState.Model.Authenticate(Email, password, offline, ApplicationState.CurrentRealm);

                if (_formChanged)
                    SaveSettings(password);

                if (!offline)
                {
                    _statusController.DisplayMessage("Fetching account name...");
                    ApplicationState.AccountName = ApplicationState.Model.GetAccountName(ApplicationState.CurrentRealm);
                    _statusController.Ok();
                    if (ForceRefresh)
                    {
                        ApplicationState.Model.ForceRefresh();
                    }
                    _statusController.DisplayMessage("Loading characters...");
                }
                else
                {
                    _statusController.DisplayMessage("Loading Procurement in offline mode...");
                }

                List<Character> chars;
                try
                {
                    chars = ApplicationState.Model.GetCharacters(ApplicationState.CurrentRealm);
                }
                catch (WebException wex)
                {
                    Logger.Log(wex);
                    _statusController.NotOK();
                    throw new Exception("Failed to load characters", wex.InnerException);
                }
                _statusController.Ok();

                UpdateCharactersByLeague(chars);

                var items = LoadItems(offline, chars).ToList();

                ApplicationState.Model.GetImages(items);

                ApplicationState.SetDefaults();

                ClientLogFileWatcher.Instance.Start();

                if (!offline)
                {
                    _statusController.DisplayMessage("\nDone!");
                    PoeTradeOnlineHelper.Instance.Start();
                }

                ApplicationState.Model.Authenticating -= Model_Authenticating;
                ApplicationState.Model.StashLoading -= Model_StashLoading;
                ApplicationState.Model.ImageLoading -= Model_ImageLoading;
                ApplicationState.Model.Throttled -= Model_Throttled;
                OnLoginCompleted();
            }).ContinueWith(t =>
            {
                Logger.Log(t.Exception.InnerException.ToString());
                _statusController.HandleError(t.Exception.InnerException.Message, ToggleControls);
            }, TaskContinuationOptions.OnlyOnFaulted);
        }

        private IEnumerable<Item> LoadItems(bool offline, IEnumerable<Character> chars)
        {
            bool downloadOnlyMyLeagues = (Settings.UserSettings.ContainsKey("DownloadOnlyMyLeagues") &&
                                          bool.TryParse(Settings.UserSettings["DownloadOnlyMyLeagues"],
                                                        out downloadOnlyMyLeagues) &&
                                          downloadOnlyMyLeagues &&
                                          Settings.Lists.ContainsKey("MyLeagues") &&
                                          Settings.Lists["MyLeagues"].Count > 0);

            foreach (var character in chars)
            {
                if (character.League == "Void")
                    continue;

                if (character.Expired)
                {
                    _statusController.DisplayMessage(Environment.NewLine + "Skipping character " + character.Name +
                        " because the character's name has expired." + Environment.NewLine);
                    continue;
                }

                if (downloadOnlyMyLeagues && !Settings.Lists["MyLeagues"].Contains(character.League))
                    continue;

                foreach (var item in LoadStashItems(character))
                    yield return item;

                foreach (var item in LoadCharacterInventoryItems(character, offline).Where(i => i.InventoryId != "MainInventory"))
                    yield return item;
            }

            if (downloadOnlyMyLeagues && ApplicationState.Characters.Count == 0)
                throw new Exception("No characters found in the leagues specified. Check spelling or try setting " +
                    "DownloadOnlyMyLeagues to false in the Settings.xml file.");

            _characterInjector.Inject();
        }

        private static void UpdateCharactersByLeague(List<Character> chars)
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

        private void SaveSettings(SecureString password)
        {
            if (!_formChanged)
                return;

            Settings.UserSettings["AccountLogin"] = Email;
            Settings.UserSettings["UseSessionID"] = _useSession.ToString();
            Settings.UserSettings["ForceRefresh"] = ForceRefresh.ToString();
            if (_passwordChanged)
                Settings.UserSettings["AccountPassword"] = password.Encrypt();
            Settings.Save();
        }

        private void ToggleControls()
        {
            _view.LoginButton.IsEnabled = !_view.LoginButton.IsEnabled;
            _view.OfflineButton.IsEnabled = !_view.OfflineButton.IsEnabled;
            _view.txtLogin.IsEnabled = !_view.txtLogin.IsEnabled;
            _view.txtPassword.IsEnabled = !_view.txtPassword.IsEnabled;
        }

        private IEnumerable<Item> LoadStashItems(Character character)
        {
            if (ApplicationState.Leagues.Contains(character.League))
                return Enumerable.Empty<Item>();

            ApplicationState.CurrentLeague = character.League;
            ApplicationState.Stash[character.League] = ApplicationState.Model.GetStash(character.League, ApplicationState.AccountName, ApplicationState.CurrentRealm);
            ApplicationState.Leagues.Add(character.League);

            return ApplicationState.Stash[character.League].Get<Item>();
        }

        private IEnumerable<Item> LoadCharacterInventoryItems(Character character, bool offline)
        {
            bool success;

            if (!offline)
                _statusController.DisplayMessage((string.Format("Loading {0}'s inventory...", character.Name)));

            List<Item> inventory;
            try
            {
                inventory = ApplicationState.Model.GetInventory(character.Name, false, ApplicationState.AccountName, ApplicationState.CurrentRealm);
                success = true;
            }
            catch (WebException)
            {
                inventory = new List<Item>();
                success = false;
            }

            _characterInjector.Add(character, inventory);
            UpdateStatus(success, offline);

            return inventory;
        }

        private void UpdateStatus(bool success, bool offline)
        {
            if (offline)
                return;

            if (success)
                _statusController.Ok();
            else
                _statusController.NotOK();
        }

        void Model_StashLoading(POEModel sender, StashLoadedEventArgs e)
        {
            Update("Loading " + ApplicationState.CurrentLeague + " Stash Tab " + (e.StashID + 1) + "...", e);
        }

        void Model_ImageLoading(POEModel sender, ImageLoadedEventArgs e)
        {
            Update("Loading Image For " + e.URL, e);
        }

        void Model_Authenticating(POEModel sender, AuthenticateEventArgs e)
        {
            Update("Authenticating " + e.Email, e);
        }

        void Model_Throttled(object sender, ThottledEventArgs e)
        {
            if (!e.Expected)
                Update(string.Format("Exceeded GGG Server request limit; throttling activated.  Waiting {0} " +
                    "seconds.  Ensure you do not have other instances of Procurement running or other apps using " +
                    "the GGG API with your account.", Convert.ToInt32(e.WaitTime.TotalSeconds)),
                    new POEEventArgs(POEEventState.BeforeEvent));
            else if (e.WaitTime.TotalSeconds > 4)
                Update(string.Format("GGG Server request limit hit, throttling activated. Please wait {0} seconds",
                    Convert.ToInt32(e.WaitTime.TotalSeconds)), new POEEventArgs(POEEventState.BeforeEvent));
        }

        private void Update(string message, POEEventArgs e)
        {
            if (e.State == POEEventState.BeforeEvent)
            {
                _statusController.DisplayMessage(message);
                return;
            }

            _statusController.Ok();
        }

        public List<string> AvailableRealms => (List<string>) Realm.AvailableRealms;

        public string SelectedRealm
        {
            get { return selectedRealm; }
            set { selectedRealm = value; OnPropertyChanged();}
        }

        public void NavigateHowToSessionIDwiki()
        {
            System.Diagnostics.Process.Start("https://github.com/Stickymaddness/Procurement/wiki/SessionID");
        }
    }
}