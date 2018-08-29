namespace Skype2.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Security;

    using Caliburn.Micro;

    using Skype2.Services.Interfaces;
    using Skype2.ViewModels.Interfaces;

    internal class LoginViewModel : ViewModelBase, ILoginViewModel
    {
        private readonly IRestService _restService;

        public LoginViewModel(IRestService restService)
        {
            _restService = restService;
        }

        public event EventHandler LoggedIn;

        private SecureString _password;
        public SecureString Password
        {
            get => _password;

            set
            {
                if (_password == value) return;

                _password = value;
                NotifyOfPropertyChange(() => Password);
            }
        }

        public IEnumerable<IResult> Login(string username)
        {
            yield return _restService.Login(username, Password).AsResult();

            LoggedIn?.Invoke(this, EventArgs.Empty);
        }
    }
}