using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actopolus.FakeLeaderboard.Src.Tween
{
    // Tween animations
    public static class TwAnimation
    {
        // Coroutines in progress
        private static readonly Dictionary<MonoBehaviour, Coroutine> Coroutines = new Dictionary<MonoBehaviour, Coroutine>();

        // Animates value
        public static void Value(
            MonoBehaviour monoBehaviour, 
            float from, 
            float to, 
            Action<float> updateValue, 
            float duration,
            float delay = 0f,
            Action onComplete = null
        ) {
            if (Coroutines.ContainsKey(monoBehaviour))
            {
                monoBehaviour.StopCoroutine(Coroutines[monoBehaviour]);
            }

            Coroutines[monoBehaviour] = monoBehaviour.StartCoroutine(
                RunAnimation(from, to, updateValue, duration, delay, () => {
                    onComplete?.Invoke();
                    Coroutines.Remove(monoBehaviour);
                })
            );
        }

        // Runs actual animation
        private static IEnumerator RunAnimation(
            float from,
            float to,
            Action<float> updateValue,
            float duration,
            float delay,
            Action onComplete
        )
        {
            yield return new WaitForSeconds(delay);

            var elapsedTime = 0f;
            var segment = to - from;

            while (elapsedTime < duration)
            {
                var value = Easing.LinearEaseInOut(elapsedTime, 0f, 1f, duration);
                updateValue(from + value * segment);
                yield return null;
                elapsedTime += Time.deltaTime;
            }

            updateValue(to);
            onComplete?.Invoke();
        }
    }
}