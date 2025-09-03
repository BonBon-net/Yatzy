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

namespace Yatzy
{
    /// <summary>
    /// Interaction logic for Terninger.xaml
    /// </summary>
    public partial class Terninger : UserControl
    {
        // Create a Random instance (ideally as a field, not inside a method for repeated use)
        private readonly Random rnd = new Random();
        public string[] TerningSides = new string[] {
            "C:\\Users\\66186\\source\\repos\\Yatzy\\Yatzy\\Terning\\1.jpg", "C:\\Users\\66186\\source\\repos\\Yatzy\\Yatzy\\Terning\\2.jpg",
            "C:\\Users\\66186\\source\\repos\\Yatzy\\Yatzy\\Terning\\3.jpg", "C:\\Users\\66186\\source\\repos\\Yatzy\\Yatzy\\Terning\\4.jpg",
            "C:\\Users\\66186\\source\\repos\\Yatzy\\Yatzy\\Terning\\5.jpg", "C:\\Users\\66186\\source\\repos\\Yatzy\\Yatzy\\Terning\\6.jpg"
        };
        private int[] TerningKast = new int[] { 0, 0, 0, 0, 0 };
        private int setTerningImage(Image imgTerning)
        {
            int RND = rnd.Next(0, 6);
            imgTerning.SetValue(Image.SourceProperty, new BitmapImage(new Uri(TerningSides[RND])));
            return RND + 1;
        }

        public Terninger()
        {
            InitializeComponent();
            setTerningImage(imgTerning1);
            setTerningImage(imgTerning2);
            setTerningImage(imgTerning3);
            setTerningImage(imgTerning4);
            setTerningImage(imgTerning5);
        }

        private async void KastTerninger_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 20; i++)
            {
                //Thread.SpinWait(1000000);
                //Thread.Sleep(5000);
                await Task.Delay(155);
                setTerningImage(imgTerning1);
                setTerningImage(imgTerning2);
                setTerningImage(imgTerning3);
                setTerningImage(imgTerning4);
                setTerningImage(imgTerning5);
            }
        }
    }
}
