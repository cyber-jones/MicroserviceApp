using Cyclone.Services.ShoppingCartAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Cyclone.Services.ShoppingCartAPI.Data
{
	public class CartDbContext : DbContext
	{
        public CartDbContext(DbContextOptions<CartDbContext> options) : base(options)
        {   
        }

        public DbSet<CartDetails> CartDetails { get; set; }
        public DbSet<CartHeader> CartHeaders { get; set; }


		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
		}
	}
}
