using System;
using System.Collections.Generic;
using System.Threading;
using RecipeOcr;
using RecipeWebRole.Models;

namespace RecipeWebRole.DataAccess
{
    //if (!_imageProcessingTimes.ContainsKey(imageId))
    //{
    //    _imageProcessingTimes.Add(imageId, DateTime.Now);
    //}

    //DateTime startProcessingTime = _imageProcessingTimes[imageId];
    //bool isComplete = (DateTime.Now - startProcessingTime).TotalMilliseconds > 4000;
    //private static Dictionary<int, DateTime> _imageProcessingTimes = new Dictionary<int, DateTime>();

    public class OcrService : IOcrService
    {
        private static readonly Dictionary<int, Task> _imageOcrTasks = new Dictionary<int, Task>();
        private static readonly Dictionary<int, string> _imageOcrResults = new Dictionary<int, string>();
        
        public void ProcessImage(Image image)
        {
            // record that processing started.
            _imageOcrTasks.Add(image.Id, null);

            // upload image to OCR-Server.
            Task task = ServiceWrapper.Instance.ProcessImage(image.Data, RecognitionLanguage.German);
            _imageOcrTasks[image.Id] = task;

            // start polling for result.
            do
            {
                Thread.Sleep(1000);
                task = ServiceWrapper.Instance.GetTaskStatus(task);

                if (task.Status == TaskStatus.Completed)
                {
                    // download and persist image.
                    string text = ServiceWrapper.Instance.DownloadResult(task);
                    _imageOcrResults[image.Id] = text;
                }

                _imageOcrTasks[image.Id] = task;

            } while (task.IsTaskActive());
        }

        public string GetOcrResult(int imageId)
        {
            if (!IsOcrFinished(imageId))
            {
                throw new InvalidOperationException("OCR-process must be finished before querying for result");
            }

            if (_imageOcrResults.ContainsKey(imageId))
            {
                return _imageOcrResults[imageId];
            }

            return null;
        }

        public bool IsOcrFinished(int imageId)
        {
            if (_imageOcrResults.ContainsKey(imageId))
            {
                return true;
            }

            if (_imageOcrTasks.ContainsKey(imageId))
            {
                Task task = _imageOcrTasks[imageId];
                if (task == null)
                {
                    return false; // not even started.
                }
                return !task.IsTaskActive();
            }

            throw new ArgumentException("No information about OCR status for given image");
        }
    }
}
