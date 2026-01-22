using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
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

        private List<Terning> AlleTerninger
        {
            get
            {
                return FuncLayer.Spil.Terninger;
            }
        }
        private Image[] TerningImages { get; set; }
        private Image[] TerningSelection { get; }
        //public Terning terning { get; set; } = new Terning();

        private string _txbKastTilbage = "Rolled:";

        // Create a Random instance (ideally as a field, not inside a method for repeated use)
        private readonly Random rnd = new Random();

        bool ManualDeveloper_CheckDataIsTrue = true;
        private IUserControlManager UserControlManager { get; set; }
        public Terninger(FuncLayer FuncLayer, IUserControlManager userControlManager)
        {
            InitializeComponent();
            this.FuncLayer = FuncLayer;
            DataContext = this.FuncLayer;
            UserControlManager = userControlManager;
            TerningImages = new Image[] { imgTerning1, imgTerning2, imgTerning3, imgTerning4, imgTerning5 };
            TerningSelection = new Image[] { imgTerningSelected1, imgTerningSelected2, imgTerningSelected3, imgTerningSelected4, imgTerningSelected5 };
        }

        // Vis alle terningerne
        public void Init()
        {
            for (int i = 0; i < AlleTerninger.Count; i++)
            {
                ((Image)FindName($"imgTerningSelected{i + 1}")).SetValue(Image.SourceProperty, new BitmapImage(new Uri(SelectetTerning)));
                TerningImages[i].SetValue(Image.SourceProperty, BitmapImages[AlleTerninger[i].DiceValue - 1]);
                SelectedTerning(TerningSelection[i], i + 1);
            }
            txbKastTilbage.Text = $"{_txbKastTilbage} {Kastet}";
        }

        private async void KastTerninger_Click(object sender, RoutedEventArgs e)
        {
            int awaitTime = 35;
            int mellemKast = 0;

            if (FuncLayer.Spil.Kasted < 3)
            {
                // Throws dice's
                int[] RulleTerninger = GetRulNumber();
                KastTerningerne(RulleTerninger);
                if (false)
                {
                    // experiment
                    await Task.Delay(awaitTime * mellemKast);
                    CheckTerningValues();
                    FindRows();
                }
                // Finishing 'KastTertinger'
                FuncLayer.Spil.Kasted++;
                txbKastTilbage.Text = $"{_txbKastTilbage} {FuncLayer.Spil.Kasted}";
                dgSpillerScoreBoard.SelectedItem = null;
                DevMessage(false);
                if (FuncLayer.Spil.Kasted >= 3)
                {
                    btnKast.IsEnabled = false;
                }
                btnSaveGame.IsEnabled = true;
            }
            else
            {
                btnKast.IsEnabled = false;
                throw new UnauthorizedAccessException("Denied - Invalid action");
            }

            void FindRows()
            {
                bool cellFound = false;
                DataGridCellInfo previousDataGridCellInfo;
                string previousHeader;

                // Gets all needed data
                try
                {
                    previousDataGridCellInfo = dgSpillerScoreBoard.SelectedCells[0];
                    previousHeader = previousDataGridCellInfo.Column.Header.ToString();
                    cellFound = true;
                }
                catch (Exception ex)
                {
                    cellFound = false;
                }

                dgSpillerScoreBoard.SelectAllCells();
                List<DataGridCellInfo> cellList = dgSpillerScoreBoard.SelectedCells.ToList();
                dgSpillerScoreBoard.UnselectAllCells();
                for (int i = 0; i < cellList.Count; i++)
                {
                    try
                    {
                        string header = cellList[i].Column.Header.ToString();
                        int score = FuncLayer.RegnHeaderValue(header);
                        if (score > 0)
                        {
                            dgSpillerScoreBoard.CurrentCell = cellList[i];
                            dgSpillerScoreBoard.SelectedCells.Add(cellList[i]);

                            //dgSpillerScoreBoard.CurrentCell.

                            //dgSpillerScoreBoard.CurrentCell = cellList[i];
                            //dgSpillerScoreBoard.SelectedCells.Add(cellList[i]);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Do nothing
                    }
                }

                // Undoing selection
                dgSpillerScoreBoard.UnselectAllCells();
                if (cellFound)
                {
                    dgSpillerScoreBoard.CurrentCell = previousDataGridCellInfo;
                    dgSpillerScoreBoard.SelectedCells.Add(previousDataGridCellInfo);
                }

            }
            void CheckTerningValues()
            {
                for (int i = 0; i < AlleTerninger.Count; i++)
                {
                    if (AlleTerninger[i].DiceValue <= -1)
                    {
                        throw new InvalidDataException("A dice had invalid data");
                    }
                }
            }
            async void KastTerningerne(int[] RulleTerninger)
            {
                for (int i = 0; i < mellemKast; i++)
                {
                    for (int j = 0; j < RulleTerninger.Length; j++)
                    {
                        await Task.Delay(awaitTime);
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
                        if (mellemKast < RulleTerninger[i])
                        {
                            mellemKast = RulleTerninger[i];
                        }
                    }
                }
                return RulleTerninger;
            }
            void DevMessage(bool devMessage)
            {
                if (devMessage && ManualDeveloper_CheckDataIsTrue)
                {
                    string messageInput = string.Empty;
                    for (int i = 0; i < AlleTerninger.Count; i++)
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
            if (sender as Image != null && Kastet > 0 && !FuncLayer.PlayerHasWon)
            {
                SelectedTerning(imgTerningSelected1, 1);
            }
        }

        private void Terning2Selected_MouseClick(object sender, MouseButtonEventArgs e)
        {
            if (sender as Image != null && Kastet > 0 && !FuncLayer.PlayerHasWon)
            {
                SelectedTerning(imgTerningSelected2, 2);
            }
        }

        private void Terning3Selected_MouseClick(object sender, MouseButtonEventArgs e)
        {
            if (sender as Image != null && Kastet > 0 && !FuncLayer.PlayerHasWon)
            {
                SelectedTerning(imgTerningSelected3, 3);
            }
        }

        private void Terning4Selected_MouseClick(object sender, MouseButtonEventArgs e)
        {
            if (sender as Image != null && Kastet > 0 && !FuncLayer.PlayerHasWon)
            {
                SelectedTerning(imgTerningSelected4, 4);
            }
        }

        private void Terning5Selected_MouseClick(object sender, MouseButtonEventArgs e)
        {
            if (sender as Image != null && Kastet > 0 && !FuncLayer.PlayerHasWon)
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
                    //TerningUserControl.txbSpillerTur.Text = $"Turn: {funcLayer.SpillerListe.First().Navn}";

                    if (FuncLayer.PlayerHasWon)
                    {
                        btnKast.IsEnabled = false;
                        btnRegister.IsEnabled = false;
                        txbSpillerTur.Text = $"Player won: {FuncLayer.Spil.HighestScorePlayer.Navn}";
                        //txbSpillerTur.Text = FuncLayer.HighestScorePlayer.Navn;
                    }
                    else
                    {
                        dgSpillerScoreBoard.UnselectAllCells();
                        SpillerSpil spiller = FuncLayer.NæsteSpiller();
                        dgSpillerScoreBoard.SelectedItem = null;
                        ResetUi();
                    }
                    btnSaveGame.IsEnabled = true;
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
                int value = Kastet;
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

        private void btnStopGame_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult result = MessageBox.Show($"Do you want to stop game?\n{IsSaved()}", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    UserControlManager.StopGame();
                    dgSpillerScoreBoard.SelectedItem = null;
                    btnSaveGame.IsEnabled = false;
                    ResetUi();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error message");
            }
            string IsSaved()
            {
                if (!btnSaveGame.IsEnabled)
                {
                    return "- Game is saved";
                }
                else
                {
                    return "- Game is not saved";
                }
            }
        }

        private void ResetUi()
        {
            txbKastTilbage.Text = $"{_txbKastTilbage} 0";
            btnRegister.IsEnabled = false;
            btnKast.IsEnabled = true;
            for (int i = 0; i < AlleTerninger.Count; i++)
            {
                SelectedTerning(((Image)FindName($"imgTerningSelected{i + 1}")), i + 1);
                //AlleTerninger[i].IsHeld = false;
                //((Image)FindName($"imgTerningSelected{i + 1}")).Visibility = Visibility.Hidden;
            }
        }

        private void btnSaveGame_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult result = MessageBox.Show("Do you want to save game?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    FuncLayer.GemSpil();
                    btnSaveGame.IsEnabled = false;
                    MessageBox.Show("Game saved successfully.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error message");
            }
        }

        private void ShowTerningSelection(int terning)
        {
            if (AlleTerninger[terning - 1].IsHeld)
            {
                TerningSelection[terning - 1].Visibility = Visibility.Visible;
            }
        }

        private void SelectedTerning(Image imgTerning, int terning)
        {
            if (!AlleTerninger[terning - 1].IsHeld)
            {
                AlleTerninger[terning - 1].IsHeld = true;
                imgTerning.Visibility = Visibility.Visible;
            }
            else if (AlleTerninger[terning - 1].IsHeld)
            {
                AlleTerninger[terning - 1].IsHeld = false;
                imgTerning.Visibility = Visibility.Hidden;
            }
        }

        private int Kastet
        {
            get
            {
                return FuncLayer.Spil.Kasted;
            }
        }
    }
}
