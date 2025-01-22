var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();

builder.Services.AddCors(
    options => options.AddPolicy(
        "AllowOrigin",
        policy => policy.WithOrigins(builder.Configuration["FrontendUrl"] ?? "https://localhost:44388")
            .AllowAnyMethod()
            .AllowAnyHeader()));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    BooksData.FillBooksCatalog();

    app.UseSwagger();
    app.UseSwaggerUI();
} else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseCors("AllowOrigin");

app.MapControllers();

app.Run();
