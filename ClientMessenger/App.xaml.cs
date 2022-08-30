using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ClientMessenger.Services;
using ClientMessenger.ViewModels;
using ClientMessenger.Views;
using SimpleInjector;

namespace ClientMessenger
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Container Container = new();

        protected override void OnStartup(StartupEventArgs e)
        {
            Container.Register<ViewModelFactory>(Lifestyle.Singleton);
            Container.Register<ClientChatViewModel>(Lifestyle.Singleton);
            Container.Register<RegistrationUserViewModel>(Lifestyle.Singleton);
            Container.Register<MainView>(Lifestyle.Singleton);
            Container.Register<MainViewModel>(Lifestyle.Singleton);
            Container.Register<UserManager>(Lifestyle.Singleton);
            Container.Register<MessageManager>(Lifestyle.Singleton);
            Container.Register<ClientListener>(Lifestyle.Singleton);
        }
    }
}