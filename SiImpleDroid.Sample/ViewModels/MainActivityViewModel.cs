// ReSharper disable ClassNeverInstantiated.Global

using System.Collections.Generic;
using System.Collections.ObjectModel;
using SimpleDroid.Services;
using SimpleDroid.Services.Remote;

namespace SimpleDroid.ViewModels
{
    public class MainActivityViewModel: ViewModelBase
    {
        private string _toolbarTitle;

        public string ToolbarTitle
        {
            get { return _toolbarTitle; }
            set
            {
                if (_toolbarTitle == value) return;
                _toolbarTitle = value;
                RaisePropertyChanged();
            }
        }

        public MainActivityViewModel(ISettingService settingService)
        {
            ToolbarTitle = "Simple Droid";

            var settings = settingService.GetFirst();

            var userName = string.IsNullOrWhiteSpace(settings.UserName)? null : settings.UserName;

            var servicePort = (settings.ServicePort == 0 ? 80 : settings.ServicePort);

            NetServices = new ObservableCollection<INetService>
            {
                new NetService(
                    
                    new NetServiceConfig
                    {
                        ServiceName = "TestService",
                        ServiceHost = settings.ServiceHost,
                        ServicePort = servicePort.ToString(),
                        UserName = userName,
                        Password = settings.Password,
                        NameSpace = "FreedomService", 
                    },
                    new List<INetActionConfig>()
                    {
                        new NetActionConfig
                        {
                            MethodName = "Echo",
                            Parameters = null,
                            Payload = null,
                            PayloadType = "application/json"
                        }
                    }),                            
            };
        }

        public ObservableCollection<INetService> NetServices { get; set; }
    }
}