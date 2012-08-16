
using System;
using System.Threading;
using RecipeOcr;
using RecipeWebRole.Models;

namespace RecipeWebRole.DataAccess
{
    /// <summary>
    /// Optical Character Recoginition for an image
    /// </summary>
    public interface IOcrService
    {
        /// <summary>
        /// Processes the image.
        /// This is a BLOCKING CALL until the process ends.
        /// </summary>
        void ProcessImage(Image image);

        bool IsOcrFinished(int imageId);

        string GetOcrResult(int imageId);        
    }
}