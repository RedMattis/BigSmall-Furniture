using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Verse;
using UnityEngine;
using Verse.Sound;
using Verse.AI;

namespace BigAndSmall
{

    public class CompProperties_Bed : CompProperties
    {
        public int sleepingSlotCount = 1;

        public CompProperties_Bed()
        {
            compClass = typeof(Comp_Bed);
        }
    }

    public class Comp_Bed : ThingComp
    {
        public CompProperties_Bed Props => (CompProperties_Bed)props;
    }

    // Patch property
    [HarmonyPatch(typeof(Building_Bed), nameof(Building_Bed.SleepingSlotsCount), MethodType.Getter)]
    public static class BedUtility_GetSleepingSlotsCount_Patch
    {
        public static void Postfix(ref int __result, Building_Bed __instance)
        {
            // Check if the bed has BS_BedComps
            if (__instance.def.HasComp(typeof(Comp_Bed)))
            {
                // Get the BS_BedComps
                var comp = __instance.GetComp<Comp_Bed>();
                __result = comp.Props.sleepingSlotCount;
            }
        }
    }

    [HarmonyPatch(typeof(BedInteractionCellSearchPattern), nameof(BedInteractionCellSearchPattern.BedCellOffsets))]
    public static class BedCellOffsets_Patch
    {
        public static bool Prefix(List<IntVec3> offsets, IntVec2 size, int slot)
        {
            return GetBedCellOffsets(offsets, size, slot);
        }

        public static bool GetBedCellOffsets(List<IntVec3> offsets, IntVec2 size, int slot)
        {
            if (size.z == 1 && size.x == 1)
            {
                return true;
            }
            else if (size.z == 2)
            {
                return true;
            }
            else
            {
                bool rightEdge = slot == 0;
                bool leftEdge = slot == BedUtility.GetSleepingSlotsCount(size) - 1;
                BedInteractionCellSearchPattern.BedCellOffsets2xN(offsets, rightEdge, leftEdge);
                return false;
            }
        }
    }

    [HarmonyPatch(typeof(ChildBirthCellSearchPattern), nameof(ChildBirthCellSearchPattern.BedCellOffsets))]
    public static class BedCellOffsetsChild_Patch
    {
        public static bool Prefix(List<IntVec3> offsets, IntVec2 size, int slot)
        {
            return BedCellOffsets_Patch.GetBedCellOffsets(offsets, size, slot);
        }
    }


    [HarmonyPatch(typeof(PawnRenderer), "GetBodyPos")]
    public static class GetBodyPos_Patch
    {
        public static void Postfix(ref Vector3 __result, Pawn ___pawn)
        {
            var pawn = ___pawn;
            Building_Bed bed = pawn.CurrentBed();
            if (bed != null)
            {
                if (bed.def.HasComp(typeof(Comp_Bed)))
                {
                    // Get the BS_BedComps
                    var comp = bed.GetComp<Comp_Bed>();
                    int bedWidth = bed.def.size.x;
                    int totalSlots = comp.Props.sleepingSlotCount;

                    var occupants = bed.CurOccupants;
                    float slot = occupants.ToList().IndexOf(pawn) + 0.5f;

                    // calculate difference between width and sleeping slots. If the slots are fewer than the width we need a small offset.
                    float widthPerSlot = bedWidth / (float)totalSlots;
                    float rightOffset = widthPerSlot * slot - slot;
                    float upOffset = 0;

                    if (pawn.RaceProps.Humanlike == false)
                    {
                        //offset += bed.def.building.bed_pawnDrawOffset; // This is normally skipped for animals.
                        upOffset += bed.def.building.bed_pawnDrawOffset;
                    }
                    // Check if bed is facing south
                    if (bed.Rotation == Rot4.South)
                    {
                        __result.x += rightOffset;
                        __result.z -= upOffset;
                    }
                    else if (bed.Rotation == Rot4.North)
                    {
                        __result.x -= rightOffset;
                        __result.z += upOffset;
                    }
                    else if (bed.Rotation == Rot4.East)
                    {
                        __result.z += rightOffset;
                        __result.x -= upOffset;
                    }
                    else if (bed.Rotation == Rot4.West)
                    {
                        __result.z -= rightOffset;
                        __result.x += upOffset;
                    }

                    //Rot4 rotation = bed.Rotation;
                    //rotation.AsInt += 1;
                    //Vector3 vector2 = rotation.FacingCell.ToVector3();
                    //__result += vector2 * offset;



                }
            }
        }
    }

}