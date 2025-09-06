using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Yatzy
{
    public interface IUserControlManager
    {
        public void StartGame();
        
        public void StopGame();

        public void ChangeUserControl(UserControl newControl);

        public void TilføjUserControl(UserControl newControl);

        public void FjernUserControl(UserControl newControl);
    }
}
