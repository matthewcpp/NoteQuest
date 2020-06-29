using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace NoteQuest
{
    public class Streak : MonoBehaviour
    {
        TextMeshProUGUI text;
        AnimationCurve animationCurve;
        float t = 0.0f;

        private enum Direction { Up, Down, None }
        Direction direction = Direction.Up;

        float pulseSpeed { get; set; } = 0.5f;
        float pulseAmount { get; set; } = 1.25f;

        public int count { get => _count; set => SetCount(value); }
        private int _count = 0;

        private void Awake()
        {
            animationCurve = AnimationCurve.Linear(0.0f, 1.0f, pulseSpeed, pulseAmount);
            text = transform.Find("Text").GetComponent<TextMeshProUGUI>();
        }

        // Update is called once per frame
        void Update()
        {
            if (direction == Direction.Up)
            {
                t += Time.deltaTime;
                if (t >= pulseSpeed)
                {
                    t = pulseSpeed;
                    direction = Direction.Down;
                }
            }
            else
            {
                t -= Time.deltaTime;

                if (t <= 0.0f)
                {
                    t = 0.0f;
                    direction = Direction.Up;
                }
            }

            text.text = count.ToString();
            var scale = animationCurve.Evaluate(t);
            text.transform.localScale = new Vector3(scale, scale, scale);
        }

        private void SetCount(int value)
        {
            _count = value;

            if (_count == 0)
            {
                t = 0;
                direction = Direction.None;
            }
        }

    }
}
