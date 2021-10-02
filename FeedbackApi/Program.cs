using FeedbackApi;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<FeedbackDbContext>(options =>
    options.UseInMemoryDatabase(builder.Environment.ApplicationName));

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = builder.Environment.ApplicationName, Version = "v1" });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{builder.Environment.ApplicationName} v1"));
}

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/feedbacks", async (FeedbackDbContext db) =>
{
    return await db.Feedbacks.ToListAsync();
});

app.MapGet("/feedbacks/{id}", async (FeedbackDbContext db, string id) =>
{
    return await db.Feedbacks.Include(x => x.Attachments).FirstOrDefaultAsync(x => x.Id == id)
        is Feedback feedback ? Results.Ok(feedback) : Results.NotFound();
});

app.MapPost("/feedbacks", async (FeedbackDbContext db, Feedback feedback) =>
{
    db.Add(feedback);
    await db.SaveChangesAsync();

    return Results.Created($"/feedbacks/{feedback.Id}", feedback);
});

app.MapPut("/feedbacks/{id}", async (FeedbackDbContext db, string id, Feedback feedback) =>
{
    if (id != feedback.Id)
    {
        return Results.BadRequest();
    }

    if (!await db.Feedbacks.AnyAsync(x => x.Id == id))
    {
        return Results.NotFound();
    }

    db.Update(feedback);
    await db.SaveChangesAsync();

    return Results.Ok();
});

app.MapDelete("/feedbacks/{id}", async (FeedbackDbContext db, string id) =>
{
    var feedback = await db.Feedbacks.FindAsync(id);

    if (feedback is null)
    {
        return Results.NotFound();
    }

    db.Remove(feedback);
    await db.SaveChangesAsync();

    return Results.Ok();
});

app.Run();
