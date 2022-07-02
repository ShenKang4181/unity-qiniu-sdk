using System.IO;
using Qiniu.Http;
using Qiniu.IO.Model;
using Qiniu.Util;

namespace Qiniu.IO
{
	public class UploadManager
	{
		private long PUT_THRESHOLD = 10485760L;

		private ChunkUnit CHUNK_UNIT = ChunkUnit.U2048K;

		private bool UPLOAD_FROM_CDN;

		private UploadProgressHandler upph;

		private StreamProgressHandler sph;

		private UploadController upctl;

		private string recordFile;

		public UploadManager(long putThreshold = 10485760L, bool uploadFromCDN = false)
		{
			PUT_THRESHOLD = putThreshold;
			UPLOAD_FROM_CDN = uploadFromCDN;
		}

		public void SetUploadProgressHandler(UploadProgressHandler upph)
		{
			this.upph = upph;
		}

		public void SetStreamrogressHandler(StreamProgressHandler sph)
		{
			this.sph = sph;
		}

		public void SetUploadController(UploadController upctl)
		{
			this.upctl = upctl;
		}

		public void SetRecordFile(string recordFile)
		{
			this.recordFile = recordFile;
		}

		public void SetChunkUnit(ChunkUnit chunkUnit)
		{
			CHUNK_UNIT = chunkUnit;
		}

		public HttpResult UploadFile(string localFile, string saveKey, string token)
		{
			HttpResult httpResult = new HttpResult();
			if (new FileInfo(localFile).Length > PUT_THRESHOLD)
			{
				if (string.IsNullOrEmpty(recordFile))
				{
					string defaultRecordKey = ResumeHelper.GetDefaultRecordKey(localFile, saveKey);
					recordFile = Path.Combine(UserEnv.GetHomeFolder(), defaultRecordKey);
				}
				if (upph == null)
				{
					upph = ResumableUploader.DefaultUploadProgressHandler;
				}
				if (upctl == null)
				{
					upctl = ResumableUploader.DefaultUploadController;
				}
				return new ResumableUploader(UPLOAD_FROM_CDN, CHUNK_UNIT).UploadFile(localFile, saveKey, token, recordFile, upph, upctl);
			}
			return new FormUploader(UPLOAD_FROM_CDN).UploadFile(localFile, saveKey, token);
		}

		public HttpResult UploadData(byte[] data, string saveKey, string token)
		{
			HttpResult httpResult = new HttpResult();
			if (data.Length > PUT_THRESHOLD)
			{
				return new ResumableUploader(UPLOAD_FROM_CDN).UploadData(data, saveKey, token, null);
			}
			return new FormUploader(UPLOAD_FROM_CDN).UploadData(data, saveKey, token);
		}

		public HttpResult UploadStream(Stream stream, string saveKey, string token)
		{
			HttpResult httpResult = new HttpResult();
			if (stream.Length > PUT_THRESHOLD)
			{
				return new ResumableUploader(UPLOAD_FROM_CDN).UploadStream(stream, saveKey, token, null);
			}
			return new FormUploader(UPLOAD_FROM_CDN).UploadStream(stream, saveKey, token);
		}
	}
}
