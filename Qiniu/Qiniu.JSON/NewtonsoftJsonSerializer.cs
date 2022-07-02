using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Qiniu.JSON
{
	public class NewtonsoftJsonSerializer : IJsonSerializer
	{
		public string Serialize<T>(T obj) where T : new()
		{
			JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
			jsonSerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
			jsonSerializerSettings.NullValueHandling = NullValueHandling.Ignore;
			return JsonConvert.SerializeObject(obj, jsonSerializerSettings);
		}
	}
}
