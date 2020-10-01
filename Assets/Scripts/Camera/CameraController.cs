using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float translationVelocity;
    public float rotationVelocity;
    public float zoomVelocity;

    //stores last and current mouse position
    private Vector3 lastPosition;
    private Vector3 currentPosition;
    private Vector3 zeroXRotation;
    private Vector3 rotationPosition;

    private Camera cam;

    void Start()
    {
        lastPosition = Constants.NullVector3;
        rotationPosition = Constants.NullVector3;
        zeroXRotation = new Vector3(0, 0, 0);
    }


    void Update()
    {
        currentPosition = Input.mousePosition;

        bool translateForward = Input.GetKey(KeyCode.W);
        bool translateBackward = Input.GetKey(KeyCode.S);
        bool translateRight = Input.GetKey(KeyCode.D);
        bool translateLeft = Input.GetKey(KeyCode.A);

        bool translationPressed = translateForward || translateBackward || translateRight || translateLeft;
        bool rotationPressed = Input.GetMouseButton(1);
        float zoomPressed = Input.GetAxis("Mouse ScrollWheel");

        if (NeedsZoom(translationPressed, rotationPressed, zoomPressed))
        {
            Zoom(zoomPressed);
        } else if (NeedsRotation(translationPressed, rotationPressed, zoomPressed))
        {
            if (HasRotationPosition() == false)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(currentPosition);
                if (Physics.Raycast(ray.origin, ray.direction, out hit) == false)
                    return;

                rotationPosition = hit.point;
            }
            
            Rotate();
        } else if (NeedsTranslation(translationPressed, rotationPressed, zoomPressed))
        {
            Translate(translateForward, translateBackward, translateRight, translateLeft);
        } else
        {
            lastPosition = Constants.NullVector3;
            rotationPosition = Constants.NullVector3;
        }
    }

    private bool NeedsZoom(bool translationPressed, bool rotationPressed, float zoomPressed)
    {
        if (zoomPressed != 0.0f && rotationPressed == false && translationPressed == false)
            return true;

        return false;
    }

    private bool HasRotationPosition()
    {
        return rotationPosition != Constants.NullVector3;
    }

    private void Zoom(float zoomPressed)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(currentPosition);
        if (Physics.Raycast(ray.origin, ray.direction, out hit) == false)
            return;

        Vector3 zoomDirection = zoomPressed * (hit.point - transform.position);
        zoomDirection.Normalize();
        transform.Translate(zoomDirection * Time.deltaTime * zoomVelocity, Space.World);
    }

    private bool NeedsRotation(bool translationPressed, bool rotationPressed, float zoomPressed)
    {
        if (translationPressed == false && rotationPressed && zoomPressed == 0.0f)
            return true;

        return false;
    }

    private void Rotate()
    {
        if (lastPosition == Constants.NullVector3)
        {
            lastPosition = currentPosition;
            return;
        }

        Vector3 screenDirection = currentPosition - lastPosition;
        screenDirection.Normalize();
        float angle = screenDirection.x * Time.deltaTime * rotationVelocity;
        transform.RotateAround(rotationPosition, Constants.RotationAroundYAxis, angle);
        lastPosition = currentPosition;
    }

    private bool NeedsTranslation(bool translationPressed, bool rotationPressed, float zoomPressed)
    {
        if (translationPressed && rotationPressed == false && zoomPressed == 0.0f)
            return true;

        return false;
    }

    private void Translate(bool translateForward, bool translateBackward, bool translateRight, bool translateLeft)
    {
        int moveVertical = 0;
        int moveHorizontal = 0;

        if (translateForward)
            moveVertical += 1;
        if (translateBackward)
            moveVertical -= 1;
        if (translateRight)
            moveHorizontal += 1;
        if (translateLeft)
            moveHorizontal -= 1;

        Vector3 eulerAng = transform.rotation.eulerAngles;
        zeroXRotation.y = eulerAng.y;
        zeroXRotation.z = eulerAng.z;
        transform.eulerAngles = zeroXRotation;

        Vector3 moveY = transform.forward * moveVertical;
        Vector3 moveX = transform.right * moveHorizontal;
        Vector3 translation = moveY + moveX;
        transform.Translate(translation * translationVelocity * Time.deltaTime, Space.World);

        //Reset to former orientation
        transform.eulerAngles = eulerAng;
    }
}
