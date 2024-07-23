using ExcelFileStorage.Api.Services;
using ExcelFileStorage.Api.Services.IServices;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddHttpClient<IHttpbinReportBuilder, HttpbinReportBuilder>();

builder.Services.AddTransient<IFileOnServer, FileOnServer>();
builder.Services.AddTransient<IFileRebuilder, ExcelRebuilder>();
builder.Services.AddTransient<IHttpbinReportBuilder, HttpbinReportBuilder>();

builder.Services.AddScoped<IAppLogger, AppFileLogger>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
}).AddSwaggerGenNewtonsoftSupport();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
