using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vektorgrafik
{
    class DialogEigenschaftenQuadrat : DialogEigenschaftenFigur
    {
        public DialogEigenschaftenQuadrat(TEigenschaften eigenschaften) : base(eigenschaften, true)
        {
            Title = "Quadrat - Einstellungen";
            Y1.Focusable = false;
            Y2.Focusable = false;
            GroupBoxPos3.Visibility = System.Windows.Visibility.Collapsed;
        }
    }
}
