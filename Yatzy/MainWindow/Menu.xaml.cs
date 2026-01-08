using System;
using System.Collections.Generic;
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
            if (this.FuncLayer.SpillerListe.Count > 0) 
            {
                btnStartSpil.IsEnabled = true; 
            }
            else 
            {
                btnStartSpil.IsEnabled = false; 
            }
            //this.FuncLayer.RaisePropertyChanged(nameof(this.FuncLayer.SpillerListe));
        }

        private void TilføjSpiller_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FuncLayer.TilføjSpiller(txtSpillerNavn.Text);
                txtSpillerNavn.Clear();
                if (FuncLayer.SpillerListe.Count > 0) { btnStartSpil.IsEnabled = true; }
                else { btnStartSpil.IsEnabled = false; }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FjernSpiller_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FuncLayer.FjernSpiller(SpillerListe.SelectedItem as Spiller);
                txtSpillerNavn.Clear();
                if (FuncLayer.SpillerListe.Count > 0) { btnStartSpil.IsEnabled = true; }
                else { btnStartSpil.IsEnabled = false; }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgbPlayerList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SpillerListe.SelectedItem != null)
            {
                btnFjernSpiller.IsEnabled = true;
                txtSpillerNavn.Text = (SpillerListe.SelectedItem as Spiller).Navn;
            }
            else
            {
                btnFjernSpiller.IsEnabled = false;
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
                MessageBox.Show(ex.Message);
            }
        }
    }
}
