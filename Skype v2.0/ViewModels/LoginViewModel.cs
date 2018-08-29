namespace Skype2.ViewModels
{
    using System;
    using System.Collections.Generic;

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

        public IEnumerable<IResult> Login(string username, string password)
        {
            yield return _restService.Login(username, password).AsResult();

            LoggedIn?.Invoke(this, EventArgs.Empty);
        }
    }
}