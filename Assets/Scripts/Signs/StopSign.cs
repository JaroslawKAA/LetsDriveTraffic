using Car;

namespace Signs
{
    public class StopSign : SignBase
    {
        protected override void SendMessageToCar(CarController car)
        {
            car.Stop();
        }
    }
}
