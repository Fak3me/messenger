namespace Messenger.Application.Models;

public class ChatDto {
    public Guid ChatId { get; set; }

    public List<UserDto> Participants { get; set; }
    //сообщения будем грузить отдельно
}