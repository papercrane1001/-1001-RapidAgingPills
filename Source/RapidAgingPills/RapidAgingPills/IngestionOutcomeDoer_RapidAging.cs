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
        static HarmonyPatches()
        {
            
            var harmony = new Harmony("rimworld.OneThousandOne.RapidAgingPills");

            System.Reflection.MethodInfo mOriginal = AccessTools.Method(typeof(Verse.Pawn_AgeTracker), "DebugMakeOlder");
            var mPostfix = AccessTools.Method(typeof(HarmonyPatches), "DebugMakeOlder_Postfix");

            harmony.Patch(mOriginal, null, new HarmonyMethod(mPostfix));
        }
        public static void DebugMakeOlder_Postfix(ref Verse.Pawn_AgeTracker __instance)
        {
            System.Reflection.MethodInfo mCalculateGrowth = AccessTools.Method(typeof(Verse.Pawn_AgeTracker), "CalculateGrowth");
            mCalculateGrowth.Invoke(__instance, new object[] { 60 * +60000 });
        }


    }
    public class IngestionOutcomeDoer_RapidAging : IngestionOutcomeDoer
    {
        protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested)
        {
            if(pawn == null) { return; }
            pawn.ageTracker.DebugMakeOlder(60 * 60000);
        }
    }
}
