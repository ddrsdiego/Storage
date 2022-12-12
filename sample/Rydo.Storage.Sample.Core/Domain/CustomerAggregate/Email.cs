namespace Rydo.Storage.Sample.Core.Domain.CustomerAggregate
{
    public readonly struct Email
    {
        public Email(string address)
        {
            Address = address;
        }
        
        public string Address { get; }

        public bool IsValid => true;
        
        public bool IsNotValid => !IsValid;
    }
}