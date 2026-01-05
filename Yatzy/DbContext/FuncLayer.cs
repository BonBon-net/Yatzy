using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
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

        private Model model { get; set; } = new Model();
        public FuncLayer()
        {
            model.SpillerTabel.Load();
            RaisePropertyChanged(nameof(SpillerListe));
        }
        public ObservableCollection<Spiller> SpillerListe
        {
            get
            {
                return model.SpillerTabel.Local.ToObservableCollection();
            }
        }

        public static List<string> YatzyBlock { get; set; } = new List<string>() {
            "1'ere", "2'ere", "3'ere", "4'ere", "5'ere", "6'ere", "Sum",
            "Bonus", "Et Par", "To par", "Tre ens", "Fire ens", "Lille straight",
            "Stor straight", "hus", "Chance", "Yatzy", "Sum"
        };
        public string CurrentPlayer { get; set; }

        public Spiller TilføjSpiller(string spillerNavn)
        {
            if (string.IsNullOrEmpty(spillerNavn))
            {
                throw new ArgumentException("Spillernavn kan ikke være tomt");
            }

            // Create new player
            spillerNavn = spillerNavn.First().ToString().ToUpper() + spillerNavn.Substring(1).ToLower();
            // lambda
            if (SpillerListe.FirstOrDefault(player => spillerNavn == player.Navn) != null)
            {
                throw new Exception("Brugernavnet er taget");
            }
            Spiller spiller = new Spiller(0, spillerNavn);

            // Add new player to List
            SpillerListe.Add(spiller);
            model.SaveChanges();
            RaisePropertyChanged(nameof(SpillerListe));

            return spiller;
        }

        public Spiller FjernSpiller(Spiller spiller)
        {
            if (spiller == null)
            {
                throw new NullReferenceException($"spiller can't be null");
            }

            // Remove Splayer from List
            SpillerListe.Remove(spiller);
            model.SaveChanges();
            RaisePropertyChanged(nameof(SpillerListe));
            return spiller;
        }

        public void Registrer(DataGridCellInfo cell)
        {
            var a = cell.Column.Header;


        }
    }
}
