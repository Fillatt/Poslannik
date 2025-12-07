namespace Poslannik.Framework.Hubs;

public static class HubConstants
{
    public static string AuthorizationHubPath = "/authorizationHub";
    public static string ChatHubPath = "/chatHub";

    public static class ChatEvents
    {
        public const string ChatCreated = "ChatCreated";
        public const string ChatUpdated = "ChatUpdated";
        public const string ChatDeleted = "ChatDeleted";
    }
}
