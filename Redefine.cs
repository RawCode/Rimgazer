using RC.Rimgazer.Gaze;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Verse;

namespace RC.Rimgazer
{
    /*

    Root level class (RLC)

    At static level this class trigger "Entry" event and initialize system.
    At instance level this class resolve def database injections.

    Entry stage happes as soon as game is started or as soon as Rimgazer mod is activated
    Entry event happens in pregame GUI

    Def database injections are moddable, every injection will trigger event
    Initialize event triggered before modifications
    Finalize after modifications

    Rimgazer injections NOT hardcoded and stored in XML defs

    Main uses of this stage:
    1) Temper with pregame GUI, alter world generation or any other pregame process
    2) Temper with def database at level not possible with simple XML defs
    3) Perform unspecified actions

    This class do require XML def in order to work properly, system work on static level, at least
    one instance is required, more possible but not required

    Type that perform dedefines must have valid fieldset

    Used only to alter ThingDef instances and nothing else at this moment
    
    <Redefines>
	    <Redefine>
        params and keys
	    </Redefine>
    </Redefines>
    */

    public class Redefine : Def
    {
        static Redefine lastToProcess      = null;
        static FieldInfo currentFile       = typeof(XmlLoader).GetField("loadingAsset", (BindingFlags)40);
        static int redefineID              = 0;
        static LoadableXmlAsset latestFile = null;
        static List<Redefine> stageBPool   = new List<Redefine>();

        //Instance fields section.
        //Multiple fields can be used at once:
        //Selecting Field, Value and Mod allows to replace content of matching fields if they have matching value
        //and def is defined by matching mod.

        public bool   isDisabled  = false;

        public string targetDef   = ""; //Defs with matching names.
        public string targetMod   = ""; //Defs from specific mod.
        public string targetType  = ""; //Defs of specific type.

        public string targetField = ""; //Specific field.
        public string targetValue = ""; //Specific value.

        public string replaceWith = ""; //String automatically resolved to valid type.

        public int    payloadFlag = 0 ; //for non default field modify action.
        public string payloadData = ""; //allow to copy, replace of remove defs from database.


        public static int TEST()
        {
            return 42;
        }

        unsafe public sealed override void ResolveReferences()
        {
            Log.Warning("x86 override test 1");
            Log.Warning(TEST().ToString());

            byte* mpx_1 = (byte*)typeof(Redefine).GetMethod ("TEST", BindingFlags.Static | BindingFlags.Public).MethodHandle.GetFunctionPointer ().ToPointer();
            
            *(mpx_1 + 0) = 0xB8; //mov eax,0x0
            *(mpx_1 + 1) = 7;
            *(mpx_1 + 2) = 2;
            *(mpx_1 + 3) = 2;
            *(mpx_1 + 4) = 1;
            *(mpx_1 + 5) = 0xC3; //ret

            Log.Warning("x86 override test 2");
            Log.Warning(TEST().ToString());


            Log.Warning("have a nice day!");

            //StageA();
        }

        public Mod ownerOfDef()
        {


            return null;
        }

        public override void PostLoad()
        {
            //Override this if you want to name your Redefines in other manner.
            //Modifications to DefDatabase are not realible at this stage.
            LoadableXmlAsset tmp = (LoadableXmlAsset)currentFile.GetValue(null);

            if (latestFile != tmp)
            {
                redefineID = 0;
                latestFile = tmp;
            }else
                redefineID++;

            this.defName = latestFile.name.Replace(".xml", "") + "_" + redefineID;
        }

        public virtual void StageA()
        {
            //StageA performed after all ThingDefs are finalized.
            if (lastToProcess == null)
            {
                //Firstrun routine, required for proper StageB initialization.
                lastToProcess = DefDatabase<Redefine>.AllDefsListForReading.Last();
                Log.Warning("Static Stage 0");
            }
            
            //Both disabled and active redefines at this stage generate transactions.
            //After all transactions are generated, redefines registered for StageB
            //given chance to review and modify transactions.
            //After all changes are done, transactions merged following priority rules and then merged with database
            Log.Warning(this.ToString() + " Stage 1");



            if (this == lastToProcess)
            {
                //StageA is finished
                //Starting StageB
                Log.Warning("Static Stage 2");

                //ThingDef humanovveride = ThingDef.Named("Human");
                //humanovveride.thingClass = typeof(GazePawn);
            }

        }

        public void SubscribeToStageB()
        {

        }
        
        public virtual void StageB()
        {
            //StageB performed only for defs subscribed to it
            //This stage respect EventHandler flags
            //Designed to instrument redefine transactions and perform conditional modifications

            //Do nothing at this time
        }


        /*
                Log.Error(Assembly.GetExecutingAssembly().GetModules(false)[0].ScopeName);


                string s = Path.Combine(Application.persistentDataPath, "cfg");
                s = Path.Combine(s, "store");
                Log.Error(s);


                Assembly asm = Assembly.GetExecutingAssembly();
                object[] data = asm.GetCustomAttributes(false);

                foreach (object az in data)
                {
                    Log.Error(az.ToString());
                }


                var attribs = (asm.GetCustomAttributes(typeof(AssemblyProductAttribute), true));
                string ssz = (attribs[0] as AssemblyProductAttribute).Product;
                Log.Error(ssz);

                foreach (Mod m in LoadedModManager.LoadedMods)
                {
                    if (m.name.Equals(ssz))
                    {
                        //Log.Error(m.RootFolder);
                    }
                }
                */
    }
}
