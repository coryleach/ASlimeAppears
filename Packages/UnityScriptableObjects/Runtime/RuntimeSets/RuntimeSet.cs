﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Gameframe.ScriptableObjects.RuntimeSets
{
  public class RuntimeSet<T> : ScriptableObject, IRuntimeSet<T>
  {
    public class RuntimeSetChangeEvent : UnityEvent<T> {}

    private readonly List<T> _items = new List<T>();
    public IReadOnlyList<T> Items => _items;

    public UnityEvent<T> OnAdded { get; } = new RuntimeSetChangeEvent();
    public UnityEvent<T> OnRemoved { get; } = new RuntimeSetChangeEvent();

    public void Add(T t)
    {
      _items.Add(t);
      OnAdded.Invoke(t);
    }

    public void Remove(T t)
    {
      _items.Remove(t);
      OnRemoved.Invoke(t);
    }
  }
}
