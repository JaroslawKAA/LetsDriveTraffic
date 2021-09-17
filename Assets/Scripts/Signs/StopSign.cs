using System.Collections;
using System.Collections.Generic;
using Car;
using UnityEngine;

public class StopSign : Sign
{
    protected override void SendMessageToCar(CarController car)
    {
        car.Stop();
    }
}
