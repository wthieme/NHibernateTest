using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using MySql.Data;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Criterion;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Linq;
using NHibTest.Domain;

namespace NHibTest {
    class Program {
        static void Main (string[] args) {
            var cfg = ConfigureNHibernate ();
            var sf = cfg.BuildSessionFactory ();
            //InitProducts (sf);
            //InitPersons (sf);
            InitSales (sf);
            ListSales (sf);
        }

        private static void ListSales (ISessionFactory sf) {
            using (var session = sf.OpenSession ()) {
                var sales =
                    session.Query<Sale> ()
                    .Select (s => new {
                        Wie = $"{s.Person.FirstName} {s.Person.LastName}",
                            Wat = s.Product.Name,
                            Prijs = s.Price,
                            Datum = s.SaleDate
                    })
                    .ToList ();

                foreach (var s in sales) {
                    Console.WriteLine ($"{s.Wie} kocht op {s.Datum} een {s.Wat} voor {s.Prijs} euro");
                }
            }
        }

        private static void InitSales (ISessionFactory sf) {
            using (var session = sf.OpenSession ()) {
                var sales = new List<Sale> ();
                var wilhelm = session.Query<Person> ()
                    .Single (p => p.FirstName == "Wilhelm" && p.LastName == "Thieme");
                var greetje = session.Query<Person> ()
                    .Single (p => p.FirstName == "Greetje" && p.LastName == "Meijer");
                var willem = session.Query<Person> ()
                    .Single (p => p.FirstName == "Willem" && p.LastName == "Thieme");
                var car = session.Query<Product> ()
                    .Single (p => p.Name == "Car");
                var bike = session.Query<Product> ()
                    .Single (p => p.Name == "Bike");

                sales.Add (new Sale {
                    Person = wilhelm,
                        Product = car,
                        SaleDate = DateTime.Now,
                        Price = 2150.56M
                });
                sales.Add (new Sale {
                    Person = wilhelm,
                        Product = bike,
                        SaleDate = DateTime.Now.AddDays (-50),
                        Price = 150.95M
                });
                sales.Add (new Sale {
                    Person = greetje,
                        Product = bike,
                        SaleDate = DateTime.Now.AddDays (-75),
                        Price = 195.95M
                });

                using (var trans = session.BeginTransaction ()) {
                    foreach (var s in sales) session.Save (s);
                    trans.Commit ();
                }
            }
        }
        private static void InitProducts (ISessionFactory sf) {
            var products = new List<Product> ();
            products.Add (new Product { Id = 1, Name = "House", Category = "Buildings" });
            products.Add (new Product { Id = 2, Name = "Car", Category = "Transport" });
            products.Add (new Product { Id = 3, Name = "Bike", Category = "Transport" });

            using (var session = sf.OpenSession ())
            using (var trans = session.BeginTransaction ()) {
                foreach (var p in products) session.Save (p);
                trans.Commit ();
            }
        }
        private static void InitPersons (ISessionFactory sf) {
            var persons = new List<Person> ();
            persons.Add (new Person { Id = 1, FirstName = "Wilhelm", LastName = "Thieme" });
            persons.Add (new Person { Id = 2, FirstName = "Greetje", LastName = "Meijer" });
            persons.Add (new Person { Id = 3, FirstName = "Willem", LastName = "Thieme" });
            persons.Add (new Person { Id = 4, FirstName = "Anne-Mieke", LastName = "Thieme" });
            persons.Add (new Person { Id = 5, FirstName = "Julia", LastName = "Thieme" });

            using (var session = sf.OpenSession ())
            using (var trans = session.BeginTransaction ()) {
                foreach (var p in persons) session.Save (p);
                trans.Commit ();
            }
        }
        private static Configuration ConfigureNHibernate () {

            var cfg = new Configuration ();
            cfg.Configure ("/home/wilhelm/Documents/nHibTest/Config/hibernate.cfg.xml");

            return cfg;
        }
    }
}