using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NavigatorTemplate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Models.Master Master { get; set; }
        public Models.Master master = new Models.Master();
        public MainWindow()
        {
            InitializeComponent();
            Master = master;

            this.DataContext = this;

            Master.Initialize();
            Loaded += delegate { Master.OnLoaded(); };
            Unloaded += delegate { Master.OnUnloaded(); };

            LoadTreeView_Click(this, new System.Windows.RoutedEventArgs());
        }

        #region "Event Driven TreeView"

        private async void OnItemSelected(object sender, RoutedEventArgs e)
        {
            bool success = true;
            Master.StatusMessage = "In Progress...";

            Master.CustomSearcher.CurrentFileCount = 0;
            Master.CustomSearcher.CurrentFolderCount = 0;

            //icfLst.Clear();
            TreeViewMigration.Tag = e.OriginalSource;

            Models.TagInfo ti = (Models.TagInfo)((System.Windows.FrameworkElement)TreeViewMigration.Tag).Tag;

            System.IO.DirectoryInfo dir;
            System.IO.DriveInfo di = ti.driveInfo;
            if (di != null)
            {
                dir = di.RootDirectory;
            }
            else
            {
                dir = ti.directoryInfo;
            }
            //Master.CurrentSelectedNode = dir.Name;
            Master.CurrentDirectory = dir;
            //Master.SetCurrentUNCObject();
            await Task.Run(() =>
            {
              //success = Master.GetDirectoryFiles();
            });
            
            if (success)
            {
                Master.StatusMessage = "Ready";
            }

        }
        private void LoadTreeView_Click(object sender, RoutedEventArgs e)
        {
            TreeViewMigration.Items.Clear();
            string[] drives = System.Environment.GetLogicalDrives();

            foreach (string drive in drives)
            {
                System.IO.DriveInfo di = new System.IO.DriveInfo(drive);
                System.IO.DriveType dt = di.DriveType;


                if (di.IsReady)
                {
                    TreeViewItem tvi = new TreeViewItem();
                    tvi.Name = drive.Substring(0, 1) + "Drive";
                    //tvi.Header = drive;
                    try
                    {
                        tvi.Header = "(" + di.Name.Substring(0, 2) + ") " + di.VolumeLabel;
                    }
                    catch (Exception)
                    {

                        tvi.Header = "(" + di.Name.Substring(0, 2) + ") -- Unavailable --";
                    }

                    //tvi.Tag = di;
                    tvi.Tag = new Models.TagInfo { driveInfo = di, root = true, loaded = false, directoryInfo = null };
                    tvi.Selected += tvi_Selected;
                    tvi.Expanded += tvi_Expanded;

                    TreeViewMigration.Items.Add(tvi);
                    AddEmptyItem(tvi);

                    if (tvi.Name.Substring(0, 1) == "C")
                    {
                        tvi.Focus();
                    }
                }
            }





        }
        private void LoadFilesBtn_Click(object sender, RoutedEventArgs e)
        {
            //IDMLView.ItemsSource = null;

        }
        private void AddEmptyItem(TreeViewItem tvi)
        {
            TreeViewItem emtpyTvi = new TreeViewItem();
            emtpyTvi.Name = "empty";
            emtpyTvi.Header = "[empty]";

            tvi.Items.Add(emtpyTvi);
        }
        void tvi_Selected(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = sender as TreeViewItem;
            Models.TagInfo ti = (Models.TagInfo)item.Tag;
        }
        void tvi_Expanded(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = sender as TreeViewItem;
            Models.TagInfo ti = (Models.TagInfo)item.Tag;

            if (!ti.loaded)
            {
                item.Items.RemoveAt(0);
                LoadDirectoryTree(item);
            }

            ti.loaded = true;
            item.Tag = ti;
        }
        private void LoadDirectoryTree(TreeViewItem tvi)
        {
            System.IO.DirectoryInfo[] subDirs = null;
            Models.TagInfo ti = (Models.TagInfo)tvi.Tag;
            if (ti.root)
            {
                System.IO.DirectoryInfo rootDir = ti.driveInfo.RootDirectory;
                List<string> subFolderLst = new List<string>();
                subDirs = rootDir.GetDirectories();
            }
            else
            {
                System.IO.DirectoryInfo dir = ti.directoryInfo;

                System.IO.FileAttributes fa = new System.IO.FileAttributes();
                fa = dir.Attributes;
                //if ((fa.HasFlag(System.IO.FileAttributes.Hidden)) || (fa.HasFlag(System.IO.FileAttributes.System)))
                //{

                //}
                //else
                //{
                try
                {
                    subDirs = dir.GetDirectories();
                }
                catch (System.IO.IOException ioEx)
                {
                    HandleException(ioEx.Message, dir, "tvLoad");
                }
                catch (System.UnauthorizedAccessException uaEx)
                {
                    HandleException(uaEx.Message, dir, "tvLoad");
                }
                catch (System.Exception unknownEx)
                {
                    HandleException(unknownEx.Message, dir, "tvLoad");
                }
                finally { }

                //}
            }
            if (subDirs != null)
            {
                foreach (System.IO.DirectoryInfo dirInfo in subDirs)
                {
                    TreeViewItem subTVI = new TreeViewItem();
                    //subTVI.Name = dirInfo.Name;
                    subTVI.Header = dirInfo.Name;
                    subTVI.ToolTip = dirInfo.FullName;
                    //bool asdfsadf = subTVI.IsLoaded;
                    //bool asdfasdfsdfsadf = subTVI.IsInitialized;

                    tvi.Items.Add(subTVI);
                    subTVI.Tag = new Models.TagInfo { root = false, directoryInfo = dirInfo, loaded = false };
                    subTVI.Selected += tvi_Selected;
                    subTVI.Expanded += tvi_Expanded;
                    AddEmptyItem(subTVI);
                }
            }
        }
        private void HandleException(string exception, System.IO.DirectoryInfo dir, string processName)
        {
            WriteToLog(dir.FullName, exception, processName);
        }
        private void WriteToLog(string obj, string exMessage, string processName)
        {
            if (System.IO.Directory.Exists(Master.FolderPath))
            {
                using (System.IO.StreamWriter logfile = new System.IO.StreamWriter(Master.FolderPath + System.IO.Path.DirectorySeparatorChar + processName + "_mainLog.log", true))
                //using (System.IO.StreamWriter logfile = new System.IO.StreamWriter(Properties.Settings.Default.LogLocation + System.IO.Path.DirectorySeparatorChar + processName + "_mainLog.csv", true))
                {
                    logfile.WriteLine(DateTime.Now.ToString() + "," + obj + "," + exMessage.Trim());
                }

            }
            else
            {
                Master.StatusMessage = "Invalid Log Path";
            }
        }

        #endregion
    }
}
