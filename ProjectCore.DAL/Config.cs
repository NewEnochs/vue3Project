using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCore.DAL
{
    public class Config
    {
        // <summary>
        /// Account have permission to create database
        /// 用有建库权限的数据库账号
        /// </summary>
        public static string? ConnectionString = GetConnString();

        public static string? CHIS = GetConnString("CHIS");


        ///获取配置的连接字符串
        public static string? GetConnString(string connName = "DefaultConnection")
        {
            //获取链接字符串
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            var config = builder.Build();
            string? connString = config?.GetConnectionString(connName);
            return connString;
        }

    }
}
