using System.Security.Policy;

namespace Niculae_AnaMaria_Lab2.Models.LibraryViewModels
{
    public class PublishedBook
    {
        public int PublisherID { get; set; }
        public int BookID { get; set; }
        public Publisher Publisher { get; set; }
        public Book Book { get; set; }
    }
}
