using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RC.Rimgazer
{
    /*
     This enumeration define how types\methods\fields processed by system

     Default params are:

     Priority            = Normal
     Activation stage    = StageEntry
     Binding strategy    = BindToType
     CatchExceptions     = true (NYI)

     Invalid combinations are IGNORED with WARNING
     Invalid use of INTERNAL flag will cause ERROR
     If BINDTOMETHOD is present, any exception inside given method will cause ERROR

     Flags
         BindToField
         BindToMethod
     require method\field with same combination of flags
     in case of multiple\none fields\methods flag is IGNORED with ERROR

     Logic implementation located inside EventHandlerList class
 */
    [Flags]
    public enum CommonFlags
    {
        Nothing = 0x000,
        //Priority
        Internal = 0x001,   //Only single Internal handler is applicable per event
        //If multiple handlers attempt to register - priority is used
        //If same priority - declaring assembly will be checked
        //Handlers declared by same assembly as event have lowest priority
        //If conflict not resolved - NONE will register and exception is thrown
        //This will happen if multiple mods want to edit same event of other mod
        //Handler of this priority always invoked first
        //Internal handler have full control over event processing
        //it should invoke other handlers, process transactions, push changes
        //it may replace event with other object of compatable class
        //EventFallback class is default internal handler for all events
        //By default special method of event is used for this
        Low = 0x002,   //Invoked before Normal and High
        Normal = 0x000,   //Invoked after Low and before High
        High = 0x004,   //Invoked after Low and Normal
        //Activation stage
        StageGameplay = 0x008,   //for types that may not construct at StageEntry
        //types of delayed construction may not register StageEntry events
        StageEntry = 0x000,   //by default all types resolved at StageEntry
        //Binding strategy
        BindToField = 0x010,   //Handler will bind to field, will search for field with same flags
        BindToSelf = 0x020,   //Handler will recieve its own instance of hosting type

        BindToMethod = 0x040,   //Control passed to arbitrary method
        //no actions done by system, accumulated errors are abandoned
        //will seach for method with same flags

        BindToType = 0x080,   //Single instance of type constructed and shared by all handlers

        CatchExceptions = 0x100,   //By default system will catch errors, this can be disabled
    }
}