using System.Windows;
using System.Windows.Controls;
using Yatzy.Bots;
using Yatzy.YatzyDbContext;

namespace Yatzy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IUserControlManager
    {
        FuncLayer FuncLayer = new FuncLayer();
        IUserControlManager userControlManager;
        Terninger TerningUserControl;
        Menu MenuUserControl;

        public MainWindow()
        {
            InitializeComponent();
            userControlManager = this;
            MenuUserControl = new Menu(FuncLayer, this);
            TerningUserControl = new Terninger(FuncLayer, this);
            //new __BotHelper(FuncLayer, TerningUserControl);
            DataContext = FuncLayer;
            ChangeUserControl(MenuUserControl);
        }

        public void StartGame()
        {
            FuncLayer.StartSpil();
            ChangeUserControl(TerningUserControl);
            TerningUserControl.Init();
        }

        public void StopGame()
        {
            ChangeUserControl(MenuUserControl);
            FuncLayer.StopSpil();
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