using DKSH.AuditionApp.Application.Dialogs;
using DKSH.AuditionApp.Infrastructure.Interfaces;
using System.ComponentModel.Composition;
using System.Windows.Threading;
using System;
using System.Threading.Tasks;

namespace DKSH.AuditionApp.Application.Services
{
    [Export(typeof(IDialogService))]
    public class DialogService : IDialogService
    {
        protected static Dispatcher Dispatcher => App.Current.Dispatcher;

        public async Task<uint> SelectNumberDialog()
        {
            var result = await Dispatcher.InvokeAsync(() =>
            {
                var dialog = new NumSelectionDialog();
                dialog.ShowDialog();

                return dialog.GetData();
            });

            return result;
        }
    }
}
