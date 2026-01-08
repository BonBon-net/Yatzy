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
            int bonusValue = 0;

            if (SpillerTur.Enere != null && header == "Enere")
            {
                for (int i = 0; i < terninger.Length; i++)
                {
                    if (terninger[i].DiceValue == 1)
                    {
                        score += terninger[i].DiceValue;
                        bonusValue++;
                    }
                }
                if (bonusValue >= 2)
                {
                    SpillerTur.Bonus += bonusValue;
                }

                SpillerTur.Enere = score;
            }
            else if (SpillerTur.Toere != null && header == "Toere")
            {
                for (int i = 0; i < terninger.Length; i++)
                {
                    if (terninger[i].DiceValue == 2)
                    {
                        score += terninger[i].DiceValue * 2;
                        bonusValue++;
                    }
                }
                if (bonusValue >= 2)
                {
                    SpillerTur.Bonus += bonusValue * 2;
                }

                SpillerTur.Toere = score;
            }
            else if (SpillerTur.Treere != null && header == "Treere")
            {
                for (int i = 0; i < terninger.Length; i++)
                {
                    if (terninger[i].DiceValue == 3)
                    {
                        score += terninger[i].DiceValue * 3;
                        bonusValue++;
                    }
                }
                if (bonusValue >= 2)
                {
                    SpillerTur.Bonus += bonusValue * 3;
                }

                SpillerTur.Treere = score;
            }
            else if (SpillerTur.Firere != null && header == "Firere")
            {
                for (int i = 0; i < terninger.Length; i++)
                {
                    if (terninger[i].DiceValue == 4)
                    {
                        score += terninger[i].DiceValue * 4;
                        bonusValue++;
                    }
                    if (bonusValue >= 2)
                    {
                        SpillerTur.Bonus += bonusValue * 4;
                    }
                }

                SpillerTur.Firere = score;
            }
            else if (SpillerTur.Femmere != null && header == "Femmere")
            {
                for (int i = 0; i < terninger.Length; i++)
                {
                    if (terninger[i].DiceValue == 5)
                    {
                        score += terninger[i].DiceValue * 5;
                        bonusValue++;
                    }
                }
                if (bonusValue >= 2)
                {
                    SpillerTur.Bonus += bonusValue * 5;
                }

                SpillerTur.Femmere = score;
            }
            else if (SpillerTur.Seksere != null && header == "Seksere")
            {
                for (int i = 0; i < terninger.Length; i++)
                {
                    if (terninger[i].DiceValue == 6)
                    {
                        score += terninger[i].DiceValue * 6;
                        bonusValue++;
                    }
                }
                if (bonusValue >= 2)
                {
                    SpillerTur.Bonus += bonusValue * 6;
                }

                SpillerTur.Seksere = score;
            }
            else if (SpillerTur.EtPar != null && header == "EtPar")
            {
                score = 9;
                SpillerTur.EtPar = score;
            }
            else if (SpillerTur.ToPar != null && header == "ToPar")
            {
                score = 10;
                SpillerTur.ToPar = score;
            }
            else if (SpillerTur.TreEns != null && header == "TreEns")
            {
                score = 11;
                SpillerTur.TreEns = score;
            }
            else if (SpillerTur.FireEns != null && header == "FireEns")
            {
                score = 12;
                SpillerTur.FireEns = score;
            }
            else if (SpillerTur.LilleStraight != null && header == "LilleStraight")
            {
                score = 13;
                SpillerTur.LilleStraight = score;
            }
            else if (SpillerTur.StorStraight != null && header == "StorStraight")
            {
                score = 14;
                SpillerTur.StorStraight = score;
            }
            else if (SpillerTur.Hus != null && header == "Hus")
            {
                score = 15;
                SpillerTur.Hus = score;
            }
            else if (SpillerTur.Chance != null && header == "Chance")
            {
                score = 16;
                SpillerTur.Chance = score;
            }
            else if (SpillerTur.Yatzy != null && header == "Yatzy")
            {
                score = 17;
                SpillerTur.Yatzy = score;
            }
            //else if (SpillerTur.Enere != null && header == "TotalSum")
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
