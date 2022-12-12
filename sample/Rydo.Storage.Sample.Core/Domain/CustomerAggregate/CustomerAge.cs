namespace Rydo.Storage.Sample.Core.Domain.CustomerAggregate
{
    using System;

    public readonly struct CustomerAge
    {
        private const int MajorAge = 18;
        
        public CustomerAge(DateTime birthDate)
        {
            BirthDate = birthDate;
        }

        public DateTime BirthDate { get; }

        public bool IsMajor => Age >= MajorAge;
        public bool IsNotMajor => !IsMajor;
        
        public int Age
        {
            get
            {
                var today = DateTime.Today;
                var age = today.Year - BirthDate.Year;

                return BirthDate.Date > today.AddYears(-age)
                    ? age--
                    : age;
            }
        }
    }
}