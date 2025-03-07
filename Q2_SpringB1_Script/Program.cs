﻿var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors();
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache(); // Hoặc sử dụng một cung cấp cache khác
builder.Services.AddSession(options =>
{
    // Set session timeout
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
var app = builder.Build();
app.UseCors
(policy =>
policy.AllowAnyOrigin()
.AllowAnyHeader()
.AllowAnyMethod());
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//   app.UseHttpsRedirection();
app.UseStaticFiles();

// app.UseRouting();
app.UseSession();

app.UseAuthorization();
app.UseHttpsRedirection();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();