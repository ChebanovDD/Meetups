using TMPro;
using UnityEngine;

namespace UiElements
{
    public abstract class InteractableElement : MonoBehaviour
    {
        [SerializeField] private TMP_Text _label;
        [SerializeField] protected Color _disabledColor = Color.white;

        private Color _defaultColor;
        
        protected virtual void Awake()
        {
            _defaultColor = _label.color;
        }

        public void SetInteractable(bool value)
        {
            _label.color = value ? _defaultColor : _disabledColor;
            OnSetInteractable(value);
        }

        protected abstract void OnSetInteractable(bool value);
    }
}