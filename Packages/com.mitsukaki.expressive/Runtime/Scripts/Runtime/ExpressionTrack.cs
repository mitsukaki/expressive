using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using VRC.SDKBase;

namespace com.mitsukaki.expressive
{
    [Serializable]
    public enum ExpressionTrackType
    {
        Constant, PiecewiseLinear, Sinusoidal, Twitch
    }

    [Serializable]
    public class ExpressionTrack
    {
        public ExpressionTrackType type;

        public string[] blendshapes;

        public float frequency = 1;
        public float amplitude = 1;
        public float offset = 0;
    }
}