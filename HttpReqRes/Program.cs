using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//app.MapGet("/", () => "Hello World!");

app.Run(async (HttpContext context) =>
{
    string path = context.Request.Path;
    string method = context.Request.Method;

    if (path == "/" || path == "/Home")
    {
        context.Response.StatusCode = 200;
        await context.Response.WriteAsync($"Welcome To Home Page.{Environment.NewLine}");
    }
    else if (path == "/Contact")
    {
        context.Response.StatusCode = 200;
        await context.Response.WriteAsync($"Welcome To Cotact Page.{Environment.NewLine}");
    }
    else if (method == "GET" && path == "/Product")
    {
        context.Response.StatusCode = 200;

        string id = "", name = "";

        if (context.Request.Query.ContainsKey("id") && context.Request.Query.ContainsKey("name"))
        {
            id = context.Request.Query["id"];
            name = context.Request.Query["name"];
            await context.Response.WriteAsync($"Your Product Id: {id} & Name: {name}");
        }
        else
        {
            await context.Response.WriteAsync($"Welcome To Product Page.{Environment.NewLine}There are no parameters.");
        }
    }
    else if (method == "POST" && path == "/Product")
    {
        context.Response.StatusCode = 200;
        StreamReader reader = new StreamReader(context.Request.Body);
        string raw = await reader.ReadToEndAsync();
        Dictionary<string, StringValues> dict = QueryHelpers.ParseQuery(raw);
        if (dict != null)
        {
            if (dict.ContainsKey("id"))
            {
                await context.Response.WriteAsync($"This Post Method and Id: {dict["id"]} {Environment.NewLine}");
            }
            if (dict.ContainsKey("name"))
            {
                foreach (var kvp in dict["name"])
                {
                    await context.Response.WriteAsync($"Name: {kvp.ToString()} {Environment.NewLine}");
                }
            }
        }
        else
            await context.Response.WriteAsync($"This Post Method.");
    }
    else
    {
        context.Response.StatusCode = 404;
        await context.Response.WriteAsync("The Page You Are Looking For Isn't Available.");
    }
});

app.Run();
