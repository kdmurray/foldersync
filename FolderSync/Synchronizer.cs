using System;
using System.IO;

namespace FolderSync
{
    class Synchronizer
    {
        public static void SyncFolders(string SourceFolder, string DestFolder, bool DeleteFromDest)
        {
            try
            {
                string[] subFolders = Directory.GetDirectories(SourceFolder);
                foreach (string folder in subFolders)
                {
                    DirectoryInfo di = new DirectoryInfo(folder);
                    SyncFolders(SourceFolder + @"\" + di.Name, DestFolder + @"\" + di.Name, DeleteFromDest);
                }

                CopyData(SourceFolder, DestFolder);

                if (DeleteFromDest)
                {
                    DeleteData(SourceFolder, DestFolder);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR :: An error occurred while synchronizing folder: " + SourceFolder);
                Console.WriteLine("     " + ex.Message);
            }
        }

        public static void CopyData(string SourceFolder, string DestFolder)
        {
            if (!Directory.Exists(DestFolder))
            {
                try
                {
                    Directory.CreateDirectory(DestFolder);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ERROR :: Unable to create directory: " + DestFolder);
                    Console.WriteLine("     " + ex.Message);
                }
            }

            string[] SourceFiles = Directory.GetFiles(SourceFolder);

            foreach (string file in SourceFiles)
            {
                FileInfo fiSrc = new FileInfo(file);
                FileInfo fiDst = new FileInfo(DestFolder + @"\" + fiSrc.Name);

                try
                {
                    if (!File.Exists(DestFolder + @"\" + fiSrc.Name))
                    {
                        Console.WriteLine("CREATE :: " + fiDst.FullName);
                        CopyFile(fiSrc.FullName, fiDst.FullName);
                    }
                    else
                    {
                        if (fiSrc.LastWriteTimeUtc > fiDst.LastWriteTimeUtc)
                        {
                            Console.WriteLine("UPDATE :: " + fiDst.FullName);
                            CopyFile(fiSrc.FullName, fiDst.FullName);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ERROR :: Unable to copy file: " + fiDst.FullName);
                    Console.WriteLine("     " + ex.Message);
                }
            }
        }

        public static void DeleteData(string SourceFolder, string DestFolder)
        {
            string[] DestFiles = Directory.GetFiles(DestFolder);

            foreach (string file in DestFiles)
            {
                FileInfo fiDst = new FileInfo(file);

                try
                {
                    if (!File.Exists(SourceFolder + @"\" + fiDst.Name))
                    {
                        Console.WriteLine("DELETE :: " + file);
                        File.Delete(file);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ERROR :: Unable to copy file: " + fiDst.FullName);
                    Console.WriteLine("     " + ex.Message);
                }
            }
        }

        public static void CopyFile(string SourceFile, string DestFile)
        {
            try
            {
                FileStream fs = new FileStream(SourceFile, FileMode.Open, FileAccess.Read);
                byte[] buffer = new byte[32768];
                bool eof = false;

                using (FileStream fs2 = new FileStream(DestFile, FileMode.Create, FileAccess.Write, FileShare.None))
                {

                    while (!eof)
                    {
                        int eofBytes = fs.Read(buffer, 0, buffer.Length);
                        fs2.Write(buffer, 0, eofBytes);
                        if (eofBytes == 0)
                        {
                            eof = true;
                        }
                    }
                }
                fs.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}