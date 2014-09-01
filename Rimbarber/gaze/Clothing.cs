using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace ColonistCreationMod
{
    public class Clothing
    {
        #region Variables

        private int _Index;
        private string _Layer;
        private string _Label;
        private string _GraphicPath;
        private Color _Color;

        #endregion

        #region Properties

        public int Index
        {
            get { return _Index; }
            set { _Index = value; }
        }

        public string Layer
        {
            get { return _Layer; }
            set { _Layer = value; }
        }

        public string Label
        {
            get { return _Label; }
            set { _Label = value; }
        }

        public string GraphicPath
        {
            get { return _GraphicPath; }
            set { _GraphicPath = value; }
        }

        public Color Color
        {
            get { return _Color; }
            set { _Color = value; }
        }

        #endregion

        #region Constructors

        public Clothing()
        {
            
        }

        #endregion
    }
}
