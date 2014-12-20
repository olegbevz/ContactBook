using System;
using System.Collections.Generic;
using System.Linq;
using ContactBook.Models;
using ContactBook.Repositories;

namespace ContactBook.Performance
{
    public class PerformanceCalculator
    {
        private readonly RepositoryFactory repositoryFactory = new RepositoryFactory();

        public IEnumerable<PerformanceResult> CalculatePerformance(int recordsCount, RepositoryType[] reporositoryTypes)
        {
            var performanceResults = new List<PerformanceResult>();

            foreach (var repositoryType in reporositoryTypes)
            {
                var performancdResult = CalculatePerformance(recordsCount, repositoryType);

                performanceResults.Add(performancdResult);
            }

            return performanceResults;
        }

        public PerformanceResult CalculatePerformance(int recordsCount, RepositoryType repositoryType)
        {
            var performanceResult = new PerformanceResult { RepositoryType = repositoryType };

            var repository = repositoryFactory.CreateRepository(repositoryType);
            if (!repository.Exist())
                repository.Create();

            var contacts = Enumerable.Range(0, recordsCount)
                .Select(index => new Contact { Name = "John", Address = "Moscow", Phone = "89234564532" })
                .ToArray();

            performanceResult.InsertRecordsDuration = InsertRecords(repository, contacts);

            performanceResult.SelectRecordsDuration = SelectRecords(repository, out contacts);

            performanceResult.UpdateRecordsDuration = UpdateRecords(repository, contacts);

            performanceResult.DeleteRecordsDuration = DeleteRecords(repository, contacts);

            return performanceResult;
        }

        private TimeSpan InsertRecords(IContactRepository repository, Contact[] contacts)
        {
            using (var stopWatch = new StopWatchCalculator())
            {
                for (int i = 0; i < contacts.Length; i++)
                {
                    repository.Add(contacts[i]);
                }

                return stopWatch.ElapsedTime;
            }
        }

        private TimeSpan SelectRecords(IContactRepository repository, out Contact[] contacts)
        {
            using (var stopWatch = new StopWatchCalculator())
            {
                contacts = repository.ToArray();

                return stopWatch.ElapsedTime;
            }
        }

        private TimeSpan UpdateRecords(IContactRepository repository, Contact[] contacts)
        {
            foreach (var contact in contacts)
            {
                contact.Name = "Oleg";
                contact.Address = "Tomsk";
                contact.Phone = "89234151708";
            }

            using (var stopWatch = new StopWatchCalculator())
            {
                for (int i = 0; i < contacts.Length; i++)
                {
                    repository.Update(contacts[i]);
                }

                return stopWatch.ElapsedTime;
            }
        }

        private TimeSpan DeleteRecords(IContactRepository repository, Contact[] contacts)
        {
            using (var stopWatch = new StopWatchCalculator())
            {
                for (int i = 0; i < contacts.Length; i++)
                {
                    repository.Remove(contacts[i].Id);
                }

                return stopWatch.ElapsedTime;
            }
        }
    }
}