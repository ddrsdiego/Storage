namespace Rydo.Storage.Sample.Core.Domain.CustomerAggregate
{
    using System;

    public readonly struct AccountNumber
    {
        public AccountNumber(string number, int digit)
        {
            Number = number;
            Digit = digit;
        }

        public string Number { get; }
        public int Digit { get; }
        public string Value => $"{Number}{Digit.ToString()}";
        public override string ToString() => Value;
    }

    public class Customer
    {
        public Customer(AccountNumber accountNumber, CustomerName name, Email email, DateTime birthDate)
        {
            Name = name;
            Email = email;
            AccountNumber = accountNumber;
            Age = new CustomerAge(birthDate);
        }

        public AccountNumber AccountNumber { get; }
        public CustomerName Name { get; }
        public Email Email { get; }
        public CustomerAge Age { get; }
    }
}