//  Copyright 2016 MaterialUI for Unity http://materialunity.com
//  Please see license file for terms and conditions of use, and more information.

using UnityEngine;

namespace MaterialUI
{
    public enum InstantiationOptions
    {
        None,
        Label,
        Icon,
        Raised,
        Mini,
        HasContent,
        HasDropdown,
        HasInputField,
        Discrete,
        Light,
        Vertical,
        Fitted,
        HasLayoutHorizontal,
        HasLayoutVertical
    }

    public abstract class InstantiationHelper : MonoBehaviour
    {
        public virtual void HelpInstantiate(params InstantiationOptions[] options)
        {
            VectorImage[] vectorImages = GetComponentsInChildren<VectorImage>();

            for (int i = 0; i < vectorImages.Length; i++)
            {
                vectorImages[i].Refresh();
            }

            DestroyImmediate(GetComponent<InstantiationHelper>());
        }
    }
}