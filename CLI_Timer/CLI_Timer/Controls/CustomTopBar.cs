using System.Windows.Controls;
using System.Windows;
using System.Diagnostics;

namespace CLI_Timer.Controls
{
    public class CustomTopBar : Control
    {
        public static DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(CustomTopBar), new PropertyMetadata(null));
        Button closeButton;
        Button CloseButton 
        {
            get => closeButton; 
            set 
            { 
                if(closeButton != null) 
                {
                    closeButton.Click -= new RoutedEventHandler(closeButton_Click);
                }
                closeButton = value;

                if(closeButton != null)
                {
                    closeButton.Click += new RoutedEventHandler(closeButton_Click);
                }
            }
                
        }



        public CustomTopBar() 
        {
            DefaultStyleKey = typeof(CustomTopBar);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            CloseButton = GetTemplateChild("Closebtn") as Button;

            (GetTemplateChild("Title_Label") as Label).Content = (string)GetValue(TitleProperty);

        }

        void closeButton_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
