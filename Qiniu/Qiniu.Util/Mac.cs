using System.Runtime.CompilerServices;

namespace Qiniu.Util
{
	public class Mac
	{
		[CompilerGenerated]
		private string _003CAccessKey_003Ek__BackingField;

		[CompilerGenerated]
		private string _003CSecretKey_003Ek__BackingField;

		public string AccessKey
		{
			[CompilerGenerated]
			get
			{
				return _003CAccessKey_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CAccessKey_003Ek__BackingField = value;
			}
		}

		public string SecretKey
		{
			[CompilerGenerated]
			get
			{
				return _003CSecretKey_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CSecretKey_003Ek__BackingField = value;
			}
		}

		public Mac(string accessKey, string secretKey)
		{
			AccessKey = accessKey;
			SecretKey = secretKey;
		}
	}
}
