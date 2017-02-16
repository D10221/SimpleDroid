using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;
using NLog;
using Square.OkHttp3;

namespace SimpleDroid.Services.Remote
{
    public class NetServiceRunner : INetServiceRunner
    {
        private readonly INetService _netService;

        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public NetServiceRunner(INetService netService)
        {
            _netService = netService;
        }

        public async Task<T> Invoke<T>(INetActionConfig action)
        {
            try
            {
                using (var requestBuilder = new Request.Builder())
                {
                    if (_netService.Config.HasCredentials)
                    {
                        requestBuilder.AddHeader("Authentication",
                            Credentials.Basic(_netService.Config.UserName, _netService.Config.Password ?? ""));
                    }

                    requestBuilder.Url(GetUri(action));

                    if (action.Payload != null)
                        requestBuilder.Post(
                            GetBody(action));

                    using (var request = requestBuilder.Build())
                    {
                        using (var clientBuilder = new OkHttpClient.Builder())
                        {
                            // Setup Client here
                            using (var client = clientBuilder.Build())
                            {
                                using (var response = await client.NewCall(request).ExecuteAsync().ConfigureAwait(false)
                                )
                                {
                                    if (!response.IsSuccessful)
                                    {
                                        throw new Exception($"Error: code:{response.Code()}, msg:{response.Message()}");
                                    }

                                    using (var contentType = response.Body().ContentType())
                                    {
                                        _logger.Info(contentType);

                                        if (contentType.Type().EndsWith("xml"))
                                        {
                                            var body = await response.Body().StringAsync();
                                            return Deserialize<T>(body);
                                        }

                                        if (contentType.Type().EndsWith("json"))
                                        {
                                            return JsonConvert.DeserializeObject<T>(
                                                await response.Body().StringAsync());
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _logger.Error(e);
            }

            return default(T);
        }

        private string GetUri(INetActionConfig action)
        {
            return $"{_netService.Config.ServiceBaseUri}/{action.MethodName}{AsString(action.Parameters)}";
        }

        private static RequestBody GetBody(INetActionConfig action)
        {
            return RequestBody.Create(MediaType.Parse(action.PayloadType),
                JsonConvert.SerializeObject(action.Payload));
        }

        private string AsString(object parameters)
        {
            return parameters == null ? "" :
                "?" +
                SString(parameters.GetType().GetProperties().ToDictionary(x => x.Name, x => x.GetValue(parameters)));
        }

        private string SString(Dictionary<string, object> d)
        {
            return d.Select(x => $"{x.Key}={x.Value}").Aggregate(Concat("&"));
        }

        private Func<string, string, string> Concat(string separator)
        {
            return (a, b) => a + separator + b;
        }

        private static T Deserialize<T>(string body)
        {
            return (T)new DataContractSerializer(typeof(T)).ReadObject(new XmlTextReader(body));
        }
    }
}