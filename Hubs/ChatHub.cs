using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.Extensions.Caching.Distributed;


using MessangerBack.Schemas;
using MessangerBack.Services;
using MessangerBack.Responces;



namespace MessangerBack.Hubs;


public interface IChatClient
{
    public Task ReceiveMessage(MessageResponce message);
    public Task UpdateChat(ChatInfoResponce chat);
}

[Authorize]
public class ChatHub : Hub<IChatClient>
{

    IMessageService _messageService;
    IChatService _chatService;
    IDistributedCache _cahce;

    public ChatHub(IMessageService messageService, IChatService chatService, IDistributedCache cahce)
    {
        _messageService = messageService;
        _chatService = chatService;
        _cahce = cahce;
    }

    public async Task JoinChat(ConnectionChatSchema connectionChat)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, connectionChat.ChatId);
    }

    public async Task ListeningChatsUpdates()
    {
        string userId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        await _cahce.SetStringAsync(userId, Context.ConnectionId);
    }

    public async Task SendMessage(SendMessageToChatSchema sendSchemaInfo)
    {
        var rawUserId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        var userId = Guid.Parse(rawUserId);
        var chatId = Guid.Parse(sendSchemaInfo.ChatId);
        
        var chat = await _chatService.GetChatInfo(chatId);
        if (!chat.Users.Contains(userId))
        {
            throw new HubException("Вы не можете отправлять сообщение в данный чат.");
        }
        var message = await _messageService.CreateMessage(
            senderId: userId, chatId: chatId, sendSchemaInfo.Message);

        await Clients
            .Group(sendSchemaInfo.ChatId)
            .ReceiveMessage(message);

        chat.LastMessageId = message.Id;
        chat.LastMessage = message;
        foreach (var user in chat.Users)
        {
            var connectionId =  await _cahce.GetStringAsync(user.ToString());
            if (connectionId == null) continue;
            
            Clients
                .Client(connectionId)
                .UpdateChat(chat);
        }
    }
}
