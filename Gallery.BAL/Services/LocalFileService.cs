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
        //delete this method if fileUpload change from local to azure storage
       
        public string UploadFile(Stream stream, string fileName, long userId)
        {
            string path = null;
            if (stream.Length != 0)
            {
                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                       @"images\" + Directory.CreateDirectory(userId.ToString()));
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(Path.Combine(path));
                    path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                     @"images\" + Directory.CreateDirectory(userId.ToString()),
                                     fileName);
                }
                else
                {
                    path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                    @"images\" + Directory.CreateDirectory(userId.ToString()),
                                    fileName);
                }
                    
            }
            using (FileStream fileStream = File.Create(path, (int)stream.Length))
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
           var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                      @"images\" + Directory.CreateDirectory(userId.ToString()));
            string[] files = Directory.GetFiles(path);
            
            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }


            Directory.Delete(path, false);
        }

        public void DeleteFile(string filePath, long userId)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

       
    }
}
