namespace Core.Messaging.Events;

internal static class Consts
{
    public static class Prefixes
    {
        public const string Car = "car-";

        public const string Ride = "ride-";

        public const string Client = "client-";
    }
    
    public static class MessageNumbers
    {
        public const int CarCreated = 0;
        public const int CarHeld = 1;
        public const int CarFreed = 2;
        public const int RideCreated = 3;
        public const int RideStarted = 4;
        public const int RideFinished = 5;
        public const int RideCancelled = 6;
        public const int ClientCreated = 7;
    }
}