using System;
using TMPro;
using UnityEngine;

namespace UI
{
    public class DialogPopup : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject holder;
        [SerializeField] private TextMeshPro text;

        [Header("Values")] 
        [SerializeField] private bool isActive;
        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
            GameManager.Instance.AddTooltip(this);
            DeActivate();
        }

        private void Update()
        {
            holder.transform.forward = Vector3.Lerp(holder.transform.forward, (transform.position - _camera.transform.position).normalized, 7.5f * Time.deltaTime);
        }

        /// <summary>
        /// Sets the text of the tooltip.
        /// </summary>
        /// <param name="targetText"></param>
        public void SetText(string targetText)
        {
            text.text = targetText;
        }

        /// <summary>
        /// Enables the tooltip.
        /// </summary>
        public void Activate()
        {
            isActive = true;
            holder.SetActive(true);
        }
        
        /// <summary>
        /// Disables the tooltip.
        /// </summary>
        public void DeActivate()
        {
            isActive = false;
            holder.SetActive(false);
        }

        /// <summary>
        /// Removes the tooltip from the instance.
        /// </summary>
        public void Remove()
        {
            Destroy(gameObject, 2f);
        }
    }
}