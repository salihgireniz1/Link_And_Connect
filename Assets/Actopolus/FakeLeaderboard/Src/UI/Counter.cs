using System;
using Actopolus.FakeLeaderboard.Src.Tween;
using TMPro;
using UnityEngine;

namespace Actopolus.FakeLeaderboard.Src.UI
{
    public class Counter : MonoBehaviour
    {
        // Render format
        [SerializeField] private string format = "{0}";

        // Counter label
        [SerializeField] private TMP_Text label;

        // Counter value
        private int _value;

        // Animated counter value
        private int _animatedValue;

        // Animation duration
        private float _duration = 1f;

        // Sets animation duration
        public void SetDuration(float duration) => _duration = duration; 

        // Returns current count
        public int GetCount() => _value;

        // Sets value without animation
        public void SetCountQuiet(int value)
        {
            _value = value;
            _animatedValue = value;

            UpdateLabel();
        }

        // Sets value with animation
        public void SetCount(int value)
        {
            _value = value;

            TwAnimation.Value(this, _animatedValue, _value, v => {
                _animatedValue = Mathf.FloorToInt(v);
                UpdateLabel();
            }, _duration);
        }

        // Updates label text
        private void UpdateLabel() => label.text = String.Format(format, _animatedValue.ToString());
    }
}