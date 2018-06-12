using System;
using System.Diagnostics;
using System.IO;

namespace TaoBaoGuanJia.Util
{
	public class Log
	{
		private static object lockObject = new object();

		private static string syslogPath = System.Environment.CurrentDirectory;

		private static long syslogMaxSize = 1048576L;

		public static string SyslogPath
		{
			get
			{
				return syslogPath;
			}
			set
			{
				syslogPath = value;
			}
		}

		public static long SyslogMaxSize
		{
			get
			{
				return syslogMaxSize;
			}
			set
			{
				syslogMaxSize = value;
			}
		}

		public static void WriteLog(string message)
		{
			WriteLog(message, true, true);
		}

		public static void WriteStackTraceInfo()
		{
			try
			{
				StackTrace stackTrace = new StackTrace();
				StackFrame[] frames = stackTrace.GetFrames();
				string text = "运行堆栈：" + Environment.NewLine;
				for (int i = 1; i < frames.Length; i++)
				{
					text = text + frames[i].ToString() + Environment.NewLine;
				}
				if (!string.IsNullOrEmpty(text))
				{
					text = text + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + Environment.NewLine;
					WriteLog(text);
				}
			}
			catch
			{
			}
		}

		public static void WriteLog(string message, bool needWriteTime, bool needChangeLine)
		{
			try
			{
				lock (lockObject)
				{
					string text = syslogPath + "/sysLog.txt";
					if (!File.Exists(text))
					{
						FileStream fileStream = File.Create(text);
						fileStream.Close();
					}
					FileInfo fileInfo = new FileInfo(text);
					if (fileInfo.Length >= syslogMaxSize)
					{
						File.Move(text, syslogPath + "/sysLog" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + ".txt");
						if (File.Exists(text))
						{
							File.Delete(text);
						}
						FileStream fileStream2 = File.Create(text);
						fileStream2.Close();
						fileInfo = new FileInfo(text);
					}
					message = Process.GetCurrentProcess().ProcessName + ":" + message;
					using (StreamWriter streamWriter = fileInfo.AppendText())
					{
						if (needChangeLine)
						{
							streamWriter.Write("\r\n");
						}
						streamWriter.Write(message);
						if (needWriteTime)
						{
							streamWriter.Write("\r\n");
							streamWriter.Write(DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString());
						}
						if (needChangeLine)
						{
							streamWriter.Write("\r\n");
						}
						streamWriter.Flush();
						streamWriter.Close();
					}
					fileInfo = null;
				}
			}
			catch
			{
			}
		}

		private static string GetExMessage(Exception ex)
		{
			string str = null;
			if (ex == null)
			{
				return null;
			}
			if (ex.InnerException != null)
			{
				str = GetExMessage(ex.InnerException) + "\r\n";
			}
			return str + ex.Message + "\r\n" + ex.StackTrace;
		}

		public static void WriteLog(Exception ex)
		{
			try
			{
				if (ex != null)
				{
					string exMessage = GetExMessage(ex);
					WriteLog(exMessage);
				}
			}
			catch
			{
			}
		}

		public static void WriteLog(string errorHead, Exception ex)
		{
			try
			{
				string str = "";
				if (!string.IsNullOrEmpty(errorHead))
				{
					str = errorHead.Trim() + " --异常发生于:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\r\n";
				}
				WriteLog(str + GetExMessage(ex) + "\r\n");
			}
			catch
			{
			}
		}

		public static string ErrorHandler(string errorHead, Exception ex)
		{
			try
			{
				string str = "";
				if (errorHead != null)
				{
					str = errorHead.Trim() + " --异常发生于:" + DateTime.Now.ToString("yyyy-MM-dd") + "。\r\n";
				}
				WriteLog(str + GetExMessage(ex) + "\r\n");
				return str + "错误原因请查看系统日志文件";
			}
			catch
			{
				return errorHead;
			}
		}
	}
}
