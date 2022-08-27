using CorridaAPI;
using CorridaAPI.Data.Contracts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDatabaseDependency(builder.Configuration);
builder.Services.AddServiceDependency();
builder.Services.AddAuthDependency(builder.Configuration);


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStatusCodePages();

SemearUsuariosPapeis(app);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

void SemearUsuariosPapeis(IApplicationBuilder app)
{

    
    using (var service = app.ApplicationServices.CreateScope())
    {
        var semar = service.ServiceProvider
                           .GetService<ISemearUsuarioPadrao>();
        semar.SemearPapeis();
        semar.SemearUsuarios();
    }
}
