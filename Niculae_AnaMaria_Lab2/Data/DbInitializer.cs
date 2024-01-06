using Microsoft.EntityFrameworkCore;
using Publisher = LibraryModel.Models.Publisher;
using LibraryModel.Models;
using LibraryModel.Data;

namespace Niculae_AnaMaria_Lab2.Data
{
    public class DbInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new
           LibraryContext(serviceProvider.GetRequiredService
            <DbContextOptions<LibraryContext>>()))
            {
                if (context.Books.Any())
                {
                    return; // BD a fost creata anterior


                }

                if (!context.Books.Any())
                {
                    // Add authors to the Author table
                    var authorMihailSadoveanu = new Author { FirstName = "Mihail", LastName = "Sadoveanu" };
                    var authorGeorgeCalinescu = new Author { FirstName = "George", LastName = "Calinescu" };
                    var authorMirceaEliade = new Author { FirstName = "Mircea", LastName = "Eliade" };

                    context.Authors.AddRange(authorMihailSadoveanu, authorGeorgeCalinescu, authorMirceaEliade);

                    context.SaveChanges(); // Save changes to generate Author IDs

                    // Add books with valid AuthorIDs
                    context.Books.AddRange(
                        new Book { Title = "Baltagul", AuthorID = authorMihailSadoveanu.ID, Price = Decimal.Parse("22") },
                        new Book { Title = "Enigma Otiliei", AuthorID = authorGeorgeCalinescu.ID, Price = Decimal.Parse("18") },
                        new Book { Title = "Maytrei", AuthorID = authorMirceaEliade.ID, Price = Decimal.Parse("27") }
                    );

                    // Add customers
                    context.Customers.AddRange(
                        new Customer
                        {
                            Name = "Popescu Marcela",
                            Adress = "Str. Plopilor, nr. 24",
                            BirthDate = DateTime.Parse("1979-09-01")
                        },
                        new Customer
                        {
                            Name = "Mihailescu Cornel",
                            Adress = "Str. Bucuresti, nr. 45, ap. 2",
                            BirthDate = DateTime.Parse("1969-07-08")
                        }
                    );

                    context.SaveChanges(); // Save changes to add books and customers
                }

                //!!Atentie in tabelel Books si Authors au fost introduse date in laboratorul
                //anterior.Ne vom asigura ca datele pe care dorim sa le introducem in Orders,
                //Publishers si PublishedBook sunt consistente
                var orders = new Order[]
                {
                         new Order{CustomerID=1,BookID=1,OrderDate=DateTime.Parse("2021-02-25")},
                         new Order{CustomerID=2,BookID=2,OrderDate=DateTime.Parse("2021-02-25")}
                };
                foreach (Order e in orders)
                {
                    context.Orders.Add(e);
                }
                context.SaveChanges();

                var publishers = new Publisher[]
                {
                     new Publisher{PublisherName="Humanitas",Adress="Str. Aviatorilor, nr. 40, Bucuresti"},
                     new Publisher{PublisherName="Nemira",Adress="Str. Plopilor, nr. 35,Ploiesti"},
                     new Publisher{PublisherName="Paralela 45",Adress="Str. Cascadelor, nr.22, Cluj-Napoca"},
                };
                foreach (Publisher p in publishers)
                {
                    context.Publishers.Add(p);
                }
                context.SaveChanges();
                var books = context.Books;
                var publishedbooks = new PublishedBook[]
                {
                 new PublishedBook {
                 BookID = books.Single(c => c.Title == "Maytrei" ).ID,
                PublisherID = publishers.Single(i => i.PublisherName == "Humanitas").ID
                 },
                 new PublishedBook {
                 BookID = books.Single(c => c.Title == "Enigma Otiliei" ).ID,
                PublisherID = publishers.Single(i => i.PublisherName == "Humanitas").ID
                 },
                 new PublishedBook {
                 BookID = books.Single(c => c.Title == "Baltagul" ).ID,
                PublisherID = publishers.Single(i => i.PublisherName == "Nemira").ID
                 }
                };
                foreach (PublishedBook pb in publishedbooks)
                {
                    context.PublishedBooks.Add(pb);
                }
                context.SaveChanges();
            }
        }
    }
}