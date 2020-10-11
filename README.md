# DFU_Injector
A way to replace methods at run time in Daggerfall Unity using Harmony.
```
https://github.com/bp-tags/UnityDoorstop
https://github.com/bp-tags/Harmony
https://github.com/bp-tags/ILSpy
```
http://forums.dfworkshop.net/viewtopic.php?f=14&p=49062&sid=f9ba8bfc7f3395049eca1b826940e014#p49062
```
DFU Injector
Unread post by ShortBeard » Fri Oct 09, 2020 6:16 am

Hi everyone,
I've attempted to create a solution to be able to replace methods at run time in DFU. It's not elegant, but it's a solution.

Known issues:
Doesn't work on Windows 7 - Fixed

Preface:
This is very much a work in progress. You should expect things not to work properly. This has the potential to mess everything and anything up. Use this at your own risk. Please see the post below with the header "risks" for more information on that. Please also note, this has only been tested on 64 bit versions of Windows.

What DFU Injector?:
In short, DFU Injector will allow modders to overwrite existing in-game methods. Though reflection can be used in .dfmods to access private members, method-changing (as far as I know) has been somewhat untouchable. Note, that this type of mod creation will create mods in an entirely different way, and will use a modders custom DLL files instead of .dfmod files.

How it works:
When Daggerfall Unity launches, a piece of software called "Doorstop" allows code to run right before the "core" of the DFU code runs. This allows arbitrary code execution, and an ability to replace methods at run time using Harmony. I'm not sure if there is a better method, but this is the solution I have come up with. DFU Injector acts as a middle-man between Doorstop and Harmony-based mods.

Should I use this?:
Probably not. Please use the regular .dfmod where possible, as they extend on existing and working functionality of the game. DFU Injector has the potential to break everything if you don't know what you're doing. You also run the risk that your mod may mess up core-functionality after a DFU update. Use this at your own risk. If your game breaks, it's up to you to fix it. If your save file stops working, that's on you. If a mod that someone else has given you breaks, talk to the person who made the mod. This is only intended to provide a platform for more mod customization, what happens beyond that is out of my control.

Requirements for mod developers:
• Decent knowledge of C#
• You know how to compile a DLL
• You know how to reference and include other DLLs
• A basic understanding of XML

Beyond providing DFU Injector and a very basic tutorial so that you know how to use it, it's up to the modders to figure out how to use Harmony to get the results they want. I can provide some outlining, and I will help where I can, but I can't write your whacky mod for you. I've tried to make what is a somewhat complicated process as simple as I can. I am far from an expert, and I am sure there are a million better ways to do it.

Technical requirements:
For this your mod to function correctly with DFU Injector, there's a few things you will need:
• DFU_Injector.dll + Doorstop & Harmony (Provided in a .rar file attached to this post)
• Naming conventions in your mods code so that DFU Injector knows how to work with it (more on this below in the tutorial)
• An XML file that accompanies your DLL file (Contains some basic data about the mod, author, version, etc.) An example XML file is also attached.

-See next post for basic tutorial-
ATTACHMENTS
DFU_Harmony_Tutorial.rar
(1 KiB) Downloaded 1 time
DFU_Injector.rar
(276.87 KiB) Not downloaded yet
TestMod_XML.rar
(228 Bytes) Downloaded 1 time
Last edited by ShortBeard on Sat Oct 10, 2020 8:33 am, edited 15 times in total.
Top
User avatarShortBeard
Posts: 38
Joined: Tue Aug 06, 2019 7:10 am
Contact: Contact ShortBeard
Re: DFU Injector
Unread post by ShortBeard » Fri Oct 09, 2020 6:18 am

Tutorial:

In this tutorial, we will replace the method that would normally play a sound when the player draws his weapon. We will instead replace it with code that teleports the player into the sky. It might be best to have a save game in a town/outdoors to make sure this is working properly for testing purposes.

• Unzip the contents of "DFU_Injector.rar" into the same location as your DaggerfallUnity.exe
• Make a new folder in your existing Mods folder and name it "injector_mods" without quotes.
• Make a new folder in injector_mods, and give it a name that is suitable for your mod. "TestMod" is fine for this tutorial. The name doesn't matter, it's just used by DFU_Injector for organizational purposes.
• Start a new project in your preferred IDE (I'm using Visual Studio 2019) in preparation to create a DLL file.
• Paste in the example code found inside DFU_Harmony_Tutorial.rar attached to the original post. Please note that the Namespace, Class and DFUReplace_Start() method naming conventions must not be altered. "MyReplacementMethod" can be named anything you like, just don't change the names of the rest.
• Add a reference to 0Harmony.dll to your solution. Also add a reference to Assembly-Csharp.dll (this can be found in the DFU directory DaggerfallUnity_Data\Managed)
• If you intend to use any Unity functionality such as transforming positions, etc. - you will also need to add a reference to UnityEngine.CoreModule.dll as I have done in this tutorial.
• Build the DLL. For more specific information about what is going on, read the comments in the tutorial code.
• Place the newly built DLL in the "TestMod" directory created earlier. Also place the XML file in this directory. Make sure that the XML node "FileName" contains the exact name of your DLL including the extension (for example, <FileName>TestMod.dll</FileName>)
• Run DaggerfallUnity.exe as normal, load your saved game, and if everything worked then the player should be teleported into the sky when drawing your weapon instead of it playing a weapon drawing sound.

Issues/Errors:
If it's not working as expected, or if DaggerfallUnity doesn't launch, check the injector_mods folder for an error log file. If no error log is generated here, it's potentially an issue with doorstop running. Your version of winhttp.dll depends on your operating system. You can try re-building winhttp.dll yourself from source (see DoorStop github's page)
Last edited by ShortBeard on Sat Oct 10, 2020 12:36 am, edited 4 times in total.
Top
User avatarShortBeard
Posts: 38
Joined: Tue Aug 06, 2019 7:10 am
Contact: Contact ShortBeard
Re: DFU Injector
Unread post by ShortBeard » Fri Oct 09, 2020 6:19 am

Other notes:
The XML file can contain multiple <FileName> nodes, and will attempt to load all of the DLLs you list in there, if your mod uses multiple. However, please also not that this does not currently contain any kind of priority organization. If two mods attempt to overwrite the same method, it's luck of the draw.


Risks:
As previously mentioned, one of the most obvious risks is you replace a method that DFU itself changes later (such as an update or bug fix) and can mess up some intended functionality. The other big risk is that a DLL provided by a modder can run arbitrary code on your system and leaves you vulnerable to potential malware, etc. If you don't trust a mod, don't use it. If you want to investigate a mod, you can always use ILSpy to inspect the assembly (see references below for link)

Conclusion:
If you have any questions, concerns, requests or if you think this can be done in a better way, please post it in this thread! Perhaps one day the DLL's can just be placed in regular .dfmods and loaded that way instead of needing to be in a separate director. I think the ideal circumstance would be to be able to have .CS source files sitting in there which are compiled by DFU_Injector at run time, to make adjustments more accessible.


References:
Doorstop - https://github.com/NeighTools/UnityDoorstop/releases
Harmony - https://github.com/pardeike/Harmony
DFU Injector Source - https://github.com/ShortBeard/DFU_Injector
ILSpy - https://github.com/icsharpcode/ILSpy
```
