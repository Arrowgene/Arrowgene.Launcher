using Arrowgene.Launcher.Http;
using Arrowgene.Launcher.Json;
using System;

namespace Arrowgene.Launcher.Api
{
    public class ArrowgeneApi
    {
        private const string API_VERSION_URL = "https://arrowgene.net/api/v1/info/version";

        public ArrowgeneApi()
        {

        }

        public void Version(Action<ApiVersion> action)
        {
            HttpRequest request = new HttpRequest();
            request.ResponseAction = (AsyncHttpResponseEventArgs result) =>
            {
                string json = HttpRequest.GetRequestContent(result.Response);
                Response<ApiVersion> response = JsonSerializer.Deserialize<Response<ApiVersion>>(json);
                if (response != null)
                {
                    if (!response.IsError)
                    {
                        action.Invoke(response.Content);
                        return;
                    }
                    App.Logger.Log(response.Message, "ArrowgeneApi::Version");
                }
                else
                {
                    App.Logger.Log("Couldn't get version", "ArrowgeneApi::Version");
                }
                action.Invoke(null);
            };
            request.RequestAsync(API_VERSION_URL);
        }

    }
}
