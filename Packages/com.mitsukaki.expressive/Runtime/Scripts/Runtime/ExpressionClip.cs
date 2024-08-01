using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using VRC.SDKBase;

namespace com.mitsukaki.expressive
{
    [Serializable]
    public class ExpressionClip
    {
        public AnimationClip SourceClip;

        public List<ExpressionTrack> Tracks;

        public int Duration = -1;
    }
}