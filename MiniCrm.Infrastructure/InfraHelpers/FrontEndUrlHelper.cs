using Microsoft.AspNetCore.WebUtilities;
using MiniCrm.Core.Interfaces;

namespace MiniCrm.Infrastructure.InfraHelpers
{
    public sealed class FrontEndUrlHelper : IFrontEndUrlHelper
    {
        private readonly Uri _baseUrl;

        private FrontEndUrlHelper(Uri baseUrl)
        {
            _baseUrl = baseUrl;
        }

        public string Link(string path, Dictionary<string, string?> queryParameters)
        {
            var urlBuilder = new UriBuilder(_baseUrl)
            {
                Path = path
            };
            var fullUrl = QueryHelpers.AddQueryString(urlBuilder.ToString(), queryParameters);
            return fullUrl!;
        }

        public static IFrontEndUrlHelper Create(string baseUrl) => new FrontEndUrlHelper(new Uri(baseUrl));
    }

}
