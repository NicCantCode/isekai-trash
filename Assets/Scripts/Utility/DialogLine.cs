/*
 * Copyright (c) NicCantCode
 * https://nichal.us/
 */

using System;
using UnityEngine;

[Serializable]
public class DialogLine
{
    public Name speakerName;
    public Color nameColor;
    public Color textColor;
    
    [TextArea(3, 6)]
    public string dialogLine;
}
