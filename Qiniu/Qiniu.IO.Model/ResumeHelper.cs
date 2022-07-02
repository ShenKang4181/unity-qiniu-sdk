using System;
using System.IO;
using Qiniu.JSON;
using Qiniu.Util;

namespace Qiniu.IO.Model
{
	public class ResumeHelper
	{
		public static string GetDefaultRecordKey(string localFile, string saveKey)
		{
			return "QiniuRU_" + Hashing.CalcMD5X(localFile + saveKey);
		}

		public static ResumeInfo Load(string recordFile)
		{
			ResumeInfo obj = null;
			try
			{
				using (FileStream stream = new FileStream(recordFile, FileMode.Open))
				{
					using (StreamReader streamReader = new StreamReader(stream))
					{
						JsonHelper.Deserialize<ResumeInfo>(streamReader.ReadToEnd(), out obj);
						return obj;
					}
				}
			}
			catch (Exception)
			{
				return null;
			}
		}

		public static void Save(ResumeInfo resumeInfo, string recordFile)
		{
			string value = string.Format("{{\"fileSize\":{0}, \"blockIndex\":{1}, \"blockCount\":{2}, \"contexts\":[{3}]}}", resumeInfo.FileSize, resumeInfo.BlockIndex, resumeInfo.BlockCount, StringHelper.JsonJoin(resumeInfo.Contexts));
			using (FileStream stream = new FileStream(recordFile, FileMode.Create))
			{
				using (StreamWriter streamWriter = new StreamWriter(stream))
				{
					streamWriter.Write(value);
				}
			}
		}
	}
}
