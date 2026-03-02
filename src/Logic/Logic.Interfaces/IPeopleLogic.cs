namespace codingfreaks.OtelDemo.Logic.Interfaces
{
    using System.Collections.ObjectModel;

    using Models;

    public interface IPeopleLogic
    {
        #region methods

        ValueTask<Person> AddAsync(PersonCreateModel data);

        ValueTask<ReadOnlyCollection<Person>> GetAsync();

        #endregion
    }
}