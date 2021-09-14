using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float cameraSpeed = 50f;
    public float borderThickness = 10f;
    public Vector2 movementLimit = new Vector2(80, 80);
    public float limitOffset = 30f;

    public float scrollSpeed = 20f;
    public float cameraMinHeight = -100f;
    public float cameraMaxHeight = 0f;

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            pos.z += cameraSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            pos.z -= cameraSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            pos.x -= cameraSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            pos.x += cameraSpeed * Time.deltaTime;
        }

        if (Input.mousePosition.y >= Screen.height - borderThickness
            || Input.mousePosition.y <= borderThickness
            || Input.mousePosition.x >= Screen.width - borderThickness
            || Input.mousePosition.x <= borderThickness)
        {
            Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);

            Vector2 cameraVector = ((Vector2) Input.mousePosition - screenCenter).normalized;

            pos.z += cameraVector.y * cameraSpeed * Time.deltaTime;
            pos.x += cameraVector.x * cameraSpeed * Time.deltaTime;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        pos.y += -scroll * scrollSpeed * 100f * Time.deltaTime;

        pos.x = Mathf.Clamp(pos.x, -movementLimit.x + limitOffset, movementLimit.x + limitOffset);
        pos.y = Mathf.Clamp(pos.y, cameraMinHeight, cameraMaxHeight);
        pos.z = Mathf.Clamp(pos.z, -movementLimit.y + limitOffset, movementLimit.y + limitOffset);

        transform.position = pos;
    }
}