using System;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalShooter.Pooling
{
    /// <summary>Generic pre-built object pool.</summary>
    public class ObjectPool<T> where T : Component
    {
        readonly Stack<T> available;
        readonly List<T> all;
        readonly Action<T> onGet;
        readonly Action<T> onRelease;

        public int Capacity => all.Count;
        public int AvailableCount => available.Count;

        public ObjectPool(T prefab, Transform parent, int size, Action<T> onGet = null, Action<T> onRelease = null)
        {
            this.onGet = onGet;
            this.onRelease = onRelease;
            available = new Stack<T>(size);
            all = new List<T>(size);

            for (int i = 0; i < size; i++)
            {
                T item = UnityEngine.Object.Instantiate(prefab, parent);
                item.gameObject.SetActive(false);
                all.Add(item);
                available.Push(item);
            }
        }

        /// <summary>Gets object, or null.</summary>
        public T Get()
        {
            if (available.Count == 0) return null;
            T item = available.Pop();
            item.gameObject.SetActive(true);
            onGet?.Invoke(item);
            return item;
        }

        public void Release(T item)
        {
            if (item == null || !item.gameObject.activeSelf) return;
            onRelease?.Invoke(item);
            item.gameObject.SetActive(false);
            available.Push(item);
        }

        /// <summary>Returns all active objects.</summary>
        public void ReleaseAll()
        {
            for (int i = 0; i < all.Count; i++) Release(all[i]);
        }
    }
}
