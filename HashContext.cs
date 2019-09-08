using System.Data.Entity;

namespace HashSumCalculation
{
    public class HashContext : DbContext
    {
        public HashContext() : base("MyConnection") { }

        public DbSet<HashSum> HashSums { get; set; }
    }
}
