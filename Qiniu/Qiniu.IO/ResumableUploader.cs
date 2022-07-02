using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Qiniu.Common;
using Qiniu.Http;
using Qiniu.IO.Model;
using Qiniu.JSON;
using Qiniu.Util;

namespace Qiniu.IO
{
	public class ResumableUploader
	{
		private long CHUNK_SIZE;

		private const long BLOCK_SIZE = 4194304L;

		private string uploadHost;

		private HttpManager httpManager;

		public ResumableUploader(bool uploadFromCDN = false, ChunkUnit chunkUnit = ChunkUnit.U2048K)
		{
			uploadHost = (uploadFromCDN ? Config.ZONE.UploadHost : Config.ZONE.UpHost);
			httpManager = new HttpManager(false);
			CHUNK_SIZE = RCU.GetChunkSize(chunkUnit);
		}

		public HttpResult UploadFile(string localFile, string saveKey, string token)
		{
			string defaultRecordKey = ResumeHelper.GetDefaultRecordKey(localFile, saveKey);
			string recordFile = Path.Combine(UserEnv.GetHomeFolder(), defaultRecordKey);
			UploadProgressHandler uppHandler = DefaultUploadProgressHandler;
			return UploadFile(localFile, saveKey, token, recordFile, uppHandler);
		}

		public HttpResult UploadFile(string localFile, string saveKey, string token, string recordFile)
		{
			UploadProgressHandler uppHandler = DefaultUploadProgressHandler;
			return UploadFile(localFile, saveKey, token, recordFile, uppHandler);
		}

		public HttpResult UploadFile(string localFile, string saveKey, string token, string recordFile, UploadProgressHandler uppHandler)
		{
			HttpResult httpResult = new HttpResult();
			FileStream fileStream = null;
			if (uppHandler == null)
			{
				uppHandler = DefaultUploadProgressHandler;
			}
			try
			{
				fileStream = new FileStream(localFile, FileMode.Open);
				long length = fileStream.Length;
				long cHUNK_SIZE = CHUNK_SIZE;
				long num = 4194304L;
				byte[] array = new byte[cHUNK_SIZE];
				int num2 = (int)((length + num - 1) / num);
				ResumeInfo resumeInfo = ResumeHelper.Load(recordFile);
				if (resumeInfo == null)
				{
					ResumeInfo resumeInfo2 = new ResumeInfo();
					resumeInfo2.FileSize = length;
					resumeInfo2.BlockIndex = 0;
					resumeInfo2.BlockCount = num2;
					resumeInfo2.Contexts = new string[num2];
					resumeInfo = resumeInfo2;
					ResumeHelper.Save(resumeInfo, recordFile);
				}
				int num3 = resumeInfo.BlockIndex;
				long num4 = num3 * num;
				string text = null;
				long num5 = length - num4;
				long num6 = 0L;
				long num7 = 0L;
				HttpResult httpResult2 = null;
				ResumeContext obj = null;
				fileStream.Seek(num4, SeekOrigin.Begin);
				while (num5 > 0)
				{
					num = ((num5 >= 4194304) ? 4194304 : num5);
					cHUNK_SIZE = ((num5 >= CHUNK_SIZE) ? CHUNK_SIZE : num5);
					fileStream.Read(array, 0, (int)cHUNK_SIZE);
					httpResult2 = mkblk(array, num, cHUNK_SIZE, token);
					if (httpResult2.Code != 200)
					{
						httpResult.Shadow(httpResult2);
						httpResult.RefText += string.Format("[{0}] [ResumableUpload] Error: mkblk: code = {1}, text = {2}, offset = {3}, blockSize = {4}, chunkSize = {5}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), httpResult2.Code, httpResult2.Text, num4, num, cHUNK_SIZE);
						return httpResult;
					}
					if (!JsonHelper.Deserialize<ResumeContext>(httpResult2.Text, out obj))
					{
						httpResult.Shadow(httpResult2);
						httpResult.RefCode = -252;
						httpResult.RefText += string.Format("[{0}] [ResumableUpload] mkblk Error: JSON Decode Error: text = {1}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), httpResult2.Text);
						return httpResult;
					}
					text = obj.Ctx;
					num4 += cHUNK_SIZE;
					num5 -= cHUNK_SIZE;
					uppHandler(num4, length);
					if (num5 > 0)
					{
						num6 = num - cHUNK_SIZE;
						num7 = cHUNK_SIZE;
						while (num6 > 0)
						{
							cHUNK_SIZE = ((num6 >= CHUNK_SIZE) ? CHUNK_SIZE : num6);
							fileStream.Read(array, 0, (int)cHUNK_SIZE);
							httpResult2 = bput(array, num7, cHUNK_SIZE, text, token);
							if (httpResult2.Code != 200)
							{
								httpResult.Shadow(httpResult2);
								httpResult.RefText += string.Format("[{0}] [ResumableUpload] Error: bput: code = {1}, text = {2}, offset = {3}, blockOffset = {4}, chunkSize = {5}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), httpResult2.Code, httpResult2.Text, num4, num7, cHUNK_SIZE);
								return httpResult;
							}
							if (!JsonHelper.Deserialize<ResumeContext>(httpResult2.Text, out obj))
							{
								httpResult.Shadow(httpResult2);
								httpResult.RefCode = -252;
								httpResult.RefText += string.Format("[{0}] [ResumableUpload] bput Error: JSON Decode Error: text = {1}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), httpResult2.Text);
								return httpResult;
							}
							text = obj.Ctx;
							num4 += cHUNK_SIZE;
							num5 -= cHUNK_SIZE;
							num7 += cHUNK_SIZE;
							num6 -= cHUNK_SIZE;
							uppHandler(num4, length);
						}
					}
					resumeInfo.BlockIndex = num3;
					resumeInfo.Contexts[num3] = text;
					ResumeHelper.Save(resumeInfo, recordFile);
					num3++;
				}
				httpResult2 = mkfile(saveKey, length, saveKey, resumeInfo.Contexts, token);
				if (httpResult2.Code != 200)
				{
					httpResult.Shadow(httpResult2);
					httpResult.RefText += string.Format("[{0}] [ResumableUpload] Error: mkfile: code ={1}, text = {2}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), httpResult2.Code, httpResult2.Text);
					return httpResult;
				}
				File.Delete(recordFile);
				httpResult.Shadow(httpResult2);
				httpResult.RefText += string.Format("[{0}] [ResumableUpload] Uploaded: \"{1}\" ==> \"{2}\"\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), localFile, saveKey);
				return httpResult;
			}
			catch (Exception ex)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat("[{0}] [ResumableUpload] Error: ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
				for (Exception ex2 = ex; ex2 != null; ex2 = ex2.InnerException)
				{
					stringBuilder.Append(ex2.Message + " ");
				}
				stringBuilder.AppendLine();
				httpResult.RefCode = -252;
				httpResult.RefText += stringBuilder.ToString();
				return httpResult;
			}
			finally
			{
				if (fileStream != null)
				{
					fileStream.Dispose();
				}
			}
		}

		public HttpResult UploadFile(string localFile, string saveKey, string token, string recordFile, UploadProgressHandler uppHandler, UploadController uploadController)
		{
			HttpResult httpResult = new HttpResult();
			FileStream fileStream = null;
			if (uppHandler == null)
			{
				uppHandler = DefaultUploadProgressHandler;
			}
			try
			{
				fileStream = new FileStream(localFile, FileMode.Open);
				long length = fileStream.Length;
				long cHUNK_SIZE = CHUNK_SIZE;
				long num = 4194304L;
				byte[] array = new byte[cHUNK_SIZE];
				int num2 = (int)((length + num - 1) / num);
				ResumeInfo resumeInfo = ResumeHelper.Load(recordFile);
				if (resumeInfo == null)
				{
					ResumeInfo resumeInfo2 = new ResumeInfo();
					resumeInfo2.FileSize = length;
					resumeInfo2.BlockIndex = 0;
					resumeInfo2.BlockCount = num2;
					resumeInfo2.Contexts = new string[num2];
					resumeInfo = resumeInfo2;
					ResumeHelper.Save(resumeInfo, recordFile);
				}
				int num3 = resumeInfo.BlockIndex;
				long num4 = num3 * num;
				string text = null;
				long num5 = length - num4;
				long num6 = 0L;
				long num7 = 0L;
				HttpResult httpResult2 = null;
				ResumeContext obj = null;
				fileStream.Seek(num4, SeekOrigin.Begin);
				UPTS uPTS = UPTS.Activated;
				bool flag = true;
				ManualResetEvent manualResetEvent = new ManualResetEvent(true);
				while (num5 > 0)
				{
					switch (uploadController())
					{
					case UPTS.Aborted:
						httpResult.Code = -255;
						httpResult.RefCode = -255;
						httpResult.RefText += string.Format("[{0}] [ResumableUpload] Info: upload task is aborted\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
						return httpResult;
					case UPTS.Suspended:
						if (flag)
						{
							flag = false;
							manualResetEvent.Reset();
							httpResult.RefCode = -254;
							httpResult.RefText += string.Format("[{0}] [ResumableUpload] Info: upload task is paused\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
						}
						manualResetEvent.WaitOne(1000);
						continue;
					}
					if (!flag)
					{
						flag = true;
						manualResetEvent.Set();
						httpResult.RefCode = -253;
						httpResult.RefText += string.Format("[{0}] [ResumableUpload] Info: upload task is resumed\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
					}
					num = ((num5 >= 4194304) ? 4194304 : num5);
					cHUNK_SIZE = ((num5 >= CHUNK_SIZE) ? CHUNK_SIZE : num5);
					fileStream.Read(array, 0, (int)cHUNK_SIZE);
					httpResult2 = mkblk(array, num, cHUNK_SIZE, token);
					if (httpResult2.Code != 200)
					{
						httpResult.Shadow(httpResult2);
						httpResult.RefText += string.Format("[{0}] [ResumableUpload] Error: mkblk: code = {1}, text = {2}, offset = {3}, blockSize = {4}, chunkSize = {5}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), httpResult2.Code, httpResult2.Text, num4, num, cHUNK_SIZE);
						return httpResult;
					}
					if (!JsonHelper.Deserialize<ResumeContext>(httpResult2.Text, out obj))
					{
						httpResult.Shadow(httpResult2);
						httpResult.RefCode = -252;
						httpResult.RefText += string.Format("[{0}] [ResumableUpload] mkblk Error: JSON Decode Error: text = {1}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), httpResult2.Text);
						return httpResult;
					}
					text = obj.Ctx;
					num4 += cHUNK_SIZE;
					num5 -= cHUNK_SIZE;
					uppHandler(num4, length);
					if (num5 > 0)
					{
						num6 = num - cHUNK_SIZE;
						num7 = cHUNK_SIZE;
						while (num6 > 0)
						{
							cHUNK_SIZE = ((num6 >= CHUNK_SIZE) ? CHUNK_SIZE : num6);
							fileStream.Read(array, 0, (int)cHUNK_SIZE);
							httpResult2 = bput(array, num7, cHUNK_SIZE, text, token);
							if (httpResult2.Code != 200)
							{
								httpResult.Shadow(httpResult2);
								httpResult.RefText += string.Format("[{0}] [ResumableUpload] Error: bput: code = {1}, text = {2}, offset={3}, blockOffset={4}, chunkSize={5}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), httpResult2.Code, httpResult2.Text, num4, num7, cHUNK_SIZE);
								return httpResult;
							}
							if (!JsonHelper.Deserialize<ResumeContext>(httpResult2.Text, out obj))
							{
								httpResult.Shadow(httpResult2);
								httpResult.RefCode = -252;
								httpResult.RefText += string.Format("[{0}] [ResumableUpload] bput Error: JSON Decode Error: text = {1}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), httpResult2.Text);
								return httpResult;
							}
							text = obj.Ctx;
							num4 += cHUNK_SIZE;
							num5 -= cHUNK_SIZE;
							num7 += cHUNK_SIZE;
							num6 -= cHUNK_SIZE;
							uppHandler(num4, length);
						}
					}
					resumeInfo.BlockIndex = num3;
					resumeInfo.Contexts[num3] = text;
					ResumeHelper.Save(resumeInfo, recordFile);
					num3++;
				}
				httpResult2 = mkfile(saveKey, length, saveKey, resumeInfo.Contexts, token);
				if (httpResult2.Code != 200)
				{
					httpResult.Shadow(httpResult2);
					httpResult.RefText += string.Format("[{0}] [ResumableUpload] Error: mkfile: code = {1}, text = {2}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), httpResult2.Code, httpResult2.Text);
					return httpResult;
				}
				File.Delete(recordFile);
				httpResult.Shadow(httpResult2);
				httpResult.RefText += string.Format("[{0}] [ResumableUpload] Uploaded: \"{1}\" ==> \"{2}\"\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), localFile, saveKey);
				return httpResult;
			}
			catch (Exception ex)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat("[{0}] [ResumableUpload] Error: ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
				for (Exception ex2 = ex; ex2 != null; ex2 = ex2.InnerException)
				{
					stringBuilder.Append(ex2.Message + " ");
				}
				stringBuilder.AppendLine();
				httpResult.RefCode = -252;
				httpResult.RefText += stringBuilder.ToString();
				return httpResult;
			}
			finally
			{
				if (fileStream != null)
				{
					fileStream.Dispose();
				}
			}
		}

		public HttpResult UploadFile(string localFile, string saveKey, string token, int maxTry)
		{
			string defaultRecordKey = ResumeHelper.GetDefaultRecordKey(localFile, saveKey);
			string recordFile = Path.Combine(UserEnv.GetHomeFolder(), defaultRecordKey);
			UploadProgressHandler uppHandler = DefaultUploadProgressHandler;
			return UploadFile(localFile, saveKey, token, recordFile, maxTry, uppHandler);
		}

		public HttpResult UploadFile(string localFile, string saveKey, string token, string recordFile, int maxTry)
		{
			UploadProgressHandler uppHandler = DefaultUploadProgressHandler;
			return UploadFile(localFile, saveKey, token, recordFile, maxTry, uppHandler);
		}

		public HttpResult UploadFile(string localFile, string saveKey, string token, string recordFile, int maxTry, UploadProgressHandler uppHandler)
		{
			HttpResult httpResult = new HttpResult();
			FileStream fileStream = null;
			int maxTry2 = getMaxTry(maxTry);
			if (uppHandler == null)
			{
				uppHandler = DefaultUploadProgressHandler;
			}
			try
			{
				fileStream = new FileStream(localFile, FileMode.Open);
				long length = fileStream.Length;
				long cHUNK_SIZE = CHUNK_SIZE;
				long num = 4194304L;
				byte[] array = new byte[cHUNK_SIZE];
				int num2 = (int)((length + num - 1) / num);
				ResumeInfo resumeInfo = ResumeHelper.Load(recordFile);
				if (resumeInfo == null)
				{
					ResumeInfo resumeInfo2 = new ResumeInfo();
					resumeInfo2.FileSize = length;
					resumeInfo2.BlockIndex = 0;
					resumeInfo2.BlockCount = num2;
					resumeInfo2.Contexts = new string[num2];
					resumeInfo = resumeInfo2;
					ResumeHelper.Save(resumeInfo, recordFile);
				}
				int num3 = resumeInfo.BlockIndex;
				long num4 = num3 * num;
				string text = null;
				long num5 = length - num4;
				long num6 = 0L;
				long num7 = 0L;
				HttpResult httpResult2 = null;
				ResumeContext obj = null;
				fileStream.Seek(num4, SeekOrigin.Begin);
				int num8 = 0;
				while (num5 > 0)
				{
					num = ((num5 >= 4194304) ? 4194304 : num5);
					cHUNK_SIZE = ((num5 >= CHUNK_SIZE) ? CHUNK_SIZE : num5);
					fileStream.Read(array, 0, (int)cHUNK_SIZE);
					num8 = 0;
					while (++num8 <= maxTry2)
					{
						httpResult.RefText += string.Format("[{0}] [ResumableUpload] try mkblk#{1}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), num8);
						httpResult2 = mkblkChecked(array, num, cHUNK_SIZE, token);
						if (httpResult2.Code == 200 && httpResult2.RefCode != -252)
						{
							break;
						}
					}
					if (httpResult2.Code != 200 || httpResult2.RefCode == -252)
					{
						httpResult.Shadow(httpResult2);
						httpResult.RefText += string.Format("[{0}] [ResumableUpload] Error: mkblk: code = {1}, text = {2}, offset = {3}, blockSize={4}, chunkSize={5}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), httpResult2.Code, httpResult2.Text, num4, num, cHUNK_SIZE);
						return httpResult;
					}
					if (!JsonHelper.Deserialize<ResumeContext>(httpResult2.Text, out obj))
					{
						httpResult.Shadow(httpResult2);
						httpResult.RefCode = -252;
						httpResult.RefText += string.Format("[{0}] [ResumableUpload] mkblk Error: JSON Decode Error: text = {1}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), httpResult2.Text);
						return httpResult;
					}
					text = obj.Ctx;
					num4 += cHUNK_SIZE;
					num5 -= cHUNK_SIZE;
					uppHandler(num4, length);
					if (num5 > 0)
					{
						num6 = num - cHUNK_SIZE;
						num7 = cHUNK_SIZE;
						while (num6 > 0)
						{
							cHUNK_SIZE = ((num6 >= CHUNK_SIZE) ? CHUNK_SIZE : num6);
							fileStream.Read(array, 0, (int)cHUNK_SIZE);
							num8 = 0;
							while (++num8 <= maxTry2)
							{
								httpResult.RefText += string.Format("[{0}] [ResumableUpload] try bput#{1}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), num8);
								httpResult2 = bputChecked(array, num7, cHUNK_SIZE, text, token);
								if (httpResult2.Code == 200 && httpResult2.RefCode != -252)
								{
									break;
								}
							}
							if (httpResult2.Code != 200 || httpResult2.RefCode == -252)
							{
								httpResult.Shadow(httpResult2);
								httpResult.RefText += string.Format("[{0}] [ResumableUpload] Error: bput: code = {1}, text = {2}, offset = {3}, blockOffset = {4}, chunkSize = {5}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), httpResult2.Code, httpResult2.Text, num4, num7, cHUNK_SIZE);
								return httpResult;
							}
							if (!JsonHelper.Deserialize<ResumeContext>(httpResult2.Text, out obj))
							{
								httpResult.Shadow(httpResult2);
								httpResult.RefCode = -252;
								httpResult.RefText += string.Format("[{0}] [ResumableUpload] bput Error: JSON Decode Error: text = {1}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), httpResult2.Text);
								return httpResult;
							}
							text = obj.Ctx;
							num4 += cHUNK_SIZE;
							num5 -= cHUNK_SIZE;
							num7 += cHUNK_SIZE;
							num6 -= cHUNK_SIZE;
							uppHandler(num4, length);
						}
					}
					resumeInfo.BlockIndex = num3;
					resumeInfo.Contexts[num3] = text;
					ResumeHelper.Save(resumeInfo, recordFile);
					num3++;
				}
				httpResult2 = mkfile(saveKey, length, saveKey, resumeInfo.Contexts, token);
				if (httpResult2.Code != 200)
				{
					httpResult.Shadow(httpResult2);
					httpResult.RefText += string.Format("[{0}] [ResumableUpload] Error: mkfile: code = {1}, text = {2}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), httpResult2.Code, httpResult2.Text);
					return httpResult;
				}
				File.Delete(recordFile);
				httpResult.Shadow(httpResult2);
				httpResult.RefText += string.Format("[{0}] [ResumableUpload] Uploaded: \"{1}\" ==> \"{2}\"\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), localFile, saveKey);
				return httpResult;
			}
			catch (Exception ex)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat("[{0}] [ResumableUpload] Error: ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
				for (Exception ex2 = ex; ex2 != null; ex2 = ex2.InnerException)
				{
					stringBuilder.Append(ex2.Message + " ");
				}
				stringBuilder.AppendLine();
				httpResult.RefCode = -252;
				httpResult.RefText += stringBuilder.ToString();
				return httpResult;
			}
			finally
			{
				if (fileStream != null)
				{
					fileStream.Dispose();
				}
			}
		}

		public HttpResult UploadFile(string localFile, string saveKey, string token, string recordFile, int maxTry, UploadProgressHandler uppHandler, UploadController uploadController)
		{
			HttpResult httpResult = new HttpResult();
			FileStream fileStream = null;
			int maxTry2 = getMaxTry(maxTry);
			if (uppHandler == null)
			{
				uppHandler = DefaultUploadProgressHandler;
			}
			try
			{
				fileStream = new FileStream(localFile, FileMode.Open);
				long length = fileStream.Length;
				long cHUNK_SIZE = CHUNK_SIZE;
				long num = 4194304L;
				byte[] array = new byte[cHUNK_SIZE];
				int num2 = (int)((length + num - 1) / num);
				ResumeInfo resumeInfo = ResumeHelper.Load(recordFile);
				if (resumeInfo == null)
				{
					ResumeInfo resumeInfo2 = new ResumeInfo();
					resumeInfo2.FileSize = length;
					resumeInfo2.BlockIndex = 0;
					resumeInfo2.BlockCount = num2;
					resumeInfo2.Contexts = new string[num2];
					resumeInfo = resumeInfo2;
					ResumeHelper.Save(resumeInfo, recordFile);
				}
				int num3 = resumeInfo.BlockIndex;
				long num4 = num3 * num;
				string text = null;
				long num5 = length - num4;
				long num6 = 0L;
				long num7 = 0L;
				HttpResult httpResult2 = null;
				ResumeContext obj = null;
				fileStream.Seek(num4, SeekOrigin.Begin);
				UPTS uPTS = UPTS.Activated;
				bool flag = true;
				ManualResetEvent manualResetEvent = new ManualResetEvent(true);
				int num8 = 0;
				while (num5 > 0)
				{
					switch (uploadController())
					{
					case UPTS.Aborted:
						httpResult.Code = -255;
						httpResult.RefCode = -255;
						httpResult.RefText += string.Format("[{0}] [ResumableUpload] Info: upload task is aborted\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
						return httpResult;
					case UPTS.Suspended:
						if (flag)
						{
							flag = false;
							manualResetEvent.Reset();
							httpResult.RefCode = -254;
							httpResult.RefText += string.Format("[{0}] [ResumableUpload] Info: upload task is paused\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
						}
						manualResetEvent.WaitOne(1000);
						continue;
					}
					if (!flag)
					{
						flag = true;
						manualResetEvent.Set();
						httpResult.RefCode = -253;
						httpResult.RefText += string.Format("[{0}] [ResumableUpload] Info: upload task is resumed\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
					}
					num = ((num5 >= 4194304) ? 4194304 : num5);
					cHUNK_SIZE = ((num5 >= CHUNK_SIZE) ? CHUNK_SIZE : num5);
					fileStream.Read(array, 0, (int)cHUNK_SIZE);
					num8 = 0;
					while (++num8 <= maxTry2)
					{
						httpResult.RefText += string.Format("[{0}] [ResumableUpload] try mkblk#{1}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), num8);
						httpResult2 = mkblkChecked(array, num, cHUNK_SIZE, token);
						if (httpResult2.Code == 200 && httpResult2.RefCode != -252)
						{
							break;
						}
					}
					if (httpResult2.Code != 200 || httpResult2.RefCode == -252)
					{
						httpResult.Shadow(httpResult2);
						httpResult.RefText += string.Format("[{0}] [ResumableUpload] Error: mkblk: code = {1}, text = {2}, offset = {3}, blockSize = {4}, chunkSize = {5}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), httpResult2.Code, httpResult2.Text, num4, num, cHUNK_SIZE);
						return httpResult;
					}
					if (!JsonHelper.Deserialize<ResumeContext>(httpResult2.Text, out obj))
					{
						httpResult.Shadow(httpResult2);
						httpResult.RefCode = -252;
						httpResult.RefText += string.Format("[{0}] [ResumableUpload] mkblk Error: JSON Decode Error: text = {1}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), httpResult2.Text);
						return httpResult;
					}
					text = obj.Ctx;
					num4 += cHUNK_SIZE;
					num5 -= cHUNK_SIZE;
					uppHandler(num4, length);
					if (num5 > 0)
					{
						num6 = num - cHUNK_SIZE;
						num7 = cHUNK_SIZE;
						while (num6 > 0)
						{
							cHUNK_SIZE = ((num6 >= CHUNK_SIZE) ? CHUNK_SIZE : num6);
							fileStream.Read(array, 0, (int)cHUNK_SIZE);
							num8 = 0;
							while (++num8 <= maxTry2)
							{
								httpResult.RefText += string.Format("[{0}] [ResumableUpload] try bput#{1}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), num8);
								httpResult2 = bputChecked(array, num7, cHUNK_SIZE, text, token);
								if (httpResult2.Code == 200 && httpResult2.RefCode != -252)
								{
									break;
								}
							}
							if (httpResult2.Code != 200 || httpResult2.RefCode == -252)
							{
								httpResult.Shadow(httpResult2);
								httpResult.RefText += string.Format("[{0}] [ResumableUpload] Error: bput: code = {1}, text = {2}, offset = {3}, blockOffset = {4}, chunkSize = {5}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), httpResult2.Code, httpResult2.Text, num4, num7, cHUNK_SIZE);
								return httpResult;
							}
							if (!JsonHelper.Deserialize<ResumeContext>(httpResult2.Text, out obj))
							{
								httpResult.Shadow(httpResult2);
								httpResult.RefCode = -252;
								httpResult.RefText += string.Format("[{0}] [ResumableUpload] bput Error: JSON Decode Error: text = {1}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), httpResult2.Text);
								return httpResult;
							}
							text = obj.Ctx;
							num4 += cHUNK_SIZE;
							num5 -= cHUNK_SIZE;
							num7 += cHUNK_SIZE;
							num6 -= cHUNK_SIZE;
							uppHandler(num4, length);
						}
					}
					resumeInfo.BlockIndex = num3;
					resumeInfo.Contexts[num3] = text;
					ResumeHelper.Save(resumeInfo, recordFile);
					num3++;
				}
				httpResult2 = mkfile(saveKey, length, saveKey, resumeInfo.Contexts, token);
				if (httpResult2.Code != 200)
				{
					httpResult.Shadow(httpResult2);
					httpResult.RefText += string.Format("[{0}] [ResumableUpload] Error: mkfile: code = {1}, text = {2}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), httpResult2.Code, httpResult2.Text);
					return httpResult;
				}
				File.Delete(recordFile);
				httpResult.Shadow(httpResult2);
				httpResult.RefText += string.Format("[{0}] [ResumableUpload] Uploaded: \"{1}\" ==> \"{2}\"\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), localFile, saveKey);
				return httpResult;
			}
			catch (Exception ex)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat("[{0}] [ResumableUpload] Error: ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
				for (Exception ex2 = ex; ex2 != null; ex2 = ex2.InnerException)
				{
					stringBuilder.Append(ex2.Message + " ");
				}
				stringBuilder.AppendLine();
				httpResult.RefCode = -252;
				httpResult.RefText += stringBuilder.ToString();
				return httpResult;
			}
			finally
			{
				if (fileStream != null)
				{
					fileStream.Dispose();
				}
			}
		}

		public HttpResult UploadFile(string localFile, string saveKey, string token, string recordFile, int maxTry, UploadProgressHandler uppHandler, UploadController uploadController, Dictionary<string, string> extraParams)
		{
			HttpResult httpResult = new HttpResult();
			FileStream fileStream = null;
			int maxTry2 = getMaxTry(maxTry);
			if (uppHandler == null)
			{
				uppHandler = DefaultUploadProgressHandler;
			}
			try
			{
				fileStream = new FileStream(localFile, FileMode.Open);
				long length = fileStream.Length;
				long cHUNK_SIZE = CHUNK_SIZE;
				long num = 4194304L;
				byte[] array = new byte[cHUNK_SIZE];
				int num2 = (int)((length + num - 1) / num);
				ResumeInfo resumeInfo = ResumeHelper.Load(recordFile);
				if (resumeInfo == null)
				{
					ResumeInfo resumeInfo2 = new ResumeInfo();
					resumeInfo2.FileSize = length;
					resumeInfo2.BlockIndex = 0;
					resumeInfo2.BlockCount = num2;
					resumeInfo2.Contexts = new string[num2];
					resumeInfo = resumeInfo2;
					ResumeHelper.Save(resumeInfo, recordFile);
				}
				int num3 = resumeInfo.BlockIndex;
				long num4 = num3 * num;
				string text = null;
				long num5 = length - num4;
				long num6 = 0L;
				long num7 = 0L;
				HttpResult httpResult2 = null;
				ResumeContext obj = null;
				fileStream.Seek(num4, SeekOrigin.Begin);
				UPTS uPTS = UPTS.Activated;
				bool flag = true;
				ManualResetEvent manualResetEvent = new ManualResetEvent(true);
				int num8 = 0;
				while (num5 > 0)
				{
					switch (uploadController())
					{
					case UPTS.Aborted:
						httpResult.Code = -255;
						httpResult.RefCode = -255;
						httpResult.RefText += string.Format("[{0}] [ResumableUpload] Info: upload task is aborted\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
						return httpResult;
					case UPTS.Suspended:
						if (flag)
						{
							flag = false;
							manualResetEvent.Reset();
							httpResult.RefCode = -254;
							httpResult.RefText += string.Format("[{0}] [ResumableUpload] Info: upload task is paused\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
						}
						manualResetEvent.WaitOne(1000);
						continue;
					}
					if (!flag)
					{
						flag = true;
						manualResetEvent.Set();
						httpResult.RefCode = -253;
						httpResult.RefText += string.Format("[{0}] [ResumableUpload] Info: upload task is resumed\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
					}
					num = ((num5 >= 4194304) ? 4194304 : num5);
					cHUNK_SIZE = ((num5 >= CHUNK_SIZE) ? CHUNK_SIZE : num5);
					fileStream.Read(array, 0, (int)cHUNK_SIZE);
					num8 = 0;
					while (++num8 <= maxTry2)
					{
						httpResult.RefText += string.Format("[{0}] [ResumableUpload] try mkblk#{1}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), num8);
						httpResult2 = mkblkChecked(array, num, cHUNK_SIZE, token);
						if (httpResult2.Code == 200 && httpResult2.RefCode != -252)
						{
							break;
						}
					}
					if (httpResult2.Code != 200 || httpResult2.RefCode == -252)
					{
						httpResult.Shadow(httpResult2);
						httpResult.RefText += string.Format("[{0}] [ResumableUpload] Error: mkblk: code = {1}, text = {2}, offset = {3}, blockSize = {4}, chunkSize = {5}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), httpResult2.Code, httpResult2.Text, num4, num, cHUNK_SIZE);
						return httpResult;
					}
					if (!JsonHelper.Deserialize<ResumeContext>(httpResult2.Text, out obj))
					{
						httpResult.Shadow(httpResult2);
						httpResult.RefCode = -252;
						httpResult.RefText += string.Format("[{0}] [ResumableUpload] mkblk Error: JSON Decode Error: text = {1}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), httpResult2.Text);
						return httpResult;
					}
					text = obj.Ctx;
					num4 += cHUNK_SIZE;
					num5 -= cHUNK_SIZE;
					uppHandler(num4, length);
					if (num5 > 0)
					{
						num6 = num - cHUNK_SIZE;
						num7 = cHUNK_SIZE;
						while (num6 > 0)
						{
							cHUNK_SIZE = ((num6 >= CHUNK_SIZE) ? CHUNK_SIZE : num6);
							fileStream.Read(array, 0, (int)cHUNK_SIZE);
							num8 = 0;
							while (++num8 <= maxTry2)
							{
								httpResult.RefText += string.Format("[{0}] [ResumableUpload] try bput#{1}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), num8);
								httpResult2 = bputChecked(array, num7, cHUNK_SIZE, text, token);
								if (httpResult2.Code == 200 && httpResult2.RefCode != -252)
								{
									break;
								}
							}
							if (httpResult2.Code != 200 || httpResult2.RefCode == -252)
							{
								httpResult.Shadow(httpResult2);
								httpResult.RefText += string.Format("[{0}] [ResumableUpload] Error: bput: code = {1}, text = {2}, offset = {3}, blockOffset = {4}, chunkSize = {5}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), httpResult2.Code, httpResult2.Text, num4, num7, cHUNK_SIZE);
								return httpResult;
							}
							if (!JsonHelper.Deserialize<ResumeContext>(httpResult2.Text, out obj))
							{
								httpResult.Shadow(httpResult2);
								httpResult.RefCode = -252;
								httpResult.RefText += string.Format("[{0}] [ResumableUpload] bput Error: JSON Decode Error: text = {1}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), httpResult2.Text);
								return httpResult;
							}
							text = obj.Ctx;
							num4 += cHUNK_SIZE;
							num5 -= cHUNK_SIZE;
							num7 += cHUNK_SIZE;
							num6 -= cHUNK_SIZE;
							uppHandler(num4, length);
						}
					}
					resumeInfo.BlockIndex = num3;
					resumeInfo.Contexts[num3] = text;
					ResumeHelper.Save(resumeInfo, recordFile);
					num3++;
				}
				httpResult2 = mkfile(saveKey, length, saveKey, resumeInfo.Contexts, token, extraParams);
				if (httpResult2.Code != 200)
				{
					httpResult.Shadow(httpResult2);
					httpResult.RefText += string.Format("[{0}] [ResumableUpload] Error: mkfile: code = {1}, text = {2}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), httpResult2.Code, httpResult2.Text);
					return httpResult;
				}
				File.Delete(recordFile);
				httpResult.Shadow(httpResult2);
				httpResult.RefText += string.Format("[{0}] [ResumableUpload] Uploaded: \"{1}\" ==> \"{2}\"\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), localFile, saveKey);
				return httpResult;
			}
			catch (Exception ex)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat("[{0}] [ResumableUpload] Error: ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
				for (Exception ex2 = ex; ex2 != null; ex2 = ex2.InnerException)
				{
					stringBuilder.Append(ex2.Message + " ");
				}
				stringBuilder.AppendLine();
				httpResult.RefCode = -252;
				httpResult.RefText += stringBuilder.ToString();
				return httpResult;
			}
			finally
			{
				if (fileStream != null)
				{
					fileStream.Dispose();
				}
			}
		}

		public static byte[] ReadToByteArray(string file)
		{
			byte[] array = null;
			using (FileStream fileStream = new FileStream(file, FileMode.Open))
			{
				array = new byte[fileStream.Length];
				fileStream.Read(array, 0, (int)fileStream.Length);
				return array;
			}
		}

		public HttpResult UploadData(byte[] data, string saveKey, string token, UploadProgressHandler uppHandler)
		{
			HttpResult httpResult = new HttpResult();
			if (uppHandler == null)
			{
				uppHandler = DefaultUploadProgressHandler;
			}
			MemoryStream memoryStream = null;
			try
			{
				memoryStream = new MemoryStream(data);
				long num = data.Length;
				long cHUNK_SIZE = CHUNK_SIZE;
				long num2 = 4194304L;
				byte[] array = new byte[cHUNK_SIZE];
				int num3 = (int)((num + num2 - 1) / num2);
				ResumeInfo resumeInfo = new ResumeInfo();
				resumeInfo.FileSize = 0L;
				resumeInfo.BlockIndex = 0;
				resumeInfo.BlockCount = num3;
				resumeInfo.Contexts = new string[num3];
				ResumeInfo resumeInfo2 = resumeInfo;
				int num4 = 0;
				long num5 = 0L;
				string text = null;
				long num6 = num;
				long num7 = 0L;
				long num8 = 0L;
				HttpResult httpResult2 = null;
				ResumeContext obj = null;
				while (num6 > 0)
				{
					num2 = ((num6 >= 4194304) ? 4194304 : num6);
					cHUNK_SIZE = ((num6 >= CHUNK_SIZE) ? CHUNK_SIZE : num6);
					memoryStream.Read(array, 0, (int)cHUNK_SIZE);
					httpResult2 = mkblk(array, num2, cHUNK_SIZE, token);
					if (httpResult2.Code != 200)
					{
						httpResult.Shadow(httpResult2);
						httpResult.RefText += string.Format("[{0}] [ResumableUpload] Error: mkblk: code = {1}, text = {2}, offset = {3}, blockSize = {4}, chunkSize = {5}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), httpResult2.Code, httpResult2.Text, num5, num2, cHUNK_SIZE);
						return httpResult;
					}
					if (!JsonHelper.Deserialize<ResumeContext>(httpResult2.Text, out obj))
					{
						httpResult.Shadow(httpResult2);
						httpResult.RefCode = -252;
						httpResult.RefText += string.Format("[{0}] [ResumableUpload] mkblk Error: JSON Decode Error: text = {1}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), httpResult2.Text);
						return httpResult;
					}
					text = obj.Ctx;
					num5 += cHUNK_SIZE;
					num6 -= cHUNK_SIZE;
					uppHandler(num5, num);
					if (num6 > 0)
					{
						num7 = num2 - cHUNK_SIZE;
						num8 = cHUNK_SIZE;
						while (num7 > 0)
						{
							cHUNK_SIZE = ((num7 >= CHUNK_SIZE) ? CHUNK_SIZE : num7);
							memoryStream.Read(array, 0, (int)cHUNK_SIZE);
							httpResult2 = bput(array, num8, cHUNK_SIZE, text, token);
							if (httpResult2.Code != 200)
							{
								httpResult.Shadow(httpResult2);
								httpResult.RefText += string.Format("[{0}] [ResumableUpload] Error: bput: code = {1}, text = {2}, offset = {3}, blockOffset = {4}, chunkSize = {5}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), httpResult2.Code, httpResult2.Text, num5, num8, cHUNK_SIZE);
								return httpResult;
							}
							if (!JsonHelper.Deserialize<ResumeContext>(httpResult2.Text, out obj))
							{
								httpResult.Shadow(httpResult2);
								httpResult.RefCode = -252;
								httpResult.RefText += string.Format("[{0}] [ResumableUpload] bput Error: JSON Decode Error: text = {1}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), httpResult2.Text);
								return httpResult;
							}
							text = obj.Ctx;
							num5 += cHUNK_SIZE;
							num6 -= cHUNK_SIZE;
							num8 += cHUNK_SIZE;
							num7 -= cHUNK_SIZE;
							uppHandler(num5, num);
						}
					}
					resumeInfo2.BlockIndex = num4;
					resumeInfo2.Contexts[num4] = text;
					num4++;
				}
				httpResult2 = mkfile(num, saveKey, resumeInfo2.Contexts, token);
				if (httpResult2.Code != 200)
				{
					httpResult.Shadow(httpResult2);
					httpResult.RefText += string.Format("[{0}] [ResumableUpload] Error: mkfile: code = {1}, text = {2}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), httpResult2.Code, httpResult2.Text);
					return httpResult;
				}
				httpResult.Shadow(httpResult2);
				httpResult.RefText += string.Format("[{0}] [ResumableUpload] Uploaded: \"#DATA#\" ==> \"{1}\"\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), saveKey);
				return httpResult;
			}
			catch (Exception ex)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat("[{0}] [ResumableUpload] Error: ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
				for (Exception ex2 = ex; ex2 != null; ex2 = ex2.InnerException)
				{
					stringBuilder.Append(ex2.Message + " ");
				}
				stringBuilder.AppendLine();
				httpResult.RefCode = -252;
				httpResult.RefText += stringBuilder.ToString();
				return httpResult;
			}
			finally
			{
				if (memoryStream != null)
				{
					memoryStream.Dispose();
				}
			}
		}

		public HttpResult UploadStream(Stream stream, string saveKey, string token, StreamProgressHandler spHandler)
		{
			HttpResult httpResult = new HttpResult();
			if (spHandler == null)
			{
				spHandler = DefaultStreamProgressHandler;
			}
			try
			{
				long num = 0L;
				int num2 = 4194304;
				int num3 = 0;
				byte[] array = new byte[num2];
				ResumeInfo resumeInfo = new ResumeInfo();
				resumeInfo.FileSize = 0L;
				resumeInfo.BlockIndex = 0;
				resumeInfo.BlockCount = 0;
				resumeInfo.SContexts = new List<string>();
				ResumeInfo resumeInfo2 = resumeInfo;
				int num4 = 0;
				long num5 = 0L;
				string text = null;
				HttpResult httpResult2 = null;
				ResumeContext obj = null;
				while (true)
				{
					num3 = stream.Read(array, 0, num2);
					if (num3 == 0)
					{
						break;
					}
					httpResult2 = ((num3 >= num2) ? mkblk(array, num2, num3, token) : mkblk(array, num3, num3, token));
					if (httpResult2.Code != 200)
					{
						httpResult.Shadow(httpResult2);
						httpResult.RefText += string.Format("[{0}] [ResumableUpload] Error: mkblk: code = {1}, text = {2}, offset = {3}, blockSize = {4}, chunkSize = {5}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), httpResult2.Code, httpResult2.Text, num5, num2, num3);
						return httpResult;
					}
					if (!JsonHelper.Deserialize<ResumeContext>(httpResult2.Text, out obj))
					{
						httpResult.Shadow(httpResult2);
						httpResult.RefCode = -252;
						httpResult.RefText += string.Format("[{0}] [ResumableUpload] mkblk Error: JSON Decode Error: text = {1}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), httpResult2.Text);
						return httpResult;
					}
					text = obj.Ctx;
					num5 += num3;
					num += num3;
					spHandler(num5);
					resumeInfo2.FileSize = num;
					resumeInfo2.BlockIndex = num4;
					resumeInfo2.BlockCount = num4 + 1;
					resumeInfo2.SContexts.Add(text);
					num4++;
				}
				httpResult2 = mkfile(num, saveKey, resumeInfo2.SContexts, token);
				spHandler(-1L);
				if (httpResult2.Code != 200)
				{
					httpResult.Shadow(httpResult2);
					httpResult.RefText += string.Format("[{0}] [ResumableUpload] Error: mkfile: code = {1}, text = {2}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), httpResult2.Code, httpResult2.Text);
					return httpResult;
				}
				httpResult.Shadow(httpResult2);
				httpResult.RefText += string.Format("[{0}] [ResumableUpload] Uploaded: #STREAM# ==> \"{1}\"\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), saveKey);
				return httpResult;
			}
			catch (Exception ex)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat("[{0}] [ResumableUpload] Error: ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
				for (Exception ex2 = ex; ex2 != null; ex2 = ex2.InnerException)
				{
					stringBuilder.Append(ex2.Message + " ");
				}
				stringBuilder.AppendLine();
				httpResult.RefCode = -252;
				httpResult.RefText += stringBuilder.ToString();
				return httpResult;
			}
			finally
			{
				stream.Dispose();
			}
		}

		private HttpResult mkfile(long size, IList<string> contexts, string token)
		{
			HttpResult httpResult = new HttpResult();
			try
			{
				string url = string.Format("{0}/mkfile/{1}", uploadHost, size);
				string data = StringHelper.Join(contexts, ",");
				string token2 = string.Format("UpToken {0}", token);
				httpResult = httpManager.PostText(url, data, token2);
				return httpResult;
			}
			catch (Exception ex)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat("[{0}] mkfile Error: ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
				for (Exception ex2 = ex; ex2 != null; ex2 = ex2.InnerException)
				{
					stringBuilder.Append(ex2.Message + " ");
				}
				stringBuilder.AppendLine();
				httpResult.RefCode = -252;
				httpResult.RefText += stringBuilder.ToString();
				return httpResult;
			}
		}

		private HttpResult mkfile(string fileName, long size, IList<string> contexts, string token)
		{
			HttpResult httpResult = new HttpResult();
			try
			{
				string url = string.Format("{0}/mkfile/{1}/fname/{2}", uploadHost, size, Base64.UrlSafeBase64Encode(fileName));
				string data = StringHelper.Join(contexts, ",");
				string token2 = string.Format("UpToken {0}", token);
				httpResult = httpManager.PostText(url, data, token2);
				return httpResult;
			}
			catch (Exception ex)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat("[{0}] mkfile Error: ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
				for (Exception ex2 = ex; ex2 != null; ex2 = ex2.InnerException)
				{
					stringBuilder.Append(ex2.Message + " ");
				}
				stringBuilder.AppendLine();
				httpResult.RefCode = -252;
				httpResult.RefText += stringBuilder.ToString();
				return httpResult;
			}
		}

		private HttpResult mkfile(long size, string saveKey, IList<string> contexts, string token)
		{
			HttpResult httpResult = new HttpResult();
			try
			{
				string arg = string.Format("/key/{0}", Base64.UrlSafeBase64Encode(saveKey));
				string url = string.Format("{0}/mkfile/{1}{2}", uploadHost, size, arg);
				string data = StringHelper.Join(contexts, ",");
				string token2 = string.Format("UpToken {0}", token);
				httpResult = httpManager.PostText(url, data, token2);
				return httpResult;
			}
			catch (Exception ex)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat("[{0}] mkfile Error: ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
				for (Exception ex2 = ex; ex2 != null; ex2 = ex2.InnerException)
				{
					stringBuilder.Append(ex2.Message + " ");
				}
				stringBuilder.AppendLine();
				httpResult.RefCode = -252;
				httpResult.RefText += stringBuilder.ToString();
				return httpResult;
			}
		}

		private HttpResult mkfile(string fileName, long size, string saveKey, IList<string> contexts, string token)
		{
			HttpResult httpResult = new HttpResult();
			try
			{
				string text = string.Format("/key/{0}", Base64.UrlSafeBase64Encode(saveKey));
				string text2 = string.Format("/fname/{0}", Base64.UrlSafeBase64Encode(fileName));
				string url = string.Format("{0}/mkfile/{1}{2}{3}", uploadHost, size, text, text2);
				string data = StringHelper.Join(contexts, ",");
				string token2 = string.Format("UpToken {0}", token);
				httpResult = httpManager.PostText(url, data, token2);
				return httpResult;
			}
			catch (Exception ex)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat("[{0}] mkfile Error: ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
				for (Exception ex2 = ex; ex2 != null; ex2 = ex2.InnerException)
				{
					stringBuilder.Append(ex2.Message + " ");
				}
				stringBuilder.AppendLine();
				httpResult.RefCode = -252;
				httpResult.RefText += stringBuilder.ToString();
				return httpResult;
			}
		}

		private HttpResult mkfile(long size, string saveKey, string mimeType, IList<string> contexts, string token)
		{
			HttpResult httpResult = new HttpResult();
			try
			{
				string text = string.Format("/key/{0}", Base64.UrlSafeBase64Encode(saveKey));
				string text2 = string.Format("/mimeType/{0}", Base64.UrlSafeBase64Encode(mimeType));
				string url = string.Format("{0}/mkfile/{1}{2}{3}", uploadHost, size, text, text2);
				string data = StringHelper.Join(contexts, ",");
				string token2 = string.Format("UpToken {0}", token);
				httpResult = httpManager.PostText(url, data, token2);
				return httpResult;
			}
			catch (Exception ex)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat("[{0}] mkfile Error: ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
				for (Exception ex2 = ex; ex2 != null; ex2 = ex2.InnerException)
				{
					stringBuilder.Append(ex2.Message + " ");
				}
				stringBuilder.AppendLine();
				httpResult.RefCode = -252;
				httpResult.RefText += stringBuilder.ToString();
				return httpResult;
			}
		}

		private HttpResult mkfile(string fileName, long size, string saveKey, string mimeType, IList<string> contexts, string token)
		{
			HttpResult httpResult = new HttpResult();
			try
			{
				string text = string.Format("/key/{0}", Base64.UrlSafeBase64Encode(saveKey));
				string text2 = string.Format("/mimeType/{0}", Base64.UrlSafeBase64Encode(mimeType));
				string text3 = string.Format("/fname/{0}", Base64.UrlSafeBase64Encode(fileName));
				string url = string.Format("{0}/mkfile/{1}{2}{3}{4}", uploadHost, size, text, text2, text3);
				string data = StringHelper.Join(contexts, ",");
				string token2 = string.Format("UpToken {0}", token);
				httpResult = httpManager.PostText(url, data, token2);
				return httpResult;
			}
			catch (Exception ex)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat("[{0}] mkfile Error: ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
				for (Exception ex2 = ex; ex2 != null; ex2 = ex2.InnerException)
				{
					stringBuilder.Append(ex2.Message + " ");
				}
				stringBuilder.AppendLine();
				httpResult.RefCode = -252;
				httpResult.RefText += stringBuilder.ToString();
				return httpResult;
			}
		}

		private HttpResult mkfile(long size, string saveKey, IList<string> contexts, string token, Dictionary<string, string> extraParams)
		{
			HttpResult httpResult = new HttpResult();
			try
			{
				string text = string.Format("/key/{0}", Base64.UrlSafeBase64Encode(saveKey));
				string text2 = "";
				if (extraParams != null && extraParams.Count > 0)
				{
					StringBuilder stringBuilder = new StringBuilder();
					foreach (KeyValuePair<string, string> extraParam in extraParams)
					{
						stringBuilder.AppendFormat("/{0}/{1}", extraParam.Key, extraParam.Value);
					}
					text2 = stringBuilder.ToString();
				}
				string url = string.Format("{0}/mkfile/{1}{2}{3}", uploadHost, size, text, text2);
				string data = StringHelper.Join(contexts, ",");
				string token2 = string.Format("UpToken {0}", token);
				httpResult = httpManager.PostText(url, data, token2);
				return httpResult;
			}
			catch (Exception ex)
			{
				StringBuilder stringBuilder2 = new StringBuilder();
				stringBuilder2.AppendFormat("[{0}] mkfile Error: ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
				for (Exception ex2 = ex; ex2 != null; ex2 = ex2.InnerException)
				{
					stringBuilder2.Append(ex2.Message + " ");
				}
				stringBuilder2.AppendLine();
				httpResult.RefCode = -252;
				httpResult.RefText += stringBuilder2.ToString();
				return httpResult;
			}
		}

		private HttpResult mkfile(string fileName, long size, string saveKey, IList<string> contexts, string token, Dictionary<string, string> extraParams)
		{
			HttpResult httpResult = new HttpResult();
			try
			{
				string text = string.Format("/fname/{0}", Base64.UrlSafeBase64Encode(fileName));
				string text2 = string.Format("/key/{0}", Base64.UrlSafeBase64Encode(saveKey));
				string text3 = "";
				if (extraParams != null && extraParams.Count > 0)
				{
					StringBuilder stringBuilder = new StringBuilder();
					foreach (KeyValuePair<string, string> extraParam in extraParams)
					{
						stringBuilder.AppendFormat("/{0}/{1}", extraParam.Key, extraParam.Value);
					}
					text3 = stringBuilder.ToString();
				}
				string url = string.Format("{0}/mkfile/{1}{2}{3}{4}", uploadHost, size, text, text2, text3);
				string data = StringHelper.Join(contexts, ",");
				string token2 = string.Format("UpToken {0}", token);
				httpResult = httpManager.PostText(url, data, token2);
				return httpResult;
			}
			catch (Exception ex)
			{
				StringBuilder stringBuilder2 = new StringBuilder();
				stringBuilder2.AppendFormat("[{0}] mkfile Error: ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
				for (Exception ex2 = ex; ex2 != null; ex2 = ex2.InnerException)
				{
					stringBuilder2.Append(ex2.Message + " ");
				}
				stringBuilder2.AppendLine();
				httpResult.RefCode = -252;
				httpResult.RefText += stringBuilder2.ToString();
				return httpResult;
			}
		}

		private HttpResult mkfile(long size, string saveKey, string mimeType, IList<string> contexts, string token, Dictionary<string, string> extraParams)
		{
			HttpResult httpResult = new HttpResult();
			try
			{
				string text = string.Format("/key/{0}", Base64.UrlSafeBase64Encode(saveKey));
				string text2 = string.Format("/mimeType/{0}", Base64.UrlSafeBase64Encode(mimeType));
				string text3 = "";
				if (extraParams != null && extraParams.Count > 0)
				{
					StringBuilder stringBuilder = new StringBuilder();
					foreach (KeyValuePair<string, string> extraParam in extraParams)
					{
						stringBuilder.AppendFormat("/{0}/{1}", extraParam.Key, extraParam.Value);
					}
					text3 = stringBuilder.ToString();
				}
				string url = string.Format("{0}/mkfile/{1}{2}{3}{4}", uploadHost, size, text, text2, text3);
				string data = StringHelper.Join(contexts, ",");
				string token2 = string.Format("UpToken {0}", token);
				httpResult = httpManager.PostText(url, data, token2);
				return httpResult;
			}
			catch (Exception ex)
			{
				StringBuilder stringBuilder2 = new StringBuilder();
				stringBuilder2.AppendFormat("[{0}] mkfile Error: ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
				for (Exception ex2 = ex; ex2 != null; ex2 = ex2.InnerException)
				{
					stringBuilder2.Append(ex2.Message + " ");
				}
				stringBuilder2.AppendLine();
				httpResult.RefCode = -252;
				httpResult.RefText += stringBuilder2.ToString();
				return httpResult;
			}
		}

		private HttpResult mkfile(string fileName, long size, string saveKey, string mimeType, IList<string> contexts, string token, Dictionary<string, string> extraParams)
		{
			HttpResult httpResult = new HttpResult();
			try
			{
				string text = string.Format("/fname/{0}", Base64.UrlSafeBase64Encode(fileName));
				string text2 = string.Format("/mimeType/{0}", Base64.UrlSafeBase64Encode(mimeType));
				string text3 = string.Format("/key/{0}", Base64.UrlSafeBase64Encode(saveKey));
				string text4 = "";
				if (extraParams != null && extraParams.Count > 0)
				{
					StringBuilder stringBuilder = new StringBuilder();
					foreach (KeyValuePair<string, string> extraParam in extraParams)
					{
						stringBuilder.AppendFormat("/{0}/{1}", extraParam.Key, extraParam.Value);
					}
					text4 = stringBuilder.ToString();
				}
				string url = string.Format("{0}/mkfile/{1}{2}{3}{4}{5}", uploadHost, size, text2, text, text3, text4);
				string data = StringHelper.Join(contexts, ",");
				string token2 = string.Format("UpToken {0}", token);
				httpResult = httpManager.PostText(url, data, token2);
				return httpResult;
			}
			catch (Exception ex)
			{
				StringBuilder stringBuilder2 = new StringBuilder();
				stringBuilder2.AppendFormat("[{0}] mkfile Error: ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
				for (Exception ex2 = ex; ex2 != null; ex2 = ex2.InnerException)
				{
					stringBuilder2.Append(ex2.Message + " ");
				}
				stringBuilder2.AppendLine();
				httpResult.RefCode = -252;
				httpResult.RefText += stringBuilder2.ToString();
				return httpResult;
			}
		}

		private HttpResult mkblk(byte[] chunkBuffer, long blockSize, long chunkSize, string token)
		{
			HttpResult httpResult = new HttpResult();
			try
			{
				string url = string.Format("{0}/mkblk/{1}", uploadHost, blockSize);
				string token2 = string.Format("UpToken {0}", token);
				using (MemoryStream memoryStream = new MemoryStream(chunkBuffer, 0, (int)chunkSize))
				{
					byte[] data = memoryStream.ToArray();
					httpResult = httpManager.PostData(url, data, token2);
					return httpResult;
				}
			}
			catch (Exception ex)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat("[{0}] mkblk Error: ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
				for (Exception ex2 = ex; ex2 != null; ex2 = ex2.InnerException)
				{
					stringBuilder.Append(ex2.Message + " ");
				}
				stringBuilder.AppendLine();
				httpResult.RefCode = -252;
				httpResult.RefText += stringBuilder.ToString();
				return httpResult;
			}
		}

		private HttpResult mkblkChecked(byte[] chunkBuffer, long blockSize, long chunkSize, string token)
		{
			HttpResult httpResult = new HttpResult();
			try
			{
				string url = string.Format("{0}/mkblk/{1}", uploadHost, blockSize);
				string token2 = string.Format("UpToken {0}", token);
				using (MemoryStream memoryStream = new MemoryStream(chunkBuffer, 0, (int)chunkSize))
				{
					byte[] data = memoryStream.ToArray();
					httpResult = httpManager.PostData(url, data, token2);
					if (httpResult.Code == 200)
					{
						Dictionary<string, string> obj = new Dictionary<string, string>();
						JsonHelper.Deserialize<Dictionary<string, string>>(httpResult.Text, out obj);
						if (obj.ContainsKey("crc32"))
						{
							uint num = Convert.ToUInt32(obj["crc32"]);
							uint num2 = CRC32.CheckSumSlice(chunkBuffer, 0, (int)chunkSize);
							if (num != num2)
							{
								httpResult.RefCode = -252;
								httpResult.RefText += string.Format(" CRC32: remote={0}, local={1}\n", num, num2);
								return httpResult;
							}
							return httpResult;
						}
						httpResult.RefText += string.Format("[{0}] JSON Decode Error: text = {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), httpResult.Text);
						httpResult.RefCode = -252;
						return httpResult;
					}
					httpResult.RefCode = -252;
					return httpResult;
				}
			}
			catch (Exception ex)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat("[{0}] mkblk Error: ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
				for (Exception ex2 = ex; ex2 != null; ex2 = ex2.InnerException)
				{
					stringBuilder.Append(ex2.Message + " ");
				}
				stringBuilder.AppendLine();
				httpResult.RefCode = -252;
				httpResult.RefText += stringBuilder.ToString();
				return httpResult;
			}
		}

		private HttpResult bput(byte[] chunkBuffer, long offset, long chunkSize, string context, string token)
		{
			HttpResult httpResult = new HttpResult();
			try
			{
				string url = string.Format("{0}/bput/{1}/{2}", uploadHost, context, offset);
				string token2 = string.Format("UpToken {0}", token);
				using (MemoryStream memoryStream = new MemoryStream(chunkBuffer, 0, (int)chunkSize))
				{
					byte[] data = memoryStream.ToArray();
					httpResult = httpManager.PostData(url, data, token2);
					return httpResult;
				}
			}
			catch (Exception ex)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat("[{0}] bput Error: ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
				for (Exception ex2 = ex; ex2 != null; ex2 = ex2.InnerException)
				{
					stringBuilder.Append(ex2.Message + " ");
				}
				stringBuilder.AppendLine();
				httpResult.RefCode = -252;
				httpResult.RefText += stringBuilder.ToString();
				return httpResult;
			}
		}

		private HttpResult bputChecked(byte[] chunkBuffer, long offset, long chunkSize, string context, string token)
		{
			HttpResult httpResult = new HttpResult();
			try
			{
				string url = string.Format("{0}/bput/{1}/{2}", uploadHost, context, offset);
				string token2 = string.Format("UpToken {0}", token);
				using (MemoryStream memoryStream = new MemoryStream(chunkBuffer, 0, (int)chunkSize))
				{
					byte[] data = memoryStream.ToArray();
					httpResult = httpManager.PostData(url, data, token2);
					if (httpResult.Code == 200)
					{
						Dictionary<string, string> obj = new Dictionary<string, string>();
						JsonHelper.Deserialize<Dictionary<string, string>>(httpResult.Text, out obj);
						if (obj.ContainsKey("crc32"))
						{
							uint num = Convert.ToUInt32(obj["crc32"]);
							uint num2 = CRC32.CheckSumSlice(chunkBuffer, 0, (int)chunkSize);
							if (num != num2)
							{
								httpResult.RefCode = -252;
								httpResult.RefText += string.Format(" CRC32: remote={0}, local={1}\n", num, num2);
								return httpResult;
							}
							return httpResult;
						}
						httpResult.RefText += string.Format("[{0}] JSON Decode Error: text = {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), httpResult.Text);
						httpResult.RefCode = -252;
						return httpResult;
					}
					httpResult.RefCode = -252;
					return httpResult;
				}
			}
			catch (Exception ex)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat("[{0}] bput Error: ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
				for (Exception ex2 = ex; ex2 != null; ex2 = ex2.InnerException)
				{
					stringBuilder.Append(ex2.Message + " ");
				}
				stringBuilder.AppendLine();
				httpResult.RefCode = -252;
				httpResult.RefText += stringBuilder.ToString();
				return httpResult;
			}
		}

		public static void DefaultUploadProgressHandler(long uploadedBytes, long totalBytes)
		{
			if (uploadedBytes < totalBytes)
			{
				Console.WriteLine("[{0}] [ResumableUpload] Progress: {1,7:0.000}%", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), 100.0 * (double)uploadedBytes / (double)totalBytes);
			}
			else
			{
				Console.WriteLine("[{0}] [ResumableUpload] Progress: {1,7:0.000}%\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), 100.0);
			}
		}

		public static void DefaultStreamProgressHandler(long uploadedBytes)
		{
			if (uploadedBytes > 0)
			{
				Console.WriteLine("[{0}] [ResumableUpload] UploadledBytes: {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), uploadedBytes);
			}
			else
			{
				Console.WriteLine("[{0}] [ResumableUpload] Done.\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
			}
		}

		public static UPTS DefaultUploadController()
		{
			return UPTS.Activated;
		}

		private int getMaxTry(int maxTry)
		{
			int num = 5;
			int num2 = 1;
			int num3 = 20;
			if (maxTry < num2)
			{
				return num2;
			}
			if (maxTry > num3)
			{
				return num3;
			}
			return maxTry;
		}
	}
}
