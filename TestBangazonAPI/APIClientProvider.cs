using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using Xunit;
using __YOUR WEB API PROJECT NAMESPACE__;

namespace __YOUR TEST PROJECT NAMESPACE__
{
    class APIClientProvider : IClassFixture<WebApplicationFactory<Startup>>
{
    public HttpClient Client { get; private set; }
    private readonly WebApplicationFactory<Startup> _factory = new WebApplicationFactory<Startup>();

    public APIClientProvider()
    {
        Client = _factory.CreateClient();
    }

    public void Dispose()
    {
        _factory?.Dispose();
        Client?.Dispose();
    }
}
}