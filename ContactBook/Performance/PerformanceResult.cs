using System;
using ContactBook.Repositories;

namespace ContactBook.Performance
{
    public class PerformanceResult
    {
        public RepositoryType RepositoryType { get; set; }

        public TimeSpan InsertRecordsDuration { get; set; }

        public TimeSpan SelectRecordsDuration { get; set; }

        public TimeSpan UpdateRecordsDuration { get; set; }

        public TimeSpan DeleteRecordsDuration { get; set; }
    }
}