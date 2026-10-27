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

            System.Reflection.MethodInfo mOriginal = AccessTools.Method(typeof(Verse.Pawn_AgeTracker), "DebugForceBirthdayBiological");
            var mPostfix = AccessTools.Method(typeof(HarmonyPatches), "DebugForceBirthdayBiological_Postfix");

            harmony.Patch(mOriginal, null, new HarmonyMethod(mPostfix));
        }
        public static void DebugForceBirthdayBiological_Postfix(ref Verse.Pawn_AgeTracker __instance)
        {
            System.Reflection.MethodInfo mCalculateGrowth = AccessTools.Method(typeof(Verse.Pawn_AgeTracker), "CalculateGrowth");
            mCalculateGrowth.Invoke(__instance, new object[] { 60 * +60000 });
        }


    }
    public class IngestionOutcomeDoer_RapidAging : IngestionOutcomeDoer
    {
        protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested, int placeholder)
        {
            if(pawn == null) { return; }
            long currentAgeTicks = pawn.ageTracker.AgeBiologicalTicks;
            pawn.ageTracker.DebugSetAge(currentAgeTicks + (60 * +60000));
            pawn.ageTracker.DebugForceBirthdayBiological();
        }
    }

    public class IngestionOutcomeDoer_SetAgeAdult : IngestionOutcomeDoer
    {
        protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested, int placeholder)
        {
            if (pawn == null) { return; }

            // Find the minimum adult age in years
            var lifeStages = pawn.RaceProps.lifeStageAges;
            float minAdultAge = 0f;
            foreach (var stage in lifeStages)
            {
                if (stage.def.developmentalStage == DevelopmentalStage.Adult)
                {
                    minAdultAge = stage.minAge;
                    break;
                }
            }
            // For humans, override to 18 if minAdultAge < 18
            if (pawn.def.defName == "Human" && minAdultAge < 18f)
            {
                minAdultAge = 18f;
            }
            long minAdultAgeTicks = (long)(minAdultAge * 60 * 60000);
            pawn.ageTracker.DebugSetAge(minAdultAgeTicks);
            pawn.ageTracker.AgeBiologicalTicks = minAdultAgeTicks;
            pawn.ageTracker.DebugForceBirthdayBiological();
        }
    }
}
