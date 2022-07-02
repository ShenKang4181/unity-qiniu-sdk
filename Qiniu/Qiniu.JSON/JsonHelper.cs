namespace Qiniu.JSON
{
	public sealed class JsonHelper
	{
		public static IJsonSerializer JsonSerializer = new NewtonsoftJsonSerializer();

		public static IJsonDeserializer JsonDeserializer = new NewtonsoftJsonDeserializer();

		public static string Serialize<T>(T obj) where T : new()
		{
			return JsonSerializer.Serialize(obj);
		}

		public static bool Deserialize<T>(string jstr, out T obj) where T : new()
		{
			return JsonDeserializer.Deserialize<T>(jstr, out obj);
		}
	}
}
