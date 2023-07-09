using System.Windows.Controls;
using System.Windows;

namespace CLI_Timer.Controls
{
    [TemplatePart(Name = "TriggerElement", Type = typeof(Button))]
    [TemplateVisualState(Name = "Toggled", GroupName = "ValueStates")]
    //[TemplateVisualState(Name = "Negative", GroupName = "ValueStates")]

    public class Switch : Control
    {
        public static DependencyProperty ToggledProperty = DependencyProperty.Register("Toggled", typeof(bool), typeof(Switch), new PropertyMetadata(false));

        public bool Toggled
        {
            get => (bool)GetValue(ToggledProperty);
            set
            {
                SetValue(ToggledProperty, value);
                UpdateVisualState();
            }
        }

        Button triggerElement;

        Button TriggerElement
        {
            get => triggerElement;
            set
            {
                if (triggerElement != null)
                {
                    triggerElement.Click -= new RoutedEventHandler(buttonElement_Click);
                }
                triggerElement = value;

                if (triggerElement != null)
                {
                    triggerElement.Click += new RoutedEventHandler(buttonElement_Click);
                }
            }
        }

        void buttonElement_Click(object sender, RoutedEventArgs e)
        {
            Toggled = !Toggled;
            UpdateVisualState();
        }

        public override void OnApplyTemplate()
        {
            TriggerElement = GetTemplateChild("Trigger") as Button;
            UpdateVisualState();
        }

        public Switch()
        {
            //DefaultStyleKeyProperty.OverrideMetadata(typeof(Switch), new FrameworkPropertyMetadata(typeof(Switch)));
            DefaultStyleKey = typeof(Switch);
        }


        void UpdateVisualState()
        {
            if (Toggled) VisualStateManager.GoToState(this, "Toggled", true);
            if (!Toggled) VisualStateManager.GoToState(this, "UnToggled", true);
        }
    }
}
