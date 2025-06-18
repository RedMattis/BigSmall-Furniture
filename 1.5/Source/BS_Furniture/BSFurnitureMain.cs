
using Verse;
using HarmonyLib;
using UnityEngine;
//using VariedBodySizes;

namespace BigAndSmall
{
    /// <summary>
    /// Better Children Skill Learning multiplies XP by 3 for children.
    /// </summary>
    [StaticConstructorOnStartup]
    internal class BSFurnitureMain : Mod
    {
        public static BSFurnitureMain instance = null;
        public static BSFurnitureSettings settings;

        //private string extractionBuff, multipakBuff, megaPackBuff, geneRegrowBuff, nutritionUse;
        public BSFurnitureMain(ModContentPack content) : base(content)
        {
            instance = this;
            settings = GetSettings<BSFurnitureSettings>();

            ApplyHarmonyPatches();
        }

        static void ApplyHarmonyPatches()
        {
            var harmony = new Harmony("RedMattis.BigSmallFurniture");
            harmony.PatchAll();

        }

        //public override void DoSettingsWindowContents(Rect inRect)
        //{
            //Listing_Standard listStd = new Listing_Standard();


            //listStd.Begin(inRect);
            
            //listStd.TextFieldNumericLabeled("Multipack Chance", ref settings.multipackChance, ref multipakBuff, min: 0.1f, max: 1.0f);
            //listStd.TextFieldNumericLabeled("Megapack Chance", ref settings.megaMultipackChance, ref megaPackBuff, min: 0.1f, max: 1.0f);

            //listStd.TextFieldNumericLabeled("Extraction Time", ref settings.extractionHours, ref extractionBuff, min: 0.1f, max: 960);
            //listStd.TextFieldNumericLabeled("Nutrition Use Multiplier", ref settings.nutritionMultiplier, ref nutritionUse, min: 0.001f, max: 10);
            //listStd.TextFieldNumericLabeled("Genes Regrowing Time", ref settings.genesRegrowingHours, ref geneRegrowBuff, min: -1f, max: 960f);
            //listStd.End();
            //base.DoSettingsWindowContents(inRect);
        //}

        //public override string SettingsCategory()
        //{
        //    return "Big and Small Furniture";
        //}
    }

    public class BSFurnitureSettings: ModSettings
    {
        //public float extractionHours = 192;
        //public float multipackChance = 0.45f;
        //public float megaMultipackChance = 0.15f;

        //public float genesRegrowingHours = -1f;
        //public float nutritionMultiplier = 1f;

        //public int ExtractionTimeInTicks => (int)extractionHours * 2500;
        //public int RegrowTimeInTicks => (int)genesRegrowingHours * 2500;

        


        //public override void ExposeData()
        //{
        //    Scribe_Values.Look(ref extractionHours, "extractionTime", 192);
        //    Scribe_Values.Look(ref multipackChance, "multipackChance", 0.45f);
        //    Scribe_Values.Look(ref megaMultipackChance, "megaMultipackChance", 0.15f);
        //    Scribe_Values.Look(ref nutritionMultiplier, "nutritionMultiplier", 1);
        //    Scribe_Values.Look(ref genesRegrowingHours, "genesRegrowingHours", -1f);
        //    base.ExposeData();
        //}


    }

}
