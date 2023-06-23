using Acme.Base.Domain.CosmosDb.Aggregate;
using Acme.Base.Domain.Entity;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Douglas.Repository.CosmosDb;

public class CosmosNewtonsoftJsonSerializer : CosmosSerializer
{
    private readonly Encoding _defaultEncoding;
    private readonly JsonSerializer _jsonSerializer;
    private readonly JsonSerializerSettings _jsonSerializerSettings;

    public CosmosNewtonsoftJsonSerializer()
    {
        _defaultEncoding = new UTF8Encoding(false, true);
        _jsonSerializerSettings = new JsonSerializerSettings
        {
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
            ContractResolver = new InsideContractResolver(),
            Converters = new List<JsonConverter>
                {
                    new StringEnumConverter
                    {
                        AllowIntegerValues = true
                    }
                },
            NullValueHandling = NullValueHandling.Ignore
        };
        _jsonSerializer = JsonSerializer.Create(_jsonSerializerSettings);
    }

    public override T FromStream<T>(Stream stream)
    {
        using (stream)
        {
            if (typeof(Stream).IsAssignableFrom(typeof(T)))
                return (T)(object)(stream);

            using var streamReader = new StreamReader(stream);
            using var jsonTextReader = new JsonTextReader(streamReader);
            return _jsonSerializer.Deserialize<T>(jsonTextReader);
        }
    }

    public override Stream ToStream<T>(T input)
    {
        var streamPayload = new MemoryStream();

        using (var streamWriter = new StreamWriter(streamPayload, _defaultEncoding, 1024, true))
        {
            using var jsonWriter = new JsonTextWriter(streamWriter)
            {
                Formatting = Formatting.None
            };

            _jsonSerializer.Serialize(jsonWriter, input);

            jsonWriter.Flush();
            streamWriter.Flush();
        }

        streamPayload.Position = 0;

        return streamPayload;
    }

    private class InsideContractResolver : DefaultContractResolver
    {
        //public InsideContractResolver() =>
        //    NamingStrategy = new CamelCaseNamingStrategy();

        protected override JsonProperty CreateProperty(
            MemberInfo memberInfo, MemberSerialization memberSerialization)
        {
            var jsonProperty = base.CreateProperty(memberInfo, memberSerialization);

            SetPrivateSetPropertiesAsWritable(jsonProperty, memberInfo);
            SetIdInInsideEntityToLowercase(jsonProperty);
            SetETagnInsideEntityToProperCase(jsonProperty);
            DoNotWriteEmptyCollections(jsonProperty);

            return jsonProperty;
        }

        private static void SetPrivateSetPropertiesAsWritable(JsonProperty jsonProperty, MemberInfo memberInfo)
        {
            if (!jsonProperty.Writable && memberInfo is PropertyInfo propertyInfo)
                jsonProperty.Writable = propertyInfo.GetSetMethod(true) is not null;
        }

        private static void SetIdInInsideEntityToLowercase(JsonProperty jsonProperty)
        {
            if (jsonProperty.DeclaringType.IsAssignableFrom(typeof(BaseEntity)) &&
                jsonProperty.PropertyName.Equals(nameof(BaseEntity.Id)))
                jsonProperty.PropertyName = nameof(BaseEntity.Id).ToLowerInvariant();
        }

        private static void SetETagnInsideEntityToProperCase(JsonProperty jsonProperty)
        {
            if (jsonProperty.DeclaringType.IsAssignableFrom(typeof(CosmosDbAggregateRoot)) &&
                jsonProperty.PropertyName.Equals(nameof(CosmosDbAggregateRoot.ETag)))
                jsonProperty.PropertyName = $"_{nameof(CosmosDbAggregateRoot.ETag).ToLowerInvariant()}";
        }

        private static void DoNotWriteEmptyCollections(JsonProperty jsonProperty)
        {
            if (jsonProperty.PropertyType != typeof(string) &&
                typeof(IEnumerable).IsAssignableFrom(jsonProperty.PropertyType))
                jsonProperty.ShouldSerialize = instance =>
                    jsonProperty.ValueProvider?.GetValue(instance) is not IEnumerable enumerable ||
                    enumerable.GetEnumerator().MoveNext();
        }
    }
}