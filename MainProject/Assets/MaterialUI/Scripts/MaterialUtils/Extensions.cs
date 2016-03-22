//  Copyright 2016 MaterialUI for Unity http://materialunity.com
//  Please see license file for terms and conditions of use, and more information.

using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MaterialUI
{
    public static class CanvasExtension
    {
        public static Canvas Copy(this Canvas canvas, GameObject gameObjectToAddTo)
        {
            Canvas dupCanvas = gameObjectToAddTo.GetAddComponent<Canvas>();

            RectTransform mainCanvasRectTransform = canvas.GetComponent<RectTransform>();
            RectTransform dropdownCanvasRectTransform = dupCanvas.GetComponent<RectTransform>();

            dropdownCanvasRectTransform.position = mainCanvasRectTransform.position;
            dropdownCanvasRectTransform.sizeDelta = mainCanvasRectTransform.sizeDelta;
            dropdownCanvasRectTransform.anchorMin = mainCanvasRectTransform.anchorMin;
            dropdownCanvasRectTransform.anchorMax = mainCanvasRectTransform.anchorMax;
            dropdownCanvasRectTransform.pivot = mainCanvasRectTransform.pivot;
            dropdownCanvasRectTransform.rotation = mainCanvasRectTransform.rotation;
            dropdownCanvasRectTransform.localScale = mainCanvasRectTransform.localScale;

            dupCanvas.gameObject.GetAddComponent<GraphicRaycaster>();
            CanvasScaler mainScaler = canvas.GetComponent<CanvasScaler>();
            if (mainScaler != null)
            {
                CanvasScaler scaler = dupCanvas.gameObject.GetAddComponent<CanvasScaler>();
                scaler.uiScaleMode = mainScaler.uiScaleMode;
                scaler.referenceResolution = mainScaler.referenceResolution;
                scaler.screenMatchMode = mainScaler.screenMatchMode;
                scaler.matchWidthOrHeight = mainScaler.matchWidthOrHeight;
                scaler.referencePixelsPerUnit = mainScaler.referencePixelsPerUnit;
            }
            dupCanvas.gameObject.GetAddComponent<MaterialUIScaler>();
            dupCanvas.renderMode = canvas.renderMode;

            return dupCanvas;
        }
        public static void CopySettingsToOtherCanvas(this Canvas canvas, Canvas otherCanvas)
        {
            RectTransform mainCanvasRectTransform = canvas.GetComponent<RectTransform>();
            RectTransform dropdownCanvasRectTransform = otherCanvas.GetComponent<RectTransform>();

            dropdownCanvasRectTransform.position = mainCanvasRectTransform.position;
            dropdownCanvasRectTransform.sizeDelta = mainCanvasRectTransform.sizeDelta;
            dropdownCanvasRectTransform.anchorMin = mainCanvasRectTransform.anchorMin;
            dropdownCanvasRectTransform.anchorMax = mainCanvasRectTransform.anchorMax;
            dropdownCanvasRectTransform.pivot = mainCanvasRectTransform.pivot;
            dropdownCanvasRectTransform.rotation = mainCanvasRectTransform.rotation;
            dropdownCanvasRectTransform.localScale = mainCanvasRectTransform.localScale;

            otherCanvas.gameObject.GetAddComponent<GraphicRaycaster>();
            CanvasScaler mainScaler = canvas.GetComponent<CanvasScaler>();
            if (mainScaler != null)
            {
                CanvasScaler scaler = otherCanvas.gameObject.GetAddComponent<CanvasScaler>();
                scaler.uiScaleMode = mainScaler.uiScaleMode;
                scaler.referenceResolution = mainScaler.referenceResolution;
                scaler.screenMatchMode = mainScaler.screenMatchMode;
                scaler.matchWidthOrHeight = mainScaler.matchWidthOrHeight;
                scaler.referencePixelsPerUnit = mainScaler.referencePixelsPerUnit;
            }
            otherCanvas.gameObject.GetAddComponent<MaterialUIScaler>();
            otherCanvas.renderMode = canvas.renderMode;
        }
    }

    public static class ActionExtension
    {
        public static void InvokeIfNotNull(this Action action)
        {
            if (action != null)
            {
                action.Invoke();
            }
        }

        public static void InvokeIfNotNull<T>(this Action<T> action, T parameter)
        {
            if (action != null)
            {
                action.Invoke(parameter);
            }
        }
    }
    public static class UnityEventExtension
    {
        public static void InvokeIfNotNull(this UnityEvent unityEvent)
        {
            if (unityEvent != null)
            {
                unityEvent.Invoke();
            }
        }

        public static void InvokeIfNotNull<T>(this UnityEvent<T> unityEvent, T parameter)
        {
            if (unityEvent != null)
            {
                unityEvent.Invoke(parameter);
            }
        }
    }

    public static class GameObjectExtension
    {
        public static T GetAddComponent<T>(this GameObject gameObject) where T : Component
        {
            if (gameObject.GetComponent<T>() != null)
            {
                return gameObject.GetComponent<T>();
            }
            else
            {
                return gameObject.AddComponent<T>();
            }

        }

        public static T GetChildByName<T>(this GameObject gameObject, string name) where T : Component
        {
            T[] items = gameObject.GetComponentsInChildren<T>(true);

            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].name == name)
                {
                    return items[i];
                }
            }

            return null;
        }
    }

    public static class MonoBehaviourExtension
    {
        public static T GetAddComponent<T>(this MonoBehaviour monoBehaviour) where T : Component
        {
            if (monoBehaviour.GetComponent<T>() != null)
            {
                return monoBehaviour.GetComponent<T>();
            }

            return monoBehaviour.gameObject.AddComponent<T>();
        }

        public static T GetChildByName<T>(this MonoBehaviour monoBehaviour, string name) where T : Component
        {
            return monoBehaviour.gameObject.GetChildByName<T>(name);
        }
    }

    public static class ComponentExtension
    {
        public static T GetChildByName<T>(this Component component, string name) where T : Component
        {
            return component.gameObject.GetChildByName<T>(name);
        }
    }

    public static class ColorExtension
    {
        public static Color WithAlpha(this Color color, float alpha)
        {
            return new Color(color.r, color.g, color.b, alpha);
        }
    }

    public static class RectTransformExtension
    {
        //	Sometimes sizeDelta works, sometimes rect works, sometimes neither work and you need to get the layout properties.
        //	This method provides a simple way to get the size of a RectTransform, no matter what's driving it or what the anchor values are.
        public static Vector2 GetProperSize(this RectTransform rectTransform) //, bool attemptToRefreshLayout = false)
        {
            Vector2 size = new Vector2(rectTransform.rect.width, rectTransform.rect.height);

            if (size.x == 0 && size.y == 0)
            {
                LayoutElement layoutElement = rectTransform.GetComponent<LayoutElement>();

                if (layoutElement != null)
                {
                    size.x = layoutElement.preferredWidth;
                    size.y = layoutElement.preferredHeight;
                }
            }
            if (size.x == 0 && size.y == 0)
            {
                LayoutGroup layoutGroup = rectTransform.GetComponent<LayoutGroup>();

                if (layoutGroup != null)
                {
                    size.x = layoutGroup.preferredWidth;
                    size.y = layoutGroup.preferredHeight;
                }
            }

            if (size.x == 0 && size.y == 0)
            {
                size.x = LayoutUtility.GetPreferredWidth(rectTransform);
                size.y = LayoutUtility.GetPreferredHeight(rectTransform);
            }

            return size;
        }

        public static Vector3 GetPositionRegardlessOfPivot(this RectTransform rectTransform)
        {
            Vector3[] corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);
            return (corners[0] + corners[2]) / 2;
        }

        public static Vector3 GetLocalPositionRegardlessOfPivot(this RectTransform rectTransform)
        {
            Vector3[] corners = new Vector3[4];
            rectTransform.GetLocalCorners(corners);
            return (corners[0] + corners[2]) / 2;
        }

        public static void SetAnchorX(this RectTransform rectTransform, float min, float max)
        {
            rectTransform.anchorMin = new Vector2(min, rectTransform.anchorMin.y);
            rectTransform.anchorMax = new Vector2(max, rectTransform.anchorMax.y);
        }

        public static void SetAnchorY(this RectTransform rectTransform, float min, float max)
        {
            rectTransform.anchorMin = new Vector2(rectTransform.anchorMin.x, min);
            rectTransform.anchorMax = new Vector2(rectTransform.anchorMax.x, max);
        }
    }

    public static class GraphicExtension
    {
        public static bool IsSpriteOrVectorImage(this Graphic graphic)
        {
            return (graphic is Image || graphic is VectorImage);
        }

        public static void SetImage(this Graphic graphic, Sprite sprite)
        {
            Image imageToSet = graphic as Image;

            if (imageToSet != null)
            {
                imageToSet.sprite = sprite;
            }
        }
        public static void SetImage(this Graphic graphic, VectorImageData vectorImageData)
        {
            VectorImage imageToSet = graphic as VectorImage;

            if (imageToSet != null)
            {
                imageToSet.vectorImageData = vectorImageData;
            }
        }
        public static void SetImage(this Graphic graphic, ImageData imageData)
        {
            VectorImage vectorImage = graphic as VectorImage;

            if (vectorImage != null && imageData != null)
            {
                if (imageData.imageDataType == ImageDataType.VectorImage)
                {
                    vectorImage.vectorImageData = imageData.vectorImageData;
                }
                return;
            }

            Image spriteImage = graphic as Image;

            if (spriteImage != null && imageData != null)
            {
                if (imageData.imageDataType == ImageDataType.Sprite)
                {
                    spriteImage.sprite = imageData.sprite;
                }
            }
        }

        public static Sprite GetSpriteImage(this Graphic graphic)
        {
            Image imageToGet = graphic as Image;

            if (imageToGet != null)
            {
                return imageToGet.sprite;
            }

            return null;
        }

        public static VectorImageData GetVectorImage(this Graphic graphic)
        {
            VectorImage imageToGet = graphic as VectorImage;

            if (imageToGet != null)
            {
                return imageToGet.vectorImageData;
            }

            return null;
        }

        public static ImageData GetImageData(this Graphic graphic)
        {
            Sprite sprite = graphic.GetSpriteImage();

            if (sprite != null)
            {
                return new ImageData(sprite);
            }

            VectorImageData vectorImageData = graphic.GetVectorImage();

            if (vectorImageData != null)
            {
                return new ImageData(vectorImageData);
            }

            return null;
        }
    }
}