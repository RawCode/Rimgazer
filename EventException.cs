using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace RC.Rimgazer.Event
{
    public class EventException
    {
        static private readonly Dictionary<string, List<string>> registeredExceptions =
                            new Dictionary<string, List<string>>();

        static public void pushException(string Exception, string channel = "default")
        {
            List<string> store = null;
            if (!registeredExceptions.ContainsKey(channel))
            {
                store = new List<string>();
                registeredExceptions.Add(channel,store);
            }
            registeredExceptions.TryGetValue(channel, out store);
            store.Add(Exception);
        }

        static public string popException(string channel = "default")
        {
            List<string> store = null;
            if (!registeredExceptions.ContainsKey(channel))
                return "No such channel is found.";
            registeredExceptions.TryGetValue(channel, out store);
            string Text = store.Last<string>();
            store.Remove(Text);
            return Text;
        }

        static public List<string> allExceptions(string channel = "default")
        {
            List<string> store = new List<string>();
            registeredExceptions.TryGetValue(channel, out store);
            return store;
        }
        static public void terminateChannel(string channel = "default")
        {
            registeredExceptions.Remove(channel);
        }

    }
}