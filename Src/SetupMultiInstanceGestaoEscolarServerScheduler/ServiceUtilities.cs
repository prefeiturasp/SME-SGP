namespace SetupMultiInstanceGestaoEscolarServerScheduler
{

    using System;
    using System.Collections.Generic;
    using System.Configuration.Install;
    using System.Linq;
    using System.Management;
    using System.Net;
    using System.Net.NetworkInformation;
    using System.ServiceProcess;

    class ServiceUtilities
    {
        private ManagementObject managementObj = null;

        public static bool PortInUse(decimal port)
        {
            return PortInUse((int)port);
        }

        public static bool PortInUse(int port)
        {
            bool inUse = false;

            IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] ipEndPoints = ipProperties.GetActiveTcpListeners();

            foreach (IPEndPoint endPoint in ipEndPoints)
            {
                if (endPoint.Port == port)
                {
                    inUse = true;
                    break;
                }
            }

            return inUse;
        }

        public static List<int> GetPorts()
        {
            IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] ipEndPoints = ipProperties.GetActiveTcpListeners();

            return ipEndPoints.Select(p => p.Port).ToList();
        }

        public static List<int> GetAvailablePorts(int minPort = 555, int maxPort = 610)
        {
            IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] ipEndPoints = ipProperties.GetActiveTcpListeners();

            List<int> listAvailablePorts = new List<int>();

            for (int i = minPort; i <= maxPort; i++)
            {
                if (!ipEndPoints.Any(p => p.Port == i))
                    listAvailablePorts.Add(i);
            }

            return listAvailablePorts;
        }
    }
}
