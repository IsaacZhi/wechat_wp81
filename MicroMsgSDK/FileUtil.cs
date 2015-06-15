using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
//using System.IO.IsolatedStorage;
namespace MicroMsg.sdk
{
	public class FileUtil
	{
        public static async void deleteDir(/*IsolatedStorageFile*/ StorageFolder isf, string path, bool bDeleteDir = true)
		{
            await isf.DeleteAsync(StorageDeleteOption.PermanentDelete);
            /*
			string[] fileNames = isf.GetFileNames(path + "/*");
			for (int i = 0; i < fileNames.Length; i++)
			{
				string text = fileNames[i];
				isf.DeleteFile(path + "/" + text);
			}
			string[] directoryNames = isf.GetDirectoryNames(path + "/*");
			for (int j = 0; j < directoryNames.Length; j++)
			{
				string text2 = directoryNames[j];
				FileUtil.deleteDir(isf, path + "/" + text2, bDeleteDir);
			}
			if (bDeleteDir)
			{
				isf.DeleteDirectory(path);
			}*/
		}
		public static async Task<bool> writeToFile(string fileName, byte[] data, bool bCreateDir = false)
		{
			if (string.IsNullOrEmpty(fileName) || data == null)
			{
				return false;
			}
			try
			{
                /*
				using (IsolatedStorageFile userStoreForApplication = IsolatedStorageFile.GetUserStoreForApplication())
				{
					if (bCreateDir)
					{
						string directoryName = Path.GetDirectoryName(fileName);
						if (!userStoreForApplication.DirectoryExists(directoryName))
						{
							userStoreForApplication.CreateDirectory(directoryName);
						}
					}
					using (IsolatedStorageFileStream isolatedStorageFileStream = new IsolatedStorageFileStream(fileName, 2, userStoreForApplication))
					{
						isolatedStorageFileStream.Write(data, 0, data.Length);
					}
				}*/
                StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;
                StorageFile file = await local.CreateFileAsync(fileName);
                if ( file != null )
                {
                    await FileIO.WriteBytesAsync(file, data);
                }
				return true;
			}
			catch (Exception)
			{
			}
			return false;
		}
		public static async void appendToFile(string fileName, byte[] data)
		{
			try
			{
                /*
				using (IsolatedStorageFile userStoreForApplication = IsolatedStorageFile.GetUserStoreForApplication())
				{
					using (IsolatedStorageFileStream isolatedStorageFileStream = new IsolatedStorageFileStream(fileName, 4, userStoreForApplication))
					{
						isolatedStorageFileStream.Seek(0L, 2);
						isolatedStorageFileStream.Write(data, 0, data.Length);
					}
				}*/
                /*StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;
                StorageFile file = await local.GetFileAsync(fileName);
                if (file != null)
                {
                    //file.Seek(0, SeekOrigin.End);
                    await FileIO.WriteBytesAsync(file, data);
                }*/
                using (Stream f = await ApplicationData.Current.LocalFolder.OpenStreamForWriteAsync
                            (fileName, CreationCollisionOption.OpenIfExists))
                {
                    f.Seek(0, SeekOrigin.End);
                    await f.WriteAsync(data, 0, data.Length);
                }
			}
			catch (Exception)
			{
			}
		}
		public static byte[] readFromFile(string fileName)
		{
			if (string.IsNullOrEmpty(fileName))
			{
				return null;
			}
			try
			{
                /*
				using (IsolatedStorageFile userStoreForApplication = IsolatedStorageFile.GetUserStoreForApplication())
				{
					if (!userStoreForApplication.FileExists(fileName))
					{
						byte[] result = null;
						return result;
					}
					using (IsolatedStorageFileStream isolatedStorageFileStream = new IsolatedStorageFileStream(fileName, 3, userStoreForApplication))
					{
						byte[] result;
						if (isolatedStorageFileStream.Length <= 0L)
						{
							result = null;
							return result;
						}
						byte[] array = new byte[isolatedStorageFileStream.Length];
						int num = isolatedStorageFileStream.Read(array, 0, array.Length);
						if ((long)num != isolatedStorageFileStream.Length)
						{
							result = null;
							return result;
						}
						result = array;
						return result;
					}
				}*/
			}
			catch (Exception)
			{
			}
			return null;
		}
		public static byte[] readFromFile(string fileName, int offset, int count)
		{
			try
			{
                /*
				using (IsolatedStorageFile userStoreForApplication = IsolatedStorageFile.GetUserStoreForApplication())
				{
					using (IsolatedStorageFileStream isolatedStorageFileStream = new IsolatedStorageFileStream(fileName, 3, userStoreForApplication))
					{
						isolatedStorageFileStream.Seek((long)offset, 0);
						byte[] array = new byte[count];
						int num = isolatedStorageFileStream.Read(array, 0, array.Length);
						byte[] result;
						if (num != count)
						{
							result = null;
							return result;
						}
						result = array;
						return result;
					}
				}*/
			}
			catch (Exception)
			{
			}
			return null;
		}
		public static long getFileExistTime(string path)
		{
			try
			{
                /*
				using (IsolatedStorageFile userStoreForApplication = IsolatedStorageFile.GetUserStoreForApplication())
				{
					long result;
					if (!userStoreForApplication.FileExists(path))
					{
						result = 0L;
						return result;
					}
					result = (long)DateTime.get_Now().Subtract(userStoreForApplication.GetCreationTime(path).get_DateTime()).get_TotalSeconds();
					return result;
				}*/
			}
			catch (Exception)
			{
			}
			return 0L;
		}
		public static long fileLength(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				return 0L;
			}
			try
			{
                /*
				using (IsolatedStorageFile userStoreForApplication = IsolatedStorageFile.GetUserStoreForApplication())
				{
					if (!userStoreForApplication.FileExists(path))
					{
						long result = 0L;
						return result;
					}
					using (IsolatedStorageFileStream isolatedStorageFileStream = new IsolatedStorageFileStream(path, 3, userStoreForApplication))
					{
						long result = isolatedStorageFileStream.Length;
						return result;
					}
				}*/
			}
			catch (Exception)
			{
			}
			return 0L;
		}
		public static async Task<bool> fileExists(string path)
		{
			try
			{
                /*
				using (IsolatedStorageFile userStoreForApplication = IsolatedStorageFile.GetUserStoreForApplication())
				{
					return userStoreForApplication.FileExists(path);
				}*/
                if (string.IsNullOrEmpty(path))
                {
                    return false;
                }

                StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;
                StorageFile findfile = await local.GetFileAsync(path);
                if (findfile != null)
                {
                    return true;
                }
			}
			catch (Exception)
			{
			}
			return false;
		}
		public static async Task<bool> dirExists(string path)
		{
			try
			{
                /*
				using (IsolatedStorageFile userStoreForApplication = IsolatedStorageFile.GetUserStoreForApplication())
				{
					return userStoreForApplication.DirectoryExists(path);
				}*/
                StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;
                StorageFolder findfolder = await local.GetFolderAsync(path);
                bool result = (findfolder != null ) ? true:false;
                return result;
			}
			catch (Exception)
			{
			}
			return false;
		}
		public static async Task<bool> deleteFile(string fileName)
		{
			if (string.IsNullOrEmpty(fileName))
			{
				return false;
			}
			try
			{
                /*
				using (IsolatedStorageFile userStoreForApplication = IsolatedStorageFile.GetUserStoreForApplication())
				{
					if (userStoreForApplication.FileExists(fileName))
					{
						userStoreForApplication.DeleteFile(fileName);
					}
				}*/
                StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;
                StorageFile findfile = await local.GetFileAsync(fileName);
                if ( findfile != null )
                {
                    await findfile.DeleteAsync();
                }
				return true;
			}
			catch (Exception)
			{
			}
			return false;
		}
		public static bool emptyDir(string strPath)
		{
			if (string.IsNullOrEmpty(strPath))
			{
				return false;
			}
			try
			{
                /*
				bool result;
				using (IsolatedStorageFile userStoreForApplication = IsolatedStorageFile.GetUserStoreForApplication())
				{
					if (!userStoreForApplication.DirectoryExists(strPath))
					{
						result = false;
						return result;
					}
					string[] fileNames = userStoreForApplication.GetFileNames(strPath + "/*");
					for (int i = 0; i < fileNames.Length; i++)
					{
						string text = fileNames[i];
						userStoreForApplication.DeleteFile(strPath + "/" + text);
					}
					string[] directoryNames = userStoreForApplication.GetDirectoryNames(strPath + "/*");
					for (int j = 0; j < directoryNames.Length; j++)
					{
						string text2 = directoryNames[j];
						FileUtil.deleteDir(userStoreForApplication, strPath + "/" + text2, true);
					}
				}
				result = true;
				return result;*/
			}
			catch (Exception)
			{
			}
			return false;
		}
		public static async Task<bool> createDir(string strPath)
		{
			if (string.IsNullOrEmpty(strPath))
			{
				return false;
			}
			try
			{
                /*
				bool result;
				using (IsolatedStorageFile userStoreForApplication = IsolatedStorageFile.GetUserStoreForApplication())
				{
					if (userStoreForApplication.DirectoryExists(strPath))
					{
						result = true;
						return result;
					}
					userStoreForApplication.CreateDirectory(strPath);
				}
				result = true;
				return result;*/

                StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;
                StorageFolder findfolder = await local.GetFolderAsync(strPath);
                bool result = false;
                if (findfolder == null)
                {
                    await local.CreateFolderAsync(strPath);
                }
                return result;
			}
			catch (Exception)
			{
			}
			return false;
		}
		public static bool emptyFile(string strPath)
		{
			if (string.IsNullOrEmpty(strPath))
			{
				return false;
			}
			try
			{
                /*
				bool result;
				using (IsolatedStorageFile userStoreForApplication = IsolatedStorageFile.GetUserStoreForApplication())
				{
					if (!userStoreForApplication.DirectoryExists(strPath))
					{
						result = false;
						return result;
					}
					FileUtil.deleteDir(userStoreForApplication, strPath, false);
				}
				result = true;
				return result;*/
			}
			catch (Exception)
			{
			}
			return false;
		}
	}
}
