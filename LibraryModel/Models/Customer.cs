using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using LibraryModel.Models;

namespace LibraryModel.Models
{
    public class Customer
    {
        public int CustomerID { get; set; }
        public string Name { get; set; }
        public string Adress { get; set; }
        public DateTime BirthDate { get; set; }
        public ICollection<Order> Orders { get; set; }

        public int CityID { get; set; } // Cheie străină
        public City City { get; set; } // Navigation property
    }
}
