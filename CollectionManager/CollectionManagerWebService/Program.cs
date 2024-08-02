using CollectionManagerWebService;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<Context>(x=>x.UseSqlServer(builder.Configuration["ConnectionString"]));
builder.Services.AddControllers();
builder.Services.AddRazorPages();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseSwagger();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.MapSwagger();
app.MapControllers();
app.Run();