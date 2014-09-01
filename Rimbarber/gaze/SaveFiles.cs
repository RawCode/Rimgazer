using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Verse;

namespace ColonistCreationMod
{
    public static class SaveFiles
    {
        public static IEnumerable<FileInfo> AllSaveFiles
        {
            get
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(SaveColonists.CharSaveGamesFolderPath);
                return
                    from f in directoryInfo.GetFiles()
                    where f.Extension == ".col"
                    orderby f.LastWriteTime descending
                    select f;
            }
        }
    }
}
