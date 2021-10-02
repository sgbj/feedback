namespace FeedbackApi;

public class Feedback
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public int Rating { get; set; }
    public string? Message { get; set; }
    public string? UserAgent { get; set; }
    public DateTimeOffset Submitted { get; set; } = DateTimeOffset.UtcNow;
    public List<Attachment> Attachments { get; set; } = new();
}
