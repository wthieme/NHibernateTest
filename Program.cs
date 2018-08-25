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
            InitProducts (sf);
            var products = GetProducts (sf);
            foreach (var p in products) {
                Console.WriteLine (p.Name);
            }
        }

        private static List<Product> GetProducts (ISessionFactory sf) {
            using (var session = sf.OpenSession ()) {
                var products =
                    session.Query<Product> ()
                    .Where (c => c.Category == "Transport")
                    .ToList ();
                return products;
            }
        }

        private static void InitProducts (ISessionFactory sf) {
            var products = new List<Product> ();
            products.Add (new Product { Name = "House", Category = "Buildings" });
            products.Add (new Product { Name = "Car", Category = "Transport" });
            products.Add (new Product { Name = "Bike", Category = "Transport" });

            using (var session = sf.OpenSession ())
            using (var trans = session.BeginTransaction ()) {
                foreach (var p in products) session.Save (p);
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