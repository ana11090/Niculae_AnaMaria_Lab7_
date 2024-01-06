using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
//using Niculae_AnaMaria_Lab2.Models.LibraryViewModels;
using LibraryModel.Models;

namespace LibraryModel.Models
{
    public class Book
    {
        //public int ID { get; set; }
        //public string Title { get; set; }
        //public string Author { get; set; }
        //public decimal Price { get; set; }
        //public ICollection<Order> Orders { get; set; }

        public int ID { get; set; }
        public string Title { get; set; }
        [Column(TypeName = "decimal(6, 2)")]

        public decimal Price { get; set; }

        //[ForeignKey("Author")]
        public int AuthorID { get; set; } 

        public Author Author { get; set; }

        public ICollection<Order> Orders { get; set; }
        public ICollection<PublishedBook> PublishedBooks { get; set; }

    }
}
