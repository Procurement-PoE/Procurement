using System.Collections.Generic;

namespace POEApi.Model
{
    public class ImageComparer : IEqualityComparer<Item>
    {
        public bool Equals(Item x, Item y)
        {
            if (x.GetHashCode() == y.GetHashCode())
                return true;

            return x.IconURL.Equals(y.IconURL);
        }

        public int GetHashCode(Item obj)
        {
            return obj.GetHashCode();
        }
    }
}
