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
        public int[] Score { get; set; } = new int[18];
    }
}
