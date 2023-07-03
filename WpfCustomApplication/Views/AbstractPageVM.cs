using MVVMCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfCustomApplication
{
    internal abstract class AbstractPageVM : CoreVM
    {
        public abstract object View { get; protected set; }

    }
}
