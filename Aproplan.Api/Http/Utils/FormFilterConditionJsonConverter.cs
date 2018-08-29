using Aproplan.Api.Model.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Http.Utils
{
    public class FormFilterConditionJsonConverter : JsonConverter
    {
        static JsonSerializerSettings SpecifiedSubclassConversion = new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() };

        public override bool CanConvert(Type objectType)
        {
            return (objectType.IsAssignableFrom(typeof(FormFilterCondition)));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            try
            {
                if (reader.TokenType == JsonToken.Null)
                {
                    return null;
                }
                JObject jo = JObject.Load(reader);

                if (objectType == typeof(FormFilterCondition))
                {
                    FormFilterCondition formCondition = null;
                    switch (jo["EntityDiscriminator"].Value<string>())
                    {
                        case "FilterProperty":
                            formCondition = new FilterProperty();

                            break;
                        case "FilterCombination":
                            formCondition = new FilterCombination();
                            break;
                        default:
                            throw new Exception();
                    }
                    serializer.Populate(jo.CreateReader(), formCondition);
                    return formCondition;
                }
            }
            catch (Exception)
            {
                return null;
            }
            throw new NotImplementedException();
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException(); // won't be called because CanWrite returns false
        }
    }
}
