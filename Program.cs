using Microsoft.EntityFrameworkCore;
using ProvaPub.Repository;
using ProvaPub.Repository.Interfaces;
using ProvaPub.Services;
using ProvaPub.Services.Interfaces;
using ProvaPub.Services.PurchasesRules;
using ProvaPub.Services.Strategies;

var builder = WebApplication.CreateBuilder(args);
 

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region DI's

builder.Services.AddScoped<IRandomService, RandomService>(); //Alterando o escopo do serviço para Transient, para sempre que
                                                             // requisitar o serviço criar uma instância de RandomService,
                                                             // Dessa forma o número retornado será sempre diferente. 
 
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
 
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IPaymentStrategy, CreditCardPaymentStrategy>();
builder.Services.AddScoped<IPaymentStrategy, PixPaymentStrategy>();
builder.Services.AddScoped<IPaymentStrategy, PaypalPaymentStrategy>();
builder.Services.AddScoped<IPaymentStrategyResolver, PaymentStrategyResolver>();

builder.Services.AddScoped<IPurchaseHandler, CustomerMustExistHandler>();
builder.Services.AddScoped<IPurchaseHandler, OneOrderPerMonthHandler>();
builder.Services.AddScoped<IPurchaseHandler, FirstPurchaseMaxLimitHandler>();
builder.Services.AddScoped<IPurchaseHandler, BusinessHoursHandler>(); 
builder.Services.AddScoped<IPurchasePolicy, PurchasePolicyChain>();

builder.Services.AddDbContext<TestDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ctx")));

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();      
builder.Services.AddScoped<IProductRepository, ProductRepository>();      
#endregion

var app = builder.Build();
 
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
