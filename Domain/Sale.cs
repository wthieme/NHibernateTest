using System;

namespace NHibTest.Domain {
    public class Sale {
        public virtual Guid Id { get; set; }
        public virtual int PersonId { get; set; }
        public virtual int ProductId { get; set; }
        public virtual DateTime SaleDate { get; set; }
        public virtual Decimal Price { get; set; }
        public virtual Person Person { get; set; }
        public virtual Product Product { get; set; }
    }
}