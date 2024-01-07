using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace Artificing
{
    public class Artifact : MonoBehaviour
    {
        [SerializeField] private WeaponType weaponType;
        [SerializeField] private ArtifactComponents currentComponents;
        [SerializeField] private ArtifactComponents targetComponents;

        private void Start()
        {
            currentComponents = ArtifactComponents.Empty();
            
            DebugArtifactString();
        }

        public void AddImbuement(Imbuement imbuement)
        {
            if (currentComponents.Imbuements.Contains(Imbuement.None))
                currentComponents.Imbuements.Remove(Imbuement.None);
            
            currentComponents.Imbuements.Add(imbuement);
            
            DebugArtifactString();
        }

        public void ChangeMod(Modification modification)
        {
            currentComponents.Modification = modification;
            
            DebugArtifactString();
        }

        public void ChangeRarity(Rarity rarity)
        {
            currentComponents.Rarity = rarity;
            
            DebugArtifactString();
        }

        public void DebugArtifactString()
        {
            Debug.Log(ArtifactComponents.ToString(currentComponents));
            Debug.Log(ArtifactComponents.ToString(targetComponents));
        }
    }

    public struct ArtifactComponents
    {
        public WeaponType WeaponType;
        public List<Imbuement> Imbuements;
        public Modification Modification;
        public Rarity Rarity;

        /// <summary>
        /// Checks whether a valid artifact is of the correct type.
        /// </summary>
        /// <param name="current"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool Equal(ArtifactComponents current, ArtifactComponents target)
        {
            if (current.WeaponType != target.WeaponType)
                return false;

            if (target.Imbuements.Any(imbuement => !current.Imbuements.Contains(imbuement)))
                return false;

            if (current.Modification != target.Modification)
                return false;

            if (current.Rarity != target.Rarity)
                return false;

            return true;
        }

        /// <summary>
        /// Checks whether a given artifact is valid and can be continued.
        /// </summary>
        /// <param name="current"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool CheckValid(ArtifactComponents current, ArtifactComponents target)
        {
            if (current.Imbuements.Count > target.Imbuements.Count)
                return false;
            
            foreach (var imbuement in current.Imbuements)
            {
                if (imbuement == Imbuement.None)
                    break;
                
                if (!target.Imbuements.Contains(imbuement))
                {
                    return false;
                }
            }
            
            if (current.Modification != target.Modification && current.Modification != Modification.None)
                return false;

            if (current.Rarity != target.Rarity && current.Rarity != Rarity.None)
                return false;
            
            return true;
        }

        /// <summary>
        /// Generates a string pertaining to the artifact in the form
        /// "{Rarity} {Modification} {Weapon Type} {Imbuements}"
        /// </summary>
        /// <param name="artifactComponents"> The artifact to convert to a string </param>
        /// <returns></returns>
        public static string ToString(ArtifactComponents artifactComponents)
        {
            string baseString = string.Empty;

            string rarity = artifactComponents.Rarity != Rarity.None ? artifactComponents.Rarity.ToString() : string.Empty;
            string modification = artifactComponents.Modification != Modification.None
                ? artifactComponents.Modification.ToString()
                : string.Empty;
            
            string weaponType = artifactComponents.WeaponType.ToString();
            
            string imbuementBase = string.Empty;

            for (int i = 0; i < artifactComponents.Imbuements.Count; i++)
            {
                if (artifactComponents.Imbuements.Contains(Imbuement.None))
                    break;
                
                if (i == 0)
                {
                    imbuementBase += $"of {artifactComponents.Imbuements[i].ToString()}";
                    continue;
                }

                if (i == artifactComponents.Imbuements.Count-1)
                {
                    imbuementBase += $" and {artifactComponents.Imbuements[i].ToString()}";
                }
                else
                {
                    imbuementBase += $", {artifactComponents.Imbuements[i].ToString()}";
                }
            }

            string modSpaced = artifactComponents.Modification == Modification.None ? string.Empty : " " + modification;
            baseString = $"{rarity}{modSpaced} {weaponType} {imbuementBase}";

            return $"{baseString.Trim()}.";
        }
        
        /// <summary>
        /// Generates a new random artifact given the parameters.
        /// </summary>
        /// <param name="maxImbuements"> The maximum number of imbuements the artifact should have </param>
        /// <param name="hasRarity"> Whether the artifact has a rarity. </param>
        /// <param name="hasModification"> Whether the artifact has a modification </param>
        /// <returns> A New Artifact Component </returns>
        public static ArtifactComponents GenerateNewArtifact(int maxImbuements, bool hasRarity, bool hasModification)
        {
            ArtifactComponents artifact = new ArtifactComponents();
            
            Random rnd = new Random();
            
            // Generate Weapon
            int randomWeaponInt = rnd.Next(0, Enum.GetNames(typeof(WeaponType)).Length-1);

            // Generate Imbuements
            List<Imbuement> imbuements = new List<Imbuement>();

            if (maxImbuements > 0)
            {
                for (int i = 0; i < maxImbuements; i++)
                {
                    Imbuement newImbuement = Imbuement.None;
                
                    do
                    {
                        int randomImbuement = rnd.Next(0, Enum.GetNames(typeof(Imbuement)).Length-1);
                        newImbuement = (Imbuement)randomImbuement;
                    
                    } while (imbuements.Contains(newImbuement));
                
                    imbuements.Add(newImbuement);
                }
            }
            else
                imbuements.Add(Imbuement.None);
            
            // Generate Rarity
            Rarity randomRarity = Rarity.None;

            if (hasRarity)
            {
                int randomRarityInt = rnd.Next(0, Enum.GetNames(typeof(Rarity)).Length-1);
                randomRarity = (Rarity)randomRarityInt;
            }
        
            // Generate Modification
            Modification randomModification = Modification.None;
        
            if (hasModification)
            {
                int randomRarityInt = rnd.Next(0, Enum.GetNames(typeof(Modification)).Length-1);
                randomModification = (Modification)randomRarityInt;
            }
            
            WeaponType randomWeapon = (WeaponType)randomWeaponInt;

            artifact.WeaponType = randomWeapon;
            artifact.Rarity = randomRarity;
            artifact.Modification = randomModification;
            artifact.Imbuements = imbuements;

            return artifact;
        }

        public static ArtifactComponents Empty()
        {
            ArtifactComponents artifactComponents = new();

            artifactComponents.WeaponType = WeaponType.Sword;
            artifactComponents.Imbuements.Add(Imbuement.None);
            artifactComponents.Rarity = Rarity.None;
            artifactComponents.Modification = Modification.None;

            return artifactComponents;
        }
    }
    
    public enum WeaponType
    {
        Sword, Spear, Bow, Hammer, Axe
    }

    public enum Imbuement
    {
        Fire, Wind, Water, None
    }

    public enum Modification
    {
        Light, Heavy, Balanced, None
    }

    public enum Rarity
    {
        Common, Rare, Legendary, None
    }
}