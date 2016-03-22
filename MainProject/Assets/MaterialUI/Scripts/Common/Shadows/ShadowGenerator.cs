//  Copyright 2016 MaterialUI for Unity http://materialunity.com
//  Please see license file for terms and conditions of use, and more information.

#if UNITY_EDITOR
using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.IO;

namespace MaterialUI
{
    [ExecuteInEditMode]
    public class ShadowGenerator : MonoBehaviour
    {
		//	Unwise to change this
		private const float shadowDarkenAmount = 0.97f;

        [HideInInspector]
        [SerializeField]
        private string m_Guid;

        [SerializeField]
        private static string m_GeneratedShadowsDirectory = "GeneratedShadows";
		public static string generatedShadowsDirectory
		{
			get { return m_GeneratedShadowsDirectory; }
			set { m_GeneratedShadowsDirectory = value; }
		}

        private bool m_IsDone;
		public bool isDone
		{
			get { return m_IsDone; }
			set { m_IsDone = value; }
		}

        //	The image that the shadow will be made from (This is a uGUI Image, *NOT* Texture or Sprite)
        private Image m_SourceImage;
		public Image sourceImage
		{
			get { return m_SourceImage; }
			set { m_SourceImage = value; }
		}

        //	The pixel range of each blur pass.
        [Range(0, 5)]
        [SerializeField]
        private int m_BlurRange = 2;
		public int blurRange
		{
			get { return m_BlurRange; }
			set { m_BlurRange = value; }
		}

        //	Number of blur passes
        private int m_BlurIterations = 2;
		public int blurIterations
		{
			get { return m_BlurIterations; }
			set { m_BlurIterations = value; }
		}

        //	Desired changes to shadow position, size and transparency
        private Vector3 m_ShadowRelativePosition = new Vector3(0f, -1f, 0f);
		public Vector3 shadowRelativePosition
		{
			get { return m_ShadowRelativePosition; }
			set { m_ShadowRelativePosition = value; }
		}

        private Vector2 m_ShadowRelativeSize = new Vector2(-2f, -2f);
		public Vector2 shadowRelativeSize
		{
			get { return m_ShadowRelativeSize; }
			set { m_ShadowRelativeSize = value; }
		}

        [Range(0, 1)]
        [SerializeField]
        private float m_ShadowAlpha = 0.5f;
		public float shadowAlpha
		{
			get { return m_ShadowAlpha; }
			set { m_ShadowAlpha = value; }
		}

        [Tooltip("Default size to add to Texture to fit shadow blur. If you know for sure you won't be using large shadows, you can lower this for performance")]
        [SerializeField]
        private int m_ImagePadding = 32;
		public int imagePadding
		{
			get { return m_ImagePadding; }
			set { m_ImagePadding = value; }
		}

        //	Used to keep track of the source/destination assets/references
        private Sprite m_SourceSprite;
		public Sprite sourceSprite
		{
			get { return m_SourceSprite; }
			set { m_SourceSprite = value; }
		}

        private Texture2D m_SourceTex;
		public Texture2D sourceTex
		{
			get { return m_SourceTex; }
			set { m_SourceTex = value; }
		}

        private Texture2D m_DestTex;
		public Texture2D destTex
		{
			get { return m_DestTex; }
			set { m_DestTex = value; }
		}

        private Sprite m_DestSprite;
		public Sprite destSprite
		{
			get { return m_DestSprite; }
			set { m_DestSprite = value; }
		}

        private Image m_DestImage;
		public Image destImage
		{
			get { return m_DestImage; }
			set { m_DestImage = value; }
		}

        //	Used to assign and keep track of unique names for each shadow Sprite
        private string m_TextureFileName;
		public static Vector4? ShadowSpriteBorder;
		private string m_ShadowDir;

        void Start()
        {
            //	For some reason, the shadow image loses the sprite reference when play mode is entered. This re-assigns it.
            if (destSprite)
            {
                destImage.sprite = destSprite;
            }
        }

        public void GenerateShadowFromImage()
        {
			isDone = false;

            //	Gets the source Image's Sprite's Texture to use
            destImage = gameObject.GetComponent<Image>();
            if (!destImage)
            {
                destImage = gameObject.AddComponent<Image>();
            }
            sourceSprite = sourceImage.sprite;

            if (sourceSprite != null)
            {
                Texture sourceOriginalTexture = sourceSprite.texture;
                sourceOriginalTexture.hideFlags = HideFlags.HideAndDontSave;

                //	We can't just use sourceOriginalTexture, as it may not be read/write enabled.
                //	Instead, we copy it using the filepath and WWW.LoadImageIntoTexture
                string path = "file://" + Application.dataPath.Replace("Assets", "");
                path += AssetDatabase.GetAssetPath(sourceOriginalTexture);
                WWW www = new WWW(path);
                ContinuationManager.Add(() => www.isDone, () =>
                {
                    sourceTex = new Texture2D(sourceOriginalTexture.width, sourceOriginalTexture.height);
                    sourceTex.hideFlags = HideFlags.HideAndDontSave;
                    www.LoadImageIntoTexture(sourceTex);
                    SetupFile();
                });
            }
            else
            {
                sourceTex = new Texture2D(8, 8);
                int yCounter = 0;
                int xCounter = 0;
                while (xCounter < sourceTex.width)
                {
                    while (yCounter < sourceTex.height)
                    {
                        sourceTex.SetPixel(xCounter, yCounter, Color.black);
                        yCounter++;
                    }

                    xCounter++;
                    yCounter = 0;
                }
                sourceTex.Apply();
                SetupFile();
            }
        }

        private void SetupFile()
        {
            m_ShadowDir = "Assets/" + generatedShadowsDirectory;

            //	Create shadow sprite directory if none exists
            if (!Directory.Exists(m_ShadowDir))
            {
                Directory.CreateDirectory(m_ShadowDir);
            }

            //	Failsafe
            if (sourceImage == null)
            {
                Debug.LogWarning("Must have source image");
                isDone = true;
                return;
            }

            //	If the shadow Image is not correctly assigned, remove the texture to create new shadow image
            if (!destImage.sprite)
            {
                if (destTex)
                {
                    DestroyImmediate(destTex);
                }
            }

            //	Check to see if the shadow is already using an image - if so, overwrite it - if not, prepare to create a new one
            if (!AssetDatabase.LoadAssetAtPath(string.Format(m_ShadowDir + "/{0}.png", m_Guid), typeof(Sprite)))
            {
                m_Guid = Guid.NewGuid().ToString();
            }

            m_TextureFileName = string.Format(m_ShadowDir + "/{0}.png", m_Guid);

            //	Calls the functions for each stage of the shadow generation process
            Setup();
            Darken();
            Blur();
            ApplyChanges();
            DestroyImmediate(sourceTex);
            DestroyImmediate(destTex);
            isDone = true;
        }

        private void Setup()
        {
            //	Creates a new texture for the shadow that is bigger to accommodate the shadow blur
            int widthWithPadding = sourceTex.width + imagePadding * 2;
            int heightWithPadding = sourceTex.height + imagePadding * 2;

            destTex = new Texture2D(widthWithPadding, heightWithPadding, TextureFormat.RGBA32, false);
            destTex.hideFlags = HideFlags.HideAndDontSave;
            destTex.filterMode = FilterMode.Trilinear;
            destTex.wrapMode = TextureWrapMode.Clamp;

            //	Makes the entire shadow image fully-transparent, pixel-by-pixel (As a newly-created texture isn't, for some strange reason)
            int yCounter = 0;
            int xCounter = 0;
            Color pixCol = new Color(0, 0, 0, 0);
            while (xCounter < destTex.width)
            {
                while (yCounter < destTex.height)
                {
                    destTex.SetPixel(xCounter, yCounter, pixCol);
                    yCounter++;
                }
                xCounter++;
                yCounter = 0;
            }
        }

		private void Darken()
        {
            //	Copies each pixel from the source texture, and darkens them
            int yCounter = 0;
            int xCounter = 0;
            Color pixCol = new Color(0, 0, 0, 0);

            while (xCounter < sourceTex.width)
            {
                while (yCounter < sourceTex.height)
                {
                    pixCol.a = sourceTex.GetPixel(xCounter, yCounter).a;
                    destTex.SetPixel(xCounter + imagePadding, yCounter + imagePadding, pixCol);
                    yCounter++;
                }

                xCounter++;
                yCounter = 0;
            }
        }

		private void Blur()
        {
            //	Iterates through each pixel of the shadow image and makes the color an average of the surrounding pixels (Radius is specified in editor)
            int i = 0;
            int xCounter = 0;
            int yCounter = 0;
            Color pixCol = new Color(0, 0, 0, 0);

            while (i < blurIterations)
            {
                while (xCounter < destTex.width)
                {
                    while (yCounter < destTex.height)
                    {
                        if (blurRange == 1)
                        {
                            pixCol.a = destTex.GetPixel(xCounter, yCounter - 1).a / 4 + destTex.GetPixel(xCounter, yCounter).a / 2 + destTex.GetPixel(xCounter, yCounter + 1).a / 4;
                            pixCol.r = 0;
                            pixCol.g = 0;
                            pixCol.b = 0;
                            destTex.SetPixel(xCounter, yCounter, pixCol / (shadowDarkenAmount));
                        }
                        else if (blurRange == 2)
                        {
                            pixCol = destTex.GetPixel(xCounter, yCounter - 2) + destTex.GetPixel(xCounter, yCounter - 1) + destTex.GetPixel(xCounter, yCounter) + destTex.GetPixel(xCounter, yCounter + 1) + destTex.GetPixel(xCounter, yCounter + 2);
                            destTex.SetPixel(xCounter, yCounter, pixCol / (shadowDarkenAmount * 5));
                        }
                        else if (blurRange == 3)
                        {
                            pixCol = destTex.GetPixel(xCounter, yCounter - 3) + destTex.GetPixel(xCounter, yCounter - 2) + destTex.GetPixel(xCounter, yCounter - 1) + destTex.GetPixel(xCounter, yCounter) + destTex.GetPixel(xCounter, yCounter + 1) + destTex.GetPixel(xCounter, yCounter + 2) + destTex.GetPixel(xCounter, yCounter + 3);
                            destTex.SetPixel(xCounter, yCounter, pixCol / (shadowDarkenAmount * 7));
                        }
                        else if (blurRange == 4)
                        {
                            pixCol = destTex.GetPixel(xCounter, yCounter - 4) + destTex.GetPixel(xCounter, yCounter - 3) + destTex.GetPixel(xCounter, yCounter - 2) + destTex.GetPixel(xCounter, yCounter - 1) + destTex.GetPixel(xCounter, yCounter) + destTex.GetPixel(xCounter, yCounter + 1) + destTex.GetPixel(xCounter, yCounter + 2) + destTex.GetPixel(xCounter, yCounter + 3) + destTex.GetPixel(xCounter, yCounter + 4);
                            destTex.SetPixel(xCounter, yCounter, pixCol / (shadowDarkenAmount * 9));
                        }
                        else if (blurRange == 5)
                        {
                            pixCol = destTex.GetPixel(xCounter, yCounter - 5) + destTex.GetPixel(xCounter, yCounter - 4) + destTex.GetPixel(xCounter, yCounter - 3) + destTex.GetPixel(xCounter, yCounter - 2) + destTex.GetPixel(xCounter, yCounter - 1) + destTex.GetPixel(xCounter, yCounter) + destTex.GetPixel(xCounter, yCounter + 1) + destTex.GetPixel(xCounter, yCounter + 2) + destTex.GetPixel(xCounter, yCounter + 3) + destTex.GetPixel(xCounter, yCounter + 4) + destTex.GetPixel(xCounter, yCounter + 5);
                            destTex.SetPixel(xCounter, yCounter, pixCol / (shadowDarkenAmount * 11));
                        }
                        yCounter++;
                    }
                    xCounter++;
                    yCounter = 0;
                }

                xCounter = 0;
                yCounter = 0;

                while (xCounter < destTex.width)
                {
                    while (yCounter < destTex.height)
                    {
                        if (blurRange == 1)
                        {
                            pixCol.a = destTex.GetPixel(xCounter - 1, yCounter).a / 4 + destTex.GetPixel(xCounter, yCounter).a / 2 + destTex.GetPixel(xCounter + 1, yCounter).a / 4;
                            pixCol.r = 0;
                            pixCol.g = 0;
                            pixCol.b = 0;
                            destTex.SetPixel(xCounter, yCounter, pixCol / (shadowDarkenAmount));
                        }
                        else if (blurRange == 2)
                        {
                            pixCol = destTex.GetPixel(xCounter - 2, yCounter) + destTex.GetPixel(xCounter - 1, yCounter) + destTex.GetPixel(xCounter, yCounter) + destTex.GetPixel(xCounter + 1, yCounter) + destTex.GetPixel(xCounter + 2, yCounter);
                            destTex.SetPixel(xCounter, yCounter, pixCol / (shadowDarkenAmount * 5));
                        }
                        else if (blurRange == 3)
                        {
                            pixCol = destTex.GetPixel(xCounter - 3, yCounter) + destTex.GetPixel(xCounter - 2, yCounter) + destTex.GetPixel(xCounter - 1, yCounter) + destTex.GetPixel(xCounter, yCounter) + destTex.GetPixel(xCounter + 1, yCounter) + destTex.GetPixel(xCounter + 2, yCounter) + destTex.GetPixel(xCounter + 3, yCounter);
                            destTex.SetPixel(xCounter, yCounter, pixCol / (shadowDarkenAmount * 7));
                        }
                        else if (blurRange == 4)
                        {
                            pixCol = destTex.GetPixel(xCounter - 4, yCounter) + destTex.GetPixel(xCounter - 3, yCounter) + destTex.GetPixel(xCounter - 2, yCounter) + destTex.GetPixel(xCounter - 1, yCounter) + destTex.GetPixel(xCounter, yCounter) + destTex.GetPixel(xCounter + 1, yCounter) + destTex.GetPixel(xCounter + 2, yCounter) + destTex.GetPixel(xCounter + 3, yCounter) + destTex.GetPixel(xCounter + 4, yCounter);
                            destTex.SetPixel(xCounter, yCounter, pixCol / (shadowDarkenAmount * 9));
                        }
                        else if (blurRange == 5)
                        {
                            pixCol = destTex.GetPixel(xCounter - 5, yCounter) + destTex.GetPixel(xCounter - 4, yCounter) + destTex.GetPixel(xCounter - 3, yCounter) + destTex.GetPixel(xCounter - 2, yCounter) + destTex.GetPixel(xCounter - 1, yCounter) + destTex.GetPixel(xCounter, yCounter) + destTex.GetPixel(xCounter + 1, yCounter) + destTex.GetPixel(xCounter + 2, yCounter) + destTex.GetPixel(xCounter + 3, yCounter) + destTex.GetPixel(xCounter + 4, yCounter) + destTex.GetPixel(xCounter + 5, yCounter);
                            destTex.SetPixel(xCounter, yCounter, pixCol / (shadowDarkenAmount * 11));
                        }


                        yCounter++;
                    }
                    xCounter++;
                    yCounter = 0;
                }

                xCounter = 0;
                yCounter = 0;
                i++;
            }
        }

		private void ApplyChanges()
        {
            destTex.Apply();

            //	Encodes destTex as a PNG
            byte[] bytes = destTex.EncodeToPNG();

            //	Tells the texture importer to automatically import the images (PNG) as sprites
            if (sourceSprite)
            {
                ShadowSpriteBorder = new Vector4(sourceSprite.border.w + imagePadding, sourceSprite.border.x + imagePadding,
                    sourceSprite.border.y + imagePadding, sourceSprite.border.z + imagePadding);
            }
            else
            {
                ShadowSpriteBorder = new Vector4(imagePadding, imagePadding, imagePadding, imagePadding);
            }

            //	Saves destTex as a PNG in /Assets/ShadowGenerator/GeneratedShadows
            File.WriteAllBytes(m_TextureFileName, bytes);

            //	Safety net for the importer
            AssetDatabase.Refresh();

            //	References the newly-created and imported Sprite
            destSprite = AssetDatabase.LoadAssetAtPath(m_TextureFileName, typeof(Sprite)) as Sprite;

            //	Resizes, slices and assigns the Sprite
            destImage.rectTransform.sizeDelta = new Vector2(sourceImage.rectTransform.sizeDelta.x + imagePadding * 2, sourceImage.rectTransform.sizeDelta.y + imagePadding * 2);
            destImage.rectTransform.position = sourceImage.rectTransform.position;
            destImage.sprite = destSprite;
            if (sourceSprite)
            {
                destImage.type = sourceImage.type;
            }
            else
            {
                destImage.type = Image.Type.Sliced;
            }

            //	Positions the shadow Image
            Vector3 tempVec3 = destImage.rectTransform.position + shadowRelativePosition;
            destImage.rectTransform.position = tempVec3;

            //	Resizes the shadow Image
            Vector2 tempVec2 = destImage.rectTransform.sizeDelta + shadowRelativeSize;
            destImage.rectTransform.sizeDelta = tempVec2;

            //	Makes the shadow Image the desired transparency
            Color tempColor = destImage.color;
            tempColor.a = shadowAlpha;
            destImage.color = tempColor;
        }
    }
}
#endif