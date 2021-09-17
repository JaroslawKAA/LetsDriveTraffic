using Car;
using UnityEngine;

namespace Signs
{
    public class LimitSpeedSign : Sign
    {
        [SerializeField] private int speedLimit = 30; 
        protected override void SendMessageToCar(CarController car)
        {
            car.LimitSpeed(speedLimit);
        }
    }
}