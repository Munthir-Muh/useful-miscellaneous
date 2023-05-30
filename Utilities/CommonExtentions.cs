using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Utilities.Extentions
{
    public static class CommonExtentions
    {
        /// <summary>
        /// Check if the underlaying object is Collection or not
        /// </summary>
        /// <param name="obj">the underlaying object</param>
        /// <param name="ignoreString">optional parameter, if set to false string object will be considered as collection.Default value is true</param>
        /// <returns>returns true if the object is collection otherwise returns false</returns>
        public static bool IsCollection<T>(this T obj, bool ignoreString = true)
        {
           if(obj.GetType() == typeof(string) && !ignoreString)
                  return true;

           return (obj as IEnumerable) != null;
        }
        /// <summary>
        /// Check if the underlaying Type is Collection or not
        /// </summary>
        /// <param name="type">the underlaying Type</param>
        /// <param name="ignoreString">optional parameter, if set to false string type will be considered as collection.Default value is true</param>
        /// <returns>returns true if the type is collection otherwise returns false</returns>
        public static bool IsCollection(this Type type, bool ignoreString = true) =>
            type.GetInterfaces().Any(s => s == typeof(IEnumerable)) && (ignoreString ? type != typeof(string) : !ignoreString);

        /// <summary>
        /// Try Get any public properties of the underlaying object type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target">the object we want to try to get its properties</param>
        /// <param name="properties">out parameter of the properties that will be found</param>
        /// <returns>returns false if the type is primitive or does not has public properties, otherwise returns true. </returns>
        public static bool TryGetProperties<T>(this T target, out PropertyInfo[] properties)
        {
            var type = target.GetType();

            properties = type.IsPrimitive || type == typeof(string) ?
                null : type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            return properties != null;
        }
        /// <summary>
        /// Transform any object to IEnumerable Collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object we need to Transform to collection</param>
        /// <param name="isCollection">Out parameter will be true if the object is already a collection; otherwise, is false</param>
        /// <param name="ignoreString">if set false, string will be considered as IEnumerable and will be returned as it is; otherwise, it will return IEnumerable<string>. the default value is true. </param>
        /// <returns>return IEnum</returns>
        public static IEnumerable ToEnumerable<T>(this T obj, out bool isCollection, bool ignoreString = true)
        {
            isCollection = false;

            if (obj.IsCollection(ignoreString))
            {
                isCollection = true;
                return obj as IEnumerable;
            }

            return Enumerable.Repeat(obj, 1);
        }
        /// <summary>
        /// Convert any collection into a paginated collection that allows you to navigate through it as pages using methods such as
        /// <see cref="PaginatedCollection{T}.GoToPageNo(int)"/>, <see cref="PaginatedCollection{T}.NextPage"/>, <see cref="PaginatedCollection{T}.PreviousPage"/>..etc
        /// </summary>
        public static PaginatedCollection<T> ToPaginatedCollection<T>(this IEnumerable<T> collection, int pageSize)
        {
            return new PaginatedCollection<T>(collection, pageSize);
        }
        /// <summary>
        /// Return true if the underlying value exists within a list of values; otherwise, return false. This is similar to the SQL operator ‘IN’.
        /// </summary>
        public static bool InValues<T>(this T current, params T[] values)
        {
            if (values != null)
            {
                return values.Contains(current);
            }

            return false;
        }
        /// <summary>
        /// Compare two values against target value and return true if the target value is between them; otherwise, return false.
        /// </summary>
        public static bool Between<T>(this T current, T leftSide, T rightSide) where T : IComparable<T>
        {
            return current.CompareTo(leftSide) >= 0 && current.CompareTo(rightSide) <= 0;
        }

        public static bool GreaterThan<T>(this T left, T right) where T : IComparable<T> =>
            left.CompareTo(right) > 0;

        public static bool LessThan<T>(this IComparable<T> left, T right) where T : IComparable<T> =>
            left.CompareTo(right) < 0;
        
        public static bool Equal<T>(this T left, T right) where T : IComparable<T> => 
            left.CompareTo(right) == 0;
        public static bool LessThanOrEqual<T>(this T left, T right) where T : IComparable<T> =>
            left.CompareTo(right) <= 0;

        public static bool GreaterThanOrEqual<T>(this T left, T right) where T : IComparable<T> => 
            left.CompareTo(right) >= 0;
    }
}
