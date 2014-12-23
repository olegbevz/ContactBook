using System;
using System.Collections.Generic;
using System.Linq;
using ContactBook.Models;
using ContactBook.Repositories;

namespace ContactBook.Performance
{
    using System.Diagnostics;

    public class PerformanceCalculator
    {
        private readonly DatabaseFactory repositoryFactory = new DatabaseFactory();

        public IEnumerable<PerformanceResult> CalculatePerformance(int recordsCount, DatabaseType[] reporositoryTypes)
        {
            var performanceResults = new List<PerformanceResult>();

            foreach (var repositoryType in reporositoryTypes)
            {
                var performancdResult = CalculatePerformance(recordsCount, repositoryType);

                performanceResults.Add(performancdResult);
            }

            return performanceResults;
        }

        public PerformanceResult CalculatePerformance(int recordsCount, DatabaseType repositoryType)
        {
            var performanceResult = new PerformanceResult { RepositoryType = repositoryType };

            var database = repositoryFactory.GetDatabase(repositoryType);
            if (!database.Exist())
                database.Create();

            var contacts = Enumerable.Range(0, recordsCount)
                .Select(index => new Contact { Name = "John", Address = "Moscow", Phone = "89234564532" })
                .ToArray();

            TimeSpan duration;
            long memoryUsage;

            InsertRecords(database, contacts, out duration, out memoryUsage);
            performanceResult.InsertRecordsDuration = duration;
            performanceResult.InsertRecordsMemoryUsage = (double)memoryUsage / 1024;

            SelectRecords(database, out contacts, out duration, out memoryUsage);
            performanceResult.SelectRecordsDuration = duration;
            performanceResult.SelectRecordsMemoryUsage = (double)memoryUsage / 1024;

            UpdateRecords(database, contacts, out duration, out memoryUsage);
            performanceResult.UpdateRecordsDuration = duration;
            performanceResult.UpdateRecordsMemoryUsage = (double)memoryUsage / 1024;

            DeleteRecords(database, contacts, out duration, out memoryUsage);
            performanceResult.DeleteRecordsDuration = duration;
            performanceResult.DeleteRecordsMemoryUsage = (double)memoryUsage / 1024;

            return performanceResult;
        }

        private void InsertRecords(IDatabase database, Contact[] contacts, out TimeSpan duration, out long memoryUsage)
        {
            using (var stopWatch = new StopWatchCalculator())
            {
                using (var session = database.OpenSession())
                {
                    memoryUsage = this.GetCurrentMemoryUsage();

                    for (int i = 0; i < contacts.Length; i++)
                    {
                        session.ContactRepository.Add(contacts[i]);
                    }

                    memoryUsage = this.GetCurrentMemoryUsage() - memoryUsage;

                    session.Commit();
                }

                duration = stopWatch.ElapsedTime;
            }
        }

        private void SelectRecords(IDatabase database, out Contact[] contacts, out TimeSpan duration, out long memoryUsage)
        {
            using (var stopWatch = new StopWatchCalculator())
            {
                using (var session = database.OpenSession())
                {
                    memoryUsage = this.GetCurrentMemoryUsage();

                    contacts = session.ContactRepository.ToArray();

                    memoryUsage = this.GetCurrentMemoryUsage() - memoryUsage;
                }

                duration = stopWatch.ElapsedTime;
            }
        }

        private void UpdateRecords(IDatabase database, Contact[] contacts, out TimeSpan duration, out long memoryUsage)
        {
            foreach (var contact in contacts)
            {
                contact.Name = "Oleg";
                contact.Address = "Tomsk";
                contact.Phone = "89234151708";
            }

            using (var stopWatch = new StopWatchCalculator())
            {
                using (var session = database.OpenSession())
                {
                    memoryUsage = this.GetCurrentMemoryUsage();

                    for (int i = 0; i < contacts.Length; i++)
                    {
                        session.ContactRepository.Update(contacts[i]);
                    }

                    memoryUsage = this.GetCurrentMemoryUsage() - memoryUsage;

                    session.Commit();
                }

                duration = stopWatch.ElapsedTime;
            }
        }

        private void DeleteRecords(IDatabase database, Contact[] contacts, out TimeSpan duration, out long memoryUsage)
        {
            using (var stopWatch = new StopWatchCalculator())
            {
                using (var session = database.OpenSession())
                {
                    memoryUsage = this.GetCurrentMemoryUsage();

                    for (int i = 0; i < contacts.Length; i++)
                    {
                        session.ContactRepository.Remove(contacts[i].Id);
                    }

                    memoryUsage = this.GetCurrentMemoryUsage() - memoryUsage;

                    session.Commit();
                }

                duration = stopWatch.ElapsedTime;
            }
        }

        private long GetCurrentMemoryUsage()
        {
            var process = Process.GetCurrentProcess();

            return process.PrivateMemorySize64;
        }
    }
}