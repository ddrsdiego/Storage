namespace Rydo.Storage.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CSharpFunctionalExtensions;
    using Exceptions;

    public static class RydoStorageExceptionExtensions
    {
        public static IEnumerable<Result> ThrowIfContainsFailure(this IEnumerable<Result> results)
        {
            var resultList = results.ToList();

            if (!resultList.Any())
                return resultList;
            
            var message = "The configuration is invalid:"
                          + Environment.NewLine
                          + string.Join(Environment.NewLine, resultList.Select(x => x.ToString()).ToArray());
            
            throw new ConfigurationException(resultList, message);
        }
    }
}