using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;

namespace MyLibrary
{
    public sealed class MyService
    {
        private static Lazy<MyService> lazy = new Lazy<MyService>(() => new MyService());

        public static MyService Instance { get { return lazy.Value; } }

        private MyService()
        {

        }

        public void WS_Restart(string ServiceName, ref string HataMesaji)
        {
            try
            {
                ServiceController[] services = ServiceController.GetServices();
                foreach (ServiceController controller in services)
                {
                    if (controller.ServiceName == ServiceName)
                    {
                        ServiceController controller2 = new ServiceController(controller.ServiceName);
                        if (controller2.Status != ServiceControllerStatus.Running)
                        {
                            controller2.Start();
                            controller2.WaitForStatus(ServiceControllerStatus.Running);
                            controller2.Refresh();
                        }
                        else
                        {
                            controller.Stop();
                            controller.WaitForStatus(ServiceControllerStatus.Stopped);
                            Thread.Sleep(5000);
                            controller.Start();
                            controller.WaitForStatus(ServiceControllerStatus.Running);
                            controller2.Refresh();
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                HataMesaji = exception.Message;
            }
        }

        public void WS_Start(string ServiceName, ref string HataMesaji)
        {
            try
            {
                ServiceController[] services = ServiceController.GetServices();
                foreach (ServiceController controller in services)
                {
                    if (controller.ServiceName == ServiceName)
                    {
                        ServiceController controller2 = new ServiceController(controller.ServiceName);
                        if (controller2.Status != ServiceControllerStatus.Running)
                        {
                            controller2.Start();
                            controller2.WaitForStatus(ServiceControllerStatus.Running);
                            controller2.Refresh();
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                HataMesaji = exception.Message;
            }
        }

        public void WS_Stop(string ServiceName, ref string HataMesaji)
        {
            try
            {
                ServiceController[] services = ServiceController.GetServices();
                foreach (ServiceController controller in services)
                {
                    if (controller.ServiceName == ServiceName)
                    {
                        ServiceController controller2 = new ServiceController(controller.ServiceName);
                        if (controller2.Status != ServiceControllerStatus.Stopped)
                        {
                            controller.Stop();
                            controller.WaitForStatus(ServiceControllerStatus.Stopped);
                            controller2.Refresh();
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                HataMesaji = exception.Message;
            }
        }

        public ServiceControllerStatus WS_State(string ServiceName)
        {
            string state = string.Empty;
            ServiceController[] services = ServiceController.GetServices();
            foreach (ServiceController controller in services)
                if (controller.ServiceName == ServiceName)
                    return new ServiceController(controller.ServiceName).Status;
            throw new Exception("Servis Bulunamadı.");
        }

        public List<string> WS_List()
        {
            List<string> liste = new List<string>();
            ServiceController[] services = ServiceController.GetServices();
            foreach (ServiceController controller in services)
                liste.Add(controller.ServiceName);
            return liste;
        }
    }
}
