using DKSH.AuditionApp.Infrastructure.Interfaces;
using System;
using System.ComponentModel.Composition;

namespace DKSH.AuditionApp.Application.Services
{
    [Export(typeof(IDialogService))]
    public class DialogService : IDialogService
    {
        public uint SelectNumberDialog()
        {
            return 5;
        }
    }
}
