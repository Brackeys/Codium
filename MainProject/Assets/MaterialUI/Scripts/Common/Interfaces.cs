//  Copyright 2016 MaterialUI for Unity http://materialunity.com
//  Please see license file for terms and conditions of use, and more information.

using System.Collections.Generic;
using UnityEngine.UI;

namespace MaterialUI
{
    public interface IRippleCreator
    {
        void OnCreateRipple();
        void OnDestroyRipple();

        RippleData rippleData { get; }
    }

    public interface ITextValidator
    {
		void Init(MaterialInputField materialInputField);
        bool IsTextValid();
    }

    public interface IOptionDataListContainer
    {
        OptionDataList optionDataList { get; set; }
    }
}