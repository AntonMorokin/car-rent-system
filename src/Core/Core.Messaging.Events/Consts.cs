namespace Core.Messaging.Events;

internal static class Consts
{
    public static class Prefixes
    {
        public const string Car = "car-";

        public const string Ride = "ride-";
    }
    
    public static class MessageNumbers
    {
        public const int CarCreated = 0;
        public const int CarHeld = 1;
        public const int CarFreed = 2;
        public const int RideCreated = 3;
    }
}