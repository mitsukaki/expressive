
// newCurve.AddKey(new Keyframe(key.Time, key.Value));

using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using VRC.SDKBase;

namespace com.mitsukaki.expressive
{
    public class CurveBuilder 
    {
        public static AnimationCurve MakeCurve(
            ExpressionTrack track, float duration
        )
        {
            switch (track.type)
            {
                case ExpressionTrackType.PiecewiseLinear:
                    return CreatePiecewiseLinearCurve(
                        track.frequency,
                        track.amplitude,
                        track.offset,
                        duration
                    );
    
                case ExpressionTrackType.Sinusoidal:
                    return CreateSinusoidalCurve(
                        track.frequency,
                        track.amplitude,
                        track.offset,
                        duration
                    );
                    
                case ExpressionTrackType.Twitch:
                    return CreateTwitchCurve(
                        track.frequency,
                        track.amplitude,
                        track.offset,
                        duration
                    );

                default:
                    return CreateConstantCurve(
                        track.offset, duration
                    );
            }
        }

        public static AnimationCurve CreatePiecewiseLinearCurve(
            float frequency, float amplitude, float offset, float duration
        )
        {
            AnimationCurve curve = new AnimationCurve();
            
            // create keyframes
            float step = 1.0f / frequency;
            float min = offset - amplitude;
            float max = offset + amplitude;

            // add all the min keyframes
            for (float t = 0.0f; t <= duration; t += step)
                curve.AddKey(new Keyframe(t, min));
            
            // add all the max keyframes
            for (float t = step / 2.0f; t <= duration; t += step)
                curve.AddKey(new Keyframe(t, max));

            return curve;
        }

        public static AnimationCurve CreateSinusoidalCurve(
            float frequency, float amplitude, float offset, float duration
        )
        {
            AnimationCurve curve = new AnimationCurve();

            // step of 30 fps
            float step = 1.0f / 30.0f;

            // create keyframes
            for (float t = 0.0f; t <= duration; t += step)
            {
                float value = offset + amplitude * Mathf.Sin(2 * Mathf.PI * frequency * t);
                curve.AddKey(new Keyframe(t, value));
            }

            return curve;
        }

        public static AnimationCurve CreateTwitchCurve(
            float frequency, float amplitude, float offset, float duration
        )
        {
            AnimationCurve curve = new AnimationCurve();

            // step of 30 fps
            float step = 1.0f / 30.0f;

            // create keyframes
            for (float t = 0.0f; t <= duration; t += step)
            {
                float value = offset + amplitude * Mathf.PerlinNoise(frequency * t, 0.0f);
                curve.AddKey(new Keyframe(t, value));
            }

            return curve;
        }

        public static AnimationCurve CreateConstantCurve(float offset, float duration)
        {
            AnimationCurve curve = new AnimationCurve();
            curve.AddKey(new Keyframe(0.0f, offset));
            curve.AddKey(new Keyframe(duration, offset));
            return curve;
        }
    }
}