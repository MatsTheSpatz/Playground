using System;

namespace RecipeOcr
{
    public class Task
    {
        public Task()
        {
            Status = TaskStatus.Unknown;
            Id = new TaskId("<unknown>");
        }

        public Task(TaskId id, TaskStatus status)
        {
            Id = id;
            Status = status;
        }

        /// <summary>
        /// Id
        /// </summary>
        public TaskId Id;

        /// <summary>
        /// Status
        /// </summary>
        public TaskStatus Status;

        /// <summary>
        /// When task was created. Can be null if no information
        /// </summary>
        public DateTime RegistrationTime;

        /// <summary>
        /// Last activity time. Can be null if no information
        /// </summary>
        public DateTime StatusChangeTime;

        /// <summary>
        /// Number of pages in task
        /// </summary>
        public int PagesCount = 1;

        /// <summary>
        /// Task cost in credits
        /// </summary>
        public int Credits;

        /// <summary>
        /// Task description provided by user
        /// </summary>
        public string Description;

        /// <summary>
        /// Url to download processed tasks
        /// </summary>
        public string DownloadUrl;
        
        public bool IsTaskActive()
        {
            return IsTaskActive(Status);
        }

        // Task is submitted or is processing
        public static bool IsTaskActive( TaskStatus status ) 
        {
            switch (status)
            {
                case TaskStatus.Submitted:
                case TaskStatus.Queued:
                case TaskStatus.InProgress:
                    return true;

                default:
                    return false;
            }
        }
    }
}