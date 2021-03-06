﻿using Client.Model;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows;
using Client.View;
using Client.Helper;
using Client.Model.Interfaces;
using Client.ServerConnection;
using Client.ViewModel.Interfaces;
using System.IO;
using Client.View.Interfaces;
using Client.View.Dialogs;
using System.Collections.Generic;

namespace Client.ViewModel
{
    class LoginViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public DependencyObject View { get; set; }

        private IAccountProvider accounts;

        public ICommand SignInCommand { get; set; }
        public ICommand CreateAccountCommand { get; set; }

        public string Server { get; set; }
        public string Username { get; set; }

        private bool isPageEnabled;
        public bool IsPageEnabled
        {
            get { return isPageEnabled; }
            set { isPageEnabled = value; NotifyPropertyChanged("IsPageEnabled"); }
        }

        internal IAccountProvider Accounts
        {
            get { return accounts; }
            set { accounts = value; }
        }

        private LoginDataManager loginDataManager;
        private IGameServer gameServer;

        public LoginViewModel()
        {
            IsPageEnabled = true;
            loginDataManager = new LoginDataManager();

            Accounts = ClientProxyManager.Instance;

            if (File.Exists("lastlogin.xml"))
            {
                LoginData lastLoginData = loginDataManager.LoadLastLogin();
                Username = lastLoginData.Username;
            }

            SignInCommand = new CommandHandler(SignIn);
            CreateAccountCommand = new CommandHandler(CreateAccount);

            gameServer = ClientProxyManager.Instance;
        }

        private async void SignIn()
        {
            IServerChanger window = (IServerChanger)Window.GetWindow(View);

            if (Username == null || Username.Length == 0)
            {
                MessageBox.Show("Username is empty!", "Hanksite", MessageBoxButton.OK);
                return;
            }

            string password = ((IPasswordProvider)View).GetPassword();
            if (password == null || password.Length == 0)
            {
                MessageBox.Show("Password is empty!", "Hanksite", MessageBoxButton.OK);
                return;
            }

            IsPageEnabled = false;
            window.Disable();

            bool success;
            try
            {
                success = await Accounts.IsAccountValid(window.GetServer(), Username, password);
            }
            catch (System.Exception e)
            {
                MessageBox.Show("An error has occured while connecting to the server.\n" + e.Message, "Hanksite", MessageBoxButton.OK);
                IsPageEnabled = true;
                window.Enable();
                return;
            }

            if (success)
            {
                GameStateForDisconnected[] games = await gameServer.GetRunningGames();

                if (games.Length == 0)
                {
                    NavigationService.GetNavigationService(View).Navigate(new MainMenu());
                }
                else
                {
                    ReconnectDialog dialog = new ReconnectDialog(Window.GetWindow(View), games);
                    if (dialog.ShowDialog() == true)
                    {
                        NavigationService.GetNavigationService(View).Navigate(new GameView(dialog.GetConnectedGameState()));
                    }
                    else
                    {
                        NavigationService.GetNavigationService(View).Navigate(new MainMenu());
                    }
                }

                window.HideChangeServerButton();
                window.HideQuitButton();

                LoginData loginData = new LoginData(window.GetServer(), Username);
                loginDataManager.SaveLastLogin(loginData);
                window.Enable();
            }
            else
            {
                IsPageEnabled = true;
                window.Enable();
                MessageBox.Show("Wrong username or password!", "Hanksite", MessageBoxButton.OK);
            }
        }

        private void CreateAccount()
        {
            NavigationService.GetNavigationService(View).Navigate(new Registration());
            IServerChanger window = (IServerChanger)Window.GetWindow(View);
            window.HideQuitButton();
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}