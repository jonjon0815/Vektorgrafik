using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;
using System.Drawing;
using System.IO;
using Microsoft.Win32;
using System.Reflection;

namespace Vektorgrafik
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        //Modus Enum
        public enum TModus { Bearbeiten, Linie, Dreieck, Rechteck, Quadrat, Ellipse, Kreis }
        //Status Enum
        public enum TStatus { Bearbeiten, Linie1, Linie2, Dreieck1, Dreieck2, Dreieck3 }
        public MainWindow()
        {
            InitializeComponent();
        }

        //basic Variablen
        public static TModus modus;
        public static TStatus status;
        public static TFigur TmpFigur;
        private TZeichnung zeichnung;

        public static bool tauschePunkte;
        public static int tauscheDreieckID;


        //Modus setzen, neue Zeichnung anlegen
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            status = TStatus.Bearbeiten;
            modus = TModus.Bearbeiten;
            TmpFigur = null;
            zeichnung = new TZeichnung();
            Bearbeiten.IsChecked = true;
            
        }
        private void Menu_exit_click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //print button
        private void Menu_drucken_click(object sender, RoutedEventArgs e)
        {
            PrintDialog prnt = new PrintDialog();

            if (prnt.ShowDialog() == true)
            {
                prnt.PrintVisual(CanvasZeichnung, "Printing Canvas");
            }
        }

        //Neu CLick
        private void Menu_neu_click(object sender, RoutedEventArgs e)
        {
            
            if (zeichnung.change)
            {
                MessageBoxResult r = MessageBox.Show("Die momentane Zeichnung ist nicht gespeichert. Wollen Sie speichern?", "Ungespeicherte Abeit?", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                if (r.Equals(MessageBoxResult.Yes))
                {
                    zeichnung.save();
                    CanvasZeichnung.Children.Clear();
                    zeichnung.Clear();
                }
                if (r.Equals(MessageBoxResult.No))
                {
                    CanvasZeichnung.Children.Clear();
                    zeichnung.Clear();
                    zeichnung.DateiName = String.Empty;
                }

            }
            else
            {
                CanvasZeichnung.Children.Clear();
                zeichnung.Clear();
            }
            
        }

        //Toolbox auswählen
        private void Click_box(object sender, RoutedEventArgs e)
        {
            modus = buttonToModus((ToggleButton)(sender));
            status = modusToStatus(modus);

            Bearbeiten.IsChecked = false;
            Linie.IsChecked = false;
            Dreieck.IsChecked = false;
            Ellipse.IsChecked = false;
            Kreis.IsChecked = false;
            Rechteck.IsChecked = false;
            Quadrat.IsChecked = false;
            ((ToggleButton)(sender)).IsChecked = true;

            entferneTmpFigur();
        }

        //button in modus
        private TModus buttonToModus(ToggleButton button)
        {
            if (button == Linie)
            {
                return TModus.Linie;
            }
            if (button == Dreieck)
            {
                return TModus.Dreieck;
            }
            if (button == Ellipse)
            {
                return TModus.Ellipse;
            }
            if (button == Kreis)
            {
                return TModus.Kreis;
            }
            if (button == Rechteck)
            {
                return TModus.Rechteck;
            }
            if (button == Quadrat)
            {
                return TModus.Quadrat;
            }
            return TModus.Bearbeiten;
        }

        //modus in status
        private TStatus modusToStatus(TModus modus)
        {
            switch (modus)
            {
                case TModus.Linie:
                case TModus.Rechteck:
                case TModus.Quadrat:
                case TModus.Ellipse:
                case TModus.Kreis:
                    return TStatus.Linie1;
                case TModus.Dreieck:
                    return TStatus.Dreieck1;
                default:
                    return TStatus.Bearbeiten;
            }
        }

        //Click in canvas zu methode zuordnen
        private void CanvasZeichnung_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            zeichnung.change = true;
            Point pos = e.GetPosition(CanvasZeichnung);
            switch (status)
            {
                case TStatus.Linie1:
                    first(pos);
                    break;
                case TStatus.Linie2:
                    second(pos);
                    break;
                case TStatus.Dreieck1:
                    Dreieck1(pos);
                    break;
                case TStatus.Dreieck2:
                    Dreieck2(pos);
                    break;
                case TStatus.Dreieck3:
                    Dreieck3(pos);
                    statusafteropen();
                    break;
            }
            
        }

        //erster klick in Canvas
        private void first(Point pos)
        {
            tauschePunkte = false;
            switch (modus)
            {
                case TModus.Linie:
                    Linie1(pos);
                    break;
                case TModus.Quadrat:
                    Quadrat1(pos);
                    break;
                case TModus.Rechteck:
                    Rechteck1(pos);
                    break;
                case TModus.Ellipse:
                    Ellipse1(pos);
                    break;
                case TModus.Kreis:
                    Kreis1(pos);
                    break;
            }
        }

        //zweiter Klick in canvas
        private void second(Point pos)
        {
            
            switch (modus)
            {
                case TModus.Linie:
                    Linie2(pos);
                    break;
                case TModus.Quadrat:
                    Quadrat2(pos);
                    break;
                case TModus.Rechteck:
                    Rechteck2(pos);
                    break;
                case TModus.Ellipse:
                    Ellipse2(pos);
                    break;
                case TModus.Kreis:
                    Kreis2(pos);
                    break;
            }
            statusafteropen();
        }

        //Canvas Dreick klick
        private void Dreieck1(Point pos)
        {
            TmpFigur = new TDreieck(CanvasZeichnung, pos);
            TmpFigur.Pos2 = pos;
            TmpFigur.Modus = TZeichenModus.Entwurf;
            status = TStatus.Dreieck2;
            
        }
        private void Dreieck2(Point pos)
        {
            TmpFigur.Pos2 = pos;
            ((TDreieck)TmpFigur).Pos3 = pos;
            TmpFigur.Modus = TZeichenModus.Normal;
            status = TStatus.Dreieck3;
        }
        private void Dreieck3(Point pos)
        {
            ((TDreieck)TmpFigur).Pos3 = pos;
            TmpFigur.Modus = TZeichenModus.Normal;
            zeichnung.Add(TmpFigur);
            if(tauschePunkte == true)
            {
                if(tauscheDreieckID == 1)
                {
                    ((TDreieck)TmpFigur).tauschePunkte_1();
                }
                if(tauscheDreieckID == 2)
                {
                    ((TDreieck)TmpFigur).tauschePunkte_2();
                }
                tauschePunkte = false;
            }
            TmpFigur = null;
            status = TStatus.Dreieck1;
        }

        //linie erster Punkt
        //Tmp figur erstellen und auf zweiten Punkt warten
        private void Linie1(Point pos)
        {
            TmpFigur = new TLinie(CanvasZeichnung, pos);
            TmpFigur.Modus = TZeichenModus.Entwurf;
            status = TStatus.Linie2;
        }
        //linie zweiter Punkt
        //Tmp Figur zweiter Punkt setzen und tmp Figur hinzufügen zu Canvas
        private void Linie2(Point pos)
        {
            TmpFigur.Pos2 = pos;
            TmpFigur.Modus = TZeichenModus.Normal;
            zeichnung.Add(TmpFigur);
            if(tauschePunkte == true)
            {
                TmpFigur.tauschePunkte();
                tauschePunkte = false;
            }
            TmpFigur = null;
            status = TStatus.Linie1;
        }
        //rechteck erster Punkt
        private void Rechteck1(Point pos)
        {
            TmpFigur = new TRechteck(CanvasZeichnung, pos);
            TmpFigur.Modus = TZeichenModus.Entwurf;
            status = TStatus.Linie2;
        }
        //Rechteck zweiter Punkt
        private void Rechteck2(Point pos)
        {
            TmpFigur.Pos2 = pos;
            TmpFigur.Modus = TZeichenModus.Normal;
            zeichnung.Add(TmpFigur);
            if (tauschePunkte == true)
            {
                TmpFigur.tauschePunkte();
                tauschePunkte = false;
            }
            TmpFigur = null;
            status = TStatus.Linie1;
        }
        //Quadrat erster Punkt
        private void Quadrat1(Point pos)
        {
            TmpFigur = new TQuadrat(CanvasZeichnung, pos);
            TmpFigur.Modus = TZeichenModus.Entwurf;
            status = TStatus.Linie2;
        }
        //Quadrat zweiter Punkt
        private void Quadrat2(Point pos)
        {
            TmpFigur.Pos2 = pos;
            TmpFigur.Modus = TZeichenModus.Normal;
            zeichnung.Add(TmpFigur);
            if (tauschePunkte == true)
            {
                TmpFigur.tauschePunkte();
                tauschePunkte = false;
            }
            TmpFigur = null;
            status = TStatus.Linie1;
        }
        //Ellipse erster Punkt
        private void Ellipse1(Point pos)
        {
            TmpFigur = new TEllipse(CanvasZeichnung, pos);
            TmpFigur.Modus = TZeichenModus.Entwurf;
            status = TStatus.Linie2;
        }
        //Ellipse zweiter Punkt
        private void Ellipse2(Point pos)
        {
            TmpFigur.Pos2 = pos;
            TmpFigur.Modus = TZeichenModus.Normal;
            zeichnung.Add(TmpFigur);
            if (tauschePunkte == true)
            {
                TmpFigur.tauschePunkte();
                tauschePunkte = false;
            }
            TmpFigur = null;
            status = TStatus.Linie1;
        }
        //Kreis erster Punkt
        private void Kreis1(Point pos)
        {
            TmpFigur = new TKreis(CanvasZeichnung, pos);
            TmpFigur.Modus = TZeichenModus.Entwurf;
            status = TStatus.Linie2;
        }
        //Kreis zweiter Punkt
        private void Kreis2(Point pos)
        {
            TmpFigur.Pos2 = pos;
            TmpFigur.Modus = TZeichenModus.Normal;
            zeichnung.Add(TmpFigur);
            if (tauschePunkte == true)
            {
                TmpFigur.tauschePunkte();
                tauschePunkte = false;
            }
            TmpFigur = null;
            status = TStatus.Linie1;
        }

        //Canvas Mouse move, zu modus zuordnen
        private void CanvasZeichnung_MouseMove(object sender, MouseEventArgs e)
        {
            
            Point pos = e.GetPosition(CanvasZeichnung);
            switch (status)
            {
                case TStatus.Linie2:
                    Linie2Move(pos);
                    break;
                case TStatus.Dreieck2:
                    Dreieck2Move(pos);
                    break;
                case TStatus.Dreieck3:
                    Dreieck3Move(pos);
                    break;
                case TStatus.Bearbeiten:
                    bearbeitenMove(pos);
                    break;
            }
        }

        //quadrat erzwingen oder linie tmp zeichnen
        private void Linie2Move(Point pos)
        {
            if ((modus == TModus.Quadrat) || (modus == TModus.Kreis))
            {
                TmpFigur.Pos2 = ErzwingeQuadrat(TmpFigur.Pos1, pos);
            }
            else
            {
                TmpFigur.Pos2 = pos;
            }

        }
        //dreieck letzter Punkt entfernen und neuen setzen
        private void Dreieck2Move(Point pos)
        {
            ((TDreieck)TmpFigur).removeLastPoint();
            TmpFigur.Pos2 = pos;
        }
        //dreieck letzter Punkt entfernen und neuen setzen
        private void Dreieck3Move(Point pos)
        {
            ((TDreieck)TmpFigur).removeLastPoint2();
            ((TDreieck)TmpFigur).Pos3 = pos;
        }

        //tmp figur entfernen
        private void entferneTmpFigur()
        {
            if (TmpFigur != null)
            {
                TmpFigur.Entfernen();
                TmpFigur = null;
            }
        }

        //rechtklick im canvas
        private void CanvasZeichnung_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            //tmp figur entfernen bei rechtsklick
            if (modus != TModus.Bearbeiten)
            {
                entferneTmpFigur();
                status = modusToStatus(modus);
                statusafteropen();
            }
            //else configuration öffnen, wenn objekt
            else
            {
                Point pos = e.GetPosition(CanvasZeichnung);
                TFigur figur = FangeFigur(pos);
                if (figur != null)
                {
                    figur.ZeigeKontextMenu(zeichnung, CanvasZeichnung);
                    zeichnung.change = true;
                }
            }
        }

        //quadrat erzwingen und mauszeiger auf bahn schieben
        private Point ErzwingeQuadrat(Point p1, Point p2)
        {
            double Width = Math.Abs(p2.X - p1.X);
            int singx = Math.Sign(p2.X - p1.X);
            double height = Math.Abs(p2.Y - p1.Y);
            int singy = Math.Sign(p2.Y - p1.Y);
            double length = Math.Min(Width, height);

            double posx = p1.X + singx * length;
            double posy = p1.Y + singy * length;

            Point offset = CanvasZeichnung.PointToScreen(new Point(0.0, 0.0));
            int cursorPosX = (int)(Math.Round(offset.X + posx));
            int cursorPosY = (int)(Math.Round(offset.Y + posy));
            SetCursorPos(cursorPosX, cursorPosY);
            return new Point(posx, posy);

        }

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll", EntryPoint = "SetCursorPos")]
        [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public static extern bool SetCursorPos(int X, int Y);

        //figur im canvas bei position suchen
        private TFigur FangeFigur(Point pos)
        {
            HitTestResult result;
            object treffer;
            result = VisualTreeHelper.HitTest(CanvasZeichnung, pos);
            if(result == null)
            {
                return null;
            }
            treffer = result.VisualHit;
            if (treffer == CanvasZeichnung)
            {
                return null;
            }
            return (TFigur)(((FrameworkElement)treffer).Tag);

        }

        //cursor ändern bei move über objekt
        private void bearbeitenMove(Point pos)
        {
            if (FangeFigur(pos) != null)
            {
                CanvasZeichnung.Cursor = Cursors.Hand;
            }
            else
            {
                CanvasZeichnung.Cursor = Cursors.Arrow;
            }
        }



        //menü klick
        private void Menu_save_click(object sender, RoutedEventArgs e)
        {
            zeichnung.save();
        }

        //speicher unter
        private void Menu_saveunter_click(object sender, RoutedEventArgs e)
        {
            zeichnung.save_unter();
        }

        //load intern
        private void load()
        {
            OpenFileDialog openfile;
            openfile = new OpenFileDialog();
            openfile.Filter = "Jonas super Zeichenprogramm Dateien|*.jszd|Alle Dateien|*.*";
            openfile.FilterIndex = 0;
            if (!openfile.ShowDialog().Value)
            {
                return;
            }
            zeichnung.DateiName = openfile.FileName;
            try
            {
                readdatei();
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
            
        }
        //read intern
        private void readdatei()
        {
            if (zeichnung.DateiName != String.Empty)
            {
                String signatur = zeichnung.getSignatur;
                int version = zeichnung.getVersion;
                FileStream Datei = new FileStream(zeichnung.DateiName, FileMode.Open);
                BinaryReader reader;
                int I;
                try
                {
                    reader = new BinaryReader(Datei);
                    if (reader.ReadString() != signatur)
                    {
                        throw new Exception("Falsche Signatur!");
                    }
                    if (reader.ReadInt32() > version)
                    {
                        throw new Exception("Die Datei wurde in einer zu neuen Version erzeugt!");
                    }
                    int rohdaten = 0;
                    rohdaten = reader.ReadInt32();

                    for (I = 0; I < rohdaten; I++)
                    {
                        LeseDatensatz(reader, I);
                    }

                }
                finally
                {
                    Datei.Close();

                }
            }
        }
        //datensatz lesen
        private void LeseDatensatz(BinaryReader read, int i)
        {
            
            int typ = read.ReadInt32();
            if(typ == 1)
            {
                Point p1 = new Point(read.ReadDouble(), read.ReadDouble());
                Point p2 = new Point(read.ReadDouble(), read.ReadDouble());
                Point p3 = new Point(read.ReadDouble(), read.ReadDouble());
                TEigenschaften eig = new TEigenschaften();
                eig.StandartPinsel = read.ReadBoolean();
                eig.StandartStift = read.ReadBoolean();
                eig.StandartStiftDicke = read.ReadBoolean();

                eig.BenutzerDicke = read.ReadDouble();

                eig.BenutzerPinsel = IDtoBrush(read.ReadInt32());
                eig.BenutzerStift = IDtoBrush(read.ReadInt32());

                eig.pos1 = p1;
                eig.pos2 = p2;
                eig.pos3 = p3;

                Dreieck1(p1);
                Dreieck2(p2);
                ((TDreieck)TmpFigur).SetEigenschaften(eig);
                Dreieck3(p3);
            }
            if (typ == 2)
            {
                Point p1 = new Point(read.ReadDouble(), read.ReadDouble());
                Point p2 = new Point(read.ReadDouble(), read.ReadDouble());
                TEigenschaften eig = new TEigenschaften();
                eig.StandartPinsel = read.ReadBoolean();
                eig.StandartStift = read.ReadBoolean();
                eig.StandartStiftDicke = read.ReadBoolean();

                eig.BenutzerDicke = read.ReadDouble();

                eig.BenutzerPinsel = IDtoBrush(read.ReadInt32());
                eig.BenutzerStift = IDtoBrush(read.ReadInt32());

                eig.pos1 = p1;
                eig.pos2 = p2;

                Rechteck1(p1);
                ((TRechteck)TmpFigur).SetEigenschaften(eig);
                Rechteck2(p2);
            }
            if (typ == 3)
            {
                Point p1 = new Point(read.ReadDouble(), read.ReadDouble());
                Point p2 = new Point(read.ReadDouble(), read.ReadDouble());
                TEigenschaften eig = new TEigenschaften();
                eig.StandartPinsel = read.ReadBoolean();
                eig.StandartStift = read.ReadBoolean();
                eig.StandartStiftDicke = read.ReadBoolean();

                eig.BenutzerDicke = read.ReadDouble();

                eig.BenutzerPinsel = IDtoBrush(read.ReadInt32());
                eig.BenutzerStift = IDtoBrush(read.ReadInt32());

                eig.pos1 = p1;
                eig.pos2 = p2;

                Quadrat1(p1);
                ((TQuadrat)TmpFigur).SetEigenschaften(eig);
                Quadrat2(p2);
            }
            if (typ == 4)
            {
                Point p1 = new Point(read.ReadDouble(), read.ReadDouble());
                Point p2 = new Point(read.ReadDouble(), read.ReadDouble());
                TEigenschaften eig = new TEigenschaften();
                eig.StandartStift = read.ReadBoolean();
                eig.StandartStiftDicke = read.ReadBoolean();

                eig.BenutzerDicke = read.ReadDouble();

                eig.BenutzerStift = IDtoBrush(read.ReadInt32());

                eig.pos1 = p1;
                eig.pos2 = p2;

                Linie1(p1);
                ((TLinie)TmpFigur).SetEigenschaften(eig);
                Linie2(p2);
            }
            if (typ == 5)
            {
                Point p1 = new Point(read.ReadDouble(), read.ReadDouble());
                Point p2 = new Point(read.ReadDouble(), read.ReadDouble());
                TEigenschaften eig = new TEigenschaften();
                eig.StandartPinsel = read.ReadBoolean();
                eig.StandartStift = read.ReadBoolean();
                eig.StandartStiftDicke = read.ReadBoolean();

                eig.BenutzerDicke = read.ReadDouble();

                eig.BenutzerPinsel = IDtoBrush(read.ReadInt32());
                eig.BenutzerStift = IDtoBrush(read.ReadInt32());

                eig.pos1 = p1;
                eig.pos2 = p2;

                Ellipse1(p1);
                ((TEllipse)TmpFigur).SetEigenschaften(eig);
                Ellipse2(p2);
            }
            if (typ == 6)
            {
                Point p1 = new Point(read.ReadDouble(), read.ReadDouble());
                Point p2 = new Point(read.ReadDouble(), read.ReadDouble());
                TEigenschaften eig = new TEigenschaften();
                eig.StandartPinsel = read.ReadBoolean();
                eig.StandartStift = read.ReadBoolean();
                eig.StandartStiftDicke = read.ReadBoolean();

                eig.BenutzerDicke = read.ReadDouble();

                eig.BenutzerPinsel = IDtoBrush(read.ReadInt32());
                eig.BenutzerStift = IDtoBrush(read.ReadInt32());

                eig.pos1 = p1;
                eig.pos2 = p2;

                Kreis1(p1);
                ((TKreis)TmpFigur).SetEigenschaften(eig);
                Kreis2(p2);
            }
            statusafteropen();
        }
        //it to brush convert
        private SolidColorBrush IDtoBrush(int id)
        {
            PropertyInfo iteminfo = (PropertyInfo)(typeof(Brushes).GetProperties().ToArray()[id]);
            SolidColorBrush itembrush = (SolidColorBrush)(iteminfo.GetValue(null, null));
            return itembrush;
        }

        //öffnen klick
        private void Menu_offnen_click(object sender, RoutedEventArgs e)
        {
            if (zeichnung.change)
            {
                MessageBoxResult r = MessageBox.Show("Die momentane Zeichnung ist nicht gespeichert. Wollen Sie speichern?", "Ungespeicherte Abeit?", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                if (r.Equals(MessageBoxResult.Yes))
                {
                    zeichnung.save();
                    CanvasZeichnung.Children.Clear();
                    zeichnung.Clear();
                    load();
                }
                if (r.Equals(MessageBoxResult.No))
                {
                    CanvasZeichnung.Children.Clear();
                    zeichnung.Clear();
                    zeichnung.DateiName = String.Empty;
                    load();
                }

            }
            else
            {
                load();
            }
            
        }

        private void Menu_neu_clisdck(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(typeof(Brushes).GetProperties().ToArray()[1].Name.ToString());
        }

        //window close prevent
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (zeichnung.change)
            {
                e.Cancel = true;
                MessageBoxResult r = MessageBox.Show("Die momentane Zeichnung ist nicht gespeichert. Wollen Sie speichern?", "Ungespeicherte Abeit?", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                if (r.Equals(MessageBoxResult.Yes))
                {
                    zeichnung.save();
                    e.Cancel = false;
                }
                if (r.Equals(MessageBoxResult.No))
                {
                    e.Cancel = false;
                }
                
            }
        }

        //set satus without click event
        private void statusafteropen()
        {
            if (Bearbeiten.IsChecked == true)
            {
                status = TStatus.Bearbeiten;
                modus = TModus.Bearbeiten;
            }
            if (Linie.IsChecked == true)
            {
                status = TStatus.Linie1;
                modus = TModus.Linie;
            }
            if (Dreieck.IsChecked == true)
            {
                status = TStatus.Dreieck1;
                modus = TModus.Dreieck;
            }
            if (Ellipse.IsChecked == true)
            {
                status = TStatus.Linie1;
                modus = TModus.Ellipse;
            }
            if (Kreis.IsChecked == true)
            {
                status = TStatus.Linie1;
                modus = TModus.Kreis;
            }
            if (Rechteck.IsChecked == true)
            {
                status = TStatus.Linie1;
                modus = TModus.Rechteck;
            }
            if (Quadrat.IsChecked == true)
            {
                status = TStatus.Linie1;
                modus = TModus.Quadrat;
            }
        }

        //strg+o/s/x press
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.O)
            {
                if(Keyboard.Modifiers == ModifierKeys.Control)
                {
                    load();
                }
            }
            if (e.Key == Key.S)
            {
                if (Keyboard.Modifiers == ModifierKeys.Control)
                {
                    zeichnung.save();
                }
            }
            if (e.Key == Key.X)
            {
                if (Keyboard.Modifiers == ModifierKeys.Control)
                {
                    this.Close();
                }
            }
        }

        private void shortcuts_view(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Speichern: Strg + s" + Environment.NewLine + "Öffnen: Strg + o" + Environment.NewLine + "Schließen: Strg + x", "Shortcuts", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
