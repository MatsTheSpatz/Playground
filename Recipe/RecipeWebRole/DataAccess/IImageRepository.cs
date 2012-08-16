using System;
using System.Collections.Generic;
using RecipeWebRole.Models;

namespace RecipeWebRole.DataAccess
{
    public interface IImageRepository
    {
        void SetImage(Image image);

        Image GetImage(int id);

        IList<int> GetImageIds();
    }
}
