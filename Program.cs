using CRUDCxC.Data;
using CRUDCxC.Utils;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();

if (builder.Environment.IsDevelopment())
{
    DotNetEnv.Env.Load();
}

builder.Services.AddDbContext<CxCDbContext>(options =>
{
    var connectionString = ConnectionStringBuilder.BuildFromEnvironment();
    options.UseSqlServer(connectionString);
});

// builder.Services.AddRazorPages();
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

var app = builder.Build();


// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

// app.UseHttpsRedirection();
// app.MapRazorPages();


// app.Run();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<CxCDbContext>();

    DbSeeder.SeedClients(context);
    DbSeeder.SeedDocumentTypes(context);
    DbSeeder.SeedTransactions(context);
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
