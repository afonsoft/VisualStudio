using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using CxViewerAction.Entities;
using Common;
using Ionic.Zip;

namespace CxViewerAction.Helpers
{
    /// <summary>
    /// Creates ZIP archives
    /// </summary>
    public class ZipHelper
    {
        /// <summary>
        /// Regular expression to make selection include file range
        /// </summary>
        private const string _fileFilter = "^((?!((exe$)|(dll$)|(pdb$))).)*$";

        /// <summary>
        /// Regular expression to make selection include directory range
        /// </summary>
        private const string _directoryFilter = "^((?!((bin)|(obj)|(.svn))).)*$";

        /// <summary>
        /// Compress string with Zip technology. If in process of zip generation the archive size
        /// exceed maxAllowedZipFileSize value operation canceled.
        /// </summary>
        /// <param name="projects">List of projects to zip</param>
        /// <param name="fileExtToExclude">List of file extensions to exclude from zip</param>
        /// <param name="foldersToExclude">List of folders to exclude from zip</param>
        /// <param name="maxAllowedZipFileSize">Max allowed zip file size</param>
        /// <returns>If maxAllowedZipFileSize value exceed function return null otherwise zip byte stream</returns>
        public static byte[] Compress(Project project, string[] fileExtToExclude, string[] foldersToExclude, long maxAllowedZipFileSize, out string error)
        {
            byte[] data;

            Project[] projectList = project.ProjectPaths.Count == 0 ? new Project[] { project } : project.ProjectPaths.ToArray();


         


            data = Compress(projectList, GetFileExcludeRegex(fileExtToExclude), GetFolderToExcludeRegex(foldersToExclude),
                            project.ProjectPaths.Count > 0, maxAllowedZipFileSize, out error);

            if (data != null && data.Length < maxAllowedZipFileSize)
            {
                return data;
            }
           
                return null;
            
        }


        private static string GetFileExcludeRegex(string[] filesExtToExclude)
        {
            if (filesExtToExclude.Length == 0)
                return null;

            StringBuilder part = new StringBuilder();

            foreach (string ext in filesExtToExclude)
                part.AppendFormat("({0}$)|", ext.Trim());

            part = part.Remove(part.Length - 1, 1);
            part = part.Replace("*", ".*");

            return string.Format("^((?!({0})).)*$", part);
        }

        private static string GetFolderToExcludeRegex(string[] foldersToExclude)
        {
            if (foldersToExclude.Length == 0)
                return null;

            StringBuilder part = new StringBuilder();

            foreach (string ext in foldersToExclude)
                part.AppendFormat("({0})|", ext.Trim());

            part = part.Remove(part.Length - 1, 1);
            part = part.Replace("*", ".*");

            return string.Format("^((?!({0})).)*$", part);
        }

        private static byte[] Compress(Project[] projects, string sExcludeFile, string sExcludePath, bool createRootDir, long maxAllowedZipFileSize, out string error)
        {
            error = string.Empty;
            byte[] compressed = null;

            try
            {
                using (MemoryStream outputStream = new MemoryStream())
                {
                    using (ZipOutputStream oZip = new ZipOutputStream(outputStream))
                    {
                        //Compress Level
                        oZip.CompressionLevel = Ionic.Zlib.CompressionLevel.Level9;

                        int commonPathLength = 0;

                        if (projects.Count() > 1)
                        {
                            commonPathLength = GetCommonPathLength(projects.Select(l => l.RootPath).ToList());
                        }
                        else if (Directory.GetParent(projects[0].RootPath) != null)
                        {

                            commonPathLength = Directory.GetParent(projects[0].RootPath).FullName.Length + 1; // +1  remove '\'
                        }

                        Regex dirMatch = new Regex(sExcludePath, RegexOptions.IgnoreCase);
                      

                        foreach (Project p in projects)
                        {

                            int subProjects = projects.Where(p2 => p.RootPath.Contains(p2.RootPath)).Count();

                            if (subProjects == 1 && dirMatch.IsMatch(Path.GetFileName(p.RootPath))) //If the project is not a subProject and not excluded
                            {
                                foreach (string filePath in p.FilePathList) // scan only the file selected
                                {
                                    if (Directory.Exists(p.RootPath))
                                    {
                                        // find number of chars to remove from orginal file path
                                        Logger.Create().Info("Zip file: " + p.FilePathList);
                                        WriteEntryToZip(oZip, filePath.Remove(0, p.RootPath.Length).TrimStart(new[] { '/', '\\' }), filePath);
                                    }
                                }


                                foreach (string folderPath in p.FolderPathList) // scan only the Folder selected
                                {
                                    Logger.Create().Info("Zip Folder: " + p.FolderPathList);
                                    if (!WriteDirectoryToZip(oZip, folderPath.TrimEnd('\\'), sExcludeFile, sExcludePath, maxAllowedZipFileSize, commonPathLength))
                                    {
                                        error = string.Format("allowable archive size {0}mb exceeded", Convert.ToInt32(maxAllowedZipFileSize / 1024 / 1204));
                                        break;
                                    }
                                }
                                if (!p.FilePathList.Any() && !p.FolderPathList.Any() && Directory.Exists(p.RootPath))
                                {
                                    //in case there are no selected files and folders - scan the whi
                                    Logger.Create().Info("Zip Root Path: " + p.RootPath);
                                    if (!WriteDirectoryToZip(oZip, p.RootPath.TrimEnd('\\'), sExcludeFile, sExcludePath, maxAllowedZipFileSize, commonPathLength))
                                    {
                                        error = string.Format("allowable archive size {0}mb exceeded", Convert.ToInt32(maxAllowedZipFileSize / 1024 / 1204));
                                        break;
                                    }

                                }
                            }
                        }

                        oZip.Flush();
                        oZip.Close();
                        compressed = outputStream.ToArray();
                    }
                }
            }
            catch (Exception err)
            {
                Common.Logger.Create().Error(err.ToString());
                error = err.Message;
            }

            return compressed;
        }

        private static int GetCommonPathLength(List<string> Files)
        {
            string LongestDir = string.Empty;
            try
            {
                var MatchingChars =
                from len in Enumerable.Range(0, Files.Min(s => s.Length)).Reverse()
                let possibleMatch = Files.First().Substring(0, len)
                where Files.All(f => f.StartsWith(possibleMatch))
                select possibleMatch;

                LongestDir = Path.GetDirectoryName(MatchingChars.First());
            }
            catch (Exception err)
            {
                Common.Logger.Create().Error("Failed to get project common path. error: " + err.Message);
                LongestDir = string.Empty;
            }
            return LongestDir == string.Empty ? 0 : LongestDir.Length + 1;// +1  remove '\'
        }


        /// <summary>
        /// Zip a folder of files  in a new zip file
        /// </summary>
        public static bool WriteDirectoryToZip(ZipOutputStream zipStream, string inputFolderPath, string sExcludeFile, string sExcludePath, long maxAllowedZipFileSize, int trimLength)

        {
            Regex fileMatch = new Regex(sExcludeFile, RegexOptions.IgnoreCase);
            Regex dirMatch = new Regex(sExcludePath, RegexOptions.IgnoreCase);

            List<string> filesToZip = GenerateFileList(inputFolderPath, fileMatch, dirMatch);

            int entryCounter = 0;

            foreach (string file in filesToZip)
            {
                WriteEntryToZip(zipStream, file.Remove(0, trimLength), file);

                entryCounter++;

                // Flush every 20 entries
                if (entryCounter % 20 == 0)
                {
                    zipStream.Flush();
                }

                if (zipStream.Position >= maxAllowedZipFileSize)
                {
                    return false;
                }
            }

            zipStream.Flush();

            return true;
        }

        /// <summary>
        /// Writes a single entry (file or folder) to an open zip file
        /// </summary>
        /// <param name="zipStream">The open zip file stream</param>
        /// <param name="entryName">The entry name to be created</param>
        /// <param name="file">The file path of the file to be written</param>
        /// <param name="openShare">Open mode (share/non share)</param>
        private static void WriteEntryToZip(ZipOutputStream zipStream, string entryName, string file)
        {
            ZipEntry zipEntry = zipStream.PutNextEntry(entryName);

            if (!file.EndsWith(@"/")) // if a file ends with '/' its a directory
            {
                using (FileStream ostream = File.OpenRead(file))
                {
                    if (ostream.Length > 0)
                    {
                        // Init the buffer
                        byte[] obuffer = new byte[ostream.Length];

                        // Read all file to buffer and write to zip stream
                        ostream.Read(obuffer, 0, obuffer.Length);
                        zipStream.Write(obuffer, 0, obuffer.Length);
                    }
                }
            }
        }


        private static List<string> GenerateFileList(string Dir, Regex fileMatch, Regex dirMatch)
        {
            List<string> items = new List<string>();
            bool empty = true;
            foreach (string file in Directory.GetFiles(Dir))
            {
                if (fileMatch.IsMatch(file))
                {
                    items.Add(file);
                }

                empty = false;
            }

            if (empty)
            {
                if (Directory.GetDirectories(Dir).Length == 0)
                {
                    items.Add(Dir + @"/");
                }
            }

            foreach (string dirs in Directory.GetDirectories(Dir))
            {
                if (dirMatch.IsMatch(Path.GetFileName(dirs)))
                {
                    foreach (string item in GenerateFileList(dirs, fileMatch, dirMatch))
                    {
                        items.Add(item);
                    }
                }
            }
            return items;
        }
    }
}

