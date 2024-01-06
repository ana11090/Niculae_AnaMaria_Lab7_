using System.ComponentModel.DataAnnotations.Schema;

namespace Niculae_AnaMaria_Lab2.Models
{
    public class Author
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
         
        public List<Book> Books { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }
}

