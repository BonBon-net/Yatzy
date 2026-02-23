using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;

namespace Yatzy.Bots
{
    public class Bot
    {
        private FuncLayer FuncLayer;
        private Terninger TerningUserControl;
        private __BotHelper __BotHelper;

        public Bot(FuncLayer funcLayer, Terninger terningUserControl, __BotHelper __botHelper)
        {
            this.FuncLayer = funcLayer;
            this.TerningUserControl = terningUserControl;
            this.__BotHelper = __botHelper;
        }

        public void BotPLayer()
        {
            List<Terning> terninger = __BotHelper.GetTernings();
            int[] values = FuncLayer.CalculateValues();
            List<DataGridCell> cellList = __BotHelper.GetScoreBoardValues();

            if (FuncLayer.Spil.Kasted == 0)
            {
                // ... inside your method
                ButtonAutomationPeer peer = new ButtonAutomationPeer(TerningUserControl.btnKast);
                IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                invokeProv?.Invoke();
            }
            else
            {
                throw new Exception();
            }

            while (TerningUserControl.btnKast.IsEnabled)
            {

            }
        }
    }
}
