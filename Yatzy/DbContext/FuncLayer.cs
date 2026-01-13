using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Yatzy.YatzyDbContext;
using static System.Formats.Asn1.AsnWriter;

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
                Spiller? Spiller;
                try
                {
                    Spiller = SpillerListe[CurrentPlayerIndex];
                }
                catch (ArgumentOutOfRangeException)
                {
                    if (!(SpillerListe.Count() > 0))
                    {
                        // Temporary add and remove a player to avoid index issues
                        Spiller = TilføjSpiller("Delete me");
                        if (SpillerListe.FirstOrDefault(player => player.Navn == Spiller.Navn) != null)
                        {
                            Spiller = FjernSpiller(SpillerListe.First(player => player.Navn == Spiller.Navn));
                        }
                        if (SpillerListe.FirstOrDefault(player => player.Navn == Spiller.Navn) != null)
                        {
                            throw new InvalidDataException("Critical error: Could not remove temporary player");
                        }
                    }
                    else
                    {
                        CurrentPlayerIndex = 0;
                        Spiller = SpillerListe[CurrentPlayerIndex];
                    }
                }
                return Spiller;
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

        public Spiller TilføjSpiller(string spillerNavn)
        {
            if (string.IsNullOrEmpty(spillerNavn))
            {
                throw new ArgumentException("Spillernavn kan ikke være tomt");
            }

            // Create new player
            string[] player = spillerNavn.Split(" ");
            spillerNavn = string.Empty;
            for (int i = 0; i < player.Length; i++)
            {
                if (spillerNavn != string.Empty)
                {
                    spillerNavn += " ";
                }
                spillerNavn += player[i].First().ToString().ToUpper() + player[i].Substring(1).ToLower();
            }
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

        public Spiller NæsteSpiller()
        {
            if ((SpillerListe.Count - 1) == CurrentPlayerIndex)
            {
                CurrentPlayerIndex = 0;
            }
            else
            {
                CurrentPlayerIndex += 1;
            }
            RaisePropertyChanged(nameof(SpillerTur));
            return SpillerTur;
        }

        public int Registrer(DataGridCellInfo cell, Terning[] terninger)
        {
            string header = cell.Column.Header.ToString();

            int score = RegistrerHeader(header, terninger);

            // Set property value by reflection
            PropertyInfo? property = typeof(Spiller).GetProperty(TrimString(header));
            property?.SetValue(SpillerTur, score);

            return score;
        }

        public int RegistrerHeader(string header, Terning[] terninger)
        {
            int[] values = CalculateValues();
            header = TrimString(header);
            int score = 0;
            string exceptionText = $"{header} er allerede brugt";

            if (header == "Enere")
            {
                if (SpillerTur.Enere != null)
                {
                    throw new Exception(exceptionText);
                }

                score = values[0] * 1;

                //SpillerTur.Enere = score;
            }
            else if (header == "Toere")
            {
                if (SpillerTur.Toere != null)
                {
                    throw new Exception(exceptionText);
                }

                score = values[1] * 2;

                //SpillerTur.Toere = score;
            }
            else if (header == "Treere")
            {
                if (SpillerTur.Treere != null)
                {
                    throw new Exception(exceptionText);
                }

                score = values[2] * 3;

                //SpillerTur.Treere = score;
            }
            else if (header == "Firere")
            {
                if (SpillerTur.Firere != null)
                {
                    throw new Exception(exceptionText);
                }

                score = values[3] * 4;

                //SpillerTur.Firere = score;
            }
            else if (header == "Femmere")
            {
                if (SpillerTur.Femmere != null)
                {
                    throw new Exception(exceptionText);
                }

                score = values[4] * 5;

                //SpillerTur.Femmere = score;
            }
            else if (header == "Seksere")
            {
                if (SpillerTur.Seksere != null)
                {
                    throw new Exception(exceptionText);
                }

                score = values[5] * 6;

                //SpillerTur.Seksere = score;
            }
            else if (header == "EtPar")
            {
                if (SpillerTur.EtPar != null)
                {
                    throw new Exception(exceptionText);
                }

                for (int i = 0; i < values.Length; i++)
                {
                    if (values[i] >= 2)
                    {
                        score = 2 * (i + 1);
                    }
                }

                //SpillerTur.EtPar = score;
            }
            else if (header == "ToPar")
            {
                if (SpillerTur.ToPar != null)
                {
                    throw new Exception(exceptionText);
                }

                int[] valuesIndex = { -1, -1 };
                for (int i = 0; i < values.Length; i++)
                {
                    if (values[i] >= 2)
                    {
                        valuesIndex[0] = i;

                        break;
                    }
                }
                if (valuesIndex[0] != -1)
                {
                    for (int i = 0; i < values.Length; i++)
                    {
                        if (values[i] >= 2 && valuesIndex[0] != i)
                        {
                            valuesIndex[1] = i;

                            if (valuesIndex[1] != -1)
                            {
                                score = (((valuesIndex[0] + 1) * 2) + ((valuesIndex[1] + 1) * 2));
                            }
                            break;
                        }
                    }
                }

                //SpillerTur.ToPar = score;
            }
            else if (header == "TreEns")
            {
                if (SpillerTur.TreEns != null)
                {
                    throw new Exception(exceptionText);
                }

                for (int i = 0; i < values.Length; i++)
                {
                    if (values[i] >= 3)
                    {
                        score = 3 * (i + 1);
                    }
                }

                //SpillerTur.TreEns = score;
            }
            else if (header == "FireEns")
            {
                if (SpillerTur.FireEns != null)
                {
                    throw new Exception(exceptionText);
                }

                for (int i = 0; i < values.Length; i++)
                {
                    if (values[i] >= 4)
                    {
                        score = 4 * (i + 1);
                    }
                }

                //SpillerTur.FireEns = score;
            }
            else if (header == "LilleStraight")
            {
                if (SpillerTur.LilleStraight != null)
                {
                    throw new Exception(exceptionText);
                }

                if (values[0] == 1 && values[1] == 1 &&
                    values[2] == 1 && values[3] == 1 &&
                    values[4] == 1)
                {
                    score = 1 + 2 + 3 + 4 + 5;
                }

                //SpillerTur.LilleStraight = score;
            }
            else if (header == "StorStraight")
            {
                if (SpillerTur.StorStraight != null)
                {
                    throw new Exception(exceptionText);
                }

                if (values[1] == 1 && values[2] == 1 &&
                    values[3] == 1 && values[4] == 1 &&
                    values[5] == 1)
                {
                    score = 2 + 3 + 4 + 5 + 6;
                }

                //SpillerTur.StorStraight = score;
            }
            else if (header == "Hus")
            {
                if (SpillerTur.Hus != null)
                {
                    throw new Exception(exceptionText);
                }

                // If in fact we have a house:
                int[] ints = { -1, -1 };
                for (int i = 0; i < values.Length; i++)
                {
                    if (values[i] == 2)
                    {
                        ints[0] = i;
                        break;
                    }
                }
                if (-1 <= ints[0])
                {
                    for (int i = 0; i < values.Length; i++)
                    {
                        if (values[i] == 3 && ints[0] != i)
                        {
                            ints[1] = i;

                            if (-1 <= ints[1])
                            {
                                score = (values[ints[0]] * (ints[0] + 1)) + (values[ints[1]] * (ints[1] + 1));
                            }
                            break;
                        }
                    }
                }

                //SpillerTur.Hus = score;
            }
            else if (header == "Chance")
            {
                if (SpillerTur.Chance != null)
                {
                    throw new Exception(exceptionText);
                }

                // calculate'ing horse + moo
                for (int i = 0; i < terninger.Length; i++)
                {
                    score += terninger[i].DiceValue;
                }

                //SpillerTur.Chance = score;
            }
            else if (header == "Yatzy")
            {
                if (SpillerTur.Yatzy != null)
                {
                    throw new Exception(exceptionText);
                }

                for (int i = 0; i < values.Length; i++)
                {
                    if (values[i] == 5)
                    {
                        score = 50;
                        break;
                    }
                }

                //SpillerTur.Yatzy = score;
            }
            else
            {
                throw new ApplicationException("Ugyldigt kolonne header");
            }
            return score;

            int[] CalculateValues()
            {
                int[] values = new int[6];
                for (int i = 0; i < terninger.Length; i++)
                {
                    int diceValue = terninger[i].DiceValue;
                    values[diceValue - 1] += 1;
                }
                return values;
            }
        }

        public string TrimString(string stringTrimed)
        {
            string[] trimStrings = stringTrimed.Split(" ");
            stringTrimed = string.Empty;
            for (int i = 0; i < trimStrings.Length; i++)
            {
                stringTrimed += trimStrings[i];
            }
            return stringTrimed;
        }
    }
}
