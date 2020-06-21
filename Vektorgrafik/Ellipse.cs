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
    class TEllipse : TFigur
    {
        private Ellipse ellipse;
        public TEllipse(Canvas ACanvasZeichnung, Point APos1) : base(ACanvasZeichnung, APos1)
        {
            ellipse = new Ellipse();
            ellipse.Tag = this;
            Figur = ellipse;

            SetModus(TZeichenModus.Normal);
            CanvasZeichnung.Children.Add(ellipse);
        }
        protected virtual void setPos()
        {
            double Left, Top, Width, Height;
            Left = Math.Min(Pos1.X, Pos2.X);
            Top = Math.Min(Pos1.Y, Pos2.Y);
            Width = Math.Abs(Pos2.X - Pos1.X);
            Height = Math.Abs(Pos2.Y - Pos1.Y);
            Canvas.SetLeft(ellipse, Left);
            Canvas.SetTop(ellipse, Top);
            ellipse.Width = Width;
            ellipse.Height = Height;
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
            TEllipse l = this;

            TEllipse rechteck = new TEllipse(canvas, base.FPos2);
            TEigenschaften neweig = getnewEig(l, base.FPos2);

            MainWindow.tauschePunkte = true;

            MainWindow.TmpFigur = rechteck;

            MainWindow.modus = MainWindow.TModus.Ellipse;
            MainWindow.status = MainWindow.TStatus.Linie2;
            ((TEllipse)MainWindow.TmpFigur).setEigenschaften(neweig);
        }

        protected override void secondpoint(TZeichnung zeichnung, Canvas canvas)
        {
            base.secondpoint(zeichnung, canvas);
            TEllipse l = this;

            TEllipse rechteck = new TEllipse(canvas, base.FPos1);
            TEigenschaften neweig = getnewEig(l, base.FPos1);

            MainWindow.TmpFigur = rechteck;

            MainWindow.modus = MainWindow.TModus.Ellipse;
            MainWindow.status = MainWindow.TStatus.Linie2;
            ((TEllipse)MainWindow.TmpFigur).setEigenschaften(neweig);
        }

        private TEigenschaften getnewEig(TEllipse from, Point point)
        {
            TEigenschaften eig = from.getEigenschaften();
            eig.pos1 = point;
            eig.pos2 = point;
            return eig;
        }
    }
}
