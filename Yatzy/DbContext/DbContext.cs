using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yatzy.YatzyDbContext
{
    public class Model : DbContext
    {
        public DbSet<Spiller> SpillerTabel { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder options) { options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Yatzy; Trusted_Connection = True; "); }
    }

    public class Spiller : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public Spiller(int id, string navn)
        {
            Id = id;
            Navn = navn;
        }

        public int Id { get; set; }
        public string Navn { get; set; }

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
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Enere)));
            }
        }
        public int? Toere { get; set; } = null;
        public int? Treere { get; set; } = null;
        public int? Firere { get; set; } = null;
        public int? Femmere { get; set; } = null;
        public int? Seksere { get; set; } = null;
        public int SUM { get; set; }
        public int? Bonus { get; set; } = null;
        public int? EtPar { get; set; } = null;
        public int? ToPar { get; set; } = null;
        public int? TreEns { get; set; } = null;
        public int? FireEns { get; set; } = null;
        public int? LilleStraight { get; set; } = null;
        public int? StorStraight { get; set; } = null;
        public int? Hus { get; set; } = null;
        public int? Chance { get; set; } = null;
        public int? Yatzy { get; set; } = null;
        public int TotalSum { get; set; }
    }
}
