using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yatzy.YatzyDbContext;

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

        Model Model { get; set; } = new Model();
        public FuncLayer()
        {
            Model.SpillerTabel.Load();
            RaisePropertyChanged(nameof(SpillerListe));
        }

        //public ObservableCollection<string> PlayerList { get; set; } = new ObservableCollection<string>() { };
        public ObservableCollection<Spiller> SpillerListe
        {
            get
            {
                return Model.SpillerTabel.Local.ToObservableCollection();
            }
        }

        public List<string> YatzyBlock { get; set; } = new List<string>() {
            "1'ere", "2'ere", "3'ere", "4'ere", "5'ere", "6'ere", "Sum",
            "Bonus", "Et Par", "To par", "Tre ens", "Fire ens", "Lille straight",
            "Stor straight", "hus", "Chance", "Yatzy", "Sum"
        };

        public string DefaultDataContent(string DataContent = "FuncLayer") { return DataContent; }

        public Spiller TilføjSpiller(string spillerNavn)
        {
            if (string.IsNullOrWhiteSpace(spillerNavn))
            { 
                throw new ArgumentException("Spillernavn kan ikke være tomt eller kun indeholde mellemrum."); 
            }
            //char[] spillerNavnChars = spillerNavn.ToLower().ToCharArray();
            spillerNavn = spillerNavn.ToCharArray().First().ToString().ToUpper() + spillerNavn.Substring(1).ToLower();
            //spillerNavn = spillerNavnChars.First().ToString().ToUpper();
            //for (int i = 1; i < spillerNavnChars.Length; i++)
            //{
            //    spillerNavn += spillerNavnChars[i];
            //}

            //if (SpillerListe.Contains(Spiller p => p spillerNavn))
            //{
            //    throw new ArgumentException("Spillernavnet findes allerede. Vælg et andet navn.");
            //}
            Spiller spiller = new Spiller(0, spillerNavn);
            SpillerListe.Add(spiller);
            Model.SaveChanges();
            return spiller;
        }

        public Spiller FjernSpiller(Spiller spiller)
        {
            if (spiller == null)
            {
                throw new ArgumentException("Spillernavn kan ikke være tomt eller kun indeholde mellemrum.");
            }

            //string spillerNavnChars = spillerNavn.ToLower();
            //spillerNavn = spillerNavnChars.First() + spillerNavnChars.Last(spillerNavnChars.Length - 1);

            //spillerNavn = spillerNavnChars.First().ToString().ToUpper();
            //for (int i = 1; i < spillerNavnChars.Length; i++)
            //{
            //    spillerNavn += spillerNavnChars[i];
            //}

            //if (!SpillerListe.Contains(spillerNavn))
            //{
            //    throw new ArgumentException("Spillernavnet findes ikke. Kan derfor ikke fjernes.");
            //}
            SpillerListe.Remove(spiller);
            Model.SaveChanges();
            return spiller;
        }

        //public List<string> GetYatzyBlock()
        //{
        //    return YatzyBlock;
        //}

        //public List<string> GetPlayerList()
        //{ 
        //    return SpillerListe.ToList();
        //}
    }
}
