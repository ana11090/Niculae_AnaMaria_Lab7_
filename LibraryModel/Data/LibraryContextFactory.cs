using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryModel.Data
{
    public class LibraryContextFactory : IDesignTimeDbContextFactory<LibraryContext>
    {
        public LibraryContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<LibraryContext>();
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=LibraryNew;TrustServerCertificate=True;Trusted_Connection=True;MultipleActiveResultSets=true");

            return new LibraryContext(optionsBuilder.Options);
        }
    }

}
