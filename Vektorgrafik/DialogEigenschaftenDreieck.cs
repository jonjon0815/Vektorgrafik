using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vektorgrafik
{
    class DialogEigenschaftenDreieck : DialogEigenschaftenFigur
    {
        //Configure Dialog Dreieck
        public DialogEigenschaftenDreieck(TEigenschaften eigenschaften) : base(eigenschaften, false)
        {
            Title = "Dreieck - Einstellungen";
        }
    }
}
