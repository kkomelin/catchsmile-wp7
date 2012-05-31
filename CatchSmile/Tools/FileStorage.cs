using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO.IsolatedStorage;
using System.IO;
using System.Windows.Media.Imaging;
using Microsoft.Phone;

namespace CatchSmile
{
    /// <summary>
    /// Operates with Isolated Storage.
    /// </summary>
    public class FileStorage
    {
        /// <summary>
        /// Saves stream to IS storage file.
        /// Overrides existing file.
        /// </summary>
        /// <param name="imageStream"></param>
        /// <param name="fileName"></param>
        /// <param name="reducingRate"></param>
        /// <param name="quality"></param>
        public static void SaveToIsolatedStorage(Stream imageStream, string fileName, int reducingRate, int quality)
        {
            using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                // Delete file if it already exists.
                if (myIsolatedStorage.FileExists(fileName))
                {
                    myIsolatedStorage.DeleteFile(fileName);
                }

                using (IsolatedStorageFileStream fileStream = myIsolatedStorage.CreateFile(fileName))
                {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.SetSource(imageStream);

                    WriteableBitmap wb = new WriteableBitmap(bitmap);

                    int resultWidth = (int)(wb.PixelWidth / reducingRate);
                    int resultHeight = (int)(wb.PixelHeight / reducingRate);

                    wb.SaveJpeg(fileStream, resultWidth, resultHeight, 0, quality);
                }
            }
        }

        /// <summary>
        /// Returns IS file size.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static long FileSize(string fileName)
        {
            using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (IsolatedStorageFileStream fileStream = myIsolatedStorage.OpenFile(fileName, FileMode.Open, FileAccess.Read))
                {
                    return fileStream.Length;
                }
            }
        }

        /// <summary>
        /// Reads file from IS to byte array.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static byte[] ReadBytesFromIsolatedStorage(string fileName)
        {
            using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (IsolatedStorageFileStream fileStream = myIsolatedStorage.OpenFile(fileName, FileMode.Open, FileAccess.Read))
                {
                    byte[] contents;
                    using (BinaryReader bReader = new BinaryReader(fileStream))
                    {
                        contents = bReader.ReadBytes((int)fileStream.Length);
                    }
                    return contents;
                }
            }
        }

        /// <summary>
        /// Reads file from IS to BitmapImage.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static BitmapImage ReadBitmapFromIsolatedStorage(string fileName)
        {
            BitmapImage bmp = new BitmapImage();
            byte[] data;

            using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (IsolatedStorageFileStream fileStream = myIsolatedStorage.OpenFile(fileName, FileMode.Open, FileAccess.Read))
                {
                    data = new byte[fileStream.Length];

                    fileStream.Read(data, 0, data.Length);

                    fileStream.Close();
                }

            }

            MemoryStream ms = new MemoryStream(data);

            bmp.SetSource(ms);

            return bmp;
        }

    }
}
