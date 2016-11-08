using Client.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Client.View
{
    public partial class Statistics : UserControl
    {
        private StatisticsViewModel viewModel;

        public Statistics()
        {
            InitializeComponent();
            viewModel = (StatisticsViewModel)base.DataContext;
            viewModel.View = this;
        }
    }
}
