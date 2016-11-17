namespace Client.ViewModel.Interfaces
{
    interface IServerChanger
    {
        string GetServer();
        void HideChangeServerButton();
        void HideQuitButton();
        void UnhideQuitButton();
    }
}
