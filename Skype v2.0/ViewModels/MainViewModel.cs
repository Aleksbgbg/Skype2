namespace Skype2.ViewModels
{
    using Skype2.ViewModels.Interfaces;

    internal class MainViewModel : ViewModelBase, IMainViewModel
    {
        public MainViewModel(IChatViewModel chatViewModel)
        {
            ChatViewModel = chatViewModel;
        }

        public IChatViewModel ChatViewModel { get; }
    }
}