namespace Qiniu.JSON
{
	public interface IJsonSerializer
	{
		string Serialize<T>(T obj) where T : new();
	}
}
