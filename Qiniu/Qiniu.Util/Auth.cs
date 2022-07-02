namespace Qiniu.Util
{
	public class Auth
	{
		private Signature signature;

		public Auth(Mac mac)
		{
			signature = new Signature(mac);
		}

		public string CreateManageToken(string url, byte[] body)
		{
			return string.Format("QBox {0}", signature.SignRequest(url, body));
		}

		public string CreateManageToken(string url)
		{
			return CreateManageToken(url, null);
		}

		public string CreateUploadToken(string jsonStr)
		{
			return signature.SignWithData(jsonStr);
		}

		public string CreateDownloadToken(string url)
		{
			return signature.Sign(url);
		}

		public string CreateStreamPublishToken(string path)
		{
			return signature.Sign(path);
		}

		public string CreateStreamManageToken(string data)
		{
			return string.Format("Qiniu {0}", signature.SignWithData(data));
		}

		public static string CreateManageToken(Mac mac, string url, byte[] body)
		{
			Signature signature = new Signature(mac);
			return string.Format("QBox {0}", signature.SignRequest(url, body));
		}

		public static string CreateManageToken(Mac mac, string url)
		{
			return CreateManageToken(mac, url, null);
		}

		public static string CreateUploadToken(Mac mac, string jsonBody)
		{
			return new Signature(mac).SignWithData(jsonBody);
		}

		public static string CreateDownloadToken(Mac mac, string url)
		{
			return new Signature(mac).Sign(url);
		}

		public static string CreateStreamPublishToken(Mac mac, string path)
		{
			return new Signature(mac).Sign(path);
		}

		public static string CreateStreamManageToken(Mac mac, string data)
		{
			Signature signature = new Signature(mac);
			return string.Format("Qiniu {0}", signature.Sign(data));
		}
	}
}
