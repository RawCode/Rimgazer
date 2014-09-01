using System;
using System.Collections.Generic;

using RimWorld;
using Verse;
using UnityEngine;

namespace ColonistCreationMod
{
    public class Colonist
    {
        #region Variables

        private string _FirstName;
        private string _NickName ;
        private string _LastName;
        private int _Gender;
        private int _Age;
        private int _SkillPool;
        private List<Colonist_Skill> _Skills;
        private List<Colonist_Backstory> _Backstory;
        private List<StoryTrait> _Traits;
        private List<Clothing> _Clothing;
        private BodyType bodyType;
        private string skinGraphicPath = string.Empty;
        private Color skinColor;
        private Color hairColor;
        private string headGraphicPath = string.Empty;
        private CrownType crownType;
        private ColHairDef hairDef;
        private string weapon;

        #endregion

        #region Properties

        public string FirstName
        {
            get { return _FirstName; }
            set { _FirstName = value; }
        }

        public string NickName
        {
            get { return _NickName; }
            set { _NickName = value; }
        }

        public string LastName
        {
            get { return _LastName; }
            set { _LastName = value; }
        }

        public int Gender
        {
            get { return _Gender; }
            set { _Gender = value; }
        }

        public int Age
        {
            get { return _Age; }
            set { _Age = value; }
        }

        public int SkillPool
        {
            get { return _SkillPool; }
            set { _SkillPool = value; }
        }

        public List<Colonist_Skill> Skills
        {
            get { return _Skills; }
            set { _Skills = value; }
        }

        public List<Colonist_Backstory> Backstory
        {
            get { return _Backstory; }
            set { _Backstory = value; }
        }

        public List<StoryTrait> Traits
        {
            get { return _Traits; }
            set { _Traits = value; }
        }

        public List<Clothing> Clothing
        {
            get { return _Clothing; }
            set { _Clothing = value; }
        }

        public BodyType BodyType
        {
            get { return bodyType; }
            set { bodyType = value; }
        }

        public string SkinGraphicPath
        {
            get { return skinGraphicPath; }
            set { skinGraphicPath = value; }
        }

        public Color SkinColor
        {
            get { return skinColor; }
            set { skinColor = value; }
        }

        public Color HairColor
        {
            get { return hairColor; }
            set { hairColor = value; }
        }

        public string HeadGraphicPath
        {
            get { return headGraphicPath; }
            set { headGraphicPath = value; }
        }

        public CrownType CrownType
        {
            get { return crownType; }
            set { crownType = value; }
        }

        public ColHairDef HairDef
        {
            get { return hairDef; }
            set { hairDef = value; }
        }

        public string Weapon
        {
            get { return weapon; }
            set { weapon = value; }
        }

        #endregion

        #region Constructors

        public Colonist()
        {
            
        }

        #endregion

    }
}
