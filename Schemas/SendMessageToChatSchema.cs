namespace MessangerBack.Schemas;


public record SendMessageToChatSchema(string ChatId, string UserName, string Message);
