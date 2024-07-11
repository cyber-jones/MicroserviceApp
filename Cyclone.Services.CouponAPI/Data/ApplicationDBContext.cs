using Cyclone.Services.CouponAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Cyclone.Services.CouponAPI.Data
{
	public class ApplicationDBContext: DbContext
	{
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options): base(options)
        {
        }

        public DbSet<Coupon> Coupons { get; set; }


		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);


			modelBuilder.Entity<Coupon>().HasData(new List<Coupon>()
			{
				new() { CouponId = Guid.NewGuid(), CouponCode = "TSR2535", DiscountAmount = 10, MinAmount = 20},
				new() { CouponId = Guid.NewGuid(), CouponCode = "DFQ6721", DiscountAmount = 15, MinAmount = 25}
			});
		}
	}
}
