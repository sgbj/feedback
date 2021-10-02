namespace FeedbackApi;

public class Attachment
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string MediaType { get; set; } = null!;
    public byte[] Data { get; set; } = null!;
}
