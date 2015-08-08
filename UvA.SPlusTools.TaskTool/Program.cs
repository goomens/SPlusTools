using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UvA.Utilities;
using System.Collections.Concurrent;
using System.Threading;
using System.Runtime.ExceptionServices;
using System.Security;
using System.Xml.Serialization;
using System.IO;
using UvA.SPlusTools.Data.Entities;
using UvA.SPlusTools.Data.Tasks;
using UvA.SPlusTools.Data;

namespace UvA.SPlusTools.TaskTool
{
    class Program
    {
        static void Main(string[] args)
        {
            if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
                Console.WriteLine("S+ TaskTool version {0}", System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion);
            Console.WriteLine();
            string fileName = null;
            if (args.Length == 0)
            {
                Console.Write("Please specify .xml configuration file: ");
                fileName = Console.ReadLine();
            }
            else
                fileName = args[1];
            if (!File.Exists(fileName))
            {
                Console.WriteLine("File not found: {0}", fileName);
                return;
            }

            SPlusTask[] tasks = null;
            XmlSerializer ser = new XmlSerializer(typeof(SPlusTask[]));
            try
            {
                using (var str = File.OpenRead(fileName))
                    tasks = (SPlusTask[])ser.Deserialize(str);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fatal error reading configuration file: {0}", ex.Message);
                return;
            }
            for (int i = 0; i < tasks.Length; i++)
            {
                var task = tasks[i];
                Console.WriteLine("Executing task {0} ({1})", i + 1, task.GetType().Name);
                task.Execute();
                Console.WriteLine("Task {0} completed", i + 1);
                Console.WriteLine();
            }
            Console.WriteLine("All tasks completed. Press any key to exit.");
            Console.ReadKey();
        }

    }

    
}
