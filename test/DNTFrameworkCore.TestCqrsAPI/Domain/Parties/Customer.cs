using DNTFrameworkCore.Domain;

namespace DNTFrameworkCore.TestCqrsAPI.Domain.Parties
{
    public class Customer : AggregateRoot, IRowVersion, INumberedEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public byte[] Version { get; set; }
        public string Number { get; set; }
    }
}