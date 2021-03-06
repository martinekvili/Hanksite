﻿using Client.Helper;
using Client.Model;
using Client.View;
using Client.View.Dialogs;
using System.Collections.Generic;
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
        public ICommand ShowGameHistoryCommand { get; set; }
        public ICommand ShowSettingsCommand { get; set; }
        public ICommand QuitCommand { get; set; }

        public MainMenuViewModel()
        {
            CreateLobbyCommand = new CommandHandler(NavigateToCreateLobby);
            ConnectLobbyCommand = new CommandHandler(NavigateToConnectLobby);
            ShowGameHistoryCommand = new CommandHandler(NavigateToGameHistory);
            ShowSettingsCommand = new CommandHandler(NavigateToSettings);
            QuitCommand = new CommandHandler(Quit);
        }

        private void NavigateToCreateLobby()
        {
            NavigationService.GetNavigationService(View).Navigate(new CreateLobby());
        }

        private void NavigateToConnectLobby()
        {
            NavigationService.GetNavigationService(View).Navigate(new ConnectLobby());
        }

        private void NavigateToGameHistory()
        {
            NavigationService.GetNavigationService(View).Navigate(new GameHistory());
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
