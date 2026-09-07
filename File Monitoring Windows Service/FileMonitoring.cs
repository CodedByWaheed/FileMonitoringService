using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management.Instrumentation;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace File_Monitoring_Windows_Service
{
    public partial class FileMonitoring : ServiceBase
    {
        public FileMonitoring()
        {
            InitializeComponent();

            if (!Directory.Exists(ConfigurationManager.AppSettings["SourceFolder"]))
            {
                Directory.CreateDirectory(ConfigurationManager.AppSettings["SourceFolder"]);
            }

            if (!Directory.Exists(ConfigurationManager.AppSettings["DestinationFolder"]))
            {
                Directory.CreateDirectory(ConfigurationManager.AppSettings["DestinationFolder"]);
            }

        }
        private void LogFunc(string Message)
        {
            string logDirectory = ConfigurationManager.AppSettings["LogDirectory"];
            string logFilePath = Path.Combine(logDirectory, ConfigurationManager.AppSettings["LogFileName"]);
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }
            string LogMessage = $"[{DateTime.Now:yyyy/MM/dd HH:mm:ss}] {Message}\n";
            File.AppendAllText(logFilePath, LogMessage);
        }
        private string RenameFile(string sourceFilePath)
        {
            try
            {
                string directory = Path.GetDirectoryName(sourceFilePath);
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(sourceFilePath);
                string extension = Path.GetExtension(sourceFilePath);

                string newFileName = Guid.NewGuid().ToString() + extension;

               
                
                LogFunc($"Renamed file: {fileNameWithoutExtension} to {newFileName}");

                return newFileName;
            }
            catch (Exception ex)
            {
                LogFunc($"Error renaming file: {ex.Message}");
            }  
            return null;
        }
        private void MoveFile(string sourceFilePath, string destinationDirectory)
        {
            try
            {
                if (!Directory.Exists(destinationDirectory))
                {
                    Directory.CreateDirectory(destinationDirectory);
                }

                string OldFileName = Path.GetFileName(sourceFilePath);

                string NewfileName = RenameFile(sourceFilePath);

                string destinationFilePath = Path.Combine(destinationDirectory, NewfileName);

                File.Move(sourceFilePath, destinationFilePath);
                //File.Delete(sourceFilePath);
                LogFunc($"Moved file: {OldFileName} to {destinationDirectory}");
            }
            catch (Exception ex)
            {
                LogFunc($"Error moving file: {ex.Message}");
            }
        }
        protected override void OnStart(string[] args)
        {
            LogFunc("File Monitoring Service Started.");
            if (Environment.UserInteractive)
            {
                Console.WriteLine("File Monitoring Service Started.");
            }

            FileSystemWatcher watcher = new FileSystemWatcher(ConfigurationManager.AppSettings["SourceFolder"]);
            watcher.EnableRaisingEvents = true;
            watcher.Created += Watcher_Created;

        }

        private void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            if (!Directory.Exists(ConfigurationManager.AppSettings["DestinationFolder"]))
                Directory.CreateDirectory(ConfigurationManager.AppSettings["DestinationFolder"]);
            try
            {
                MoveFile(e.FullPath, ConfigurationManager.AppSettings["DestinationFolder"]);
            }   
            catch
            {
                LogFunc($"The File didnt move: {ConfigurationManager.AppSettings["SourceFolder"].ToString()}");
                // Write to console if running interactively
                if (Environment.UserInteractive)
                {
                    Console.WriteLine($"The File Fucked me Doesnt Exist: {ConfigurationManager.AppSettings["SourceFolder"]}");
                }
            }
        }

        protected override void OnStop()
        {
            LogFunc("FileMonitoring Service Stopped.");
            // Write to console if running interactively
            if (Environment.UserInteractive)
            {
                Console.WriteLine("File Monitoring Service Stopped.");
            }
        }




       
        // Simulate service behavior in console mode
        public void StartInConsole()
        {
            OnStart(null); // Trigger OnStart logic
            Console.WriteLine("Press Enter to stop the service...");
            Console.ReadLine(); // Wait for user input to simulate service stopping
            OnStop(); // Trigger OnStop logic
            Console.ReadKey();

        }
    }
}



