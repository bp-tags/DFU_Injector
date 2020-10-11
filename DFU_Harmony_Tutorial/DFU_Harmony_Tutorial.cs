//////////////////////////////////////////////////////////////////
/// DFU Injector tutorial by ShortBeard - 9/10/2020.
/// This tutorial shows how to replace the "PlayActivateSound" method in the FPSWeapon class with an arbitrary method of our choosing.
///////////////////////////////////////////////////////////////////


using System; //This must be included 
using HarmonyLib; //This must be included. Add a reference to 0Harmony.dll to solution.
using UnityEngine; //This also must be included to use any kind of Unity functionality in your replcement method. Add reference to "UnityEngine.CoreModule.dll" in the solution.



//Namespace MUST be DFU_Harmony
namespace DFU_Harmony
{

    //Namespace MUST be DFU_Patch
    public class DFU_Patch
    {

        //Name method whatever you like. I've only tested it with public static void. You will need to experiment with others.
        public static void MyReplacementMethod()
        {
            //Code for your replacement method goes here
            GameObject playerObj = GameObject.Find("PlayerAdvanced");
            playerObj.transform.position = new Vector3(playerObj.transform.position.x + 100, playerObj.transform.position.y + 100, playerObj.transform.position.z + 100);
        }


        //Method name MUST be DFUReplace_Start
        public static void DFUReplace_Start()
        {
            var harmony = new Harmony("com.shortbeard.harmonymodding.harmonytest"); //Doesn't matter, as long as it's in this format and unique
            Type FpsWeaponType = typeof(DaggerfallWorkshop.Game.FPSWeapon); //First, get the type of the class which contains the method you want to replace. As long as the DLL solution has a reference to Assembly-CSharp then this will compile without errors.
            var prefix = typeof(DFU_Patch).GetMethod("MyReplacementMethod"); //Create an object which stores our replacement method
            var original = FpsWeaponType.GetMethod("PlayActivateSound"); //Get a reference to the original method we want to replace

            harmony.Patch(original, new HarmonyMethod(prefix)); //Harmony applies the patch, by giving it the arguments of the original method and the replacement. 
        }
    }
}
