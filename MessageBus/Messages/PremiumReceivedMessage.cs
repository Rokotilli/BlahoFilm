﻿namespace MessageBus.Messages
{
    public class PremiumReceivedMessage : IntegrationBaseEvent
    {
        public string UserId { get; set; }
    }
}
