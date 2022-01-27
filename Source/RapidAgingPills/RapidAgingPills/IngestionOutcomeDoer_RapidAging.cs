using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RimWorld;
using Verse;
using UnityEngine;

namespace RapidAgingPills
{
    //[DefOf]
    //class AgingDefOf
    //{
    //    //public static HediffDef 

    //}
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
