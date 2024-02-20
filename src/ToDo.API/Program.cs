using ToDo.API.Configuration;
using ToDo.Application.Configuration;
using ToDo.Infra.Data.DependencyInjection;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSingleton(d => builder.Configuration);

//configuration
builder.Services.ConfigureCors();
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddAplicattionConfig(builder.Configuration);
builder.Services.ConfigureSwagger();
builder.Services.AuthenticationConfiguration(builder.Configuration);
//endprogrammerconfigs

var app = builder.Build();


// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
    app.UseSwagger();
    app.UseSwaggerUI();
// }

// app.UseHttpsRedirection();
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();