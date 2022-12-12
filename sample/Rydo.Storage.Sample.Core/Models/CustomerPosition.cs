namespace Rydo.Storage.Sample.Core.Models
{
    using Attributes;

    [TableStorage("customers-positions")]
    public class CustomerPosition
    {
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
    }
}