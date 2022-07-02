using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Qiniu.IO.Model
{
	public class ResumeInfo
	{
		[CompilerGenerated]
		private long _003CFileSize_003Ek__BackingField;

		[CompilerGenerated]
		private int _003CBlockIndex_003Ek__BackingField;

		[CompilerGenerated]
		private int _003CBlockCount_003Ek__BackingField;

		[CompilerGenerated]
		private string[] _003CContexts_003Ek__BackingField;

		[CompilerGenerated]
		private List<string> _003CSContexts_003Ek__BackingField;

		public long FileSize
		{
			[CompilerGenerated]
			get
			{
				return _003CFileSize_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CFileSize_003Ek__BackingField = value;
			}
		}

		public int BlockIndex
		{
			[CompilerGenerated]
			get
			{
				return _003CBlockIndex_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CBlockIndex_003Ek__BackingField = value;
			}
		}

		public int BlockCount
		{
			[CompilerGenerated]
			get
			{
				return _003CBlockCount_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CBlockCount_003Ek__BackingField = value;
			}
		}

		public string[] Contexts
		{
			[CompilerGenerated]
			get
			{
				return _003CContexts_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CContexts_003Ek__BackingField = value;
			}
		}

		public List<string> SContexts
		{
			[CompilerGenerated]
			get
			{
				return _003CSContexts_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CSContexts_003Ek__BackingField = value;
			}
		}
	}
}
