// Copyright (C) 2014 - 2015 Stephan Bouchard - All Rights Reserved
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms


using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace TMPro
{
    [Serializable]
    public class TMP_FontAsset : TMP_Asset
    {
        public enum FontAssetTypes { None = 0, SDF = 1, Bitmap = 2 };
        public FontAssetTypes fontAssetType;
        
        /// <summary>
        /// The general information about the font.
        /// </summary>
        public FaceInfo fontInfo
        { get { return m_fontInfo; } }

        [SerializeField]
        private FaceInfo m_fontInfo;
        public int fontHashCode;

        [SerializeField]
        public Texture2D atlas; // Should add a property to make this read-only.

        [SerializeField]
        public Material material; // Should add a property to make this read-only.
        public int materialHashCode;

        // Glyph Info

        [SerializeField]
        private List<TMP_Glyph> m_glyphInfoList;

        public Dictionary<int, TMP_Glyph> characterDictionary
        { get { return m_characterDictionary; } }

        private Dictionary<int, TMP_Glyph> m_characterDictionary;


        // Kerning 
        public Dictionary<int, KerningPair> kerningDictionary
        { get { return m_kerningDictionary; } }

        private Dictionary<int, KerningPair> m_kerningDictionary;

        public KerningTable kerningInfo
        { get { return m_kerningInfo; } }

        [SerializeField]
        private KerningTable m_kerningInfo;

        [SerializeField]
        #pragma warning disable 0169 // Property is used to create an empty Kerning Pair in the editor.
        private KerningPair m_kerningPair;  // Used for creating a new kerning pair in Editor Panel.


        // Line Breaking Characters
        public LineBreakingTable lineBreakingInfo
        { get { return m_lineBreakingInfo; } }

        [SerializeField]
        private LineBreakingTable m_lineBreakingInfo;


        [SerializeField]
        public FontCreationSetting fontCreationSettings;


        //[SerializeField]
        //public bool propertiesChanged = false;


        private int[] m_characterSet; // Array containing all the characters in this font asset.

        public float normalStyle = 0;
        public float normalSpacingOffset = 0;

        public float boldStyle = 0.75f;
        public float boldSpacing = 7f;
        public byte italicStyle = 35;
        public byte tabSize = 10;

        private byte m_oldTabSize;


        void OnEnable()
        {
            //Debug.Log("OnEnable has been called on " + this.name);

            if (m_characterDictionary == null)
            {
                //Debug.Log("Loading Dictionary for " + this.name);
                ReadFontDefinition();
            }
            else
            {
                //Debug.Log("Dictionary for " + this.name + " has already been Initialized.");
            }
        }


        void OnDisable()
        {
            //Debug.Log("TextMeshPro Font Asset [" + this.name + "] has been disabled!");      
        }


        void OnValidate()
        {
            if (m_oldTabSize != tabSize)
            {
                m_oldTabSize = tabSize;
                ReadFontDefinition();
            }
        }


        public void AddFaceInfo(FaceInfo faceInfo)
        {
            m_fontInfo = faceInfo;
        }


        public void AddGlyphInfo(TMP_Glyph[] glyphInfo)
        {
            m_glyphInfoList = new List<TMP_Glyph>();
            int characterCount = glyphInfo.Length;

            m_fontInfo.CharacterCount = characterCount;
            m_characterSet = new int[characterCount];

            for (int i = 0; i < characterCount; i++)
            {
                TMP_Glyph g = new TMP_Glyph();
                g.id = glyphInfo[i].id;
                g.x = glyphInfo[i].x;
                g.y = glyphInfo[i].y;
                g.width = glyphInfo[i].width;
                g.height = glyphInfo[i].height;
                g.xOffset = glyphInfo[i].xOffset;
                g.yOffset = (glyphInfo[i].yOffset) + m_fontInfo.Padding; // Padding added to keep baseline at Y = 0.  
                g.xAdvance = glyphInfo[i].xAdvance;

                m_glyphInfoList.Add(g);

                // While iterating through list of glyphs, find the Descender & Ascender for this GlyphSet.
                //m_fontInfo.Ascender = Mathf.Max(m_fontInfo.Ascender, glyphInfo[i].yOffset);
                //m_fontInfo.Descender = Mathf.Min(m_fontInfo.Descender, glyphInfo[i].yOffset - glyphInfo[i].height);
                //Debug.Log(m_fontInfo.Ascender + "  " + m_fontInfo.Descender);
                m_characterSet[i] = g.id; // Add Character ID to Array to make it easier to get the kerning pairs.
            }

            // Sort List by ID.
            m_glyphInfoList = m_glyphInfoList.OrderBy(s => s.id).ToList();
        }


        public void AddKerningInfo(KerningTable kerningTable)
        {
            m_kerningInfo = kerningTable;
        }


        public void ReadFontDefinition()
        {
            //Debug.Log("Reading Font Definition for " + this.name + ".");
            // Make sure that we have a Font Asset file assigned.   
            if (m_fontInfo == null)
            {
                return;
            }

            // Check Font Asset type
            //Debug.Log(name + "   " + fontAssetType);

            // Create new instance of GlyphInfo Dictionary for fast access to glyph info.
            m_characterDictionary = new Dictionary<int, TMP_Glyph>();
            foreach (TMP_Glyph glyph in m_glyphInfoList)
            {
                if (!m_characterDictionary.ContainsKey(glyph.id))
                    m_characterDictionary.Add(glyph.id, glyph);
            }


            //Debug.Log("PRE: BaseLine:" + m_fontInfo.Baseline + "  Ascender:" + m_fontInfo.Ascender + "  Descender:" + m_fontInfo.Descender); // + "  Centerline:" + m_fontInfo.CenterLine);

            TMP_Glyph temp_charInfo = new TMP_Glyph();

            // Add Character (10) LineFeed, (13) Carriage Return & Space (32) to Dictionary if they don't exists.
            if (m_characterDictionary.ContainsKey(32))
            {
                m_characterDictionary[32].width = m_characterDictionary[32].xAdvance; // m_fontInfo.Ascender / 5;
                m_characterDictionary[32].height = m_fontInfo.Ascender - m_fontInfo.Descender;
                m_characterDictionary[32].yOffset= m_fontInfo.Ascender;
            }
            else
            {
                //Debug.Log("Adding Character 32 (Space) to Dictionary for Font (" + m_fontInfo.Name + ").");
                temp_charInfo = new TMP_Glyph();
                temp_charInfo.id = 32;
                temp_charInfo.x = 0; 
                temp_charInfo.y = 0;
                temp_charInfo.width = m_fontInfo.Ascender / 5;
                temp_charInfo.height = m_fontInfo.Ascender - m_fontInfo.Descender;
                temp_charInfo.xOffset = 0; 
                temp_charInfo.yOffset = m_fontInfo.Ascender; 
                temp_charInfo.xAdvance = m_fontInfo.PointSize / 4;
                m_characterDictionary.Add(32, temp_charInfo);
            }

            // Add Non-Breaking Space (160)
            if (!m_characterDictionary.ContainsKey(160))
            {
                temp_charInfo = TMP_Glyph.Clone(m_characterDictionary[32]);
                m_characterDictionary.Add(160, temp_charInfo);
            }


            if (m_characterDictionary.ContainsKey(10) == false)
            {
                //Debug.Log("Adding Character 10 (Linefeed) to Dictionary for Font (" + m_fontInfo.Name + ").");

                temp_charInfo = new TMP_Glyph();
                temp_charInfo.id = 10;
                temp_charInfo.x = 0; // m_characterDictionary[32].x;
                temp_charInfo.y = 0; // m_characterDictionary[32].y;
                temp_charInfo.width = 10; // m_characterDictionary[32].width;
                temp_charInfo.height = m_characterDictionary[32].height;
                temp_charInfo.xOffset = 0; // m_characterDictionary[32].xOffset;
                temp_charInfo.yOffset = m_characterDictionary[32].yOffset;
                temp_charInfo.xAdvance = 0;
                m_characterDictionary.Add(10, temp_charInfo);

                if (!m_characterDictionary.ContainsKey(13))
                    m_characterDictionary.Add(13, temp_charInfo);
            }

            // Add Tab Character to Dictionary. Tab is Tab Size * Space Character Width.
            if (m_characterDictionary.ContainsKey(9) == false)
            {
                //Debug.Log("Adding Character 9 (Tab) to Dictionary for Font (" + m_fontInfo.Name + ").");

                temp_charInfo = new TMP_Glyph();
                temp_charInfo.id = 9;
                temp_charInfo.x = m_characterDictionary[32].x;
                temp_charInfo.y = m_characterDictionary[32].y;
                temp_charInfo.width = m_characterDictionary[32].width * tabSize + (m_characterDictionary[32].xAdvance - m_characterDictionary[32].width) * (tabSize - 1);
                temp_charInfo.height = m_characterDictionary[32].height;
                temp_charInfo.xOffset = m_characterDictionary[32].xOffset;
                temp_charInfo.yOffset = m_characterDictionary[32].yOffset;
                temp_charInfo.xAdvance = m_characterDictionary[32].xAdvance * tabSize;
                m_characterDictionary.Add(9, temp_charInfo);
            }

            // Centerline is located at the center of character like { or in the middle of the lowercase o.
            //m_fontInfo.CenterLine = m_characterDictionary[111].yOffset - m_characterDictionary[111].height * 0.5f;

            // Tab Width is using the same xAdvance as space (32).
            m_fontInfo.TabWidth = m_characterDictionary[9].xAdvance;

            // Adjust Font Scale for compatibility reasons
            if (m_fontInfo.Scale == 0)
                m_fontInfo.Scale = 1.0f;

            // Populate Dictionary with Kerning Information
            m_kerningDictionary = new Dictionary<int, KerningPair>();
            List<KerningPair> pairs = m_kerningInfo.kerningPairs;

            //Debug.Log(m_fontInfo.Name + " has " + pairs.Count +  " Kerning Pairs.");
            for (int i = 0; i < pairs.Count; i++)
            {
                KerningPair pair = pairs[i];
                KerningPairKey uniqueKey = new KerningPairKey(pair.AscII_Left, pair.AscII_Right);

                if (m_kerningDictionary.ContainsKey(uniqueKey.key) == false)
                    m_kerningDictionary.Add(uniqueKey.key, pair);
                else
                    Debug.Log("Kerning Key for [" + uniqueKey.ascii_Left + "] and [" + uniqueKey.ascii_Right + "] already exists.");
            }

            // Add Line Breaking Characters 
            m_lineBreakingInfo = new LineBreakingTable();
            
            
            TextAsset leadingCharFile = Resources.Load("LineBreaking Leading Characters", typeof(TextAsset)) as TextAsset;
            if (leadingCharFile != null)
                m_lineBreakingInfo.leadingCharacters = GetCharacters(leadingCharFile);

            TextAsset followingCharFile = Resources.Load("LineBreaking Following Characters", typeof(TextAsset)) as TextAsset;
            if (followingCharFile != null)
                m_lineBreakingInfo.followingCharacters = GetCharacters(followingCharFile);


            // Compute Hashcode for the font asset name
            fontHashCode = TMP_TextUtilities.GetSimpleHashCode(this.name);

            // Compute Hashcode for the material name
            materialHashCode = TMP_TextUtilities.GetSimpleHashCode(material.name);

        }


        // Get the characters from the line breaking files
        private Dictionary<int, char> GetCharacters(TextAsset file)
        {                      
            Dictionary<int, char> dict = new Dictionary<int, char>();
            string text = file.text;

            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                // Check to make sure we don't include duplicates
                if (dict.ContainsKey((int)c) == false)
                {
                    dict.Add((int)c, c);
                    //Debug.Log("Adding [" + (int)c + "] to dictionary.");
                }
                //else
                //    Debug.Log("Character [" + text[i] + "] is a duplicate.");
            }          
            
            return dict;
        }


        /// <summary>
        /// Function to check if a certain character exists in the font asset.
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        public bool HasCharacter(int character)
        {
            if (m_characterDictionary == null)
                return false;

            if (m_characterDictionary.ContainsKey(character))
                return true;

            return false;
        }


        /// <summary>
        /// Function to check if a certain character exists in the font asset.
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        public bool HasCharacter(char character)
        {
            if (m_characterDictionary == null)
                return false;

            if (m_characterDictionary.ContainsKey(character))
                return true;

            return false;
        }


        /// <summary>
        /// Function to check if certain characters exists in the font asset. Function returns a list of missing characters.
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        public bool HasCharacters(string text, out List<char> missingCharacters)
        {
            if (m_characterDictionary == null)
            {
                missingCharacters = null;
                return false;
            }

            missingCharacters = new List<char>();

            for (int i = 0; i < text.Length; i++)
            {
                if (!m_characterDictionary.ContainsKey(text[i]))
                    missingCharacters.Add(text[i]);
            }

            if (missingCharacters.Count == 0)
                return true;

            return false;
        }

    }
}