namespace codingfreaks.OtelDemo.Logic.Core
{
    using Interfaces;

    public class WebLogic : IWebLogic
    {
        #region constructors and destructors

        public WebLogic(HttpClient client)
        {
            Client = client;
        }

        #endregion

        #region explicit interfaces

        /// <inheritdoc />
        public async ValueTask<int> CountSiteBytesAsync(string url)
        {
            var response = await Client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsByteArrayAsync();
            return content.Length;
        }

        #endregion

        #region properties

        private HttpClient Client { get; }

        #endregion
    }
}