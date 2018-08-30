namespace Skype2.ViewModels.Interfaces
{
    using System;

    internal interface IRegisterViewModel : IViewModelBase
    {
        event EventHandler Registered;

        event EventHandler SwitchToLoginRequested;
    }
}