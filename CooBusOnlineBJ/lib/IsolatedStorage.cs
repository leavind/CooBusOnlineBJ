using System.IO;
using System.IO.IsolatedStorage;
using System.Windows.Media.Imaging;

namespace RealTimeBusBJ
{
    public static class IsoStorage
    {
        static string HistoryPath = "ListHistory.dat";
        static string LineNamePath = "ListLineName.dat";
        static string LocationPath = "ListLocation.dat";
        static string bgimgPath = "bgimgFilesNamesPaths.jpg";

        public static BitmapImage ReadbgImg()
        {
            using (IsolatedStorageFile isoF = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (isoF.FileExists(bgimgPath))
                {
                    using (IsolatedStorageFileStream fileStream = isoF.OpenFile(bgimgPath, FileMode.Open, FileAccess.Read))
                    {
                        BitmapImage bi = new BitmapImage();
                        bi.SetSource(fileStream);
                        return bi;
                    }
                }
                else
                    return null;
            }
        }

        public static void SavebgImg(BitmapImage bitmap)
        {
            using (IsolatedStorageFile isoF = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (isoF.FileExists(bgimgPath))
                    isoF.DeleteFile(bgimgPath);
                IsolatedStorageFileStream fileStream = isoF.CreateFile(bgimgPath);
                WriteableBitmap wb = new WriteableBitmap(bitmap);
                wb.SaveJpeg(fileStream, wb.PixelWidth, wb.PixelHeight, 0, 100);
                fileStream.Close();
            }
        }

        public static string ReadLocation()
        {
            return ReadValue(LocationPath);
        }

        public static void WriteLocation(string str)
        {
            string tmp = ReadLocation().Replace(str, "") + str;
            WriteValue(LocationPath, tmp.Replace(";;", ";"));
        }

        public static void RemoveLocation(string str)
        {
            string tmp = ReadLocation().Replace(str, "");
            WriteValue(LocationPath, tmp.Replace(";;", ";"));
        }


        public static string ReadLineName()
        {
            return ReadValue(LineNamePath);
        }

        public static void WriteLineName(string str)
        {
            string tmp = ReadLineName().Replace(str, "") + str;
            WriteValue(LineNamePath, tmp.Replace(";;", ";"));
        }

        public static void RemoveLineName(string str)
        {
            string tmp = ReadLineName().Replace(str, "");
            WriteValue(LineNamePath, tmp.Replace(";;", ";"));
        }

        public static string ReadLineHistory()
        {
            return ReadValue(HistoryPath);
        }
        public static void WirteLineHistory(string str)
        {
            string tmp = ReadLineHistory().Replace(str, "") + str;
            WriteValue(HistoryPath, tmp.Replace(";;", ";"));
        }
        public static void RemoveHistory(string str)
        {
            string tmp = ReadLineHistory().Replace(str, "");
            WriteValue(HistoryPath, tmp.Replace(";;", ";"));
        }

        private static string ReadValue(string path) //读取
        {
            using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (StreamReader sr = new StreamReader(
                    storage.OpenFile(path, FileMode.OpenOrCreate, FileAccess.Read)))
                {
                    return sr.ReadToEnd();
                }
            }
        }

        private static void WriteValue(string path, string str) //保存
        {
            using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (StreamWriter sw = new StreamWriter(
                    storage.OpenFile(path, FileMode.Create, FileAccess.Write)))
                {
                    sw.Write(str);
                }
            }
        }
    }

    public static class IsoStorageSetting
    {
        private static IsolatedStorageSettings iss = IsolatedStorageSettings.ApplicationSettings;
        public static string Read(string key) //读取
        {
            string obj;
            iss.TryGetValue(key, out obj);
            return obj;
        }

        public static void Write(string key, string value) //写入
        {
            Delete(key);
            iss.Add(key, value);
            iss.Save();
        }


        public static void Delete(string key) //删除
        {
            if (iss.Contains(key))
                iss.Remove(key);
            iss.Save();
        }
    }
}
