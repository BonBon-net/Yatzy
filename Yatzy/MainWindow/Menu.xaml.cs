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
                Spiller? spiller = dgbSpillerListe.SelectedItem as Spiller;
                if (spiller != null)
                {
                    string nytNavn = txtSpillerNavn.Text;
                    FuncLayer.GemSpiller(spiller, nytNavn);
                    dgbSpillerListe.SelectedItem = null;
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

        private void FjernSpiller_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Spiller? spiller = dgbSpillerListe.SelectedItem as Spiller;
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

        private void dgbPlayerList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Spiller? spiller = dgbSpillerListe.SelectedItem as Spiller;
                if (spiller != null && dgbSpillerListe.SelectedItem != null)
                {
                    btnFjernSpiller.IsEnabled = true;
                    txtSpillerNavn.Text = spiller.Navn;
                }
                else
                {
                    btnFjernSpiller.IsEnabled = false;
                }
                if (spiller != null && !string.IsNullOrEmpty(txtSpillerNavn.Text) && txtSpillerNavn.Text == spiller.Navn)
                {
                    btnSaveSpiller.IsEnabled = true;
                }
                else
                {
                    btnSaveSpiller.IsEnabled = false;
                }
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
            if (FuncLayer.SpillerListe.Count > 0)
            {
                btnStartSpil.IsEnabled = true;
            }
            else
            {
                btnStartSpil.IsEnabled = false;
            }
        }
    }
}
