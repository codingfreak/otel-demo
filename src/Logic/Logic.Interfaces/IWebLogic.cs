namespace codingfreaks.OtelDemo.Logic.Interfaces
{
    public interface IWebLogic
    {
        #region methods

        ValueTask<int> CountSiteBytesAsync(string url);

        #endregion
    }
}