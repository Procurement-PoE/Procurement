using POEApi.Infrastructure;
using POEApi.Infrastructure.Events;
using POEApi.Model.Events;
using Procurement.ViewModel;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Procurement.View
{
    public partial class RefreshView : UserControl
    {
        private StatusController statusController;

        public RefreshView()
        {
            InitializeComponent();
        }

        public void RefreshAllTabs()
        {
            RefreshTabs(true);
        }

        public void RefreshUsedTabs()
        {
            RefreshTabs(false);
        }

        public void RefreshTabs(bool refreshAllTabs)
        {
            statusController = new StatusController(StatusBox, 140);

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = cancellationTokenSource.Token;

            Task task = Task.Factory.StartNew(() =>
            {
                try
                {
                    ApplicationState.Model.StashLoading += model_StashLoading;
                    ApplicationState.Model.Throttled += model_Throttled;
                    var stash = ApplicationState.Stash[ApplicationState.CurrentLeague];
                    if (refreshAllTabs)
                    {
                        stash.RefreshAll(ApplicationState.Model, ApplicationState.CurrentLeague,
                            ApplicationState.AccountName, ApplicationState.CurrentRealm);
                    }
                    else
                    {
                        stash.RefreshUsedTabs(ApplicationState.Model, ApplicationState.CurrentLeague,
                            ApplicationState.AccountName, ApplicationState.CurrentRealm);
                    }
                    ApplicationState.Model.StashLoading -= model_StashLoading;
                    ApplicationState.Model.Throttled -= model_Throttled;
                }
                catch (Exception ex)
                {
                    Logger.Log("Exception bulk refreshing tabs: " + ex.ToString());
                    MessageBox.Show("Error encountered during refreshing tabs; error details logged to DebugInfo.log",
                        "Error refreshing tabs", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    ScreenController.Instance.ReloadStash();
                    ScreenController.Instance.RefreshRecipeScreen();
                    ScreenController.Instance.UpdateTrading();
                }
            });
        }

        private void model_StashLoading(POEApi.Model.POEModel sender, POEApi.Model.Events.StashLoadedEventArgs e)
        {
            update("Loading " + ApplicationState.CurrentLeague + " Stash Tab " + (e.StashID + 1) + "...", e);
        }

        void model_Throttled(object sender, ThottledEventArgs e)
        {
            if (!e.Expected)
                update(string.Format("Exceeded GGG Server request limit; throttling activated.  Waiting {0} " +
                    "seconds.  Ensure you do not have other instances of Procurement running or other apps using " +
                    "the GGG API with your account.", Convert.ToInt32(e.WaitTime.TotalSeconds)),
                    new POEEventArgs(POEEventState.BeforeEvent));
            else if (e.WaitTime.TotalSeconds > 4)
                update(string.Format("GGG Server request limit hit, throttling activated. Please wait {0} seconds",
                    Convert.ToInt32(e.WaitTime.TotalSeconds)), new POEEventArgs(POEEventState.BeforeEvent));
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
