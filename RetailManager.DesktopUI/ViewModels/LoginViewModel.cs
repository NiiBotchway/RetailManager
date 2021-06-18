using Caliburn.Micro;
using RetailManager.DesktopUI.EventModels;
using RetailManager.DesktopUI.Library.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailManager.DesktopUI.ViewModels
{
    public class LoginViewModel : Screen
    {
        private readonly IApiHelper _apiHelper;
        private readonly IEventAggregator _events;
        private string _userName;
        private string _password;
        private string _errorMessage;
        private string _successMessage;

        public LoginViewModel(IApiHelper apiHelper, IEventAggregator events)
        {
            _apiHelper = apiHelper;
            _events = events;
        }

        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                NotifyOfPropertyChange(() => UserName);
                NotifyOfPropertyChange(() => CanLogin);
            }
        }

        public bool IsErrorVisible
        {
            get
            {
                return ErrorMessage?.Length > 0;
            }
        }

        public bool IsSuccessVisible
        {
            get
            {
                return SuccessMessage?.Length > 0;
            }
        }

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                NotifyOfPropertyChange(() => ErrorMessage);
                NotifyOfPropertyChange(() => IsErrorVisible);
            }
        }


        public string SuccessMessage
        {
            get { return _successMessage; }
            set
            {
                _successMessage = value;
                NotifyOfPropertyChange(() => SuccessMessage);
                NotifyOfPropertyChange(() => IsSuccessVisible);
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                NotifyOfPropertyChange(() => Password);
                NotifyOfPropertyChange(() => CanLogin);
            }
        }

        public bool CanLogin
        {
            get
            {
                return ((UserName?.Length ?? 0) > 0 && (Password?.Length ?? 0) > 0);
            }

        }

        public async Task Login(string username, string password)
        {
            try
            {
                ErrorMessage = string.Empty;
                SuccessMessage = string.Empty;
                var result = await _apiHelper.Authenticate(UserName, Password);
                if (result.Access_Token != null)
                {
                    var info = _apiHelper.GetLoggedInUserInfo();
                    SuccessMessage = "Login Successful";

                    _events.PublishOnUIThread(new LogOnEvent());
                }
                else
                {
                    ErrorMessage = "Wrong username or password";
                }
            }
            catch (Exception)
            {
                ErrorMessage = $"Unknown Error Occured. \nPlease contact admin";

            }
        }
    }
}
