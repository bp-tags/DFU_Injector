//using HarmonyLib;
using System;
using System.IO;
using System.Reflection;
using System.Xml;

namespace DFU_Injector
{
    class Program
    {

        /// <summary>
        /// Begin iterating over the folders found in the injector_mods directory
        /// </summary>
        /// <param name="injectorModPath">Injector mods directory</param>
        private static void ProcessModDirectories(string injectorModPath)
        {
            string[] directories = Directory.GetDirectories(injectorModPath);
            foreach (string injectorModDirectory in directories)
            {
                ReadModInformation(injectorModDirectory);
            }
        }


        /// <summary>
        /// Check for a valid mod DLL. Right now, this only just checks if the file exists, but other potential functionality can be added later.
        /// </summary>
        /// <param name="dllName">Name of the DLL file</param>
        /// <param name="modDirectory">Location of the DLL</param>
        /// <returns></returns>
        private static bool CheckDLLValidity(string dllName, string modDirectory)
        {
            if (File.Exists(Path.Combine(modDirectory, dllName)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Run the DLL of the found mod
        /// </summary>
        /// <param name="modDLL">Assembly to execute</param>
        private static void ExecuteDLL(Assembly modDLL)
        {
            Type type = modDLL.GetType("DFU_Harmony.DFU_Patch");
            type.GetMethod("DFUReplace_Start").Invoke(Activator.CreateInstance(type), null);
        }


        /// <summary>
        /// Read the Mod Info from the XML file that accompanies the DLL
        /// </summary>
        /// <param name="modDirectory"></param>
        private static void ReadModInformation(string modDirectory)
        {
            string[] modXmlFiles = System.IO.Directory.GetFiles(modDirectory, "*.xml"); //Normally, a mod would only have one XML file associated, but this functionality allows for multiple to be checked in any given mod directory for future circumstances

            foreach (string modXmlInfo in modXmlFiles)
            {
                try
                {
                    XmlDocument modInformation = new XmlDocument();
                    modInformation.Load(modXmlInfo);

                    //Try to read the expected data from an DFU_Injector XML file

                    XmlNode modNameNode = modInformation.DocumentElement.SelectSingleNode("/DFUMod/ModName");
                    XmlNode modFilesNode = modInformation.DocumentElement.SelectSingleNode("/DFUMod/ModFiles");

                    foreach (XmlNode fileNode in modFilesNode)
                    {
                        //outputFile.WriteLine(fileNode.InnerText);
                        if (CheckDLLValidity(fileNode.InnerText, modDirectory) == true)
                        {
                            //Process DLL to inject it into the DFU code
                            Assembly modDLL = Assembly.LoadFrom(Path.Combine(modDirectory, fileNode.InnerText));
                            ExecuteDLL(modDLL);
                        }
                        else
                        {
                            PrintInjectorError("Cannot find DLL file \"" + fileNode.InnerText + "\" in:\r\n" + modDirectory + " \r\nas directed by XML file:\r\n" + modXmlInfo);
                        }
                    }

                }
                catch (Exception ex)
                {
                    PrintInjectorError("The following exception(s) occured when attempting to load: " + modXmlInfo + "\r\n\r\n" + ex);
                }
            }
        }


        /// <summary>
        /// For printing errors
        /// </summary>
        /// <param name="error">Error string</param>
        static void PrintInjectorError(string error)
        {
            string injectorModPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DaggerfallUnity_Data\\StreamingAssets\\Mods\\injector_mods\\");
            FileStream errorLogFile = File.Create(Path.Combine(injectorModPath, ("DFU_Injector_Error_Log.log")));

            using (StreamWriter errorOutput = new StreamWriter(errorLogFile))
            {
                errorOutput.WriteLine(error.ToString());
            }
        }

        /// <summary>
        /// Main method. This runs when targeted by DoorStop.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            string injectorModsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DaggerfallUnity_Data\\StreamingAssets\\Mods\\injector_mods\\");
            if (Directory.Exists(injectorModsPath))
            {
                ProcessModDirectories(injectorModsPath);
            }
        }
    }
}
