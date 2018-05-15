using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using JudgeCore;

namespace JudgeWeb
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) =>
            {
                if (context.Request.Path == "/test.html")
                {
                    var cmp = new JudgeCore.Compiler.Msvc();
                    var op = new List<IJudger>
                    {
                        new JudgeCore.Judger.CommonJudge("1 2\n", "3\n"),
                        new JudgeCore.Judger.CommonJudge("3 4\n", "7\n"),
                        new JudgeCore.Judger.CommonJudge("5 6\n", "11\n"),
                        new JudgeCore.Judger.CommonJudge("7 8\n", "15\n"),
                        new JudgeCore.Judger.CommonJudge("9 10\n", "19\n"),
                    };
                    var current = new Job(cmp, op);
                    await context.Response.WriteAsync("<!DOCTYPE html><html><head><meta charset=\"utf-8\" /><title>运行测试</title></head><body><h1>A + B Problem</h1><textarea name=\"code\" style=\"width:500px;height:300px\">");
                    await context.Response.WriteAsync(context.Request.Form["code"]);
                    
                    current.Build(context.Request.Form["code"]);
                    current.Judge(false);
                    
                    await context.Response.WriteAsync("</textarea><br><ol>");
                    current.State.ForEach(async (j) => await context.Response.WriteAsync($"<li>{j.Result.ToString()}</li>"));
                    await context.Response.WriteAsync("</ol>");

                    if (current.State[0].Result == JudgeResult.CompileError)
                    {
                        await context.Response.WriteAsync($"<pre>{current.CompileInfo}</pre>");
                    }
                    await context.Response.WriteAsync("</body></html>");
                }
                else
                {
                    var to = await System.IO.File.ReadAllTextAsync("submit.html");
                    await context.Response.WriteAsync(to);
                }
            });
        }
    }
}
