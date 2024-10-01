using System.Web;

namespace WeatherForecast.Host.Common;

public class QueryParameterHandler : DelegatingHandler
{
    private readonly string _parameterName;
    private readonly string? _parameterValue;

    public QueryParameterHandler(string parameterName, string? parameterValue)
    {
        _parameterName = parameterName;
        _parameterValue = parameterValue;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var uriBuilder = new UriBuilder(request.RequestUri!);
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);
        query[_parameterName] = _parameterValue;
        uriBuilder.Query = query.ToString();
        request.RequestUri = uriBuilder.Uri;

        return await base.SendAsync(request, cancellationToken);
    }
}
