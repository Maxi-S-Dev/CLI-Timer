using System.Windows.Controls;
using System.Windows;
using System.Diagnostics;

namespace CLI_Timer.Controls
{
    public class CustomTopBar : Control
    {
        CustomTopBar()
        {
           DefaultStyleKey = typeof(CustomTopBar);
        }

        public override void OnApplyTemplate()
        {
            (GetTemplateChild("Title_Label") as Label).Content = Title;
        }

        public static DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(CustomTopBar), new PropertyMetadata(string.Empty));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set 
            { 
                SetValue(TitleProperty, value);
            }
        }
    }
}
