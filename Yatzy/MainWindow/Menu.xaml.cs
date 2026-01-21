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
                FuncLayer.TilføjSpiller(txtSpillerNavn.Text);
                txtSpillerNavn.Clear();
                StartSpil_IsEnabled();
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
                    FuncLayer.GemSpiller(spiller, nytNavn);
                    lbSpillerList.SelectedItem = null;
                    txtSpillerNavn.Clear();
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
                    throw new AccessViolationException("Der skal være mindst 1 spillere for at starte spillet.");
                }
                else
                {
                    UserControlManager.StartGame();
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
                if (spiller == null)
                {
                    throw new InvalidOperationException("Player was empty");
                }
                FuncLayer.FjernSpillerFraSpil(spiller);
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
                    throw new Exception("Spil er ikke valgt");
                }
                if (spiller == null)
                {
                    throw new NullReferenceException("Spiller er ikke valgt");
                }
                if (FuncLayer.Spil.Spillere.FirstOrDefault(spilSpiller => spilSpiller.Spiller == spiller) != null)
                {
                    throw new InvalidOperationException("Spiller har blevet valgt");
                }

                FuncLayer.TilføjSpillerTilSpil(spiller);
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
                Spiller? spiller = lbSpillerList.SelectedItem as Spiller;
                if (spiller != null)
                {
                    txtSpillerNavn.Text = spiller.Navn;
                    btnFjernSpiller.IsEnabled = true;
                    btnSaveSpiller.IsEnabled = true;
                    btnTilføjSpillerTilSpil.IsEnabled = true;
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
                    lbSpillerList.SelectedIndex = FuncLayer.Spillere.IndexOf(FuncLayer.Spillere.First(spiller => spiller.Navn == spillerSpil.Navn));
                    btnFjernSpillerFraSpil.IsEnabled = true;
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
                    btnFjernActivSpil.IsEnabled = true;
                    FuncLayer.LoadSpil(spil);
                    StartSpil_IsEnabled();
                }
                else
                {
                    btnFjernActivSpil.IsEnabled = false;
                }
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
                FuncLayer.NytSpil();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error message");
            }
        }
    }
}
