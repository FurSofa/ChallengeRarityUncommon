using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;
using MelonLoader;
using HarmonyLib;

[assembly: MelonInfo(typeof(ChallengeRarity.Main), "ChallengeOnlyUncommon", "1.0.0", "Fur")]
[assembly: MelonGame("TheJaspel", "Backpack Hero")]

namespace ChallengeRarity
{
    public class Main : MelonMod
    {

    }
}

public class ChallengeOnlyUncommon : MelonMod
{
    //Redefines Backpack tile gain to be flatter and more difficult early game
    [HarmonyPatch(typeof(GameManager), "SpawnItem", new[] { typeof(List<Item2.ItemType>), typeof(List<Item2.Rarity>) })]
    class LuckPatch
    {
        [HarmonyPrefix]
        static bool Prefix(List<Item2.ItemType> itemTypes, List<Item2.Rarity> itemRarities, ref GameObject __result, GameManager __instance)
        {
            itemRarities = new List<Item2.Rarity> { Item2.Rarity.Uncommon };

            List<GameObject> list = new List<GameObject>();
            foreach (GameObject gameObject in __instance.itemsToSpawn)
            {
                Item2 component = gameObject.GetComponent<Item2>();
                if ((itemTypes.Count == 0 || itemTypes.Contains(Item2.ItemType.Any) || Item2.ShareItemTypes(itemTypes, component.itemType)) && itemRarities.Contains(component.rarity))
                {
                    list.Add(gameObject);
                }
            }
            if (list.Count == 0)
            {
                __result = null;
            }
            int index = UnityEngine.Random.Range(0, list.Count);
            __result = UnityEngine.Object.Instantiate<GameObject>(list[index], Vector3.zero, Quaternion.identity, __instance.itemsParent);

            return false;
        }
    }
}
