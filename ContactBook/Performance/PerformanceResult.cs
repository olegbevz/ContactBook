using System;
using ContactBook.Repositories;

namespace ContactBook.Performance
{
    public class PerformanceResult
    {
        public DatabaseType RepositoryType { get; set; }

        public TimeSpan InsertRecordsDuration { get; set; }

        public double InsertRecordsMemoryUsage { get; set; }

        public TimeSpan SelectRecordsDuration { get; set; }

        public double SelectRecordsMemoryUsage { get; set; }

        public TimeSpan UpdateRecordsDuration { get; set; }

        public double UpdateRecordsMemoryUsage { get; set; }

        public TimeSpan DeleteRecordsDuration { get; set; }

        public double DeleteRecordsMemoryUsage { get; set; }
    }
}