using System;
using System.Collections.Generic;
using System.Linq;
using Player;
using UnityEngine;

namespace Objects
{
    public class HighlightObject : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Material highlightMaterial;
        private static readonly int Alpha = Shader.PropertyToID("_Alpha");
        private Camera mainCamera;

        private void Start()
        {
            mainCamera = Camera.main;
        }

        private void Update()
        {
            RaycastHit[] hits = new RaycastHit[5];
            Physics.RaycastNonAlloc(mainCamera.transform.position, mainCamera.transform.forward, hits, 
                PlayerInteractionController.Instance.GetPickupDistance());
            
            List<Transform> hitList = hits.Select(hit => hit.transform).ToList();

            if (hitList.Count <= 0) return;
            if (!hitList.Contains(transform.parent))
            {
                DoHighlight(false);
                return;
            }
            
            foreach (var hit in hitList)
            {
                if (hit == null) continue;
                if (!hit.CompareTag("Interactable")) continue;
                
                if (hit.Equals(transform.parent))
                {
                    DoHighlight(!CheckComponents(hit));
                    
                    break;
                }
                
                if (CheckComponents(hit))
                    continue;

                DoHighlight(false);
                break;
            }
        }

        /// <summary>
        /// Checks to see if the object being looked at has any affecting components.
        /// </summary>
        /// <param name="hit"></param>
        /// <returns></returns>
        private bool CheckComponents(Transform hit)
        {
            if (hit.TryGetComponent<Pickup>(out Pickup p))
                if (p.GetIsHeld())
                    return true;

            if (hit.TryGetComponent<Workstation>(out Workstation w))
                if (w.GetIsTargeted())
                    return true;

            return false;
        }

        /// <summary>
        /// Highlights this object.
        /// </summary>
        /// <param name="highlight"></param>
        private void DoHighlight(bool highlight)
        {
            //GetComponent<MeshRenderer>().materials[1].SetFloat(Alpha, highlight ? 0.5f : 0f);

            List<Material> materials = new List<Material>();
            GetComponent<MeshRenderer>().GetMaterials(materials);

            materials.Find(mat => mat.shader.name == (highlightMaterial.shader.name)).SetFloat(Alpha, highlight ? 0.5f : 0f);
        }
    }
}