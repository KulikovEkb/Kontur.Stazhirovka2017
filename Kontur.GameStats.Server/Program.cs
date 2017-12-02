using Fclp;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.GameStats.Server
{
    class Options
    {
        string prefix;
        public string Prefix
        {
            get
            {
                return prefix;
            }
            set
            {
                if (value.Substring(0, 9) == "http://+:" || value.Substring(0, 9) == "http://*:")
                    prefix = "http://" + Dns.GetHostEntry("localhost").HostName + value.Substring(8);
                else
                    prefix = value;
            }
        }
    }

    class Program
    {
        internal static readonly string databasePath = AppDomain.CurrentDomain.BaseDirectory + "Storage.db";
        internal static Serilog.Core.Logger log = new LoggerConfiguration()
                .MinimumLevel.Error()
                .WriteTo.RollingFile(AppDomain.CurrentDomain.BaseDirectory + "log.txt")
                .CreateLogger();

        static void Main(string[] args)
        {
            var commandLineParser = new FluentCommandLineParser<Options>();

            commandLineParser
                .Setup(options => options.Prefix)
                .As("prefix")
                .SetDefault("http://+:8080/")
                .WithDescription("HTTP prefix to listen on");

            commandLineParser
                .SetupHelp("h", "help")
                .WithHeader($"{AppDomain.CurrentDomain.FriendlyName} [--prefix <prefix>]")
                .Callback(text => Console.WriteLine(text));

            if (commandLineParser.Parse(args).HelpCalled)
                return;



            ServiceHost serviceHost = new ServiceHost(typeof(Service.Service), new Uri(commandLineParser.Object.Prefix));
            bool openSucceeded = false;

            try
            {
                ServiceEndpoint serviceEndpoint = serviceHost.AddServiceEndpoint(typeof(Service.IService), new WebHttpBinding(), "");
                serviceEndpoint.Behaviors.Add(new WebHttpBehavior { AutomaticFormatSelectionEnabled = true });
                serviceHost.Open();
                openSucceeded = true;
            }
            catch (Exception exc)
            {
                Console.WriteLine("Service host failed to open! - {0}", exc.ToString());
                Console.ReadLine();
            }
            finally
            {
                if (!openSucceeded)
                    serviceHost.Abort();
            }

            if (serviceHost.State == CommunicationState.Opened)
            {
                Console.WriteLine("Service is up and running at {0}. Press Enter to stop.", commandLineParser.Object.Prefix);
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("Service failed to open!!");
                Console.ReadLine();
            }

            bool closeSucceeded = false;
            try
            {
                serviceHost.Close();
                closeSucceeded = true;
            }
            catch (Exception exc)
            {
                Console.WriteLine("Service failed to close - {0}", exc.ToString());
                Console.ReadLine();
            }
            finally
            {
                if (!closeSucceeded)
                    serviceHost.Abort();
            }
        }
    }
}
