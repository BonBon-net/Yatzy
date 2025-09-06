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
using Yatzy.YatzyDbContext;

namespace Yatzy
{
    /// <summary>
    /// Interaction logic for Terninger.xaml
    /// </summary>
    public partial class Terninger : UserControl
    {
        Terninger TerningUserControl = new Terninger();
        FuncLayer FuncLayer = new FuncLayer();
        Model Model { get; set; } = new Model();

        // Create a Random instance (ideally as a field, not inside a method for repeated use)
        private readonly Random rnd = new Random();
        public string[] TerningSides = new string[] {
            "C:\\Users\\66186\\source\\repos\\Yatzy\\Yatzy\\Terning\\1.jpg", "C:\\Users\\66186\\source\\repos\\Yatzy\\Yatzy\\Terning\\2.jpg",
            "C:\\Users\\66186\\source\\repos\\Yatzy\\Yatzy\\Terning\\3.jpg", "C:\\Users\\66186\\source\\repos\\Yatzy\\Yatzy\\Terning\\4.jpg",
            "C:\\Users\\66186\\source\\repos\\Yatzy\\Yatzy\\Terning\\5.jpg", "C:\\Users\\66186\\source\\repos\\Yatzy\\Yatzy\\Terning\\6.jpg"
        };
        private int[] TerningSloget = new int[] { 0, 0, 0, 0, 0 };
        private int[] RulleTerninger = new int[] { 0, 0, 0, 0, 0 };
        private int setTerningImage(Image imgTerning)
        {
            int rndIndex = rnd.Next(0, TerningSides.Count());
            imgTerning.SetValue(Image.SourceProperty, new BitmapImage(new Uri(TerningSides[rndIndex])));
            TerningSloget[(int)char.GetNumericValue(imgTerning.Name.ToCharArray().Last()) - 1] = rndIndex + 1;
            return rndIndex + 1;
        }

        bool CheckDataIsTrue = true;
        public Terninger()
        {
            InitializeComponent();
            DataContext = FuncLayer.DefaultDataContent();
            for (int i = 0; i < TerningSloget.Count(); i++)
            {
                setTerningImage((Image)FindName($"imgTerning{i + 1}"));
            }
        }

        private async void KastTerninger_Click(object sender, RoutedEventArgs e)
        {
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
                        setTerningImage((Image)FindName($"imgTerning{j + 1}"));
                        RulleTerninger[j] -= 1;
                    }
                }
            }
            if (CheckDataIsTrue)
            {
                MessageBox.Show(
                    TerningSloget[0].ToString() + "-" +
                    TerningSloget[1].ToString() + "-" +
                    TerningSloget[2].ToString() + "-" +
                    TerningSloget[3].ToString() + "-" +
                    TerningSloget[4].ToString());
            }
        }
    }
}
