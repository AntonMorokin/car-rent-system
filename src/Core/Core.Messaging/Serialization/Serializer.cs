using Confluent.Kafka;
using MessagePack;

namespace Core.Messaging.Serialization;

internal sealed class Serializer<T> : ISerializer<T>, IDeserializer<T>
{
    public static Serializer<T> Single = new();

    private Serializer()
    {
    }

    public byte[] Serialize(T data, SerializationContext context)
    {
        return MessagePackSerializer.Serialize(data);
    }

    public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        if (isNull)
        {
            throw new InvalidOperationException($"Value of type={typeof(T).FullName} is null");
        }

        return MessagePackSerializer.Deserialize<T>(new ReadOnlyMemory<byte>(data.ToArray()));
    }
}