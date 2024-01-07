using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryModel.Models
{
    public class City
    {
        public int ID { get; set; }
        public string CityName { get; set; }

        // Navigation property
        public List<Customer> Customers { get; set; }
    }
}
