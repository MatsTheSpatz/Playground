using System;
using System.Collections.Generic;
using System.Threading;
using RecipeWebRole.Models;

namespace RecipeWebRole.DataAccess
{
    public class MockOcrService : IOcrService
    {
        private static Dictionary<int, DateTime> _imageProcessingTimes = new Dictionary<int, DateTime>();
        private static Dictionary<int, string> _imageResults = new Dictionary<int, string>();

        public void ProcessImage(Image image)
        {
            _imageProcessingTimes[image.Id] = DateTime.Now;

            Thread.Sleep(2500);

            _imageResults[image.Id] = "Hier ist der Text des Ocr Resultes aber es ist nur ein Mock";
        }

        public string GetOcrResult(int imageId)
        {
            if (!IsOcrFinished(imageId))
            {
                throw new InvalidOperationException("OCR-process must be finished before querying for result");
            }

            if (_imageResults.ContainsKey(imageId))
            {
                return _imageResults[imageId];
            }

            return null;
        }

        public bool IsOcrFinished(int imageId)
        {
            if (_imageResults.ContainsKey(imageId))
            {
                return true;
            }

            if (_imageProcessingTimes.ContainsKey(imageId))
            {
                return false;
            }

            throw new ArgumentException("No information about OCR status for given image");
        }
    }
}
