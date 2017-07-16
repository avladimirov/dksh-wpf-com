using DKSH.AuditionApp.Application.Dialogs;
using DKSH.AuditionApp.Infrastructure.Interfaces;
using System.ComponentModel.Composition;
using System.Windows.Threading;
using System;

namespace DKSH.AuditionApp.Application.Services
{
    [Export(typeof(IDialogService))]
    public class DialogService : IDialogService
    {
        protected static Dispatcher Dispatcher => App.Current.Dispatcher;

        public uint SelectNumberDialog()
        {
            var dialog = new NumSelectionDialog();
            dialog.ShowDialog();

            return dialog.GetData();
        }
    }
}
