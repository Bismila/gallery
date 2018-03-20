using Gallery.BAL.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Gallery.BAL.Services
{
    public class LocalFileService : IFileService
    {
        public string UploadFile(Stream stream, string fileName, long userId)
        {
            string path = null;
            if (stream.Length != 0)
            {
                //string currentPath = Directory.GetCurrentDirectory();
                //if (!Directory.Exists(Path.Combine(currentPath, userId.ToString())))
                //    Directory.CreateDirectory(Path.Combine(currentPath, userId.ToString()));
                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, 
                                        @"images\"+ Directory.CreateDirectory(userId.ToString()),
                                        fileName);
                //path = Path.Combine("images", fileName);
            }
            using (FileStream fileStream = File.Create(path, (int)stream.Length))// new FileStream(path, FileMode.Append, FileAccess.Write))// File.Create(fileName, (int)stream.Length)))
            {
                byte[] data = new byte[stream.Length];
                MemoryStream ms = new MemoryStream(data);
                stream.Read(data, 0, (int)data.Length);
                fileStream.Write(data, 0, data.Length);
                ms.CopyTo(fileStream);
            }


            return path;
        }

        public void DeleteAllFile(long userId)
        {
            throw new NotImplementedException();
        }

        public void DeleteFile(string filePath, long userId)
        {
            throw new NotImplementedException();
        }

       
    }
}
