using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;


namespace WhateverExtensions
{
    public static class ExtensionsAreMe
    {
        public static List<int> AllIndexesOf(this string str, string value)
        {
            if (String.IsNullOrEmpty(value))
                throw new ArgumentException("the string to find may not be empty", "value");
            List<int> indexes = new List<int>();
            for (int index = 0; ; index += value.Length)
            {
                index = str.IndexOf(value, index);
                if (index == -1)
                    return indexes;
                indexes.Add(index);
            }
        }
    }
}
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
        //NDSSamePathAsAbove
        private bool ndsSamePathAsAbove = PathLenghtCrawl.Properties.Settings.Default.NDSSamePathAsAbove;



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
        private bool speed = PathLenghtCrawl.Properties.Settings.Default.Speed;
        public bool Speed
        {
            get
            {
                return speed;
            }
            set
            {
                if (speed != value)
                {
                    speed = value;
                    PathLenghtCrawl.Properties.Settings.Default.Speed = value;
                    SaveProperties();
                    NotifyPropertyChanged("Speed");
                }
            }
        }
        private bool skip = false;
        public bool Skip
        {
            get
            {
                return skip;
            }
            set
            {
                if (skip != value)
                {
                    skip = value;
                    NotifyPropertyChanged("Skip");
                }
            }
        }
        private bool pause = false;
        public bool Pause
        {
            get
            {
                return pause;
            }
            set
            {
                if (pause != value)
                {
                    pause = value;
                    NotifyPropertyChanged("Pause");
                }
            }
        }
        private bool quit = false;
        public bool Quit
        {
            get
            {
                return quit;
            }
            set
            {
                if (quit != value)
                {
                    quit = value;
                    NotifyPropertyChanged("Quit");
                }
            }
        }

        private bool details = PathLenghtCrawl.Properties.Settings.Default.Details;
        public bool Details
        {
            get
            {
                return details;
            }
            set
            {
                if (details != value)
                {
                    details = value;
                    PathLenghtCrawl.Properties.Settings.Default.Details = value;
                    SaveProperties();
                    NotifyPropertyChanged("Details");
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

        private int csvReadCount = 0;
        public int CSVReadCount
        {
            get
            {
                return csvReadCount;
            }
            set
            {
                if (csvReadCount != value)
                {
                    csvReadCount = value;
                    NotifyPropertyChanged("CSVReadCount");
                }
            }
        }

        private int csvWriteCount = 0;
        public int CSVWriteCount
        {
            get
            {
                return csvWriteCount;
            }
            set
            {
                if (csvWriteCount != value)
                {
                    csvWriteCount = value;
                    NotifyPropertyChanged("CSVWriteCount");
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


        private string dateGuid = string.Empty;// DateTime.Now.ToString("yyyyMMddHHmmssffff");
        public string DateGuid
        {
            get
            {
                return dateGuid;
            }
            set
            {
                if (dateGuid != value)
                {
                    dateGuid = value;
                    NotifyPropertyChanged("DateGuid");
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
        #region "NDS Processing"

        public bool NDSSamePathAsAbove
        {
            get
            {
                return ndsSamePathAsAbove;
            }
            set
            {
                if (ndsSamePathAsAbove != value)
                {
                    ndsSamePathAsAbove = value;
                    if (value)
                    {
                        NDSLogDestination = System.IO.Path.GetDirectoryName(NDSLog2Process);
                    }
                    else
                    {
                        NDSLogDestination = string.Empty;
                    }
                    PathLenghtCrawl.Properties.Settings.Default.NDSSamePathAsAbove = value;
                    SaveProperties();
                    NotifyPropertyChanged("NDSSamePathAsAbove");
                }
            }
        }

        private bool ndsLogFolderType = PathLenghtCrawl.Properties.Settings.Default.NDSLogFolderType;
        public bool NDSLogFolderType
        {
            get
            {
                return ndsLogFolderType;
            }
            set
            {
                if (ndsLogFolderType != value)
                {
                    ndsLogFolderType = value;
                    PathLenghtCrawl.Properties.Settings.Default.NDSLogFolderType = value;
                    SaveProperties();
                    NotifyPropertyChanged("NDSLogFolderType");
                }
            }
        }
        //NDSLogFileType
        private bool ndsLogFileType = PathLenghtCrawl.Properties.Settings.Default.NDSLogFileType;
        public bool NDSLogFileType
        {
            get
            {
                return ndsLogFileType;
            }
            set
            {
                if (ndsLogFileType != value)
                {
                    ndsLogFileType = value;
                    PathLenghtCrawl.Properties.Settings.Default.NDSLogFileType = value;
                    SaveProperties();
                    NotifyPropertyChanged("NDSLogFileType");
                }
            }
        }
        private string ndsLogDestination = PathLenghtCrawl.Properties.Settings.Default.NDSLogDestination;
        public string NDSLogDestination
        {
            get
            {
                return ndsLogDestination;
            }
            set
            {
                if (ndsLogDestination != value)
                {
                    ndsLogDestination = value;
                    PathLenghtCrawl.Properties.Settings.Default.NDSLogDestination = value;
                    SaveProperties();
                    NotifyPropertyChanged("NDSLogDestination");
                }
            }
        }
        private string ndsLog2Process = PathLenghtCrawl.Properties.Settings.Default.NDSLog2Process;
        public string NDSLog2Process
        {
            get
            {
                return ndsLog2Process;
            }
            set
            {
                if (ndsLog2Process != value)
                {
                    ndsLog2Process = value;
                    if (NDSSamePathAsAbove)
                    {
                        NDSLogDestination = System.IO.Path.GetDirectoryName(value);
                    }
                    PathLenghtCrawl.Properties.Settings.Default.NDSLog2Process = value;
                    SaveProperties();
                    NotifyPropertyChanged("NDSLog2Process");
                }
            }
        }

        #endregion
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

        //FileToStarTxt

        private string fileToStarTxt = PathLenghtCrawl.Properties.Settings.Default.FileToStarTxt;
        public string FileToStarTxt
        {
            get
            {
                return fileToStarTxt;
            }
            set
            {
                if (fileToStarTxt != value)
                {
                    fileToStarTxt = value;
                    PathLenghtCrawl.Properties.Settings.Default.FileToStarTxt = value;
                    SaveProperties();
                    NotifyPropertyChanged("FileToStarTxt");
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

        private string pathFolder2Evalulate = PathLenghtCrawl.Properties.Settings.Default.PathFolder2Evalulate;
        public string PathFolder2Evalulate
        {
            get
            {
                return pathFolder2Evalulate;
            }
            set
            {
                if (pathFolder2Evalulate != value)
                {
                    pathFolder2Evalulate = value;
                    PathLenghtCrawl.Properties.Settings.Default.PathFolder2Evalulate = value;
                    SaveProperties();
                    NotifyPropertyChanged("PathFolder2Evalulate");
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

        #region "LPFN Process"

        //private ICommand importPathFileCommand;
        //public ICommand ImportPathFileCommand
        //{
        //    get
        //    {
        //        return importPathFileCommand ?? (importPathFileCommand = new CommandHandler(() => ImportPathFile(), _canExecute));
        //    }
        //}
        //private async void ImportPathFile()
        //{
        //    SelectBulkFile2ProcessButtonEnabled = false;
        //    ImportProcessBulkFileButtonEnabled = false;
        //    StatusMessage = "Ready";

        //    PathImportedCount = 0;
        //    PathImportedCountTotal = 0;
        //    PathProcessedCount = 0;
        //    PathProcessedCountTotal = 0;
        //    PathCurrentlyProcessing = "N/A";

        //    ObjectCount = 0;
        //    FolderCount = 0;
        //    FileCount = 0;
        //    ObjectCountTotal = 0;
        //    FolderCountTotal = 0;
        //    FileCountTotal = 0;
        //    WarningCount = 0;
        //    ErrorCount = 0;

        //    List<string> uncPathsToScan = new List<string>();

        //    System.IO.FileInfo fi = new System.IO.FileInfo(PathFileImportTxt);
        //    PathImportedCountTotal = System.IO.File.ReadLines(fi.FullName).Count();


        //    using (System.IO.StreamReader reader = new System.IO.StreamReader(PathFileImportTxt))
        //    {
        //        while (!reader.EndOfStream)
        //        {
        //            var CSValues = reader.ReadLine().Split(',');
        //            string stringToDitch = "NDS://HEALTH_TREE";

        //            string realValue = CSValues.FirstOrDefault().Replace(stringToDitch, "");

        //            uncPathsToScan.Add(realValue);
        //            PathImportedCount++;
        //        }
        //    }

        //    PathProcessedCountTotal = uncPathsToScan.Count();
        //    //UNCObjectFileLst.Clear();
        //    //UNCObjectFolderLst.Clear();
        //    UNCObjectFileList.Clear();

        //    string DateGuid = DateTime.Now.ToString("yyyyMMddHHmmssffff");
        //    //List<PathLenghtCrawl.POCO.Duration> durations = new List<PathLenghtCrawl.POCO.Duration>();
        //    DurationList.Clear();


        //    await Task.Run(() =>
        //    {
        //        foreach (String path in uncPathsToScan)
        //        {
        //            UNCObjectFileList.Clear();
        //            UNCObjectFolderList.Clear();

        //            ObjectCountTotal = 0;
        //            FolderCountTotal = 0;
        //            FileCountTotal = 0;

        //            App.Current.Dispatcher.BeginInvoke((Action)delegate ()
        //            {
        //                UNCObjectFileLst.Clear();
        //                UNCObjectFolderLst.Clear();
        //            });

        //            dtRunningPer.Start();
        //            ResetTimerPer();
        //            StartTimerPer();

        //            try
        //            {
        //                CurrentDirectory = new System.IO.DirectoryInfo(path);
        //                PathCurrentlyProcessing = path;
        //                //Thread.Sleep(100);

        //                bool success = ExecuteLPFNBulkList_Linq(DateGuid);
        //                PathProcessedCount++;

        //                StopTimerPer();
        //                dtRunningPer.Stop();
        //                AverageRunningTimePer = String.Format("{0:0.00}", Math.Round(AverageRunningTimePerLst.Average(), 2));
        //                try
        //                {
        //                    //AverageRunningTimePer = String.Format("{0:00}:{1:00}:{2:00}:{3:00}", Math.Round(AverageRunningTimePerLst.Average(), 2));
        //                }
        //                catch (Exception ex)
        //                {
        //                    PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "AvrRunTImePer Error: line 875");
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "Line 855");
        //            }
        //        }
        //    });

        //    ObjectCountTotal = 0;
        //    FolderCountTotal = 0;
        //    FileCountTotal = 0;

        //    using (System.IO.StreamWriter resultsFile = new System.IO.StreamWriter(LogLocationTxt + System.IO.Path.DirectorySeparatorChar + "Statistics_" + DateGuid + ".csv", true, Encoding.Unicode))
        //    {
        //        foreach (PathLenghtCrawl.POCO.Duration duration in DurationList)
        //        {
        //            resultsFile.WriteLine(duration.Name + "," + duration.Time);
        //        }
        //    }

        //    PathCurrentlyProcessing = "N/A";
        //    SelectBulkFile2ProcessButtonEnabled = true;
        //    ImportProcessBulkFileButtonEnabled = true;

        //}


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



        private ICommand quitLoopCommand;
        public ICommand QuitLoopCommand
        {
            get
            {
                return quitLoopCommand ?? (quitLoopCommand = new CommandHandler(() => QuitLoop(), _canExecute));
            }
        }
        private void QuitLoop()
        {
            Quit = true;
        }


        private ICommand skipPathCommand;
        public ICommand SkipPathCommand
        {
            get
            {
                return skipPathCommand ?? (skipPathCommand = new CommandHandler(() => SkipPath(), _canExecute));
            }
        }
        private void SkipPath()
        {
            Skip = true;
        }

        private void LoopFiles(string currentDI)
        {
            try
            {
                int x = System.IO.Directory.EnumerateFiles(currentDI).Count();
                //if (System.IO.Directory.EnumerateFiles(currentDI).Count)
                IEnumerable<string> fiList = System.IO.Directory.EnumerateFiles(currentDI).Where(x => x.Length > MinPathLength);
                int lkj = fiList.Count();

                foreach (string fi in fiList)
                {
                    try
                    {
                        if (Quit) { return; }
                        if (Skip)
                        {
                            break;
                        }
                        else
                        {
                            if (Details)
                            {
                                ObjectCount++;
                                FileCount++;
                                ObjectCountTotal++;
                                FileCountTotal++;

                                UNCObject uncObject = new UNCObject() { Count = FileCount, CharacterCount = fi.Length, NameUNC = fi };
                                UNCObjectFileList.Add(uncObject);
                                App.Current.Dispatcher.BeginInvoke((Action)delegate ()
                                {
                                    UNCObjectFileLst.Add(uncObject);
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorCount++;
                        StatusMessage = ex.Message;
                        PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "Exception: LoopFiles().ForEach", DateGuid);
                    }
                }
            }
            #region "Catches"
            catch (ArgumentNullException ex)
            {
                ErrorCount++;
                StatusMessage = ex.Message;
                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "ArgumentNullException: LoopFiles()", DateGuid);
            }
            catch (System.IO.FileNotFoundException ex)
            {
                ErrorCount++;
                StatusMessage = ex.Message;
                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "FileNotFoundException: LoopFiles()", DateGuid);
            }
            catch (System.IO.PathTooLongException ex)
            {
                ErrorCount++;
                StatusMessage = ex.Message;
                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "PathTooLongException: LoopFiles()", DateGuid);
            }
            catch (System.IO.IOException ex)
            {
                ErrorCount++;
                StatusMessage = ex.Message;
                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "IOException: LoopFiles()", DateGuid);
            }
            catch (System.Security.SecurityException ex)
            {
                ErrorCount++;
                StatusMessage = ex.Message;
                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "SecurityException: LoopFiles()", DateGuid);
            }
            catch (UnauthorizedAccessException ex)
            {
                ErrorCount++;
                StatusMessage = ex.Message;
                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "UnauthorizedAccessException: LoopFiles()", DateGuid);
            }
            catch (ArgumentException ex)
            {
                ErrorCount++;
                StatusMessage = ex.Message;
                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "ArgumentException: LoopFiles()", DateGuid);
            }
            catch (Exception ex)
            {
                ErrorCount++;
                StatusMessage = ex.Message;
                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "Generic Exception: LoopFiles()", DateGuid);
            }
            #endregion


        }
        private void LoopDirectories(string currentDI)
        {
            try
            {
                System.IO.Directory.CreateDirectory(LogLocationTxt + System.IO.Path.DirectorySeparatorChar + "Details");
                //bool success = true;

                if (Details)
                {
                    PathCurrentlyCounting = currentDI;
                }
                if (currentDI.Length > MinPathLength)
                {
                    if (IncludeFolders)
                    {
                        if (Details)
                        {
                            ///LOG LP FOLDER NAME
                            ObjectCount++;
                            FolderCount++;
                            ObjectCountTotal++;
                            FolderCountTotal++;

                            UNCObject uncObject = new UNCObject() { Count = FolderCount, CharacterCount = currentDI.Length, NameUNC = currentDI };
                            UNCObjectFolderList.Add(uncObject);
                            App.Current.Dispatcher.BeginInvoke((Action)delegate ()
                            {
                                UNCObjectFolderLst.Add(uncObject);
                            });
                        }
                    }
                }
                else
                {
                    if (IncludeDocuments)
                    {
                        LoopFiles(currentDI);
                    }

                    IEnumerable<string> diList = System.IO.Directory.EnumerateDirectories(currentDI);
                    if (diList.Count() > 0)
                    {
                        foreach (string di in diList)
                        {
                            try
                            {
                                if (Quit) { return; }
                                if (Skip) { break; }
                                else
                                {
                                    LoopDirectories(di);
                                }
                            }
                            catch (Exception ex)
                            {
                                ErrorCount++;
                                StatusMessage = ex.Message;
                                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "Exception: LoopDirectories().ForEach", DateGuid);
                            }
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
                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "ArgumentNullException: LoopDirectories()", DateGuid);
            }
            catch (System.IO.DirectoryNotFoundException ex)
            {
                ErrorCount++;
                StatusMessage = ex.Message;
                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "DirectoryNotFoundException: LoopDirectories()", DateGuid);
            }
            catch (System.IO.PathTooLongException ex)
            {
                ErrorCount++;
                StatusMessage = ex.Message;
                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "PathTooLongException: LoopDirectories()", DateGuid);
            }
            catch (System.IO.IOException ex)
            {
                ErrorCount++;
                StatusMessage = ex.Message;
                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "IOException: LoopDirectories()", DateGuid);
            }
            catch (System.Security.SecurityException ex)
            {
                ErrorCount++;
                StatusMessage = ex.Message;
                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "SecurityException: LoopDirectories()", DateGuid);
            }
            catch (UnauthorizedAccessException ex)
            {
                ErrorCount++;
                StatusMessage = ex.Message;
                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "UnauthorizedAccessException: LoopDirectories()", DateGuid);
            }
            catch (ArgumentException ex)
            {
                ErrorCount++;
                StatusMessage = ex.Message;
                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "ArgumentException: LoopDirectories()", DateGuid);
            }
            catch (Exception ex)
            {
                ErrorCount++;
                StatusMessage = ex.Message;
                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "Generic Exception: LoopDirectories()", DateGuid);
            }
            #endregion

        }
        private void EvaluatePath(string path)
        {
            try
            {
                if (Quit) { return; }
                Skip = false;
                if (Details)
                {
                    UNCObjectFileList.Clear();
                    UNCObjectFolderList.Clear();

                    App.Current.Dispatcher.BeginInvoke((Action)delegate ()
                    {
                        UNCObjectFileLst.Clear();
                        UNCObjectFolderLst.Clear();
                    });

                    ObjectCountTotal = 0;
                    FolderCountTotal = 0;
                    FileCountTotal = 0;

                    dtRunningPer.Start();
                    ResetTimerPer();
                    StartTimerPer();

                    CurrentDirectory = new System.IO.DirectoryInfo(path);
                    PathCurrentlyProcessing = path;
                }
                if (CurrentDirectory.Exists)
                {
                    LoopDirectories(path);

                    if (Details)
                    {
                        StopTimerPer();
                        dtRunningPer.Stop();

                        PathProcessedCount++;
                    }
                    PathLenghtCrawl.POCO.Duration currentDuration = new PathLenghtCrawl.POCO.Duration() { Time = ValidatorRunningTimePer, Name = CurrentDirectory.FullName };
                    DurationList.Add(currentDuration);

                    if (Details)
                    {
                        string fileNamePath = CurrentDirectory.FullName.Substring(2).Replace("\\", "_");

                        string logFileName = "Files_" + fileNamePath + "_" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + "_" + "lpfn.txt";
                        using (System.IO.StreamWriter resultsFileByPathType = new System.IO.StreamWriter(LogLocationTxt + System.IO.Path.DirectorySeparatorChar + "Details" + System.IO.Path.DirectorySeparatorChar + logFileName, false, Encoding.Unicode))
                        {
                            foreach (UNCObject uncFileObject in UNCObjectFileList)
                            {
                                resultsFileByPathType.WriteLine(uncFileObject.Count + "," + uncFileObject.CharacterCount + "," + uncFileObject.NameUNC);
                            }
                        }

                        string logFolderName = "Folders" + fileNamePath + "_" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + "_" + "lpfn.txt";
                        using (System.IO.StreamWriter resultsFolderByPathType = new System.IO.StreamWriter(LogLocationTxt + System.IO.Path.DirectorySeparatorChar + "Details" + System.IO.Path.DirectorySeparatorChar + logFolderName, false, Encoding.Unicode))
                        {
                            foreach (UNCObject uncFolderObject in UNCObjectFolderList)
                            {
                                resultsFolderByPathType.WriteLine(uncFolderObject.Count + "," + uncFolderObject.CharacterCount + "," + uncFolderObject.NameUNC);
                            }
                        }
                    }

                    string masterFileNameWOXtension = System.IO.Path.GetFileNameWithoutExtension(PathFileImportTxt);
                    string masterFileLogName = masterFileNameWOXtension + "_" + DateGuid + "_" + "lpfn.txt";
                    using (System.IO.StreamWriter resultsFileGlobal = new System.IO.StreamWriter(LogLocationTxt + System.IO.Path.DirectorySeparatorChar + masterFileLogName, true, Encoding.Unicode))
                    {
                        foreach (UNCObject uncFileObject in UNCObjectFileList)
                        {
                            resultsFileGlobal.WriteLine(uncFileObject.Count + "," + uncFileObject.CharacterCount + "," + uncFileObject.NameUNC);
                        }
                        foreach (UNCObject uncFolderObject in UNCObjectFolderList)
                        {
                            resultsFileGlobal.WriteLine(uncFolderObject.Count + "," + uncFolderObject.CharacterCount + "," + uncFolderObject.NameUNC);
                        }
                    }
                }
                else
                {
                    ErrorCount++;
                    StatusMessage = "Directory(Path) does not exist";
                    PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, path + " does not exist", "EvaluatePath()", DateGuid);
                }
            }
            catch (Exception ex)
            {
                ErrorCount++;
                StatusMessage = ex.Message;
                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "Exception: EvaluatePath()", DateGuid);
            }
        }


        private ICommand clearStatisticsCommand;
        public ICommand ClearStatisticsCommand
        {
            get
            {
                return clearStatisticsCommand ?? (clearStatisticsCommand = new CommandHandler(() => ClearStatistics(), _canExecute));
            }
        }
        private void ClearStatistics()
        {
            UNCObjectFileLst.Clear();
            UNCObjectFolderLst.Clear();
            ObjectCount = 0;
            FolderCount = 0;
            FileCount = 0;
            ObjectCountTotal = 0;
            FolderCountTotal = 0;
            FileCountTotal = 0;

            ErrorCount = 0;
            WarningCount = 0;
            PathProcessedCount = 0;
            PathProcessedCountTotal = 0;
            PathImportedCount = 0;
            PathImportedCountTotal = 0;
            PathCurrentlyCounting = "N/A";
            PathCurrentlyProcessing = "N/A";
            ResetTimerPer();
            SelectBulkFile2ProcessButtonEnabled = true;
            ImportProcessBulkFileButtonEnabled = true;
        }
        private ICommand fetchLPFNsInFolderCommand;
        public ICommand FetchLPFNsInFolderCommand
        {
            get
            {
                return fetchLPFNsInFolderCommand ?? (fetchLPFNsInFolderCommand = new CommandHandler(() => FetchLPFNsInFolder(), _canExecute));
            }
        }
        private async void FetchLPFNsInFolder()
        {
            string DateGuid = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            try
            {
                if (System.IO.File.Exists(PathFileImportTxt))
                {
                    StatusMessage = "Processing...";

                    ObjectCount = 0;
                    FolderCount = 0;
                    FileCount = 0;
                    ObjectCountTotal = 0;
                    FolderCountTotal = 0;
                    FileCountTotal = 0;

                    ErrorCount = 0;
                    WarningCount = 0;
                    PathProcessedCount = 0;
                    PathImportedCount = 0;
                    PathCurrentlyCounting = "N/A";
                    PathCurrentlyProcessing = "N/A";

                    SelectBulkFile2ProcessButtonEnabled = false;
                    ImportProcessBulkFileButtonEnabled = false;

                    if (Details)
                    {
                        PathProcessedCountTotal = 1;

                        dtRunningPer.Tick += new EventHandler(dtRunningPer_Tick);
                        dtRunningPer.Interval = new TimeSpan(0, 0, 0, 0, 1);
                    }
                    else
                    { ExpandOptionsStatsBool = false; }
                    DurationList.Clear();
                    await Task.Run(() =>
                    {
                        EvaluatePath(PathFolder2Evalulate);
                    });
                    //UNCObjectFileLst.Clear();
                    //UNCObjectFolderLst.Clear();
                    //});

                    //ObjectCountTotal = 0;
                    //FolderCountTotal = 0;
                    //FileCountTotal = 0;

                    Quit = false;
                    if (Details)
                    {
                        using (System.IO.StreamWriter resultsFile = new System.IO.StreamWriter(LogLocationTxt + System.IO.Path.DirectorySeparatorChar + "Statistics_" + DateGuid + ".txt", true, Encoding.Unicode))
                        {
                            foreach (PathLenghtCrawl.POCO.Duration duration in DurationList)
                            {
                                resultsFile.WriteLine(duration.Name + "," + duration.Time);
                            }
                        }
                        //PathCurrentlyProcessing = "N/A";
                        //PathCurrentlyCounting = "N/A";
                        //ResetTimerPer();
                    }
                    SelectBulkFile2ProcessButtonEnabled = true;
                    ImportProcessBulkFileButtonEnabled = true;
                    
                    StatusMessage = "Completed";
                    if (ErrorCount > 0) 
                    {
                        StatusMessage += StatusMessage + " with errors.  See Logs.";
                    }
                }
            }
            #region "Catches"
            catch (ArgumentNullException ex)
            {
                ErrorCount++;
                StatusMessage = ex.Message;
                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "ArgumentNullException: FetchLPFNsInFolder()", DateGuid);
            }
            catch (System.IO.DirectoryNotFoundException ex)
            {
                ErrorCount++;
                StatusMessage = ex.Message;
                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "DirectoryNotFoundException: FetchLPFNsInFolder()", DateGuid);
            }
            catch (System.IO.PathTooLongException ex)
            {
                ErrorCount++;
                StatusMessage = ex.Message;
                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "PathTooLongException: FetchLPFNsInFolder()", DateGuid);
            }
            catch (System.IO.IOException ex)
            {
                ErrorCount++;
                StatusMessage = ex.Message;
                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "IOException: FetchLPFNsInFolder()", DateGuid);
            }
            catch (System.Security.SecurityException ex)
            {
                ErrorCount++;
                StatusMessage = ex.Message;
                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "SecurityException: FetchLPFNsInFolder()", DateGuid);
            }
            catch (UnauthorizedAccessException ex)
            {
                ErrorCount++;
                StatusMessage = ex.Message;
                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "UnauthorizedAccessException: FetchLPFNsInFolder()", DateGuid);
            }
            catch (ArgumentException ex)
            {
                ErrorCount++;
                StatusMessage = ex.Message;
                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "ArgumentException: FetchLPFNsInFolder()", DateGuid);
            }
            catch (Exception ex)
            {
                ErrorCount++;
                StatusMessage = ex.Message;
                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "Generic Exception: FetchLPFNsInFolder()", DateGuid);
            }
            #endregion
        }


        private ICommand fetchLPFNsFromFileCommand;
        public ICommand FetchLPFNsFromFileCommand
        {
            get
            {
                return fetchLPFNsFromFileCommand ?? (fetchLPFNsFromFileCommand = new CommandHandler(() => FetchLPFNsFromFile(), _canExecute));
            }
        }
        private async void FetchLPFNsFromFile()
        {
            string DateGuid = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            try
            {
                if (System.IO.File.Exists(PathFileImportTxt))
                {
                    StatusMessage = "Processing...";

                    ObjectCount = 0;
                    FolderCount = 0;
                    FileCount = 0;
                    ObjectCountTotal = 0;
                    FolderCountTotal = 0;
                    FileCountTotal = 0;

                    ErrorCount = 0;
                    WarningCount = 0;
                    PathProcessedCount = 0;
                    PathImportedCount = 0;
                    PathCurrentlyCounting = "N/A";
                    PathCurrentlyProcessing = "N/A";

                    SelectBulkFile2ProcessButtonEnabled = false;
                    ImportProcessBulkFileButtonEnabled = false;

                    List<string> uncPathsToScan = new List<string>();
                    //string currentDI = CurrentDirectory.FullName;

                    System.IO.FileInfo fi = new System.IO.FileInfo(PathFileImportTxt);
                    if (Details)
                    {
                        //ExpandOptionsStatsBool = true;
                        PathImportedCountTotal = System.IO.File.ReadLines(fi.FullName).Count();

                        dtRunningPer.Tick += new EventHandler(dtRunningPer_Tick);
                        dtRunningPer.Interval = new TimeSpan(0, 0, 0, 0, 1);
                    }
                    else
                    { ExpandOptionsStatsBool = false; }
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(PathFileImportTxt))
                    {
                        while (!reader.EndOfStream)
                        {
                            var CSValues = reader.ReadLine().Split(',');
                            string stringToDitch = "NDS://HEALTH_TREE";

                            string realValue = CSValues.FirstOrDefault().Replace(stringToDitch, "");

                            uncPathsToScan.Add(realValue);
                            if (Details)
                            {
                                PathImportedCount++;
                            }
                        }
                    }
                    if (Details)
                    {
                        PathProcessedCountTotal = uncPathsToScan.Count();
                    }

                    DurationList.Clear();
                    await Task.Run(() =>
                    {
                        foreach (String path in uncPathsToScan)
                        {
                            EvaluatePath(path);
                        }
                    });

                    //App.Current.Dispatcher.BeginInvoke((Action)delegate ()
                    //{
                    UNCObjectFileLst.Clear();
                    UNCObjectFolderLst.Clear();
                    //});

                    ObjectCountTotal = 0;
                    FolderCountTotal = 0;
                    FileCountTotal = 0;

                    Quit = false;
                    if (Details)
                    {
                        using (System.IO.StreamWriter resultsFile = new System.IO.StreamWriter(LogLocationTxt + System.IO.Path.DirectorySeparatorChar + "Statistics_" + DateGuid + ".txt", true, Encoding.Unicode))
                        {
                            foreach (PathLenghtCrawl.POCO.Duration duration in DurationList)
                            {
                                resultsFile.WriteLine(duration.Name + "," + duration.Time);
                            }
                        }
                        PathCurrentlyProcessing = "N/A";
                        PathCurrentlyCounting = "N/A";
                        ResetTimerPer();
                    }
                    SelectBulkFile2ProcessButtonEnabled = true;
                    ImportProcessBulkFileButtonEnabled = true;
                    StatusMessage = "Completed";
                    if (ErrorCount > 0)
                    {
                        StatusMessage += StatusMessage + " with errors.  See Logs.";
                    }
                }
                else
                {
                    StatusMessage = "File does not exists.";
                }
            }
            #region "Catches"
            catch (ArgumentNullException ex)
            {
                ErrorCount++;
                StatusMessage = ex.Message;
                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "ArgumentNullException: FetchLPFNsFromFile()", DateGuid);
            }
            catch (System.IO.DirectoryNotFoundException ex)
            {
                ErrorCount++;
                StatusMessage = ex.Message;
                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "DirectoryNotFoundException: FetchLPFNsFromFile()", DateGuid);
            }
            catch (System.IO.PathTooLongException ex)
            {
                ErrorCount++;
                StatusMessage = ex.Message;
                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "PathTooLongException: FetchLPFNsFromFile()", DateGuid);
            }
            catch (System.IO.IOException ex)
            {
                ErrorCount++;
                StatusMessage = ex.Message;
                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "IOException: FetchLPFNsFromFile()", DateGuid);
            }
            catch (System.Security.SecurityException ex)
            {
                ErrorCount++;
                StatusMessage = ex.Message;
                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "SecurityException: FetchLPFNsFromFile()", DateGuid);
            }
            catch (UnauthorizedAccessException ex)
            {
                ErrorCount++;
                StatusMessage = ex.Message;
                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "UnauthorizedAccessException: FetchLPFNsFromFile()", DateGuid);
            }
            catch (ArgumentException ex)
            {
                ErrorCount++;
                StatusMessage = ex.Message;
                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "ArgumentException: FetchLPFNsFromFile()", DateGuid);
            }
            catch (Exception ex)
            {
                ErrorCount++;
                StatusMessage = ex.Message;
                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "Generic Exception: FetchLPFNsFromFile()", DateGuid);
            }
            #endregion
        }





        //private void WhateverCheckFiles(string currentDI)
        //{
        //    try
        //    {
        //        IEnumerable<string> fiList = System.IO.Directory.EnumerateFiles(currentDI);
        //        int lkj = fiList.Where(x => x.Length > MinPathLength).Count();

        //        if (lkj > 0)
        //        {
        //            //some, maybe all, files in this folder are LPFN
        //            //LOG IT
        //            //check parent
        //            ObjectCountTotal += lkj;
        //            FileCountTotal += lkj;

        //            string fn = System.IO.Directory.GetParent(currentDI).FullName;
        //            WhateverCheckFiles(fn);
        //        }
        //        else
        //        {
        //            //exit this branch, NO LPFN
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string lsdkjf = ex.Message;
        //        throw;
        //    }


        //    //IEnumerable<System.IO.FileInfo> fiList = currentDI.EnumerateFiles();
        //    //int lkj = fiList.Where(x => x.FullName.Length > 255).Count();

        //    //if (lkj > 0)
        //    //{
        //    //    //some, maybe all, files in this folder are LPFN
        //    //    //LOG IT
        //    //    //check parent
        //    //    WhateverCheckFiles(currentDI.Parent);
        //    //}
        //    //else
        //    //{
        //    //    //exit this branch, NO LPFN
        //    //}

        //}



        //private ICommand whateverStartCommand;
        //public ICommand WhateverStartCommand
        //{
        //    get
        //    {
        //        return whateverStartCommand ?? (whateverStartCommand = new CommandHandler(() => WhateverStart(), _canExecute));
        //    }
        //}
        //private async void WhateverStart()
        //{
        //    StatusMessage = "Processing...";
        //    ObjectCountTotal = 0;
        //    FolderCountTotal = 0;
        //    FileCountTotal = 0;

        //    string currentDI = CurrentDirectory.FullName;
        //    await Task.Run(() =>
        //    {
        //        //Whatever(currentDI);

        //        //System.IO.DirectoryInfo did = new System.IO.DirectoryInfo(@"\\HOGA_HOGUC1S\HOGUC1\Users\JHamrlik");
        //        System.IO.DirectoryInfo did = new System.IO.DirectoryInfo(@"C:\Mitch");


        //        Whatever(@"\\HOGA_HOGUC1S\HOGUC1\Users\JHamrlik");
        //        //Whatever(did.FullName);


        //        int lkj = 9;
        //    });
        //    StatusMessage = "Completed";
        //}

        //private void Whatever(string currentDI)
        //{
        //    try
        //    {
        //        IEnumerable<string> diList = System.IO.Directory.EnumerateDirectories(currentDI);
        //        if (diList.Count() > 0)
        //        {
        //            foreach (string di in diList)
        //            {
        //                if (di.Length > MinPathLength)
        //                {
        //                    //LP FolderNAMe
        //                    //LOG IT
        //                    ObjectCountTotal++;
        //                    FolderCountTotal++;
        //                }
        //                else
        //                {
        //                    Whatever(di);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            WhateverCheckFiles(currentDI);

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        string lsdkjf = ex.Message;
        //        throw;
        //    }





        //    //if (currentDI.FullName.Length > 255)
        //    //{
        //    //    //LONG PATH FOLDER NAME, LOG IT
        //    //}
        //    //else
        //    //{
        //    //    IEnumerable<System.IO.DirectoryInfo> diList = currentDI.EnumerateDirectories();
        //    //    if (diList.Count() > 0)
        //    //    {
        //    //        foreach (System.IO.DirectoryInfo di in diList)
        //    //        {
        //    //            Whatever(di);
        //    //        }
        //    //    }
        //    //    else
        //    //    {
        //    //        WhateverCheckFiles(currentDI);
        //    //    }
        //    //}




        //    //if (currentDI.FullName.Length > 255)
        //    //{
        //    //    //add everything here as LPFN
        //    //}
        //    //else
        //    //{
        //    //    IEnumerable<System.IO.FileInfo> fiList = currentDI.EnumerateFiles();
        //    //    int lkj = fiList.Where(x => x.FullName.Length > 255).Count();

        //    //    if (lkj > 0)
        //    //    {
        //    //        //some, maybe all, files in this folder are LPFN
        //    //    }
        //    //    else
        //    //    {
        //    //        IEnumerable<System.IO.DirectoryInfo> diList = currentDI.EnumerateDirectories();
        //    //        if (diList.Count() > 0)
        //    //        {
        //    //            foreach (System.IO.DirectoryInfo di in diList)
        //    //            {
        //    //                Whatever(di);
        //    //            }
        //    //        }
        //    //    }
        //    //}



        //}


        //private ICommand replaceFirstCommaWithStarCSVWAYCommand;
        //public ICommand ReplaceFirstCommaWithStarCSVWAYCommand
        //{
        //    get
        //    {
        //        return replaceFirstCommaWithStarCSVWAYCommand ?? (replaceFirstCommaWithStarCSVWAYCommand = new CommandHandler(() => ReplaceFirstCommaWithStarCSVWAY(), _canExecute));
        //    }
        //}
        //private async void ReplaceFirstCommaWithStarCSVWAY()
        //{
        //    await Task.Run(() =>
        //    {
        //        StatusMessage = "Ready";
        //        CSVReadCount = 0;
        //        CSVWriteCount = 0;

        //        List<string> destinationStrings = new List<string>();

        //        using (var reader = new System.IO.StreamReader(FileToStarTxt))
        //        {
        //            while (!reader.EndOfStream)
        //            {
        //                string oneS = reader.ReadLine();
        //                int twoI = oneS.IndexOf(',');
        //                string threeS = oneS.Substring(twoI + 1);
        //                int fourI = threeS.IndexOf(',');
        //                string fiveS = threeS.Substring(0, fourI);
        //                string sixS = threeS.Substring(fourI + 1);
        //                string sevenS = fiveS + "*" + sixS;

        //                destinationStrings.Add(sixS);
        //                CSVReadCount++;
        //            }
        //        }

        //        using (System.IO.StreamWriter writetext = new System.IO.StreamWriter(FileToStarTxt + @"CSV_Star_" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".csv"))
        //        {
        //            int cnt = 2;
        //            writetext.WriteLine("Length*Path");
        //            foreach (string s in destinationStrings)
        //            {
        //                //writetext.WriteLine("=LEN(B" + cnt.ToString() + ")*" + s);
        //                writetext.WriteLine(s.Length.ToString() + "*" + s);
        //                cnt++;
        //                CSVWriteCount++;
        //            }
        //        }

        //        StatusMessage = "Complete";
        //    });
        //}


        private ICommand morph2StarCommand;
        public ICommand Morph2StarCommand
        {
            get
            {
                return morph2StarCommand ?? (morph2StarCommand = new CommandHandler(() => Morph2Star(), _canExecute));
            }
        }
        private async void Morph2Star()
        {
            await Task.Run(() =>
            {
                StatusMessage = "Ready";
                CSVReadCount = 0;
                CSVWriteCount = 0;

                List<string> destinationStrings = new List<string>();

                using (var reader = new System.IO.StreamReader(FileToStarTxt))
                {
                    while (!reader.EndOfStream)
                    {
                        string oneS = reader.ReadLine();
                        int twoI = oneS.IndexOf(',');
                        string threeS = oneS.Substring(twoI + 1);
                        int fourI = threeS.IndexOf(',');
                        string fiveS = threeS.Substring(0, fourI);
                        string sixS = threeS.Substring(fourI + 1);
                        string sevenS = fiveS + "*" + sixS;

                        destinationStrings.Add(sixS);
                        CSVReadCount++;
                    }
                }

                using (System.IO.StreamWriter writetext = new System.IO.StreamWriter(FileToStarTxt + @"_Star_" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".txt"))
                {
                    int cnt = 2;
                    writetext.WriteLine("Length*Path");
                    foreach (string s in destinationStrings)
                    {
                        writetext.WriteLine("=LEN(B" + cnt.ToString() + ")*" + s);
                        cnt++;
                        CSVWriteCount++;
                    }
                }

                StatusMessage = "Complete";
            });
        }

        #endregion

        #region "Checker vs Crawler Path Compare"
        private ICommand processNDSLogCommand;
        public ICommand ProcessNDSLogCommand
        {
            get
            {
                return processNDSLogCommand ?? (processNDSLogCommand = new CommandHandler(() => ProcessNDSLog(), _canExecute));
            }
        }
        private async void ProcessNDSLog()
        {
            await Task.Run(() =>
            {
                CSVReadCount = 0;
                CSVWriteCount = 0;
                if (NDSLogFileType)
                {
                    List<string> fileNames = new List<string>();
                    string fileName = string.Empty;
                    string sourceFolder = string.Empty;
                    string destinationFolder = string.Empty;

                    using (var reader = new System.IO.StreamReader(NDSLog2Process))
                    {
                        while (!reader.EndOfStream)
                        {
                            string line = reader.ReadLine();
                            List<int> guillements = WhateverExtensions.ExtensionsAreMe.AllIndexesOf(line, "\"");
                            if (guillements.Count > 0)
                            {
                                CSVReadCount++;
                                fileName = line.Substring(guillements[0] + 1, guillements[1] - guillements[0] - 1);
                                sourceFolder = line.Substring(guillements[2] + 1, guillements[3] - guillements[2] - 1);
                                destinationFolder = line.Substring(guillements[4] + 1, guillements[5] - guillements[4] - 1);
                                string fullFileName = sourceFolder + System.IO.Path.DirectorySeparatorChar + fileName;
                                string lskdjfsdf = "\\\\?\\UNC";
                                string lskdjfsddf = @"\\?\UNC";
                                string lsksdjfsdf = @"\*";
                                string lskadjfsdf = "\\*";
                                string fullFileNameV2 = fullFileName.Replace(@"\\?\UNC", @"\");
                                fileNames.Add(fullFileNameV2);
                            }

                        }
                    }
                    //using (System.IO.StreamWriter writetext = new System.IO.StreamWriter((NDSLogDestination + @"_Star_" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".csv"), true, Encoding.Unicode))
                    string logName = System.IO.Path.GetFileNameWithoutExtension(ndsLog2Process);
                    using (System.IO.StreamWriter writetext = new System.IO.StreamWriter(NDSLogDestination + System.IO.Path.DirectorySeparatorChar + logName + "_" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".csv", false, Encoding.Unicode))
                    {
                        foreach (string fnItem in fileNames)
                        {
                            writetext.WriteLine(fnItem.Length.ToString() + "*" + fnItem);
                            CSVWriteCount++;
                        }

                    }
                    StatusMessage = "Completed";
                }
                else //folder
                {
                    List<string> folderNames = new List<string>();
                    string folderName = string.Empty;
                    string sourceFolder = string.Empty;
                    string destinationFolder = string.Empty;

                    using (var reader = new System.IO.StreamReader(NDSLog2Process))
                    {
                        while (!reader.EndOfStream)
                        {

                            string line = reader.ReadLine();
                            //string lskdjfsdf = "\\\\?\\UNC";
                            string theFrontOne = @"\\?\UNC";
                            //string lsksdjfsdf = @"\*";
                            //string lskadjfsdf = "\\*";
                            int theFrontOneIndex = line.IndexOf(theFrontOne);
                            string theBackOne = @"\*:";
                            int theBackOneIndex = line.IndexOf(theBackOne);
                            if ((theFrontOneIndex > -1) && ((theBackOneIndex > -1)))
                            {
                                //int indexOfTheThingyAtTheFront = line.IndexOf(@"\\?\UNC");
                                //int indexOfTheThingyAtTheBack = line.IndexOf(\"\*");
                                int testCnt = line.Length;
                                string eeee = line.Substring(0, theBackOneIndex);
                                string eeeqwww = line.Substring(theFrontOneIndex);

                                string whatever = line.Substring(theFrontOneIndex, theBackOneIndex - theFrontOneIndex);
                                string whateverV2 = whatever.Replace(@"\\?\UNC", @"\");

                                CSVReadCount++;
                                folderNames.Add(whateverV2);
                            }
                        }
                    }
                    string logName = System.IO.Path.GetFileNameWithoutExtension(ndsLog2Process);
                    using (System.IO.StreamWriter writetext = new System.IO.StreamWriter(NDSLogDestination + System.IO.Path.DirectorySeparatorChar + logName + "_" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".csv", false, Encoding.Unicode))
                    {
                        foreach (string fnItem in folderNames)
                        {
                            writetext.WriteLine(fnItem.Length.ToString() + "*" + fnItem);
                            CSVWriteCount++;
                        }

                    }
                }
                StatusMessage = "Completed";
            });

        }

        #endregion

        //private bool ExecuteLPFNBulkList_Linq(string DateGuid)
        //{
        //    System.IO.Directory.CreateDirectory(LogLocationTxt + System.IO.Path.DirectorySeparatorChar + "Details");
        //    bool success = true;

        //    //CurrentDirectory = new System.IO.DirectoryInfo(@"Z:\Users\JHamrlik");
        //    //CurrentDirectory = new System.IO.DirectoryInfo(@"C:\");
        //    if (!System.IO.File.Exists(PathFileImportTxt))
        //    {
        //        StatusMessage = "File does not exists.";
        //    }
        //    else
        //    {
        //        bool alreadyUNC = false;
        //        if (CurrentDirectory.FullName.StartsWith("\\"))
        //        {
        //            alreadyUNC = true;
        //        }
        //        try
        //        {
        //            dtRunningPer.Tick += new EventHandler(dtRunningPer_Tick);
        //            dtRunningPer.Interval = new TimeSpan(0, 0, 0, 0, 1);

        //            if (IncludeDocuments)
        //            {
        //                try
        //                {
        //                    StatusMessage = "Processing Files...";
        //                    string fileNamePath = CurrentDirectory.FullName.Substring(2).Replace("\\", "_");
        //                    string logFileName = "Files_" + fileNamePath + "_" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + "_" + "lpfn.csv";
        //                    using (System.IO.StreamWriter resultsFileByPathType = new System.IO.StreamWriter(LogLocationTxt + System.IO.Path.DirectorySeparatorChar + "Details" + System.IO.Path.DirectorySeparatorChar + logFileName, false, Encoding.Unicode))
        //                    {
        //                        string masterFileNameWOXtension = System.IO.Path.GetFileNameWithoutExtension(PathFileImportTxt);
        //                        string masterFileLogName = masterFileNameWOXtension + "_" + DateGuid + "_" + "lpfn.csv";
        //                        using (System.IO.StreamWriter resultsFileGlobal = new System.IO.StreamWriter(LogLocationTxt + System.IO.Path.DirectorySeparatorChar + masterFileLogName, true, Encoding.Unicode))
        //                        {
        //                            StatusMessage = "Processing Files...";
        //                            try
        //                            {
        //                                foreach (String fi in System.IO.Directory.EnumerateFiles(CurrentDirectory.FullName, "*.*", System.IO.SearchOption.AllDirectories).Where(x => x.Length > MinPathLength))
        //                                {
        //                                    try
        //                                    {
        //                                        PathCurrentlyCounting = fi;
        //                                        //if (!System.IO.File.Exists(fi))
        //                                        //{ 
        //                                        //    WarningCount++;
        //                                        //    PathLenghtCrawl.Log.Log.Write2WarningLog(LogLocationTxt, DateTime.Now, "File does Not Exists.", fi);
        //                                        //}
        //                                        ObjectCountTotal++;
        //                                        FileCountTotal++;
        //                                        ObjectCount++;
        //                                        FileCount++;
        //                                        try
        //                                        {
        //                                            resultsFileByPathType.WriteLine(FileCount + "," + fi.Length + "," + fi);
        //                                        }
        //                                        catch (Exception ex)
        //                                        {
        //                                            StatusMessage = ex.Message;
        //                                            PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "Error while writing to " + fileNamePath);
        //                                        }
        //                                        try
        //                                        {
        //                                            resultsFileGlobal.WriteLine(FolderCount + "," + fi.Length + "," + fi);
        //                                        }
        //                                        catch (Exception ex)
        //                                        {
        //                                            StatusMessage = ex.Message;
        //                                            PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "Error while writing to " + masterFileLogName);
        //                                        }

        //                                        UNCObject uncObject = new UNCObject() { Count = FileCount, CharacterCount = fi.Length, NameUNC = fi };

        //                                        UNCObjectFileList.Add(uncObject);
        //                                        App.Current.Dispatcher.BeginInvoke((Action)delegate ()
        //                                        {
        //                                            UNCObjectFileLst.Add(uncObject);
        //                                        });
        //                                    }
        //                                    #region "Catches"
        //                                    catch (ArgumentNullException ex)
        //                                    {
        //                                        ErrorCount++;
        //                                        StatusMessage = ex.Message;
        //                                        PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "ArgumentNullException: Error in Files inside ForEach with " + fi);
        //                                    }
        //                                    catch (System.IO.DirectoryNotFoundException ex)
        //                                    {
        //                                        ErrorCount++;
        //                                        StatusMessage = ex.Message;
        //                                        PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "DirectoryNotFoundException: Error in Files inside ForEach with " + fi);
        //                                    }
        //                                    catch (System.IO.PathTooLongException ex)
        //                                    {
        //                                        ErrorCount++;
        //                                        StatusMessage = ex.Message;
        //                                        PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "PathTooLongException: Error in Files inside ForEach with " + fi);
        //                                    }
        //                                    catch (System.IO.IOException ex)
        //                                    {
        //                                        ErrorCount++;
        //                                        StatusMessage = ex.Message;
        //                                        PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "IOException: Error in Files inside ForEach with " + fi);
        //                                    }
        //                                    catch (System.Security.SecurityException ex)
        //                                    {
        //                                        ErrorCount++;
        //                                        StatusMessage = ex.Message;
        //                                        PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "SecurityException: Error in Files inside ForEach with " + fi);
        //                                    }
        //                                    catch (UnauthorizedAccessException ex)
        //                                    {
        //                                        ErrorCount++;
        //                                        StatusMessage = ex.Message;
        //                                        PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "UnauthorizedAccessException: Error in Files inside ForEach with " + fi);
        //                                    }
        //                                    catch (ArgumentException ex)
        //                                    {
        //                                        ErrorCount++;
        //                                        StatusMessage = ex.Message;
        //                                        PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "ArgumentException: Error in Files inside ForEach with " + fi);
        //                                    }
        //                                    catch (Exception ex)
        //                                    {
        //                                        ErrorCount++;
        //                                        StatusMessage = ex.Message;
        //                                        PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "Generic Exception: Error in Files inside ForEach with " + fi);
        //                                    }
        //                                    #endregion
        //                                }
        //                            }
        //                            #region "Catches"
        //                            catch (ArgumentNullException ex)
        //                            {
        //                                ErrorCount++;
        //                                StatusMessage = ex.Message;
        //                                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "ArgumentNullException: Error in Files outside ForEach");
        //                            }
        //                            catch (System.IO.DirectoryNotFoundException ex)
        //                            {
        //                                ErrorCount++;
        //                                StatusMessage = ex.Message;
        //                                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "DirectoryNotFoundException: Error in Files outside ForEach");
        //                            }
        //                            catch (System.IO.PathTooLongException ex)
        //                            {
        //                                ErrorCount++;
        //                                StatusMessage = ex.Message;
        //                                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "PathTooLongException: Error in Files outside ForEach");
        //                            }
        //                            catch (System.IO.IOException ex)
        //                            {
        //                                ErrorCount++;
        //                                StatusMessage = ex.Message;
        //                                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "IOException: Error in Files outside ForEach");
        //                            }
        //                            catch (System.Security.SecurityException ex)
        //                            {
        //                                ErrorCount++;
        //                                StatusMessage = ex.Message;
        //                                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "SecurityException: Error in Files outside ForEach");
        //                            }
        //                            catch (UnauthorizedAccessException ex)
        //                            {
        //                                ErrorCount++;
        //                                StatusMessage = ex.Message;
        //                                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "UnauthorizedAccessException: Error in Files outside ForEach");
        //                            }
        //                            catch (ArgumentException ex)
        //                            {
        //                                ErrorCount++;
        //                                StatusMessage = ex.Message;
        //                                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "ArgumentException: Error in Files outside ForEach");
        //                            }
        //                            catch (Exception ex)
        //                            {
        //                                ErrorCount++;
        //                                StatusMessage = ex.Message;
        //                                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "Generic Exception: Error in Files outside ForEach");
        //                            }
        //                            #endregion

        //                            PathLenghtCrawl.POCO.Duration currentDuration = new PathLenghtCrawl.POCO.Duration() { Name = CurrentDirectory.FullName + " (Files)", Time = ValidatorRunningTimePer };
        //                            DurationList.Add(currentDuration);

        //                        }
        //                    }
        //                }
        //                #region "Catches"
        //                catch (ArgumentNullException ex)
        //                {
        //                    ErrorCount++;
        //                    StatusMessage = ex.Message;
        //                    PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "ArgumentNullException: Error while gathering files");
        //                }
        //                catch (System.IO.DirectoryNotFoundException ex)
        //                {
        //                    ErrorCount++;
        //                    StatusMessage = ex.Message;
        //                    PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "DirectoryNotFoundException: Error while gathering files");
        //                }
        //                catch (System.IO.PathTooLongException ex)
        //                {
        //                    ErrorCount++;
        //                    StatusMessage = ex.Message;
        //                    PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "PathTooLongException: Error while gathering files");
        //                }
        //                catch (System.IO.IOException ex)
        //                {
        //                    ErrorCount++;
        //                    StatusMessage = ex.Message;
        //                    PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "IOException: Error while gathering files");
        //                }
        //                catch (System.Security.SecurityException ex)
        //                {
        //                    ErrorCount++;
        //                    StatusMessage = ex.Message;
        //                    PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "SecurityException: Error while gathering files");
        //                }
        //                catch (UnauthorizedAccessException ex)
        //                {
        //                    ErrorCount++;
        //                    StatusMessage = ex.Message;
        //                    PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "UnauthorizedAccessException: Error while gathering files");
        //                }
        //                catch (ArgumentException ex)
        //                {
        //                    ErrorCount++;
        //                    StatusMessage = ex.Message;
        //                    PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "ArgumentException: Error while gathering files");
        //                }
        //                catch (Exception ex)
        //                {
        //                    ErrorCount++;
        //                    StatusMessage = ex.Message;
        //                    PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "Generic Exception: Error while gathering files");
        //                }
        //                #endregion
        //                finally
        //                {
        //                    //StatusMessage = "Creating Reports...";

        //                    //string fileNamePath = CurrentDirectory.FullName.Substring(2).Replace("\\", "_");
        //                    //using (System.IO.StreamWriter resultsFile = new System.IO.StreamWriter(LogLocationTxt + System.IO.Path.DirectorySeparatorChar + "Files_" + fileNamePath + "_" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + "_" + "results.csv", false, Encoding.Unicode))
        //                    //{
        //                    //    try
        //                    //    {
        //                    //        foreach (UNCObject path in UNCObjectFileList)
        //                    //        {
        //                    //            resultsFile.WriteLine(path.Count + "," + path.CharacterCount + "," + path.NameUNC);
        //                    //        }
        //                    //    }
        //                    //    catch (Exception ex)
        //                    //    {
        //                    //        PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "Error while writing log (files)");
        //                    //    }
        //                    //}
        //                    //string masterFileNameWOXtension = System.IO.Path.GetFileNameWithoutExtension(PathFileImportTxt);
        //                    //using (System.IO.StreamWriter resultsFile = new System.IO.StreamWriter(LogLocationTxt + System.IO.Path.DirectorySeparatorChar + masterFileNameWOXtension + "_" + DateGuid + "_" + "lpfn.csv", true, Encoding.Unicode))
        //                    //{
        //                    //    try
        //                    //    {
        //                    //        foreach (UNCObject path in UNCObjectFileList)
        //                    //        {
        //                    //            resultsFile.WriteLine(path.Count + "," + path.CharacterCount + "," + path.NameUNC);
        //                    //        }
        //                    //    }
        //                    //    catch (Exception ex)
        //                    //    {
        //                    //        PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "Error while writing master log (files)");
        //                    //    }
        //                    //}
        //                }
        //            }

        //            if (IncludeFolders)
        //            {
        //                try
        //                {
        //                    string fileNamePath = CurrentDirectory.FullName.Substring(2).Replace("\\", "_");
        //                    string logFileName = "Folders_" + fileNamePath + "_" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + "_" + "lpfn.csv";
        //                    using (System.IO.StreamWriter resultsFileByPathType = new System.IO.StreamWriter(LogLocationTxt + System.IO.Path.DirectorySeparatorChar + "Details" + System.IO.Path.DirectorySeparatorChar + logFileName, false, Encoding.Unicode))
        //                    {
        //                        string masterFileNameWOXtension = System.IO.Path.GetFileNameWithoutExtension(PathFileImportTxt);
        //                        string masterFileLogName = masterFileNameWOXtension + "_" + DateGuid + "_" + "lpfn.csv";
        //                        using (System.IO.StreamWriter resultsFileGlobal = new System.IO.StreamWriter(LogLocationTxt + System.IO.Path.DirectorySeparatorChar + masterFileNameWOXtension + "_" + DateGuid + "_" + "lpfn.csv", true, Encoding.Unicode))
        //                        {
        //                            StatusMessage = "Processing Folders...";
        //                            try
        //                            {
        //                                foreach (String di in System.IO.Directory.EnumerateDirectories(CurrentDirectory.FullName, "*", System.IO.SearchOption.AllDirectories).Where(x => x.Length > MinPathLength))
        //                                {
        //                                    try
        //                                    {
        //                                        PathCurrentlyCounting = di;
        //                                        //if (!System.IO.Directory.Exists(di))
        //                                        //{
        //                                        //    WarningCount++;
        //                                        //    PathLenghtCrawl.Log.Log.Write2WarningLog(LogLocationTxt, DateTime.Now, "Directory does Not Exists.", di);
        //                                        //}
        //                                        ObjectCountTotal++;
        //                                        FolderCountTotal++;
        //                                        ObjectCount++;
        //                                        FolderCount++;

        //                                        try
        //                                        {
        //                                            resultsFileByPathType.WriteLine(FolderCount + "," + di.Length + "," + di);
        //                                        }
        //                                        catch (Exception ex)
        //                                        {
        //                                            StatusMessage = ex.Message;
        //                                            PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "Error while writing to " + logFileName);
        //                                        }
        //                                        try
        //                                        {
        //                                            resultsFileGlobal.WriteLine(FolderCount + "," + di.Length + "," + di);
        //                                        }
        //                                        catch (Exception ex)
        //                                        {
        //                                            StatusMessage = ex.Message;
        //                                            PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "Error while writing to " + masterFileLogName);
        //                                        }

        //                                        UNCObject uncObject = new UNCObject() { Count = FolderCount, CharacterCount = di.Length, NameUNC = di };
        //                                        UNCObjectFolderList.Add(uncObject);

        //                                        App.Current.Dispatcher.BeginInvoke((Action)delegate ()
        //                                        {
        //                                            UNCObjectFolderLst.Add(uncObject);
        //                                        });
        //                                    }
        //                                    #region "Catches"
        //                                    catch (ArgumentNullException ex)
        //                                    {
        //                                        ErrorCount++;
        //                                        StatusMessage = ex.Message;
        //                                        PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "ArgumentNullException: Error in Directory inside ForEach with " + di);
        //                                    }
        //                                    catch (System.IO.DirectoryNotFoundException ex)
        //                                    {
        //                                        ErrorCount++;
        //                                        StatusMessage = ex.Message;
        //                                        PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "DirectoryNotFoundException: Error in Directory inside ForEach with " + di);
        //                                    }
        //                                    catch (System.IO.PathTooLongException ex)
        //                                    {
        //                                        ErrorCount++;
        //                                        StatusMessage = ex.Message;
        //                                        PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "PathTooLongException: Error in Directory inside ForEach with " + di);
        //                                    }
        //                                    catch (System.IO.IOException ex)
        //                                    {
        //                                        ErrorCount++;
        //                                        StatusMessage = ex.Message;
        //                                        PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "IOException: Error in Directory inside ForEach with " + di);
        //                                    }
        //                                    catch (System.Security.SecurityException ex)
        //                                    {
        //                                        ErrorCount++;
        //                                        StatusMessage = ex.Message;
        //                                        PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "SecurityException: Error in Directory inside ForEach with " + di);
        //                                    }
        //                                    catch (UnauthorizedAccessException ex)
        //                                    {
        //                                        ErrorCount++;
        //                                        StatusMessage = ex.Message;
        //                                        PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "UnauthorizedAccessException: Error in Directory inside ForEach with " + di);
        //                                    }
        //                                    catch (ArgumentException ex)
        //                                    {
        //                                        ErrorCount++;
        //                                        StatusMessage = ex.Message;
        //                                        PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "ArgumentException: Error in Directory inside ForEach with " + di);
        //                                    }
        //                                    catch (Exception ex)
        //                                    {
        //                                        ErrorCount++;
        //                                        StatusMessage = ex.Message;
        //                                        PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "Generic Exception: Error in Directory inside ForEach with " + di);
        //                                    }
        //                                    #endregion
        //                                }
        //                            }
        //                            #region "Catches"
        //                            catch (ArgumentNullException ex)
        //                            {
        //                                ErrorCount++;
        //                                StatusMessage = ex.Message;
        //                                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "ArgumentNullException: Error in Directory outside ForEach");
        //                            }
        //                            catch (System.IO.DirectoryNotFoundException ex)
        //                            {
        //                                ErrorCount++;
        //                                StatusMessage = ex.Message;
        //                                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "DirectoryNotFoundException: Error in Directory outside ForEach");
        //                            }
        //                            catch (System.IO.PathTooLongException ex)
        //                            {
        //                                ErrorCount++;
        //                                StatusMessage = ex.Message;
        //                                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "PathTooLongException: Error in Directory outside ForEach");
        //                            }
        //                            catch (System.IO.IOException ex)
        //                            {
        //                                ErrorCount++;
        //                                StatusMessage = ex.Message;
        //                                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "IOException: Error in Directory outside ForEach");
        //                            }
        //                            catch (System.Security.SecurityException ex)
        //                            {
        //                                ErrorCount++;
        //                                StatusMessage = ex.Message;
        //                                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "SecurityException: Error in Directory outside ForEach");
        //                            }
        //                            catch (UnauthorizedAccessException ex)
        //                            {
        //                                ErrorCount++;
        //                                StatusMessage = ex.Message;
        //                                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "UnauthorizedAccessException: Error in Directory outside ForEach");
        //                            }
        //                            catch (ArgumentException ex)
        //                            {
        //                                ErrorCount++;
        //                                StatusMessage = ex.Message;
        //                                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "ArgumentException: Error in Directory outside ForEach");
        //                            }
        //                            catch (Exception ex)
        //                            {
        //                                ErrorCount++;
        //                                StatusMessage = ex.Message;
        //                                PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "Generic Exception: Error in Directory outside ForEach");
        //                            }
        //                            #endregion

        //                            PathLenghtCrawl.POCO.Duration currentDuration = new PathLenghtCrawl.POCO.Duration() { Name = CurrentDirectory.FullName + " (+ Folders)", Time = ValidatorRunningTimePer };
        //                            DurationList.Add(currentDuration);
        //                        }
        //                    }
        //                }
        //                #region "Catches"
        //                catch (ArgumentNullException ex)
        //                {
        //                    ErrorCount++;
        //                    StatusMessage = ex.Message;
        //                    PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "ArgumentNullException: Error while gathering directories");
        //                }
        //                catch (System.IO.DirectoryNotFoundException ex)
        //                {
        //                    ErrorCount++;
        //                    StatusMessage = ex.Message;
        //                    PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "DirectoryNotFoundException: Error while gathering directories");
        //                }
        //                catch (System.IO.PathTooLongException ex)
        //                {
        //                    ErrorCount++;
        //                    StatusMessage = ex.Message;
        //                    PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "PathTooLongException: Error while gathering directories");
        //                }
        //                catch (System.IO.IOException ex)
        //                {
        //                    ErrorCount++;
        //                    StatusMessage = ex.Message;
        //                    PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "IOException: Error while gathering directories");
        //                }
        //                catch (System.Security.SecurityException ex)
        //                {
        //                    ErrorCount++;
        //                    StatusMessage = ex.Message;
        //                    PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "SecurityException: Error while gathering directories");
        //                }
        //                catch (UnauthorizedAccessException ex)
        //                {
        //                    ErrorCount++;
        //                    StatusMessage = ex.Message;
        //                    PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "UnauthorizedAccessException: Error while gathering directories");
        //                }
        //                catch (ArgumentException ex)
        //                {
        //                    ErrorCount++;
        //                    StatusMessage = ex.Message;
        //                    PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "ArgumentException: Error while gathering directories");
        //                }
        //                catch (Exception ex)
        //                {
        //                    ErrorCount++;
        //                    StatusMessage = ex.Message;
        //                    PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "Generic Exception: Error while gathering directories");
        //                }
        //                #endregion
        //                finally
        //                {
        //                    //StatusMessage = "Creating Reports...";

        //                    //string fileNamePath = CurrentDirectory.FullName.Substring(2).Replace("\\", "_");
        //                    //using (System.IO.StreamWriter resultsFile = new System.IO.StreamWriter(LogLocationTxt + System.IO.Path.DirectorySeparatorChar + "Folders_" + fileNamePath + "_" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + "_" + "lpfn.csv", false, Encoding.Unicode))
        //                    //{
        //                    //    try
        //                    //    {
        //                    //        foreach (UNCObject path in UNCObjectFolderList)
        //                    //        {
        //                    //            resultsFile.WriteLine(path.Count + "," + path.CharacterCount + "," + path.NameUNC);
        //                    //        }
        //                    //    }
        //                    //    catch (Exception ex)
        //                    //    {
        //                    //        StatusMessage = ex.Message;
        //                    //        PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "Error while writing log (folders)");
        //                    //    }
        //                    //}
        //                    //string masterFileNameWOXtension = System.IO.Path.GetFileNameWithoutExtension(PathFileImportTxt);
        //                    //using (System.IO.StreamWriter resultsFile = new System.IO.StreamWriter(LogLocationTxt + System.IO.Path.DirectorySeparatorChar + masterFileNameWOXtension + "_" + DateGuid + "_" + "lpfn.csv", true, Encoding.Unicode))
        //                    //{
        //                    //    try
        //                    //    {
        //                    //        foreach (UNCObject path in UNCObjectFolderList)
        //                    //        {
        //                    //            resultsFile.WriteLine(path.Count + "," + path.CharacterCount + "," + path.NameUNC);
        //                    //        }
        //                    //    }
        //                    //    catch (Exception ex)
        //                    //    {
        //                    //        StatusMessage = ex.Message;
        //                    //        PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "Error while writing master log (folders)");
        //                    //    }
        //                    //}
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            ErrorCount++;
        //            StatusMessage = ex.Message;
        //            PathLenghtCrawl.Log.Log.Write2ErrorLog(LogLocationTxt, DateTime.Now, ex.Message, "General error in ExecuteLPFNBulkList_Linq()");
        //        }
        //        finally
        //        { PathCurrentlyCounting = "N/A"; }


        //    }
        //    if (ErrorCount > 0)
        //    {
        //        success = false;
        //        StatusMessage = "Check Error Logs";

        //    }
        //    else
        //    {
        //        StatusMessage = "Ready";
        //    }
        //    return success;
        //}

        //private void ExecuteLPFNBulkList()
        //{

        //    bool alreadyUNC = false;
        //    if (CurrentDirectory.FullName.StartsWith("\\")) { alreadyUNC = true; }
        //    try
        //    {
        //        dtRunningPer.Tick += new EventHandler(dtRunningPer_Tick);
        //        dtRunningPer.Interval = new TimeSpan(0, 0, 0, 0, 1);

        //        StatusMessage = "Gathering Information (Files)...";
        //        System.IO.FileInfo[] fileInfos = CurrentDirectory.GetFiles();
        //        IEnumerable<System.IO.FileInfo> fileList = CurrentDirectory.GetFiles("*.*", System.IO.SearchOption.AllDirectories);
        //        StatusMessage = "Processing Files...";

        //        try
        //        {
        //            foreach (System.IO.FileInfo fi in fileInfos)
        //            {
        //                string fiInUNC = String.Empty;
        //                if (!alreadyUNC)
        //                {
        //                    try
        //                    {
        //                        fiInUNC = MappedDriveResolver.ResolveToUNC(fi.FullName);
        //                    }
        //                    catch (Exception)
        //                    {
        //                        throw;
        //                    }

        //                }
        //                else
        //                {
        //                    fiInUNC = fi.FullName;
        //                }
        //                try
        //                {

        //                    int fileLenght = fiInUNC.Length;
        //                    if (fileLenght > MinPathLength)
        //                    {
        //                        UNCObject uncObject = new UNCObject() { CharacterCount = fileLenght, NameUNC = fiInUNC };
        //                        ObjectCount++;
        //                        FileCount++;
        //                        UNCObjectFileLst.Add(uncObject);
        //                    }
        //                }
        //                catch (Exception)
        //                {

        //                    throw;
        //                }
        //            }
        //        }
        //        catch (Exception)
        //        {

        //            throw;
        //        }
        //        StatusMessage = "Gathering Information (Folders)...";
        //        System.IO.DirectoryInfo[] directoryInfos = CurrentDirectory.GetDirectories("*.*", System.IO.SearchOption.AllDirectories);
        //        StatusMessage = "Processing Folders...";
        //        try
        //        {


        //            foreach (System.IO.DirectoryInfo di in directoryInfos)
        //            {
        //                string diInUNC = String.Empty;
        //                if (!alreadyUNC)
        //                {
        //                    try
        //                    {
        //                        diInUNC = MappedDriveResolver.ResolveToUNC(di.FullName);
        //                    }
        //                    catch (Exception)
        //                    {
        //                        throw;
        //                    }

        //                }
        //                else
        //                {
        //                    diInUNC = di.FullName;
        //                }



        //                int directoryLenght = diInUNC.Length;
        //                if (directoryLenght > MinPathLength)
        //                {
        //                    try
        //                    {
        //                        UNCObject uncObject = new UNCObject() { CharacterCount = directoryLenght, NameUNC = diInUNC };
        //                        ObjectCount++;
        //                        FolderCount++;
        //                        UNCObjectFileLst.Add(uncObject);
        //                    }
        //                    catch (Exception)
        //                    {
        //                        throw;
        //                    }

        //                }
        //                try
        //                {
        //                    System.IO.FileInfo[] fileInfos1;
        //                    try
        //                    {
        //                        fileInfos1 = di.GetFiles();
        //                    }
        //                    catch (Exception ex)
        //                    {

        //                        throw;
        //                    }

        //                    foreach (System.IO.FileInfo fi in fileInfos1)
        //                    {
        //                        try
        //                        {


        //                            string fiInUNC = String.Empty;
        //                            if (!alreadyUNC)
        //                            {
        //                                try
        //                                {
        //                                    fiInUNC = MappedDriveResolver.ResolveToUNC(fi.FullName);
        //                                }
        //                                catch (Exception ex)
        //                                {

        //                                    throw;
        //                                }

        //                            }
        //                            else
        //                            {
        //                                fiInUNC = fi.FullName;
        //                            }
        //                            try
        //                            {
        //                                int fileLenght = fiInUNC.Length;
        //                                if (fileLenght > MinPathLength)
        //                                {
        //                                    UNCObject uncObject = new UNCObject() { CharacterCount = fileLenght, NameUNC = fiInUNC };
        //                                    ObjectCount++;
        //                                    FileCount++;
        //                                    UNCObjectFileLst.Add(uncObject);

        //                                }
        //                            }
        //                            catch (Exception ex)
        //                            {

        //                                throw;
        //                            }
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            throw;
        //                        }
        //                    }
        //                }
        //                catch (Exception es)
        //                {
        //                    //break was here the last time
        //                    throw;
        //                }
        //            }
        //        }
        //        catch (Exception)
        //        {

        //            throw;
        //        }
        //    }
        //    catch (UnauthorizedAccessException uae)
        //    {
        //        StatusMessage = "Unauthorized Access";
        //        //success = false;
        //    }
        //    catch (Exception ex)
        //    {
        //        StatusMessage = "Problem detected";
        //        //success = false;
        //    }
        //    StatusMessage = "Ready";
        //}


        //private ICommand executeLPFNListCommand;
        //public ICommand ExecuteLPFNListCommand
        //{
        //    get
        //    {
        //        return executeLPFNListCommand ?? (executeLPFNListCommand = new CommandHandler(() => ExecuteLPFNList(), _canExecute));
        //    }
        //}
        //private async void ExecuteLPFNList()
        //{
        //    await Task.Run(() =>
        //    {
        //        CopyAsCSVEnabled = false;
        //        SaveAsCSVEnabled = false;

        //        ObjectCount = 0;
        //        FolderCount = 0;
        //        FileCount = 0;
        //        ObjectCountTotal = 0;
        //        FolderCountTotal = 0;
        //        FileCountTotal = 0;



        //        App.Current.Dispatcher.BeginInvoke((Action)delegate ()
        //        {
        //            UNCObjectLst.Clear();
        //        });
        //        try
        //        {
        //            //FileCountTotal = System.IO.Directory.EnumerateFiles(CurrentDirectory.FullName, "*.*", System.IO.SearchOption.AllDirectories).Count();
        //            //FolderCountTotal = System.IO.Directory.EnumerateDirectories(CurrentDirectory.FullName, "*", System.IO.SearchOption.AllDirectories).Count();
        //            //ObjectCountTotal = FileCountTotal + FolderCountTotal;

        //            //System.Collections.ObjectModel.ObservableCollection<UNCObject> uncObjectList = new System.Collections.ObjectModel.ObservableCollection<UNCObject>();
        //            foreach (System.IO.FileInfo fi in CurrentDirectory.GetFiles())
        //            {
        //                if (MappedDriveResolver.ResolveToUNC(fi.FullName).Length > MinPathLength)
        //                {
        //                    UNCObject uncObject = new UNCObject() { CharacterCount = MappedDriveResolver.ResolveToUNC(fi.FullName).Length, NameUNC = MappedDriveResolver.ResolveToUNC(fi.FullName) };
        //                    App.Current.Dispatcher.BeginInvoke((Action)delegate ()
        //                    {
        //                        UNCObjectLst.Add(uncObject);
        //                        ObjectCount++;
        //                        FileCount++;
        //                    });
        //                }
        //            }

        //            foreach (System.IO.DirectoryInfo di in CurrentDirectory.GetDirectories("*.*", System.IO.SearchOption.AllDirectories))
        //            {
        //                if (MappedDriveResolver.ResolveToUNC(di.FullName).Length > MinPathLength)
        //                {
        //                    UNCObject uncObject = new UNCObject() { CharacterCount = MappedDriveResolver.ResolveToUNC(di.FullName).Length, NameUNC = MappedDriveResolver.ResolveToUNC(di.FullName) };
        //                    App.Current.Dispatcher.BeginInvoke((Action)delegate ()
        //                    {
        //                        UNCObjectLst.Add(uncObject);
        //                        FolderCount++;
        //                        ObjectCount++;
        //                    });
        //                }
        //                foreach (System.IO.FileInfo fi in di.GetFiles())
        //                {
        //                    if (MappedDriveResolver.ResolveToUNC(fi.FullName).Length > MinPathLength)
        //                    {
        //                        UNCObject uncObject = new UNCObject() { CharacterCount = MappedDriveResolver.ResolveToUNC(fi.FullName).Length, NameUNC = MappedDriveResolver.ResolveToUNC(fi.FullName) };
        //                        App.Current.Dispatcher.BeginInvoke((Action)delegate ()
        //                        {
        //                            UNCObjectLst.Add(uncObject);
        //                            FileCount++;
        //                            ObjectCount++;
        //                        });
        //                    }
        //                }
        //            }
        //        }
        //        catch (UnauthorizedAccessException uae)
        //        {
        //            StatusMessage = "Unauthorized Access";
        //            //success = false;
        //        }
        //        catch (Exception ex)
        //        {
        //            StatusMessage = "Problem detected";
        //            //success = false;
        //        }

        //        CopyAsCSVEnabled = true;
        //        SaveAsCSVEnabled = true;
        //    });
        //}

        #region "Comparisons"

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


        private ICommand comparePathCommand;
        public ICommand ComparePathCommand
        {
            get
            {
                return comparePathCommand ?? (comparePathCommand = new CommandHandler(() => ComparePath(), _canExecute));
            }
        }
        private void ComparePath()
        {
            //SAVE LOGS AS UNICODE
            List<string> CrawlerList = new List<string>();
            List<string> CheckerList = new List<string>();

            using (var reader = new System.IO.StreamReader(@"C:\Mitch\New folder\Paths\CrawlerUnicode.txt"))
            {
                while (!reader.EndOfStream)
                {
                    string fullString = reader.ReadLine();
                    if (fullString.Contains("TRS release v.5.5.8 Sunday April 7 2019 starting at 700 am EST RAPPEL nou"))
                    {
                        int lkj = 8;
                    }

                    int indexOfFirstSeperatorChar = fullString.IndexOf(@"\\");
                    string pathOnly = fullString.Substring(indexOfFirstSeperatorChar);
                    //TRS release v.5.5.8 Sunday April 7 2019 starting at 700 am EST RAPPEL nou

                    if (pathOnly.Contains("TRS release v.5.5.8 Sunday April 7 2019 starting at 700 am EST RAPPEL nou"))
                    {
                        int lkj = 8;
                    }

                    CrawlerList.Add(pathOnly);
                }
            }

            using (var reader = new System.IO.StreamReader(@"C:\Mitch\New folder\Paths\CheckerUnicode.txt"))
            {
                while (!reader.EndOfStream)
                {
                    string fullString = reader.ReadLine();
                    if (fullString.Contains("TRS release v.5.5.8 Sunday April 7 2019 starting at 700 am EST RAPPEL nou"))
                    {
                        int lkj = 8;
                    }


                    int indexOfFirstSeperatorChar = fullString.IndexOf(@"\\");
                    string pathWQuotes = fullString.Substring(indexOfFirstSeperatorChar);
                    string pathOnly = pathWQuotes.Substring(0, pathWQuotes.Length - 1);

                    if (pathOnly.Contains("TRS release v.5.5.8 Sunday April 7 2019 starting at 700 am EST RAPPEL nou"))
                    {
                        int lkj = 8;
                    }

                    CheckerList.Add(pathOnly);
                }
            }

            CrawlerList.Sort();
            CheckerList.Sort();

            using (System.IO.StreamWriter writetext = new System.IO.StreamWriter(@"C:\Mitch\New folder\Paths\Results_Crawler_AlphaOrder_" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".txt"))
            {
                foreach (string item in CrawlerList)
                {
                    writetext.WriteLine(item);
                }

            }

            using (System.IO.StreamWriter writetext = new System.IO.StreamWriter(@"C:\Mitch\New folder\Paths\Results_Checker_AlphaOrder_" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".txt"))
            {
                foreach (string item in CheckerList)
                {
                    writetext.WriteLine(item);
                }
            }

            IEnumerable<string> inFirstOnly = CrawlerList.Distinct().Except(CheckerList, StringComparer.OrdinalIgnoreCase).ToList();
            IEnumerable<string> inSecondOnly = CheckerList.Distinct().Except(CrawlerList, StringComparer.OrdinalIgnoreCase).ToList();

            IEnumerable<string> alsoInFirst = CrawlerList.Distinct().Intersect(CheckerList, StringComparer.OrdinalIgnoreCase).ToList();
            IEnumerable<string> alsoInSecond = CheckerList.Distinct().Intersect(CrawlerList, StringComparer.OrdinalIgnoreCase).ToList();


            using (System.IO.StreamWriter writetext = new System.IO.StreamWriter(@"C:\Mitch\New folder\Paths\Results_PathInCrawlerListButNotInCheckerList_" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".txt"))
            {
                foreach (string item in inFirstOnly)
                {
                    writetext.WriteLine(item);
                }

            }

            using (System.IO.StreamWriter writetext = new System.IO.StreamWriter(@"C:\Mitch\New folder\Paths\Results_PathInCheckerListButNotInCrawlerList_" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".txt"))
            {
                foreach (string item in inSecondOnly)
                {
                    writetext.WriteLine(item);
                }

            }


            int count = 0;

            //foreach (string item in inSecondOnly)
            foreach (string item in CrawlerList)
            {

                int lkj = inSecondOnly.Where(x => x.StartsWith(item)).Count();
                if (lkj > 0)
                {
                    string lskdjf = item;
                    string lsdkfjls = inSecondOnly.Where(x => x.StartsWith(item)).FirstOrDefault();

                    int werweweerw = 0;

                }
                count += lkj;

            }


            //int count = 0;
            //foreach (string crawlerInput in CrawlerList)
            //{
            //    foreach (string item in inSecondOnly)
            //    {
            //        bool lkj = item.StartsWith(crawlerInput);
            //        if (lkj)
            //        {
            //            count++;
            //            break;
            //        }
            //    }
            //}



            int totalCntUserList1 = CrawlerList.Count();
            int totalCntUserList2 = CheckerList.Count();

            int totalList1WOutDupes = CrawlerList.Distinct().Count();
            int totalList2WOutDupes = CheckerList.Distinct().Count();

            //int totalList1WOutDupes = userList1.Distinct().Count();
            //int totalList2WOutDupes = userList2.Distinct().Count();

            int usersThatAreInTheFirstListButNotInTheSecondList = inFirstOnly.Count();
            int usersThatAreInTheSecondListButNotInTheFirstList = inSecondOnly.Count();

            int usersThatAreBothInTheFirstAndSecondList = totalCntUserList2 - usersThatAreInTheSecondListButNotInTheFirstList;
            int usersThatAreBothInTheSecondAndFirstList = totalCntUserList1 - usersThatAreInTheFirstListButNotInTheSecondList;



            int s = 9;
        }

        #endregion

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

        //private ICommand setUNCObjectCommand;
        //public ICommand SetUNCObjectCommand
        //{
        //    get
        //    {
        //        return setUNCObjectCommand ?? (setUNCObjectCommand = new CommandHandler(() => SetUNCObject(), _canExecute));
        //    }
        //}
        //private void SetUNCObject()
        //{
        //    SetCurrentUNCObject();
        //}


        //private ICommand addSinglePathCommand;
        //public ICommand AddSinglePathCommand
        //{
        //    get
        //    {
        //        return addSinglePathCommand ?? (addSinglePathCommand = new CommandHandler(() => AddSinglePath(), _canExecute));
        //    }
        //}
        //private void AddSinglePath()
        //{

        //}

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
                case "FilePathTxt":
                    //N/A - Dummy case
                    break;
                case "FileToStarTxt":
                    openFD.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                    //handles empty strings
                    if (System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(FileToStarTxt)))
                    {
                        openFD.InitialDirectory = System.IO.Path.GetDirectoryName(FileToStarTxt);
                    }
                    break;
                case "DatabaseValLocationPath":
                    openFD.Filter = "db files (*.db)|*.db|All files (*.*)|*.*";
                    //handles empty strings
                    if (System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(DatabaseValLocationPath)))
                    {
                        openFD.InitialDirectory = System.IO.Path.GetDirectoryName(DatabaseValLocationPath);
                    }
                    break;
                case "PathFileImportTxt":
                    //openFD.ValidateNames = false;
                    //openFD.CheckFileExists = false;
                    //openFD.CheckPathExists = true;
                    //openFD.FileName = "Folders Included";
                    openFD.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
                    //handles empty strings
                    if (System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(PathFileImportTxt)))
                    {
                        openFD.InitialDirectory = System.IO.Path.GetDirectoryName(PathFileImportTxt);
                    }
                    break;
                case "MagikFileSource":
                    //handles empty strings
                    if (System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(MagikFileSource)))
                    {
                        openFD.InitialDirectory = System.IO.Path.GetDirectoryName(MagikFileSource);
                    }
                    break;
                case "NDSLog2Process":
                    if (System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(NDSLog2Process)))
                    {
                        openFD.InitialDirectory = System.IO.Path.GetDirectoryName(NDSLog2Process);
                    }
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
                    case "FileToStarTxt":
                        FileToStarTxt = pathName;
                        break;
                    case "DatabaseValLocationPath":
                        DatabaseValLocationPath = pathName;
                        break;
                    case "PathFileImportTxt":
                        //if (System.IO.Path.GetExtension(pathName) != ".csv")
                        //{
                        //    StatusMessage = ".csv format required.";
                        //}
                        //else
                        //{
                        PathFileImportTxt = pathName;
                        //}
                        break;
                    case "MagikFileSource":
                        MagikFileSource = pathName;
                        break;
                    case "NDSLog2Process":
                        NDSLog2Process = pathName;
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
                case "NDSLogDestination":
                    if (NDSLogDestination != string.Empty)
                    {
                        folderBrowserDialog.SelectedPath = NDSLogDestination;
                    }
                    break;
                case "PathFolder2Evalulate":
                    if (PathFolder2Evalulate != string.Empty)
                    {
                        folderBrowserDialog.SelectedPath = PathFolder2Evalulate;
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
                    case "NDSLogDestination":
                        NDSLogDestination = pathName;
                        break;
                    case "PathFolder2Evalulate":
                        PathFolder2Evalulate = pathName;
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

        #region "Magik Copy"

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

        #endregion
    }
}
