using System;
using System.Collections.Generic;
using System.Linq;

namespace NavigatorTemplate.Models
{
    public class CustomSearcher : Models.mvvmHandlers
    {
        private List<string> _lstFolders = new List<string>();
        public List<string> LstFolders
        {
            get { return _lstFolders; }
            set
            {
                if (_lstFolders != value)
                {
                    _lstFolders = value;
                    NotifyPropertyChanged("LstFolderCount");
                }
            }
        }


        private int _currentFileCount = 0;
        public int CurrentFileCount
        {
            get { return _currentFileCount; }
            set
            {
                if (_currentFileCount != value)
                {
                    _currentFileCount = value;
                    NotifyPropertyChanged("CurrentFileCount");
                }
            }
        }

        private int _currentFolderCount = 0;
        public int CurrentFolderCount
        {
            get { return _currentFolderCount; }
            set
            {
                if (_currentFolderCount != value)
                {
                    _currentFolderCount = value;
                    NotifyPropertyChanged("CurrentFolderCount");
                }
            }
        }

        public static List<string> GetDirectories(string path, string searchPattern = "*", System.IO.SearchOption searchOption = System.IO.SearchOption.AllDirectories)
        {
            List<string> directories = new List<string>();
            directories.Add(path);

            directories = GetDirectories(path, searchPattern);

            for (int i = 0; i < directories.Count; i++)
            {
                directories.AddRange(GetDirectories(directories[i], searchPattern));

            }

            return directories;
        }

        //public void GetDirectories1(string path, string searchPattern = "*", System.IO.SearchOption searchOption = System.IO.SearchOption.AllDirectories)
        //{

        //    LstFolders = GetDirectories(path, searchPattern);

        //    for (int i = 0; i < LstFolders.Count; i++)
        //    {
        //        LstFolders.AddRange(GetDirectories(LstFolders[i], searchPattern));
        //        CurrentFolderCount++;
        //    }

        //    //insert root folder
        //    LstFolders.Insert(0, path);
        //    CurrentFolderCount++;
        //}

        public void GetDirectories1(string path, string searchPattern = "*", System.IO.SearchOption searchOption = System.IO.SearchOption.AllDirectories)
        {

            LstFolders = GetDirectories(path, searchPattern);

            for (int i = 0; i < LstFolders.Count; i++)
            {
                LstFolders.AddRange(GetDirectories(LstFolders[i], searchPattern));
                CurrentFolderCount++;
            }

            //insert root folder
            LstFolders.Insert(0, path);
            CurrentFolderCount++;
        }

        public void GetFiles1(string searchPattern = "*.*", System.IO.SearchOption searchOption = System.IO.SearchOption.AllDirectories)
        {
            List<string> docLst = new List<string>();

            for (var i = 0; i < LstFolders.Count; i++)
            {
                List<string> tmpLst = GetFiles(LstFolders[i], searchPattern);
                CurrentFileCount += tmpLst.Count();
                docLst.AddRange(tmpLst);
            }
        }

        public void GetDirectories1(List<string> paths, string searchPattern = "*", System.IO.SearchOption searchOption = System.IO.SearchOption.AllDirectories)
        {
            List<string> folderLst = new List<string>();
            LstFolders.Clear();
            foreach (string path in paths)
            {
                //LstFolders = GetDirectories(path, searchPattern);
                folderLst = GetDirectories(path, searchPattern);
                for (int i = 0; i < folderLst.Count; i++)
                {
                    folderLst.AddRange(GetDirectories(folderLst[i], searchPattern));
                    CurrentFolderCount++;
                }

                //insert root folder
                folderLst.Insert(0, path);
                CurrentFolderCount++;

                LstFolders.AddRange(folderLst);
            }

        }



        private static List<string> GetDirectories(string path, string searchPattern)
        {
            try
            {
                //return System.IO.Directory.GetDirectories(path, searchPattern).ToList();
                return System.IO.Directory.EnumerateDirectories(path, searchPattern).ToList();
            }
            catch (UnauthorizedAccessException uaEx)
            {
                using (System.IO.StreamWriter logfile = new System.IO.StreamWriter(PathLenghtCrawl.Properties.Settings.Default.LogLocalDestinationFolderTxt + System.IO.Path.DirectorySeparatorChar + "tvFolderLogUA.log", true))
                {
                    logfile.WriteLine(DateTime.Now.ToString() + ": Type: Folder. |" + path + " -> Exception:" + uaEx.Message.Trim());
                }
                return new List<string>();
            }
            catch (System.IO.IOException ioEx)
            {
                using (System.IO.StreamWriter logfile = new System.IO.StreamWriter(PathLenghtCrawl.Properties.Settings.Default.LogLocalDestinationFolderTxt + System.IO.Path.DirectorySeparatorChar + "tvFolderLogIO.log", true))
                {
                    logfile.WriteLine(DateTime.Now.ToString() + ": Type: Folder. |" + path + " -> Exception:" + ioEx.Message.Trim());
                }
                return new List<string>();
            }
            catch (System.Exception unknownEx)
            {
                using (System.IO.StreamWriter logfile = new System.IO.StreamWriter(PathLenghtCrawl.Properties.Settings.Default.LogLocalDestinationFolderTxt + System.IO.Path.DirectorySeparatorChar + "tvFolderLogUE.log", true))
                {
                    logfile.WriteLine(DateTime.Now.ToString() + ": Type: Folder. |" + path + " -> Exception:" + unknownEx.Message.Trim());
                }
                return new List<string>();
            }
        }



        public static List<string> GetFiles(List<string> folders, string searchPattern = "*.*", System.IO.SearchOption searchOption = System.IO.SearchOption.AllDirectories)
        {
            List<string> docLst = new List<string>();

            for (var i = 0; i < folders.Count; i++)
            {
                docLst.AddRange(GetFiles(folders[i], searchPattern));
            }
            return docLst;
        }

        private static List<string> GetFiles(string path, string searchPattern)
        {
            try
            {
                //List<string> fileLst = System.IO.Directory.GetFiles(path, searchPattern).ToList();
                List<string> fileLst = System.IO.Directory.EnumerateFiles(path, searchPattern).ToList();

                return fileLst;
            }
            catch (UnauthorizedAccessException uaEx)
            {
                using (System.IO.StreamWriter logfile = new System.IO.StreamWriter(PathLenghtCrawl.Properties.Settings.Default.LogLocalDestinationFolderTxt + System.IO.Path.DirectorySeparatorChar + "tvFileLogUA.log", true))
                {
                    logfile.WriteLine(DateTime.Now.ToString() + ": Type: Document. |" + path + " -> Exception:" + uaEx.Message.Trim());
                }
                return new List<string>();
            }
            catch (System.IO.IOException ioEx)
            {
                using (System.IO.StreamWriter logfile = new System.IO.StreamWriter(PathLenghtCrawl.Properties.Settings.Default.LogLocalDestinationFolderTxt + System.IO.Path.DirectorySeparatorChar + "tvFileLogOI.log", true))
                {
                    logfile.WriteLine(DateTime.Now.ToString() + ": Type: Document. |" + path + " -> Exception:" + ioEx.Message.Trim());
                }
                return new List<string>();
            }
            catch (System.Exception unknownEx)
            {
                using (System.IO.StreamWriter logfile = new System.IO.StreamWriter(PathLenghtCrawl.Properties.Settings.Default.LogLocalDestinationFolderTxt + System.IO.Path.DirectorySeparatorChar + "tvFileLogUE.log", true))
                {
                    logfile.WriteLine(DateTime.Now.ToString() + ": Type: Document. |" + path + " -> Exception:" + unknownEx.Message.Trim());
                }
                return new List<string>();
            }
        }
    }
}
