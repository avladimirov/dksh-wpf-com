using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKSH.AuditionApp.Application.ViewModels
{
    public class NumSelectionDialogViewModel
    {
        public IEnumerable<int> NumericItems { get; } = Enumerable.Range(1, 10);

        public int SelectedNumericItem { get; set; }

    }
}
