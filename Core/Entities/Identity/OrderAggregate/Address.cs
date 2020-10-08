// 207
// Address is a value owned by an order.
namespace Core.Entities.OrderAggregate
{
    public class Address
    {
        // this constructor is need, or EF will complying
        public Address()
        {
        }

        public Address(string firstName, string lastName, string street, string city, string state, string zipcode)
        {
            FirstName = firstName;
            LastName = lastName;
            Street = street;
            City = city;
            State = state;
            Zipcode = zipcode;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
    }
}