using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yatzy.Suport
{
    public class ExtraFuncLayer : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        //RaisePropertyChanged(nameof());
        private void RaisePropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        public Spil spil;
        public bool PlayerHasWon = false;
        public int HighestScorePlayerIndex;
        public int CurrentPlayerIndex;
    }
}
