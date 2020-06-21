using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Vektorgrafik
{
    class TRechteck : TLinie
    {
        private Rectangle rechteck;
        public TRechteck(Canvas ACanvasZeichnung, Point APos1) : base(ACanvasZeichnung, APos1)
        {
            rechteck = new Rectangle();
            rechteck.Tag = this;
            Figur = rechteck;
            
            SetModus(TZeichenModus.Normal);
            CanvasZeichnung.Children.Add(rechteck);

        }
        protected virtual void setPos()
        {
            double Left, Top, Width, Height;
            Left = Math.Min(Pos1.X, Pos2.X);
            Top = Math.Min(Pos1.Y, Pos2.Y);
            Width = Math.Abs(Pos2.X - Pos1.X);
            Height = Math.Abs(Pos2.Y - Pos1.Y);
            Canvas.SetLeft(rechteck, Left);
            Canvas.SetTop(rechteck, Top);
            rechteck.Width = Width;
            rechteck.Height = Height;
        }
        protected override void SetPos1(Point APos1)
        {
            FPos1 = APos1;
            setPos();
        }
        protected override void SetPos2(Point APos2)
        {
            FPos2 = APos2;
            setPos();
        }
        public override void zeigeEigenschaftenDialog()
        {

            SetModus(TZeichenModus.Highlight);
            TEigenschaften eigenschaften = GetEigenschaften();
            DialogEigenschaftenFläche dlg = new DialogEigenschaftenFläche(eigenschaften);
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

        protected override void firstpoint(TZeichnung zeichnung, Canvas canvas)
        {
            base.firstpoint(zeichnung, canvas);
            TRechteck l = this;

            TRechteck rechteck = new TRechteck(canvas, base.FPos2);
            TEigenschaften neweig = getnewEig(l, base.FPos2);

            MainWindow.tauschePunkte = true;

            MainWindow.TmpFigur = rechteck;

            MainWindow.modus = MainWindow.TModus.Rechteck;
            MainWindow.status = MainWindow.TStatus.Linie2;
            ((TRechteck)MainWindow.TmpFigur).setEigenschaften(neweig);
        }

        protected override void secondpoint(TZeichnung zeichnung, Canvas canvas)
        {
            base.secondpoint(zeichnung, canvas);
            TRechteck l = this;

            TRechteck rechteck = new TRechteck(canvas, base.FPos1);
            TEigenschaften neweig = getnewEig(l, base.FPos1);

            MainWindow.TmpFigur = rechteck;

            MainWindow.modus = MainWindow.TModus.Rechteck;
            MainWindow.status = MainWindow.TStatus.Linie2;
            ((TRechteck)MainWindow.TmpFigur).setEigenschaften(neweig);
        }

        private TEigenschaften getnewEig(TRechteck from, Point point)
        {
            TEigenschaften eig = from.getEigenschaften();
            eig.pos1 = point;
            eig.pos2 = point;
            return eig;
        }
    }

}
