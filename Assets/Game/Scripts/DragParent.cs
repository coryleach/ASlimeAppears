using System;
using UnityEngine;

namespace Game.Scripts
{
    public class DragParent : MonoBehaviour
    {
        private static DragParent instance;
        public static DragParent Instance => instance;

        private void Awake()
        {
            instance = this;
        }
    }
}
