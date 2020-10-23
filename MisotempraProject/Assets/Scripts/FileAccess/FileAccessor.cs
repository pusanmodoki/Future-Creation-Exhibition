//制作者: 植村将太
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using FileAccess.Detail;

namespace FileAccess
{
	public static class FileAccessor
	{
		public static readonly string cExtension = "dat";

		public static bool IsExistsFile(string filePath, string fileName)
		{
			string fullFilePath = $"{filePath}/{fileName}.{cExtension}";

			return File.Exists(fullFilePath);
		}

		public static void SaveString(string filePath, string fileName, List<List<string>> data)
		{
			if (fileName == null || fileName.Length == 0)
				throw new System.ArgumentNullException("fileName", "Invalid fileName. (null)");
			if (filePath == null || filePath.Length == 0)
				throw new System.ArgumentNullException("fileName", "Invalid filePath. (null)");
			if (data == null)
				throw new System.ArgumentNullException("data", "Invalid data. (null)");

			string fullFilePath = $"{filePath}/{fileName}.{cExtension}";
			string linkageData = "";
			byte[] saveData = null;

			for (int i = 0, count = data.Count; i < count; ++i)
			{
				for (int k = 0, count1 = data[i].Count; k < count1; ++k)
					linkageData += data[i][k] + ",";
				if (i < count - 1)
					linkageData += "\n";
			}

			saveData = Encoding.UTF8.GetBytes(linkageData);
			saveData = ByteCompressor.Compress(saveData);
			saveData = ByteEncryptor.EncryptAuto(saveData);

			if (!Directory.Exists(filePath))
				Directory.CreateDirectory(filePath);

			using (FileStream fileStream = File.Create(fullFilePath))
				fileStream.Write(saveData, 0, saveData.Length);
		}

		public static void SaveString(string filePath, string fileName, List<SerializePackageString> data)
		{
			if (fileName == null || fileName.Length == 0)
				throw new System.ArgumentNullException("fileName", "Invalid fileName. (null)");
			if (filePath == null || filePath.Length == 0)
				throw new System.ArgumentNullException("fileName", "Invalid filePath. (null)");
			if (data == null)
				throw new System.ArgumentNullException("data", "Invalid data. (null)");

			string fullFilePath = $"{filePath}/{fileName}.{cExtension}";
			string linkageData = "";
			byte[] saveData = null;

			for (int i = 0, count = data.Count; i < count; ++i)
			{
				for (int k = 0, count1 = data[i].list.Count; k < count1; ++k)
					linkageData += data[i][k] + ",";
				if (i < count - 1)
					linkageData += "\n";
			}

			saveData = Encoding.UTF8.GetBytes(linkageData);
			saveData = ByteCompressor.Compress(saveData);
			saveData = ByteEncryptor.EncryptAuto(saveData);

			if (!Directory.Exists(filePath))
				Directory.CreateDirectory(filePath);

			using (FileStream fileStream = File.Create(fullFilePath))
				fileStream.Write(saveData, 0, saveData.Length);
		}
		public static void SaveString(string filePath, string fileName, List<string> data)
		{
			if (fileName == null || fileName.Length == 0)
				throw new System.ArgumentNullException("fileName", "Invalid fileName. (null)");
			if (filePath == null || filePath.Length == 0)
				throw new System.ArgumentNullException("fileName", "Invalid filePath. (null)");
			if (data == null)
				throw new System.ArgumentNullException("data", "Invalid data. (null)");

			string fullFilePath = $"{filePath}/{fileName}.{cExtension}";
			string linkageData = "";
			byte[] saveData = null;

			for (int i = 0, count = data.Count; i < count; ++i)
				linkageData += data[i] + ",";

			saveData = Encoding.UTF8.GetBytes(linkageData);
			saveData = ByteCompressor.Compress(saveData);
			saveData = ByteEncryptor.EncryptAuto(saveData);

			if (!Directory.Exists(filePath))
				Directory.CreateDirectory(filePath);

			using (FileStream fileStream = File.Create(fullFilePath))
				fileStream.Write(saveData, 0, saveData.Length);
		}
		public static void SaveString(string filePath, string fileName, string data)
		{
			if (fileName == null || fileName.Length == 0)
				throw new System.ArgumentNullException("fileName", "Invalid fileName. (null)");
			if (filePath == null || filePath.Length == 0)
				throw new System.ArgumentNullException("fileName", "Invalid filePath. (null)");
			if (data == null)
				throw new System.ArgumentNullException("data", "Invalid data. (null)");

			string fullFilePath = $"{filePath}/{fileName}.{cExtension}";
			byte[] saveData = null;

			saveData = Encoding.UTF8.GetBytes(data);
			saveData = ByteCompressor.Compress(saveData);
			saveData = ByteEncryptor.EncryptAuto(saveData);

			if (!Directory.Exists(filePath))
				Directory.CreateDirectory(filePath);

			using (FileStream fileStream = File.Create(fullFilePath))
				fileStream.Write(saveData, 0, saveData.Length);
		}


		public static void LoadString(string filePath, string fileName, out List<List<string>> data)
		{
			if (fileName == null || fileName.Length == 0)
			{
				data = null;
				throw new System.ArgumentNullException("fileName", "Invalid fileName. (null)");
			}
			if (filePath == null || filePath.Length == 0)
			{
				data = null;
				throw new System.ArgumentNullException("fileName", "Invalid filePath. (null)");
			}

			string fullFilePath = $"{filePath}/{fileName}.{cExtension}";
			string convertData = "";
			byte[] readData = null;

			if (!File.Exists(fullFilePath))
			{
				data = null;
				throw new System.SystemException("File not found. file path: " + fullFilePath);
			}
			using (FileStream fileStream = File.OpenRead(fullFilePath))
			{
				readData = new byte[fileStream.Length];
				fileStream.Read(readData, 0, readData.Length);
			}

			readData = ByteEncryptor.UnencryptAuto(readData);
			readData = ByteCompressor.Uncompress(readData);

			convertData = Encoding.UTF8.GetString(readData);

			SplitData(convertData, out data);
		}
		public static void LoadString(string filePath, string fileName, out List<SerializePackageString> data)
		{
			if (fileName == null || fileName.Length == 0)
			{
				data = null;
				throw new System.ArgumentNullException("fileName", "Invalid fileName. (null)");
			}
			if (filePath == null || filePath.Length == 0)
			{
				data = null;
				throw new System.ArgumentNullException("fileName", "Invalid filePath. (null)");
			}

			string fullFilePath = $"{filePath}/{fileName}.{cExtension}";
			string convertData = "";
			byte[] readData = null;

			if (!File.Exists(fullFilePath))
			{
				data = null;
				throw new System.SystemException("File not found. file path: " + fullFilePath);
			}
			using (FileStream fileStream = File.OpenRead(fullFilePath))
			{
				readData = new byte[fileStream.Length];
				fileStream.Read(readData, 0, readData.Length);
			}

			readData = ByteEncryptor.UnencryptAuto(readData);
			readData = ByteCompressor.Uncompress(readData);

			convertData = Encoding.UTF8.GetString(readData);

			SplitData(convertData, out data);
		}
		public static void LoadString(string filePath, string fileName, out List<string> data)
		{
			if (fileName == null || fileName.Length == 0)
			{
				data = null;
				throw new System.ArgumentNullException("fileName", "Invalid fileName. (null)");
			}
			if (filePath == null || filePath.Length == 0)
			{
				data = null;
				throw new System.ArgumentNullException("fileName", "Invalid filePath. (null)");
			}

			string fullFilePath = $"{filePath}/{fileName}.{cExtension}";
			string convertData = "";
			byte[] readData = null;

			if (!File.Exists(fullFilePath))
			{
				data = null;
				throw new System.SystemException("File not found. file path: " + fullFilePath);
			}
			using (FileStream fileStream = File.OpenRead(fullFilePath))
			{
				readData = new byte[fileStream.Length];
				fileStream.Read(readData, 0, readData.Length);
			}

			readData = ByteEncryptor.UnencryptAuto(readData);
			readData = ByteCompressor.Uncompress(readData);

			convertData = Encoding.UTF8.GetString(readData);

			SplitData(convertData, out data);
		}
		public static void LoadString(string filePath, string fileName, out string data)
		{
			if (fileName == null || fileName.Length == 0)
			{
				data = null;
				throw new System.ArgumentNullException("fileName", "Invalid fileName. (null)");
			}
			if (filePath == null || filePath.Length == 0)
			{
				data = null;
				throw new System.ArgumentNullException("fileName", "Invalid filePath. (null)");
			}

			string fullFilePath = $"{filePath}/{fileName}.{cExtension}";
			byte[] readData = null;

			if (!File.Exists(fullFilePath))
			{
				data = null;
				throw new System.SystemException("File not found. file path: " + fullFilePath);
			}
			using (FileStream fileStream = File.OpenRead(fullFilePath))
			{
				readData = new byte[fileStream.Length];
				fileStream.Read(readData, 0, readData.Length);
			}

			readData = ByteEncryptor.UnencryptAuto(readData);
			readData = ByteCompressor.Uncompress(readData);

			data = Encoding.UTF8.GetString(readData);
		}





		public static void SaveObject<DataType>(string filePath, string fileName, ref DataType data, string beginMark = null)
		{
			if (fileName == null || fileName.Length == 0)
				throw new System.ArgumentNullException("fileName", "Invalid fileName. (null)");
			if (filePath == null || filePath.Length == 0)
				throw new System.ArgumentNullException("fileName", "Invalid filePath. (null)");
			if (data == null)
				throw new System.ArgumentNullException("data", "Invalid data. (null)");

			string fullFilePath = $"{filePath}/{fileName}.{cExtension}";
			string convertData = "";
			byte[] saveData = null;

			if (beginMark != null)
			{
				while (beginMark.Length > 0 && beginMark[beginMark.Length - 1] == '\n')
					beginMark.Remove(beginMark.Length - 1);
				convertData = beginMark + "\n";
			}
			convertData += JsonUtility.ToJson(data);

			saveData = Encoding.UTF8.GetBytes(convertData);
			saveData = ByteCompressor.Compress(saveData);
			saveData = ByteEncryptor.EncryptAuto(saveData);

			if (!Directory.Exists(filePath))
				Directory.CreateDirectory(filePath);

			using (FileStream fileStream = File.Create(fullFilePath))
				fileStream.Write(saveData, 0, saveData.Length);
		}
		public static void SaveObject<DataType>(string filePath, string fileName, ref List<DataType> data, string beginMark = null)
		{
			if (fileName == null || fileName.Length == 0)
				throw new System.ArgumentNullException("fileName", "Invalid fileName. (null)");
			if (filePath == null || filePath.Length == 0)
				throw new System.ArgumentNullException("fileName", "Invalid filePath. (null)");
			if (data == null)
				throw new System.ArgumentNullException("data", "Invalid data. (null)");

			string fullFilePath = $"{filePath}/{fileName}.{cExtension}";
			string convertData = "";
			byte[] saveData = null;

			if (beginMark != null)
			{
				while (beginMark.Length > 0 && beginMark[beginMark.Length - 1] == '\n')
					beginMark.Remove(beginMark.Length - 1);
				convertData = beginMark + "\n";
			}

			foreach (var e in data)
				convertData += JsonUtility.ToJson(e) + "\n";

			saveData = Encoding.UTF8.GetBytes(convertData);
			saveData = ByteCompressor.Compress(saveData);
			saveData = ByteEncryptor.EncryptAuto(saveData);

			if (!Directory.Exists(filePath))
				Directory.CreateDirectory(filePath);

			using (FileStream fileStream = File.Create(fullFilePath))
				fileStream.Write(saveData, 0, saveData.Length);
		}
		public static void LoadObject<DataType>(string filePath, string fileName, out DataType data, string beginMark = null)
		{
			if (fileName == null || fileName.Length == 0)
			{
				data = default;
				throw new System.ArgumentNullException("fileName", "Invalid fileName. (null)");
			}
			if (filePath == null || filePath.Length == 0)
			{
				data = default;
				throw new System.ArgumentNullException("fileName", "Invalid filePath. (null)");
			}

			string fullFilePath = $"{filePath}/{fileName}.{cExtension}";
			string convertData = "";
			byte[] readData = null;
			int readIndex = 0;

			if (!File.Exists(fullFilePath))
			{
				data = default;
				throw new System.SystemException("File not found. file path: " + fullFilePath);
			}
			using (FileStream fileStream = File.OpenRead(fullFilePath))
			{
				readData = new byte[fileStream.Length];
				fileStream.Read(readData, 0, readData.Length);
			}

			readData = ByteEncryptor.UnencryptAuto(readData);
			readData = ByteCompressor.Uncompress(readData);

			convertData = Encoding.UTF8.GetString(readData);
			string[] split = convertData.Split('\n');

			if (beginMark != null)
			{
				while (beginMark.Length > 0 && beginMark[beginMark.Length - 1] == '\n')
					beginMark.Remove(beginMark.Length - 1);
				if (split[0] == beginMark)
					readIndex = 1;
				else
				{
					data = default;
					throw new System.SystemException("BeginMark not found. file path: " + fullFilePath);
				}
			}
			if (split.Length > readIndex && split[readIndex].Length > 0)
				data = JsonUtility.FromJson<DataType>(split[readIndex]);
			else
				data = default;
		}
		public static void LoadObject<DataType>(string filePath, string fileName, out List<DataType> data, string beginMark = null)
		{
			if (fileName == null || fileName.Length == 0)
			{
				data = default;
				throw new System.ArgumentNullException("fileName", "Invalid fileName. (null)");
			}
			if (filePath == null || filePath.Length == 0)
			{
				data = default;
				throw new System.ArgumentNullException("fileName", "Invalid filePath. (null)");
			}

			string fullFilePath = $"{filePath}/{fileName}.{cExtension}";
			string convertData = "";
			byte[] readData = null;
			int readIndex = 0;

			if (!File.Exists(fullFilePath))
			{
				data = default;
				throw new System.SystemException("File not found. file path: " + fullFilePath);
			}
			using (FileStream fileStream = File.OpenRead(fullFilePath))
			{
				readData = new byte[fileStream.Length];
				fileStream.Read(readData, 0, readData.Length);
			}

			readData = ByteEncryptor.UnencryptAuto(readData);
			readData = ByteCompressor.Uncompress(readData);

			convertData = Encoding.UTF8.GetString(readData);

			data = new List<DataType>();

			string[] split = convertData.Split('\n');

			if (beginMark != null)
			{
				while (beginMark.Length > 0 && beginMark[beginMark.Length - 1] == '\n')
					beginMark.Remove(beginMark.Length - 1);
				if (split[0] == beginMark)
					readIndex = 1;
				else
					throw new System.SystemException("BeginMark not found. file path: " + fullFilePath);
			}

			for (; readIndex < split.Length; ++readIndex)
			{
				if (split[readIndex].Length > 0)
					data.Add(JsonUtility.FromJson<DataType>(split[readIndex]));
			}
		}

		public static bool IsExistMark(string fullFilePath, string beginMark)
		{
			if (fullFilePath == null || fullFilePath.Length == 0)
				throw new System.ArgumentNullException("fullFilePath", "Invalid fileName. (null)");
			if (beginMark == null || beginMark.Length == 0)
				throw new System.ArgumentNullException("beginMark", "Invalid beginMark. (null)");

			string[] convertData = null;
			byte[] readData = null;

			using (FileStream fileStream = File.OpenRead(fullFilePath))
			{
				readData = new byte[fileStream.Length];
				fileStream.Read(readData, 0, readData.Length);
			}

			try
			{
				readData = ByteEncryptor.UnencryptAuto(readData);
				readData = ByteCompressor.Uncompress(readData);

				convertData = Encoding.UTF8.GetString(readData).Split('\n');
				if (convertData.Length == 0)
					throw new System.SystemException("Load failed. file path: " + fullFilePath);

				while (beginMark.Length > 0 && beginMark[beginMark.Length - 1] == '\n')
					beginMark.Remove(beginMark.Length - 1);
			}
			catch(System.Exception) { return false; }
			return beginMark == convertData[0];
		}

		static void SplitData(string data, out List<List<string>> read)
		{
			if (data == "")
			{
				read = new List<List<string>>();
				return;
			}

			// StringSplitOption
			System.StringSplitOptions option = System.StringSplitOptions.None;
			// 行に分ける
			string[] lines = data.Split(new char[] { '\r', '\n' }, option);
			// 区分けする文字
			char[] spliter = new char[1] { ',' };


			// 行データを切り分けて,2次元配列へ変換する
			read = new List<List<string>>();
			for (int i = 0, length = lines.Length; i < length; ++i)
			{
				string[] lineToSplit = lines[i].Split(spliter, option);

				read.Add(new List<string>());

				for (int k = 0, length1 = lineToSplit.Length; k < length1; ++k)
					read[i].Add(lineToSplit[k]);

				if (read[i].Count > 0)
					read[i].RemoveAt(read[i].Count - 1);
			}
		}
		static void SplitData(string data, out List<SerializePackageString> read)
		{
			if (data == "")
			{
				read = new List<SerializePackageString>();
				return;
			}

			// StringSplitOption
			System.StringSplitOptions option = System.StringSplitOptions.None;
			// 行に分ける
			string[] lines = data.Split(new char[] { '\r', '\n' }, option);
			// 区分けする文字
			char[] spliter = new char[1] { ',' };


			// 行データを切り分けて,2次元配列へ変換する
			read = new List<SerializePackageString>();
			for (int i = 0, length = lines.Length; i < length; ++i)
			{
				string[] lineToSplit = lines[i].Split(spliter, option);
				read.Add(new SerializePackageString());

				for (int k = 0, length1 = lineToSplit.Length; k < length1; ++k)
					read[i].list.Add(lineToSplit[k]);

				if (read[i].list.Count > 0)
					read[i].list.RemoveAt(read[i].list.Count - 1);
			}
		}
		static void SplitData(string data, out List<string> read)
		{
			if (data == "")
			{
				read = new List<string>();
				return;
			}

			// StringSplitOption
			System.StringSplitOptions option = System.StringSplitOptions.None;
			// 区分けする文字
			char[] spliter = new char[1] { ',' };

			string[] lineToSplit = data.Split(spliter, option);

			// 行データを切り分けて,2次元配列へ変換する
			read = new List<string>();
			for (int k = 0, length = lineToSplit.Length; k < length; ++k)
				read.Add(lineToSplit[k]);

			read.RemoveAt(read.Count - 1);
		}
	}
}