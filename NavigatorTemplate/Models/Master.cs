using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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

        private bool useAuditDatabase = Properties.Settings.Default.UseAuditDatabase;
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
                    Properties.Settings.Default.UseAuditDatabase = value;
                    SaveProperties();
                    NotifyPropertyChanged("UseAuditDatabase");
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


        private string databaseValLocationPath = Properties.Settings.Default.DatabaseValLocationPath;
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
                    Properties.Settings.Default.DatabaseValLocationPath = value;
                    SaveProperties();
                    NotifyPropertyChanged("DatabaseValLocationPath");
                }
            }
        }

        private string databaseValName = Properties.Settings.Default.DatabaseValName;
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
                    Properties.Settings.Default.DatabaseValName = value;
                    SaveProperties();
                    NotifyPropertyChanged("DatabaseValName");
                }
            }
        }

        private string databaseValLocation = Properties.Settings.Default.DatabaseValLocation;
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
                    Properties.Settings.Default.DatabaseValLocation = value;
                    SaveProperties();
                    NotifyPropertyChanged("DatabaseValLocation");
                }
            }
        }


        private string filePath = Properties.Settings.Default.FilePath;
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
                    Properties.Settings.Default.FilePath = value;
                    SaveProperties();
                    NotifyPropertyChanged("FilePath");
                }
            }
        }

        private string folderPath = Properties.Settings.Default.FolderPath;
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
                    Properties.Settings.Default.FolderPath = value;
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
                    //GetDirectoryFiles();
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

        public bool GetDirectoryFiles()
        {           
            FileCount = 0;
            bool success = true;
            //StatusMessage = "Loading...";
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

        #region "************************************************************************************************* OI Functions"

        private void SaveProperties()
        {
            StatusMessage = "Saving...";
            Properties.Settings.Default.Save();
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

            if (openFD.ShowDialog() == true)
            {
                string pathName = openFD.FileName;
                switch (obj.ToString())
                {
                    case "FilePathTxt":
                        FilePath = pathName;
                        break;
                    case "DatabaseValLocationPath":
                        FilePath = pathName;
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
                case "FolderPathTxt":
                    FolderPath = CurrentDirectory.FullName;
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
                case "FolderPathTxt":
                    if (FolderPath != string.Empty)
                    {
                        folderBrowserDialog.SelectedPath = FolderPath;
                    }
                    break;
                case "DatabaseValLocation":
                    if (DatabaseValLocation != string.Empty)
                    {
                        folderBrowserDialog.SelectedPath = DatabaseValLocation;
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
                    case "FolderPathTxt":
                        FolderPath = pathName;
                        break;
                    case "DatabaseValLocation":
                        DatabaseValLocation = pathName;
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
            //AverageRunningTimePerLst.Add(stopWatchRunningPer.Elapsed.TotalSeconds);
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
    }
}
