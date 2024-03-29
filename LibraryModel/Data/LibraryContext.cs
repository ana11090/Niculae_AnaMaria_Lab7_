﻿using Microsoft.EntityFrameworkCore;
//using Niculae_AnaMaria_Lab2.Models;
//using Niculae_AnaMaria_Lab2.Models.LibraryViewModels;
using LibraryModel.Data;
using LibraryModel.Models;

namespace LibraryModel.Data
{
    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options) :
       base(options)
        {
        }


        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<PublishedBook> PublishedBooks { get; set; }
        public DbSet<City> Cities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().ToTable("Customer");
            modelBuilder.Entity<Order>().ToTable("Order");
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Author)
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.AuthorID).OnDelete(DeleteBehavior.Cascade); ;
            modelBuilder.Entity<Author>().ToTable("Author");
            modelBuilder.Entity<Publisher>().ToTable("Publisher");
            modelBuilder.Entity<PublishedBook>().ToTable("PublishedBook");
            modelBuilder.Entity<PublishedBook>()
            .HasKey(c => new { c.BookID, c.PublisherID });//configureaza cheia primara compusa
            modelBuilder.Entity<Customer>().HasOne(c => c.City).WithMany(c => c.Customers).HasForeignKey(c => c.CityID);


        }

    }
}
