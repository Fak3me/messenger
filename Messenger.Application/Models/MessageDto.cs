namespace Messenger.Application.Models {
    public class MessageDto {
        public Guid? MessageId { get; set; }
        public string? Message { get; set; }
        public Guid ChatId { get; set; }
    }
}
