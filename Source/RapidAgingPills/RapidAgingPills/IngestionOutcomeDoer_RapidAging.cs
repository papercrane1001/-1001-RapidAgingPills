using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HarmonyLib;
using RimWorld;
using Verse;
using UnityEngine;

namespace RapidAgingPills
{
    [StaticConstructorOnStartup]
    static class HarmonyPatches
    {
        // this static constructor runs to create a HarmonyInstance and install a patch.
        static HarmonyPatches()
        {
            //var harmony = Harmony.
            var harmony = new Harmony("rimworld.roxxploxx.unificamagica");

            //HarmonyInstance harmony = HarmonyInstance.Create("rimworld.roxxploxx.unificamagica");

            // find the FillTab method of the class RimWorld.ITab_Pawn_Character
            System.Reflection.MethodInfo targetMethod = AccessTools.Method(typeof(RimWorld.ITab_Pawn_Character), "FillTab");
            //MethodInfo targetmethod = AccessTools.Method(typeof(RimWorld.ITab_Pawn_Character), "FillTab");

            // find the static method to call before (i.e. Prefix) the targetmethod
            HarmonyMethod prefixmethod = new HarmonyMethod(typeof(UnificaMagica.HarmonyPatches).GetMethod("FillTab_Prefix"));

            // patch the targetmethod, by calling prefixmethod before it runs, with no postfixmethod (i.e. null)
            harmony.Patch(targetmethod, prefixmethod, null);
        }

        // This method is now always called right before RimWorld.ITab_Pawn_Character.FillTab.
        // So, before the ITab_Pawn_Character is instantiated, reset the height of the dialog window.
        // The class RimWorld.ITab_Pawn_Character is static so there is no this __instance.
        public static void FillTab_Prefix()
        {
            RimWorld.CharacterCardUtility.PawnCardSize.y = DefDatabase<RimWorld.SkillDef>.AllDefsListForReading.Count * 47.5f;
        }
    }
    public class IngestionOutcomeDoer_RapidAging : IngestionOutcomeDoer
    {
        protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested)
        {
            if(pawn == null) { return; }
            //pawn.ageTracker.DebugForceBirthdayBiological();
            pawn.ageTracker.DebugMakeOlder(60 * 60000);
            //pawn.ageTracker.
            //pawn.ageTracker.DebugForceBirthdayBiological();
        }
    }
}
