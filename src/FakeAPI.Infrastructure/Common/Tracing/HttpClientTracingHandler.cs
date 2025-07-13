public class HttpClientTracingHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.Content != null)
        {
            var body = await request.Content.ReadAsStringAsync(cancellationToken);
            request.Options.Set(new HttpRequestOptionsKey<string>("http.request.body"), body);
        }

        var response = await base.SendAsync(request, cancellationToken);

        if (response.Content != null)
        {
            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
            response.RequestMessage?.Options.Set(new HttpRequestOptionsKey<string>("http.response.body"), responseBody);
        }

        return response;
    }
}
