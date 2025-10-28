using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using UploadFiles.App;
using UploadFiles.Domain.Abstractions;
using UploadFiles.Infra;
using UploadFiles.Infra.Database;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApp();
builder.Services.AddInfra();

builder.Services.AddControllers().AddJsonOptions(x =>
{
    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    x.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    x.JsonSerializerOptions.WriteIndented = true;
    x.JsonSerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(UnicodeRanges.All);
});

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
})
    .AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

var versionsApi = new[] { "v1" };
foreach (var version in versionsApi)
{
    builder.Services.AddOpenApi(version, op =>
    {
        op.AddDocumentTransformer((document, context, _) =>
        {
            var provider = context.ApplicationServices
                .GetRequiredService<IApiVersionDescriptionProvider>();
            var desc = provider.ApiVersionDescriptions.FirstOrDefault(x => x.GroupName == version);

            document.Info.Version = desc?.ApiVersion.ToString();
            return Task.CompletedTask;
        });
    });
}

builder.Services.AddDbContext<UploadFilesDbContext>(op =>
{
    op.UseSqlite(
    UploadFilesDbConnection.SQL_CONNECTION,
    b => b.MigrationsAssembly("UploadFiles.Infra")
    .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
});




var app = builder.Build();
app.Use(async (context, next) =>
{
    await next();

    if (context.Response.StatusCode == StatusCodes.Status405MethodNotAllowed)
    {
        context.Response.ContentType = "application/json";
        var result = Result.Failure(Error.MethodNotAllowed("Metodo não implementado ou não permitido"));
        await context.Response.WriteAsJsonAsync(result.Error);
    }
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(opt =>
    {
        opt.AddDocuments("v1");
        opt.WithTitle("Upload Files")
           .WithTheme(ScalarTheme.BluePlanet)
           .WithDefaultHttpClient(ScalarTarget.JavaScript, ScalarClient.Axios);
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
