using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RC.Rimgazer.Event
{
    /*

    */
    [Flags]
    enum EventHandlerFlags : byte
    {
        //default key apply to all default setting
        //to evoid issues such cases are commented and not allowed to use directly
        //inside handling code listed cases are reflected by corresponding actions
        Default         = 0x00,


        //Priority subsection

        Internal        = 0x01, //Only single Internal handler is applicable per event
                            //If multiple handlers attempt to register - priority is used
                            //If same priority - declaring assembly will be checked
                            //Handlers declared by same assembly as event have lowest priority
                            //If conflict not resolved - NONE will register and exception is thrown
                            //This will happen if multiple mods want to edit same event of other mod

                            //Handler of this priority always invoked first
                            //Internal handler have full control on event processing
                            //this power includes invocation of other handlers
                            //altering event class or suppressing event in unspecified way

                            //By default special method of event is used for this

        Low             = 0x02, //Invoked before Normal and High
        //Normal        = 0x00, //Invoked after Low and before High
        High            = 0x04, //Invoked after Low and Normal

        //activation stage
        StageGameplay   = 0x08, //for types that may not construct at StageEntry
        //StageEntry    = 0x00, //by default all types resolved at StageEntry

        //bind strategy
        BindToField     = 0x100, //Handler will bind to field defined by annotation
                                 //its up to developer to construct object and place it to given field
                                 //empty field will cause exception
        BindToSelf      = 0x200, //Handler will recieve its own instance of hosting type
        //BintToType    = 0x000, //Single instance of type constructed and shared by all handlers

    }
}
