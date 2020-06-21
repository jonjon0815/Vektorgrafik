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
    class TDreieck : TFigur
    {
        private Polygon dreieck;

        //Dreieck Methode
        public TDreieck(Canvas ACanvasZeichnung, Point APos1) : base(ACanvasZeichnung, APos1)
        {
            dreieck = new Polygon();
            dreieck.Tag = this;
            Figur = dreieck;
            dreieck.Points.Add(APos1);
            SetModus(TZeichenModus.Normal);
            CanvasZeichnung.Children.Add(dreieck);
            
        }
        protected override void SetPos1(Point APos1)
        {
            FPos1 = APos1;
            dreieck.Points.Add(APos1);
        }
        protected override void SetPos2(Point APos2)
        {
            FPos2 = APos2;
            dreieck.Points.Add(APos2);
        }

        //position 3 wird hinzugefügt
        protected void SetPos3(Point APos3)
        {
            FPos3 = APos3;
            dreieck.Points.Add(APos3);
        }
        public Point Pos3
        {
            get { return FPos3; }
            set { SetPos3(value); }
        }
        //remove point, sonst sehr viele Punkte, da Punkte mit .Add hinzugefügt werden
        public void removeLastPoint()
        {
            dreieck.Points.RemoveAt(1);
        }
        public void removeLastPoint2()
        {
            dreieck.Points.RemoveAt(2);
        }

        //überschreiben des zeigeEigenschaften
        public override void zeigeEigenschaftenDialog()
        {

            SetModus(TZeichenModus.Highlight);
            TEigenschaften eigenschaften = GetEigenschaften();
            DialogEigenschaftenDreieck dlg = new DialogEigenschaftenDreieck(eigenschaften);
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

        protected override TEigenschaften GetEigenschaften()
        {
            TEigenschaften eigenschaften;
            eigenschaften = new TEigenschaften();
            eigenschaften.pos1 = FPos1;
            eigenschaften.pos2 = FPos2;
            eigenschaften.pos3 = FPos3;

            eigenschaften.BenutzerStift = BenutzerStift;
            eigenschaften.BenutzerPinsel = BenutzerPinsel;
            eigenschaften.BenutzerDicke = BenutzerDicke;

            eigenschaften.StandartStift = StandartStift;
            eigenschaften.StandartPinsel = StandartPinsel;
            eigenschaften.StandartStiftDicke = StandartDicke;

            return eigenschaften;
        }

        protected override void setEigenschaften(TEigenschaften eigenschaften)
        {
            CanvasZeichnung.Children.Remove(dreieck);

            SetPos1(eigenschaften.pos1);
            SetPos2(eigenschaften.pos2);
            SetPos3(eigenschaften.pos3);

            BenutzerStift = eigenschaften.BenutzerStift;
            BenutzerPinsel = eigenschaften.BenutzerPinsel;
            BenutzerDicke = eigenschaften.BenutzerDicke;

            StandartStift = eigenschaften.StandartStift;
            StandartPinsel = eigenschaften.StandartPinsel;
            StandartDicke = eigenschaften.StandartStiftDicke;

            dreieck = new Polygon();
            dreieck.Tag = this;

            Figur = dreieck;

            dreieck.Points.Add(FPos1);
            dreieck.Points.Add(FPos2);
            dreieck.Points.Add(FPos3);

            SetModus(TZeichenModus.Normal);
            CanvasZeichnung.Children.Add(dreieck);
        }
        public TEigenschaften getEigenschaften()
        {
            return GetEigenschaften();
        }

        public void SetEigenschaften(TEigenschaften eig)
        {
            setEigenschaften(eig);
        }


        public override void ZeigeKontextMenu(TZeichnung zeichnung, Canvas canvas)
        {
            ContextMenu menu;
            MenuItem m;
            menu = new ContextMenu();
            m = new MenuItem();
            m.Header = "Löschen";
            m.Click += delegate { loescheFigur(zeichnung); };
            menu.Items.Add(m);

            m = new MenuItem();
            m.Header = "Eigenschaften";
            m.Click += delegate { zeigeEigenschaftenDialog(); };
            menu.Items.Add(m);

            MenuItem r = new MenuItem();
            r.Header = "Punkte ändern";

            m = new MenuItem();
            m.Header = "1. Punkt ändern";
            m.Click += delegate { firstpoint(zeichnung, canvas); };
            r.Items.Add(m);

            m = new MenuItem();
            m.Header = "2. Punkt ändern";
            m.Click += delegate { secondpoint(zeichnung, canvas); };
            r.Items.Add(m);

            m = new MenuItem();
            m.Header = "3. Punkt ändern";
            m.Click += delegate { thirdpoint(zeichnung, canvas); };
            r.Items.Add(m);

            menu.Items.Add(r);

            menu.IsOpen = true;
        }

        private void thirdpoint(TZeichnung zeichnung, Canvas canvas)
        {
            base.secondpoint(zeichnung, canvas);
            TDreieck l = this;

            TDreieck rechteck = new TDreieck(canvas, base.FPos1);
            rechteck.SetPos2(l.FPos2);
            TEigenschaften neweig = getnewEig(l, base.FPos1, l.FPos2);

            MainWindow.TmpFigur = rechteck;

            MainWindow.modus = MainWindow.TModus.Dreieck;
            MainWindow.status = MainWindow.TStatus.Dreieck3;
            ((TDreieck)MainWindow.TmpFigur).setEigenschaften(neweig);
        }

        protected override void firstpoint(TZeichnung zeichnung, Canvas canvas)
        {
            base.firstpoint(zeichnung, canvas);
            TDreieck l = this;

            TDreieck rechteck = new TDreieck(canvas, l.FPos2);
            rechteck.SetPos2(l.FPos3);
            TEigenschaften neweig = getnewEig(l, base.FPos2, l.FPos3);

            MainWindow.tauschePunkte = true;
            MainWindow.tauscheDreieckID = 1;

            MainWindow.TmpFigur = rechteck;

            MainWindow.modus = MainWindow.TModus.Dreieck;
            MainWindow.status = MainWindow.TStatus.Dreieck3;
            ((TDreieck)MainWindow.TmpFigur).setEigenschaften(neweig);
        }

        protected override void secondpoint(TZeichnung zeichnung, Canvas canvas)
        {
            base.secondpoint(zeichnung, canvas);
            TDreieck l = this;

            TDreieck rechteck = new TDreieck(canvas, base.FPos1);
            rechteck.SetPos2(l.FPos3);
            TEigenschaften neweig = getnewEig(l, base.FPos1, l.FPos3);

            MainWindow.tauschePunkte = true;
            MainWindow.tauscheDreieckID = 2;

            MainWindow.TmpFigur = rechteck;

            MainWindow.modus = MainWindow.TModus.Dreieck;
            MainWindow.status = MainWindow.TStatus.Dreieck3;
            ((TDreieck)MainWindow.TmpFigur).setEigenschaften(neweig);
        }

        private TEigenschaften getnewEig(TDreieck from, Point point, Point point2)
        {
            TEigenschaften eig = from.getEigenschaften();
            eig.pos1 = point;
            eig.pos2 = point2;
            eig.pos3 = point;
            return eig;
        }

        //punkte neu setzen punkte neu anordnen
        public void tauschePunkte_1()
        {
            Point tmp1 = FPos1;
            Point tmp2 = FPos2;
            Point tmp3 = FPos3;
            FPos1 = tmp3;
            FPos2 = tmp1;
            FPos3 = tmp2;
        }
        public void tauschePunkte_2()
        {
            Point tmp1 = FPos1;
            Point tmp2 = FPos2;
            Point tmp3 = FPos3;
            FPos1 = tmp1;
            FPos2 = tmp3;
            FPos3 = tmp2;
        }
    }
}
