using System;

namespace Extensions
{
    public static class TypeExtensions
    {
        public static bool IsSameOrSubclass(this Type potentialDescendant, Type potentialBase)
        {
            return potentialDescendant.IsSubclassOf(potentialBase)
                   || potentialDescendant == potentialBase;
        }
    }
}