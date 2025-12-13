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
        public const string ParticipantRemoved = "ParticipantRemoved";
        public const string AdminRightsTransferred = "AdminRightsTransferred";
    }

    public static class MessageEvents
    {
        public const string MessageSended = "MessageSended";
    }
}
