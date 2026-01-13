using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Yatzy.YatzyDbContext;

namespace Yatzy
{
    /// <summary>
    /// Interaction logic for Terninger.xaml
    /// </summary>
    /// 

    public partial class Terninger : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private FuncLayer FuncLayer;

        private static string[] TerningSides = Directory.GetFiles($"{Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName}\\Terning\\6_Sides");
        private static string SelectetTerning = $"{Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName}\\Terning\\00bfff.png";
        private BitmapImage[] BitmapImages { get; set; } = new BitmapImage[6] {
            new BitmapImage(new Uri(TerningSides[0])),
            new BitmapImage(new Uri(TerningSides[1])),
            new BitmapImage(new Uri(TerningSides[2])),
            new BitmapImage(new Uri(TerningSides[3])),
            new BitmapImage(new Uri(TerningSides[4])),
            new BitmapImage(new Uri(TerningSides[5]))
        };

        private Terning[] AlleTerninger { get; set; }
        private Image[] TerningImages { get; set; }
        //public Terning terning { get; set; } = new Terning();

        // Create a Random instance (ideally as a field, not inside a method for repeated use)
        private readonly Random rnd = new Random();

        bool ManualDeveloper_CheckDataIsTrue = true;
        public Terninger(FuncLayer FuncLayer)
        {
            InitializeComponent();
            this.FuncLayer = FuncLayer;
            DataContext = this.FuncLayer;
            TerningImages = new Image[]
            {
                imgTerning1, imgTerning2, imgTerning3, imgTerning4, imgTerning5
            };
            AlleTerninger = new Terning[TerningImages.Length];
            for (int i = 0; i < AlleTerninger.Length; i++)
            {
                AlleTerninger[i] = new Terning();
                ((Image)FindName($"imgTerningSelected{i + 1}")).SetValue(Image.SourceProperty, new BitmapImage(new Uri(SelectetTerning)));
                TerningImages[i].SetValue(Image.SourceProperty, BitmapImages[rnd.Next(0, TerningSides.Length)]);
            }
        }

        private void KastTerninger_Click(object sender, RoutedEventArgs e)
        {
            int kastet = 0;
            int ThrowedNumber = ReturnThrowNumber() + 1;

            if (ThrowedNumber < 4)
            {
                // Throws dice's
                int[] RulleTerninger = GetRulNumber();
                KastTerningerne(RulleTerninger);

                // Finishing 'KastTertinger'
                txbKastTilbage.Text = $"Rulls: {ThrowedNumber}";
                DevMessage();
                if (ThrowedNumber >= 3)
                {
                    btnKast.IsEnabled = false;
                }

                if (false)
                {
                    // experiment
                    FindRows();
                }
            }
            else
            {
                btnKast.IsEnabled = false;
                throw new UnauthorizedAccessException("Denied - Invalid action");
            }

            void FindRows()
            {
                int spillerIndex = FuncLayer.SpillerListe.IndexOf(FuncLayer.SpillerListe.First(spiller => txbSpillerTur.Text == spiller.Navn));
                Spiller spiller = FuncLayer.SpillerListe[spillerIndex];
                int[] diceValues = new int[AlleTerninger.Length];

                FillDiceValues();

                if (spiller.Enere != null && diceValues[0] > 0)
                {
                    //dgSpillerScoreBoard.Columns.
                    dgSpillerScoreBoard.Columns.First(Column => "Enere" == Column.Header.ToString());
                }

                void FillDiceValues()
                {
                    for (int i = 0; i < diceValues.Length; i++)
                    {
                        diceValues[AlleTerninger[i].DiceValue] += 1;
                    }
                }
            }
            async void KastTerningerne(int[] RulleTerninger)
            {
                for (int i = 0; i < kastet; i++)
                {
                    for (int j = 0; j < RulleTerninger.Length; j++)
                    {
                        await Task.Delay(35);
                        if (RulleTerninger[j] > 0)
                        {
                            ChanceImange(j);
                            RulleTerninger[j] -= 1;
                        }
                    }
                }
            }
            void ChanceImange(int index)
            {
                // Thorws dice 'dummyIndex' for value
                AlleTerninger[index].DiceValue = rnd.Next(0, TerningSides.Count()) + 1;

                // Sets the new dice image in array 'AlleTerninger' at value 'dummyIndex'
                AlleTerninger[index].Terningside.SetValue(Image.SourceProperty, BitmapImages[AlleTerninger[index].DiceValue - 1]);

                // Sets the new dice image in UI 'Image'
                TerningImages[index].SetValue(Image.SourceProperty, BitmapImages[AlleTerninger[index].DiceValue - 1]);
            }
            int[] GetRulNumber()
            {
                int[] RulleTerninger = { 0, 0, 0, 0, 0 };
                for (int i = 0; i < RulleTerninger.Count(); i++)
                {
                    if (!AlleTerninger[i].IsHeld)
                    {
                        RulleTerninger[i] = rnd.Next(5, 25);
                        if (kastet < RulleTerninger[i])
                        {
                            kastet = RulleTerninger[i];
                        }
                    }
                }
                return RulleTerninger;
            }
            void DevMessage()
            {
                if (false && ManualDeveloper_CheckDataIsTrue)
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
        }

        private void Terning1Selected_MouseClick(object sender, MouseButtonEventArgs e)
        {
            if (sender as Image != null && ReturnThrowNumber() > 0)
            {
                SelectedTerning(imgTerningSelected1, 1);
            }
        }

        private void Terning2Selected_MouseClick(object sender, MouseButtonEventArgs e)
        {
            if (sender as Image != null && ReturnThrowNumber() > 0)
            {
                SelectedTerning(imgTerningSelected2, 2);
            }
        }

        private void Terning3Selected_MouseClick(object sender, MouseButtonEventArgs e)
        {
            if (sender as Image != null && ReturnThrowNumber() > 0)
            {
                SelectedTerning(imgTerningSelected3, 3);
            }
        }

        private void Terning4Selected_MouseClick(object sender, MouseButtonEventArgs e)
        {
            if (sender as Image != null && ReturnThrowNumber() > 0)
            {
                SelectedTerning(imgTerningSelected4, 4);
            }
        }

        private void Terning5Selected_MouseClick(object sender, MouseButtonEventArgs e)
        {
            if (sender as Image != null && ReturnThrowNumber() > 0)
            {
                SelectedTerning(imgTerningSelected5, 5);
            }
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgSpillerScoreBoard.SelectedCells.Count > 0)
                {
                    DataGridCellInfo cell = dgSpillerScoreBoard.SelectedCells[0];

                    int Score = FuncLayer.Registrer(cell, AlleTerninger);
                    //TerningUserControl.txbSpillerTur.Text = $"Turn: {FuncLayer.SpillerListe.First().Navn}";

                    Spiller spiller = FuncLayer.NæsteSpiller();
                    txbKastTilbage.Text = "Rulls: 0";
                    btnKast.IsEnabled = true;
                    btnRegister.IsEnabled = false;
                    AlleTerninger[0].IsHeld = false;
                    imgTerningSelected1.Visibility = Visibility.Hidden;
                    AlleTerninger[1].IsHeld = false;
                    imgTerningSelected2.Visibility = Visibility.Hidden;
                    AlleTerninger[2].IsHeld = false;
                    imgTerningSelected3.Visibility = Visibility.Hidden;
                    AlleTerninger[3].IsHeld = false;
                    imgTerningSelected4.Visibility = Visibility.Hidden;
                    AlleTerninger[4].IsHeld = false;
                    imgTerningSelected5.Visibility = Visibility.Hidden;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error message");
            }
        }

        private void dgSpillerScoreBoard_SelectionChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            try
            {
                int value = ReturnThrowNumber();
                if (dgSpillerScoreBoard.CurrentColumn != null && value > 0)
                {
                    btnRegister.IsEnabled = true;
                }
                else
                {
                    btnRegister.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error message");
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
