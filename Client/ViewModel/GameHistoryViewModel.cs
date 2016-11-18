using Client.Helper;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Client.ViewModel
{
    class GameHistoryViewModel
    {
        public DependencyObject View { get; set; }

        public ICommand BackCommand { get; set; }

        public GameHistoryViewModel()
        {
            BackCommand = new CommandHandler(Back);
        }

        private void Back()
        {
            NavigationService.GetNavigationService(View).GoBack();
        }
    }
}
