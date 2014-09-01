using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Verse;

namespace ColonistCreationMod
{
    public static class ModdedGenText
    {
        public static string Possessive(this Colonist c)
        {
            if (c.Gender == 1)
            {
                return "Prohis".Translate();
            }
            return "Proher".Translate();
        }

        public static string PossessiveCap(this Colonist c)
        {
            if (c.Gender == 1)
            {
                return "ProhisCap".Translate();
            }
            return "ProherCap".Translate();
        }

        public static string ProObj(this Colonist c)
        {
            if (c.Gender == 1)
            {
                return "ProhimObj".Translate();
            }
            return "ProherObj".Translate();
        }

        public static string ProObjCap(this Colonist c)
        {
            if (c.Gender == 1)
            {
                return "ProhimObjCap".Translate();
            }
            return "ProherObjCap".Translate();
        }

        public static string ProSubj(this Colonist c)
        {
            if (c.Gender == 1)
            {
                return "Prohe".Translate();
            }
            return "Proshe".Translate();
        }

        public static string ProSubjCap(this Colonist c)
        {
            if (c.Gender == 1)
            {
                return "ProheCap".Translate();
            }
            return "ProsheCap".Translate();
        }

        public static string TextAdjustedFor(Colonist c, string baseText)
        {
            return baseText.Replace("NAME", c.NickName).Replace("HISCAP", c.PossessiveCap()).Replace("HIMCAP", c.ProObjCap()).Replace("HECAP", c.ProSubjCap()).Replace("HIS", c.Possessive()).Replace("HIM", c.ProObj()).Replace("HE", c.ProSubj());
        }
    }
}
