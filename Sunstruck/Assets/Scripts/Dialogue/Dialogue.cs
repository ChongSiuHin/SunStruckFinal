using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string name_1;
    public string name_2;

    [TextArea(3, 10)]
    public string[] sentences_n1;
    public string[] sentences_n2;
}
