namespace Core.Messaging.Events;

internal static class Consts
{
    public static class Prefixes
    {
        public const string Car = "car-";
    }
    
    public static class MessageNumbers
    {
        public const int CarCreated = 0;
        public const int CarHeld = 1;
        public const int CarFreed = 2;
    }
}