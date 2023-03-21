using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Model.Other;
using Newtonsoft.Json;
using SqlSugar;
using System.Text;
using WebAPI.Config;

namespace ApiDemo.config
{
    /// <summary>
    /// 扩展类，用于注册
    /// </summary>
    public static class HostBuilderExtend
    {
        public static void Register(this WebApplicationBuilder app)
        {
            app.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            app.Host.ConfigureContainer<ContainerBuilder>(builder =>
            {
                #region 注册Sqlsugar
                builder.Register<ISqlSugarClient>(context =>
                {
                    SqlSugarClient Db = new SqlSugarClient(new ConnectionConfig()
                    {
                        //连接字符串
                        ConnectionString = "Data Source=MLK;Initial Catalog=ApiDemo;Persist Security Info=True;User ID=sa;Password=1",
                        DbType = DbType.SqlServer,
                        IsAutoCloseConnection = true
                    });
                    //支持sql语句的输出，便于排查
                    Db.Aop.OnLogExecuting = (sql, pars) =>
                    {
                        Console.WriteLine(sql);//输出sql,查看执行sql 性能无影响

                        //5.0.8.2 获取无参数化SQL 对性能有影响，特别大的SQL参数多的，调试使用
                        //UtilMethods.GetSqlString(DbType.SqlServer,sql,pars)
                    };

                    return Db;
                });
                #endregion
                //注册接口(规定怎么做)(Interface)和实现层(Service)
                builder.RegisterModule(new AutofacModuleRegister());
            });
            //Automapper映射
            app.Services.AddAutoMapper(typeof(AutoMapperConfigs));
            //第一步，注册JWT
            app.Services.Configure<JWTTokenOptions>(app.Configuration.GetSection("JWTTokenOptions"));

            #region JWT校验
            //第二步，增加鉴权逻辑
            JWTTokenOptions tokenOptions = new JWTTokenOptions();
            app.Configuration.Bind("JWTTokenOptions", tokenOptions);
            app.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)//Scheme
             .AddJwtBearer(options =>  //这里是配置的鉴权的逻辑
             {
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     //JWT有一些默认的属性，就是给鉴权时就可以筛选了
                     ValidateIssuer = true,//是否验证Issuer
                     ValidateAudience = true,//是否验证Audience
                     ValidateLifetime = true,//是否验证失效时间
                     ValidateIssuerSigningKey = true,//是否验证SecurityKey
                     ValidAudience = tokenOptions.Audience,//
                     ValidIssuer = tokenOptions.Issuer,//Issuer，这两项和前面签发jwt的设置一致
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.SecurityKey))//拿到SecurityKey 
                 };
             });
            #endregion

            //添加跨域策略
            app.Services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", opt => opt.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().WithExposedHeaders("X-Pagination"));
            });

            //设置Json返回的日期格式
            app.Services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            });
        }

    }
}

