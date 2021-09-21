using System;
using System.Collections;
using Car;
using UnityEngine;
using UnityEngine.Assertions;

namespace Signs
{
    internal enum LightsState
    {
        Red,
        Yellow,
        Green
    }

    public class Lights : SignBase
    {
        [SerializeField] private float _changeStateTime = 10f;
        [SerializeField] private float _yellowLightTime = 1f;
        [SerializeField] private SpriteRenderer[] _lights;
        [SerializeField] private float _lightTurnedOfAlpha = .3f;

        [Header("Defined dynamically")] [SerializeField]
        private LightsState _state = LightsState.Red;

        [SerializeField] private float _timer;

        private LightsState State
        {
            get => _state;
            set
            {
                _state = value;
                SetLight();
            }
        }

        protected override void SendMessageToCar(CarController car)
        {
            switch (State)
            {
                case LightsState.Red:
                    car.Stop();
                    break;
                case LightsState.Yellow:
                    car.LimitSpeed((int)(car.SpeedMax / 2));
                    break;
                case LightsState.Green:
                    car.ResetSpeed();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void Awake()
        {
            Assert.IsNotNull(_lights);
            Assert.IsTrue(_lights.Length == 3, "Traffic lights should be in 3 count: Red, Yellow and Green.");
        }

        private void Update()
        {
            ChangeState();
        }

        /// <summary>
        /// Turn on current State light and turn off other lights
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private void SetLight()
        {
            switch (State)
            {
                case LightsState.Red:
                    _lights[(int) LightsState.Red].SetAlpha(1);
                    _lights[(int) LightsState.Yellow].SetAlpha(_lightTurnedOfAlpha);
                    _lights[(int) LightsState.Green].SetAlpha(_lightTurnedOfAlpha);
                    break;
                case LightsState.Yellow:
                    _lights[(int) LightsState.Yellow].SetAlpha(1);
                    _lights[(int) LightsState.Red].SetAlpha(_lightTurnedOfAlpha);
                    _lights[(int) LightsState.Green].SetAlpha(_lightTurnedOfAlpha);
                    break;
                case LightsState.Green:
                    _lights[(int) LightsState.Green].SetAlpha(1);
                    _lights[(int) LightsState.Yellow].SetAlpha(_lightTurnedOfAlpha);
                    _lights[(int) LightsState.Red].SetAlpha(_lightTurnedOfAlpha);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void ChangeState()
        {
            _timer += Time.deltaTime;
            if (_timer >= _changeStateTime + _yellowLightTime)
            {
                switch (State)
                {
                    case LightsState.Red:
                        StartCoroutine(ChangeLights(LightsState.Green));
                        break;
                    case LightsState.Green:
                        StartCoroutine(ChangeLights(LightsState.Red));
                        break;
                    case LightsState.Yellow:
                    default:
                        break;
                }

                _timer = 0f;
            }
        }

        private IEnumerator ChangeLights(LightsState targetState)
        {
            State = LightsState.Yellow;
            yield return new WaitForSeconds(_yellowLightTime);
            State = targetState;
        }
    }
}