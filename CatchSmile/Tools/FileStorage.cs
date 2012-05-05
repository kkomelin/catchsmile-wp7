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
    public class FileStorage
    {
        public static WriteableBitmap SaveToIsolatedStorage(Stream imageStream, string fileName)
        {
            using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (myIsolatedStorage.FileExists(fileName))
                {
                    myIsolatedStorage.DeleteFile(fileName);
                }

                using (IsolatedStorageFileStream fileStream = myIsolatedStorage.CreateFile(fileName))
                {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.SetSource(imageStream);

                    WriteableBitmap wb = new WriteableBitmap(bitmap);
                    wb.SaveJpeg(fileStream, wb.PixelWidth, wb.PixelHeight, 0, 85);

                    return wb;
                }
            }
        }

        /*public void SaveToFile(string fileName, string content)
        {
            try
            {
                ///<summary>
                /// Get the user Store and then create the file in the store.
                /// Finally write the content to the file.
                ///</summary>
                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                using (var writeStream = new IsolatedStorageFileStream(fileName, FileMode.Create, store))
                using (var writer = new StreamWriter(writeStream))
                {
                    writer.Write(content);
                }
            }
            catch (IsolatedStorageException e)
            {
                MessageBox.Show(e.Message);
            }
        }*/

        /*public string LoadFromFile(string fileName)
        {
            try
            {
                ///<summary>
                /// Get the user Store and then open the file in the store.
                /// Finally read the content of the file and return it.
                ///</summary>
                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                using (var readStream = new IsolatedStorageFileStream(fileName, FileMode.Open, store))
                using (var reader = new StreamReader(readStream))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (IsolatedStorageException e)
            {
                MessageBox.Show(e.Message);
                return String.Empty;
            }
        }*/

        /*public static long FileLength(string fileName)
        {
            using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (IsolatedStorageFileStream fileStream = myIsolatedStorage.OpenFile(fileName, FileMode.Open, FileAccess.Read))
                {
                    return fileStream.Length;
                }
            }
        }*/

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

        public static BitmapImage ReadBitmapFromIsolatedStorage(string fileName)
        {
            BitmapImage bmp = new BitmapImage();
            byte[] data;

            using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (IsolatedStorageFileStream fileStream = myIsolatedStorage.OpenFile(fileName, FileMode.Open, FileAccess.Read))
                {
                    // Decode the JPEG stream.
                    /*WriteableBitmap wb = PictureDecoder.DecodeJpeg(fileStream);

                     MemoryStream ms = new MemoryStream();

                     wb.SaveJpeg(ms, wb.PixelWidth, wb.PixelHeight, 0, 100);
  
                     bmp.SetSource(ms);*/

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
