using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Qiniu.CDN.Model
{
	public class BandWidthData
	{
		[CompilerGenerated]
		private List<int> _003CChina_003Ek__BackingField;

		[CompilerGenerated]
		private List<int> _003COversea_003Ek__BackingField;

		public List<int> China
		{
			[CompilerGenerated]
			get
			{
				return _003CChina_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CChina_003Ek__BackingField = value;
			}
		}

		public List<int> Oversea
		{
			[CompilerGenerated]
			get
			{
				return _003COversea_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003COversea_003Ek__BackingField = value;
			}
		}
	}
}
