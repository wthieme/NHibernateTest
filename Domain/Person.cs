using System;
using System.Collections.Generic;

namespace NHibTest.Domain {
    public class Person {
        public virtual int Id { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual ISet<Sale> Purchases { get; set; }

    }
}