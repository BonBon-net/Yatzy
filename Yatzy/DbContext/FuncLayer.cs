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
            spillerNavn = spillerNavn.ToCharArray().First().ToString().ToUpper() + spillerNavn.Substring(1).ToLower();
            Spiller spiller = new Spiller(0, spillerNavn);
            SpillerListe.Add(spiller);
            Model.SaveChanges();
            RaisePropertyChanged(nameof(SpillerListe));
            return spiller;
        }

        public Spiller FjernSpiller(Spiller spiller)
        {
            if (spiller == null)
            {
                throw new ArgumentException("Spillernavn kan ikke være tomt eller kun indeholde mellemrum.");
            }
            SpillerListe.Remove(spiller);
            Model.SaveChanges();
            RaisePropertyChanged(nameof(SpillerListe));
            return spiller;
        }
    }
}
