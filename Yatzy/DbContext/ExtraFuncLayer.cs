using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Windows.Controls;
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
        private void OnPropertyChanged()
        {
            RaisePropertyChanged(nameof(Spillere));
            RaisePropertyChanged(nameof(SpillerListe));
            RaisePropertyChanged(nameof(SpillerTur));
            RaisePropertyChanged(nameof(Spil));
            RaisePropertyChanged(nameof(SpilListe));
        }

        private Model model { get; set; }
        public FuncLayer()
        {
            LoadModel();
            OnPropertyChanged();
        }

        private void LoadModel()
        {
            model = new Model();
            model.SpilTabel.Load();
            model.SpillerSpil.Load();
            model.Spillere.Load();
            model.ScoreBoards.Load();
            model.Terninger.Load();
        }

        public ObservableCollection<Spil> SpilListe
        {
            get
            {
                return model.SpilTabel.Local.ToObservableCollection();
            }
        }
        public ObservableCollection<SpillerSpil> SpillerListe
        {
            get
            {
                if (Spil != null)
                {
                    return Spil.Spillere;
                }
                else
                {
                    return new ObservableCollection<SpillerSpil>();
                }
            }
        }

        public ObservableCollection<Spiller> Spillere
        {
            get
            {
                return model.Spillere.Local.ToObservableCollection();
            }
        }

        public Spil Spil { get; set; }
        public bool PlayerHasWon = false;
        public int CurrentPlayerIndex;
        public SpillerSpil SpillerTur
        {
            get
            {
                SpillerSpil? Spiller;
                try
                {
                    Spiller = Spil.Spillere[CurrentPlayerIndex];
                }
                catch (Exception)
                {
                    Spiller = null;
                }
                return Spiller;
            }
        }

        // Public methods

        public void StartSpil()
        {
            if (Spil == null || Spil.Spillere.Count <= 0)
            {
                throw new InvalidOperationException("Der er igen spillere");
            }

            CurrentPlayerIndex = Spil.SpillerTurIndex;

            RaisePropertyChanged(nameof(SpillerTur));
        }

        public Spil NytSpil()
        {
            Spil spil = Spil.CreateSpil();
            model.SpilTabel.Add(spil);
            model.SaveChanges();
            OnPropertyChanged();
            return spil;
        }

        public void LoadSpil(Spil spil, bool preventLoadModel)
        {
            if (spil == null)
            {
                throw new ArgumentNullException(nameof(spil));
            }

            if (!preventLoadModel)
            {
                model.Dispose();
                LoadModel();
            }

            Spil = spil;

            OnPropertyChanged();
        }

        public void StopSpil()
        {
            for (int i = 0; i < SpillerListe.Count; i++)
            {
                ScoreBoard oldScoreBoard = SpillerListe[i].ResetScoreBoard();
                model.ScoreBoards.Remove(oldScoreBoard);
            }

            CurrentPlayerIndex = 0;

            OnPropertyChanged();
        }

        public Spil GemSpil()
        {
            model.SaveChanges();
            OnPropertyChanged();

            return Spil;
        }

        public void TilføjSpillerTilSpil(Spiller spiller)
        {
            if (spiller == null)
            {
                throw new Exception("Ingen spiller valgt");
            }

            SpillerSpil spillerSpil = new(spiller);
            Spil.Spillere.Add(spillerSpil);
            model.SaveChanges();
            OnPropertyChanged();
        }

        public Spiller TilføjSpiller(string spillerNavn)
        {
            if (string.IsNullOrEmpty(spillerNavn))
            {
                throw new InvalidDataException("Spiller navnet kan ikke være tom");
            }

            // Update player name
            spillerNavn = FormaterSpillerNavn(spillerNavn);
            // lambda
            if (Spillere.FirstOrDefault(spiller => spiller.Navn == spillerNavn) != null)
            {
                throw new InvalidOperationException("Spiller navnet er allerede i brug");
            }
            Spiller spiller = new(0, spillerNavn);

            // Add new player to List
            Spillere.Add(spiller);
            model.SaveChanges();
            OnPropertyChanged();

            return spiller;
        }

        public Spiller GemSpiller(Spiller spiller, string spillerNavn)
        {
            if (spiller == null)
            {
                throw new InvalidOperationException("Spiller er ikke vælgt");
            }
            if (string.IsNullOrEmpty(spillerNavn))
            {
                throw new InvalidDataException("Spiller navnet kan ikke være tom");
            }

            // Update player name
            spillerNavn = FormaterSpillerNavn(spillerNavn);
            // lambda
            if (Spillere.FirstOrDefault(spiller => spiller.Navn == spillerNavn) != null)
            {
                throw new InvalidOperationException("Spiller navnet er allerede i brug");
            }

            spiller.Navn = spillerNavn;
            model.SaveChanges();
            OnPropertyChanged();

            return spiller;
        }

        // Fjern spiller HELT
        public Spiller FjernSpiller(Spiller spiller)
        {
            if (spiller == null)
            {
                throw new InvalidOperationException("Spiller er ikke vælgt");
            }

            // Remove player from List
            Spillere.Remove(spiller);
            model.SaveChanges();
            OnPropertyChanged();

            return spiller;
        }

        public Spil FjernActivSpil(Spil spil)
        {
            if (spil == null)
            {
                throw new InvalidOperationException("Der blev ikke vælgt et spil");
            }

            for (int i = spil.Spillere.Count; i > 0; i--)
            {
                model.ScoreBoards.Remove(spil.Spillere[i - 1].ScoreBoard);
                model.SpillerSpil.Remove(spil.Spillere[i - 1]);
            }
            for (int i = spil.Terninger.Count; i > 0; i--)
            {
                model.Terninger.Remove(spil.Terninger[i - 1]);
            }

            model.SpilTabel.Remove(spil);
            model.SaveChanges();
            this.Spil = null;
            OnPropertyChanged();

            return spil;
        }

        public void FjernSpillerFraSpil(SpillerSpil spiller)
        {
            if (spiller == null)
            {
                throw new Exception("Spiller er ikke vælgt");
            }

            // Remove player from Game
            Spil.Spillere.Remove(spiller);
            model.SaveChanges();
            OnPropertyChanged();
        }

        public SpillerSpil NæsteSpiller()
        {
            if ((SpillerListe.Count - 1) == CurrentPlayerIndex)
            {
                CurrentPlayerIndex = 0;
            }
            else
            {
                CurrentPlayerIndex += 1;
            }
            OnPropertyChanged();
            return SpillerTur;
        }

        public int Registrer(DataGridCellInfo cell, List<Terning> terninger)
        {
            string header = cell.Column.Header.ToString();

            int score = RegnHeaderValue(header);

            // Set property value by reflection
            PropertyInfo property = typeof(ScoreBoard).GetProperty(TrimString(header));
            property.SetValue(SpillerTur.ScoreBoard, score);

            if (Spil.HighestScorePlayer == null)
            {
                Spil.HighestScorePlayer = SpillerTur;
            }
            else if (SpillerTur.ScoreBoard.TotalSum > Spil.HighestScorePlayer.ScoreBoard.TotalSum)
            {
                Spil.HighestScorePlayer = SpillerTur;
            }

            if (SpillerListe.Count - 1 == CurrentPlayerIndex)
            {
                string[] headers = { "Enere", "Toere", "Treere", "Firere", "Femmere", "Seksere", "EtPar", "ToPar", "TreEns", "FireEns", "LilleStraight", "StorStraight", "Hus", "Chance", "Yatzy" };
                // check if Last player has a null header
                // If last player has no null value then end game
                int count = 0;
                for (int i = 0; i < headers.Length; i++)
                {
                    try
                    {
                        RegnHeaderValue(headers[i]);
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

            OnPropertyChanged();

            return score;
        }

        public int RegnHeaderValue(string header)
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
                for (int i = 0; i < Spil.Terninger.Count; i++)
                {
                    score += Spil.Terninger[i].DiceValue;
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
                for (int i = 0; i < Spil.Terninger.Count; i++)
                {
                    int diceValue = Spil.Terninger[i].DiceValue;
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

        // Private methods

        private string FormaterSpillerNavn(string spillerNavn)
        {
            string[] navnDele = spillerNavn.Split(" ");
            spillerNavn = string.Empty;
            for (int i = 0; i < navnDele.Length; i++)
            {
                if (spillerNavn != string.Empty)
                {
                    spillerNavn += " ";
                }
                spillerNavn += navnDele[i].First().ToString().ToUpper() + navnDele[i].Substring(1).ToLower();
            }
            return spillerNavn;
        }
    }
}
