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
    class TKreis : TEllipse
    {
        private Ellipse kreis;
        public TKreis(Canvas ACanvasZeichnung, Point APos1) : base(ACanvasZeichnung, APos1)
        {
            kreis = new Ellipse();
            kreis.Tag = this;
            Figur = kreis;
            setPos();
            SetModus(TZeichenModus.Normal);
            CanvasZeichnung.Children.Add(kreis);
        }
        protected override void setPos()
        {
            double Left = Math.Min(Pos1.X, Pos2.X);
            double Top = Math.Min(Pos1.Y, Pos2.Y);
            double Width = Math.Abs(Pos2.X - Pos1.X);
            double Height = Math.Abs(Pos2.Y - Pos1.Y);
            double length = Math.Min(Width, Height);
            Canvas.SetLeft(kreis, Left);
            Canvas.SetTop(kreis, Top);
            kreis.Width = length;
            kreis.Height = length;
        }

        protected override void firstpoint(TZeichnung zeichnung, Canvas canvas)
        {
            base.firstpoint(zeichnung, canvas);
            TKreis l = this;

            TKreis rechteck = new TKreis(canvas, base.FPos2);
            TEigenschaften neweig = getnewEig(l, base.FPos2);

            MainWindow.tauschePunkte = true;

            MainWindow.TmpFigur = rechteck;

            MainWindow.modus = MainWindow.TModus.Kreis;
            MainWindow.status = MainWindow.TStatus.Linie2;
            ((TKreis)MainWindow.TmpFigur).setEigenschaften(neweig);
        }

        protected override void secondpoint(TZeichnung zeichnung, Canvas canvas)
        {
            base.secondpoint(zeichnung, canvas);
            TKreis l = this;

            TKreis rechteck = new TKreis(canvas, base.FPos1);
            TEigenschaften neweig = getnewEig(l, base.FPos1);

            MainWindow.TmpFigur = rechteck;

            MainWindow.modus = MainWindow.TModus.Kreis;
            MainWindow.status = MainWindow.TStatus.Linie2;
            ((TKreis)MainWindow.TmpFigur).setEigenschaften(neweig);
        }

        private TEigenschaften getnewEig(TKreis from, Point point)
        {
            TEigenschaften eig = from.getEigenschaften();
            eig.pos1 = point;
            eig.pos2 = point;
            return eig;
        }
    }
}
