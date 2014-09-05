using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RC.Rimgazer.Event
{
    /*
        This enumeration used to describe how handler should work

        Default params are:

        Priority = Normal
        Activation stage = StageEntry
        Binding strategy = BindToType
        AllowExceptions = false

    */

    [Flags]
    enum EventHandlerFlags : int
    {
        Nothing         = 0x000,
        //Priority
        Internal        = 0x001,   //Only single Internal handler is applicable per event
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
        Low             = 0x002,   //Invoked before Normal and High
        Normal          = 0x000,   //Invoked after Low and before High
        High            = 0x004,   //Invoked after Low and Normal
        //Activation stage
        StageGameplay   = 0x008,   //for types that may not construct at StageEntry
                                   //types of delayed construction may not register StageEntry events
        StageEntry      = 0x000,   //by default all types resolved at StageEntry
        //Binding strategy
        BindToField     = 0x010,   //Handler will bind to field defined by annotation
                                   //its up to developer to construct object and place it to given field
                                   //empty field will cause NPE on event fire
        BindToSelf      = 0x020,   //Handler will recieve its own instance of hosting type
        BindToMethod    = 0x040,   //System will pass control to method named in annotation
                                   //no other actions are performed
                                   //given method invoked after all normal handlers of priority are registered
                                   //can be used to deregister unwanter handlers from other mods
        BindToType      = 0x080,   //Single instance of type constructed and shared by all handlers

        AllowExceptions = 0x100,   //by default system will automatically disable faulty handlers
                                   //actions "punished" with removal are:

                                   //execution longer then 1000ms inside main thread
                                   //runtime exceptions
                                   //event fire recursion

                                   //setting this flag will allow handler to stay, ever if this will cause
                                   //entire game to crash
    }
}
