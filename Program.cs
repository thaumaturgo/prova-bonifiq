using Microsoft.EntityFrameworkCore;
using ProvaPub.Repository;
using ProvaPub.Services;
using ProvaPub.Services.Interfaces;
using ProvaPub.Services.Strategies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
#region PARTE 1

builder.Services.AddScoped<IRandomService, RandomService>(); //Alterando o escopo do serviço para Transient, para sempre que
                                                             // requisitar o serviço criar uma instância de RandomService,
                                                             // Dessa forma o número retornado será sempre diferente.

#endregion

#region PARTE 2
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
#endregion

#region PARTE 3
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IPaymentStrategy, CreditCardPaymentStrategy>();
builder.Services.AddScoped<IPaymentStrategy, PixPaymentStrategy>();
builder.Services.AddScoped<IPaymentStrategy, PaypalPaymentStrategy>();
builder.Services.AddScoped<IPaymentStrategyResolver, PaymentStrategyResolver>();
#endregion

builder.Services.AddDbContext<TestDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ctx")));
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
