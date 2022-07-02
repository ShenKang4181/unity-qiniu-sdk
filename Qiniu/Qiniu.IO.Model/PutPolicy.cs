using System.Runtime.CompilerServices;
using Qiniu.JSON;
using Qiniu.Util;

namespace Qiniu.IO.Model
{
	public class PutPolicy
	{
		[CompilerGenerated]
		private string _003CScope_003Ek__BackingField;

		[CompilerGenerated]
		private int _003CDeadline_003Ek__BackingField;

		[CompilerGenerated]
		private int? _003CInsertOnly_003Ek__BackingField;

		[CompilerGenerated]
		private string _003CSaveKey_003Ek__BackingField;

		[CompilerGenerated]
		private string _003CEndUser_003Ek__BackingField;

		[CompilerGenerated]
		private string _003CReturnUrl_003Ek__BackingField;

		[CompilerGenerated]
		private string _003CReturnBody_003Ek__BackingField;

		[CompilerGenerated]
		private string _003CCallbackUrl_003Ek__BackingField;

		[CompilerGenerated]
		private string _003CCallbackBody_003Ek__BackingField;

		[CompilerGenerated]
		private string _003CCallbackBodyType_003Ek__BackingField;

		[CompilerGenerated]
		private string _003CCallbackHost_003Ek__BackingField;

		[CompilerGenerated]
		private int? _003CCallbackFetchKey_003Ek__BackingField;

		[CompilerGenerated]
		private string _003CPersistentOps_003Ek__BackingField;

		[CompilerGenerated]
		private string _003CPersistentNotifyUrl_003Ek__BackingField;

		[CompilerGenerated]
		private string _003CPersistentPipeline_003Ek__BackingField;

		[CompilerGenerated]
		private int? _003CFsizeMin_003Ek__BackingField;

		[CompilerGenerated]
		private int? _003CFsizeLimit_003Ek__BackingField;

		[CompilerGenerated]
		private int? _003CDetectMime_003Ek__BackingField;

		[CompilerGenerated]
		private string _003CMimeLimit_003Ek__BackingField;

		[CompilerGenerated]
		private int? _003CDeleteAfterDays_003Ek__BackingField;

		[JsonProperty("scope")]
		public string Scope
		{
			[CompilerGenerated]
			get
			{
				return _003CScope_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CScope_003Ek__BackingField = value;
			}
		}

		[JsonProperty("deadline")]
		public int Deadline
		{
			[CompilerGenerated]
			get
			{
				return _003CDeadline_003Ek__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				_003CDeadline_003Ek__BackingField = value;
			}
		}

		[JsonProperty("insertOnly")]
		public int? InsertOnly
		{
			[CompilerGenerated]
			get
			{
				return _003CInsertOnly_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CInsertOnly_003Ek__BackingField = value;
			}
		}

		[JsonProperty("saveKey")]
		public string SaveKey
		{
			[CompilerGenerated]
			get
			{
				return _003CSaveKey_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CSaveKey_003Ek__BackingField = value;
			}
		}

		[JsonProperty("endUser")]
		public string EndUser
		{
			[CompilerGenerated]
			get
			{
				return _003CEndUser_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CEndUser_003Ek__BackingField = value;
			}
		}

		[JsonProperty("returnUrl")]
		public string ReturnUrl
		{
			[CompilerGenerated]
			get
			{
				return _003CReturnUrl_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CReturnUrl_003Ek__BackingField = value;
			}
		}

		[JsonProperty("returnBody")]
		public string ReturnBody
		{
			[CompilerGenerated]
			get
			{
				return _003CReturnBody_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CReturnBody_003Ek__BackingField = value;
			}
		}

		[JsonProperty("callBackUrl")]
		public string CallbackUrl
		{
			[CompilerGenerated]
			get
			{
				return _003CCallbackUrl_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CCallbackUrl_003Ek__BackingField = value;
			}
		}

		[JsonProperty("callbackBody")]
		public string CallbackBody
		{
			[CompilerGenerated]
			get
			{
				return _003CCallbackBody_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CCallbackBody_003Ek__BackingField = value;
			}
		}

		[JsonProperty("callbackBodyType")]
		public string CallbackBodyType
		{
			[CompilerGenerated]
			get
			{
				return _003CCallbackBodyType_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CCallbackBodyType_003Ek__BackingField = value;
			}
		}

		[JsonProperty("callbackHost")]
		public string CallbackHost
		{
			[CompilerGenerated]
			get
			{
				return _003CCallbackHost_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CCallbackHost_003Ek__BackingField = value;
			}
		}

		[JsonProperty("callbackFetchKey")]
		public int? CallbackFetchKey
		{
			[CompilerGenerated]
			get
			{
				return _003CCallbackFetchKey_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CCallbackFetchKey_003Ek__BackingField = value;
			}
		}

		[JsonProperty("persistentOps")]
		public string PersistentOps
		{
			[CompilerGenerated]
			get
			{
				return _003CPersistentOps_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CPersistentOps_003Ek__BackingField = value;
			}
		}

		[JsonProperty("persistentNotifyUrl")]
		public string PersistentNotifyUrl
		{
			[CompilerGenerated]
			get
			{
				return _003CPersistentNotifyUrl_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CPersistentNotifyUrl_003Ek__BackingField = value;
			}
		}

		[JsonProperty("persistentPipeline")]
		public string PersistentPipeline
		{
			[CompilerGenerated]
			get
			{
				return _003CPersistentPipeline_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CPersistentPipeline_003Ek__BackingField = value;
			}
		}

		[JsonProperty("fsizeMin")]
		public int? FsizeMin
		{
			[CompilerGenerated]
			get
			{
				return _003CFsizeMin_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CFsizeMin_003Ek__BackingField = value;
			}
		}

		[JsonProperty("fsizeLimit")]
		public int? FsizeLimit
		{
			[CompilerGenerated]
			get
			{
				return _003CFsizeLimit_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CFsizeLimit_003Ek__BackingField = value;
			}
		}

		[JsonProperty("detectMime")]
		public int? DetectMime
		{
			[CompilerGenerated]
			get
			{
				return _003CDetectMime_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CDetectMime_003Ek__BackingField = value;
			}
		}

		[JsonProperty("mimeLimit")]
		public string MimeLimit
		{
			[CompilerGenerated]
			get
			{
				return _003CMimeLimit_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CMimeLimit_003Ek__BackingField = value;
			}
		}

		[JsonProperty("deleteAfterDays")]
		public int? DeleteAfterDays
		{
			[CompilerGenerated]
			get
			{
				return _003CDeleteAfterDays_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CDeleteAfterDays_003Ek__BackingField = value;
			}
		}

		public void SetExpires(int expireInSeconds)
		{
			Deadline = (int)UnixTimestamp.GetUnixTimestamp(expireInSeconds);
		}

		public string ToJsonString()
		{
			return JsonHelper.Serialize(this);
		}
	}
}
