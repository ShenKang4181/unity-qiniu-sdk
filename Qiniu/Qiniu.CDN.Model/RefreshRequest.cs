using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Qiniu.CDN.Model
{
	public class RefreshRequest
	{
		[CompilerGenerated]
		private List<string> _003CUrls_003Ek__BackingField;

		[CompilerGenerated]
		private List<string> _003CDirs_003Ek__BackingField;

		public List<string> Urls
		{
			[CompilerGenerated]
			get
			{
				return _003CUrls_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CUrls_003Ek__BackingField = value;
			}
		}

		public List<string> Dirs
		{
			[CompilerGenerated]
			get
			{
				return _003CDirs_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CDirs_003Ek__BackingField = value;
			}
		}

		public RefreshRequest()
		{
			Urls = new List<string>();
			Dirs = new List<string>();
		}

		public RefreshRequest(IList<string> urls, IList<string> dirs)
		{
			Urls = new List<string>();
			Dirs = new List<string>();
			if (urls != null)
			{
				AddUrls(urls);
			}
			if (dirs != null)
			{
				AddDirs(dirs);
			}
		}

		public void AddUrls(IList<string> urls)
		{
			if (urls == null)
			{
				return;
			}
			foreach (string url in urls)
			{
				if (!Urls.Contains(url))
				{
					Urls.Add(url);
				}
			}
		}

		public void AddDirs(IList<string> dirs)
		{
			if (dirs == null)
			{
				return;
			}
			foreach (string dir in Dirs)
			{
				if (!Dirs.Contains(dir))
				{
					Dirs.Add(dir);
				}
			}
		}

		public string ToJsonStr()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("{ ");
			stringBuilder.Append("\"urls\":[");
			if (Urls != null)
			{
				if (Urls.Count == 1)
				{
					stringBuilder.AppendFormat("\"{0}\"", Urls[0]);
				}
				else
				{
					for (int i = 0; i < Urls.Count; i++)
					{
						if (i < Urls.Count - 1)
						{
							stringBuilder.AppendFormat("\"{0}\",", Urls[i]);
						}
						else
						{
							stringBuilder.AppendFormat("\"{0}\"", Urls[i]);
						}
					}
				}
			}
			stringBuilder.Append("], ");
			stringBuilder.Append("\"dirs\":[");
			if (Dirs != null)
			{
				if (Dirs.Count == 1)
				{
					stringBuilder.AppendFormat("\"{0}\"", Dirs[0]);
				}
				else
				{
					for (int j = 0; j < Dirs.Count; j++)
					{
						if (j < Dirs.Count - 1)
						{
							stringBuilder.AppendFormat("\"{0}\",", Dirs[j]);
						}
						else
						{
							stringBuilder.AppendFormat("\"{0}\"", Dirs[j]);
						}
					}
				}
			}
			stringBuilder.Append("] }");
			return stringBuilder.ToString();
		}
	}
}
