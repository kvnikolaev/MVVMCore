using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace WpfElements
{
    public class AutoGrayImage : Image
    {
        static AutoGrayImage()
        {
            IsEnabledProperty.OverrideMetadata(typeof(AutoGrayImage), new FrameworkPropertyMetadata(OnEnableChangedImage));
        }

        private static void OnEnableChangedImage(DependencyObject source, DependencyPropertyChangedEventArgs args)
        {
            if (source is AutoGrayImage agImage)
            {
                bool isEnabled = Convert.ToBoolean(args.NewValue);
                if (!isEnabled)
                {
                    var bitmapImage = new BitmapImage(new Uri(agImage.Source.ToString()));
                    agImage.Source = new FormatConvertedBitmap(bitmapImage, PixelFormats.Gray32Float, null, 0);
                    agImage.OpacityMask = new ImageBrush(bitmapImage);
                }
                else
                {
                    agImage.Source = ((FormatConvertedBitmap)agImage.Source).Source;
                    agImage.OpacityMask = null;
                }
            }
        }
    }
}
