using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfElements
{
    public class DialogPanel : Panel
    {
        protected override Size ArrangeOverride(Size finalSize)
        {
            foreach (UIElement child in InternalChildren)
            {
                child.Arrange(new Rect(new Point(0, 0), finalSize));
            }
            return finalSize;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            Size panelDesiredSize = new Size();
            foreach(UIElement child in InternalChildren)
            {
                child.Measure(availableSize);
                panelDesiredSize = child.DesiredSize;
            }
            return panelDesiredSize;
        }
    }
}
