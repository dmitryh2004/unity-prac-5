using UnityEngine;

public class MouseLook : MonoBehaviour
{
    int cameraMode = 0; //0 - first person, 1 - third person back, 2 - third person front
    float thirdPersonRadius = 5.0f;

    public float mouseSensitivity = 300f;
    public Transform playerBody;
    float xRotation = 0f;
    float yRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        yRotation = playerBody.rotation.eulerAngles.y;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            cameraMode++;
            if (cameraMode == 3) cameraMode = 0;
        }
        //сброс на первое лицо
        transform.localPosition = new Vector3(0, 2, 0);

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -85f, 85f);

        yRotation += mouseX;

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.rotation = Quaternion.Euler(0f, yRotation, 0f);

        if (cameraMode != 0)
        {
            float z = thirdPersonRadius * Mathf.Cos(Mathf.Deg2Rad * yRotation);
            float x = thirdPersonRadius * Mathf.Sin(Mathf.Deg2Rad * yRotation);

            if (cameraMode == 1)
            {
                x = -x;
                z = -z;
            }

            float y = thirdPersonRadius * Mathf.Cos(Mathf.Deg2Rad * xRotation);
            transform.localPosition = new Vector3(x, y + 2, z);
            transform.LookAt(playerBody);
        }
    }
}