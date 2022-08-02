using DotLiquid;
using DotLiquid.FileSystems;
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
var currentDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
Template.FileSystem = new LocalFileSystem(currentDirectory);
app.MapGet("/", (HttpContext context) =>
{
    context.Response.Headers.Add("Cache-Control","no-cache");
    context.Response.ContentType = "text/html";
    var parseString = "{% include 'Home' %}";
    Template template = Template.Parse(parseString);
    return template.Render();
});

app.MapGet("/{slug}", (HttpContext context,String slug) =>
{
    context.Response.Headers.Add("Cache-Control","no-cache");
    context.Response.ContentType = "text/html";
    if (!File.Exists(Path.Combine(currentDirectory, "_"+slug+".liquid")))
    {
        context.Response.StatusCode = 404;
        return "404";
    }
    else
    {
        var parseString = "{% include '"+slug+"' %}";
        Template template = Template.Parse(parseString);
        return template.Render();
    }
});
app.Run();