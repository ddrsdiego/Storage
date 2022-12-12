namespace Rydo.Storage.Sample.Core.UnitTest
{
    using System;
    using Domain.CustomerAggregate;
    using NUnit.Framework;

    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var name = new CustomerName("Diego", "Dias Ribeiro da Silva");
            var newAccountNumber = new AccountNumber("509001", 6);

            var customer = new Customer(newAccountNumber, name, new Email("ddrsdiego@hotmail.com"),
                new DateTime(1982, 11, 22));

            var c = customer;
        }
    }
}