using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using IniUtils;

namespace Simulator
{
    class Configurations
    {
        private static bool _configSetup = SetupConfig();

        public static IniFile Config { get; set; }

        public static bool SetupConfig()
        {
            IniFileUtility.UseIniFileParser = true;

            string location = Assembly.GetEntryAssembly().Location;
            if (string.IsNullOrEmpty(location))
            {
                return true;
            }
            IniFileUtility.IniFileDirectory = Path.GetDirectoryName(location);

            Config = IniFileParser.ParseIniFile("Models.ini");

            return true;
        }



    }
}
