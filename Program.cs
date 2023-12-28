using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Refit;
using System.Net.Http.Headers;
using YahooFinanceAgregator.Domain;
using YahooFinanceAgregator.ExternalClients;
using YahooFinanceAgregator.Services;
using YahooFinanceAgregator.Stores;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services
    .AddRefitClient<IYahooFinance>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.Configuration.GetValue<string>("YahooFinanceBaseAPI")));

builder.Services.AddScoped<TickerService>();
builder.Services.AddSingleton<YahooSessionStore>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins(builder.Configuration.GetValue<string>("WebUIBaseUrl"));
        });
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// App builder
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();