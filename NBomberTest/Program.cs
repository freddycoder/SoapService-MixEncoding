using NBomber.Contracts;
using NBomber.CSharp;
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
        ? Response.Ok(xml, (int)response.StatusCode)
        : Response.Fail();
});


var scenario = ScenarioBuilder.CreateScenario("load", step);

NBomberRunner
    .RegisterScenarios(scenario)
    .Run();