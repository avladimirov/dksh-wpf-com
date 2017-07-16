using DKSH.AuditionApp.Application.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;

namespace DKSH.AuditionApp.Application.Dialogs
{
    /// <summary>
    /// Interaction logic for NumSelectionDialog.xaml
    /// </summary>
    public partial class NumSelectionDialog : Window
    {
        private NumSelectionDialogViewModel ViewModel { get; set; }

        public NumSelectionDialog()
        {
            InitializeComponent();

            ViewModel = new NumSelectionDialogViewModel();
            DataContext = ViewModel;
        }

        public uint GetData()
        {
            return Convert.ToUInt32(ViewModel.SelectedNumericItem);
        }
    }
}
