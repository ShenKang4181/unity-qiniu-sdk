namespace Qiniu.JSON
{
	public interface IJsonDeserializer
	{
		bool Deserialize<T>(string str, out T obj) where T : new();
	}
}
