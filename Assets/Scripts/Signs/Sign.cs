using System;
using Car;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// Object to place on the road
/// </summary>
public abstract class Sign : MonoBehaviour
{
    [SerializeField] private Sprite _signSprite;

    public Sprite SignSprite
    {
        get => _signSprite;
        private set => _signSprite = value;
    }

    private void Awake()
    {
        Texture2D mainTexture = GetComponentInChildren<Renderer>()
            .material.mainTexture as Texture2D;
        SignSprite = Sprite.Create(mainTexture,
            new Rect(0, 0, mainTexture.width, mainTexture.height),
            new Vector2());
    }

    private void Start()
    {
        Assert.IsNotNull(_signSprite);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Car"))
        {
            CarController car = other.GetComponent<CarController>();
            SendMessageToCar(car);
        }
    }

    /// <summary>
    /// Send message to car.
    /// </summary>
    /// <param name="car"></param>
    protected abstract void SendMessageToCar(CarController car);
}