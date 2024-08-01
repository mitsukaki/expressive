using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using VRC.SDKBase;

namespace com.mitsukaki.expressive
{
    public class ExpressiveContainer : MonoBehaviour, IEditorOnly
    {
        public ExpressionClip[] Clips;

        public string OutputPath;

        public int DefaultDuration = 10 * 60;
    }
}