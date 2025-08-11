using System;
using System.IO;
using System.Net.NetworkInformation;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        List<string> servers = new List<string> { "8.8.8.8", "1.1.1.1" }; /// -> example IPs for Google DNS and Cloudflare DNS
        string csvPath = "SystemHealthReport.csv";

        using (StreamWriter sw = new StreamWriter(csvPath))
        {
            sw.WriteLine("Server,Status,ResponseTime(ms)");
            foreach (var server in servers)
            {
                try
                {
                    Ping ping = new Ping();
                    PingReply reply = ping.Send(server, 2000);
                    if (reply.Status == IPStatus.Success)
                        sw.WriteLine($"{server},Online,{reply.RoundtripTime}");
                    else
                        sw.WriteLine($"{server},Offline,");
                }
                catch
                {
                    sw.WriteLine($"{server},Error,");
                }
            }

            sw.WriteLine();
            sw.WriteLine("Drive,TotalSpaceGB,FreeSpaceGB");

            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                if (drive.IsReady)
                {
                    double totalGB = Math.Round(drive.TotalSize / (1024.0 * 1024 * 1024), 2);
                    double freeGB = Math.Round(drive.AvailableFreeSpace / (1024.0 * 1024 * 1024), 2);
                    sw.WriteLine($"{drive.Name},{totalGB},{freeGB}");
                }
            }
        }

        Console.WriteLine($"System health report saved to {csvPath}");
    }
}
