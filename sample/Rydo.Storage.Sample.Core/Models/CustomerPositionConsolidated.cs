namespace Rydo.Storage.Sample.Core.Models
{
    using Attributes;

    [TableStorage("customers-positions-consolidated")]
    public class CustomerPositionConsolidated
    {
        public Customer Customer { get; set; }
        public CustomerPosition Position { get; set; }
    }
}