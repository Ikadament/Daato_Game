using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SmallChallengeAsset : ScriptableObject
{
    [TextArea]
    public string smallChallengeTitle;
    public float smallChallengeTimeInSec;
}

