using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MI_Metrics
{
    class TextBoxHelper
    {
        // Manditory Property
        public static readonly DependencyProperty MandatoryProperty = DependencyProperty.RegisterAttached("Mandatory", typeof(Boolean), typeof(TextBoxHelper), new PropertyMetadata(false));
        public static void SetMandatory(UIElement element, Boolean value)
        {
            element.SetValue(MandatoryProperty, value);
        }
        public static Boolean GetMandatory(UIElement element)
        {
            return (Boolean)element.GetValue(MandatoryProperty);
        }

        // Safety Property
        public static readonly DependencyProperty SafetyProperty = DependencyProperty.RegisterAttached("Safety", typeof(Boolean), typeof(TextBoxHelper), new PropertyMetadata(false));
        public static void SetSafety(UIElement element, Boolean value)
        {
            element.SetValue(SafetyProperty, value);
        }
        public static Boolean GetSafety(UIElement element)
        {
            return (Boolean)element.GetValue(SafetyProperty);
        }

        // Error Property
        public static readonly DependencyProperty ErrorProperty = DependencyProperty.RegisterAttached("Error", typeof(Boolean), typeof(TextBoxHelper), new PropertyMetadata(false));
        public static void SetError(UIElement element, Boolean value)
        {
            element.SetValue(ErrorProperty, value);
        }
        public static Boolean GetError(UIElement element)
        {
            return (Boolean)element.GetValue(ErrorProperty);
        }

        // Success Property
        public static readonly DependencyProperty SuccessProperty = DependencyProperty.RegisterAttached("Success", typeof(Boolean), typeof(TextBoxHelper), new PropertyMetadata(false));
        public static void SetSuccess(UIElement element, Boolean value)
        {
            element.SetValue(SuccessProperty, value);
        }
        public static Boolean GetSuccess(UIElement element)
        {
            return (Boolean)element.GetValue(SuccessProperty);
        }

        // Hint Property
        public static readonly DependencyProperty HintProperty = DependencyProperty.RegisterAttached("Hint", typeof(String), typeof(TextBoxHelper), new PropertyMetadata(default(String)));
        public static void SetHint(UIElement element, String value)
        {
            element.SetValue(HintProperty, value);
        }
        public static String GetHint(UIElement element)
        {
            return (String)element.GetValue(HintProperty);
        }

        // Hint Text Visible Property
        public static readonly DependencyProperty HintTextVisibleProperty = DependencyProperty.RegisterAttached("HintTextVisible", typeof(Boolean), typeof(TextBoxHelper), new PropertyMetadata(false));
        public static void SetHintTextVisible(UIElement element, Boolean value)
        {
            element.SetValue(HintTextVisibleProperty, value);
        }
        public static Boolean GetHintTextVisible(UIElement element)
        {
            return (Boolean)element.GetValue(HintTextVisibleProperty);
        }

        // Text Property
        public static readonly DependencyProperty TextProperty = DependencyProperty.RegisterAttached("Text", typeof(String), typeof(TextBoxHelper), new PropertyMetadata(default(String)));
        public static void SetText(UIElement element, String value)
        {
            element.SetValue(TextProperty, value);
        }
        public static String GetText(UIElement element)
        {
            return (String)element.GetValue(TextProperty);
        }
    }
}
