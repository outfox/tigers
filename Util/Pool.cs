using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tiger.Util
{
    public class Pool<T>
    {
        private readonly T[] _instances;
        private int _index = 0;
        
        public Pool(int count, Func<T> factory)
        {
            _instances = new T[count];
            _instances.Produce(factory);
        }

        public T Get()
        {
            _index = _index % _instances.Length;
            return _instances[_index++];
        }

        public bool Try(out T result, Predicate<T> predicate)
        {
            _index = _index % _instances.Length;
            
            if (predicate.Invoke(_instances[_index]))
            {
                result = _instances[_index++];
                return true;
            }

            result = default;
            return false;
        }
    }


    public struct TimedItem<T>
    {
        public float time;
        public T item;
        
        public TimedItem(float t, T i)
        {
            time = t;
            item = i;
        }
    }

    
    public class TimedPool<T>
    {
        private readonly List<TimedItem<T>> _available;
        private readonly List<TimedItem<T>> _busy;

        public TimedPool(int count, Func<T> factory)
        {
            _available = new List<TimedItem<T>>(count);
            for (var i = 0; i < count; i++)
            {
                _available.Add(new TimedItem<T>(0, factory()));
            }
            _busy = new List<TimedItem<T>>(count);
        }

        public T Get()
        {
            var timed = _available.TakeLast();
            return timed.item;
        }

        public void Return(float until, T item)
        {
            _busy.Add(new TimedItem<T>(until, item));
        }

        public T Get(float until)
        {
            var timed = _available.TakeLast();
            timed.time = until;
            _busy.Insert(0, timed);
            return timed.item;
        }

        public bool Try(out T result)
        {
            if (_available.Count == 0 && _lastRecycle < Time.unscaledTime) Recycle();

            if (_available.Count > 0)
            {
                result = Get();
                return true;
            }
            
            result = default;
            return false;
        }

        private float _lastRecycle;
        private void Recycle()
        {
            _lastRecycle = Time.unscaledTime;  //To avoid even the 1 GC if we get hammered in 1 frame.
            _available.AddRange(_busy.Where(Ready.Predicate));
            _busy.RemoveAll(Ready.Predicate);            
        }
    }

    
    public class Ready
    {
        public static bool Predicate(AudioSource source)
        {
            return !source.isPlaying;
        }

        public static bool Predicate<T>(TimedItem<T> item)
        {
            return item.time < Time.time;
        }

        public static bool Predicate(GameObject obj)
        {
            return !obj.activeSelf;
        }
    }
}

/*
Written by Tiger Blue in 2021

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