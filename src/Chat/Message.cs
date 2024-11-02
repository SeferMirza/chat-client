namespace Chat;

public record Message(Guid Id, string Content, string Sender, Guid ServerId, DateTime SentAt);
