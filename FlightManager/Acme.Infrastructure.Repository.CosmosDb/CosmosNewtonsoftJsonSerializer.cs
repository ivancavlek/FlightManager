using Acme.SharedKernel.Domain.CosmosDb.Aggregate;
using Acme.SharedKernel.Domain.Entity;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Acme.Base.Repository.CosmosDb;

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
                new StringEnumConverter()
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

        protected override IList<JsonProperty> CreateProperties(System.Type type, MemberSerialization memberSerialization)
        {
            var props = base.GetSerializableMembers(type)
                .Select(pio => CreateProperty(pio, memberSerialization))
                .Union(type
                    .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                    .Where(fio => fio.Name is "id")
                    .Select(fio => CreateProperty(fio, memberSerialization)))
                .ToList();

            return props;
        }

        protected override JsonProperty CreateProperty(
            MemberInfo memberInfo, MemberSerialization memberSerialization)
        {
            var jsonProperty = base.CreateProperty(memberInfo, memberSerialization);

            SetPrivateSetPropertiesAsWritable(jsonProperty, memberInfo);
            SetIdInInsideEntityToLowercase(jsonProperty);
            SetETagInInsideEntityToProperCase(jsonProperty);
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

        private static void SetETagInInsideEntityToProperCase(JsonProperty jsonProperty)
        {
            if (jsonProperty.DeclaringType.IsAssignableFrom(typeof(CosmosDbBaseEntity)) &&
                jsonProperty.PropertyName.Equals(nameof(CosmosDbBaseEntity.ETag)))
                jsonProperty.PropertyName = $"_{nameof(CosmosDbBaseEntity.ETag).ToLowerInvariant()}";
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