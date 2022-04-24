using NBomber.Contracts;
using NBomber.CSharp;
using NBomber.Plugins.Network.Ping;
using System.Text;

var xmlCache = File.ReadAllText("template.xml");

string GetXml()
{
    return xmlCache;
}

using var httpClient = new HttpClient();

var step = Step.Create("send_xml", async context =>
{
    var xml = GetXml();

    var response = await httpClient.PostAsync("http://localhost:5180/Endpoint.svc", 
        new StringContent(xml, Encoding.UTF8, "text/xml"));

    return response.IsSuccessStatusCode
        ? Response.Ok(xml, (int)response.StatusCode, sizeBytes: xml.Length + (int)(response.Content.Headers.ContentLength ?? 0))
        : Response.Fail();
});


var scenario = ScenarioBuilder.CreateScenario("load", step)
                              .WithWarmUpDuration(TimeSpan.FromSeconds(30))
                              .WithLoadSimulations(
                                  Simulation.InjectPerSec(rate: 15, during: TimeSpan.FromSeconds(120))
                              );

var pingPluginConfig = PingPluginConfig.CreateDefault(new[] { "localhost:5180" });
var pingPlugin = new PingPlugin(pingPluginConfig);

NBomberRunner
    .RegisterScenarios(scenario)
    .WithWorkerPlugins(pingPlugin)
    .Run();