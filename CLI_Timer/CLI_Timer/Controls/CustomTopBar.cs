using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;

namespace CLI_Timer.Controls
{
    public class CustomTopBar : Control
    {
        Button? closeButton;
        Button CloseButton
        {
            get => closeButton;
            set 
            {
                if(closeButton != null)
                {
                    closeButton.Click -= new RoutedEventHandler(CloseButtonClicked);
                }
                closeButton = value;
                if(closeButton != null)
                {
                    closeButton.Click += new RoutedEventHandler(CloseButtonClicked);
                }
            }
        }

        Border topBar;
        Border TopBar
        {
            get => topBar;
            set
            {
                if (topBar != null)
                {
                    topBar.MouseLeftButtonDown -= new MouseButtonEventHandler(BorderMouseDown);
                }
                topBar = value;
                if (topBar != null)
                {
                    topBar.MouseLeftButtonDown += new MouseButtonEventHandler(BorderMouseDown);
                }
            }
        }

        public static DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(CustomTopBar), new PropertyMetadata(string.Empty));

        public static DependencyProperty CloseClickedProperty =
            DependencyProperty.Register("CloseClicked", typeof(RoutedEventHandler), typeof(CustomTopBar), new PropertyMetadata());

        public static DependencyProperty LeftMouseDownProperty =
            DependencyProperty.Register("LeftMouseDown", typeof(MouseButtonEventHandler), typeof(CustomTopBar), new PropertyMetadata());
       
        public CustomTopBar()
        {
           DefaultStyleKey = typeof(CustomTopBar);
        }

        private void CloseButtonClicked(object sender, RoutedEventArgs e)
        {
            CloseClicked.Invoke(this, new RoutedEventArgs());
        }

        private void BorderMouseDown(object sender, MouseButtonEventArgs e)
        {
            LeftMouseDown.Invoke(sender, e);
        }

        public override void OnApplyTemplate()
        {
            (GetTemplateChild("Title_Label") as Label).Content = Title;

            CloseButton = GetTemplateChild("CloseButton") as Button;
            TopBar = GetTemplateChild("TopBar") as Border;
        }

        

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set 
            { 
                SetValue(TitleProperty, value);
            }
        }

        public RoutedEventHandler CloseClicked
        {
            get => (RoutedEventHandler)GetValue(CloseClickedProperty);
            set => SetValue(CloseClickedProperty, value);
        }

        public MouseButtonEventHandler LeftMouseDown
        {
            get => (MouseButtonEventHandler)GetValue(LeftMouseDownProperty);
            set => SetValue(LeftMouseDownProperty, value);
        }
    }
}
