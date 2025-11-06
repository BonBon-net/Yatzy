using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using Yatzy.YatzyDbContext;

namespace Yatzy
{
    /// <summary>
    /// Interaction logic for Terninger.xaml
    /// </summary>
    /// 

    public class Terning : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public int DiceValue { get; set; }
        public Image Terningside { get; set; }
        public Image TerningRamme { get; set; }
        public bool IsHeld { get; set; }
    }

    public partial class Terninger : UserControl
    {
        //Terninger TerningUserControl = new Terninger();
        FuncLayer FuncLayer = new FuncLayer();

        Terning[] AlleTerninger = new Terning[5];
        //Model Model { get; set; } = new Model();

        // Create a Random instance (ideally as a field, not inside a method for repeated use)
        private readonly Random rnd = new Random();
        public string[] TerningSides = new string[] {
            "C:\\Users\\66186\\source\\repos\\Yatzy\\Yatzy\\Terning\\1.jpg", "C:\\Users\\66186\\source\\repos\\Yatzy\\Yatzy\\Terning\\2.jpg",
            "C:\\Users\\66186\\source\\repos\\Yatzy\\Yatzy\\Terning\\3.jpg", "C:\\Users\\66186\\source\\repos\\Yatzy\\Yatzy\\Terning\\4.jpg",
            "C:\\Users\\66186\\source\\repos\\Yatzy\\Yatzy\\Terning\\5.jpg", "C:\\Users\\66186\\source\\repos\\Yatzy\\Yatzy\\Terning\\6.jpg"
        };
        public string SelectetTerning = "C:\\Users\\66186\\source\\repos\\Yatzy\\Yatzy\\Terning\\00bfff.png";
        private int setRndTerningImage(Image imgTerning)
        {
            int diceValue = rnd.Next(0, TerningSides.Count()) + 1;
            imgTerning.SetValue(Image.SourceProperty, new BitmapImage(new Uri(TerningSides[diceValue - 1])));
            return diceValue;
        }

        private void VisTerningKast(Terning terning)
        {
            terning.Terningside.SetValue(Image.SourceProperty, new BitmapImage(new Uri(TerningSides[terning.DiceValue - 1])));
        }

        private void ChangeTerningImage(Image imgTerning, string imagePath)
        {
            imgTerning.SetValue(Image.SourceProperty, new BitmapImage(new Uri(imagePath)));
        }

        bool ManualDeveloper_CheckDataIsTrue = true;
        public Terninger()
        {
            InitializeComponent();
            DataContext = FuncLayer;
            imgTerningSelected1.Visibility = Visibility.Hidden;



            for (int i = 0; i < 5; i++)
            {
                Terning terning = new Terning();
                terning.DiceValue = rnd.Next(0, TerningSides.Count()) + 1;
                terning.TerningRamme = (Image)FindName($"imgTerning{i + 1}");
                VisTerningKast(terning);
                terning.IsHeld = false;

                AlleTerninger[i] = terning;

                //AlleTerninger[i].diceValue = setRndTerningImage((Image)FindName($"imgTerning{i + 1}"));
            }
        }

        private async void KastTerninger_Click(object sender, RoutedEventArgs e)
        {
            int[] RulleTerninger = new int[] { 0, 0, 0, 0, 0 };
            int kastet = 0;
            for (int i = 0; i < RulleTerninger.Count(); i++)
            {
                RulleTerninger[i] = rnd.Next(0, 35);
                if (kastet < RulleTerninger[i])
                {
                    kastet = RulleTerninger[i];
                }
            }
            for (int i = 0; i < kastet; i++)
            {
                await Task.Delay(145);
                for (int j = 0; j < RulleTerninger.Count(); j++)
                {
                    if (RulleTerninger[j] > 0)
                    {
                        setRndTerningImage((Image)FindName($"imgTerning{j + 1}"));
                        RulleTerninger[j] -= 1;
                    }
                }
            }
            if (ManualDeveloper_CheckDataIsTrue)
            {
                MessageBox.Show(
                    AlleTerninger[0].DiceValue.ToString() + "-" +
                    AlleTerninger[1].DiceValue.ToString() + "-" +
                    AlleTerninger[2].DiceValue.ToString() + "-" +
                    AlleTerninger[3].DiceValue.ToString() + "-" +
                    AlleTerninger[4].DiceValue.ToString(), "Test data for terninger");
            }
        }

        private void Terning1Selected_MouseClick(object sender, MouseButtonEventArgs e)
        {
            Image img = sender as Image;
            if (img != null)
            {
                SelectedTerning(imgTerningSelected1, img);
            }
        }

        private void Terning2Selected_MouseClick(object sender, MouseButtonEventArgs e)
        {
            Image img = sender as Image;
            if (img != null)
            {
                SelectedTerning(imgTerningSelected2, img);
            }
        }

        private void Terning3Selected_MouseClick(object sender, MouseButtonEventArgs e)
        {
            Image img = sender as Image;
            if (img != null)
            {
                SelectedTerning(imgTerningSelected3, img);
            }
        }

        private void Terning4Selected_MouseClick(object sender, MouseButtonEventArgs e)
        {
            Image img = sender as Image;
            if (img != null)
            {
                SelectedTerning(imgTerningSelected4, img);
            }
        }

        private void Terning5Selected_MouseClick(object sender, MouseButtonEventArgs e)
        {
            Image img = sender as Image;
            if (img != null)
            {
                SelectedTerning(imgTerningSelected5, img);
            }
        }

        private void SelectedTerning(Image imgTerning, Image img)
        {
            if (imgTerning.Visibility == Visibility.Hidden)
            {
                ChangeTerningImage(imgTerning, SelectetTerning);
                imgTerning.Visibility = Visibility.Visible;
            }
            else if (imgTerning.Visibility == Visibility.Visible)
            {
                imgTerning.Visibility = Visibility.Hidden;
            }
        }
    }
}
