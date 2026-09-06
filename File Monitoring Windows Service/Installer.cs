using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace File_Monitoring_Windows_Service
{
    [RunInstaller(true)]
    public partial class Installer : System.Configuration.Install.Installer
    {
        private ServiceProcessInstaller serviceProcessInstaller;
        private ServiceInstaller serviceInstaller;
        public Installer()
        {
            InitializeComponent();
            // Configure the Service Process Installer
            serviceProcessInstaller = new ServiceProcessInstaller
            {
                Account = ServiceAccount.LocalSystem // Adjust as needed (e.g., NetworkService, LocalService)
            };

            // Configure the Service Installer
            serviceInstaller = new ServiceInstaller
            {
                ServiceName = "FileMonitoring", // Must match the ServiceName in your ServiceBase class
                DisplayName = "File Monitoring Windows Service",
                StartType = ServiceStartMode.Automatic // Or Automatic, depending on requirements
            };

            // Add installers to the installer collection
            Installers.Add(serviceProcessInstaller);
            Installers.Add(serviceInstaller);
        }
    }
}



