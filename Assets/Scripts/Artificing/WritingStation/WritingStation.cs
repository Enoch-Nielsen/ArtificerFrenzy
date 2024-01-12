using System;
using System.Collections.Generic;
using Objects;
using UnityEngine;
using Artificing;

namespace Artificing.WritingStation
{
    public class WritingStation : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Station station;
        
        [Header("Writing Stones")]
        [SerializeField] private WritingStone rarityStone;
        [SerializeField] private WritingStone modStone;
        [SerializeField] private WritingStone imbueStone_1, imbueStone_2, imbueStone_3;

        [Header("Values")] 
        [SerializeField] private Artifact currentArtifact;
        [SerializeField] private bool hasArtifact;

        private void Start()
        {
            station.OnPlayerExitStation += SubmitArtifact;
        }

        private void Update()
        {
            if (station.TryGetArtifact(out Artifact a))
            {
                currentArtifact = a;
                hasArtifact = true;
            }
            else
            {
                currentArtifact = null;
                hasArtifact = false;
            }
        }

        private void SubmitArtifact()
        {
            if (currentArtifact == null || !hasArtifact)
            {
                ResetSymbols();
                return;
            }

            if (currentArtifact.targetComponentsSet)
                return;
            
            List<Imbuement> imbuements = new List<Imbuement>();
            
            imbuements.Add((Imbuement)Enum.Parse(typeof(Imbuement), imbueStone_1.GetCurrentSymbol().ToString()));
            imbuements.Add((Imbuement)Enum.Parse(typeof(Imbuement), imbueStone_2.GetCurrentSymbol().ToString()));
            imbuements.Add((Imbuement)Enum.Parse(typeof(Imbuement), imbueStone_3.GetCurrentSymbol().ToString()));
            
            Modification modification = (Modification)Enum.Parse(typeof(Modification), modStone.GetCurrentSymbol().ToString());
            Rarity rarity = (Rarity)Enum.Parse(typeof(Rarity), rarityStone.GetCurrentSymbol().ToString());


            ArtifactComponents ac = new ArtifactComponents(currentArtifact.GetArtifactComponent().WeaponType,
                imbuements, modification, rarity);
            
            currentArtifact.SetTargetArtifact(ac);
            
            ResetSymbols();
        }

        private void ResetSymbols()
        {
            imbueStone_1.Reset();
            imbueStone_2.Reset();
            imbueStone_3.Reset();
            modStone.Reset();
            rarityStone.Reset();
        }
    }
}