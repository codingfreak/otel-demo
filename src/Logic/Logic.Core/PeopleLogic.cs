namespace codingfreaks.OtelDemo.Logic.Core
{
    using System.Collections.ObjectModel;

    using Interfaces;

    using Microsoft.Extensions.Logging;

    using Models;

    public class PeopleLogic : IPeopleLogic
    {
        #region constructors and destructors

        public PeopleLogic(ILogger<PeopleLogic> logger, IPeopleRepository repository)
        {
            Logger = logger;
            Repository = repository;
        }

        #endregion

        #region explicit interfaces

        /// <inheritdoc />
        public async ValueTask<Person> AddAsync(PersonCreateModel data)
        {
            var result = await Repository.AddAsync(data);
            Logger.LogInformation("Person with id {PersonId} created.", result.Id);
            return result;
        }

        /// <inheritdoc />
        public async ValueTask<ReadOnlyCollection<Person>> GetAsync()
        {
            var result = await Repository.GetAsync();
            Logger.LogInformation("Retrieved {PersonCount} people.", result.Count);
            return result;
        }

        #endregion

        #region properties

        private ILogger<PeopleLogic> Logger { get; }

        private IPeopleRepository Repository { get; }

        #endregion
    }
}