using System.Collections.Generic;
using RimWorld;

namespace ColonistCreationMod
{
    public class ColHairDef
    {
        #region Variables

        private string _DefName = "";
        private string _GraphicPath = "";
        private string _Label = "";
        private HairGender _HairGender;
        private List<string> _HairTags;

        #endregion

        #region Properties

        public string DefName
        {
            get { return _DefName; }
            set { _DefName = value; }
        }

        public string GraphicPath
        {
            get { return _GraphicPath; }
            set { _GraphicPath = value; }
        }

        public string Label
        {
            get { return _Label; }
            set { _Label = value; }
        }

        public HairGender HairGender
        {
            get { return _HairGender; }
            set { _HairGender = value; }
        }

        public List<string> HairTags
        {
            get { return _HairTags; }
            set { _HairTags = value; }
        }

        #endregion

        #region Constructors

        public ColHairDef()
        {
            
        }

        #endregion
    }
}
