using System.Globalization;
using TetraLeagueOverlay.Api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddScoped<TetraLeagueApi>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMvc(options => options.EnableEndpointRouting = false).AddControllersAsServices();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin",
        b => b.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var app = builder.Build();

// Configure the desired culture
var cultureInfo = new CultureInfo("en-US");
cultureInfo.NumberFormat.NumberDecimalSeparator = ".";

// Set default culture
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

    app.UseCors("AllowAnyOrigin");
app.UseHttpsRedirection();

app.MapControllers();

app.Run();