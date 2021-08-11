using AutoMapper;
using Caliburn.Micro;
using RetailManager.DesktopUI.Helpers;
using RetailManager.DesktopUI.Library.Api;
using RetailManager.DesktopUI.Library.Helpers;
using RetailManager.DesktopUI.Library.Models;
using RetailManager.DesktopUI.Models;
using RetailManager.DesktopUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace RetailManager.DesktopUI
{
    public class Bootstrapper : BootstrapperBase
    {
        private SimpleContainer _container = new SimpleContainer();

        public Bootstrapper()
        {
            Initialize();

            ConventionManager.AddElementConvention<PasswordBox>(
            PasswordBoxHelper.BoundPasswordProperty,
            "Password",
            "PasswordChanged");
        }

        protected override void Configure()
        {
            IMapper mapper = ConfigureAutoMapper();

            _container.Instance(mapper);

            _container.Instance(_container)
                .PerRequest<IProductEndpoint, ProductEndpoint>();

            _container
                .Singleton<IWindowManager, WindowManager>()
                .Singleton<IEventAggregator, EventAggregator>()
                .Singleton<ILoggedInUserModel, LoggedInUserModel>()
                .Singleton<IConfigHelper, ConfigHelper>()
                .Singleton<IUICoreEndpoint, UICoreEndpoint>()
                .Singleton<ISaleEndpoint, SaleEndpoint>();

            GetType().Assembly.GetTypes()
                .Where(type => type.IsClass)
                .Where(type => type.Name.EndsWith("ViewModel"))
                .ToList()
                .ForEach(viewModelType => _container.RegisterPerRequest(
                    viewModelType, viewModelType.ToString(), viewModelType));
        }

        private static IMapper ConfigureAutoMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Product, ProductDisplayModel>();
                cfg.CreateMap<CartItem, CartItemDisplayModel>();
            });

            var mapper = config.CreateMapper();
            return mapper;
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }

        protected override object GetInstance(Type service, string key)
        {
            return _container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }
    }
}
