using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.Management.Instrumentation;
using Microsoft.Win32;
using System.IO;
using System.Reflection;

namespace DeviceStatus
{
    class Program
    {
        static void Main(string[] args)
        {
   
            GetInfo();
            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }

        static void GetInfo()
        {
            var dir = Path.Combine(Path.GetFullPath(Assembly.GetEntryAssembly().Location), @"\text.csv");
            if(!File.Exists(dir))
            {
                File.CreateText(dir); 
            }
            var myQueryoObject = new ManagementObjectSearcher("select * from Win32_PnPSignedDriver");
            var myQueryObject1 = new ManagementObjectSearcher("select * from Win32_PnPEntity");
            string DeviceID ;
            var csv = new StringBuilder();
            var item = new List<string>();
            var newLine = string.Format(new NullFormat(), "{0},{1},{2},{3},{4},{5},{6},{7},{8}", "DeviceName", "DriverName", "Status", "DeviceID", "Description", "ClassGuid", "DriverVersion", "InfName", "Manufacturer");
            csv.AppendLine(newLine);
            item.Add(newLine);
            foreach (var obj in myQueryoObject.Get())
            {
                try
                {
                    DeviceID = obj.GetPropertyValue("DeviceID").ToString();
                    

                    foreach (var obj1 in myQueryObject1.Get())
                    {
                       string DeviceID1 = obj1.GetPropertyValue("DeviceID").ToString();
                        if (DeviceID.Contains(DeviceID1))
                        {
                             newLine = string.Format(new NullFormat(),"{0},{1},{2},{3},{4},{5},{6},{7},{8}", obj["DeviceName"]?.ToString(), obj["DriverName"]?.ToString(), obj1["Status"]?.ToString(), obj1["DeviceID"]?.ToString(), obj["Description"]?.ToString(), obj["ClassGuid"]?.ToString(), obj["DriverVersion"]?.ToString(), obj["InfName"]?.ToString(), obj["Manufacturer"]?.ToString());
                            csv.AppendLine(newLine);
                            item.Add(newLine);
                            File.WriteAllText(dir, csv.ToString());

                            Console.WriteLine("DeviceName  :  " + obj["DeviceName"] + "\t");
                            Console.WriteLine("\t");
                            Console.WriteLine("DriverName  :  " + obj["DriverName"] + "\t");
                            Console.WriteLine("\t");
                            Console.WriteLine("Status  :  " + obj1["Status"] + "\t");
                            Console.WriteLine("\t");
                            Console.WriteLine("DeviceID  :  " + obj["DeviceID"] + "\t");
                            Console.WriteLine("\t");
                            Console.WriteLine("Description  :  " + obj["Description"] + "\t");
                            Console.WriteLine("\t");
                            Console.WriteLine("ClassGuid  :  " + obj["ClassGuid"] + "\t");
                            Console.WriteLine("\t");
                            Console.WriteLine("DriverVersion  :  " + obj["DriverVersion"] + "\t");
                            Console.WriteLine("\t");
                            Console.WriteLine("InfName  :  " + obj["InfName"] + "\t");
                            Console.WriteLine("\t");
                            Console.WriteLine("Manufacturer  :  " + obj["Manufacturer"] + "\t");
                            Console.WriteLine("\t");
                            Console.WriteLine("-----------------------------------------------------------------------------");
                            Console.WriteLine("\t");
                           

                        }
                    }

                   
                }
                catch(Exception a)
                {
                    Console.WriteLine( a.Message);
                }
            }
           
        


        }

       
    }
    public class NullFormat : IFormatProvider, ICustomFormatter
    {
        public object GetFormat(Type service)
        {
            if (service == typeof(ICustomFormatter))
            {
                return this;
            }
            else
            {
                return null;
            }
        }

        public string Format(string format, object arg, IFormatProvider provider)
        {
            if (arg == null)
            {
                return "NULL";
            }
            IFormattable formattable = arg as IFormattable;
            if (formattable != null)
            {
                return formattable.ToString(format, provider);
            }
            return arg.ToString();
        }
    }

}