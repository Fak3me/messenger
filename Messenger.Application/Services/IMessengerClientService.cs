﻿using Messenger.Application.Models;

namespace Messenger.Application.Services
{
    public interface IMessengerClientService {
        Task MessageDeletedNotification(MessageDto message, CancellationToken ct);
        Task MessageEditedNotification(MessageDto message, CancellationToken ct);
        Task NewMessageNotification(MessageDto message, CancellationToken ct);
    }
}