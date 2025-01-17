using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI
{
    public abstract class Page : UserControl
    {
        protected INavigator Navigator { get; init; }

        public Page(INavigator navigator)
        {
            Navigator = navigator;
        }

        public virtual void BeforeLoad(object? loadData) { }
        public virtual void BeforeClose() { }
    }
}
