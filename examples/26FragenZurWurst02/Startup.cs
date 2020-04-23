using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FragenZurWurst.Bot;
using FragenZurWurst.Dialogs;
using FragenZurWurst.CognitiveModels;

namespace FragenZurWurst
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSingleton<IBotFrameworkHttpAdapter, AdapterWithErrorHandler>();

            services.AddSingleton<IStorage, MemoryStorage>();

            //services.AddSingleton<IFragenZurWurstRecognizer, LuisFragenZurWurstRecognizer>();
            services.AddSingleton<IFragenZurWurstRecognizer, SimpleFragenZurWurstRecognizer>();

            services.AddSingleton<UserState>();

            services.AddSingleton<ConversationState>();

            services.AddSingleton<MainDialog>();
            services.AddSingleton<OrderingDialog>();
            services.AddSingleton<ChooseSausageKindAndSauceDialog>();

            services.AddTransient<IBot, FragenZurWurstBot>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseMvc();
        }
    }
}
