using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Windows.Themes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using static Yatzy.Terninger;

namespace Yatzy.Bots
{
    public class __BotHelper
    {
        private FuncLayer FuncLayer;
        private Terninger TerningUserControl;

        public __BotHelper(FuncLayer funcLayer, Terninger terningUserControl)
        {
            this.FuncLayer = funcLayer;
            this.TerningUserControl = terningUserControl;
            new OldBot(FuncLayer, TerningUserControl, this);
        }

        public List<Terning> GetTernings()
        {
            return FuncLayer.Spil.Terninger;
        }

        public List<DataGridCell> GetScoreBoardValues()
        {
            int columnsCount = TerningUserControl.dgSpillerScoreBoard.Columns.Count();
            List<DataGridCell> cells = new List<DataGridCell>();

            for (int i = 0; i < columnsCount; i++)
            {
                DataGridCell cell = VisualTreeHelpers.GetCell(TerningUserControl.dgSpillerScoreBoard, FuncLayer.Spil.SpillerTurIndex, i);
                if (cell != null)
                {
                    if (TerningUserControl.dgSpillerScoreBoard.Columns[i].Header.ToString() != "Navn" &&
                        TerningUserControl.dgSpillerScoreBoard.Columns[i].Header.ToString() != "SUM" &&
                        TerningUserControl.dgSpillerScoreBoard.Columns[i].Header.ToString() != "Bonus" &&
                        TerningUserControl.dgSpillerScoreBoard.Columns[i].Header.ToString() != "Total")
                    {
                        cells.Add(cell);
                    }
                }
                else
                {
                    throw new NullReferenceException("Cell not found");
                }
            }

            return cells;
        }
    }
}

