namespace Chat;

public record Message(Guid Id, string Content, string SenderId, Guid RoomId, DateTime SentAt);
