namespace Poslannik.Framework.Hubs;

public static class HubConstants
{
    public static string AuthorizationHubPath = "/authorizationHub";
    public static string ChatHubPath = "/chatHub";
    public static string UserHubPath = "/userHub";
    public static string MessageHubPath = "/messageHub";

    public static class ChatEvents
    {
        public const string ChatCreated = "ChatCreated";
        public const string ChatUpdated = "ChatUpdated";
        public const string ChatDeleted = "ChatDeleted";
    }

    public static class MessageEvents
    {
        public const string MessageReceived = "MessageReceived";
        public const string MessageSent = "MessageSent";
        public const string MessageDeleted = "MessageDeleted";
        public const string MessageUpdated = "MessageUpdated";
    }
}
