using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Verse;
using RimWorld;

namespace ColonistCreationMod
{
    public class Colonist_Skill
    {
        private string _SkillName;
        private int _SkillValue;
        private int _SkillPassion;
        private string _SkillDescription;

        public string SkillName
        {
            get { return _SkillName; }
            set { _SkillName = value; }
        }

        public int SkillValue
        {
            get { return _SkillValue; }
            set { _SkillValue = value; }
        }

        public int SkillPassion
        {
            get { return _SkillPassion; }
            set { _SkillPassion = value; }
        }

        public string SkillDescription
        {
            get { return _SkillDescription; }
            set { _SkillDescription = value; }
        }

        public Colonist_Skill()
        {

        }
    }
}
