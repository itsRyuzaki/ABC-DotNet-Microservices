using ABC.Accessories.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// for swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// for db
var conString = builder.Configuration.GetConnectionString("Accessories_Database") ??
     throw new InvalidOperationException("Connection string 'Accessories_Database' not found.");
     
Console.WriteLine(conString);

builder.Services.AddNpgsql<AccessoriesDataContext>(conString);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHttpsRedirection();
}


app.UseAuthorization();

app.MapControllers();

app.Run();
