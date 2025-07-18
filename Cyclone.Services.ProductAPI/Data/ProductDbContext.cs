﻿using Cyclone.Services.ProductAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Cyclone.Services.ProductAPI.Data
{
	public class ProductDbContext : DbContext
	{
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
        {   
        }

        public DbSet<Product> Products { get; set; }


		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Product>().HasData(new Product
			{
				ProductId = Guid.NewGuid(),
				Name = "Samosa",
				Price = 15,
				Description = " Quisque vel lacus ac magna, vehicula sagittis ut non lacus.<br/> Vestibulum arcu turpis, maximus malesuada neque. Phasellus commodo cursus pretium.",
				ImageUrl = "https://placehold.co/603x403",
				CategoryName = "Appetizer"
			});
			modelBuilder.Entity<Product>().HasData(new Product
			{
				ProductId = Guid.NewGuid(),
				Name = "Paneer Tikka",
				Price = 13.99,
				Description = " Quisque vel lacus ac magna, vehicula sagittis ut non lacus.<br/> Vestibulum arcu turpis, maximus malesuada neque. Phasellus commodo cursus pretium.",
				ImageUrl = "https://placehold.co/602x402",
				CategoryName = "Appetizer"
			});
			modelBuilder.Entity<Product>().HasData(new Product
			{
				ProductId = Guid.NewGuid(),
				Name = "Sweet Pie",
				Price = 10.99,
				Description = " Quisque vel lacus ac magna, vehicula sagittis ut non lacus.<br/> Vestibulum arcu turpis, maximus malesuada neque. Phasellus commodo cursus pretium.",
				ImageUrl = "https://placehold.co/601x401",
				CategoryName = "Dessert"
			});
			modelBuilder.Entity<Product>().HasData(new Product
			{
				ProductId = Guid.NewGuid(),
				Name = "Pav Bhaji",
				Price = 15,
				Description = " Quisque vel lacus ac magna, vehicula sagittis ut non lacus.<br/> Vestibulum arcu turpis, maximus malesuada neque. Phasellus commodo cursus pretium.",
				ImageUrl = "https://placehold.co/600x400",
				CategoryName = "Entree"
			});
		}
	}
}
