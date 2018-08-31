namespace Skype2.ViewModels
{
    using System.Collections.Generic;
    using Caliburn.Micro;

    using Skype2.Services.Interfaces;
    using Skype2.ViewModels.Interfaces;

    internal sealed class MainViewModel : Conductor<IViewModelBase>.Collection.OneActive, IMainViewModel
    {
        private readonly IRestService _restService;

        public MainViewModel(IRestService restService, ILoginViewModel loginViewModel, IRegisterViewModel registerViewModel, IChatViewModel chatViewModel)
        {
            _restService = restService;

            LoginViewModel = loginViewModel;
            RegisterViewModel = registerViewModel;
            ChatViewModel = chatViewModel;

            Items.Add(loginViewModel);
            Items.Add(registerViewModel);
            Items.Add(chatViewModel);

            ScreenExtensions.TryActivate(this);

            ActivateItem(LoginViewModel);
        }

        public ILoginViewModel LoginViewModel { get; }

        public IRegisterViewModel RegisterViewModel { get; }

        public IChatViewModel ChatViewModel { get; }

        protected override IViewModelBase DetermineNextItemToActivate(IList<IViewModelBase> list, int lastIndex)
        {
            if (_restService.LoggedInUser == null)
            {
                if (list[lastIndex] == LoginViewModel)
                {
                    return RegisterViewModel;
                }

                return LoginViewModel;
            }

            return ChatViewModel;
        }
    }
}