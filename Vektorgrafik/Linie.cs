using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Vektorgrafik
{
    public class TLinie : TFigur
    {
        private Line linie;

        public TLinie(Canvas ACanvasZeichnung, Point APos1) : base(ACanvasZeichnung, APos1)
        {
            linie = new Line();
            linie.Tag = this;
            Figur = linie;
            SetModus(TZeichenModus.Normal);
            linie.X1 = Pos1.X;
            linie.X2 = Pos2.X;
            linie.Y1 = Pos1.Y;
            linie.Y2 = Pos2.Y;
           
            
            CanvasZeichnung.Children.Add(linie);
        }
        protected override void SetPos1(Point APos1)
        {
            FPos1 = APos1;
            linie.X1 = Pos1.X;
            linie.Y1 = Pos1.Y;
        }
        protected override void SetPos2(Point APos2)
        {
            FPos2 = APos2;
            linie.X2 = Pos2.X;
            linie.Y2 = Pos2.Y;
        }
        public override void zeigeEigenschaftenDialog()
        {

            SetModus(TZeichenModus.Highlight);
            TEigenschaften eigenschaften = GetEigenschaften();
            DialogEingenschaftenLinie dlg = new DialogEingenschaftenLinie(eigenschaften);
            if (dlg.ShowDialog() == true)
            {
                setEigenschaften(eigenschaften);
                SetModus(TZeichenModus.Normal);
                
            }
            else
            {
                SetModus(TZeichenModus.Normal);
            }
        }

        public TEigenschaften getEigenschaften()
        {
            return GetEigenschaften();
        }
        public void SetEigenschaften(TEigenschaften eig)
        {
            setEigenschaften(eig);
        }

        protected override void firstpoint(TZeichnung zeichnung, Canvas canvas)
        {
            base.firstpoint(zeichnung, canvas);
            TLinie l = this;

            TLinie linie = new TLinie(canvas, base.FPos2);
            TEigenschaften neweig = getnewEig(l, base.FPos2);

            MainWindow.tauschePunkte = true;
            
            MainWindow.TmpFigur = linie;

            MainWindow.modus = MainWindow.TModus.Linie;
            MainWindow.status = MainWindow.TStatus.Linie2;
            ((TLinie)MainWindow.TmpFigur).setEigenschaften(neweig);
        }

        protected override void secondpoint(TZeichnung zeichnung, Canvas canvas)
        {
            base.secondpoint(zeichnung, canvas);
            TLinie l = this;

            TLinie linie = new TLinie(canvas, base.FPos1);
            TEigenschaften neweig = getnewEig(l, base.FPos1);

            MainWindow.TmpFigur = linie;

            MainWindow.modus = MainWindow.TModus.Linie;
            MainWindow.status = MainWindow.TStatus.Linie2;
            ((TLinie)MainWindow.TmpFigur).setEigenschaften(neweig);
        }

        private TEigenschaften getnewEig(TLinie from, Point point)
        {
            TEigenschaften eig = from.getEigenschaften();
            eig.pos1 = point;
            eig.pos2 = point;
            return eig;
        }

    }
}
