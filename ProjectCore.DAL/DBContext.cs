using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using System.Configuration;


namespace ProjectCore.DAL
{
    public class DBContext
    {
        public SqlSugarClient? db;
        #region 数据库连接配置
        public SqlSugarClient CreateDbConnection()
        {
            db = new SqlSugarClient(new ConnectionConfig()
            {
                DbType = DbType.SqlServer,
                ConnectionString = Config.ConnectionString,
                InitKeyType = InitKeyType.Attribute,
                IsAutoCloseConnection = true,
                AopEvents = new AopEvents
                {
                    OnLogExecuting = (sql, p) =>
                    {
                        Console.WriteLine(sql);
                    }
                }
            });

            return db;
        }
        #endregion
    }
    //创建一个注入类
    public static class SqlsugarSetup
    {
        public static void AddSqlsugarSetup(this IServiceCollection services, IConfiguration configuration)
        {
            //多租户 new SqlSugarScope(List<ConnectionConfig>,db=>{});

            SqlSugarScope sqlSugar = new SqlSugarScope(new ConnectionConfig()
            {
                DbType = SqlSugar.DbType.SqlServer,
                ConnectionString = Config.ConnectionString,
                IsAutoCloseConnection = true,
            },
             db => {  /***写AOP等方法***/});
            //ISugarUnitOfWork<DBContext> context = new SugarUnitOfWork<DBContext>(sqlSugar);
            //services.AddSingleton<ISugarUnitOfWork<DBContext>>(context);
        }
    }

}
