namespace Chat;

public record Server(Guid ServerId, string ServerName, ServerType ServerType, bool Public);
public record ServerDetail(Guid ServerId, string ServerName, ServerType ServerType, bool Public, int Capacity, List<string> ConnectedUsers);
public enum ServerType { Chat = 0, Voice = 1 }
