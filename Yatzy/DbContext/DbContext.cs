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
        private int enere;
        public int Enere 
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
        public int Toere { get; set; }
        public int Treere { get; set; }
        public int Firere { get; set; }
        public int Femmere { get; set; }
        public int Seksere { get; set; }
        public int SUM { get; set; }
        public int Bonus { get; set; }
        public int EtPar { get; set; }
        public int ToPar { get; set; }
        public int TreEns { get; set; }
        public int FireEns { get; set; }
        public int LilleStraight { get; set; }
        public int StorStraight { get; set; }
        public int Hus { get; set; }
        public int Chance { get; set; }
        public int Yatzy { get; set; }
        public int TotalSum { get; set; }
    }
}
