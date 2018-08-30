namespace Skype2.ViewModels.Interfaces
{
    using System;

    internal interface ILoginViewModel : IViewModelBase
    {
        event EventHandler LoggedIn;

        event EventHandler SwitchToRegisterRequested;
    }
}