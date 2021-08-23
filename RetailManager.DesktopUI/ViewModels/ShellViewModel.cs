using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using RetailManager.DesktopUI.EventModels;
using RetailManager.DesktopUI.Library.Models;

namespace RetailManager.DesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
    {
        private readonly SalesViewModel _salesViewModel;
        private readonly IEventAggregator _events;
        private readonly ILoggedInUserModel _user;

        public ShellViewModel(SalesViewModel salesViewModel, IEventAggregator events, ILoggedInUserModel user)
        {
            _salesViewModel = salesViewModel;
            _events = events;
            _user = user;
            _events.Subscribe(this);

            ActivateItem(IoC.Get<LoginViewModel>());
        }

        public void Handle(LogOnEvent message)
        {
            ActivateItem(_salesViewModel);
            NotifyOfPropertyChange(() => IsLoggedIn);
            //var loginViewModel = _container.GetInstance<LoginViewModel>();
        }

        public void ExitApplication()
        {
            TryClose();
        }

        public void LogOut()
        {
            _user.LogOffUser();
            ActivateItem(IoC.Get<LoginViewModel>());
            NotifyOfPropertyChange(() => IsLoggedIn);
        }

        public bool IsLoggedIn
        {
            get
            {
                return _user.Token?.Length > 0;
            }
        }
    }
}
