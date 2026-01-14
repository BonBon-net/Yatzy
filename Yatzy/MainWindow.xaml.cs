using System.Windows;
using System.Windows.Controls;

namespace Yatzy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IUserControlManager
    {
        FuncLayer funcLayer = new FuncLayer();
        IUserControlManager userControlManager;
        Terninger TerningUserControl;
        Menu MenuUserControl;

        public MainWindow()
        {
            InitializeComponent();
            userControlManager = this;
            MenuUserControl = new Menu(funcLayer, this);
            TerningUserControl = new Terninger(funcLayer, this);
            DataContext = funcLayer;
            ChangeUserControl(MenuUserControl);
        }

        public void StartGame()
        {
            funcLayer.StartGame();
            ChangeUserControl(TerningUserControl);
        }

        public void StopGame()
        {
            ChangeUserControl(MenuUserControl);
            funcLayer.StopGame();
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