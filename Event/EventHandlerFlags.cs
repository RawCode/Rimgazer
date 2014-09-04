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


        //Priority subsection default Normal

        Internal        = 0x01, 
                            //Only single Internal handler is applicable per event
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

        Low             = 0x02, //Invoked before Normal and High
        //Normal        = 0x00, //Invoked after Low and before High
        High            = 0x04, //Invoked after Low and Normal

        //activation stage default StageEntry
        StageGameplay   = 0x08, //for types that may not construct at StageEntry
                                //types of delayed construction may not register StageEntry events
        //StageEntry    = 0x00, //by default all types resolved at StageEntry

        //bind strategy default BintToType
        BindToField     = 0x100, //Handler will bind to field defined by annotation
                                 //its up to developer to construct object and place it to given field
                                 //empty field will cause NPE on event fire
        BindToSelf      = 0x200, //Handler will recieve its own instance of hosting type
        BindToMethod    = 0x400, //System will pass control to method named in annotation
                                 //no other actions are performed
                                 //given method invoked after all normal handlers of priority are registered
                                 //can be used to deregister unwanter handlers from other mods

        //BindToType    = 0x000, //Single instance of type constructed and shared by all handlers


        //tracking features, disabled by default
        TrackOverflow  = 0x000, //Setting this flag will enable Stackoverflow prediction
                                //Used for handlers that may (or will) trigger event they registered for
                                //if handler A is registered for event B had triggered event B from his body
                                //that new event B instance not passed to handler A again
                                //still passed to other handlers
                                //if handler C triggered event B again and also have TrackOverlow flag
                                //both handlers excluded for next recursion
                                //recursion tracking always on and can be accessed from EventRuntime
                                //without this flag faulty handler will reach stackoverflow and fail

        TrackTime = 0x000,      //if set, will report how long handler took to complete
        
        
        Evilbit = 0x000,        //if set, handler allowed to throw exceptions and block main thread

    }
}
