using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static System.Formats.Asn1.AsnWriter;

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
        private string _txbSpillerTur = "Tur:";

        // Create a Random instance (ideally as a field, not inside a method for repeated use)
        private readonly Random rnd = new Random();

        // Styles for DataGrid columns
        Style SelectedColumnStyle = new Style(typeof(DataGridCell));
        Style UnselectedColumnStyle = new Style(typeof(DataGridCell));
        Style ScoreAbilityColumnStyle = new Style(typeof(DataGridCell));

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
            SetDefaltColumnStyle();
            ResetScoreBoardStyles();
        }

        // Vis alle terningerne
        public void Init()
        {
            ResetScoreBoardStyles();
            for (int i = 0; i < AlleTerninger.Count; i++)
            {
                ((Image)FindName($"imgTerningSelected{i + 1}")).SetValue(Image.SourceProperty, new BitmapImage(new Uri(SelectetTerning)));
                //TerningImages[i].SetValue(Image.SourceProperty, BitmapImages[AlleTerninger[i].DiceValue - 1]);
                ((Image)FindName($"imgTerning{i + 1}")).SetValue(Image.SourceProperty, new BitmapImage(new Uri(TerningSides[FuncLayer.Spil.Terninger[i].DiceValue - 1])));
                if (FuncLayer.Spil.Kasted > 0 && AlleTerninger[i].IsHeld)
                {
                    //SelectedTerning(TerningSelection[i], i + 1);
                    AlleTerninger[i].IsHeld = true;
                    ((Image)FindName($"imgTerningSelected{i + 1}")).Visibility = Visibility.Visible;
                }
            }

            txbKastTilbage.Text = $"{_txbKastTilbage} {Kastet}";
            txbSpillerTur.Text = $"{_txbSpillerTur} {FuncLayer.SpillerTur.Navn}";

            if (FuncLayer.Spillere.Count == FuncLayer.Spil.NullPlayerCount)
            {
                SpillerSpil? spiller = FuncLayer.Spil.Spillere.FirstOrDefault(Spiller => Spiller.HasPlayerNullScoreBoardValue() == true);
                if (spiller == null)
                {
                    btnKast.IsEnabled = false;
                    btnRegister.IsEnabled = false;
                    btnSaveGame.IsEnabled = false;
                    txbSpillerTur.Text = $"Player Won: {FuncLayer.Spil.HighestScorePlayer!.Navn}";
                }
            }

            if (FuncLayer.Spil.Kasted > 0)
            {
                FindRows();
            }
        }

        private async void KastTerninger_Click(object sender, RoutedEventArgs e)
        {
            int awaitTime = 35;
            int mellemKast = 0;

            if (FuncLayer.Spil.Kasted < 3)
            {
                // Throws dice's
                ResetScoreBoardStyles();
                int[] RulleTerninger = GetRulNumber();
                KastTerningerne(RulleTerninger);
                await Task.Delay(awaitTime * mellemKast * 6);
                FindRows();
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
            if (sender as Image != null && Kastet > 0 && FuncLayer.Spil.Spillere.Count > FuncLayer.Spil.NullPlayerCount)
            {
                SelectedTerning(imgTerningSelected1, 1);
            }
        }

        private void Terning2Selected_MouseClick(object sender, MouseButtonEventArgs e)
        {
            if (sender as Image != null && Kastet > 0 && FuncLayer.Spil.Spillere.Count > FuncLayer.Spil.NullPlayerCount)
            {
                SelectedTerning(imgTerningSelected2, 2);
            }
        }

        private void Terning3Selected_MouseClick(object sender, MouseButtonEventArgs e)
        {
            if (sender as Image != null && Kastet > 0 && FuncLayer.Spil.Spillere.Count > FuncLayer.Spil.NullPlayerCount)
            {
                SelectedTerning(imgTerningSelected3, 3);
            }
        }

        private void Terning4Selected_MouseClick(object sender, MouseButtonEventArgs e)
        {
            if (sender as Image != null && Kastet > 0 && FuncLayer.Spil.Spillere.Count > FuncLayer.Spil.NullPlayerCount)
            {
                SelectedTerning(imgTerningSelected4, 4);
            }
        }

        private void Terning5Selected_MouseClick(object sender, MouseButtonEventArgs e)
        {
            if (sender as Image != null && Kastet > 0 && FuncLayer.Spil.Spillere.Count > FuncLayer.Spil.NullPlayerCount)
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
                    string header = cell.Column.Header.ToString()!;
                    PropertyInfo? property = FuncLayer.SpillerTur.ScoreBoard.GetType().GetProperty(FuncLayer.TrimString(header));
                    if (property == null)
                    {
                        throw new NullReferenceException("Column not found");
                    }

                    ResetScoreBoardStyles();
                    if ((header == "Enere" && FuncLayer.SpillerTur.ScoreBoard.Enere != null) || (header == "Toere" && FuncLayer.SpillerTur.ScoreBoard.Toere != null) || (header == "Treere" && FuncLayer.SpillerTur.ScoreBoard.Treere != null) || (header == "Firere" && FuncLayer.SpillerTur.ScoreBoard.Firere != null) || (header == "Femmere" && FuncLayer.SpillerTur.ScoreBoard.Femmere != null) || (header == "Seksere" && FuncLayer.SpillerTur.ScoreBoard.Seksere != null) ||
                    (header == "EtPar" && FuncLayer.SpillerTur.ScoreBoard.EtPar != null) || (header == "ToPar" && FuncLayer.SpillerTur.ScoreBoard.ToPar != null) || (header == "TreEns" && FuncLayer.SpillerTur.ScoreBoard.TreEns != null) || (header == "FireEns" && FuncLayer.SpillerTur.ScoreBoard.Firere != null) || (header == "LilleStraight" && FuncLayer.SpillerTur.ScoreBoard.LilleStraight != null) ||
                    (header == "StorStraight" && FuncLayer.SpillerTur.ScoreBoard.StorStraight != null) || (header == "Hus" && FuncLayer.SpillerTur.ScoreBoard.Hus != null) || (header == "Chance" && FuncLayer.SpillerTur.ScoreBoard.Chance != null) || (header == "Yatzy" && FuncLayer.SpillerTur.ScoreBoard.Yatzy != null))
                    {
                        FindRows();
                        MessageBox.Show("Column allready taken");
                        return;
                    }
                    else
                    {
                        FuncLayer.Registrer(cell, FuncLayer.Spil.Terninger);
                        if (FuncLayer.Spil.Spillere.Count == FuncLayer.Spil.NullPlayerCount)
                        {
                            btnKast.IsEnabled = false;
                            btnRegister.IsEnabled = false;
                            btnSaveGame.IsEnabled = false;
                            txbSpillerTur.Text = $"Player won: {FuncLayer.Spil.HighestScorePlayer!.Navn}";
                        }
                        else
                        {
                            bool HasNull = false;
                            if (FuncLayer.Spil.Spillere.Count != FuncLayer.Spil.NullPlayerCount)
                            {
                                while (!HasNull)
                                {
                                    SpillerSpil spiller = FuncLayer.NæsteSpiller();
                                    dgSpillerScoreBoard.UnselectAllCells();
                                    dgSpillerScoreBoard.SelectedItem = null;
                                    FuncLayer.Spil.Kasted = 0;
                                    FuncLayer.Spil.IsStarted = true;
                                    ResetUi();
                                    HasNull = spiller.HasPlayerNullScoreBoardValue();
                                    txbSpillerTur.Text = $"{_txbSpillerTur} {FuncLayer.SpillerTur.Navn}";
                                }
                            }
                        }
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
                    ResetScoreBoardStyles();
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
            btnRegister.IsEnabled = false;
            btnKast.IsEnabled = true;
            txbKastTilbage.Text = $"{_txbKastTilbage} 0";
            for (int i = 0; i < AlleTerninger.Count; i++)
            {
                //SelectedTerning(((Image)FindName($"imgTerningSelected{i + 1}")), i + 1);
                AlleTerninger[i].IsHeld = false;
                ((Image)FindName($"imgTerningSelected{i + 1}")).Visibility = Visibility.Hidden;
            }
        }

        private void btnSaveGame_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ResetScoreBoardStyles();
                MessageBoxResult result = MessageBox.Show("Do you want to save game?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    btnSaveGame.IsEnabled = false;
                    FuncLayer.GemSpil();
                    MessageBox.Show("Game saved successfully.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                FindRows();
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
            btnSaveGame.IsEnabled = true;
        }

        private void ResetScoreBoardStyles()
        {
            if (FuncLayer.SpillerTur == null)
            {
                return;
            }
            // Set all columns to unselected style
            for (int i = 0; i < dgSpillerScoreBoard.Columns.Count; i++)
            {
                if (i != 0 && i != 7 && i != 8 && i != 19)
                {
                    DataGridCell cell = VisualTreeHelpers.GetCell(dgSpillerScoreBoard, FuncLayer.Spil.SpillerTurIndex, i);
                    //DataGridCell cell = VisualTreeHelpers.GetCell(dgSpillerScoreBoard, FuncLayer.Spil.SpillerTurIndex, i + 3);
                    //cell.Style = ScoreAbilityColumnStyle;

                    if (cell != null && cell.Style == ScoreAbilityColumnStyle)
                    {
                        string Header = dgSpillerScoreBoard.Columns[i].Header.ToString()!;
                        Header = FuncLayer.TrimString(Header);
                        if (Header == "Enere" || Header == "Toere" || Header == "Treere" || Header == "Firere" || Header == "Femmere" || Header == "Seksere" ||
                        Header == "EtPar" || Header == "ToPar" || Header == "TreEns" || Header == "FireEns" || Header == "LilleStraight" ||
                        Header == "StorStraight" || Header == "Hus" || Header == "Chance" || Header == "Yatzy")
                        {
                            PropertyInfo? property = FuncLayer.SpillerTur.ScoreBoard.GetType().GetProperty(Header);
                            if (property == null)
                            {
                                throw new NullReferenceException("Property not found in SpillerTur");
                            }
                            int? scoreValue = (int?)property.GetValue(FuncLayer.SpillerTur.ScoreBoard);
                            if (scoreValue != null)
                            {
                                //FuncLayer.SpillerTur.ScoreBoard.GetType().GetProperty(Header)!.SetValue(FuncLayer.SpillerTur.ScoreBoard, null);
                                // set score to null
                                if (Header == "Enere")
                                {
                                    FuncLayer.SpillerTur.ScoreBoard.Enere = null;
                                    dgSpillerScoreBoard.Columns[1].CellStyle = null;
                                    dgSpillerScoreBoard.Columns[1].CellStyle = UnselectedColumnStyle;
                                }
                                else if (Header == "Toere")
                                {
                                    FuncLayer.SpillerTur.ScoreBoard.Toere = null;
                                    dgSpillerScoreBoard.Columns[2].CellStyle = null;
                                    dgSpillerScoreBoard.Columns[2].CellStyle = UnselectedColumnStyle;
                                }
                                else if (Header == "Treere")
                                {
                                    FuncLayer.SpillerTur.ScoreBoard.Treere = null;
                                    dgSpillerScoreBoard.Columns[3].CellStyle = null;
                                    dgSpillerScoreBoard.Columns[3].CellStyle = UnselectedColumnStyle;
                                }
                                else if (Header == "Firere")
                                {
                                    FuncLayer.SpillerTur.ScoreBoard.Firere = null;
                                    dgSpillerScoreBoard.Columns[4].CellStyle = null;
                                    dgSpillerScoreBoard.Columns[4].CellStyle = UnselectedColumnStyle;
                                }
                                else if (Header == "Femmere")
                                {
                                    FuncLayer.SpillerTur.ScoreBoard.Femmere = null;
                                    dgSpillerScoreBoard.Columns[5].CellStyle = null;
                                    dgSpillerScoreBoard.Columns[5].CellStyle = UnselectedColumnStyle;
                                }
                                else if (Header == "Seksere")
                                {
                                    FuncLayer.SpillerTur.ScoreBoard.Seksere = null;
                                    dgSpillerScoreBoard.Columns[6].CellStyle = null;
                                    dgSpillerScoreBoard.Columns[6].CellStyle = UnselectedColumnStyle;
                                }
                                else if (Header == "EtPar")
                                {
                                    FuncLayer.SpillerTur.ScoreBoard.EtPar = null;
                                    dgSpillerScoreBoard.Columns[9].CellStyle = null;
                                    dgSpillerScoreBoard.Columns[9].CellStyle = UnselectedColumnStyle;
                                }
                                else if (Header == "ToPar")
                                {
                                    FuncLayer.SpillerTur.ScoreBoard.ToPar = null;
                                    dgSpillerScoreBoard.Columns[10].CellStyle = null;
                                    dgSpillerScoreBoard.Columns[10].CellStyle = UnselectedColumnStyle;
                                }
                                else if (Header == "TreEns")
                                {
                                    FuncLayer.SpillerTur.ScoreBoard.TreEns = null;
                                    dgSpillerScoreBoard.Columns[11].CellStyle = null;
                                    dgSpillerScoreBoard.Columns[11].CellStyle = UnselectedColumnStyle;
                                }
                                else if (Header == "FireEns")
                                {
                                    FuncLayer.SpillerTur.ScoreBoard.FireEns = null;
                                    dgSpillerScoreBoard.Columns[12].CellStyle = null;
                                    dgSpillerScoreBoard.Columns[12].CellStyle = UnselectedColumnStyle;
                                }
                                else if (Header == "LilleStraight")
                                {
                                    FuncLayer.SpillerTur.ScoreBoard.LilleStraight = null;
                                    dgSpillerScoreBoard.Columns[13].CellStyle = null;
                                    dgSpillerScoreBoard.Columns[13].CellStyle = UnselectedColumnStyle;
                                }
                                else if (Header == "StorStraight")
                                {
                                    FuncLayer.SpillerTur.ScoreBoard.StorStraight = null;
                                    dgSpillerScoreBoard.Columns[14].CellStyle = null;
                                    dgSpillerScoreBoard.Columns[14].CellStyle = UnselectedColumnStyle;
                                }
                                else if (Header == "Hus")
                                {
                                    FuncLayer.SpillerTur.ScoreBoard.Hus = null;
                                    dgSpillerScoreBoard.Columns[15].CellStyle = null;
                                    dgSpillerScoreBoard.Columns[15].CellStyle = UnselectedColumnStyle;
                                }
                                else if (Header == "Chance")
                                {
                                    FuncLayer.SpillerTur.ScoreBoard.Chance = null;
                                    dgSpillerScoreBoard.Columns[16].CellStyle = null;
                                    dgSpillerScoreBoard.Columns[16].CellStyle = UnselectedColumnStyle;
                                }
                                else if (Header == "Yatzy")
                                {
                                    FuncLayer.SpillerTur.ScoreBoard.Yatzy = null;
                                    dgSpillerScoreBoard.Columns[17].CellStyle = null;
                                    dgSpillerScoreBoard.Columns[17].CellStyle = UnselectedColumnStyle;
                                }
                            }
                        }
                    }
                }
            }

            // Refresh the DataGrid to apply style changes
            dgSpillerScoreBoard.Items.Refresh();
        }

        private int Kastet
        {
            get
            {
                return FuncLayer.Spil.Kasted;
            }
        }

        private void SetDefaltColumnStyle()
        {
            // Style for selected column
            // SelectedColumnStyle = new Style(typeof(DataGridCell));
            SelectedColumnStyle.Setters.Add(new Setter(DataGridCell.BackgroundProperty, Brushes.Green));
            SelectedColumnStyle.Setters.Add(new Setter(DataGridCell.ForegroundProperty, Brushes.Black));
            SelectedColumnStyle.Setters.Add(new Setter(DataGridCell.LayoutTransformProperty, new RotateTransform(270)));

            // Style for unselected column
            // UnselectedColumnStyle = new Style(typeof(DataGridCell));
            // UnselectedColumnStyle.Setters.Add(new Setter(DataGridCell.BackgroundProperty, Brushes.White));
            // UnselectedColumnStyle.Setters.Add(new Setter(DataGridCell.ForegroundProperty, Brushes.Black));
            UnselectedColumnStyle.Setters.Add(new Setter(DataGridCell.LayoutTransformProperty, new RotateTransform(-90)));

            // Style for score ability column
            // ScoreAbilityColumnStyle
            // headerStyle
            // DataGridCell
            // ScoreAbilityColumnStyle = new Style(typeof(DataGridCell));
            //ScoreAbilityColumnStyle.Setters.Add(new Setter(DataGridCell.BackgroundProperty, Brushes.Green));
            //ScoreAbilityColumnStyle.Setters.Add(new Setter(DataGridCell.ForegroundProperty, Brushes.Black));
            //ScoreAbilityColumnStyle.Setters.Add(new Setter(DataGridCell.BackgroundProperty, Brushes.Green));
            //ScoreAbilityColumnStyle.Setters.Add(new Setter(DataGridCell.ForegroundProperty, Brushes.Black));
            //ScoreAbilityColumnStyle.Setters.Add(new Setter(Control.ForegroundProperty, new RotateTransform(270)));
            //ScoreAbilityColumnStyle.Setters.Add(new Setter(DataGridCell.LayoutTransformProperty, new RotateTransform(270)));
            //ScoreAbilityColumnStyle.Setters.Add(new Setter(Control.ForegroundProperty, new RotateTransform(270)));
            //ScoreAbilityColumnStyle.Setters.Add(new Setter(Control.ForegroundProperty, Brushes.White));

            // 1. Create a Style for the Column Header
            //Style headerStyle = new Style(typeof());
            //ScoreAbilityColumnStyle.Setters.Add(new Setter(Control.BackgroundProperty, Brushes.DarkRed));
            //ScoreAbilityColumnStyle.Setters.Add(new Setter(Control.ForegroundProperty, Brushes.White));
            //ScoreAbilityColumnStyle.Setters.Add(new Setter(Control.FontWeightProperty, FontWeights.Bold));

            ScoreAbilityColumnStyle = new Style(typeof(DataGridCell));
            ScoreAbilityColumnStyle.Setters.Add(new Setter(DataGridCell.BackgroundProperty, Brushes.Green));
            ScoreAbilityColumnStyle.Setters.Add(new Setter(DataGridCell.ForegroundProperty, Brushes.Black));
            ScoreAbilityColumnStyle.Setters.Add(new Setter(DataGridCell.LayoutTransformProperty, new RotateTransform(270)));
            ScoreAbilityColumnStyle.Setters.Add(new Setter(Control.FontWeightProperty, FontWeights.Bold));
        }

        private void FindRows()
        {
            string[] Headers = new string[] {
                    "Enere",
                    "Toere",
                    "Treere",
                    "Firere",
                    "Femmere",
                    "Seksere",
                    "EtPar",
                    "ToPar",
                    "TreEns",
                    "FireEns",
                    "LilleStraight",
                    "StorStraight",
                    "Hus",
                    "Chance",
                    "Yatzy"
            };

            for (int i = 0; i < Headers.Length; i++)
            {
                PropertyInfo? property = FuncLayer.SpillerTur.ScoreBoard.GetType().GetProperty(Headers[i]);
                if (property == null)
                {
                    throw new NullReferenceException("Property not found in SpillerTur");
                }
                int? scoreValue = (int?)property.GetValue(FuncLayer.SpillerTur.ScoreBoard);
                if (scoreValue == null)
                {
                    int calculatedScore = FuncLayer.RegnHeaderValue(Headers[i]);
                    if (calculatedScore > 0)
                    {
                        FuncLayer.SpillerTur.ScoreBoard.GetType().GetProperty(Headers[i])!.SetValue(FuncLayer.SpillerTur.ScoreBoard, calculatedScore);
                        //dgSpillerScoreBoard.Items.Refresh();

                        DataGridCell cell = VisualTreeHelpers.GetCell(dgSpillerScoreBoard, FuncLayer.Spil.SpillerTurIndex, i + 1);

                        if (i > 5)
                        {
                            cell = VisualTreeHelpers.GetCell(dgSpillerScoreBoard, FuncLayer.Spil.SpillerTurIndex, i + 3);
                        }
                        if (cell != null)
                        {
                            cell.Style = ScoreAbilityColumnStyle;
                        }
                    }
                }
            }
        }
    }

    internal static class VisualTreeHelpers
    {
        public static DataGridCell GetCell(DataGrid grid, int rowIndex = 0, int columnIndex = 0)
        {
            DataGridRow row = GetRow(grid, rowIndex);

            if (row == null) return null;

            var presenter = FindVisualChild<DataGridCellsPresenter>(row);
            if (presenter == null) return null;

            var cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(columnIndex);
            if (cell != null) return cell;

            // now try to bring into view and retreive the cell
            grid.ScrollIntoView(row, grid.Columns[columnIndex]);
            cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(columnIndex);

            return cell;
        }

        // Get row by index
        public static DataGridRow? GetRow(DataGrid grid, int index)
        {
            if (index < 0 || index >= grid.Items.Count) return null;

            var row = (DataGridRow?)grid.ItemContainerGenerator.ContainerFromIndex(index);
            if (row != null) return row;

            // Force generation for virtualized rows
            grid.UpdateLayout();
            grid.ScrollIntoView(grid.Items[index]);
            return (DataGridRow?)grid.ItemContainerGenerator.ContainerFromIndex(index);
        }

        // Returns the first descendant of type T or null if none found.
        public static T? FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            if (parent == null) return null;

            int childCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T t)
                {
                    return t;
                }

                var result = FindVisualChild<T>(child);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }
    }
}
