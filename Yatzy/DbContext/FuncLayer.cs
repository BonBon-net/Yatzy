using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Controls;
using Yatzy.OldYatzyDbContext;

namespace OldYatzy
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

        public bool PlayerHasWon = false;
        public Spiller HighestScorePlayer;
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
                    Spiller = null;
                }
                return Spiller;
            }
        }
        private Model model { get; set; } = new Model();
        public FuncLayer()
        {
            model.SpillerTabel.Load();
            RaisePropertyChanged(nameof(SpillerListe));
            //model.SpilTabel.Load();
            //RaisePropertyChanged(nameof(SpilListe));
        }
        public ObservableCollection<Spiller> SpillerListe
        {
            get
            {
                return model.SpillerTabel.Local.ToObservableCollection();
            }
        }
        private bool isGameSaved = false;
        public bool IsGameSaved 
        {
            get
            {
                return isGameSaved;
            } 
            set
            {
                isGameSaved = !value;
                RaisePropertyChanged(nameof(IsGameSaved));
                RaisePropertyChanged(nameof(isGameSaved));
            }
        }
        //public ObservableCollection<Spil> SpilListe
        //{
        //    get
        //    {
        //        return model.SpilTabel.Local.ToObservableCollection();
        //    }
        //}

        public Spiller TilføjSpiller(string spillerNavn)
        {
            if (string.IsNullOrEmpty(spillerNavn))
            {
                throw new ArgumentException("Spillernavn kan ikke være tomt");
            }

            // Create new player
            spillerNavn = SpillerNavn(spillerNavn);
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

        public Spiller GemSpiller(Spiller spiller, string spillerNavn)
        {
            if (spiller == null)
            {
                throw new NullReferenceException($"spiller can't be null");
            }
            if (string.IsNullOrEmpty(spillerNavn))
            {
                throw new ArgumentException("Spillernavn kan ikke være tomt");
            }
            // Update player name
            spillerNavn = SpillerNavn(spillerNavn);
            // lambda
            if (SpillerListe.FirstOrDefault(spiller => spiller.Navn == spillerNavn) != null)
            {
                throw new Exception("Brugernavnet er taget");
            }

            spiller.Navn = spillerNavn;
            model.SaveChanges();
            RaisePropertyChanged(nameof(SpillerListe));
            //RaisePropertyChanged(nameof(SpilListe));

            return spiller;
        }

        private string SpillerNavn(string inputSpillerNavn)
        {
            string[] player = inputSpillerNavn.Split(" ");
            string nytSpillerNavn = string.Empty;
            for (int i = 0; i < player.Length; i++)
            {
                if (nytSpillerNavn != string.Empty)
                {
                    nytSpillerNavn += " ";
                }
                nytSpillerNavn += player[i].First().ToString().ToUpper() + player[i].Substring(1).ToLower();
            }
            return nytSpillerNavn;
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
            RaisePropertyChanged(nameof(SpillerTur));
            //RaisePropertyChanged(nameof(SpilListe));

            return spiller;
        }

        //public void NewGame(List<Terning> terninger)
        //{
        //    // Lave et nyt Spil-objekt med alle spiller fra SpillerListe
        //    Spil Spil = new Spil(-1, SpillerListe, 0, terninger, 0, DateTime.Now);

        //    StartGame();
        //}

        public void StartGame()
        {
            CurrentPlayerIndex = 0;
            RaisePropertyChanged(nameof(SpillerTur));
            HighestScorePlayer = SpillerTur;
            IsGameSaved = false;
        }
        public void StopGame()
        {
            for (int i = 0; i < SpillerListe.Count; i++)
            {
                SpillerListe[i].ResetScoreBoard();
            }

            CurrentPlayerIndex = 0;
            RaisePropertyChanged(nameof(SpillerListe));
            RaisePropertyChanged(nameof(SpillerTur));
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

        //public void SaveGame(List<Terning> terninger, int antalKasted)
        //{
        //    Spil Spil = new Spil(0, SpillerListe, CurrentPlayerIndex, terninger, antalKasted, DateTime.Now);
        //    SpilListe.Add(Spil);
        //    model.SaveChanges();
        //    RaisePropertyChanged(nameof(SpilListe));
        //}

        public int Registrer(DataGridCellInfo cell, List<Terning> terninger)
        {
            string header = cell.Column.Header.ToString();

            int score = RegnHeaderValue(header, terninger);

            // Set property value by reflection
            PropertyInfo property = typeof(ScoreBoard).GetProperty(TrimString(header));
            property.SetValue(SpillerTur.ScoreBoard, score);

            if (SpillerTur.ScoreBoard.TotalSum > HighestScorePlayer.ScoreBoard.TotalSum)
            {
                HighestScorePlayer = SpillerTur;
            }

            if (SpillerListe.Count - 1 == CurrentPlayerIndex)
            {
                string[] headers = { "Enere", "Toere", "Treere", "Firere", "Femmere", "Seksere", "EtPar","ToPar", "TreEns", "FireEns", "LilleStraight", "StorStraight", "Hus", "Chance", "Yatzy" };
                // check if Last player has a null header
                // If last player has no null value then end game
                int count = 0;
                for (int i = 0; i < headers.Length; i++)
                {
                    try
                    {
                        RegnHeaderValue(headers[i], terninger);
                    }
                    catch
                    {
                        count++;
                    }
                }
                if (count == headers.Length)
                {
                    PlayerHasWon = true;
                }
            }

            RaisePropertyChanged(nameof(SpillerListe));
            RaisePropertyChanged(nameof(SpillerTur));

            return score;
        }

        public int RegnHeaderValue(string header, List<Terning> terninger)
        {
            int[] values = CalculateValues();
            header = TrimString(header);
            int score = 0;
            string exceptionText = $"{header} er allerede brugt";

            ScoreBoard scoreBoard = SpillerTur.ScoreBoard;

            if (header == "Enere")
            {
                if (scoreBoard.Enere != null)
                {
                    throw new Exception(exceptionText);
                }

                score = values[0] * 1;

                //SpillerTur.ScoreBoard.Enere = score;
            }
            else if (header == "Toere")
            {
                if (scoreBoard.Toere != null)
                {
                    throw new Exception(exceptionText);
                }

                score = values[1] * 2;

                //SpillerTur.Toere = score;
            }
            else if (header == "Treere")
            {
                if (SpillerTur.ScoreBoard.Treere != null)
                {
                    throw new Exception(exceptionText);
                }

                score = values[2] * 3;

                //SpillerTur.Treere = score;
            }
            else if (header == "Firere")
            {
                if (scoreBoard.Firere != null)
                {
                    throw new Exception(exceptionText);
                }

                score = values[3] * 4;

                //SpillerTur.Firere = score;
            }
            else if (header == "Femmere")
            {
                if (scoreBoard.Femmere != null)
                {
                    throw new Exception(exceptionText);
                }

                score = values[4] * 5;

                //SpillerTur.Femmere = score;
            }
            else if (header == "Seksere")
            {
                if (scoreBoard.Seksere != null)
                {
                    throw new Exception(exceptionText);
                }

                score = values[5] * 6;

                //SpillerTur.Seksere = score;
            }
            else if (header == "EtPar")
            {
                if (scoreBoard.EtPar != null)
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
                if (scoreBoard.ToPar != null)
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
                if (scoreBoard.TreEns != null)
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
                if (scoreBoard.FireEns != null)
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
                if (scoreBoard.LilleStraight != null)
                {
                    throw new Exception(exceptionText);
                }

                if (values[0] >= 1 && values[1] >= 1 &&
                    values[2] >= 1 && values[3] >= 1 &&
                    values[4] >= 1)
                {
                    score = 1 + 2 + 3 + 4 + 5;
                }

                //SpillerTur.LilleStraight = score;
            }
            else if (header == "StorStraight")
            {
                if (scoreBoard.StorStraight != null)
                {
                    throw new Exception(exceptionText);
                }

                if (values[1] >= 1 && values[2] >= 1 &&
                    values[3] >= 1 && values[4] >= 1 &&
                    values[5] >= 1)
                {
                    score = 2 + 3 + 4 + 5 + 6;
                }

                //SpillerTur.StorStraight = score;
            }
            else if (header == "Hus")
            {
                if (scoreBoard.Hus != null)
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
                if (ints[0] > -1)
                {
                    for (int i = 0; i < values.Length; i++)
                    {
                        if (values[i] == 3 && ints[0] != i)
                        {
                            ints[1] = i;

                            if (ints[1] > -1)
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
                if (scoreBoard.Chance != null)
                {
                    throw new Exception(exceptionText);
                }

                // calculate'ing horse + moo
                for (int i = 0; i < terninger.Count; i++)
                {
                    score += terninger[i].DiceValue;
                }

                //SpillerTur.Chance = score;
            }
            else if (header == "Yatzy")
            {
                if (SpillerTur.ScoreBoard.Yatzy != null)
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
                for (int i = 0; i < terninger.Count; i++)
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
