using System;
using System.Collections.Generic;
using UnityEngine;

namespace Artificing
{
    public class ArtifactMeshSwitcher : MonoBehaviour
    {
        [Header("References")] 
        [SerializeField] private Transform meshHolder;
        [SerializeField] private Transform highlightTransform;
        [SerializeField] private MeshFilter highlightMeshFilter;
        
        
        [Header("Fields")]
        [SerializeField] private List<Mesh> meshList;
        [SerializeField] private List<GameObject> prefabList;

        [field: Header("Values")]
        [field: SerializeField] public WeaponType currentWeaponType { get; private set; }
        private Dictionary<WeaponType, Mesh> weaponMeshHighlightDictionary = new Dictionary<WeaponType, Mesh>();
        private Dictionary<WeaponType, GameObject> weaponPrefabDictionary = new Dictionary<WeaponType, GameObject>();

        private void Awake()
        {
            int i = 0;
            foreach (var weapon in Enum.GetValues(typeof(WeaponType)))
            {
                weaponMeshHighlightDictionary.Add((WeaponType)weapon, meshList[i]);
                weaponPrefabDictionary.Add((WeaponType)weapon, prefabList[i]);
                i++;
            }
        }

        public void SetWeaponType(WeaponType newWeaponType)
        {
            Destroy(meshHolder.GetChild(0).gameObject);

            GameObject newWeapon = Instantiate(weaponPrefabDictionary[newWeaponType], meshHolder);
            newWeapon.transform.localPosition = newWeapon.GetComponent<WeaponOffset>().offset;

            highlightMeshFilter.mesh = weaponMeshHighlightDictionary[newWeaponType];
            highlightTransform.localPosition = newWeapon.GetComponent<WeaponOffset>().offset;
        }
    }
}