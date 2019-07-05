using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoJun.Shadow.WebApi
{
    /// <summary>
    /// 参数自动去除前后空格转换器
    /// </summary>
    public class TrimmingConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String)
                if (reader.Value != null)
                    return (reader.Value as string).Trim();

            return reader.Value;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var text = (string)value;
            if (text == null)
                writer.WriteNull();
            else
                writer.WriteValue(text.Trim());
        }
    }
}
