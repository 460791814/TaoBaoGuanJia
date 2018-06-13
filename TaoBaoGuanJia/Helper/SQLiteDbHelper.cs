using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using Dapper;

namespace TaoBaoGuanJia.Helper
{
    
    public class SQLiteBaseRepository
    {
        public static string DbFile
        {
            get
            {
                return System.Environment.CurrentDirectory + @"/data/tbgj.db";
               // return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "VideoInfo.db");
            }
        }
        public static SQLiteConnection SimpleDbConnection()
        {

            string connString = string.Format("Data Source={0};Password=;", DbFile);
            return new SQLiteConnection(connString);
        }
    }
}
