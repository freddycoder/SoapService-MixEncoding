using SoapCore;
using SoapService;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSoapCore()
                .AddTransient<IContractService, Contract>()
                .AddMvc();

var app = builder.Build();

app.UseDeveloperExceptionPage()
   .Use(async (context, next) =>
   {
       context.Request.EnableBuffering();

       using var streamReader = new StreamReader(context.Request.Body);

       var body = await streamReader.ReadToEndAsync();

       var builder = new StringBuilder();

       var two_spaces_array = new byte[2];

       for (int i = 0; i < body.Length; i++)
       {
           var c = (byte)body[i];

           if (c >= 0xC2 && c <= 0xC3 && 
               i + 1 < body.Length &&
               body[i + 1] >= 0x80 && body[i + 1] <= 0xBF)
           {
               two_spaces_array[0] = c;
               two_spaces_array[1] = (byte)body[i + 1];
               var s = Encoding.UTF8.GetString(two_spaces_array);
               builder.Append(s);
               i++;
           }
           else
           {
               var c3 = (char)c;
               builder.Append(c3);
           }
       }

       var message = builder.ToString();
       context.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes(message));

       await next();
   })
   .UseRouting()
   .UseEndpoints(endpoints =>
   {
       endpoints.UseSoapEndpoint<IContractService>(o =>
       {
           o.SoapSerializer = SoapSerializer.XmlSerializer;
           o.IndentXml = true;
           o.Path = "/Endpoint.svc";
       });
   });

app.Run();
