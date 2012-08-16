using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace RecipeOcr
{
    static class ServerXml
    {
        public static TaskId GetTaskId(XDocument xml)
        {
            string id = xml.Root.Element("task").Attribute("id").Value;
            return new TaskId(id);
        }

        public static Task GetTaskStatus(XDocument xml)
        {
            return GetTaskInfo(xml.Root.Element("task"));
        }

        public static Task[] GetAllTasks(XDocument xml)
        {
            var result = new List<Task>();
            XElement xResponse = xml.Root;
            foreach (XElement xTask in xResponse.Elements("task"))
            {
                Task task = GetTaskInfo(xTask);
                result.Add(task);
            }

            return result.ToArray();
        }

        private static TaskStatus StatusFromString(string status)
        {
            switch (status.ToLower())
            {
                case "submitted":
                    return TaskStatus.Submitted;
                case "queued":
                    return TaskStatus.Queued;
                case "inprogress":
                    return TaskStatus.InProgress;
                case "completed":
                    return TaskStatus.Completed;
                case "processingfailed":
                    return TaskStatus.ProcessingFailed;
                case "deleted":
                    return TaskStatus.Deleted;
                case "notenoughcredits":
                    return TaskStatus.NotEnoughCredits;
                default:
                    return TaskStatus.Unknown;
            }
        }
        /// <summary>
        /// Get task data from xml node "task"
        /// </summary>
        private static Task GetTaskInfo(XElement xTask)
        {
            TaskId id = new TaskId(xTask.Attribute("id").Value);
            TaskStatus status = StatusFromString(xTask.Attribute("status").Value);

            Task task = new Task();
            task.Id = id;
            task.Status = status;

            XAttribute xRegistrationTime = xTask.Attribute("registrationTime");
            if (xRegistrationTime != null)
            {
                DateTime time;
                if (DateTime.TryParse(xRegistrationTime.Value, out time))
                    task.RegistrationTime = time;
            }

            XAttribute xStatusChangeTime = xTask.Attribute("statusChangeTime");
            if (xStatusChangeTime != null)
            {
                DateTime time;
                if (DateTime.TryParse(xStatusChangeTime.Value, out time))
                    task.StatusChangeTime = time;
            }

            XAttribute xPagesCount = xTask.Attribute("filesCount");
            if (xPagesCount != null)
            {
                int pagesCount;
                if (Int32.TryParse(xPagesCount.Value, out pagesCount))
                    task.PagesCount = pagesCount;
            }

            XAttribute xCredits = xTask.Attribute("credits");
            if (xCredits != null)
            {
                int credits;
                if( Int32.TryParse( xCredits.Value, out credits ))
                    task.Credits = credits;
            }

            XAttribute xDescription = xTask.Attribute("description");
            if (xDescription != null)
                task.Description = xDescription.Value;

            XAttribute xResultUrl = xTask.Attribute("resultUrl");
            if (xResultUrl != null)
            {
                task.DownloadUrl = xResultUrl.Value;
            }

            return task;
        }
    }
}
