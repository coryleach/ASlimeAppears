using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts
{
    public class FloatyTextManager : MonoBehaviour
    {
        private static FloatyTextManager instance;
        public static FloatyTextManager Instance => instance;

        [SerializeField]
        private FloatyText prefab;

        private Queue<FloatyText> textsPool = new Queue<FloatyText>();

        private void Awake()
        {
            instance = this;
            prefab.gameObject.SetActive(false);
        }

        public void SpawnText(string text, Color color, Vector3 position)
        {
            FloatyText spawnedText = null;
            if (textsPool.Count > 0)
            {
                spawnedText = textsPool.Dequeue();
            }
            else
            {
                spawnedText = Instantiate(prefab, transform);
                spawnedText.OnComplete.AddListener(OnFloatyTextComplete);
            }

            spawnedText.Text = text;
            spawnedText.TextColor = color;
            spawnedText.Pivot = position;
            spawnedText.transform.position = position;
            spawnedText.gameObject.SetActive(true);
            spawnedText.enabled = true;
        }

        private void OnFloatyTextComplete(FloatyText text)
        {
            textsPool.Enqueue(text);
            text.gameObject.SetActive(false);
        }

    }
}
