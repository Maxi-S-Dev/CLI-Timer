using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using System.Diagnostics;

namespace CLI_TImer.Controls
{
    [TemplatePart(Name = "TriggerElement", Type = typeof(Button))]
    [TemplateVisualState(Name = "Toggled", GroupName = "ValueStates")]
    [TemplateVisualState(Name = "Negative", GroupName = "ValueStates")]

    public class Switch : Control
    {
        public static DependencyProperty ToggledProperty = DependencyProperty.Register("Toggled", typeof(bool), typeof(Switch), new PropertyMetadata(false));

        private bool toggled = false;

        public bool Toggled
        {
            get => (bool)GetValue(ToggledProperty);
            set
            {
                toggled = value;
                SetValue(ToggledProperty, value);
                Trace.WriteLine("Toggled:" + toggled);
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

        Switch()
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
