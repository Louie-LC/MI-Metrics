using System;
using System.Windows;

namespace MI_Metrics
{
    public class DataGridHelper
    {
        // Safety Property
        public static readonly DependencyProperty SafetyProperty = DependencyProperty.RegisterAttached("Safety", typeof(Boolean), typeof(DataGridHelper), new PropertyMetadata(false));
        public static void SetSafety(UIElement element, Boolean value)
        {
            element.SetValue(SafetyProperty, value);
        }
        public static Boolean GetSafety(UIElement element)
        {
            return (Boolean)element.GetValue(SafetyProperty);
        }

        // Error Property
        public static readonly DependencyProperty ErrorProperty = DependencyProperty.RegisterAttached("Error", typeof(Boolean), typeof(DataGridHelper), new PropertyMetadata(false));
        public static void SetError(UIElement element, Boolean value)
        {
            element.SetValue(ErrorProperty, value);
        }
        public static Boolean GetError(UIElement element)
        {
            return (Boolean)element.GetValue(ErrorProperty);
        }

        // Success Property
        public static readonly DependencyProperty SuccessProperty = DependencyProperty.RegisterAttached("Success", typeof(Boolean), typeof(DataGridHelper), new PropertyMetadata(false));
        public static void SetSuccess(UIElement element, Boolean value)
        {
            element.SetValue(SuccessProperty, value);
        }
        public static Boolean GetSuccess(UIElement element)
        {
            return (Boolean)element.GetValue(SuccessProperty);
        }
    }
}
