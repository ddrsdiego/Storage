namespace Rydo.Storage.Sample.Core.Domain.CustomerAggregate
{
    public readonly struct CustomerName
    {
        public CustomerName(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public string FirstName { get; }
        public string LastName { get; }
        public string Name => $"{FirstName} {LastName}";

        public override string ToString() => Name;
    }
}