using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RC.Rimgazer
{
    [PersistentComment("This is code comment visible in decompiled classes.")]
    public class PersistentCommentAttribute : Attribute
    {
        public PersistentCommentAttribute(string PersistentCommentData)
        {
        }
    }
}
