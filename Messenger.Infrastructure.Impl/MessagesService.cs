using Messenger.Application.Models;
using Messenger.Application.Repositories;
using Messenger.Application.Services;
using Messenger.Domain;

namespace Messenger.Infrastructure.Impl {
    public class MessagesService : IMessagesService {
        private readonly IMessageRepository _messageRepository;
        private readonly IUserRepository _userRepository;
        private readonly IChatRepository _chatRepository;
        private readonly IMessengerClientService _messengerClientService;

        public MessagesService(IMessageRepository messageRepository, IUserRepository userRepository,
            IChatRepository chatRepository, IMessengerClientService messengerClientService) {
            _messageRepository = messageRepository;
            _userRepository = userRepository;
            _chatRepository = chatRepository;
            _messengerClientService = messengerClientService;
        }

        public async Task CreateMessage(MessageDto message, Guid creatorId, CancellationToken ct = default) {
            var chat = await _chatRepository.GetByIdAsync(message.ChatId, ct);
            if (chat == null) {
                throw new ArgumentException($"Чата с Id = {message.ChatId} не существует");
            }

            var creator = await _userRepository.GetByIdAsync(creatorId, ct);
            if (creator == null) {
                throw new ArgumentException($"Юзера с Id = {creatorId} не существует");
            }

            var entity = Message.Create(message.Message, creator, chat);
            await _messageRepository.CreateAsync(entity, ct);
            message.MessageId = entity.Id;
            await _messengerClientService.NewMessageNotification(message, ct);
        }

        public async Task DeleteMessage(MessageDto message, CancellationToken ct = default) {
            if (message.MessageId == null) {
                throw new ArgumentNullException("не передан параметр MessageId");
            }

            var entity = await _messageRepository.GetByIdAsync(message.MessageId.Value, ct);
            if (entity == null) {
                throw new ArgumentException($"Сообщения с Id = {message.ChatId} не существует");
            }

            var chat = await _chatRepository.GetByIdAsync(message.ChatId, ct);
            if (chat == null) {
                throw new ArgumentException($"Чата с Id = {message.ChatId} не существует");
            }

            entity.Delete();
            await _messageRepository.UpdateAsync(entity, ct);
            await _messengerClientService.MessageDeletedNotification(message, ct);
        }

        public async Task EditMessage(MessageDto message, CancellationToken ct = default) {
            if (message.MessageId == null) {
                throw new ArgumentNullException("не передан параметр MessageId");
            }

            var entity = await _messageRepository.GetByIdAsync(message.MessageId.Value, ct);
            if (entity == null) {
                throw new ArgumentException($"Сообщения с Id = {message.ChatId} не существует");
            }

            var chat = await _chatRepository.GetByIdAsync(message.ChatId, ct);
            if (chat == null) {
                throw new ArgumentException($"Чата с Id = {message.ChatId} не существует");
            }

            entity.ChangeMessage(message.Message);
            await _messageRepository.UpdateAsync(entity, ct);
            await _messengerClientService.MessageEditedNotification(message, ct);
        }

        public async Task<IList<MessageDto>> GetMessages(Guid chatId, CancellationToken ct) {
            var chat = await _chatRepository.GetByIdAsync(chatId, ct);
            if (chat == null) {
                throw new ArgumentException($"Чата с Id = {chatId} не существует");
            }

            return chat.Messages.Where(x => !x.IsDeleted).Select(x => new MessageDto
                    { ChatId = chatId, Message = x.Text, MessageId = x.Id })
                .ToList();
        }
    }
}