using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace NavigatorTemplate.Models
{
    public class Master : mvvmHandlers
    {
        public CustomSearcher CustomSearcher { get; set; }
        public CustomSearcher customSearcher = new CustomSearcher();

        System.Windows.Threading.DispatcherTimer dtRunning = new System.Windows.Threading.DispatcherTimer();
        //System.Windows.Threading.DispatcherTimer dtRunningAverage = new System.Windows.Threading.DispatcherTimer();
        System.Windows.Threading.DispatcherTimer dtRunningPer = new System.Windows.Threading.DispatcherTimer();
        System.Diagnostics.Stopwatch stopWatchRunning = new System.Diagnostics.Stopwatch();
        System.Diagnostics.Stopwatch stopWatchRunningPer = new System.Diagnostics.Stopwatch();
        string currentRunningTime = string.Empty;
        string currentRunningTimePer = string.Empty;


        public Master()
        {
            SetFileCommand = new RelayCommand(SetFile, param => this._canExecute);
            SetFolderCommand = new RelayCommand(SetFolder, param => this._canExecute);
            SetFolderViaTVCommand = new RelayCommand(SetFolderViaTV, param => this._canExecute);

            CustomSearcher = customSearcher;

            dtRunning.Tick += new EventHandler(dtRunning_Tick);
            dtRunning.Interval = new TimeSpan(0, 0, 0, 0, 1);
            dtRunning.Start();
        }

        #region "************************************************************************************************* Global Properties"

        private string expandOptionsStats = "Visible";
        public string ExpandOptionsStats
        {
            get
            {
                return expandOptionsStats;
            }
            set
            {
                if (expandOptionsStats != value)
                {
                    expandOptionsStats = value;
                    NotifyPropertyChanged("ExpandOptionsStats");
                }
            }
        }

        private bool expandOptionsStatsBool = true;
        public bool ExpandOptionsStatsBool
        {
            get
            {
                return expandOptionsStatsBool;
            }
            set
            {
                if (expandOptionsStatsBool != value)
                {
                    expandOptionsStatsBool = value;
                    if (value) { ExpandOptionsStats = "Visible"; } else { ExpandOptionsStats = "Collapsed"; }
                    NotifyPropertyChanged("ExpandOptionsStatsBool");
                }
            }
        }

        private bool selectBulkFile2ProcessButtonEnabled = true;
        public bool SelectBulkFile2ProcessButtonEnabled
        {
            get
            {
                return selectBulkFile2ProcessButtonEnabled;
            }
            set
            {
                if (selectBulkFile2ProcessButtonEnabled != value)
                {
                    selectBulkFile2ProcessButtonEnabled = value;
                    NotifyPropertyChanged("SelectBulkFile2ProcessButtonEnabled");
                }
            }
        }

        private bool importProcessBulkFileButtonEnabled = true;
        public bool ImportProcessBulkFileButtonEnabled
        {
            get
            {
                return importProcessBulkFileButtonEnabled;
            }
            set
            {
                if (importProcessBulkFileButtonEnabled != value)
                {
                    importProcessBulkFileButtonEnabled = value;
                    NotifyPropertyChanged("ImportProcessBulkFileButtonEnabled");
                }
            }
        }

        private bool useAuditDatabase = PathLenghtCrawl.Properties.Settings.Default.UseAuditDatabase;
        public bool UseAuditDatabase
        {
            get
            {
                return useAuditDatabase;
            }
            set
            {
                if (useAuditDatabase != value)
                {
                    useAuditDatabase = value;
                    PathLenghtCrawl.Properties.Settings.Default.UseAuditDatabase = value;
                    SaveProperties();
                    NotifyPropertyChanged("UseAuditDatabase");
                }
            }
        }

        private bool includeFolders = PathLenghtCrawl.Properties.Settings.Default.IncludeFolders;
        public bool IncludeFolders
        {
            get
            {
                return includeFolders;
            }
            set
            {
                if (includeFolders != value)
                {
                    includeFolders = value;
                    PathLenghtCrawl.Properties.Settings.Default.IncludeFolders = value;
                    SaveProperties();
                    NotifyPropertyChanged("IncludeFolders");
                }
            }
        }

        private bool includeSubFolders = PathLenghtCrawl.Properties.Settings.Default.IncludeSubFolders;
        public bool IncludeSubFolders
        {
            get
            {
                return includeSubFolders;
            }
            set
            {
                if (includeSubFolders != value)
                {
                    includeSubFolders = value;
                    PathLenghtCrawl.Properties.Settings.Default.IncludeSubFolders = value;
                    SaveProperties();
                    NotifyPropertyChanged("IncludeSubFolders");
                }
            }
        }

        private bool includeDocuments = PathLenghtCrawl.Properties.Settings.Default.IncludeDocuments;
        public bool IncludeDocuments
        {
            get
            {
                return includeDocuments;
            }
            set
            {
                if (includeDocuments != value)
                {
                    includeDocuments = value;
                    PathLenghtCrawl.Properties.Settings.Default.IncludeDocuments = value;
                    SaveProperties();
                    NotifyPropertyChanged("IncludeDocuments");
                }
            }
        }

        private int minPathLength = PathLenghtCrawl.Properties.Settings.Default.MinPathLength;
        public int MinPathLength
        {
            get
            {
                return minPathLength;
            }
            set
            {
                if (minPathLength != value)
                {
                    minPathLength = value;
                    PathLenghtCrawl.Properties.Settings.Default.MinPathLength = value;
                    SaveProperties();
                    NotifyPropertyChanged("MinPathLength");
                }
            }
        }

        private int maxPathLength = PathLenghtCrawl.Properties.Settings.Default.MaxPathLength;
        public int MaxPathLength
        {
            get
            {
                return maxPathLength;
            }
            set
            {
                if (maxPathLength != value)
                {
                    maxPathLength = value;
                    PathLenghtCrawl.Properties.Settings.Default.MaxPathLength = value;
                    SaveProperties();
                    NotifyPropertyChanged("MaxPathLength");
                }
            }
        }

        private bool copyAsCSVEnabled = true;
        public bool CopyAsCSVEnabled
        {
            get
            {
                return copyAsCSVEnabled;
            }
            set
            {
                if (copyAsCSVEnabled != value)
                {
                    copyAsCSVEnabled = value;
                    NotifyPropertyChanged("CopyAsCSVEnabled");
                }
            }
        }

        private bool saveAsCSVEnabled = true;
        public bool SaveAsCSVEnabled
        {
            get
            {
                return saveAsCSVEnabled;
            }
            set
            {
                if (saveAsCSVEnabled != value)
                {
                    saveAsCSVEnabled = value;
                    NotifyPropertyChanged("SaveAsCSVEnabled");
                }
            }
        }

        private bool includeEmptyFolders = PathLenghtCrawl.Properties.Settings.Default.IncludeEmptyFolders;
        public bool IncludeEmptyFolders
        {
            get
            {
                return includeEmptyFolders;
            }
            set
            {
                if (includeEmptyFolders != value)
                {
                    includeEmptyFolders = value;
                    PathLenghtCrawl.Properties.Settings.Default.IncludeEmptyFolders = value;
                    SaveProperties();
                    NotifyPropertyChanged("IncludeEmptyFolders");
                }
            }
        }

        private string pathCurrentlyProcessing = "N/A";
        public string PathCurrentlyProcessing
        {
            get
            {
                return pathCurrentlyProcessing;
            }
            set
            {
                if (pathCurrentlyProcessing != value)
                {
                    pathCurrentlyProcessing = value;
                    NotifyPropertyChanged("PathCurrentlyProcessing");
                }
            }
        }


        private string pathCurrentlyCounting = "N/A";
        public string PathCurrentlyCounting
        {
            get
            {
                return pathCurrentlyCounting;
            }
            set
            {
                if (pathCurrentlyCounting != value)
                {
                    pathCurrentlyCounting = value;
                    NotifyPropertyChanged("PathCurrentlyCounting");
                }
            }
        }


        private string magikFileSource = PathLenghtCrawl.Properties.Settings.Default.MagikFileSource;
        public string MagikFileSource
        {
            get
            {
                return magikFileSource;
            }
            set
            {
                if (magikFileSource != value)
                {
                    magikFileSource = value;
                    PathLenghtCrawl.Properties.Settings.Default.MagikFileSource = value;
                    SaveProperties();
                    NotifyPropertyChanged("MagikFileSource");
                }
            }
        }

        private string magikFolderDestination = PathLenghtCrawl.Properties.Settings.Default.MagikFolderDestination;
        public string MagikFolderDestination
        {
            get
            {
                return magikFolderDestination;
            }
            set
            {
                if (magikFolderDestination != value)
                {
                    magikFolderDestination = value;
                    PathLenghtCrawl.Properties.Settings.Default.MagikFolderDestination = value;
                    SaveProperties();
                    NotifyPropertyChanged("MagikFolderDestination");
                }
            }
        }



        private int pathProcessedCountTotal = 0;
        public int PathProcessedCountTotal
        {
            get
            {
                return pathProcessedCountTotal;
            }
            set
            {
                if (pathProcessedCountTotal != value)
                {
                    pathProcessedCountTotal = value;
                    NotifyPropertyChanged("PathProcessedCountTotal");
                }
            }
        }

        private int pathImportedCountTotal = 0;
        public int PathImportedCountTotal
        {
            get
            {
                return pathImportedCountTotal;
            }
            set
            {
                if (pathImportedCountTotal != value)
                {
                    pathImportedCountTotal = value;
                    NotifyPropertyChanged("PathImportedCountTotal");
                }
            }
        }

        private int pathProcessedCount = 0;
        public int PathProcessedCount
        {
            get
            {
                return pathProcessedCount;
            }
            set
            {
                if (pathProcessedCount != value)
                {
                    pathProcessedCount = value;
                    NotifyPropertyChanged("PathProcessedCount");
                }
            }
        }

        private int pathImportedCount = 0;
        public int PathImportedCount
        {
            get
            {
                return pathImportedCount;
            }
            set
            {
                if (pathImportedCount != value)
                {
                    pathImportedCount = value;
                    NotifyPropertyChanged("PathImportedCount");
                }
            }
        }

        private int warningCount = 0;
        public int WarningCount
        {
            get
            {
                return warningCount;
            }
            set
            {
                if (warningCount != value)
                {
                    warningCount = value;
                    NotifyPropertyChanged("WarningCount");
                }
            }
        }

        private int errorCount = 0;
        public int ErrorCount
        {
            get
            {
                return errorCount;
            }
            set
            {
                if (errorCount != value)
                {
                    errorCount = value;
                    NotifyPropertyChanged("ErrorCount");
                }
            }
        }



        private int objectCount = 0;
        public int ObjectCount
        {
            get
            {
                return objectCount;
            }
            set
            {
                if (objectCount != value)
                {
                    objectCount = value;
                    NotifyPropertyChanged("ObjectCount");
                }
            }
        }

        private int fileCount = 0;
        public int FileCount
        {
            get
            {
                return fileCount;
            }
            set
            {
                if (fileCount != value)
                {
                    fileCount = value;
                    NotifyPropertyChanged("FileCount");
                }
            }
        }

        private int folderCount = 0;
        public int FolderCount
        {
            get
            {
                return folderCount;
            }
            set
            {
                if (folderCount != value)
                {
                    folderCount = value;
                    NotifyPropertyChanged("FolderCount");
                }
            }
        }

        private int objectCountTotal = 0;
        public int ObjectCountTotal
        {
            get
            {
                return objectCountTotal;
            }
            set
            {
                if (objectCountTotal != value)
                {
                    objectCountTotal = value;
                    NotifyPropertyChanged("ObjectCountTotal");
                }
            }
        }

        private int fileCountTotal = 0;
        public int FileCountTotal
        {
            get
            {
                return fileCountTotal;
            }
            set
            {
                if (fileCountTotal != value)
                {
                    fileCountTotal = value;
                    NotifyPropertyChanged("FileCountTotal");
                }
            }
        }

        private int folderCountTotal = 0;
        public int FolderCountTotal
        {
            get
            {
                return folderCountTotal;
            }
            set
            {
                if (folderCountTotal != value)
                {
                    folderCountTotal = value;
                    NotifyPropertyChanged("FolderCountTotal");
                }
            }
        }
        private string pathFileImportTxt = PathLenghtCrawl.Properties.Settings.Default.PathFileImportTxt;
        public string PathFileImportTxt
        {
            get
            {
                return pathFileImportTxt;
            }
            set
            {
                if (pathFileImportTxt != value)
                {
                    pathFileImportTxt = value;
                    PathLenghtCrawl.Properties.Settings.Default.PathFileImportTxt = value;
                    SaveProperties();
                    NotifyPropertyChanged("PathFileImportTxt");
                }
            }
        }

        private string databaseValLocationPath = PathLenghtCrawl.Properties.Settings.Default.DatabaseValLocationPath;
        public string DatabaseValLocationPath
        {
            get
            {
                return databaseValLocationPath;
            }
            set
            {
                if (databaseValLocationPath != value)
                {
                    databaseValLocationPath = value;
                    PathLenghtCrawl.Properties.Settings.Default.DatabaseValLocationPath = value;
                    SaveProperties();
                    NotifyPropertyChanged("DatabaseValLocationPath");
                }
            }
        }

        private string databaseValName = PathLenghtCrawl.Properties.Settings.Default.DatabaseValName;
        public string DatabaseValName
        {
            get
            {
                return databaseValName;
            }
            set
            {
                if (databaseValName != value)
                {
                    databaseValName = value;
                    PathLenghtCrawl.Properties.Settings.Default.DatabaseValName = value;
                    SaveProperties();
                    NotifyPropertyChanged("DatabaseValName");
                }
            }
        }

        private string databaseValLocation = PathLenghtCrawl.Properties.Settings.Default.DatabaseValLocation;
        public string DatabaseValLocation
        {
            get
            {
                return databaseValLocation;
            }
            set
            {
                if (databaseValLocation != value)
                {
                    databaseValLocation = value;
                    PathLenghtCrawl.Properties.Settings.Default.DatabaseValLocation = value;
                    SaveProperties();
                    NotifyPropertyChanged("DatabaseValLocation");
                }
            }
        }


        private string logLocationTxt = PathLenghtCrawl.Properties.Settings.Default.LogLocationTxt;
        public string LogLocationTxt
        {
            get
            {
                return logLocationTxt;
            }
            set
            {
                if (logLocationTxt != value)
                {
                    logLocationTxt = value;
                    PathLenghtCrawl.Properties.Settings.Default.LogLocationTxt = value;
                    SaveProperties();
                    NotifyPropertyChanged("LogLocationTxt");
                }
            }
        }

        private string filePath = PathLenghtCrawl.Properties.Settings.Default.FilePath;
        public string FilePath
        {
            get
            {
                return filePath;
            }
            set
            {
                if (filePath != value)
                {
                    filePath = value;
                    PathLenghtCrawl.Properties.Settings.Default.FilePath = value;
                    SaveProperties();
                    NotifyPropertyChanged("FilePath");
                }
            }
        }

        private string folderPath = PathLenghtCrawl.Properties.Settings.Default.FolderPath;
        public string FolderPath
        {
            get
            {
                return folderPath;
            }
            set
            {
                if (folderPath != value)
                {
                    folderPath = value;
                    PathLenghtCrawl.Properties.Settings.Default.FolderPath = value;
                    SaveProperties();
                    NotifyPropertyChanged("FolderPath");
                }
            }
        }

        private string statusMessage = "Ready";
        public string StatusMessage
        {
            get
            {
                return statusMessage;
            }
            set
            {
                if (statusMessage != value)
                {
                    statusMessage = value;
                    NotifyPropertyChanged("StatusMessage");
                }
            }
        }
        private System.IO.DirectoryInfo currentDirectory = new System.IO.DirectoryInfo(@"C:\");
        public System.IO.DirectoryInfo CurrentDirectory
        {
            get
            {
                return currentDirectory;
            }
            set
            {
                if (currentDirectory != value)
                {
                    currentDirectory = value;
                    //CurrentSelectedNode = value.Name;
                    NotifyPropertyChanged("CurrentDirectory");
                    SetCurrentUNCObject();
                    //GetDirectoryFiles();
                }
            }
        }

        private UNCObject currentUNCObject = new UNCObject();
        public UNCObject CurrentUNCObject
        {
            get
            {
                return currentUNCObject;
            }
            set
            {
                if (currentUNCObject != value)
                {
                    currentUNCObject = value;
                    NotifyPropertyChanged("CurrentUNCObject");
                }
            }
        }

        private List<UNCObject> uncObjectFileList = new List<UNCObject>();
        public List<UNCObject> UNCObjectFileList
        {
            get
            {
                return uncObjectFileList;
            }
            set
            {
                if (uncObjectFileList != value)
                {
                    uncObjectFileList = value;
                    NotifyPropertyChanged("UNCObjectFileList");
                }
            }
        }

        private List<UNCObject> uncObjectFolderList = new List<UNCObject>();
        public List<UNCObject> UNCObjectFolderList
        {
            get
            {
                return uncObjectFolderList;
            }
            set
            {
                if (uncObjectFolderList != value)
                {
                    uncObjectFolderList = value;
                    NotifyPropertyChanged("UNCObjectFolderList");
                }
            }
        }

        private System.Collections.ObjectModel.ObservableCollection<UNCObject> uncObjectFileLst = new System.Collections.ObjectModel.ObservableCollection<UNCObject>();
        public System.Collections.ObjectModel.ObservableCollection<UNCObject> UNCObjectFileLst
        {
            get
            {
                return uncObjectFileLst;
            }
            set
            {
                if (uncObjectFileLst != value)
                {
                    uncObjectFileLst = value;
                    NotifyPropertyChanged("UNCObjectFileLst");
                }
            }
        }

        private System.Collections.ObjectModel.ObservableCollection<UNCObject> uncObjectFolderLst = new System.Collections.ObjectModel.ObservableCollection<UNCObject>();
        public System.Collections.ObjectModel.ObservableCollection<UNCObject> UNCObjectFolderLst
        {
            get
            {
                return uncObjectFolderLst;
            }
            set
            {
                if (uncObjectFolderLst != value)
                {
                    uncObjectFolderLst = value;
                    NotifyPropertyChanged("UNCObjectFolderLst");
                }
            }
        }


        private System.Collections.ObjectModel.ObservableCollection<UNCObject> uncObjectLst = new System.Collections.ObjectModel.ObservableCollection<UNCObject>();
        public System.Collections.ObjectModel.ObservableCollection<UNCObject> UNCObjectLst
        {
            get
            {
                return uncObjectLst;
            }
            set
            {
                if (uncObjectLst != value)
                {
                    uncObjectLst = value;
                    NotifyPropertyChanged("UNCObjectLst");
                }
            }
        }

        private List<PathLenghtCrawl.POCO.Duration> durationList = new List<PathLenghtCrawl.POCO.Duration>();
        public List<PathLenghtCrawl.POCO.Duration> DurationList
        {
            get
            {
                return durationList;
            }
            set
            {
                if (durationList != value)
                {
                    durationList = value;
                    NotifyPropertyChanged("DurationList");
                }
            }
        }



        #endregion



        #region "************************************************************************************************* Bread & Butter"


        private ICommand importPathFileCommand;
        public ICommand ImportPathFileCommand
        {
            get
            {
                return importPathFileCommand ?? (importPathFileCommand = new CommandHandler(() => ImportPathFile(), _canExecute));
            }
        }
        //private async void ImportPathFile()
        private async void ImportPathFile()
        {
            SelectBulkFile2ProcessButtonEnabled = false;
            ImportProcessBulkFileButtonEnabled = false;
            StatusMessage = "Ready";

            PathImportedCount = 0;
            PathImportedCountTotal = 0;
            PathProcessedCount = 0;
            PathProcessedCountTotal = 0;
            PathCurrentlyProcessing = "N/A";

            ObjectCount = 0;
            FolderCount = 0;
            FileCount = 0;
            ObjectCountTotal = 0;
            FolderCountTotal = 0;
            FileCountTotal = 0;
            WarningCount = 0;
            ErrorCount = 0;

            //UNCObjectFileList.Clear();
            //UNCObjectFolderList.Clear();
            //UNCObjectFileLst.Clear();
            //UNCObjectFolderLst.Clear();

            List<string> uncPathsToScan = new List<string>();

            System.IO.FileInfo fi = new System.IO.FileInfo(PathFileImportTxt);
            PathImportedCountTotal = System.IO.File.ReadLines(fi.FullName).Count();


            using (System.IO.StreamReader reader = new System.IO.StreamReader(PathFileImportTxt))
            {
                while (!reader.EndOfStream)
                {
                    var CSValues = reader.ReadLine().Split(',');
                    string stringToDitch = "NDS://HEALTH_TREE";

                    string realValue = CSValues.FirstOrDefault().Replace(stringToDitch, "");

                    uncPathsToScan.Add(realValue);
                    PathImportedCount++;
                }
            }

            PathProcessedCountTotal = uncPathsToScan.Count();
            //UNCObjectFileLst.Clear();
            //UNCObjectFolderLst.Clear();
            UNCObjectFileList.Clear();

            string DateGuid = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            //List<PathLenghtCrawl.POCO.Duration> durations = new List<PathLenghtCrawl.POCO.Duration>();
            DurationList.Clear();


            await Task.Run(() =>
            {
                foreach (String path in uncPathsToScan)
                {
                    UNCObjectFileList.Clear();
                    UNCObjectFolderList.Clear();

                    ObjectCountTotal = 0;
                    FolderCountTotal = 0;
                    FileCountTotal = 0;

                    App.Current.Dispatcher.BeginInvoke((Action)delegate ()
                    {
                        UNCObjectFileLst.Clear();
                        UNCObjectFolderLst.Clear();
                    });

                    dtRunningPer.Start();
                    ResetTimerPer();
                    StartTimerPer();

                    try
                    {
                        CurrentDirectory = new System.IO.DirectoryInfo(path);
                        PathCurrentlyProcessing = path;
                        //Thread.Sleep(100);

                        bool success = ExecuteLPFNBulkList_Linq(DateGuid);
                        PathProcessedCount++;

                        StopTimerPer();
                        dtRunningPer.Stop();
                        AverageRunningTimePer = String.Format("{0:0.00}", Math.Round(AverageRunningTimePerLst.Average(), 2));
                        try
                        {
                            //AverageRunningTimePer = String.Format("{0:00}:{1:00}:{2:00}:{3:00}", Math.Round(AverageRunningTimePerLst.Average(), 2));
                        }
                        catch (Exception ex)
                        {
                            PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "AvrRunTImePer Error: line 875");
                        }
                    }
                    catch (Exception ex)
                    {
                        PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "Line 855");
                    }
                }
            });

            ObjectCountTotal = 0;
            FolderCountTotal = 0;
            FileCountTotal = 0;

            using (System.IO.StreamWriter resultsFile = new System.IO.StreamWriter(LogLocationTxt + System.IO.Path.DirectorySeparatorChar + "Statistics_" + DateGuid + ".csv", true, Encoding.Unicode))
            {
                foreach (PathLenghtCrawl.POCO.Duration duration in DurationList)
                {
                    resultsFile.WriteLine(duration.Name + "," + duration.Time);
                }
            }

            PathCurrentlyProcessing = "N/A";
            SelectBulkFile2ProcessButtonEnabled = true;
            ImportProcessBulkFileButtonEnabled = true;

        }


        private void NewAlgorithm()
        {


        }

        static long GetFileLength(System.IO.FileInfo fi)
        {
            long retval;
            try
            {
                //retval = fi.Length;
                retval = fi.FullName.Length;
            }
            catch (System.IO.FileNotFoundException)
            {
                // If a file is no longer present,  
                // just add zero bytes to the total.  
                retval = 0;
            }
            return retval;
        }
        static long GetFileLength(string fi)
        {
            long retval;
            try
            {
                //retval = fi.Length;
                retval = fi.Length;
            }
            catch (System.IO.FileNotFoundException)
            {
                // If a file is no longer present,  
                // just add zero bytes to the total.  
                retval = 0;
            }
            return retval;
        }
        static long GetDirectoryLength(System.IO.DirectoryInfo di)
        {
            long retval;
            try
            {
                retval = di.FullName.Length;
            }
            catch (System.IO.DirectoryNotFoundException)
            {
                // If a file is no longer present,  
                // just add zero bytes to the total.  
                retval = 0;
            }
            return retval;
        }
        static long GetDirectoryLength(string di)
        {
            long retval;
            try
            {
                retval = di.Length;
            }
            catch (System.IO.DirectoryNotFoundException)
            {
                // If a file is no longer present,  
                // just add zero bytes to the total.  
                retval = 0;
            }
            return retval;
        }

        //private void WhateverCheckFiles(System.IO.DirectoryInfo currentDI)
        private void WhateverCheckFilesLongWay(string currentDI)
        {
            try
            {
                IEnumerable<string> fiList = System.IO.Directory.EnumerateFiles(currentDI);
                int lkj = fiList.Where(x => x.Length > MinPathLength).Count();

                if (lkj > 0)
                {                    
                    //some, maybe all, files in this folder are LPFN
                    //LOG IT
                    //check parent
                    ObjectCountTotal += lkj;
                    FileCountTotal += lkj;
                }
            }
            #region "Catches"
            catch (ArgumentNullException ex)
            {
                ErrorCount++;
                StatusMessage = ex.Message;
                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "ArgumentNullException: Code4");
            }
            catch (System.IO.DirectoryNotFoundException ex)
            {
                ErrorCount++;
                StatusMessage = ex.Message;
                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "DirectoryNotFoundException: Code4");
            }
            catch (System.IO.PathTooLongException ex)
            {
                ErrorCount++;
                StatusMessage = ex.Message;
                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "PathTooLongException: Code4");
            }
            catch (System.IO.IOException ex)
            {
                ErrorCount++;
                StatusMessage = ex.Message;
                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "IOException: Code4");
            }
            catch (System.Security.SecurityException ex)
            {
                ErrorCount++;
                StatusMessage = ex.Message;
                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "SecurityException: Code4");
            }
            catch (UnauthorizedAccessException ex)
            {
                ErrorCount++;
                StatusMessage = ex.Message;
                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "UnauthorizedAccessException: Code4");
            }
            catch (ArgumentException ex)
            {
                ErrorCount++;
                StatusMessage = ex.Message;
                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "ArgumentException: Code4");
            }
            catch (Exception ex)
            {
                ErrorCount++;
                StatusMessage = ex.Message;
                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "Generic Exception: Code4");
            }
            #endregion


        }



        private ICommand whateverStartLongWayCommand;
        public ICommand WhateverStartLongWayCommand
        {
            get
            {
                return whateverStartLongWayCommand ?? (whateverStartLongWayCommand = new CommandHandler(() => WhateverStartLongWay(), _canExecute));
            }
        }

        private async void WhateverStartLongWay()
        {
            StatusMessage = "Processing...";
            ObjectCountTotal = 0;
            FolderCountTotal = 0;
            FileCountTotal = 0;
            ErrorCount = 0;
            WarningCount = 0;
            PathProcessedCount = 0;
            PathImportedCount = 0;
            PathCurrentlyCounting = "N/A";
            PathCurrentlyProcessing = "N/A";

            List<string> uncPathsToScan = new List<string>();
            string currentDI = CurrentDirectory.FullName;
            string DateGuid = DateTime.Now.ToString("yyyyMMddHHmmssffff");

            System.IO.FileInfo fi = new System.IO.FileInfo(PathFileImportTxt);
            PathImportedCountTotal = System.IO.File.ReadLines(fi.FullName).Count();


            using (System.IO.StreamReader reader = new System.IO.StreamReader(PathFileImportTxt))
            {
                while (!reader.EndOfStream)
                {
                    var CSValues = reader.ReadLine().Split(',');
                    string stringToDitch = "NDS://HEALTH_TREE";

                    string realValue = CSValues.FirstOrDefault().Replace(stringToDitch, "");

                    uncPathsToScan.Add(realValue);
                    PathImportedCount++;
                }
            }

            PathProcessedCountTotal = uncPathsToScan.Count();

            await Task.Run(() =>
            {
                foreach (String path in uncPathsToScan)
                {
                    CurrentDirectory = new System.IO.DirectoryInfo(path);
                    PathCurrentlyProcessing = path;

                    //Whatever(@"\\HOGA_HOGUC1S\HOGUC1\Users\JHamrlik");
                    WhateverLongWay(path);

                    

                    PathProcessedCount++;

                }
            });
            StatusMessage = "Completed";
        }
        private void WhateverLongWay(string currentDI)
        {
            try
            {
                PathCurrentlyCounting = currentDI;
                if (currentDI.Length > MinPathLength)
                {
                    ///LOG LP FOLDER NAME
                    ObjectCountTotal++;
                    FolderCountTotal++;
                    try
                    {
                        //WhateverCheckFilesLongWay(currentDI);

                        //IEnumerable<string> diList = System.IO.Directory.EnumerateDirectories(currentDI);
                        //if (diList.Count() > 0)
                        //{
                        //    foreach (string di in diList)
                        //    {
                        //        WhateverLongWay(di);
                        //    }
                        //}
                        //else
                        //{
                        //    //finished this branch
                        //}
                    }
                    #region "Catches"
                    catch (ArgumentNullException ex)
                    {
                        ErrorCount++;
                        StatusMessage = ex.Message;
                        PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "ArgumentNullException: Code1");
                    }
                    catch (System.IO.DirectoryNotFoundException ex)
                    {
                        ErrorCount++;
                        StatusMessage = ex.Message;
                        PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "DirectoryNotFoundException: Code1");
                    }
                    catch (System.IO.PathTooLongException ex)
                    {
                        ErrorCount++;
                        StatusMessage = ex.Message;
                        PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "PathTooLongException: Code1");
                    }
                    catch (System.IO.IOException ex)
                    {
                        ErrorCount++;
                        StatusMessage = ex.Message;
                        PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "IOException: Code1");
                    }
                    catch (System.Security.SecurityException ex)
                    {
                        ErrorCount++;
                        StatusMessage = ex.Message;
                        PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "SecurityException: Code1");
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        ErrorCount++;
                        StatusMessage = ex.Message;
                        PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "UnauthorizedAccessException: Code1");
                    }
                    catch (ArgumentException ex)
                    {
                        ErrorCount++;
                        StatusMessage = ex.Message;
                        PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "ArgumentException: Code1");
                    }
                    catch (Exception ex)
                    {
                        ErrorCount++;
                        StatusMessage = ex.Message;
                        PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "Generic Exception: Code1");
                    }
                    #endregion
                }
                else
                {
                    WhateverCheckFilesLongWay(currentDI);

                    IEnumerable<string> diList = System.IO.Directory.EnumerateDirectories(currentDI);
                    if (diList.Count() > 0)
                    {
                        foreach (string di in diList)
                        {
                            WhateverLongWay(di);
                        }
                    }
                    else
                    {
                        //finished this branch
                    }


                }
            }
            #region "Catches"
            catch (ArgumentNullException ex)
            {
                ErrorCount++;
                StatusMessage = ex.Message;
                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "ArgumentNullException: Code2");
            }
            catch (System.IO.DirectoryNotFoundException ex)
            {
                ErrorCount++;
                StatusMessage = ex.Message;
                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "DirectoryNotFoundException: Code2");
            }
            catch (System.IO.PathTooLongException ex)
            {
                ErrorCount++;
                StatusMessage = ex.Message;
                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "PathTooLongException: Code2");
            }
            catch (System.IO.IOException ex)
            {
                ErrorCount++;
                StatusMessage = ex.Message;
                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "IOException: Code2");
            }
            catch (System.Security.SecurityException ex)
            {
                ErrorCount++;
                StatusMessage = ex.Message;
                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "SecurityException: Code2");
            }
            catch (UnauthorizedAccessException ex)
            {
                ErrorCount++;
                StatusMessage = ex.Message;
                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "UnauthorizedAccessException: Code2");
            }
            catch (ArgumentException ex)
            {
                ErrorCount++;
                StatusMessage = ex.Message;
                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "ArgumentException: Code2");
            }
            catch (Exception ex)
            {
                ErrorCount++;
                StatusMessage = ex.Message;
                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "Generic Exception: Code2");
            }
            #endregion





            //if (currentDI.FullName.Length > 255)
            //{
            //    //LONG PATH FOLDER NAME, LOG IT
            //}
            //else
            //{
            //    IEnumerable<System.IO.DirectoryInfo> diList = currentDI.EnumerateDirectories();
            //    if (diList.Count() > 0)
            //    {
            //        foreach (System.IO.DirectoryInfo di in diList)
            //        {
            //            Whatever(di);
            //        }
            //    }
            //    else
            //    {
            //        WhateverCheckFiles(currentDI);
            //    }
            //}




            //if (currentDI.FullName.Length > 255)
            //{
            //    //add everything here as LPFN
            //}
            //else
            //{
            //    IEnumerable<System.IO.FileInfo> fiList = currentDI.EnumerateFiles();
            //    int lkj = fiList.Where(x => x.FullName.Length > 255).Count();

            //    if (lkj > 0)
            //    {
            //        //some, maybe all, files in this folder are LPFN
            //    }
            //    else
            //    {
            //        IEnumerable<System.IO.DirectoryInfo> diList = currentDI.EnumerateDirectories();
            //        if (diList.Count() > 0)
            //        {
            //            foreach (System.IO.DirectoryInfo di in diList)
            //            {
            //                Whatever(di);
            //            }
            //        }
            //    }
            //}



        }

        //private void WhateverCheckFiles(System.IO.DirectoryInfo currentDI)
        private void WhateverCheckFiles(string currentDI)
        {
            try
            {
                IEnumerable<string> fiList = System.IO.Directory.EnumerateFiles(currentDI);
                int lkj = fiList.Where(x => x.Length > MinPathLength).Count();

                if (lkj > 0)
                {
                    //some, maybe all, files in this folder are LPFN
                    //LOG IT
                    //check parent
                    ObjectCountTotal += lkj;
                    FileCountTotal += lkj;

                    string fn = System.IO.Directory.GetParent(currentDI).FullName;
                    WhateverCheckFiles(fn);
                }
                else
                {
                    //exit this branch, NO LPFN
                }
            }
            catch (Exception ex)
            {
                string lsdkjf = ex.Message;
                throw;
            }


            //IEnumerable<System.IO.FileInfo> fiList = currentDI.EnumerateFiles();
            //int lkj = fiList.Where(x => x.FullName.Length > 255).Count();

            //if (lkj > 0)
            //{
            //    //some, maybe all, files in this folder are LPFN
            //    //LOG IT
            //    //check parent
            //    WhateverCheckFiles(currentDI.Parent);
            //}
            //else
            //{
            //    //exit this branch, NO LPFN
            //}

        }



        private ICommand whateverStartCommand;
        public ICommand WhateverStartCommand
        {
            get
            {
                return whateverStartCommand ?? (whateverStartCommand = new CommandHandler(() => WhateverStart(), _canExecute));
            }
        }

        private async void WhateverStart()
        {
            StatusMessage = "Processing...";
            ObjectCountTotal = 0;
            FolderCountTotal = 0;
            FileCountTotal = 0;

            string currentDI = CurrentDirectory.FullName;
            await Task.Run(() =>
            {
                //Whatever(currentDI);

                //System.IO.DirectoryInfo did = new System.IO.DirectoryInfo(@"\\HOGA_HOGUC1S\HOGUC1\Users\JHamrlik");
                System.IO.DirectoryInfo did = new System.IO.DirectoryInfo(@"C:\Mitch");


                Whatever(@"\\HOGA_HOGUC1S\HOGUC1\Users\JHamrlik");
                //Whatever(did.FullName);


                int lkj = 9;
            });
            StatusMessage = "Completed";
        }
        private void Whatever(string currentDI)
        {
            try
            {
                IEnumerable<string> diList = System.IO.Directory.EnumerateDirectories(currentDI);
                if (diList.Count() > 0)
                {
                    foreach (string di in diList)
                    {
                        if (di.Length > MinPathLength)
                        {
                            //LP FolderNAMe
                            //LOG IT
                            ObjectCountTotal++;
                            FolderCountTotal++;
                        }
                        else
                        {
                            Whatever(di);
                        }
                    }
                }
                else
                {
                    WhateverCheckFiles(currentDI);

                }

            }
            catch (Exception ex)
            {
                string lsdkjf = ex.Message;
                throw;
            }





            //if (currentDI.FullName.Length > 255)
            //{
            //    //LONG PATH FOLDER NAME, LOG IT
            //}
            //else
            //{
            //    IEnumerable<System.IO.DirectoryInfo> diList = currentDI.EnumerateDirectories();
            //    if (diList.Count() > 0)
            //    {
            //        foreach (System.IO.DirectoryInfo di in diList)
            //        {
            //            Whatever(di);
            //        }
            //    }
            //    else
            //    {
            //        WhateverCheckFiles(currentDI);
            //    }
            //}




            //if (currentDI.FullName.Length > 255)
            //{
            //    //add everything here as LPFN
            //}
            //else
            //{
            //    IEnumerable<System.IO.FileInfo> fiList = currentDI.EnumerateFiles();
            //    int lkj = fiList.Where(x => x.FullName.Length > 255).Count();

            //    if (lkj > 0)
            //    {
            //        //some, maybe all, files in this folder are LPFN
            //    }
            //    else
            //    {
            //        IEnumerable<System.IO.DirectoryInfo> diList = currentDI.EnumerateDirectories();
            //        if (diList.Count() > 0)
            //        {
            //            foreach (System.IO.DirectoryInfo di in diList)
            //            {
            //                Whatever(di);
            //            }
            //        }
            //    }
            //}



        }
        private bool ExecuteLPFNBulkList_Linq(string DateGuid)
        {
            System.IO.Directory.CreateDirectory(LogLocationTxt + System.IO.Path.DirectorySeparatorChar + "Details");
            bool success = true;

            //CurrentDirectory = new System.IO.DirectoryInfo(@"Z:\Users\JHamrlik");
            //CurrentDirectory = new System.IO.DirectoryInfo(@"C:\");
            if (!System.IO.File.Exists(PathFileImportTxt))
            {
                StatusMessage = "File does not exists.";
            }
            else
            {
                bool alreadyUNC = false;
                if (CurrentDirectory.FullName.StartsWith("\\"))
                {
                    alreadyUNC = true;
                }
                try
                {
                    dtRunningPer.Tick += new EventHandler(dtRunningPer_Tick);
                    dtRunningPer.Interval = new TimeSpan(0, 0, 0, 0, 1);

                    if (IncludeDocuments)
                    {
                        try
                        {
                            StatusMessage = "Processing Files...";
                            string fileNamePath = CurrentDirectory.FullName.Substring(2).Replace("\\", "_");
                            string logFileName = "Files_" + fileNamePath + "_" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + "_" + "lpfn.csv";
                            using (System.IO.StreamWriter resultsFileByPathType = new System.IO.StreamWriter(LogLocationTxt + System.IO.Path.DirectorySeparatorChar + "Details" + System.IO.Path.DirectorySeparatorChar + logFileName, false, Encoding.Unicode))
                            {
                                string masterFileNameWOXtension = System.IO.Path.GetFileNameWithoutExtension(PathFileImportTxt);
                                string masterFileLogName = masterFileNameWOXtension + "_" + DateGuid + "_" + "lpfn.csv";
                                using (System.IO.StreamWriter resultsFileGlobal = new System.IO.StreamWriter(LogLocationTxt + System.IO.Path.DirectorySeparatorChar + masterFileLogName, true, Encoding.Unicode))
                                {
                                    StatusMessage = "Processing Files...";
                                    try
                                    {
                                        foreach (String fi in System.IO.Directory.EnumerateFiles(CurrentDirectory.FullName, "*.*", System.IO.SearchOption.AllDirectories).Where(x => x.Length > MinPathLength))
                                        {
                                            try
                                            {
                                                PathCurrentlyCounting = fi;
                                                //if (!System.IO.File.Exists(fi))
                                                //{ 
                                                //    WarningCount++;
                                                //    PathLenghtCrawl.Log.Log.Write2WarningLog(LogLocationTxt, DateTime.Now, "File does Not Exists.", fi);
                                                //}
                                                ObjectCountTotal++;
                                                FileCountTotal++;
                                                ObjectCount++;
                                                FileCount++;
                                                try
                                                {
                                                    resultsFileByPathType.WriteLine(FileCount + "," + fi.Length + "," + fi);
                                                }
                                                catch (Exception ex)
                                                {
                                                    StatusMessage = ex.Message;
                                                    PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "Error while writing to " + fileNamePath);
                                                }
                                                try
                                                {
                                                    resultsFileGlobal.WriteLine(FolderCount + "," + fi.Length + "," + fi);
                                                }
                                                catch (Exception ex)
                                                {
                                                    StatusMessage = ex.Message;
                                                    PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "Error while writing to " + masterFileLogName);
                                                }

                                                UNCObject uncObject = new UNCObject() { Count = FileCount, CharacterCount = fi.Length, NameUNC = fi };

                                                UNCObjectFileList.Add(uncObject);
                                                App.Current.Dispatcher.BeginInvoke((Action)delegate ()
                                                {
                                                    UNCObjectFileLst.Add(uncObject);
                                                });
                                            }
                                            #region "Catches"
                                            catch (ArgumentNullException ex)
                                            {
                                                ErrorCount++;
                                                StatusMessage = ex.Message;
                                                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "ArgumentNullException: Error in Files inside ForEach with " + fi);
                                            }
                                            catch (System.IO.DirectoryNotFoundException ex)
                                            {
                                                ErrorCount++;
                                                StatusMessage = ex.Message;
                                                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "DirectoryNotFoundException: Error in Files inside ForEach with " + fi);
                                            }
                                            catch (System.IO.PathTooLongException ex)
                                            {
                                                ErrorCount++;
                                                StatusMessage = ex.Message;
                                                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "PathTooLongException: Error in Files inside ForEach with " + fi);
                                            }
                                            catch (System.IO.IOException ex)
                                            {
                                                ErrorCount++;
                                                StatusMessage = ex.Message;
                                                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "IOException: Error in Files inside ForEach with " + fi);
                                            }
                                            catch (System.Security.SecurityException ex)
                                            {
                                                ErrorCount++;
                                                StatusMessage = ex.Message;
                                                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "SecurityException: Error in Files inside ForEach with " + fi);
                                            }
                                            catch (UnauthorizedAccessException ex)
                                            {
                                                ErrorCount++;
                                                StatusMessage = ex.Message;
                                                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "UnauthorizedAccessException: Error in Files inside ForEach with " + fi);
                                            }
                                            catch (ArgumentException ex)
                                            {
                                                ErrorCount++;
                                                StatusMessage = ex.Message;
                                                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "ArgumentException: Error in Files inside ForEach with " + fi);
                                            }
                                            catch (Exception ex)
                                            {
                                                ErrorCount++;
                                                StatusMessage = ex.Message;
                                                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "Generic Exception: Error in Files inside ForEach with " + fi);
                                            }
                                            #endregion
                                        }
                                    }
                                    #region "Catches"
                                    catch (ArgumentNullException ex)
                                    {
                                        ErrorCount++;
                                        StatusMessage = ex.Message;
                                        PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "ArgumentNullException: Error in Files outside ForEach");
                                    }
                                    catch (System.IO.DirectoryNotFoundException ex)
                                    {
                                        ErrorCount++;
                                        StatusMessage = ex.Message;
                                        PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "DirectoryNotFoundException: Error in Files outside ForEach");
                                    }
                                    catch (System.IO.PathTooLongException ex)
                                    {
                                        ErrorCount++;
                                        StatusMessage = ex.Message;
                                        PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "PathTooLongException: Error in Files outside ForEach");
                                    }
                                    catch (System.IO.IOException ex)
                                    {
                                        ErrorCount++;
                                        StatusMessage = ex.Message;
                                        PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "IOException: Error in Files outside ForEach");
                                    }
                                    catch (System.Security.SecurityException ex)
                                    {
                                        ErrorCount++;
                                        StatusMessage = ex.Message;
                                        PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "SecurityException: Error in Files outside ForEach");
                                    }
                                    catch (UnauthorizedAccessException ex)
                                    {
                                        ErrorCount++;
                                        StatusMessage = ex.Message;
                                        PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "UnauthorizedAccessException: Error in Files outside ForEach");
                                    }
                                    catch (ArgumentException ex)
                                    {
                                        ErrorCount++;
                                        StatusMessage = ex.Message;
                                        PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "ArgumentException: Error in Files outside ForEach");
                                    }
                                    catch (Exception ex)
                                    {
                                        ErrorCount++;
                                        StatusMessage = ex.Message;
                                        PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "Generic Exception: Error in Files outside ForEach");
                                    }
                                    #endregion

                                    PathLenghtCrawl.POCO.Duration currentDuration = new PathLenghtCrawl.POCO.Duration() { Name = CurrentDirectory.FullName + " (Files)", Time = ValidatorRunningTimePer };
                                    DurationList.Add(currentDuration);

                                }
                            }
                        }
                        #region "Catches"
                        catch (ArgumentNullException ex)
                        {
                            ErrorCount++;
                            StatusMessage = ex.Message;
                            PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "ArgumentNullException: Error while gathering files");
                        }
                        catch (System.IO.DirectoryNotFoundException ex)
                        {
                            ErrorCount++;
                            StatusMessage = ex.Message;
                            PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "DirectoryNotFoundException: Error while gathering files");
                        }
                        catch (System.IO.PathTooLongException ex)
                        {
                            ErrorCount++;
                            StatusMessage = ex.Message;
                            PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "PathTooLongException: Error while gathering files");
                        }
                        catch (System.IO.IOException ex)
                        {
                            ErrorCount++;
                            StatusMessage = ex.Message;
                            PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "IOException: Error while gathering files");
                        }
                        catch (System.Security.SecurityException ex)
                        {
                            ErrorCount++;
                            StatusMessage = ex.Message;
                            PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "SecurityException: Error while gathering files");
                        }
                        catch (UnauthorizedAccessException ex)
                        {
                            ErrorCount++;
                            StatusMessage = ex.Message;
                            PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "UnauthorizedAccessException: Error while gathering files");
                        }
                        catch (ArgumentException ex)
                        {
                            ErrorCount++;
                            StatusMessage = ex.Message;
                            PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "ArgumentException: Error while gathering files");
                        }
                        catch (Exception ex)
                        {
                            ErrorCount++;
                            StatusMessage = ex.Message;
                            PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "Generic Exception: Error while gathering files");
                        }
                        #endregion
                        finally
                        {
                            //StatusMessage = "Creating Reports...";

                            //string fileNamePath = CurrentDirectory.FullName.Substring(2).Replace("\\", "_");
                            //using (System.IO.StreamWriter resultsFile = new System.IO.StreamWriter(LogLocationTxt + System.IO.Path.DirectorySeparatorChar + "Files_" + fileNamePath + "_" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + "_" + "results.csv", false, Encoding.Unicode))
                            //{
                            //    try
                            //    {
                            //        foreach (UNCObject path in UNCObjectFileList)
                            //        {
                            //            resultsFile.WriteLine(path.Count + "," + path.CharacterCount + "," + path.NameUNC);
                            //        }
                            //    }
                            //    catch (Exception ex)
                            //    {
                            //        PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "Error while writing log (files)");
                            //    }
                            //}
                            //string masterFileNameWOXtension = System.IO.Path.GetFileNameWithoutExtension(PathFileImportTxt);
                            //using (System.IO.StreamWriter resultsFile = new System.IO.StreamWriter(LogLocationTxt + System.IO.Path.DirectorySeparatorChar + masterFileNameWOXtension + "_" + DateGuid + "_" + "lpfn.csv", true, Encoding.Unicode))
                            //{
                            //    try
                            //    {
                            //        foreach (UNCObject path in UNCObjectFileList)
                            //        {
                            //            resultsFile.WriteLine(path.Count + "," + path.CharacterCount + "," + path.NameUNC);
                            //        }
                            //    }
                            //    catch (Exception ex)
                            //    {
                            //        PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "Error while writing master log (files)");
                            //    }
                            //}
                        }
                    }

                    if (IncludeFolders)
                    {
                        try
                        {
                            string fileNamePath = CurrentDirectory.FullName.Substring(2).Replace("\\", "_");
                            string logFileName = "Folders_" + fileNamePath + "_" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + "_" + "lpfn.csv";
                            using (System.IO.StreamWriter resultsFileByPathType = new System.IO.StreamWriter(LogLocationTxt + System.IO.Path.DirectorySeparatorChar + "Details" + System.IO.Path.DirectorySeparatorChar + logFileName, false, Encoding.Unicode))
                            {
                                string masterFileNameWOXtension = System.IO.Path.GetFileNameWithoutExtension(PathFileImportTxt);
                                string masterFileLogName = masterFileNameWOXtension + "_" + DateGuid + "_" + "lpfn.csv";
                                using (System.IO.StreamWriter resultsFileGlobal = new System.IO.StreamWriter(LogLocationTxt + System.IO.Path.DirectorySeparatorChar + masterFileNameWOXtension + "_" + DateGuid + "_" + "lpfn.csv", true, Encoding.Unicode))
                                {
                                    StatusMessage = "Processing Folders...";
                                    try
                                    {
                                        foreach (String di in System.IO.Directory.EnumerateDirectories(CurrentDirectory.FullName, "*", System.IO.SearchOption.AllDirectories).Where(x => x.Length > MinPathLength))
                                        {
                                            try
                                            {
                                                PathCurrentlyCounting = di;
                                                //if (!System.IO.Directory.Exists(di))
                                                //{
                                                //    WarningCount++;
                                                //    PathLenghtCrawl.Log.Log.Write2WarningLog(LogLocationTxt, DateTime.Now, "Directory does Not Exists.", di);
                                                //}
                                                ObjectCountTotal++;
                                                FolderCountTotal++;
                                                ObjectCount++;
                                                FolderCount++;

                                                try
                                                {
                                                    resultsFileByPathType.WriteLine(FolderCount + "," + di.Length + "," + di);
                                                }
                                                catch (Exception ex)
                                                {
                                                    StatusMessage = ex.Message;
                                                    PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "Error while writing to " + logFileName);
                                                }
                                                try
                                                {
                                                    resultsFileGlobal.WriteLine(FolderCount + "," + di.Length + "," + di);
                                                }
                                                catch (Exception ex)
                                                {
                                                    StatusMessage = ex.Message;
                                                    PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "Error while writing to " + masterFileLogName);
                                                }

                                                UNCObject uncObject = new UNCObject() { Count = FolderCount, CharacterCount = di.Length, NameUNC = di };
                                                UNCObjectFolderList.Add(uncObject);

                                                App.Current.Dispatcher.BeginInvoke((Action)delegate ()
                                                {
                                                    UNCObjectFolderLst.Add(uncObject);
                                                });
                                            }
                                            #region "Catches"
                                            catch (ArgumentNullException ex)
                                            {
                                                ErrorCount++;
                                                StatusMessage = ex.Message;
                                                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "ArgumentNullException: Error in Directory inside ForEach with " + di);
                                            }
                                            catch (System.IO.DirectoryNotFoundException ex)
                                            {
                                                ErrorCount++;
                                                StatusMessage = ex.Message;
                                                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "DirectoryNotFoundException: Error in Directory inside ForEach with " + di);
                                            }
                                            catch (System.IO.PathTooLongException ex)
                                            {
                                                ErrorCount++;
                                                StatusMessage = ex.Message;
                                                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "PathTooLongException: Error in Directory inside ForEach with " + di);
                                            }
                                            catch (System.IO.IOException ex)
                                            {
                                                ErrorCount++;
                                                StatusMessage = ex.Message;
                                                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "IOException: Error in Directory inside ForEach with " + di);
                                            }
                                            catch (System.Security.SecurityException ex)
                                            {
                                                ErrorCount++;
                                                StatusMessage = ex.Message;
                                                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "SecurityException: Error in Directory inside ForEach with " + di);
                                            }
                                            catch (UnauthorizedAccessException ex)
                                            {
                                                ErrorCount++;
                                                StatusMessage = ex.Message;
                                                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "UnauthorizedAccessException: Error in Directory inside ForEach with " + di);
                                            }
                                            catch (ArgumentException ex)
                                            {
                                                ErrorCount++;
                                                StatusMessage = ex.Message;
                                                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "ArgumentException: Error in Directory inside ForEach with " + di);
                                            }
                                            catch (Exception ex)
                                            {
                                                ErrorCount++;
                                                StatusMessage = ex.Message;
                                                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "Generic Exception: Error in Directory inside ForEach with " + di);
                                            }
                                            #endregion
                                        }
                                    }
                                    #region "Catches"
                                    catch (ArgumentNullException ex)
                                    {
                                        ErrorCount++;
                                        StatusMessage = ex.Message;
                                        PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "ArgumentNullException: Error in Directory outside ForEach");
                                    }
                                    catch (System.IO.DirectoryNotFoundException ex)
                                    {
                                        ErrorCount++;
                                        StatusMessage = ex.Message;
                                        PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "DirectoryNotFoundException: Error in Directory outside ForEach");
                                    }
                                    catch (System.IO.PathTooLongException ex)
                                    {
                                        ErrorCount++;
                                        StatusMessage = ex.Message;
                                        PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "PathTooLongException: Error in Directory outside ForEach");
                                    }
                                    catch (System.IO.IOException ex)
                                    {
                                        ErrorCount++;
                                        StatusMessage = ex.Message;
                                        PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "IOException: Error in Directory outside ForEach");
                                    }
                                    catch (System.Security.SecurityException ex)
                                    {
                                        ErrorCount++;
                                        StatusMessage = ex.Message;
                                        PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "SecurityException: Error in Directory outside ForEach");
                                    }
                                    catch (UnauthorizedAccessException ex)
                                    {
                                        ErrorCount++;
                                        StatusMessage = ex.Message;
                                        PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "UnauthorizedAccessException: Error in Directory outside ForEach");
                                    }
                                    catch (ArgumentException ex)
                                    {
                                        ErrorCount++;
                                        StatusMessage = ex.Message;
                                        PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "ArgumentException: Error in Directory outside ForEach");
                                    }
                                    catch (Exception ex)
                                    {
                                        ErrorCount++;
                                        StatusMessage = ex.Message;
                                        PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "Generic Exception: Error in Directory outside ForEach");
                                    }
                                    #endregion

                                    PathLenghtCrawl.POCO.Duration currentDuration = new PathLenghtCrawl.POCO.Duration() { Name = CurrentDirectory.FullName + " (+ Folders)", Time = ValidatorRunningTimePer };
                                    DurationList.Add(currentDuration);
                                }
                            }
                        }
                        #region "Catches"
                        catch (ArgumentNullException ex)
                        {
                            ErrorCount++;
                            StatusMessage = ex.Message;
                            PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "ArgumentNullException: Error while gathering directories");
                        }
                        catch (System.IO.DirectoryNotFoundException ex)
                        {
                            ErrorCount++;
                            StatusMessage = ex.Message;
                            PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "DirectoryNotFoundException: Error while gathering directories");
                        }
                        catch (System.IO.PathTooLongException ex)
                        {
                            ErrorCount++;
                            StatusMessage = ex.Message;
                            PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "PathTooLongException: Error while gathering directories");
                        }
                        catch (System.IO.IOException ex)
                        {
                            ErrorCount++;
                            StatusMessage = ex.Message;
                            PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "IOException: Error while gathering directories");
                        }
                        catch (System.Security.SecurityException ex)
                        {
                            ErrorCount++;
                            StatusMessage = ex.Message;
                            PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "SecurityException: Error while gathering directories");
                        }
                        catch (UnauthorizedAccessException ex)
                        {
                            ErrorCount++;
                            StatusMessage = ex.Message;
                            PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "UnauthorizedAccessException: Error while gathering directories");
                        }
                        catch (ArgumentException ex)
                        {
                            ErrorCount++;
                            StatusMessage = ex.Message;
                            PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "ArgumentException: Error while gathering directories");
                        }
                        catch (Exception ex)
                        {
                            ErrorCount++;
                            StatusMessage = ex.Message;
                            PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "Generic Exception: Error while gathering directories");
                        }
                        #endregion
                        finally
                        {
                            //StatusMessage = "Creating Reports...";

                            //string fileNamePath = CurrentDirectory.FullName.Substring(2).Replace("\\", "_");
                            //using (System.IO.StreamWriter resultsFile = new System.IO.StreamWriter(LogLocationTxt + System.IO.Path.DirectorySeparatorChar + "Folders_" + fileNamePath + "_" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + "_" + "lpfn.csv", false, Encoding.Unicode))
                            //{
                            //    try
                            //    {
                            //        foreach (UNCObject path in UNCObjectFolderList)
                            //        {
                            //            resultsFile.WriteLine(path.Count + "," + path.CharacterCount + "," + path.NameUNC);
                            //        }
                            //    }
                            //    catch (Exception ex)
                            //    {
                            //        StatusMessage = ex.Message;
                            //        PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "Error while writing log (folders)");
                            //    }
                            //}
                            //string masterFileNameWOXtension = System.IO.Path.GetFileNameWithoutExtension(PathFileImportTxt);
                            //using (System.IO.StreamWriter resultsFile = new System.IO.StreamWriter(LogLocationTxt + System.IO.Path.DirectorySeparatorChar + masterFileNameWOXtension + "_" + DateGuid + "_" + "lpfn.csv", true, Encoding.Unicode))
                            //{
                            //    try
                            //    {
                            //        foreach (UNCObject path in UNCObjectFolderList)
                            //        {
                            //            resultsFile.WriteLine(path.Count + "," + path.CharacterCount + "," + path.NameUNC);
                            //        }
                            //    }
                            //    catch (Exception ex)
                            //    {
                            //        StatusMessage = ex.Message;
                            //        PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "Error while writing master log (folders)");
                            //    }
                            //}
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrorCount++;
                    StatusMessage = ex.Message;
                    PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "General error in ExecuteLPFNBulkList_Linq()");
                }
                finally
                { PathCurrentlyCounting = "N/A"; }


            }
            if (ErrorCount > 0)
            {
                success = false;
                StatusMessage = "Check Error Logs";

            }
            else
            {
                StatusMessage = "Ready";
            }
            return success;
        }

        private void ExecuteLPFNBulkList()
        {

            bool alreadyUNC = false;
            if (CurrentDirectory.FullName.StartsWith("\\")) { alreadyUNC = true; }
            try
            {
                dtRunningPer.Tick += new EventHandler(dtRunningPer_Tick);
                dtRunningPer.Interval = new TimeSpan(0, 0, 0, 0, 1);

                StatusMessage = "Gathering Information (Files)...";
                System.IO.FileInfo[] fileInfos = CurrentDirectory.GetFiles();
                IEnumerable<System.IO.FileInfo> fileList = CurrentDirectory.GetFiles("*.*", System.IO.SearchOption.AllDirectories);
                StatusMessage = "Processing Files...";

                try
                {
                    foreach (System.IO.FileInfo fi in fileInfos)
                    {
                        string fiInUNC = String.Empty;
                        if (!alreadyUNC)
                        {
                            try
                            {
                                fiInUNC = MappedDriveResolver.ResolveToUNC(fi.FullName);
                            }
                            catch (Exception)
                            {
                                throw;
                            }

                        }
                        else
                        {
                            fiInUNC = fi.FullName;
                        }
                        try
                        {

                            int fileLenght = fiInUNC.Length;
                            if (fileLenght > MinPathLength)
                            {
                                UNCObject uncObject = new UNCObject() { CharacterCount = fileLenght, NameUNC = fiInUNC };
                                ObjectCount++;
                                FileCount++;
                                UNCObjectFileLst.Add(uncObject);
                            }
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                    }
                }
                catch (Exception)
                {

                    throw;
                }
                StatusMessage = "Gathering Information (Folders)...";
                System.IO.DirectoryInfo[] directoryInfos = CurrentDirectory.GetDirectories("*.*", System.IO.SearchOption.AllDirectories);
                StatusMessage = "Processing Folders...";
                try
                {


                    foreach (System.IO.DirectoryInfo di in directoryInfos)
                    {
                        string diInUNC = String.Empty;
                        if (!alreadyUNC)
                        {
                            try
                            {
                                diInUNC = MappedDriveResolver.ResolveToUNC(di.FullName);
                            }
                            catch (Exception)
                            {
                                throw;
                            }

                        }
                        else
                        {
                            diInUNC = di.FullName;
                        }



                        int directoryLenght = diInUNC.Length;
                        if (directoryLenght > MinPathLength)
                        {
                            try
                            {
                                UNCObject uncObject = new UNCObject() { CharacterCount = directoryLenght, NameUNC = diInUNC };
                                ObjectCount++;
                                FolderCount++;
                                UNCObjectFileLst.Add(uncObject);
                            }
                            catch (Exception)
                            {
                                throw;
                            }

                        }
                        try
                        {
                            System.IO.FileInfo[] fileInfos1;
                            try
                            {
                                fileInfos1 = di.GetFiles();
                            }
                            catch (Exception ex)
                            {

                                throw;
                            }

                            foreach (System.IO.FileInfo fi in fileInfos1)
                            {
                                try
                                {


                                    string fiInUNC = String.Empty;
                                    if (!alreadyUNC)
                                    {
                                        try
                                        {
                                            fiInUNC = MappedDriveResolver.ResolveToUNC(fi.FullName);
                                        }
                                        catch (Exception ex)
                                        {

                                            throw;
                                        }

                                    }
                                    else
                                    {
                                        fiInUNC = fi.FullName;
                                    }
                                    try
                                    {
                                        int fileLenght = fiInUNC.Length;
                                        if (fileLenght > MinPathLength)
                                        {
                                            UNCObject uncObject = new UNCObject() { CharacterCount = fileLenght, NameUNC = fiInUNC };
                                            ObjectCount++;
                                            FileCount++;
                                            UNCObjectFileLst.Add(uncObject);

                                        }
                                    }
                                    catch (Exception ex)
                                    {

                                        throw;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    throw;
                                }
                            }
                        }
                        catch (Exception es)
                        {
                            //break was here the last time
                            throw;
                        }
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }
            catch (UnauthorizedAccessException uae)
            {
                StatusMessage = "Unauthorized Access";
                //success = false;
            }
            catch (Exception ex)
            {
                StatusMessage = "Problem detected";
                //success = false;
            }
            StatusMessage = "Ready";
        }
        private ICommand executeLPFNListCommand;
        public ICommand ExecuteLPFNListCommand
        {
            get
            {
                return executeLPFNListCommand ?? (executeLPFNListCommand = new CommandHandler(() => ExecuteLPFNList(), _canExecute));
            }
        }
        private async void ExecuteLPFNList()
        {
            await Task.Run(() =>
            {
                CopyAsCSVEnabled = false;
                SaveAsCSVEnabled = false;

                ObjectCount = 0;
                FolderCount = 0;
                FileCount = 0;
                ObjectCountTotal = 0;
                FolderCountTotal = 0;
                FileCountTotal = 0;



                App.Current.Dispatcher.BeginInvoke((Action)delegate ()
                {
                    UNCObjectLst.Clear();
                });
                try
                {
                    //FileCountTotal = System.IO.Directory.EnumerateFiles(CurrentDirectory.FullName, "*.*", System.IO.SearchOption.AllDirectories).Count();
                    //FolderCountTotal = System.IO.Directory.EnumerateDirectories(CurrentDirectory.FullName, "*", System.IO.SearchOption.AllDirectories).Count();
                    //ObjectCountTotal = FileCountTotal + FolderCountTotal;

                    //System.Collections.ObjectModel.ObservableCollection<UNCObject> uncObjectList = new System.Collections.ObjectModel.ObservableCollection<UNCObject>();
                    foreach (System.IO.FileInfo fi in CurrentDirectory.GetFiles())
                    {
                        if (MappedDriveResolver.ResolveToUNC(fi.FullName).Length > MinPathLength)
                        {
                            UNCObject uncObject = new UNCObject() { CharacterCount = MappedDriveResolver.ResolveToUNC(fi.FullName).Length, NameUNC = MappedDriveResolver.ResolveToUNC(fi.FullName) };
                            App.Current.Dispatcher.BeginInvoke((Action)delegate ()
                            {
                                UNCObjectLst.Add(uncObject);
                                ObjectCount++;
                                FileCount++;
                            });
                        }
                    }

                    foreach (System.IO.DirectoryInfo di in CurrentDirectory.GetDirectories("*.*", System.IO.SearchOption.AllDirectories))
                    {
                        if (MappedDriveResolver.ResolveToUNC(di.FullName).Length > MinPathLength)
                        {
                            UNCObject uncObject = new UNCObject() { CharacterCount = MappedDriveResolver.ResolveToUNC(di.FullName).Length, NameUNC = MappedDriveResolver.ResolveToUNC(di.FullName) };
                            App.Current.Dispatcher.BeginInvoke((Action)delegate ()
                            {
                                UNCObjectLst.Add(uncObject);
                                FolderCount++;
                                ObjectCount++;
                            });
                        }
                        foreach (System.IO.FileInfo fi in di.GetFiles())
                        {
                            if (MappedDriveResolver.ResolveToUNC(fi.FullName).Length > MinPathLength)
                            {
                                UNCObject uncObject = new UNCObject() { CharacterCount = MappedDriveResolver.ResolveToUNC(fi.FullName).Length, NameUNC = MappedDriveResolver.ResolveToUNC(fi.FullName) };
                                App.Current.Dispatcher.BeginInvoke((Action)delegate ()
                                {
                                    UNCObjectLst.Add(uncObject);
                                    FileCount++;
                                    ObjectCount++;
                                });
                            }
                        }
                    }
                }
                catch (UnauthorizedAccessException uae)
                {
                    StatusMessage = "Unauthorized Access";
                    //success = false;
                }
                catch (Exception ex)
                {
                    StatusMessage = "Problem detected";
                    //success = false;
                }

                CopyAsCSVEnabled = true;
                SaveAsCSVEnabled = true;
            });
        }
        private ICommand compareUserListsCommand;
        public ICommand CompareUserListsCommand
        {
            get
            {
                return compareUserListsCommand ?? (compareUserListsCommand = new CommandHandler(() => CompareUserLists(), _canExecute));
            }
        }
        private void CompareUserLists()
        {
            List<string> userList1 = new List<string>();
            List<string> userList2 = new List<string>();

            using (var reader = new System.IO.StreamReader(@"C:\Mitch\New folder\UsersHCISC\Users1.csv"))
            {
                while (!reader.EndOfStream)
                {
                    userList1.Add(reader.ReadLine());
                }
            }

            using (var reader = new System.IO.StreamReader(@"C:\Mitch\New folder\UsersHCISC\Users2.csv"))
            {
                while (!reader.EndOfStream)
                {
                    userList2.Add(reader.ReadLine());
                }
            }

            userList1.Sort();
            userList2.Sort();

            using (System.IO.StreamWriter writetext = new System.IO.StreamWriter(@"C:\Mitch\New folder\UsersHCISC\Users1AlphaOrder_" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".txt"))
            {
                foreach (string item in userList1)
                {
                    writetext.WriteLine(item);
                }

            }

            using (System.IO.StreamWriter writetext = new System.IO.StreamWriter(@"C:\Mitch\New folder\UsersHCISC\Users2AlphaOrder_" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".txt"))
            {
                foreach (string item in userList2)
                {
                    writetext.WriteLine(item);
                }

            }

            IEnumerable<string> inFirstOnly = userList1.Distinct().Except(userList2, StringComparer.OrdinalIgnoreCase).ToList();
            IEnumerable<string> inSecondOnly = userList2.Distinct().Except(userList1, StringComparer.OrdinalIgnoreCase).ToList();

            IEnumerable<string> alsoInFirst = userList1.Distinct().Intersect(userList2, StringComparer.OrdinalIgnoreCase).ToList();
            IEnumerable<string> alsoInSecond = userList2.Distinct().Intersect(userList1, StringComparer.OrdinalIgnoreCase).ToList();

            //Dupes?




            int totalCntUserList1 = userList1.Count();
            int totalCntUserList2 = userList2.Count();

            int totalList1WOutDupes = userList1.Distinct().Count();
            int totalList2WOutDupes = userList2.Distinct().Count();

            //int totalList1WOutDupes = userList1.Distinct().Count();
            //int totalList2WOutDupes = userList2.Distinct().Count();

            int usersThatAreInTheFirstListButNotInTheSecondList = inFirstOnly.Count();
            int usersThatAreInTheSecondListButNotInTheFirstList = inSecondOnly.Count();

            int usersThatAreBothInTheFirstAndSecondList = totalCntUserList2 - usersThatAreInTheSecondListButNotInTheFirstList;
            int usersThatAreBothInTheSecondAndFirstList = totalCntUserList1 - usersThatAreInTheFirstListButNotInTheSecondList;



            int s = 9;
        }

        public void SetCurrentUNCObject()
        {
            UNCObject xUNCObject = new UNCObject();
            xUNCObject.CharacterCount = MappedDriveResolver.ResolveToUNC(CurrentDirectory.FullName).Length;
            xUNCObject.NameObject = CurrentDirectory.Name;
            xUNCObject.Folder = true;  //always a directory at this point
            xUNCObject.NameUNC = MappedDriveResolver.ResolveToUNC(CurrentDirectory.FullName);
            xUNCObject.NameNetwork = CurrentDirectory.FullName;
            CurrentUNCObject = xUNCObject;
        }
        public bool GetDirectoryFiles()
        {
            FileCount = 0;
            bool success = true;

            App.Current.Dispatcher.BeginInvoke((Action)delegate ()
            {
                UNCObjectLst.Clear();
            });
            try
            {
                System.IO.EnumerationOptions eo = new System.IO.EnumerationOptions();
                System.IO.SearchOption so = new System.IO.SearchOption();

                //foreach (System.IO.FileInfo fi in CurrentDirectory.GetFiles("*.*", System.IO.SearchOption.AllDirectories))
                foreach (System.IO.FileInfo fi in CurrentDirectory.GetFiles())
                {
                    UNCObject currentUNCObject = new UNCObject();
                    currentUNCObject.NameObject = fi.Name;
                    currentUNCObject.NameNetwork = fi.DirectoryName;
                    Thread.Sleep(100);
                    App.Current.Dispatcher.BeginInvoke((Action)delegate ()
                    {
                        UNCObjectLst.Add(currentUNCObject);
                        FileCount++;
                    });
                }
            }
            catch (UnauthorizedAccessException uae)
            {
                StatusMessage = "Unauthorized Access";
                success = false;
            }
            catch (Exception ex)
            {
                success = false;
            }

            return success;
            //StatusMessage = "Ready";


            //Test Code
            //await Task.Run(() =>
            //{
            //    FileCount = 0;
            //    StatusMessage = "Loading...";

            //    string newFolderName = DateTime.Now.ToString("yyyyMMddHHmmssffff");

            //    System.IO.DirectoryInfo destinationDI = System.IO.Directory.CreateDirectory(@"C:\Mitch\" + newFolderName);
            //    System.IO.DirectoryInfo sourceDI = new System.IO.DirectoryInfo(@"C:\MitchDB");
            //    foreach (System.IO.FileInfo fi in sourceDI.GetFiles("*.*", System.IO.SearchOption.AllDirectories))
            //    {
            //        System.IO.File.Copy(fi.FullName, destinationDI.FullName + System.IO.Path.DirectorySeparatorChar + fi.Name, true);
            //        FileCount++;
            //    }

            //    StatusMessage = "Ready";
            //});

        }

        public bool GetDirectoryAtts()
        {
            bool success = true;



            return success;
        }

        private ICommand setUNCObjectCommand;
        public ICommand SetUNCObjectCommand
        {
            get
            {
                return setUNCObjectCommand ?? (setUNCObjectCommand = new CommandHandler(() => SetUNCObject(), _canExecute));
            }
        }
        private void SetUNCObject()
        {
            SetCurrentUNCObject();
        }

        private ICommand addSinglePathCommand;
        public ICommand AddSinglePathCommand
        {
            get
            {
                return addSinglePathCommand ?? (addSinglePathCommand = new CommandHandler(() => AddSinglePath(), _canExecute));
            }
        }
        private void AddSinglePath()
        {

        }
        #endregion

        #region "************************************************************************************************* OI Functions"

        private void SaveProperties()
        {
            StatusMessage = "Saving...";
            PathLenghtCrawl.Properties.Settings.Default.Save();
            StatusMessage = "Ready";
        }

        private ICommand setFileCommand;
        public ICommand SetFileCommand
        {
            get
            {
                return setFileCommand;
            }
            set
            {
                setFileCommand = value;
            }
        }
        public void SetFile(object obj)
        {
            Microsoft.Win32.OpenFileDialog openFD = new Microsoft.Win32.OpenFileDialog();

            switch (obj.ToString())
            {
                case "PathFileImportTxt":
                    openFD.Filter = "csv files (*.csv)|*.csv";
                    break;
                default:
                    break;
            }

            if (openFD.ShowDialog() == true)
            {
                string pathName = openFD.FileName;
                switch (obj.ToString())
                {
                    case "FilePathTxt":
                        FilePath = pathName;
                        break;
                    case "DatabaseValLocationPath":
                        DatabaseValLocationPath = pathName;
                        break;
                    case "PathFileImportTxt":
                        if (System.IO.Path.GetExtension(pathName) != ".csv")
                        {
                            StatusMessage = ".csv format required.";
                        }
                        else
                        {
                            PathFileImportTxt = pathName;
                        }
                        break;
                    case "MagikFileSource":
                        MagikFileSource = pathName;
                        break;
                    default:
                        StatusMessage = "Save Incomplete.";
                        break;
                }
            }
        }

        private ICommand setFolderViaTVCommand;
        public ICommand SetFolderViaTVCommand
        {
            get
            {
                return setFolderViaTVCommand;
            }
            set
            {
                setFolderViaTVCommand = value;
            }
        }
        public void SetFolderViaTV(object obj)
        {
            switch (obj.ToString())
            {
                case "LogLocationTxt":
                    LogLocationTxt = CurrentDirectory.FullName;
                    break;
                case "DatabaseValLocation":
                    DatabaseValLocation = CurrentDirectory.FullName;
                    break;
                default:
                    StatusMessage = "Save Incomplete";
                    break;
            }
        }


        private ICommand setFolderCommand;
        public ICommand SetFolderCommand
        {
            get
            {
                return setFolderCommand;
            }
            set
            {
                setFolderCommand = value;
            }
        }
        public void SetFolder(object obj)
        {

            //Microsoft.Win32.OpenFileDialog openFD = new Microsoft.Win32.OpenFileDialog();
            System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            // Microsoft.Win32.OpenFileDialog folderBrowserDialog = new Microsoft.Win32.OpenFileDialog();


            switch (obj.ToString())
            {
                case "LogLocationTxt":
                    if (LogLocationTxt != string.Empty)
                    {
                        folderBrowserDialog.SelectedPath = LogLocationTxt;
                    }
                    break;
                case "DatabaseValLocation":
                    if (DatabaseValLocation != string.Empty)
                    {
                        folderBrowserDialog.SelectedPath = DatabaseValLocation;
                    }
                    break;
                case "MagikFolderDestination":
                    if (MagikFolderDestination != string.Empty)
                    {
                        folderBrowserDialog.SelectedPath = MagikFolderDestination;
                    }
                    break;
                default:
                    break;
            }

            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //string pathName = openFD.FileName;
                string pathName = folderBrowserDialog.SelectedPath;
                switch (obj.ToString())
                {
                    case "LogLocationTxt":
                        LogLocationTxt = pathName;
                        break;
                    case "DatabaseValLocation":
                        DatabaseValLocation = pathName;
                        break;
                    case "MagikFolderDestination":
                        MagikFolderDestination = pathName;
                        break;
                    default:
                        StatusMessage = "Save Incomplete.";
                        break;
                }
            }
        }
        #endregion

        #region "************************************************************************************************* SQLite Functions"

        private ICommand inventDatabaseNameCommand;
        public ICommand InventDatabaseNameCommand
        {
            get
            {
                return inventDatabaseNameCommand ?? (inventDatabaseNameCommand = new CommandHandler(() => InventDatabaseName(), _canExecute));
            }
        }
        private void InventDatabaseName()
        {
            DatabaseValName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".db";
        }

        private ICommand createAuditDBCommand;
        public ICommand CreateAuditDBCommand
        {
            get
            {
                return createAuditDBCommand ?? (createAuditDBCommand = new CommandHandler(() => CreateAuditDB(), _canExecute));
            }
        }
        private void CreateAuditDB()
        {
            StatusMessage = "Creating Database...";
            if (System.IO.Directory.Exists(DatabaseValLocation))
            {
                if (DatabaseValName != String.Empty)
                {
                    if (!System.IO.File.Exists(System.IO.Path.Combine(DatabaseValLocation, "Audit" + DatabaseValName)))
                    {
                        //InventDatabaseName();
                        //CreateLocalDB();
                        if (CreateLocalDB() != -1)
                        {
                            StatusMessage = "Ready";
                        }
                        else
                        {
                            StatusMessage = "Database error.  Check Logs.";
                        }

                    }
                    else
                    {
                        StatusMessage = "Database already exists.  Cannot create.";
                    }
                }
                else
                {
                    StatusMessage = "Database name required.";
                }
            }
            else
            {
                StatusMessage = "Database location required.";
            }
        }
        private int CreateLocalDB()
        {
            int success = -1;
            try
            {
                string fullDatabaseValPath = System.IO.Path.Combine(DatabaseValLocation, "Audit" + DatabaseValName);

                string sqLiteConnection = @"Data Source=" + fullDatabaseValPath;

                using (var con = new Microsoft.Data.Sqlite.SqliteConnection())
                {
                    con.ConnectionString = sqLiteConnection;
                    con.Open();

                    using (var cmd = new Microsoft.Data.Sqlite.SqliteCommand())
                    {
                        cmd.Connection = con;

                        cmd.CommandText = "DROP TABLE IF EXISTS AUDIT";
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = @"CREATE TABLE AUDIT(Id INTEGER PRIMARY KEY,DateTime TEXT,DocNumber NUMERIC,GCDocsLocation TEXT,GCDocsTitle TEXT,RDIMSLocation TEXT,Generated NUMERIC,Migrated NUMERIC,Validated NUMERIC,GCDocsId NUMERICx)";
                        success = cmd.ExecuteNonQuery();
                    }
                }

                DatabaseValLocationPath = fullDatabaseValPath;
            }
            catch (Exception)
            {

                //throw;
            }
            finally
            {

            }
            return success;

        }

        private void UpdateRecord()//Poco.AuditDBValues auditDBValues)
        {
            string cs = @"Data Source=" + DatabaseValLocationPath;

            using (var con = new Microsoft.Data.Sqlite.SqliteConnection())
            {
                con.ConnectionString = cs;
                con.Open();
                try
                {
                    using (var cmd = new Microsoft.Data.Sqlite.SqliteCommand())
                    {
                        cmd.CommandText = "UPDATE Audit SET DateTime=@datetime,GCDocsLocation=@gcDocsLocation,GCDocsTitle=@gcDocsTitle,RDIMSLocation=@rdimsLocation,Migrated=@migrated,Generated = @generated,Validated=@validated,GCDocsId=@gcDocsId WHERE DocNumber = @docNumber";
                        cmd.Connection = con;

                        //cmd.Parameters.AddWithValue("@docNumber", auditDBValues.DocNumber);
                        //cmd.Parameters.AddWithValue("@datetime", auditDBValues.DateTime);
                        //cmd.Parameters.AddWithValue("@gcDocsLocation", auditDBValues.GCDocsLocation);
                        //cmd.Parameters.AddWithValue("@gcDocsTitle", auditDBValues.GCDocsTitle);
                        //cmd.Parameters.AddWithValue("@rdimsLocation", auditDBValues.RDIMSLocation);
                        //cmd.Parameters.AddWithValue("@generated", auditDBValues.Generated);
                        //cmd.Parameters.AddWithValue("@migrated", auditDBValues.Migrated);
                        //cmd.Parameters.AddWithValue("@validated", auditDBValues.Migrated);
                        //cmd.Parameters.AddWithValue("@gcDocsId", auditDBValues.GCDocsId);
                        cmd.Prepare();

                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    //Log.Log.WriteLog(LocalLogLocationPath, DateTime.Now.ToString("yyyyMMddHHmmssffff") + "Update Record Error");
                }
                finally
                {
                    //if(con.==Microsoft.Data.Sqlite.SqliteConnection
                }
            }
        }

        private void InsertRecord(long docNumber)
        {
            //Counts.Status = "Running.";
            string cs = @"Data Source=" + DatabaseValLocationPath;

            using (var con = new Microsoft.Data.Sqlite.SqliteConnection())
            {
                con.ConnectionString = cs;
                con.Open();
                try
                {
                    using (var cmd = new Microsoft.Data.Sqlite.SqliteCommand())
                    {
                        cmd.CommandText = "INSERT INTO AUDIT(DocNumber) VALUES(@docNumber)";
                        cmd.Connection = con;

                        cmd.Parameters.AddWithValue("@docNumber", docNumber);
                        cmd.Prepare();

                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    //Log.Log.WriteLog(LocalLogLocationPath, DateTime.Now.ToString("yyyyMMddHHmmssffff") + "Insert Record Error");
                }
                finally
                {
                    //if(con.==Microsoft.Data.Sqlite.SqliteConnection
                }
            }
        }
        private void InsertRecords(List<int> docNumbers)
        {
            string cs = @"Data Source=" + DatabaseValLocationPath;

            using (var con = new Microsoft.Data.Sqlite.SqliteConnection())
            {
                con.ConnectionString = cs;
                con.Open();
                try
                {
                    foreach (int docNumber in docNumbers)
                    {
                        using (var cmd = new Microsoft.Data.Sqlite.SqliteCommand())
                        {
                            cmd.CommandText = "INSERT INTO AUDIT(DocNumber) VALUES(@docNumber)";
                            cmd.Connection = con;

                            cmd.Parameters.AddWithValue("@docNumber", docNumber);
                            cmd.Prepare();

                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    //Log.Log.WriteLog(LocalLogLocationPath, DateTime.Now.ToString("yyyyMMddHHmmssffff") + "Insert Records Error");
                }
                finally
                {
                    //if(con.==Microsoft.Data.Sqlite.SqliteConnection
                }


            }


        }
        #endregion

        #region "************************************************************************************************* StopWatch"

        private double currentAverageRunningTimePer = 0;
        public double CurrentAverageRunningTimePer
        {
            get
            {
                return currentAverageRunningTimePer;
            }
            set
            {
                currentAverageRunningTimePer = value;
                AverageRunningTimePerLst.Add(value);
                NotifyPropertyChanged("CurrentAverageRunningTimePer");
            }
        }

        private List<double> averageRunningTimePerLst = new List<double>();
        public List<double> AverageRunningTimePerLst
        {
            get
            {
                return averageRunningTimePerLst;
            }
            set
            {
                averageRunningTimePerLst = value;
                //AverageRunningTimePer = averageRunningTimePerLst.Average();
                NotifyPropertyChanged("AverageRunningTimePerLst");
            }
        }

        private string averageRunningTimePer = "N/A";
        public string AverageRunningTimePer
        {
            get
            {
                return averageRunningTimePer;
            }
            set
            {
                averageRunningTimePer = value;
                NotifyPropertyChanged("AverageRunningTimePer");
            }
        }


        private string validatorRunningTimePer = "N/A";
        public string ValidatorRunningTimePer
        {
            get
            {
                return validatorRunningTimePer;
            }
            set
            {
                validatorRunningTimePer = value;
                NotifyPropertyChanged("ValidatorRunningTimePer");
            }
        }

        void dtRunningPer_Tick(object sender, EventArgs e)
        {
            TimeSpan ts = stopWatchRunningPer.Elapsed;
            currentRunningTimePer = String.Format("{0:00}:{1:00}:{2:00}:{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);
            ValidatorRunningTimePer = currentRunningTimePer;

        }

        void dtRunning_Tick(object sender, EventArgs e)
        {
            TimeSpan ts = stopWatchRunning.Elapsed;
            currentRunningTime = String.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);
            ValidatorRunningTime = currentRunningTime;
        }

        private string validatorRunningTime = "N/A";
        public string ValidatorRunningTime
        {
            get
            {
                return validatorRunningTime;
            }
            set
            {
                validatorRunningTime = value;
                NotifyPropertyChanged("ValidatorRunningTime");
            }
        }
        private ICommand startTimerPerCommand;
        public ICommand StartTimerPerCommand
        {
            get
            {
                return startTimerPerCommand ?? (startTimerPerCommand = new CommandHandler(() => StartTimerPer(), _canExecute));
            }
        }

        private ICommand resetTimerPerCommand;
        public ICommand ResetTimerPerCommand
        {
            get
            {
                return resetTimerPerCommand ?? (resetTimerPerCommand = new CommandHandler(() => ResetTimerPer(), _canExecute));
            }
        }
        private void ResetTimerPer()
        {
            stopWatchRunningPer.Reset();
        }
        private void StartTimerPer()
        {
            stopWatchRunningPer.Start();
        }

        private ICommand stopTimerPerCommand;
        public ICommand StopTimerPerCommand
        {
            get
            {
                return stopTimerPerCommand ?? (stopTimerPerCommand = new CommandHandler(() => StopTimerPer(), _canExecute));
            }
        }
        private void StopTimerPer()
        {
            stopWatchRunningPer.Stop();
            AverageRunningTimePerLst.Add(stopWatchRunningPer.Elapsed.TotalSeconds);
        }

        private ICommand startTimerCommand;
        public ICommand StartTimerCommand
        {
            get
            {
                return startTimerCommand ?? (startTimerCommand = new CommandHandler(() => StartTimer(), _canExecute));
            }
        }
        private void StartTimer()
        {
            stopWatchRunning.Start();
        }

        private ICommand stopTimerCommand;
        public ICommand StopTimerCommand
        {
            get
            {
                return stopTimerCommand ?? (stopTimerCommand = new CommandHandler(() => StopTimer(), _canExecute));
            }
        }
        private void StopTimer()
        {
            stopWatchRunning.Stop();
        }

        #endregion

        #region "************************************************************************************************* View Life Cycle"

        private bool _isLoaded = false;

        public void Initialize()
        {
            StatusMessage = "Initializing...";
            // TODO: Add your initialization code here 
            // This method is only called when the application is running
            StatusMessage = "Ready";
        }

        public void OnLoaded()
        {
            if (!_isLoaded)
            {
                StatusMessage = "Loading...";
                // TODO: Add your loaded code here 
                _isLoaded = true;
                StatusMessage = "Ready";
            }
        }

        public void OnUnloaded()
        {
            if (_isLoaded)
            {
                StatusMessage = "Unloading...";
                // TODO: Add your cleanup/unloaded code here 
                _isLoaded = false;
                StatusMessage = "Ready";
            }
        }

        #endregion

        private ICommand magikCopyCommand;
        public ICommand MagikCopyCommand
        {
            get
            {
                return magikCopyCommand ?? (magikCopyCommand = new CommandHandler(() => MagikCopy(), _canExecute));
            }
        }
        private void MagikCopy()
        {
            //string fileName = @"test.txt";
            //string magicSource = @"\\?\M:\Analytical Team\Summer 2007 onwards\check for archiving\Alan's Folders\Attachments\DAPS - Varia\Briefing for SJW\draft BRIEFING NOTE_DG - Lead study co-presentation by AT and OR at the FN Water Symposium March 18 and 19, 2008.wpd";
            //string source = @"M:\Analytical Team\Summer 2007 onwards\check for archiving\Alan's Folders\Attachments\DAPS - Varia\Briefing for SJW\draft BN TO DG_OR and AT co-presentation at FN Water Symposium March 19th.wpd";
            ////string source = @"M:\Analytical Team\Summer 2007 onwards\check for archiving\Alan's Folders\Attachments\DAPS - Varia\Briefing for SJW\Appendix A  - First Nations Water Symposium Poster.doc";
            //string xxx = MappedDriveResolver.ResolveToUNC(source);

            //\\NCR-A_FNIHBC3S\FNIHBC3\FNIHB-HQ-VOL1\SHARED\PHCPH\Environmental Research\Analytical Team\Summer 2007 onwards\check for archiving\Alan's Folders\Attachments\DAPS - Varia\Briefing for SJW\draft BN TO DG_OR and AT co-presentation at FN Water Symposium March 19th.wpd
            try
            {
                MagikFileSource = @"\?\\Health_tree\.HOGA_HOGUC1.HOGA.CLUSTERS.MB.hc-sc\common\fnih\admin\Business Office\Archived Folders\2019-2020\Accounts Payable\Business Office\Travel\Training Applications\01 IPAC - ON-LINE COURSES -  Papio, Elmer\Papio Elmer - TAA - IPAC DISTANCE - Canada Sponsored Online Novice IPAC Course - signed.pdf";

                //\\Health_tree\.HOGA_HOGUC1.HOGA.CLUSTERS.MB.hc-sc\common\fnih\admin\Business Office\Archived Folders\2019-2020\Accounts Payable\Business Office\Travel\Training Applications\01 IPAC - ON-LINE COURSES -  Papio, Elmer

                //\\Health_tree\.HOGA_HOGUC1.HOGA.CLUSTERS.MB.hc-sc\common\fnih\admin\Business Office\Archived Folders\2019-2020\Accounts Payable\Business Office\Travel\Training Applications\01 IPAC - ON-LINE COURSES -  Papio, Elmer
                System.IO.File.Copy(MagikFileSource, MagikFolderDestination + System.IO.Path.DirectorySeparatorChar + DateTime.Now.ToString("yyyyMMddHHmmssffff"));
                //System.IO.File.Copy(@"\\NCR-A_FNIHBC3S\FNIHBC3\FNIHB-HQ-VOL1\SHARED\PHCPH\Environmental Research\Analytical Team\Summer 2007 onwards\check for archiving\Alan's Folders\Attachments\DAPS - Varia\Briefing for SJW\draft BN TO DG_OR and AT co-presentation at FN Water Symposium March 19th.wpd", MagicFolderDestination + System.IO.Path.DirectorySeparatorChar + fileName, true);
                ////System.IO.File.Copy(@"\\NCR-A_FNIHBC3S\FNIHBC3\FNIHB-HQ-VOL1\SHARED\PHCPH\Environmental Research\Analytical Team\Summer 2007 onwards\check for archiving\Alan's Folders\Attachments\DAPS - Varia\Briefing for SJW\Appendix A  - First Nations Water Symposium Poster.doc", MagicFolderDestination + System.IO.Path.DirectorySeparatorChar + fileName, true);
                ////System.IO.File.Copy(xxx, MagicFolderDestination + System.IO.Path.DirectorySeparatorChar + fileName, true);
                ////System.IO.File.Copy(xxx, MagicFolderDestination + System.IO.Path.DirectorySeparatorChar + fileName, true);
            }
            catch (System.IO.IOException ioEx)
            {
                StatusMessage = ioEx.Message;
            }
            catch (System.UnauthorizedAccessException uaEx)
            {
                StatusMessage = uaEx.Message;
            }
            catch (System.Exception unknownEx)
            {
                StatusMessage = unknownEx.Message;
            }
            finally
            { }
        }
    }
}
