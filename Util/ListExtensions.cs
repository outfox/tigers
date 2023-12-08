//SPDX-License-Identifier: Unlicense

using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace Tiger.Util
{
    public static class ListExtensions
    {
        /// <summary>Returns a random element from the list</summary>
        public static T Pick<T>(this IList<T> list) => list.Count < 1 ? default : list[Random.Range(0, list.Count)];

        /// <summary>Returns the first element from the list</summary>
        public static T First<T>(this IList<T> list) => list.Count < 1 ? default : list[0];

        /// <summary>Returns the last element from the list</summary>
        public static T Last<T>(this IList<T> list) => list.Count < 1 ? default : list[^1];

        /// <summary>Returns and removes the last element from the list</summary>
        public static T TakeLast<T>(this IList<T> list)
        {
            if (list.Count < 1) return default;
            var element = list[^1];
            list.RemoveAt(list.Count - 1);
            return element;
        }

        /// <summary>Returns and removes the first element from the list</summary>
        public static T Take<T>(this IList<T> list)
        {
            var element = list[0];
            list.RemoveAt(0);
            return element;
        }

        /// <summary>Returns and removes a random element from the list</summary>
        public static T Pluck<T>(this IList<T> list)
        {
            var i = Random.Range(0, list.Count);
            var element = list[i];
            list.RemoveAt(i); //TODO: This is slow, use a swap and pop
            return element;
        }

        /// <summary>Returns the first element from the list and cycles it to its end</summary>
        public static T Shift<T>(this IList<T> list)
        {
            if (list.Count < 1) return default;            
            var a = list[0];
            list.RemoveAt(0);
            list.Add(a);

            return a;
        }

        /// <summary>Creates a fully random permutation. (Fisher-Yates Shuffle)</summary>
        public static void Shuffle<T>(this IList<T> list)
        {
            //Fisher-Yates Algorithm
            var n = list.Count;
            for (var i = 0; i < n - 1; i++)
            {
                var j = Random.Range(i, n);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }

        /// <summary>Creates a derangement, i.e. a permutation with each element in a new position. (Sattolo's Algorithm)</summary>
        public static void Derange<T>(this IList<T> list)
        {
            var n = list.Count;
            for (var i = 0; i < n - 1; i++)
            {
                var j = Random.Range(i + 1, n);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }

        /// <summary>Reverses the list in-situ</summary>
        public static void Reverse<T>(this IList<T> list)
        {
            var n = list.Count;
            for (var i = 0; i < n / 2; i++)
            {
                var j = n - 1 - i;
                (list[i], list[j]) = (list[j], list[i]);
            }
        }

        /// <summary>Returns a random element from the array</summary>
        public static T Pick<T>(this T[] array) => array.Length < 1 ? default : array[Random.Range(0, array.Length)];

        /// <summary>Returns the first element from the array</summary>
        public static T First<T>(this T[] array) => array.Length < 1 ? default : array[0];

        /// <summary>Returns the last element from the array</summary>
        public static T Last<T>(this T[] array) => array.Length < 1 ? default : array[^1];

        /// <summary>Returns the first element from the array and cycles it to its end</summary>
        public static T Shift<T>(this T[] array)
        {
            if (array.Length < 1) return default;            
            var a = array[0];

            Array.Copy(array, 1, array, 0, array.Length-1);
            array[array.Length-1] = a;
            
            return a;
        }

        /// <summary>Creates a fully random permutation.</summary>
        public static void Shuffle<T>(this T[] array)
        {
            //Fisher-Yates Algorithm
            var n = array.Length;
            for (var i = 0; i < n - 1; i++)
            {
                var j = Random.Range(i, n);
                (array[i], array[j]) = (array[j], array[i]);
            }
        }

        /// <summary>Creates a derangement, i.e. a permutation with each element in a new position</summary>
        public static void Derange<T>(this T[] array)
        {
            //Sattolo's Algorithm
            var n = array.Length;
            for (var i = 0; i < n - 1; i++)
            {
                var j = Random.Range(i + 1, n);
                (array[i], array[j]) = (array[j], array[i]);
            }
        }

        /// <summary>Reverses the array in-situ</summary>
        public static void Reverse<T>(this T[] array) => Array.Reverse(array);
        
        /// <summary>Sets all entries in array to the given value</summary>
        public static void Fill<T>(this T[] array, T value)
        {
            for (var i = 0; i < array.Length; i++)
            {
                array[i] = value;
            }
        }
        
        /// <summary>Sets all entries in array to the given factory function's output</summary>
        public static void Produce<T>(this T[] array, Func<T> factory)
        {
            for (var i = 0; i < array.Length; i++)
            {
                array[i] = factory();
            }
        }
        
        /// <summary>Adds count entries to the List from the given factory function's output</summary>
        public static void Produce<T>(this List<T> list, int count, Func<T> factory)
        {
            for (var i = 0; i < count; i++)
            {
                list[i] = factory();
            }
        }

        /// <summary>Adds count entries to the Queue from the given factory function's output</summary>
        public static void Produce<T>(this Queue<T> queue, int count, Func<T> factory)
        {
            for (var i = 0; i < count; i++)
            {
                queue.Enqueue(factory());
            }
        }

        /// <summary>Ensures the list has the given amount of items.</summary>
        public static void Ensure<T>(this List<T> list, int count, Func<T> factory = null)
        {
            while (list.Count > count) list.RemoveAt(list.Count - 1);
            while (list.Count < count) list.Add(factory == null ? default : factory());
        }

        /// <summary>Returns a value from the list, or if out of bounds, a default value depending on under or overflow.</summary>
        public static T Eval<T>(this List<T> list, int index, T underflow = default, T overflow = default)
        {
            if (index < 0) return underflow;
            if (index >= list.Count) return overflow;
            return list[index];
        }
    }
}

/*
Written by Tiger Blue in 2017, 2018, 2021, 2023

This is free and unencumbered software released into the public domain.

Anyone is free to copy, modify, publish, use, compile, sell, or
distribute this software, either in source code form or as a compiled
binary, for any purpose, commercial or non-commercial, and by any
means.

In jurisdictions that recognize copyright laws, the author or authors
of this software dedicate any and all copyright interest in the
software to the public domain. We make this dedication for the benefit
of the public at large and to the detriment of our heirs and
successors. We intend this dedication to be an overt act of
relinquishment in perpetuity of all present and future rights to this
software under copyright law.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
OTHER DEALINGS IN THE SOFTWARE.

For more information, please refer to <http://unlicense.org>
*/