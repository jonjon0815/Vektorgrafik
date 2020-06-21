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
    public enum TZeichenModus { Entwurf, Normal, Highlight}

    public abstract class TFigur
    {
        //Figur Variablen
        protected static SolidColorBrush entwurfStift = Brushes.Red;
        protected static SolidColorBrush entwurfPinsel = Brushes.Transparent;
        protected static double entwurfDicke = 1.0;

        protected static SolidColorBrush normalStift = Brushes.Black;
        protected static SolidColorBrush normalPinsel = Brushes.Silver;
        protected static double normalDicke = 2.0;

        protected static SolidColorBrush highStift = Brushes.Red;
        protected static SolidColorBrush highPinsel = Brushes.Red;
        protected static double highDicke = 2.0;

        protected bool StandartStift, StandartDicke, StandartPinsel;
        protected SolidColorBrush BenutzerStift, BenutzerPinsel;
        protected double BenutzerDicke;

        protected Canvas CanvasZeichnung;
        protected Point FPos1, FPos2, FPos3;
        protected Shape Figur;
        protected TZeichenModus FModus;

        //positions
        public Point Pos1
        {
            get { return FPos1; }
            set { SetPos1(value); }
        }
        public Point Pos2
        {
            get { return FPos2; }
            set { SetPos2(value); }
        }

        //figur.modus
        public TZeichenModus Modus
        {
            get { return FModus; }
            set { SetModus(value); }
        }
        //figur zeichen
        public TFigur(Canvas ACanvasZeichnung, Point APos1)
        {
            CanvasZeichnung = ACanvasZeichnung;
            FPos1 = APos1;
            FPos2 = APos1;
            FModus = TZeichenModus.Normal;

            StandartStift = true;
            BenutzerStift = normalStift;
            StandartDicke = true;
            BenutzerDicke = normalDicke;
            StandartPinsel = true;
            BenutzerPinsel = normalPinsel;
        }

        protected abstract void SetPos1(Point APos1);
        protected abstract void SetPos2(Point APos2);

        protected void SetModus(TZeichenModus AModus)
        {
            FModus = AModus;
            switch (FModus)
            {
                case TZeichenModus.Entwurf:
                    Figur.Stroke = entwurfStift;
                    Figur.StrokeThickness = entwurfDicke;
                    Figur.Fill = entwurfPinsel;
                    
                    break;
                case TZeichenModus.Normal:
                    if (StandartStift)
                    {
                        Figur.Stroke = normalStift;
                    }
                    else
                    {
                        Figur.Stroke = BenutzerStift;
                    }
                    if (StandartDicke)
                    {
                        Figur.StrokeThickness = normalDicke;
                    }
                    else
                    {
                        Figur.StrokeThickness = BenutzerDicke;
                    }
                    if (StandartPinsel)
                    {
                        Figur.Fill = normalPinsel;
                    }
                    else
                    {
                        Figur.Fill = BenutzerPinsel;
                    }
                    break;
                case TZeichenModus.Highlight:
                    Figur.Stroke = highStift;
                    Figur.Fill = highPinsel;
                    break;
            }
        }
        //figur entfernen
        public void Entfernen()
        {
            CanvasZeichnung.Children.Remove(Figur);
        }
        //eingenschaften Dialog
        public abstract void zeigeEigenschaftenDialog();

        //eigenschaften get
        protected virtual TEigenschaften GetEigenschaften()
        {
            TEigenschaften eigenschaften;
            eigenschaften = new TEigenschaften();
            eigenschaften.pos1 = FPos1;
            eigenschaften.pos2 = FPos2;
            eigenschaften.pos3 = new Point(0.0, 0.0);

            eigenschaften.BenutzerStift = BenutzerStift;
            eigenschaften.BenutzerPinsel = BenutzerPinsel;
            eigenschaften.BenutzerDicke = BenutzerDicke;

            eigenschaften.StandartStift = StandartStift;
            eigenschaften.StandartPinsel = StandartPinsel;
            eigenschaften.StandartStiftDicke = StandartDicke;

            return eigenschaften;
        }

        //eigenschaften set
        protected virtual void setEigenschaften(TEigenschaften eigenschaften)
        {
            SetPos1(eigenschaften.pos1);
            SetPos2(eigenschaften.pos2);

            BenutzerStift = eigenschaften.BenutzerStift;
            BenutzerPinsel = eigenschaften.BenutzerPinsel;
            BenutzerDicke = eigenschaften.BenutzerDicke;

            StandartStift = eigenschaften.StandartStift;
            StandartPinsel = eigenschaften.StandartPinsel;
            StandartDicke = eigenschaften.StandartStiftDicke;
        }

        //context menü bei rechtsklick erstellen und öffnen
        //kontext menü
        public virtual void ZeigeKontextMenu(TZeichnung zeichnung, Canvas canvas)
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

            menu.Items.Add(r);

            menu.IsOpen = true;
        }

        //virtual punkt tauschen
        public virtual void tauschePunkte()
        {
            Point tmp = FPos2;
            FPos2 = FPos1;
            FPos1 = tmp;
        }

        protected virtual void firstpoint(TZeichnung zeichnung, Canvas canvas)
        {
            zeichnung.EntferneFigur(this);
        }

        protected virtual void secondpoint(TZeichnung zeichnung, Canvas canvas)
        {
            zeichnung.EntferneFigur(this);
        }

        //Kontext menü löschen mit bestätigung
        protected void loescheFigur(TZeichnung zeichnung)
        {
            Modus = TZeichenModus.Highlight;
            if (MessageBox.Show("Soll diese Figur wirklich gelöscht werden?", "Figur Löschen", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                zeichnung.EntferneFigur(this);
            }
            else
            {
                Modus = TZeichenModus.Normal;
            }
        }
    }

    //Eigenschaften Klasse
    public class TEigenschaften
    {
        public Point pos1, pos2, pos3;
        public bool StandartStift, StandartStiftDicke, StandartPinsel;
        public SolidColorBrush BenutzerStift, BenutzerPinsel;
        public double BenutzerDicke;



        public void nullAllPoints(Point point)
        {
            Point p = new Point(float.NaN, float.NaN);
            pos1 = point;
            pos2 = point;
            pos3 = p;
        }
    }
}
