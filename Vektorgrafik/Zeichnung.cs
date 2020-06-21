using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;


namespace Vektorgrafik
{
    public class TZeichnung : List<TFigur>
    {
        //Liste mit Figuren
        public void EntferneFigur(TFigur figur)
        {
            figur.Entfernen();
            Remove(figur);
        }

        private String datei = "";

        private int version = 1;
        private String signatur = "JonasZeichenProgramm";

        private static bool changed;
        public bool change
        {
            get { return changed; }
            set { changed = value; }
        }

        public String getSignatur { get { return signatur; } }
        public int getVersion { get { return version; } }
        public String DateiName { get { return datei; } set { datei = value; } }

        public void save_unter()
        {
            SaveFileDialog savefile;

            savefile = new SaveFileDialog();
            savefile.Filter = "Jonas super Zeichenprogramm Dateien|*.jszd|Alle Dateien|*.*";
            savefile.FilterIndex = 0;
            
            if (datei != String.Empty)
            {
                savefile.FileName = datei;
            }
            if (savefile.ShowDialog() == null)
            {
                return;
            }
            datei = savefile.FileName;
            if(datei != String.Empty)
            {
                save();
            }
            
        }

        public void save()
        {

            if (datei == String.Empty)
            {
                save_unter();
            }
            else
            {
                save_intern();
                change = false;
                
            }


        }

        private void save_intern()
        {
            FileStream Datei;
            BinaryWriter Writer;
            int I;
            Datei = new FileStream(datei, FileMode.Create);
            try
            {
                int anzahldatensaetze = Count;
                int satz = 0;

                Writer = new BinaryWriter(Datei);
                Writer.Write(signatur);
                Writer.Write(version);
                Writer.Write(anzahldatensaetze);
                for (I = 0; I < anzahldatensaetze; I++)
                {
                    parse(Writer, satz);
                    satz++;
                }
                Writer.Flush();
            }
            finally
            {
                Datei.Close();

            }
        }


        private void parse(BinaryWriter Writer, int i)
        {
            if (this[i].GetType().Name == "TDreieck")
            {
                writeDreieck(Writer, (TDreieck)this[i]);
            }
            if (this[i].GetType().Name == "TLinie")
            {
                writeLinie(Writer, (TLinie)this[i]);
            }
            if (this[i].GetType().Name == "TQuadrat")
            {
                writeQuadrat(Writer, (TQuadrat)this[i]);

            }
            if (this[i].GetType().Name == "TRechteck")
            {
                writeRechteck(Writer, (TRechteck)this[i]);
            }
            if (this[i].GetType().Name == "TEllipse")
            {
                writeEllipse(Writer, (TEllipse)this[i]);
            }
            if (this[i].GetType().Name == "TKreis")
            {
                writeKreis(Writer, (TKreis)this[i]);
            }
        }

        private void writeDreieck(BinaryWriter Writer, TDreieck dreieck)
        {
            Writer.Write(1);
            Writer.Write(dreieck.Pos1.X);
            Writer.Write(dreieck.Pos1.Y);
            Writer.Write(dreieck.Pos2.X);
            Writer.Write(dreieck.Pos2.Y);
            Writer.Write(dreieck.Pos3.X);
            Writer.Write(dreieck.Pos3.Y);

            TEigenschaften eig = dreieck.getEigenschaften();
            Writer.Write(eig.StandartPinsel);
            Writer.Write(eig.StandartStift);
            Writer.Write(eig.StandartStiftDicke);

            Writer.Write(eig.BenutzerDicke);

            Writer.Write(BrushtoID(eig.BenutzerPinsel));
            Writer.Write(BrushtoID(eig.BenutzerStift));
        }
        private void writeRechteck(BinaryWriter Writer, TRechteck rechteck)
        {
            Writer.Write(2);
            Writer.Write(rechteck.Pos1.X);
            Writer.Write(rechteck.Pos1.Y);
            Writer.Write(rechteck.Pos2.X);
            Writer.Write(rechteck.Pos2.Y);

            TEigenschaften eig = rechteck.getEigenschaften();
            Writer.Write(eig.StandartPinsel);
            Writer.Write(eig.StandartStift);
            Writer.Write(eig.StandartStiftDicke);

            Writer.Write(eig.BenutzerDicke);

            Writer.Write(BrushtoID(eig.BenutzerPinsel));
            Writer.Write(BrushtoID(eig.BenutzerStift));
        }
        private void writeQuadrat(BinaryWriter Writer, TQuadrat quadrat)
        {
            Writer.Write(3);
            Writer.Write(quadrat.Pos1.X);
            Writer.Write(quadrat.Pos1.Y);
            Writer.Write(quadrat.Pos2.X);
            Writer.Write(quadrat.Pos2.Y);

            TEigenschaften eig = quadrat.getEigenschaften();
            Writer.Write(eig.StandartPinsel);
            Writer.Write(eig.StandartStift);
            Writer.Write(eig.StandartStiftDicke);

            Writer.Write(eig.BenutzerDicke);

            Writer.Write(BrushtoID(eig.BenutzerPinsel));
            Writer.Write(BrushtoID(eig.BenutzerStift));
        }
        private void writeLinie(BinaryWriter Writer, TLinie linie)
        {
            Writer.Write(4);
            Writer.Write(linie.Pos1.X);
            Writer.Write(linie.Pos1.Y);
            Writer.Write(linie.Pos2.X);
            Writer.Write(linie.Pos2.Y);

            TEigenschaften eig = linie.getEigenschaften();
            Writer.Write(eig.StandartStift);
            Writer.Write(eig.StandartStiftDicke);

            Writer.Write(eig.BenutzerDicke);

            Writer.Write(BrushtoID(eig.BenutzerStift));
        }
        private void writeEllipse(BinaryWriter Writer, TEllipse ellipse)
        {
            Writer.Write(5);
            Writer.Write(ellipse.Pos1.X);
            Writer.Write(ellipse.Pos1.Y);
            Writer.Write(ellipse.Pos2.X);
            Writer.Write(ellipse.Pos2.Y);

            TEigenschaften eig = ellipse.getEigenschaften();
            Writer.Write(eig.StandartPinsel);
            Writer.Write(eig.StandartStift);
            Writer.Write(eig.StandartStiftDicke);

            Writer.Write(eig.BenutzerDicke);

            Writer.Write(BrushtoID(eig.BenutzerPinsel));
            Writer.Write(BrushtoID(eig.BenutzerStift));
        }
        private void writeKreis(BinaryWriter Writer, TKreis kreis)
        {
            Writer.Write(6);
            Writer.Write(kreis.Pos1.X);
            Writer.Write(kreis.Pos1.Y);
            Writer.Write(kreis.Pos2.X);
            Writer.Write(kreis.Pos2.Y);

            TEigenschaften eig = kreis.getEigenschaften();
            Writer.Write(eig.StandartPinsel);
            Writer.Write(eig.StandartStift);
            Writer.Write(eig.StandartStiftDicke);

            Writer.Write(eig.BenutzerDicke);

            Writer.Write(BrushtoID(eig.BenutzerPinsel));
            Writer.Write(BrushtoID(eig.BenutzerStift));
        }

        //Farben zum Speichern ID geben
        private int BrushtoID(SolidColorBrush b)
        {
            int i, Count;
            PropertyInfo iteminfo;
            SolidColorBrush itembrush;
            Count = typeof(Brushes).GetProperties().ToArray().Count();
            for (i = 0; i < Count; i++)
            {
                iteminfo = (PropertyInfo)(typeof(Brushes).GetProperties().ToArray()[i]);
                itembrush = (SolidColorBrush)(iteminfo.GetValue(null, null));
                if (itembrush.Equals(b))
                {
                    return i;
                }
            }
            return 0;
        }


    }

}
