namespace Skype2
{
    using System;
    using System.Collections.Generic;
    using System.Windows;

    using Caliburn.Micro;

    using Skype2.Services;
    using Skype2.Services.Interfaces;
    using Skype2.ViewModels;
    using Skype2.ViewModels.Interfaces;

    internal class AppBootstrapper : BootstrapperBase
    {
        private readonly SimpleContainer _container = new SimpleContainer();

        internal AppBootstrapper()
        {
            Initialize();
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }

        protected override void Configure()
        {
            // Register Services
            _container.Singleton<IWindowManager, WindowManager>();

            _container.Singleton<IUserService, UserService>();

            // Register ViewModels
            _container.Singleton<IShellViewModel, ShellViewModel>();
            _container.Singleton<IMainViewModel, MainViewModel>();

            _container.Singleton<IChatViewModel, ChatViewModel>();
        }

        protected override object GetInstance(Type service, string key)
        {
            return _container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return _container.GetAllInstances(serviceType);
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<IShellViewModel>();
        }
    }
}