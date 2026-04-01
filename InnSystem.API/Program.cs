using InnSystem.IOC;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

//agrgando cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("NewPolicy", app =>
    {
        app.AllowAnyOrigin()
           .AllowAnyHeader()
           .AllowAnyMethod();
    });
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//se llama a la dependecias ICO
builder.Services.DependencyInyections(builder.Configuration);


var app = builder.Build();


//activando CORS
app.UseCors("NewPolicy");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    //agregando scalar
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
