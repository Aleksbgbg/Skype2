namespace Skype2.ViewModels
{
    using Caliburn.Micro;

    using Skype2.ViewModels.Interfaces;

    internal sealed class MainViewModel : Conductor<IViewModelBase>, IMainViewModel
    {
        public MainViewModel(ILoginViewModel loginViewModel, IChatViewModel chatViewModel)
        {
            LoginViewModel = loginViewModel;
            ChatViewModel = chatViewModel;;

            ScreenExtensions.TryActivate(this);

            ActivateItem(LoginViewModel);

            LoginViewModel.LoggedIn += (sender, e) => ActivateItem(ChatViewModel);
        }

        public ILoginViewModel LoginViewModel { get; }

        public IChatViewModel ChatViewModel { get; }
    }
}