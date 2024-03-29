﻿using Semaphore.Infrastructure.WorkWithFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semaphore.Infrastructure.Settings
{
    public static class AppSettings
    {
        //static string _name;
        // public static string Name { get {return _name;} set { _name = value;}}
        static string _pathToSynchronizerFolder = @"x:\utils\Semaphore\synchronizer";
        public static string PathToSynchronizerFolder { get { return _pathToSynchronizerFolder; } set { _pathToSynchronizerFolder = value; } }


        static string _pathToSynchronizerFile = @"x:\utils\Semaphore\synchronizer\1.txt";
        public static string PathToSynchronizerFile { get { return _pathToSynchronizerFile; } set { _pathToSynchronizerFile = value; } }
        
        //static string _pathToData = @"..\\..\\settings\\data.txt";
        //public static string PathToData { get { return _pathToData; } set { _pathToData = value; } }

        static string _dbConnectionString = "User ID=import_user;password=sT7hk9Lm;Data Source=CD_WORK";
        public static string DbConnectionString { get { return _dbConnectionString; } set { _dbConnectionString = value; } }
    }
}
