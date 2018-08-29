namespace Skype2.Helpers
{
    using System.Security;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Interactivity;

    public class PasswordExpositionBehavior : Behavior<PasswordBox>
    {
        public static readonly DependencyProperty PasswordProperty = DependencyProperty.Register(nameof(Password), typeof(SecureString), typeof(PasswordExpositionBehavior));

        public SecureString Password
        {
            get => (SecureString)GetValue(PasswordProperty);

            set => SetValue(PasswordProperty, value);
        }

        protected override void OnAttached()
        {
            AssociatedObject.PasswordChanged += OnPasswordChanged;
        }

        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            BindingExpression binding = BindingOperations.GetBindingExpression(this, PasswordProperty);

            binding?.DataItem
                    .GetType()
                    .GetProperty(binding.ParentBinding.Path.Path)
                    ?.SetValue(binding.DataItem, AssociatedObject.SecurePassword);
        }
    }
}