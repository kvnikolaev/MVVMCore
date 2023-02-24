using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace MVVMCore
{
    public class VisualTreeFinder
    {
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject dependencyObject) where T : DependencyObject
        {
            if (dependencyObject != null)
            {
                var count1 = VisualTreeHelper.GetChildrenCount(dependencyObject);
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(dependencyObject); i++)
                {
                    var child = VisualTreeHelper.GetChild(dependencyObject, i);
                    if (child is T) // != null
                    {
                        yield return (T)child;
                    }

                    foreach (T subChild in FindVisualChildren<T>(child))
                    {
                        yield return subChild;
                    }

                }
            }
        }


    }
}
