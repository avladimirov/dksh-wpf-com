using System.Collections.Generic;
using System.Linq;

namespace DKSH.Application.ViewModels
{
    internal class MainWindowViewModel
    {
        public bool CanSend { get; set; }

        public string ConnectionState { get; set; }

        public IEnumerable<int> NumericItems { get; } = Enumerable.Range(1, 10);

        public int SelectedNumericItem { get; set; }

        public string Response { get; set; }
    }
}
