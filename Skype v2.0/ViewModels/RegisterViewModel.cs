namespace Skype2.ViewModels
{
    using System.Collections.Generic;
    using System.Security;

    using Caliburn.Micro;

    using Skype2.Extensions;
    using Skype2.Services.Interfaces;
    using Skype2.ViewModels.Interfaces;

    internal class RegisterViewModel : ViewModelBase, IRegisterViewModel
    {
        private readonly IRestService _restService;

        public RegisterViewModel(IRestService restService)
        {
            _restService = restService;
        }

        private string _username;
        public string Username
        {
            get => _username;

            set
            {
                if (_username == value) return;

                _username = value;
                NotifyOfPropertyChange(() => Username);
                NotifyOfPropertyChange(() => CanRegister);
            }
        }

        private SecureString _password;
        public SecureString Password
        {
            get => _password;

            set
            {
                if (_password == value) return;

                _password = value;
                NotifyOfPropertyChange(() => Password);
                NotifyOfPropertyChange(() => CanRegister);
            }
        }

        private SecureString _repeatPassword;
        public SecureString RepeatPassword
        {
            get => _repeatPassword;

            set
            {
                if (_repeatPassword == value) return;

                _repeatPassword = value;
                NotifyOfPropertyChange(() => RepeatPassword);
                NotifyOfPropertyChange(() => CanRegister);
            }
        }

        public bool CanRegister => !string.IsNullOrWhiteSpace(Username) && Password != null && RepeatPassword != null && Password.Length > 6 && Password.IsEqualTo(RepeatPassword);

        public IEnumerable<IResult> Register()
        {
            yield return _restService.Register(Username, Password).AsResult();

            TryClose();
        }

        public void SwitchToLogin()
        {
            TryClose();
        }
    }
}