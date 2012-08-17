using System;
using System.IO;
using System.Net;
using System.Xml;
using System.Xml.Linq;

namespace RecipeOcr
{
    public class OcrRestServiceClient
    {
        /// <summary>
        /// Address of the server excluding protocol
        /// </summary>
        private string _serverAddress;

        public OcrRestServiceClient()
        {
            ServerUrl = "http://cloud.ocrsdk.com/";
            IsSecureConnection = false;
            Proxy = WebRequest.DefaultWebProxy;
        }

        /// <summary>
        /// Url of the server
        /// On set, IsSecureConnection property is changed url contains protocol (http:// or https://)
        /// </summary>
        public string ServerUrl
        {
            get
            {
                return (IsSecureConnection ? "https://" : "http://") + _serverAddress;
            }
            set
            {
                if (value.StartsWith("http://", StringComparison.InvariantCultureIgnoreCase))
                {
                    IsSecureConnection = false;
                }
                else if (value.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase))
                {
                    IsSecureConnection = true;
                }

                // Trim http(s):// from the beginning
                _serverAddress = System.Text.RegularExpressions.Regex.Replace(value, "^https?://", "");
            }
        }

        public string ApplicationId { get; set; }

        public string Password { get; set; }

        public IWebProxy Proxy { get; set; }

        /// <summary>
        /// Does the connection use SSL or not. Set this property after ServerUrl
        /// </summary>
        public bool IsSecureConnection { get; set; }

        /// <summary>
        /// Upload a file to service synchronously and start processing
        /// </summary>
        /// <param name="filePath">Path to an image to process</param>
        /// <param name="settings">Language and output format</param>
        /// <returns>Id of the task. Check task status to see if you have enough units to process the task</returns>
        /// <exception cref="ProcessingErrorException">thrown when something goes wrong</exception>
        public Task ProcessImage(byte[] data, ProcessingSettings settings)
        {
            string url = String.Format("{0}/processImage?{1}", ServerUrl, settings.AsUrlParams);

            if (!String.IsNullOrEmpty(settings.Description))
            {
                url = url + "&description=" + Uri.EscapeDataString(settings.Description);
            }

            try
            {
                // Build post request
                WebRequest request = WebRequest.Create(url);
                SetupPostRequest(url, request);
                WriteFileToRequest(data, request);

                XDocument response = PerformRequest(request);
                Task task = ServerXml.GetTaskStatus(response);
                return task;
            }
            catch (WebException e)
            {
                String friendlyMessage = retrieveFriendlyMessage(e);
                if (friendlyMessage != null)
                {
                    throw new ProcessingErrorException(friendlyMessage, e);
                }
                throw new ProcessingErrorException("Cannot upload file", e);
            }
        }

        /// <summary>
        /// Upload a file to service synchronously and start processing
        /// </summary>
        /// <param name="filePath">Path to an image to process</param>
        /// <param name="settings">Language and output format</param>
        /// <returns>Id of the task. Check task status to see if you have enough units to process the task</returns>
        /// <exception cref="ProcessingErrorException">thrown when something goes wrong</exception>
        public Task ProcessImage(string filePath, ProcessingSettings settings)
        {
            string url = String.Format("{0}/processImage?{1}", ServerUrl,  settings.AsUrlParams);

            if (!String.IsNullOrEmpty(settings.Description))
            {
                url = url + "&description=" + Uri.EscapeDataString(settings.Description);
            }

            try
            {
                // Build post request
                WebRequest request = WebRequest.Create(url);
                SetupPostRequest(url, request);
                WriteFileToRequest(filePath, request);

                XDocument response = PerformRequest(request);
                Task task = ServerXml.GetTaskStatus(response);
                return task;
            }
            catch (WebException e )
            {
                String friendlyMessage = retrieveFriendlyMessage( e );
				if (friendlyMessage != null)
				{
					throw new ProcessingErrorException(friendlyMessage, e);
				}
				throw new ProcessingErrorException("Cannot upload file", e);
            }
        }

        private string retrieveFriendlyMessage( System.Net.WebException fromException )
        {
            try
            {
                using (HttpWebResponse result = (HttpWebResponse)fromException.Response)
                {
                    // try extract the user-friendly text that might have been supplied
                    // by the service.
                    try
                    {
                        using (Stream stream = result.GetResponseStream())
                        {
                            XDocument responseXml = XDocument.Load( new XmlTextReader( stream ) );
                            XElement messageElement = responseXml.Root.Element("message");
                            String serviceMessage = messageElement.Value;
                            if (!String.IsNullOrEmpty(serviceMessage))
                            {
                                return serviceMessage;
                            }
                        }
                    } catch
                    {
                    }
                    try
                    {
                        String protocolMessage = result.StatusDescription;
                        if (!String.IsNullOrEmpty(protocolMessage))
                        {
                            return protocolMessage;
                        }
                    }
                    catch
                    {
                    }
                }
            } catch
            {
            }
            return null;
        }

        public string DonwloadTextResult(Task task)
        {
            if (task.Status != TaskStatus.Completed)
            {
                throw new ArgumentException("Cannot download result for not completed task");
            }

            if (task.DownloadUrl == null)
            {
                throw new ArgumentException("Cannot download task without download url");
            }

            try
            {
                string url = task.DownloadUrl;

                WebRequest request = WebRequest.Create(url);
                SetupGetRequest(url, request);

                using (var result = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream stream = result.GetResponseStream())
                    {
                        var reader = new StreamReader(stream);
                        string text = reader.ReadToEnd();
                        return text;
                    }
                }
            }
            catch (WebException e)
            {
                throw new ProcessingErrorException(e.Message, e);
            }
        }
        /// <summary>
        /// Download filePath that has finished processing and save it to given path
        /// </summary>
        /// <param name="task">Id of a task</param>
        /// <param name="outputFile">Path to save a filePath</param>
        public void DownloadResult(Task task, string outputFile)
        {
            if (task.Status != TaskStatus.Completed)
            {
                throw new ArgumentException("Cannot download result for not completed task");
            }

            try
            {
                if (File.Exists(outputFile))
                    File.Delete(outputFile);

               
                if (task.DownloadUrl == null)
                {
                    throw new ArgumentException("Cannot download task without download url");
                }

                string url = task.DownloadUrl;

                // Emergency code. In normal situations it shouldn't be called
                /*
                url = String.Format("{0}/{1}?TaskId={2}", ServerUrl, _getResultUrl,
                    Uri.EscapeDataString(task.Id.ToString()));
                 */

                WebRequest request = WebRequest.Create(url);
                SetupGetRequest(url, request);

                using (HttpWebResponse result = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream stream = result.GetResponseStream())
                    {
                        // Write result directly to file
                        using (Stream file = File.OpenWrite(outputFile))
                        {
                            CopyStream(stream, file);
                        }
                    }
                }
            }
            catch (WebException e)
            {
                throw new ProcessingErrorException(e.Message, e);
            }
        }

        public Task GetTaskStatus(TaskId task)
        {
            string url = String.Format("{0}/getTaskStatus?TaskId={1}", ServerUrl, Uri.EscapeDataString(task.ToString()));

            WebRequest request = WebRequest.Create(url);
            SetupGetRequest(url, request);
            XDocument response = PerformRequest(request);
            Task serverTask = ServerXml.GetTaskStatus(response);
            return serverTask;
        }

        /// <summary>
        /// List all tasks modified within last 7 days
        /// </summary>
        public Task[] ListTasks()
        {
            DateTime now = DateTime.UtcNow;
            return ListTasks(now.AddDays(-7));
        }

        /// <summary>
        /// List all tasks which status changed since given UTC timestamp
        /// </summary>
        public Task[] ListTasks( DateTime changedSince )
        {
            string url = String.Format("{0}/listTasks?fromDate={1}", ServerUrl, 
                Uri.EscapeDataString(changedSince.ToUniversalTime().ToString("s")+"Z"));

            WebRequest request = WebRequest.Create(url);
            SetupGetRequest(url, request);
            XDocument response = PerformRequest(request);

            Task[] tasks = ServerXml.GetAllTasks(response);
            return tasks;
        }

        /// <summary>
        /// Get list of tasks that are no more queued on a server.
        /// The tasks can be processed, failed, or not started becuase there is 
        /// not enough credits to process them.
        /// </summary>
        public Task[] ListFinishedTasks()
        {
            string url = String.Format("{0}/listFinishedTasks", ServerUrl);
            WebRequest request = WebRequest.Create(url);
            SetupGetRequest(url, request);
            XDocument response = PerformRequest(request);

            Task[] tasks = ServerXml.GetAllTasks(response);
            return tasks;
        }

        /// <summary>
        /// Delete task on a server. This function cannot delete tasks that are being processed.
        /// </summary>
        public Task DeleteTask(Task task)
        {
            switch (task.Status)
            {
                case TaskStatus.Deleted:
                case TaskStatus.InProgress:
                case TaskStatus.Unknown:
                    throw new ArgumentException("Invalid task status: " + task.Status + ". Cannot delete");
            }

            string url = String.Format("{0}/deleteTask?TaskId={1}", ServerUrl, Uri.EscapeDataString(task.Id.ToString()));
            WebRequest request = WebRequest.Create(url);
            SetupGetRequest(url, request);

            XDocument response = PerformRequest(request);
            Task serverTask = ServerXml.GetTaskStatus(response);
            return serverTask;
        }

        private static void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[8 * 1024];
            int len;
            while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, len);
            }
        }

        private XDocument PerformRequest(WebRequest request)
        {
            using (var result = (HttpWebResponse)request.GetResponse())
            {
                using (Stream stream = result.GetResponseStream())
                {
                    return XDocument.Load(new XmlTextReader(stream));
                }
            }
        }

        private void SetupRequest(string serverUrl, WebRequest request)
        {
            if (Proxy != null)
                request.Proxy = Proxy;

            // Support authentication in case url is ABBYY SDK
            if (serverUrl.StartsWith(ServerUrl, StringComparison.InvariantCultureIgnoreCase))
            {
                request.Credentials = new NetworkCredential(ApplicationId, Password);
            }

            // Set user agent string so that server is able to collect statistics
            ((HttpWebRequest)request).UserAgent = ".Net Cloud OCR SDK client";
        }

        private void SetupPostRequest(string serverUrl, WebRequest request)
        {
            SetupRequest(serverUrl, request);
            request.Method = "POST";
            request.ContentType = "application/octet-stream";
        }

        private void SetupGetRequest(string serverUrl, WebRequest request)
        {
            SetupRequest(serverUrl, request);
            request.Method = "GET";
        }

        private void WriteFileToRequest(byte[] data, WebRequest request)
        {
            request.ContentLength = data.Length;
            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
        }

        private void WriteFileToRequest( string filePath, WebRequest request )
		{
			using( BinaryReader reader = new BinaryReader( File.OpenRead( filePath ) ) ) {
				request.ContentLength = reader.BaseStream.Length;
				using( Stream stream = request.GetRequestStream() ) {
					byte[] buf = new byte[reader.BaseStream.Length];
					while( true ) {
						int bytesRead = reader.Read( buf, 0, buf.Length );
						if( bytesRead == 0 ) {
							break;
						}
						stream.Write( buf, 0, bytesRead );
					}
				}
			}
        }
    }
}
