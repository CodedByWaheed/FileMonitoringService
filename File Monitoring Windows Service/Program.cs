using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace File_Monitoring_Windows_Service
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        //static void Main()
        //{
        //    ServiceBase[] ServicesToRun;
        //    ServicesToRun = new ServiceBase[]
        //    {
        //        new FileMonitoring()
        //    };
        //    ServiceBase.Run(ServicesToRun);
        //}
        static void Main()
        {
            if (Environment.UserInteractive)
            {
                // Running in console mode
                Console.WriteLine("Running in console mode...");
                FileMonitoring service = new FileMonitoring();
                service.StartInConsole();
            }
            else
            {
                // Running as a Windows Service
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                    new FileMonitoring()
                };
                ServiceBase.Run(ServicesToRun);
            }

        }
    }
}
