using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vektorgrafik
{
    class DialogEingenschaftenLinie : DialogEigenschaftenFigur
    {
        //Configure Dialog Linie
        public DialogEingenschaftenLinie(TEigenschaften eigenschaften) : base(eigenschaften, false)
        {
            Title = "Linien Einstellungen";
            GroupBoxPos3.Visibility = System.Windows.Visibility.Collapsed;
            PinselFarbe.Visibility = System.Windows.Visibility.Collapsed;
        }
    }
}
