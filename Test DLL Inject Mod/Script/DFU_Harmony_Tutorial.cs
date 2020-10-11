//////////////////////////////////////////////////////////////////
/// DFU Injector tutorial by ShortBeard - 9/10/2020.
/// This tutorial shows how to replace the "PlayActivateSound" method in the FPSWeapon class with an arbitrary method of our choosing.
///////////////////////////////////////////////////////////////////


using System; //This must be included 
using DaggerfallWorkshop.Game;
using DaggerfallWorkshop.Game.Entity;
using DaggerfallWorkshop.Game.UserInterface;
using HarmonyLib; //This must be included. Add a reference to 0Harmony.dll to solution.
using UnityEngine; //This also must be included to use any kind of Unity functionality in your replcement method. Add reference to "UnityEngine.CoreModule.dll" in the solution.



//Namespace MUST be DFU_Harmony
namespace DFU_Harmony
{

    //Namespace MUST be DFU_Patch
    public class DFU_Patch
    {

        //Name method whatever you like. I've only tested it with public static void. You will need to experiment with others.
        public static void MyReplacementMethod(PlayerEntity playerEntity, bool suppress = false)
        {
            TextLabel[] armourLabels = new TextLabel[DaggerfallEntity.NumberBodyParts];

            //Code for your replacement method goes here
            for (int bpIdx = 0; bpIdx < DaggerfallWorkshop.Game.Entity.DaggerfallEntity.NumberBodyParts; bpIdx++)
            {
                int armorMod = playerEntity.DecreasedArmorValueModifier - playerEntity.IncreasedArmorValueModifier;

                sbyte av = playerEntity.ArmorValues[bpIdx];
                int bpAv = (100 - av) / 5 + armorMod;
                string bpAVTest = "Test (+#&%))";
                armourLabels[bpIdx].Text = (!suppress) ? bpAVTest : string.Empty;

                if (armorMod < 0)
                    armourLabels[bpIdx].TextColor = DaggerfallUI.DaggerfallUnityStatDrainedTextColor;
                else if (armorMod > 0)
                    armourLabels[bpIdx].TextColor = DaggerfallUI.DaggerfallUnityStatIncreasedTextColor;
                else
                    armourLabels[bpIdx].TextColor = DaggerfallUI.DaggerfallDefaultTextColor;
            }
        }


        //Method name MUST be DFUReplace_Start
        public static void DFUReplace_Start()
        {
            var harmony = new Harmony("com.shortbeard.harmonymodding.harmonytest"); //Doesn't matter, as long as it's in this format and unique
            Type PaperDollType = typeof(PaperDoll); //First, get the type of the class which contains the method you want to replace. As long as the DLL solution has a reference to Assembly-CSharp then this will compile without errors.
            var prefix = typeof(DFU_Patch).GetMethod("MyReplacementMethod"); //Create an object which stores our replacement method
            var original = PaperDollType.GetMethod("RefreshArmourValues");

            harmony.Patch(original, new HarmonyMethod(prefix)); //Harmony applies the patch, by giving it the arguments of the original method and the replacement. 
        }
    }
}
