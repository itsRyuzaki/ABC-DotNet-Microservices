using ABC.Users.ABCMapper;
using ABC.Users.Facade;
using ABC.Users.Middleware;
using ABC.Users.Models;
using ABC.Users.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Add Cors
string origin = builder.Configuration.GetSection("AppSettings")
                                    .GetValue<string>("AppOrigin") ?? "";
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy
                .WithOrigins(origin)
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});


// for swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// for db
builder.Services.Configure<UsersDatabaseSettings>(
    builder.Configuration.GetSection("ABCUsersDatabase"));

/** 
    Add Services to DI here 
*/

builder.Services.AddAutoMapper(typeof(ABCMapper));
builder.Services.AddSingleton<IMongoDBService, MongoDbService>();
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddSingleton<IUserFacade, UserFacade>();


// Post builder 
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

app.UseCors();

app.UseAuthorization();

app.UseMiddleware<HttpSessionMiddleware>();

app.MapControllers();

app.Run();
