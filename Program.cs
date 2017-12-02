using Fclp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.GameStats.Server
{
    class Options
    {
        public string Prefix { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var commandLineParser = new FluentCommandLineParser<Options>();

            commandLineParser
                .Setup(options => options.Prefix)
                .As("prefix")
                .SetDefault("http://localhost:8080/")
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
                serviceEndpoint.Behaviors.Add(new WebHttpBehavior());
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
