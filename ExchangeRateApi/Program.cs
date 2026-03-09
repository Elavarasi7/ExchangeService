var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<ExchangeRateApiOptions>(
    builder.Configuration.GetSection("ExchangeRateApi"));
builder.Services.AddTransient<IExchangeRateProvider, ExchangeRateProvider>();
builder.Services.AddTransient<IFlurlBase, FlurlBase>();

var app = builder.Build();
    
{
    var opts = app.Services.GetRequiredService<IOptions<ExchangeRateApiOptions>>().Value;
    FlurlHttp.ConfigureClientForUrl(opts.BaseUrl)
    .WithSettings(s => s.Timeout = TimeSpan.FromSeconds(10)); 
}


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();


app.UseRouting();

app.MapControllers();

app.Run();
