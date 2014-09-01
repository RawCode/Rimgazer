using RimWorld;

namespace ColonistCreationMod
{
    public class StoryTrait
    {
        #region Variables

        private string _TraitName;
        private string _TraitDescription;
        private TraitEffect _Effect;

        #endregion

        #region Properties

        public string TraitName
        {
            get { return _TraitName; }
            set { _TraitName = value; }
        }

        public string TraitDescription
        {
            get { return _TraitDescription; }
            set { _TraitDescription = value; }
        }

        public TraitEffect Effect
        {
            get { return _Effect; }
            set { _Effect = value; }
        }

        #endregion

        #region Constructor

        public StoryTrait()
        {
            
        }

        #endregion
    }
}
