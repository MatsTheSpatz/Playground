using System;
using System.Configuration;
using System.Net;

namespace RecipeOcr
{
    public interface IOcrServiceWrapper
    {
        Task ProcessImage(byte[] data, RecognitionLanguage language);

        Task GetTaskStatus(Task taskId);

        string DownloadResult(Task task);
    }


    public class ServiceWrapper : IOcrServiceWrapper
    {
        private const bool UseZuehlkeProxy = true;

        private static readonly ServiceWrapper _instance;
        private readonly OcrRestServiceClient _restServiceClient;

        static ServiceWrapper()
        {
            _instance = new ServiceWrapper();
        }

        public static IOcrServiceWrapper Instance
        {
            get { return _instance; }
        }

        private ServiceWrapper()
        {
            ServicePointManager.Expect100Continue = false;

            var config = (OcrConfigurationSection)ConfigurationManager.GetSection("recipe.ocr");

            _restServiceClient = new OcrRestServiceClient
                                     {
                                         ServerUrl = config.ServerUrl,
                                         ApplicationId = config.ApplicationId,
                                         Password = config.Password
                                     };

            if (!String.IsNullOrEmpty(config.Proxy))
            {
                string proxyAddress = config.Proxy.Split(':')[0];
                int proxyPort = int.Parse(config.Proxy.Split(':')[1]);

                IWebProxy proxyObject = new WebProxy(proxyAddress, proxyPort);
                _restServiceClient.Proxy = proxyObject;

            }

            //_restServiceClient = new OcrRestServiceClient
            //                         {
            //                             ServerUrl = @"http://cloud.ocrsdk.com",
            //                             ApplicationId = "RecipeReader",
            //                             Password = "aFkLQq1bMqhTkq8v2W812KUU"
            //                         };

            //if (UseZuehlkeProxy)
            //{
            //    IWebProxy proxyObject = new WebProxy("proxy.zuehlke.com", 8080);
            //    _restServiceClient.Proxy = proxyObject;
            //}
        }

        public Task ProcessImage(byte[] data, RecognitionLanguage language)
        {
            var settings = new ProcessingSettings
            {
                Language = language,
                OutputFormat = OutputFormat.txt,
                TextTypes = TextType.Normal
            };

            // set request
            Task task = _restServiceClient.ProcessImage(data, settings);
            return task;
        }

        public Task GetTaskStatus(Task task)
        {
            Task newTask = _restServiceClient.GetTaskStatus(task.Id);
            return newTask;
        }

        public string DownloadResult(Task task)
        {
            return _restServiceClient.DonwloadTextResult(task);
        }
    }
}
