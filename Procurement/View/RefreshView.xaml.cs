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
            statusController = new StatusController(StatusBox, 140);

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = cancellationTokenSource.Token;

            Task task = Task.Factory.StartNew(() =>
            {
                try
                {
                    ApplicationState.Model.StashLoading += model_StashLoading;
                    ApplicationState.Model.Throttled += model_Throttled;
                    ApplicationState.Stash[ApplicationState.CurrentLeague].RefreshAll(ApplicationState.Model, ApplicationState.CurrentLeague, ApplicationState.AccountName);
                    ApplicationState.Model.StashLoading -= model_StashLoading;
                    ApplicationState.Model.Throttled -= model_Throttled;
                }
                catch (Exception ex)
                {
                    Logger.Log("Exception refreshing all tabs: " + ex.ToString());
                    MessageBox.Show("Error encountered during refreshing all tabs, error details logged to DebugInfo.log", "Error refreshing all tabs", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    ScreenController.Instance.ReloadStash();
                }
            });
        }

        private void model_StashLoading(POEApi.Model.POEModel sender, POEApi.Model.Events.StashLoadedEventArgs e)
        {
            update("Loading " + ApplicationState.CurrentLeague + " Stash Tab " + (e.StashID + 1) + "...", e);
        }

        void model_Throttled(object sender, ThottledEventArgs e)
        {
            if (e.WaitTime.TotalSeconds > 4)
                update(string.Format("GGG Server request limit hit, throttling activated. Please wait {0} seconds", e.WaitTime.Seconds), new POEEventArgs(POEEventState.BeforeEvent));
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
