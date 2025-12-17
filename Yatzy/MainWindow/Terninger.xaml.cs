using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
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
//using static System.Net.Mime.MediaTypeNames;

namespace Yatzy
{
    /// <summary>
    /// Interaction logic for Terninger.xaml
    /// </summary>
    /// 

    public class Terning : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public Terning() { }
        public Terning(Image newImage)
        {
            Terningside = newImage;
        }

        public int DiceValue { get; set; } = -1;
        public Image Terningside { get; set; } = new Image();
        public bool IsHeld { get; set; } = false;
    }

    public partial class Terninger : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        FuncLayer FuncLayer;

        public Terning[] AlleTerninger { get; set; } = new Terning[5];
        public Terning terning { get; set; } = new Terning();

        // Create a Random instance (ideally as a field, not inside a method for repeated use)
        private readonly Random rnd = new Random();

        public string[] TerningSides = Directory.GetFiles($"{Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName}\\Terning\\6_Sides");

        public static string SelectetTerning = $"{Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName}\\Terning\\00bfff.png";

        Image image { get; set; } = new Image();

        bool ManualDeveloper_CheckDataIsTrue = true;
        public Terninger(FuncLayer FuncLayer)
        {
            InitializeComponent();
            //this.FuncLayer = FuncLayer;
            //DataContext = this.FuncLayer;
            DataContext = this;
            for (int i = 0; i < AlleTerninger.Length; i++)
            {
                AlleTerninger[i] = new Terning();
            }
            image.SetValue(Image.SourceProperty, new BitmapImage(new Uri(SelectetTerning)));
            KastTertinger();
        }

        private async void KastTerninger_Click(object sender, RoutedEventArgs e)
        {
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
            // Image.DpiChanged="{Binding AlleTerninger[0].Terningside}"
            for (int i = 0; i < AlleTerninger.Length; i++)
            {
                // Throw dice if not held
                if (!AlleTerninger[i].IsHeld) // < Error Message
                {
                    // Thorws dice 'i' for value
                    AlleTerninger[i].DiceValue = rnd.Next(0, TerningSides.Count()) + 1;

                    // Sets the new dice Image in class 'Terning'
                    AlleTerninger[i].Terningside.SetValue(Image.SourceProperty, new BitmapImage(new Uri(TerningSides[AlleTerninger[i].DiceValue - 1])));
                    /*(Image)FindName($"imgTerning{AlleTerninger[i].DiceValue}")*/

                    // Sets the new dice Image in UI 'Image'
                    //((Image)FindName($"imgTerning{AlleTerninger[i].DiceValue}")).SetValue(Image.SourceProperty, new BitmapImage(new Uri(TerningSides[AlleTerninger[i].DiceValue - 1])));
                }
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
                imgTerning.Visibility = Visibility.Visible;
            }
            else if (imgTerning.Visibility == Visibility.Visible)
            {
                imgTerning.Visibility = Visibility.Hidden;
            }
        }

        private void imgTerning1_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {

        }
    }
}
