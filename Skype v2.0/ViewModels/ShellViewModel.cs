namespace Skype2.ViewModels
{
    using Caliburn.Micro;

    using Skype2.ViewModels.Interfaces;

    internal sealed class ShellViewModel : Conductor<IViewModelBase>, IShellViewModel
    {
        public ShellViewModel(ILoginViewModel loginViewModel, IMainViewModel mainViewModel)
        {
            DisplayName = "Skype v2.0";

            LoginViewModel = loginViewModel;
            MainViewModel = mainViewModel;

            ActivateItem(LoginViewModel);

            LoginViewModel.LoggedIn += (sender, e) => ActivateItem(MainViewModel);
        }

        public ILoginViewModel LoginViewModel { get; }

        public IMainViewModel MainViewModel { get; }
    }
}