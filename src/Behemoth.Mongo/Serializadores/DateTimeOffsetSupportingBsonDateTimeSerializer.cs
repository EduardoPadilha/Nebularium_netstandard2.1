using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using System;
using System.Globalization;

namespace Nebularium.Behemoth.Mongo.Serializadores
{
    public class DateTimeOffsetSupportingBsonDateTimeSerializer : StructSerializerBase<DateTimeOffset>,
                 IRepresentationConfigurable<DateTimeOffsetSupportingBsonDateTimeSerializer>
    {
        private string StringSerializationFormat = "YYYY-MM-ddTHH:mm:ss.FFFK";
        public BsonType Representation { get; }

        public DateTimeOffsetSupportingBsonDateTimeSerializer() : this(BsonType.DateTime)
        {
        }

        public DateTimeOffsetSupportingBsonDateTimeSerializer(BsonType representation)
        {
            switch (representation)
            {
                case BsonType.String:
                case BsonType.DateTime:
                    break;
                default:
                    throw new ArgumentException($"{representation} is not a valid representation for {GetType().Name}");
            }

            Representation = representation;
        }


        public override DateTimeOffset Deserialize(BsonDeserializationContext context,
                                                   BsonDeserializationArgs args)
        {
            var bsonReader = context.Reader;

            BsonType bsonType = bsonReader.GetCurrentBsonType();
            switch (bsonType)
            {
                case BsonType.String:
                    var stringValue = bsonReader.ReadString();
                    return DateTimeOffset.ParseExact
                        (stringValue, StringSerializationFormat, DateTimeFormatInfo.InvariantInfo);

                case BsonType.DateTime:
                    var dateTimeValue = bsonReader.ReadDateTime();
                    return DateTimeOffset.FromUnixTimeMilliseconds(dateTimeValue);

                default:
                    throw CreateCannotDeserializeFromBsonTypeException(bsonType);
            }
        }

        public override void Serialize
           (BsonSerializationContext context, BsonSerializationArgs args, DateTimeOffset value)
        {
            var bsonWriter = context.Writer;

            switch (Representation)
            {
                case BsonType.String:
                    bsonWriter.WriteString(value.ToString
                          (StringSerializationFormat, DateTimeFormatInfo.InvariantInfo));
                    break;

                case BsonType.DateTime:
                    bsonWriter.WriteDateTime(value.ToUnixTimeMilliseconds());
                    break;

                default:
                    var message = $"'{Representation}' is not a valid DateTimeOffset representation.";


                    throw new BsonSerializationException(message);
            }
        }

        public DateTimeOffsetSupportingBsonDateTimeSerializer WithRepresentation(BsonType representation)
        {
            if (representation == Representation)
            {
                return this;
            }
            return new DateTimeOffsetSupportingBsonDateTimeSerializer(representation);
        }

        IBsonSerializer IRepresentationConfigurable.WithRepresentation(BsonType representation)
        {
            return WithRepresentation(representation);
        }

        protected new Exception CreateCannotDeserializeFromBsonTypeException(BsonType bsonType)
        {
            var message = $"Cannot deserialize a '{BsonUtils.GetFriendlyTypeName(ValueType)}' from BsonType '{bsonType}'.";
            return new FormatException(message);
        }
    }
}
