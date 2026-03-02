namespace codingfreaks.OtelDemo.Repositories.Mock
{
    using System.Collections.ObjectModel;
    using System.Diagnostics;

    using Logic.Interfaces;
    using Logic.Models;
    using Logic.OpenTelemetry;

    public class PeopleRepository : IPeopleRepository
    {
        #region constants

        private static readonly List<Person> _people = new();

        #endregion

        #region explicit interfaces

        /// <inheritdoc />
        public ValueTask<Person> AddAsync(PersonCreateModel data)
        {
            var newPerson = new Person
            {
                Id = !_people.Any() ? 1 : _people.Max(p => p.Id) + 1,
                Firstname = data.Firstname,
                Lastname = data.Lastname
            };
            _people.Add(newPerson);
            Meters.PeopleCounter?.Add(1);
            Activity.Current?.SetTag("id", newPerson.Id);
            return ValueTask.FromResult(newPerson);
        }

        /// <inheritdoc />
        public ValueTask<ReadOnlyCollection<Person>> GetAsync()
        {
            return ValueTask.FromResult(_people.AsReadOnly());
        }

        #endregion
    }
}