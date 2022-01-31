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
            ;
            var harmony = new Harmony("");

            System.Reflection.MethodInfo mOriginal = AccessTools.Method(typeof(Verse.Pawn_AgeTracker), "DebugMakeOlder");

            var mPostfix = SymbolExtensions.GetMethodInfo(() => DebugMakeOlder_Postfix());

            harmony.Patch(mOriginal, null, new HarmonyMethod(mPostfix));
        }
        public static void DebugMakeOlder_Postfix()
        {
            this.RecalculateLifeStageIndex();
        }


    }
    public class IngestionOutcomeDoer_RapidAging : IngestionOutcomeDoer
    {
        protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested)
        {
            if(pawn == null) { return; }
            pawn.ageTracker.DebugMakeOlder(60 * 60000);
            //pawn.ageTracker.
            //pawn.ageTracker.DebugForceBirthdayBiological();
            //pawn.ageTracker.
        }
    }
}
