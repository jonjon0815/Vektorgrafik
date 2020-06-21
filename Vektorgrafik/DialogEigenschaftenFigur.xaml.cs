using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Vektorgrafik
{
    /// <summary>
    /// Interaction logic for DialogEigenschaftenFigur.xaml
    /// </summary>
    /// 
    /// Dialog Winodw Preferences
    /// 
    public partial class DialogEigenschaftenFigur : Window
    {
        protected TEigenschaften eigenschaften;
        protected bool zwingeQuadrat;
        protected double x1, x2, y1, y2;

        
        public DialogEigenschaftenFigur(TEigenschaften AEigenschaften, bool zwingeQuadrat)
        {
            this.zwingeQuadrat = zwingeQuadrat;
            eigenschaften = AEigenschaften;
            InitializeComponent();
        }

        //Felder mit vorhandenen Eigenschaften füllen
        private void setEigenschaften()
        {
            X1.Text = (Math.Round(eigenschaften.pos1.X)).ToString();
            X2.Text = (Math.Round(eigenschaften.pos2.X)).ToString();

            x1 = (Math.Round(eigenschaften.pos1.X));
            x2 = (Math.Round(eigenschaften.pos2.X));

            X3.Text = (Math.Round(eigenschaften.pos3.X)).ToString();

            Y1.Text = (Math.Round(eigenschaften.pos1.Y)).ToString();
            Y2.Text = (Math.Round(eigenschaften.pos2.Y)).ToString();

            y1 = (Math.Round(eigenschaften.pos1.Y));
            y2 = (Math.Round(eigenschaften.pos2.Y));

            Y3.Text = (Math.Round(eigenschaften.pos3.Y)).ToString();


            SetBrush(ComboStiftFarbe, eigenschaften.BenutzerStift);
            SetBrush(ComboPinselFarbe, eigenschaften.BenutzerPinsel);

            Stifbreite.Text = eigenschaften.BenutzerDicke.ToString();

            Ckeck_Dicke.IsChecked = eigenschaften.StandartStiftDicke;
            Ckeck_Stift.IsChecked = eigenschaften.StandartStift;
            Ckeck_Pinsel.IsChecked = eigenschaften.StandartPinsel;
        }

        //Brush setzen
        private void SetBrush(ComboBox combobrush, SolidColorBrush brush)
        {
            int i, Count;
            PropertyInfo iteminfo;
            SolidColorBrush itembrush;
            Count = combobrush.Items.Count;
            for(i=0; i<Count; i++)
            {
                iteminfo = (PropertyInfo)(combobrush.Items[i]);
                itembrush = (SolidColorBrush)(iteminfo.GetValue(null, null));
                if (itembrush.Equals(brush))
                {
                    combobrush.SelectedIndex = i;
                    break;
                }
            }
        }

        //getBrush from Window
        private SolidColorBrush getBrush(ComboBox combox)
        {
            PropertyInfo iteminfo = (PropertyInfo)(combox.Items[combox.SelectedIndex]);
            SolidColorBrush itembrush = (SolidColorBrush)(iteminfo.GetValue(null, null));
            return itembrush;
        }

        //Initalize
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ComboStiftFarbe.ItemsSource = (typeof(Brushes)).GetProperties();
            ComboPinselFarbe.ItemsSource = (typeof(Brushes)).GetProperties();
            setEigenschaften();
        }

        //Okay BUtton
        //Set Eigenschaften to Figur
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int dicke, x1, x2, x3, y1, y2, y3;
            try
            {
                dicke = int.Parse(Stifbreite.Text);

                x1 = int.Parse(X1.Text);
                x2 = int.Parse(X2.Text);
                x3 = int.Parse(X3.Text);

                y1 = int.Parse(Y1.Text);
                y2 = int.Parse(Y2.Text);
                y3 = int.Parse(Y3.Text);

                eigenschaften.StandartStiftDicke = Ckeck_Dicke.IsChecked.Value;
                eigenschaften.StandartStift = Ckeck_Stift.IsChecked.Value;
                eigenschaften.StandartPinsel = Ckeck_Pinsel.IsChecked.Value;

                eigenschaften.pos1.X = x1;
                eigenschaften.pos2.X = x2;
                eigenschaften.pos3.X = x3;

                eigenschaften.pos1.Y = y1;
                eigenschaften.pos2.Y = y2;
                eigenschaften.pos3.Y = y3;

                eigenschaften.BenutzerDicke = dicke;
                
                eigenschaften.BenutzerStift = getBrush(ComboStiftFarbe);
                try
                {
                    eigenschaften.BenutzerPinsel = getBrush(ComboPinselFarbe);
                }
                catch
                {

                }

                DialogResult = true;
                this.Close();
            }
            catch
            {
                DialogResult = false;
                this.Close();
            }
            
        }

        //Close Button
        //Abbrechen
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        //Check Boxen
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ComboStiftFarbe.IsEnabled = false;
        }

        private void CheckBox_Checked_1(object sender, RoutedEventArgs e)
        {
            Stifbreite.IsEnabled = false;
        }

        private void CheckBox_Checked_2(object sender, RoutedEventArgs e)
        {
            ComboPinselFarbe.IsEnabled = false;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            ComboStiftFarbe.IsEnabled = true;
        }

        private void CheckBox_Unchecked_1(object sender, RoutedEventArgs e)
        {
            Stifbreite.IsEnabled = true;
        }

        private void CheckBox_Unchecked_2(object sender, RoutedEventArgs e)
        {
            ComboPinselFarbe.IsEnabled = true;
        }

        //text change for quadrat
        private void X1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (zwingeQuadrat)
            {
                try
                {
                    int x1 = int.Parse(X1.Text);
                    if (x1 < 0)
                    {
                        return;
                    }
                    double faktor = x1 / this.x1;
                    if ((x1 * faktor) < 0)
                    {
                        return;
                    }
                    Y1.Text = Math.Round((y1 * faktor)).ToString();
                }
                catch
                {

                }
            }
        }

        private void X2_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                int x2 = int.Parse(X2.Text);
                if (x2 < 0)
                {
                    return;
                }
                double faktor = x2 / this.x2;
                if ((y2 * faktor) < 0)
                {
                    return;
                }
                Y2.Text = Math.Round((y2 * faktor)).ToString();
            }
            catch
            {

            }
        }
    }
}
