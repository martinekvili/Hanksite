using Client.Helper;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Client.ViewModel
{
    class StatisticsViewModel
    {
        public DependencyObject View { get; set; }

        public ICommand BackCommand { get; set; }

        public StatisticsViewModel()
        {
            BackCommand = new CommandHandler(Back, true);
        }

        private void Back()
        {
            NavigationService.GetNavigationService(View).GoBack();
        }
    }
}
