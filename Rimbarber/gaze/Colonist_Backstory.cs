using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using Verse;
using RimWorld;

namespace ColonistCreationMod
{
    public class Colonist_Backstory
    {
        #region Variables

        private int _StoryAge;
        private int _StoryIndex;
        private string _StoryName;
        private string _StoryDescription;
        private string _BaseDescription;
        private List<StorySkillGain> _SkillGains;
        private List<string> _WorkRestrictions;
        private BodyType bodyTypeGlobal;
        private BodyType bodyTypeMale;
        private BodyType bodyTypeFemale;

        #endregion

        #region Properties

        public int StoryAge
        {
            get { return _StoryAge; }
            set { _StoryAge = value; }
        }

        public int StoryIndex
        {
            get { return _StoryIndex; }
            set { _StoryIndex = value; }
        }

        public string StoryName
        {
            get { return _StoryName; }
            set { _StoryName = value; }
        }

        public string StoryDescription
        {
            get { return _StoryDescription; }
            set { _StoryDescription = value; }
        }

        public string BaseDescription
        {
            get { return _BaseDescription; }
            set { _BaseDescription = value; }
        }

        public List<StorySkillGain> SkillGains
        {
            get { return _SkillGains; }
            set { _SkillGains = value; }
        }

        public List<String> WorkRestrictions
        {
            get { return _WorkRestrictions; }
            set { _WorkRestrictions = value; }
        }

        public BodyType BodyTypeGlobal
        {
            get { return bodyTypeGlobal; }
            set { bodyTypeGlobal = value; }
        }

        public BodyType BodyTypeMale
        {
            get { return bodyTypeMale; }
            set { bodyTypeMale = value; }
        }

        public BodyType BodyTypeFemale
        {
            get { return bodyTypeFemale; }
            set { bodyTypeFemale = value; }
        }

        #endregion

        #region Constructor

        public Colonist_Backstory()
        {

        }

        #endregion

        #region Methods

        public string FullDescriptionFor(List<StorySkillGain> SkillGains, List<string> WorkRestrictions, string desc, Colonist c)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(ModdedGenText.TextAdjustedFor(c, desc));
            stringBuilder.AppendLine();
            stringBuilder.AppendLine();

            foreach (StorySkillGain gain in SkillGains)
            {
                stringBuilder.AppendLine(gain.SkillName + ":   " + gain.SkillValue.ToString("+##;-##"));
            }

            stringBuilder.AppendLine();

            foreach (string current in WorkRestrictions)
            {
                stringBuilder.AppendLine(current + " " + "disabled");
            }

            return stringBuilder.ToString();
        }

        #endregion
    }
}
