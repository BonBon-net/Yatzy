using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Yatzy.YatzyDbContext;

namespace Yatzy
{
    /// <summary>
    /// Interaction logic for Menu.xaml
    /// </summary>
    public partial class Menu : UserControl
    {
        private FuncLayer FuncLayer;
        private IUserControlManager UserControlManager { get; set; }

        public Menu(FuncLayer FuncLayer, IUserControlManager userControlManager)
        {
            InitializeComponent();
            //DataContext = MainWindow.funcLayer;
            this.FuncLayer = FuncLayer;
            DataContext = this.FuncLayer;
            UserControlManager = userControlManager;
            StartSpil_IsEnabled();
            txtSpillerNavn.Text = string.Empty;
            //this.funcLayer.RaisePropertyChanged(nameof(this.funcLayer.SpillerListe));
        }

        private void TilføjSpiller_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Spiller spiller = FuncLayer.TilføjSpiller(txtSpillerNavn.Text);
                txtSpillerNavn.Clear();
                StartSpil_IsEnabled();
                lbSpillerList.SelectedItem = spiller;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error message");
            }
        }

        private void btnSaveSpiller_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Spiller? spiller = lbSpillerList.SelectedItem as Spiller;
                if (spiller != null)
                {
                    string nytNavn = txtSpillerNavn.Text;
                    spiller = FuncLayer.GemSpiller(spiller, nytNavn);
                    lbSpillerList.SelectedItem = spiller;
                }
                else
                {
                    MessageBox.Show("Vælg en spiller fra listen for at gemme ændringer.", "Info");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error message");
            }
        }

        // Fjern spiller HELT
        private void FjernSpiller_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Spiller? spiller = lbSpillerList.SelectedItem as Spiller;
                if (spiller == null)
                {
                    MessageBox.Show("Vælg en spiller fra listen for at fjerne.", "Info");
                    return;
                }
                FuncLayer.FjernSpiller(spiller);
                txtSpillerNavn.Clear();
                StartSpil_IsEnabled();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error message");
            }
        }

        private void StartSpil_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (FuncLayer.SpillerListe.Count < 1)
                {
                    btnStartSpil.IsEnabled = false;
                    throw new AccessViolationException("Der skal være mindst 1 spillere for at starte spillet.");
                }
                else
                {
                    Spil? spil = lbActiveSpil.SelectedItem as Spil;
                    if (spil != null)
                    {
                        int spilId = spil.Id;
                        FuncLayer.LoadSpil(spil, false);
                        StartSpil_IsEnabled();
                        if (lbActiveSpil.SelectedItem == null)
                        {
                            lbActiveSpil.SelectedItem = FuncLayer.SpilListe.First(spil => spil.Id == spilId);
                        }
                        UserControlManager.StartGame();
                    }
                    else
                    {
                        throw new NullReferenceException();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error message");
            }
        }

        private void txtSpillerNavn_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtSpillerNavn.Text))
                {
                    btnTilføjSpiller.IsEnabled = false;
                }
                else
                {
                    btnTilføjSpiller.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error message");
            }
        }

        private void StartSpil_IsEnabled()
        {
            if (FuncLayer.Spil != null && FuncLayer.Spil.Spillere.Count > 0)
            {
                btnStartSpil.IsEnabled = true;
            }
            else
            {
                btnStartSpil.IsEnabled = false;
            }
        }

        private void btnFjernSpillerFraSpil_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SpillerSpil? spiller = lbSpillerSpil.SelectedItem as SpillerSpil;
                Spil? spil = lbActiveSpil.SelectedItem as Spil;
                if (spiller == null)
                {
                    throw new NullReferenceException("No player is selected");
                }
                if (spil == null)
                {
                    throw new NullReferenceException("No game is selected");
                }

                FuncLayer.FjernSpillerFraSpil(spiller, spil);

                StartSpil_IsEnabled();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error message");
            }
        }

        private void btnTilføjSpillerTilSpil_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Spiller? spiller = lbSpillerList.SelectedItem as Spiller;

                if (FuncLayer.Spil == null)
                {
                    throw new NullReferenceException("Spil er ikke valgt");
                }
                if (spiller == null)
                {
                    throw new NullReferenceException("Spiller er ikke valgt");
                }
                if (FuncLayer.Spil.IsStarted)
                {
                    throw new InvalidOperationException("Spillet er gået igang");
                }
                if (FuncLayer.Spil.Spillere.FirstOrDefault(spilSpiller => spilSpiller.Spiller == spiller) != null)
                {
                    throw new InvalidOperationException("Spiller har blevet valgt");
                }

                FuncLayer.TilføjSpillerTilSpil(spiller);
                SpillerSpil? spillerSpil = FuncLayer.Spil.Spillere.FirstOrDefault(spillere => spillere.Navn == spiller.Navn);
                if (spillerSpil != null && lbSpillerList.SelectedItem != spillerSpil)
                {
                    lbSpillerSpil.SelectedItem = spillerSpil;
                }
                StartSpil_IsEnabled();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error message");
            }
        }

        // Fjern spiller fra aktive spil
        private void btnFjernActivSpil_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Spil? spil = lbActiveSpil.SelectedItem as Spil;
                if (spil == null)
                {
                    throw new InvalidOperationException("Der blev ikke vælgt et spil");
                }

                FuncLayer.FjernActivSpil(spil);
                StartSpil_IsEnabled();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error message");
            }
        }

        private void lbSpillerList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Spiller? spiller_1 = lbSpillerList.SelectedItem as Spiller;
                if (spiller_1 != null)
                {
                    txtSpillerNavn.Text = spiller_1.Navn;
                    btnFjernSpiller.IsEnabled = true;
                    btnSaveSpiller.IsEnabled = true;
                    btnTilføjSpillerTilSpil.IsEnabled = true;
                    //Spiller? spiller_2 = FuncLayer.Spillere.FirstOrDefault(spillere => spillere == spiller_1);
                    SpillerSpil? spillerSpil = FuncLayer.SpillerListe.FirstOrDefault(spillerSpil => spillerSpil.Spiller == spiller_1);
                    if (spillerSpil != null && lbSpillerList.SelectedItem != spillerSpil)
                    {
                        lbSpillerSpil.SelectedItem = spillerSpil;
                    }
                }
                else
                {
                    btnFjernSpiller.IsEnabled = false;
                    btnSaveSpiller.IsEnabled = false;
                    btnTilføjSpillerTilSpil.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error message");
            }
        }

        private void lbSpillerSpil_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                SpillerSpil? spillerSpil = lbSpillerSpil.SelectedItem as SpillerSpil;
                if (spillerSpil != null)
                {
                    btnFjernSpillerFraSpil.IsEnabled = true;
                    Spiller? spiller = FuncLayer.Spillere.FirstOrDefault(spiller => spiller.Navn == spillerSpil.Navn);
                    if (spiller != null && lbSpillerList.SelectedItem != spiller)
                    {
                        lbSpillerList.SelectedItem = spiller;
                    }
                }
                else
                {
                    btnFjernSpillerFraSpil.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error message");
            }
        }

        private void lbActiveSpil_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Spil? spil = lbActiveSpil.SelectedItem as Spil;
                if (spil != null)
                {
                    int spilId = spil.Id;
                    FuncLayer.LoadSpil(spil, true);
                    btnFjernActivSpil.IsEnabled = true;
                    if (lbActiveSpil.SelectedItem == null)
                    {
                        lbActiveSpil.SelectedItem = FuncLayer.SpilListe.First(spil => spil.Id == spilId);
                    }
                }
                else
                {
                    btnFjernActivSpil.IsEnabled = false;
                }
                StartSpil_IsEnabled();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error message");
            }
        }

        private void lbSpillerList_Selected(object sender, RoutedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error message");
            }
        }

        private void btnNytSpil_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Spil spil = FuncLayer.NytSpil();
                lbActiveSpil.SelectedIndex = FuncLayer.SpilListe.IndexOf(spil);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error message");
            }
        }
    }
}
