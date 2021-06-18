using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using RetailManager.DesktopUI.EventModels;

namespace RetailManager.DesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
    {
        private readonly SimpleContainer _container;
        private readonly SalesViewModel _salesViewModel;
        private readonly IEventAggregator _events;

        public ShellViewModel(SimpleContainer container, SalesViewModel salesViewModel, IEventAggregator events)
        {
            _container = container;
            _salesViewModel = salesViewModel;
            _events = events;
            _events.Subscribe(this);

            ActivateItem(_container.GetInstance<LoginViewModel>());
        }

        public void Handle(LogOnEvent message)
        {
            ActivateItem(_salesViewModel);
            //var loginViewModel = _container.GetInstance<LoginViewModel>();
        }
    }
}
