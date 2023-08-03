using MVVMCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfCustomApplication
{
    internal class BubbleAnimationVM : TabPageVM
    {
        public BubbleAnimationVM()
        {
            this.View = new BubbleAnimation();
            Bubbles = new ObservableCollection<Bubble>() { new Bubble { X = 20, Y = 20 }, new Bubble { X = 25, Y = 25 } };
        }

        #region TabPageVM Implementation

        private string _title = "Анимация";
        public override string Title { get => _title; set => SetField(ref _title, value); }
        public override object View { get; protected set; }

        protected override bool CloseCanExecuted(object parameter)
        {
            return true;
        }

        protected override void CloseExecuted(object parameter)
        {
            TabPager?.CloseTabPage(this);
        }
        #endregion

        public ObservableCollection<Bubble> Bubbles { get; set; }

        public class Bubble : CoreVM
        {
            private int _x;
            public int X { get => _x; set => SetField(ref _x, value); }

            private int _y;
            public int Y { get => _y; set => SetField(ref _y, value); }
        }
    }
}
