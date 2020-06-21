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
    class TQuadrat : TRechteck
    {
        private Rectangle quadrat;
        public TQuadrat(Canvas ACanvasZeichnung, Point APos1) : base(ACanvasZeichnung, APos1)
        {
            quadrat = new Rectangle();
            quadrat.Tag = this;
            Figur = quadrat;
            setPos();
            SetModus(TZeichenModus.Normal);
            CanvasZeichnung.Children.Add(quadrat);
        }
        protected override void setPos()
        {
            double Left = Math.Min(Pos1.X, Pos2.X);
            double Top = Math.Min(Pos1.Y, Pos2.Y);
            double Width = Math.Abs(Pos2.X - Pos1.X);
            double Height = Math.Abs(Pos2.Y - Pos1.Y);
            double length = Math.Min(Width, Height);
            Canvas.SetLeft(quadrat, Left);
            Canvas.SetTop(quadrat, Top);
            quadrat.Width = length;
            quadrat.Height = length;
        }
        public override void zeigeEigenschaftenDialog()
        {

            SetModus(TZeichenModus.Highlight);
            TEigenschaften eigenschaften = GetEigenschaften();
            DialogEigenschaftenQuadrat dlg = new DialogEigenschaftenQuadrat(eigenschaften);
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
            TQuadrat l = this;

            TQuadrat rechteck = new TQuadrat(canvas, base.FPos2);
            TEigenschaften neweig = getnewEig(l, base.FPos2);

            MainWindow.tauschePunkte = true;

            MainWindow.TmpFigur = rechteck;

            MainWindow.modus = MainWindow.TModus.Quadrat;
            MainWindow.status = MainWindow.TStatus.Linie2;
            ((TQuadrat)MainWindow.TmpFigur).setEigenschaften(neweig);
        }

        protected override void secondpoint(TZeichnung zeichnung, Canvas canvas)
        {
            base.secondpoint(zeichnung, canvas);
            TQuadrat l = this;

            TQuadrat rechteck = new TQuadrat(canvas, base.FPos1);
            TEigenschaften neweig = getnewEig(l, base.FPos1);

            MainWindow.TmpFigur = rechteck;

            MainWindow.modus = MainWindow.TModus.Quadrat;
            MainWindow.status = MainWindow.TStatus.Linie2;
            ((TQuadrat)MainWindow.TmpFigur).setEigenschaften(neweig);
        }

        private TEigenschaften getnewEig(TQuadrat from, Point point)
        {
            TEigenschaften eig = from.getEigenschaften();
            eig.pos1 = point;
            eig.pos2 = point;
            return eig;
        }
    }
}
