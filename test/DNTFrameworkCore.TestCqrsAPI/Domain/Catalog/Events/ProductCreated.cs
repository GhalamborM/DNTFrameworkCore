using DNTFrameworkCore.Domain;

namespace DNTFrameworkCore.TestCqrsAPI.Domain.Catalog.Events
{
    public sealed class ProductCreated : IDomainEvent
    {
        public ProductCreated(Product product)
        {
            Product = product;
        }

        public Product Product { get;  }
    }
}