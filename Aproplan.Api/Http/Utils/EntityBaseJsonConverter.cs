using System;
using System.Reflection;
using Aproplan.Api.Model;
using Aproplan.Api.Model.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Aproplan.Api.Http.Utils
{
    public class EntityBaseJsonConverter: JsonConverter
    {
        static JsonSerializerSettings SpecifiedSubclassConversion = new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() };

        public override bool CanConvert(Type objectType)
        {
            return (objectType.IsAssignableFrom(typeof(Entity)));
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

                if (typeof(Entity).IsAssignableFrom(objectType))
                {
                    Entity entity = null;
                    if (jo.ContainsKey("EntityDiscriminator"))
                    {
                        var entityDiscriminatorValue = jo["EntityDiscriminator"].Value<string>();
                        switch (entityDiscriminatorValue)
                        {
                            case "Note":
                                entity = new Note();

                                break;
                            case "Form":
                                entity = new Form();
                                break;
                            case "FilterProperty":
                                entity = new FilterProperty();

                                break;
                            case "FilterCombination":
                                entity = new FilterCombination();
                                break;
                            default:
                                throw new Exception("EntityDiscriminator " + entityDiscriminatorValue + " not managed in the serialization");
                        }
                    }
                    else
                    {
                        entity = (Entity) Activator.CreateInstance(objectType);
                    }
                    serializer.Populate(jo.CreateReader(), entity);
                    return entity;
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
            get { return true; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JObject jo = new JObject();
            Type type = value.GetType();
            if(value is NoteBase || value is FormFilterCondition) 
            {
                jo.Add("EntityDiscriminator", type.Name);    
            }
            foreach (PropertyInfo prop in type.GetProperties())
            {
                if (prop.CanRead)
                {
                    object propVal = prop.GetValue(value, null);
                    if (propVal != null)
                    {
                        jo.Add(prop.Name, JToken.FromObject(propVal, serializer));
                    }
                }
            }
            jo.WriteTo(writer);
        }
    }
}