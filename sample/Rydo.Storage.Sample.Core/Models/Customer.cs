namespace Rydo.Storage.Sample.Core.Models
{
    using System;
    using Attributes;

    [TableStorage("customers")]
    public class Customer
    {
        public string AccountNumber { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public DateTime BirthDate { get; set; }
    }
}