namespace Rydo.Storage.UnitTest
{
    using System;
    using Storage.Attributes;

    [TableStorage("dummy-model")]
    public class DummyModel
    {
        public string AccountNumber { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public decimal AccountBalance { get; set; }
        public DateTime BirthDate { get; set; }
    }
}