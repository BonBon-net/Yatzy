using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yatzy
{
    public class FuncLayer : INotifyPropertyChanged
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

        public List<string> YatzyBlock { get; set; } = new List<string>() {
            "1'ere", "2'ere", "3'ere", "4'ere", "5'ere", "6'ere", "Sum",
            "Bonus", "Et Par", "To par", "Tre ens", "Fire ens", "Lille straight",
            "Stor straight", "Fuldt hus", "Chance", "Yatzy", "Sum"
        };
        public List<string> PlayerList { get; set; } = new List<string>() { "Henning" };
        public string DefaultDataContent(string DataContent = "FuncLayer") { return DataContent; }

        public string TilføjSpiller(string spillerNavn)
        {
            if (string.IsNullOrWhiteSpace(spillerNavn))
            { 
                throw new ArgumentException("Spillernavn kan ikke være tomt eller kun indeholde mellemrum."); 
            }
            char[] spillerNavnChars = spillerNavn.ToLower().ToCharArray();
            spillerNavn = spillerNavnChars.First().ToString().ToUpper();
            for (int i = 1; i < spillerNavnChars.Length; i++)
            {
                spillerNavn += spillerNavnChars[i];
            }
            if (PlayerList.Contains(spillerNavn))
            {
                throw new ArgumentException("Spillernavnet findes allerede. Vælg et andet navn.");
            }
            PlayerList.Add(spillerNavn);
            RaisePropertyChanged(nameof(PlayerList));
            return spillerNavn;
        }

        public string FjernSpiller(string spillerNavn)
        {
            if (string.IsNullOrWhiteSpace(spillerNavn))
            {
                throw new ArgumentException("Spillernavn kan ikke være tomt eller kun indeholde mellemrum.");
            }
            char[] spillerNavnChars = spillerNavn.ToLower().ToCharArray();
            spillerNavn = spillerNavnChars.First().ToString().ToUpper();
            for (int i = 1; i < spillerNavnChars.Length; i++)
            {
                spillerNavn += spillerNavnChars[i];
            }
            if (!PlayerList.Contains(spillerNavn))
            {
                throw new ArgumentException("Spillernavnet findes ikke. Kan derfor ikke fjernes.");
            }
            PlayerList.Remove(spillerNavn);
            RaisePropertyChanged(nameof(PlayerList));
            return spillerNavn;
        }
    }

    class Spiller
    {
        public string Navn { get; set; }
        public int[] Score { get; set; } = new int[18];
        public Spiller(string navn)
        {
            Navn = navn;
        }
    }
}
