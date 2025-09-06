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
using Yatzy.YatzyDbContext;

namespace Yatzy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Terninger TerningUserControl = new Terninger();
        FuncLayer FuncLayer = new FuncLayer();
        Menu MenuUserControl;

        public MainWindow()
        {
            InitializeComponent();
            MenuUserControl = new Menu(FuncLayer, );
            DataContext = FuncLayer;
            ChangeUserControl(MenuUserControl);
        }

        public void StartGame()
        {
            ChangeUserControl(TerningUserControl);
        }

        public void StopGame()
        {
            ChangeUserControl(MenuUserControl);
        }

        public void ChangeUserControl(UserControl newControl)
        {
            this.MainGrid.Children.Clear();
            this.MainGrid.Children.Add(newControl);
        }

        public void TilføjUserControl(UserControl newControl)
        {
            this.MainGrid.Children.Add(newControl);
        }

        public void FjernUserControl(UserControl newControl)
        {
            this.MainGrid.Children.Remove(newControl);
        }
    }
}