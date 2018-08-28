namespace Skype2.Helpers
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Interactivity;

    internal class AutoScrollBehavior : Behavior<ScrollViewer>
    {
        private bool _isUserScrolling;

        protected override void OnAttached()
        {
            AssociatedObject.ScrollChanged += AssociatedObject_ScrollChanged;
            AssociatedObject.SizeChanged += AssociatedObject_SizeChanged;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.ScrollChanged -= AssociatedObject_ScrollChanged;
            AssociatedObject.SizeChanged -= AssociatedObject_SizeChanged;
        }

        private void AssociatedObject_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (!_isUserScrolling)
            {
                AssociatedObject.ScrollToEnd();
            }
        }

        private void AssociatedObject_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            _isUserScrolling = AssociatedObject.VerticalOffset != AssociatedObject.ScrollableHeight;
        }
    }
}