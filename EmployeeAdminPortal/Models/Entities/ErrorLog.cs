public class ErrorLog
{
    public Guid Id { get; set; }

    public required string Message { get; set; }
    public required string StackTrace { get; set; }
    public string? InnerException { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
