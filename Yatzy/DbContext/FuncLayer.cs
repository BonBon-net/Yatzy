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
    public class Terning : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public int DiceValue { get; set; } = -1;
        public Image Terningside { get; set; } = new Image();
        public bool IsHeld { get; set; } = false;
    }

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

        public int CurrentPlayerIndex;
        public Spiller SpillerTur
        {
            get
            {
                return SpillerListe[CurrentPlayerIndex];
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

        public void StartGame()
        {
            CurrentPlayerIndex = 0;
        }

        public int Registrer(DataGridCellInfo cell, Terning[] terninger)
        {
            string header = cell.Column.Header.ToString();
            int score = 0;

            if (header == "Enere")
            {
                // Beregn det rigtige antal points
                score = 1;
                // Indsæt antal points i Spiller.Enere
                SpillerTur.Enere = score;
            }
            else if (header == "Toere")
            {
                score = 2;
                SpillerTur.Toere = score;
            }
            else if (header == "Treere")
            {
                score = 3;
                SpillerTur.Treere = score;
            }
            else if (header == "Firere")
            {
                score = 4;
                SpillerTur.Firere = score;
            }
            else if (header == "Femmere")
            {
                score = 5;
                SpillerTur.Femmere = score;
            }
            else if (header == "Seksere")
            {
                score = 6;
                SpillerTur.Seksere = score;
            }
            //else if (header == "SUM")
            //{
            //    score = 7;
            //    SpillerTur.Seksere = score;
            //}
            else if (header == "Bonus")
            {
                score = 8;
                SpillerTur.Seksere = score;
            }
            else if (header == "EtPar")
            {
                score = 9;
                SpillerTur.Seksere = score;
            }
            else if (header == "ToPar")
            {
                score = 10;
                SpillerTur.Seksere = score;
            }
            else if (header == "TreEns")
            {
                score = 11;
                SpillerTur.Seksere = score;
            }
            else if (header == "FireEns")
            {
                score = 12;
                SpillerTur.Seksere = score;
            }
            else if (header == "LilleStraight")
            {
                score = 13;
                SpillerTur.Seksere = score;
            }
            else if (header == "StorStraight")
            {
                score = 14;
                SpillerTur.Seksere = score;
            }
            else if (header == "Hus")
            {
                score = 15;
                SpillerTur.Seksere = score;
            }
            else if (header == "Chance")
            {
                score = 16;
                SpillerTur.Seksere = score;
            }
            else if (header == "Yatzy")
            {
                score = 17;
                SpillerTur.Seksere = score;
            }
            //else if (header == "TotalSum")
            //{
            //    score = 18;
            //    SpillerTur.Seksere = score;
            //}
            else
            {
                throw new ApplicationException("Ugyldigt kolonne header");
            }
            return score;
        }
    }
}
