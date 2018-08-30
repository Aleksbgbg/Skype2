namespace Skype2.ViewModels
{
    using Caliburn.Micro;

    using Skype2.ViewModels.Interfaces;

    internal sealed class MainViewModel : Conductor<IViewModelBase>, IMainViewModel
    {
        public MainViewModel(ILoginViewModel loginViewModel, IRegisterViewModel registerViewModel, IChatViewModel chatViewModel)
        {
            LoginViewModel = loginViewModel;
            RegisterViewModel = registerViewModel;
            ChatViewModel = chatViewModel;;

            ScreenExtensions.TryActivate(this);

            ActivateItem(LoginViewModel);

            LoginViewModel.LoggedIn += LoggedIn;
            RegisterViewModel.Registered += Registered;

            LoginViewModel.SwitchToRegisterRequested += SwitchToRegister;
            RegisterViewModel.SwitchToLoginRequested += SwitchToLogin;
        }

        private void LoggedIn(object sender, System.EventArgs e)
        {
            OnLoggedIn();
        }

        private void Registered(object sender, System.EventArgs e)
        {
            OnLoggedIn();
        }

        private void SwitchToRegister(object sender, System.EventArgs e)
        {
            ActivateItem(RegisterViewModel);
        }

        private void SwitchToLogin(object sender, System.EventArgs e)
        {
            ActivateItem(LoginViewModel);
        }

        public ILoginViewModel LoginViewModel { get; }

        public IRegisterViewModel RegisterViewModel { get; }

        public IChatViewModel ChatViewModel { get; }

        private void OnLoggedIn()
        {
            ActivateItem(ChatViewModel);

            LoginViewModel.LoggedIn -= LoggedIn;
            RegisterViewModel.Registered -= Registered;

            LoginViewModel.SwitchToRegisterRequested -= SwitchToRegister;
            RegisterViewModel.SwitchToLoginRequested -= SwitchToLogin;
        }
    }
}