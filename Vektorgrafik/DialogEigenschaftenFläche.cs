using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vektorgrafik
{
    class DialogEigenschaftenFläche : DialogEigenschaftenFigur
    {
        //Configure Dialog Fläche
        public DialogEigenschaftenFläche(TEigenschaften eigenschaften) : base(eigenschaften, false)
        {
            Title = "Einstellungen";
            GroupBoxPos3.Visibility = System.Windows.Visibility.Collapsed;
        }
    }
}
