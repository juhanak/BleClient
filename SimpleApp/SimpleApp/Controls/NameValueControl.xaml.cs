using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SimpleApp.Controls
{
    public sealed partial class NameValueControl : UserControl
    {
        public new string Name
        {
            get
            {
                return (string)GetValue(NameStringProperty);
            }
            set
            {
                SetValue(NameStringProperty, value);
            }
        }
        public static readonly DependencyProperty NameStringProperty =
            DependencyProperty.Register("Name", typeof(string), typeof(NameValueControl),
            new PropertyMetadata(string.Empty, OnNamePropertyChanged));

        private static void OnNamePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NameValueControl control = d as NameValueControl;

            if (control != null)
            {
                string newValue = e.NewValue as string;
                control.nameTextBlock.Text = newValue as string;
            }
        }

        public object Value
        {
            get
            {
                return (object)GetValue(ValueProperty);
            }
            set
            {
                SetValue(ValueProperty, value);
            }
        }
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(object), typeof(NameValueControl),
            new PropertyMetadata(null, OnValuePropertyChanged));

        private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NameValueControl control = d as NameValueControl;
            object newValue = e.NewValue;

            if (control != null && newValue != null)
            {
                if (newValue is string)
                {
                    control.valueTextBlock.Text = newValue as string;
                }
                else if (newValue is double)
                {
                    if (control.RoundFloatingPointNumbers)
                    {
                        control.valueTextBlock.Text = Math.Round((double)newValue, 1).ToString();
                    }
                    else
                    {
                        control.valueTextBlock.Text = newValue.ToString();
                    }
                }
                else if (newValue is bool)
                {
                    control.valueTextBlock.Text = (bool)newValue ? "Yes" : "No";
                }
                else
                {
                    control.valueTextBlock.Text = newValue.ToString();
                }
            }
        }

        public bool RoundFloatingPointNumbers
        {
            get
            {
                return (bool)GetValue(RoundFloatingPointNumbersProperty);
            }
            set
            {
                SetValue(RoundFloatingPointNumbersProperty, value);
            }
        }
        public static readonly DependencyProperty RoundFloatingPointNumbersProperty =
            DependencyProperty.Register("RoundFloatingPointNumbers", typeof(bool), typeof(NameValueControl),
            new PropertyMetadata(true));

        public NameValueControl()
        {
            this.InitializeComponent();
        }
    }
}
