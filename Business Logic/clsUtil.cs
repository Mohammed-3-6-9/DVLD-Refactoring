using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic
{
    public class clsUtil
    {
        public static string GenerateGUID()
        {
            Guid guid = Guid.NewGuid();
            return guid.ToString();
        }

        public static bool CreateFolderIfNotExist(string FolderPath)
        {
            if(!Directory.Exists(FolderPath))
            {
                try
                {
                    Directory.CreateDirectory(FolderPath);
                    return true;
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }

            return true;
        }

        public static string ReplaceFileNameWithGUID(string sourceFile)
        {
            FileInfo file = new FileInfo(sourceFile);
            string Extension = file.Extension;
            return GenerateGUID() + Extension;
        }

        public static bool CopyImageToProjectImagesFolder(ref string sourceFile)
        {
            string DestinationFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PeopleImages");

            if (!CreateFolderIfNotExist(DestinationFolder))
                return false;


            string DestinationFile = Path.Combine(DestinationFolder, ReplaceFileNameWithGUID(sourceFile));

            try
            {
                System.IO.File.Copy(sourceFile, DestinationFile, true);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

            sourceFile = DestinationFile;
            return true;
        }
    }
}
