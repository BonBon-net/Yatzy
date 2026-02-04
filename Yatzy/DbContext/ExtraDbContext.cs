using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Yatzy.YatzyDbContext;

namespace Yatzy.YatzyDbContext
{
    public class Model : DbContext
    {
        //public DbSet<Spiller> SpillerTabel { get; set; }
        public DbSet<Spil> SpilTabel { get; set; }

        public DbSet<SpillerSpil> SpillerSpil { get; set; }
        public DbSet<Spiller> Spillere { get; set; }
        public DbSet<ScoreBoard> ScoreBoards { get; set; }
        public DbSet<Terning> Terninger { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder options) { options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Yatzy; Trusted_Connection = True; "); }
    }

    public class Spil : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        // Helper method to keep the setters clean
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public Spil() { }
        public Spil(DateTime dateTime)
        {
            DateTime = dateTime;
        }
        public Spil(int id, DateTime dateTime)
        {
            Id = id;
            DateTime = dateTime;
        }

        static public Spil CreateSpil()
        {
            Spil spil = new(DateTime.Now);

            spil.Terninger.Clear();
            for (int i = 0; i < 5; i++)
            {
                spil.Terninger.Add(new Terning());
            }

            return spil;
        }

        public int Id { get; set; }

        public ObservableCollection<SpillerSpil> Spillere { get; set; } = new ObservableCollection<SpillerSpil>();
        public List<Terning> Terninger { get; set; } = new List<Terning>();

        public DateTime DateTime { get; set; }
        public int Kasted { get; set; } = 0;
        public int SpillerTurIndex { get; set; } = 0;
        public int NullPlayerCount { get; set; } = 0;
        public SpillerSpil? HighestScorePlayer { get; set; } = null;
        public bool IsStarted { get; set; } = false;
    }
}

public class SpillerSpil : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    // Helper method to keep the setters clean
    protected void OnPropertyChanged(string name)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public SpillerSpil() { }

    public SpillerSpil(int id, Spiller spiller)
    {
        Id = id;
        Spiller = spiller;
        scoreBoard = new ScoreBoard();
    }

    public SpillerSpil(Spiller spiller)
    {
        Id = 0;
        Spiller = spiller;
        scoreBoard = new ScoreBoard();
    }

    public SpillerSpil(int id, string spillerNavn)
    {
        Id = id;
        Spiller = new Spiller(0, spillerNavn);
        scoreBoard = new ScoreBoard();
    }

    public int Id { get; set; }

    public string Navn
    {
        get
        {
            return Spiller.Navn;
        }
    }

    private Spiller spiller;
    public Spiller Spiller
    {
        get
        {
            return spiller;
        }
        set
        {
            spiller = value;
            OnPropertyChanged(nameof(Spiller));
        }
    }
    private ScoreBoard scoreBoard = new();
    public ScoreBoard ScoreBoard
    {
        get
        {
            return scoreBoard;
        }
        set
        {
            scoreBoard = value;
            OnPropertyChanged(nameof(scoreBoard));
        }
    }

    public ScoreBoard ResetScoreBoard()
    {
        ScoreBoard scoreBoard = ScoreBoard;
        ScoreBoard = new();
        return scoreBoard;
    }

    public bool HasPlayerNullScoreBoardValue()
    {
        ScoreBoard scoreBoard = ScoreBoard;

        if (scoreBoard.Enere == null || scoreBoard.Toere == null ||
            scoreBoard.Treere == null || scoreBoard.Firere == null ||
            scoreBoard.Femmere == null || scoreBoard.Seksere == null ||
            scoreBoard.EtPar == null || scoreBoard.ToPar == null ||
            scoreBoard.TreEns == null || scoreBoard.FireEns == null ||
            scoreBoard.LilleStraight == null || scoreBoard.StorStraight == null ||
            scoreBoard.Hus == null || scoreBoard.Chance == null || scoreBoard.Yatzy == null)
        {
            return true;
        }

        return false;
    }
}

public class Terning : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public static Random rnd = new();

    public int Id { get; set; }

    public int DiceValue { get; set; } = rnd.Next(1, 7);
    public bool IsHeld { get; set; } = false;
}

public class Spiller : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    // Helper method to keep the setters clean
    protected void OnPropertyChanged(string name)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public Spiller() { }
    public Spiller(int id, string navn)
    {
        Id = id;
        Navn = navn;
    }

    public int Id { get; set; }

    private string navn = string.Empty;
    public string Navn
    {
        get
        {
            return navn;
        }
        set
        {
            navn = value;
            OnPropertyChanged(nameof(navn));
        }
    }
}

public class ScoreBoard : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    // Helper method to keep the setters clean
    protected void OnPropertyChanged(string name)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public ScoreBoard()
    {
        ScoreBoardId = 0;
    }

    public int ScoreBoardId { get; set; }

    private int? enere = null;
    public int? Enere
    {
        get
        {
            return enere;
        }
        set
        {
            enere = value;
            OnPropertyChanged(nameof(Enere));
            OnPropertyChanged(nameof(BonusValue));
            OnPropertyChanged(nameof(TotalSum));
        }
    }

    private int? toere = null;
    public int? Toere
    {
        get
        {
            return toere;
        }
        set
        {
            toere = value;
            OnPropertyChanged(nameof(Toere));
            OnPropertyChanged(nameof(BonusValue));
            OnPropertyChanged(nameof(TotalSum));
        }
    }

    private int? treere = null;
    public int? Treere
    {
        get
        {
            return treere;
        }
        set
        {
            treere = value;
            OnPropertyChanged(nameof(Treere));
            OnPropertyChanged(nameof(BonusValue));
            OnPropertyChanged(nameof(TotalSum));
        }
    }

    private int? firere = null;
    public int? Firere
    {
        get
        {
            return firere;
        }
        set
        {
            firere = value;
            OnPropertyChanged(nameof(Firere));
            OnPropertyChanged(nameof(BonusValue));
            OnPropertyChanged(nameof(TotalSum));
        }
    }

    private int? femmere = null;
    public int? Femmere
    {
        get
        {
            return femmere;
        }
        set
        {
            femmere = value;
            OnPropertyChanged(nameof(Femmere));
            OnPropertyChanged(nameof(BonusValue));
            OnPropertyChanged(nameof(TotalSum));
        }
    }

    private int? seksere = null;
    public int? Seksere
    {
        get
        {
            return seksere;
        }
        set
        {
            seksere = value;
            OnPropertyChanged(nameof(Seksere));
            OnPropertyChanged(nameof(BonusValue));
            OnPropertyChanged(nameof(TotalSum));
        }
    }

    private int? BudgetValue = 0;
    private int? SumValue = 0;
    private int bonus;
    public string BonusValue
    {
        get
        {
            BudgetValue = GetBudgetValue();
            SumValue = GetSumValue();
            if (SumValue >= 63 && bonus == 0)
            {
                bonus = 50;
                OnPropertyChanged(nameof(Bonus));
            }
            if (BudgetValue >= 0)
            {
                return $"({BudgetValue}+) {SumValue}";
            }
            else
            {
                return $"({BudgetValue - BudgetValue - BudgetValue}-) {SumValue}";
            }
        }
        set
        {
            OnPropertyChanged(nameof(BonusValue));
            OnPropertyChanged(nameof(TotalSum));
        }
    }
    public int Bonus
    {
        get
        {
            return bonus;
        }
        set
        {
            bonus = value;
            OnPropertyChanged(nameof(Bonus));
            OnPropertyChanged(nameof(TotalSum));
        }
    }

    private int? etPar = null;
    public int? EtPar
    {
        get
        {
            return etPar;
        }
        set
        {
            etPar = value;
            OnPropertyChanged(nameof(EtPar));
            OnPropertyChanged(nameof(TotalSum));
        }
    }

    private int? toPar = null;
    public int? ToPar
    {
        get
        {
            return toPar;
        }
        set
        {
            toPar = value;
            OnPropertyChanged(nameof(ToPar));
            OnPropertyChanged(nameof(TotalSum));
        }
    }

    private int? treEns = null;
    public int? TreEns
    {
        get
        {
            return treEns;
        }
        set
        {
            treEns = value;
            OnPropertyChanged(nameof(TreEns));
            OnPropertyChanged(nameof(TotalSum));
        }
    }

    private int? fireEns = null;
    public int? FireEns
    {
        get
        {
            return fireEns;
        }
        set
        {
            fireEns = value;
            OnPropertyChanged(nameof(FireEns));
            OnPropertyChanged(nameof(TotalSum));
        }
    }

    private int? lilleStraight = null;
    public int? LilleStraight
    {
        get
        {
            return lilleStraight;
        }
        set
        {
            lilleStraight = value;
            OnPropertyChanged(nameof(LilleStraight));
            OnPropertyChanged(nameof(TotalSum));
        }
    }

    private int? storStraight = null;
    public int? StorStraight
    {
        get
        {
            return storStraight;
        }
        set
        {
            storStraight = value;
            OnPropertyChanged(nameof(StorStraight));
            OnPropertyChanged(nameof(TotalSum));
        }
    }

    private int? hus = null;
    public int? Hus
    {
        get
        {
            return hus;
        }
        set
        {
            hus = value;
            OnPropertyChanged(nameof(Hus));
            OnPropertyChanged(nameof(TotalSum));
        }
    }

    private int? chance = null;
    public int? Chance
    {
        get
        {
            return chance;
        }
        set
        {
            chance = value;
            OnPropertyChanged(nameof(Chance));
            OnPropertyChanged(nameof(TotalSum));
        }
    }

    private int? yatzy = null;
    public int? Yatzy
    {
        get
        {
            return yatzy;
        }
        set
        {
            yatzy = value;
            OnPropertyChanged(nameof(Yatzy));
            OnPropertyChanged(nameof(TotalSum));
        }
    }

    private int? totalSum = 0;
    public int? TotalSum
    {
        get
        {
            totalSum = ((Enere ?? 0) + (Toere ?? 0) + (Treere ?? 0) + (Firere ?? 0) + (Femmere ?? 0) + (Seksere ?? 0) + (Bonus) + (EtPar ?? 0) + (ToPar ?? 0) + (TreEns ?? 0) + (FireEns ?? 0) + (LilleStraight ?? 0) + (StorStraight ?? 0) + (Hus ?? 0) + (Chance ?? 0) + (Yatzy ?? 0));
            return totalSum;
        }
        set
        {
            totalSum = value;
            OnPropertyChanged(nameof(TotalSum));
        }
    }

    private int? GetBudgetValue()
    {
        int? BudgetValue = 0;
        if (Enere != null)
        {
            BudgetValue += Enere.Value - 3;
        }
        if (Toere != null)
        {
            BudgetValue += Toere.Value - 6;
        }
        if (Treere != null)
        {
            BudgetValue += Treere.Value - 9;
        }
        if (Firere != null)
        {
            BudgetValue += Firere.Value - 12;
        }
        if (Femmere != null)
        {
            BudgetValue += Femmere.Value - 15;
        }
        if (Seksere != null)
        {
            BudgetValue += Seksere.Value - 18;
        }
        return BudgetValue;
    }

    private int? GetSumValue()
    {
        int? SumValue = 0;
        if (Enere != null)
        {
            SumValue += Enere.Value;
        }
        if (Toere != null)
        {
            SumValue += Toere.Value;
        }
        if (Treere != null)
        {
            SumValue += Treere.Value;
        }
        if (Firere != null)
        {
            SumValue += Firere.Value;
        }
        if (Femmere != null)
        {
            SumValue += Femmere.Value;
        }
        if (Seksere != null)
        {
            SumValue += Seksere.Value;
        }
        return SumValue;
    }
}
