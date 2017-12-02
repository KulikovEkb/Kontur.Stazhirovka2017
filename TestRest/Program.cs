using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace TestRest
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost serviceHost = new ServiceHost(typeof(UserService), new Uri("http://localhost:8080"));
            bool openSucceeded = false;

            try
            {
                ServiceEndpoint serviceEndpoint = serviceHost.AddServiceEndpoint(typeof(IUserService), new WebHttpBinding(), "hosting");
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
                
                Console.WriteLine("Service is running! Press Enter to stop.");
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
