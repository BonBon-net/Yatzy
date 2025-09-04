using System.Text;
using System.Threading;
using System.Timers;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Terninger TerningUserControl = new Terninger();
        FuncLayer FuncLayer = new FuncLayer();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = FuncLayer.DefaultDataContent();
            //MainGrid.Children.Add(TerningUserControl);
        }

        private void TilføjSpiller_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FuncLayer.TilføjSpiller(txtSpillerNavn.Text);
                txtSpillerNavn.Clear();
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
                FuncLayer.FjernSpiller(dgbPlayerList.SelectedItem.ToString());
                txtSpillerNavn.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgbPlayerList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgbPlayerList.SelectedItem != null)
            {
                btnFjernSpiller.IsEnabled = true;
                txtSpillerNavn.Text = dgbPlayerList.SelectedItem.ToString();
            }
            else
            {
                btnFjernSpiller.IsEnabled = false;
            }
        }
    }
}