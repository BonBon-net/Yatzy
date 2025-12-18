using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
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

        public int DiceValue { get; set; } = -1;
        public Image Terningside { get; set; } = new Image();
        public bool IsHeld { get; set; } = false;
    }

    public partial class Terninger : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        FuncLayer FuncLayer;

        private static string[] TerningSides = Directory.GetFiles($"{Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName}\\Terning\\6_Sides");
        private static string SelectetTerning = $"{Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName}\\Terning\\00bfff.png";
        private Terning[] AlleTerninger { get; set; } = new Terning[5];
        private BitmapImage[] BitmapImages { get; set; } = new BitmapImage[6] {
            new BitmapImage(new Uri(TerningSides[0])),
            new BitmapImage(new Uri(TerningSides[1])),
            new BitmapImage(new Uri(TerningSides[2])),
            new BitmapImage(new Uri(TerningSides[3])),
            new BitmapImage(new Uri(TerningSides[4])),
            new BitmapImage(new Uri(TerningSides[5]))
        };

        private Image[] TerningImages { get; set; }
        public Terning terning { get; set; } = new Terning();

        // Create a Random instance (ideally as a field, not inside a method for repeated use)
        private readonly Random rnd = new Random();

        bool ManualDeveloper_CheckDataIsTrue = true;
        public Terninger(FuncLayer FuncLayer)
        {
            InitializeComponent();
            this.FuncLayer = FuncLayer;
            DataContext = this.FuncLayer;
            for (int i = 0; i < AlleTerninger.Length; i++)
            {
                AlleTerninger[i] = new Terning();
                ((Image)FindName($"imgTerningSelected{i + 1}")).SetValue(Image.SourceProperty, new BitmapImage(new Uri(SelectetTerning)));
            }
            TerningImages = new Image[]
            {
                imgTerning1, imgTerning2, imgTerning3, imgTerning4, imgTerning5
            };
            KastTertinger();
        }

        private async void KastTerninger_Click(object sender, RoutedEventArgs e)
        {
            txbKastTilbage.Text = $"Kast: " + (ReturnThrowNumber() - 1);
            if (ReturnThrowNumber() <= 0)
            {
                btnKast.IsEnabled = false;
            }
            int[] RulleTerninger = { 0, 0, 0, 0, 0 };
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
                        KastTertinger();
                        RulleTerninger[j] -= 1;
                    }
                }
            }
            if (ManualDeveloper_CheckDataIsTrue)
            {
                string messageInput = string.Empty;
                for (int i = 0; i < AlleTerninger.Length; i++)
                {
                    if (messageInput != string.Empty)
                    {
                        messageInput += $"-{AlleTerninger[i].DiceValue.ToString()}";
                    }
                    else
                    {
                        messageInput = AlleTerninger[i].DiceValue.ToString();
                    }
                }
                MessageBox.Show(messageInput, "Test data for terninger");
            }
        }

        private void KastTertinger()
        {
            if (ReturnThrowNumber() > 0)
            {
                for (int i = 0; i < AlleTerninger.Length; i++)
                {
                    // Throw dice if not held
                    if (!AlleTerninger[i].IsHeld)
                    {
                        // Thorws dice 'i' for value
                        AlleTerninger[i].DiceValue = rnd.Next(0, TerningSides.Count()) + 1;

                        // Sets the new dice image in array 'AlleTerninger' at value 'i'
                        AlleTerninger[i].Terningside.SetValue(Image.SourceProperty, BitmapImages[AlleTerninger[i].DiceValue - 1]);

                        // Sets the new dice image in UI 'Image'
                        TerningImages[i].SetValue(Image.SourceProperty, BitmapImages[AlleTerninger[i].DiceValue - 1]);
                        //((Image)FindName($"imgTerning{i + 1}")).SetValue(Image.SourceProperty, BitmapImages[AlleTerninger[i].DiceValue - 1]);
                    }
                }
            }
        }

        private void Terning1Selected_MouseClick(object sender, MouseButtonEventArgs e)
        {
            if (sender as Image != null && ReturnThrowNumber() < 3)
            {
                SelectedTerning(imgTerningSelected1, 1);
            }
        }

        private void Terning2Selected_MouseClick(object sender, MouseButtonEventArgs e)
        {
            if (sender as Image != null && ReturnThrowNumber() < 3)
            {
                SelectedTerning(imgTerningSelected2, 2);
            }
        }

        private void Terning3Selected_MouseClick(object sender, MouseButtonEventArgs e)
        {
            if (sender as Image != null && ReturnThrowNumber() < 3)
            {
                SelectedTerning(imgTerningSelected3, 3);
            }
        }

        private void Terning4Selected_MouseClick(object sender, MouseButtonEventArgs e)
        {
            if (sender as Image != null && ReturnThrowNumber() < 3)
            {
                SelectedTerning(imgTerningSelected4, 4);
            }
        }

        private void Terning5Selected_MouseClick(object sender, MouseButtonEventArgs e)
        {
            if (sender as Image != null && ReturnThrowNumber() < 3)
            {
                SelectedTerning(imgTerningSelected5, 5);
            }
        }

        private void SelectedTerning(Image imgTerning, int terning)
        {
            if (imgTerning.Visibility == Visibility.Hidden)
            {
                AlleTerninger[terning - 1].IsHeld = true;
                imgTerning.Visibility = Visibility.Visible;
            }
            else if (imgTerning.Visibility == Visibility.Visible)
            {
                AlleTerninger[terning - 1].IsHeld = false;
                imgTerning.Visibility = Visibility.Hidden;
            }
        }

        private int ReturnThrowNumber()
        {
            return int.Parse(txbKastTilbage.Text.Split(": ").Last());
        }
    }
}
