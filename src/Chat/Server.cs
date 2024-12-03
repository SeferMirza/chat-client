namespace Chat;

public record Server(Guid ServerId, string ServerName, ServerType ServerType, bool Public, int Capacity, List<User> ConnectedUsers);
public enum ServerType { Chat = 0, Voice = 1 }
public record User(string ConnectionId, string Name);