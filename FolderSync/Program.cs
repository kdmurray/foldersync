using System;
using System.Text;

namespace FolderSync
{
    class Program
    {
        static string SourceFolder = "";
        static string DestFolder = "";

        static bool ForceDestDelete = false;
        static bool DisplayLoadedParams = false;

        static void Main(string[] args)
        {
            LoadParams(args);

            CleanFolderParams();
            
            if (!ParamsAreValid())
            {
                DisplayUsageMessage();
            }
            else
            {
                if (DisplayLoadedParams)
                {
                    Console.WriteLine("SourceFolder = " + SourceFolder);
                    Console.WriteLine("DestFolder = " + DestFolder);
                    Console.WriteLine("ForceDestDelete = " + ForceDestDelete.ToString());
                    Console.WriteLine("DisplayLoadedParams = " + DisplayLoadedParams.ToString());
                }
                Synchronizer.SyncFolders(SourceFolder, DestFolder, ForceDestDelete);
            }

        }

        private static void DisplayUsageMessage()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(System.Environment.NewLine);
            sb.Append("USAGE: foldersync -s=<SourceFolder> -d=<DestFolder> [-del|--delete]");
            sb.Append(System.Environment.NewLine);
            sb.Append(System.Environment.NewLine);
            sb.Append("-s    --source      Source folder for the transfer operation");
            sb.Append(System.Environment.NewLine);
            sb.Append(System.Environment.NewLine);
            sb.Append("-d    --destination Destination folder for the transfer operation");
            sb.Append(System.Environment.NewLine);
            sb.Append(System.Environment.NewLine);
            sb.Append("-del  --delete      Flag whether or not to delete files at the destination");
            sb.Append(System.Environment.NewLine);
            sb.Append("                    which do not appear on the source");
            sb.Append(System.Environment.NewLine);
            Console.WriteLine(sb.ToString());
        }

        private static bool ParamsAreValid()
        {
            bool validParams = false;

            validParams = SourceFolder != "" & DestFolder != "";
            validParams = System.IO.Directory.Exists(SourceFolder);

            return validParams;
        }

        static void CleanFolderParams()
        {
            if (SourceFolder.EndsWith(@"\"))
            {
                SourceFolder = SourceFolder.Substring(0, SourceFolder.Length - 1);
            }

            if (DestFolder.EndsWith(@"\"))
            {
                DestFolder = DestFolder.Substring(0, DestFolder.Length - 1);
            }
        }

        static void LoadParams(string[] args)
        {
            foreach (string param in args)
            {
                if (param.ToLower().StartsWith("-s="))
                {
                    SourceFolder = param.Substring(3);
                }
                if (param.ToLower().StartsWith("--source="))
                {
                    SourceFolder = param.Substring(9);
                }

                if (param.ToLower().StartsWith("-d="))
                {
                    DestFolder = param.Substring(3);
                }
                if (param.ToLower().StartsWith("--destination="))
                {
                    DestFolder = param.Substring(14);
                }

                if (param.ToLower() == "-del" || param.ToLower() == "--delete")
                {
                    ForceDestDelete = true;
                }

                if (param.ToLower() == "--displayloadedparams")
                {
                    DisplayLoadedParams = true;
                }
            }
        }
    } 
}