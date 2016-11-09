﻿using Client.Helper;
using Client.View;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Client.ViewModel
{
    class MainMenuViewModel
    {
        public DependencyObject View { get; set; }

        public ICommand CreateLobbyCommand { get; set; }
        public ICommand ConnectLobbyCommand { get; set; }
        public ICommand ShowStatisticsCommand { get; set; }
        public ICommand ShowSettingsCommand { get; set; }
        public ICommand QuitCommand { get; set; }

        public MainMenuViewModel()
        {
            CreateLobbyCommand = new CommandHandler(NavigateToCreateLobby, true);
            ConnectLobbyCommand = new CommandHandler(NavigateToConnectLobby, true);
            ShowStatisticsCommand = new CommandHandler(NavigateToStatistics, true);
            ShowSettingsCommand = new CommandHandler(NavigateToSettings, true);
            QuitCommand = new CommandHandler(Quit, true);
        }

        private void NavigateToCreateLobby()
        {
            NavigationService.GetNavigationService(View).Navigate(new CreateLobby());
        }

        private void NavigateToConnectLobby()
        {
            NavigationService.GetNavigationService(View).Navigate(new ConnectLobby());
        }

        private void NavigateToStatistics()
        {
            NavigationService.GetNavigationService(View).Navigate(new Statistics());
        }

        private void NavigateToSettings()
        {
            NavigationService.GetNavigationService(View).Navigate(new Settings());
        }

        private void Quit()
        {
            Application.Current.Shutdown();
        }
    }
}