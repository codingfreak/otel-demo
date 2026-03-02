# otel-demo

A full demo of an API sending telemetry using OpenTelemetry

# Quick start

After you cloned the code locally you should start the Aspire dashboard
as a container locally first:

```shell
docker run --rm -it `
    -p 18888:18888 `
    -p 4317:18889 `
    --name aspire-dashboard `
    mcr.microsoft.com/dotnet/aspire-dashboard:latest
```

In the output of this command you'll see a token URL for the Aspire Dashboard.

Now run the following command to start the API:

```
dotnet run --project src/Services/Services.CoreApi/Services.CoreApi.csproj
```

After that execute the following 3 commands and check the Aspire dashboard:

```
curl --location 'https://localhost:7135/api/People'
curl --location 'https://localhost:7135/api/People' `
--header 'Content-Type: application/json' `
--data '{ "firstName": "Klaus", "lastName": "Testmann" }'
curl --location 'https://localhost:7135/api/People'
```

In Aspire after some waiting (about 60s) you should see Traces with Logs and also under
`Metrics` a counter named `people.creations`.

If you want to test the app with Azure Monitor execute the following command:

```
dotnet user-secrets set APPLICATIONINSIGHTS_CONNECTION_STRING "YOUR_CONNECTION_STRING" --project src/Services/Services.CoreApi/Services.CoreApi.csproj
```

and then open the Live Metrics of your Application Insights. You will see data flowing.

## Traces

All responses if this API will contain 3 additional headers `X-Activity-TraceId`, `X-Activity-SpanId` and `X-Activity-Id`. The trace id
is the most important of these.