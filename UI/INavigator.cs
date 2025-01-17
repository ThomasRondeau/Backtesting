using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace UI
{
    public interface INavigator
    {
        Page? Current { get; }
        void GoTo(Page page, object? loadData = null);
    }
}
