using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfCustomApplication
{
    public class ControlDoubleClick
    {
        public static readonly DependencyProperty CommandProperty = 
            DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(ControlDoubleClick), new PropertyMetadata(OnCommandChanged));
        
        public static ICommand GetCommand(FrameworkElement target) 
        { 
            return (ICommand)target.GetValue(CommandProperty); 
        }

        public static void SetCommand(FrameworkElement target, ICommand value) 
        { 
            target.SetValue(CommandProperty, value); 
        }

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter", typeof(object), typeof(ControlDoubleClick), new PropertyMetadata(null));

        public static object GetCommandParameter(FrameworkElement target)
        {
            return target.GetValue(CommandParameterProperty);
        }

        public static void SetCommandParameter(FrameworkElement target, object value)
        {
            target.SetValue(CommandParameterProperty, value);
        }

        private static void Element_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e) 
        { 
            FrameworkElement control = sender as FrameworkElement; 
            ICommand command = GetCommand(control); 
            if (command.CanExecute(GetCommandParameter(control))) 
            { 
                command.Execute(GetCommandParameter(control)); 
                e.Handled = true; 
            } 
        }

        private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) 
        { 
            Control control = d as Control; 
            control.PreviewMouseDoubleClick += new MouseButtonEventHandler(Element_PreviewMouseDoubleClick); 
        }
    }
}
