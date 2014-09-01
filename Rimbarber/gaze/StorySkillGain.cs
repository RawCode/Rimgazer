namespace ColonistCreationMod
{
    public class StorySkillGain
    {
        #region Variables

        private string _SkillName;
        private int _SkillValue;

        #endregion

        #region Properties

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

        #endregion

        #region Constructor

        public StorySkillGain(string SkillName, int SkillValue)
        {
            this._SkillName = SkillName;
            this._SkillValue = SkillValue;
        }

        public StorySkillGain()
        {

        }

        #endregion
    }
}
