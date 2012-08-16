using System;

namespace RecipeOcr
{
    public class TaskId : IEquatable<TaskId>
    {
        private readonly string _id;
        
        public TaskId(string id)
        {
            _id = id;
        }

        public override string ToString()
        {
            return _id;
        }

        public override int GetHashCode()
        {
            return _id.GetHashCode();
        }

        public bool Equals(TaskId b)
        {
            return b._id == _id;
        }
    }
}