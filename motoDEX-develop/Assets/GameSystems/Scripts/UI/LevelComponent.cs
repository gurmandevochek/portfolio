using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LevelComponent : MonoBehaviour
    {
        public bool IsSelected => borders.activeSelf;
        
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private Button button;
        [SerializeField] private GameObject borders;

        private Action<int> onClick;
        private int index;
        
        private void Awake()
        {
            InitComponents();   
        }

        private void InitComponents()
        {
            button.onClick.AddListener(OnClick);
        }

        public void Set(int index)
        {
            this.index = index;
        }
        
        public void SetCallback(Action<int> callback)
        {
            onClick = callback;
        }

        public void Select()
        {
            borders.SetActive(true);
        }

        public void Deselect()
        {
            borders.SetActive(false);
        }

        private void OnClick()
        {
            onClick?.Invoke(index);
        }
    }
}