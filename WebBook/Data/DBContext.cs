using Microsoft.EntityFrameworkCore;
using WebBook.Models;

namespace WebBook.Data
{
    public class DBContext :DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {

        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Warehause> Warehauses { get; set; }
        public DbSet<Stock> Stocks { get; set; }
    }
}