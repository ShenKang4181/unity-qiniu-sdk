using System;
using Newtonsoft.Json;

namespace Qiniu.JSON
{
	public class NewtonsoftJsonDeserializer : IJsonDeserializer
	{
		public bool Deserialize<T>(string str, out T obj) where T : new()
		{
			obj = default(T);
			bool result = true;
			try
			{
				obj = JsonConvert.DeserializeObject<T>(str);
				return result;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}
