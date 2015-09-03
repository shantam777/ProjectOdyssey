using System;
using System.Collections.Generic;
using System.Configuration;
using Awesomium.Core;
using System.Data;
using System.Linq;
using System.Windows;

namespace ProjectOdyssey
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        WebConfig defaultConfig = new WebConfig()
        {
            HomeURL = "http://google.com".ToUri(),
            LogPath = @".\OdysseyStartup.log",
            LogLevel = LogLevel.Verbose
        };
        protected override void OnStartup(StartupEventArgs e)
        {
            if(!WebCore.IsInitialized)
            {
                WebCore.Initialize(defaultConfig);
            }
            base.OnStartup(e);
        }
        protected override void OnExit(ExitEventArgs e)
        {
            if (WebCore.IsInitialized)
                WebCore.Shutdown();
            base.OnExit(e);
        }
    }
}
