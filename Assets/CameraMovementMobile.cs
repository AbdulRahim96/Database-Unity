using UnityEngine;
using System.Collections;


public class CameraMovementMobile : MonoBehaviour
{
    public Transform target;
    public Vector3 targetOffset;
    public float distance = 5.0f;
    public float maxDistance = 20;
    public float minDistance = .6f;
    public float xSpeed = 200.0f;
    public float ySpeed = 200.0f;
    public int yMinLimit = -80;
    public int yMaxLimit = 80;
    public int zoomRate = 40;
    public float zoomDampening = 5.0f;

    private float xDeg = 0.0f;
    private float yDeg = 0.0f;
    private float currentDistance;
    private float desiredDistance;
    private Quaternion currentRotation;
    private Quaternion desiredRotation;
    private Quaternion rotation;
    private Vector3 position;
    public bool canRotate = true;
    void Start() { Init(); }
    void OnEnable() { Init(); }

    public void Init()
    {
        
        target = GameObject.FindGameObjectWithTag("Player").transform;

        //be sure to grab the current rotations as starting points.
        position = transform.position;
        rotation = transform.rotation;
        currentRotation = transform.rotation;
        desiredRotation = transform.rotation;

        xDeg = Vector3.Angle(Vector3.right, transform.right);
        yDeg = Vector3.Angle(Vector3.up, transform.up);
    }

    void LateUpdate()
    {

        Rotate();
        Zoom();
        Move();
    }

    void Rotate()
    {
        if (canRotate)
        {
            //if the screen is touching just one finger and it is moving on the screen perform rotation of the camera
            if (Input.touchCount == 1 && Input.touches[0].phase == TouchPhase.Moved)
            {
                float swipeSpeed = Input.touches[0].deltaPosition.magnitude / Input.touches[0].deltaTime;

                xDeg += Input.touches[0].deltaPosition.x * xSpeed * Time.deltaTime * swipeSpeed * 0.00001f;
                yDeg -= Input.touches[0].deltaPosition.y * ySpeed * Time.deltaTime * swipeSpeed * 0.00001f;

                ////////OrbitAngle

                //Clamp the vertical axis for the orbit
                yDeg = ClampAngle(yDeg, yMinLimit, yMaxLimit);
                // set camera rotation 
                desiredRotation = Quaternion.Euler(yDeg, xDeg, 0);
                currentRotation = transform.rotation;

                rotation = Quaternion.Lerp(currentRotation, desiredRotation, Time.deltaTime * zoomDampening);
                transform.rotation = rotation;
            }
            //stop rotation if you click on the screen
            else if (Input.touchCount == 1 && Input.touches[0].phase == TouchPhase.Began)
            {
                desiredRotation = transform.rotation;
            }

            //continue rotation even after releasing the finger fro the screen
            if (transform.rotation != desiredRotation)
            {
                rotation = Quaternion.Lerp(transform.rotation, desiredRotation, Time.deltaTime * zoomDampening);
                transform.rotation = rotation;
            }
        }
    }

    void Zoom()
    {
        
            if (Input.touchCount == 2)
            {
                // Store both touches.
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                // Find the position in the previous frame of each touch.
                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                // Find the magnitude of the vector (the distance) between the touches in each frame.
                float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

                // Find the difference in the distances between each frame.
                float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

                // affect the desired Zoom distance if we pinch
                desiredDistance += deltaMagnitudeDiff * Time.deltaTime * zoomRate * Mathf.Abs(desiredDistance) * 0.001f;
                //clamp the zoom min/max
                desiredDistance = Mathf.Clamp(desiredDistance, minDistance, maxDistance);
                // For smoothing of the zoom, lerp distance
                currentDistance = Mathf.Lerp(currentDistance, desiredDistance, Time.deltaTime * zoomDampening);
            }
    }

    void Move()
    {
        // calculate position based on the new currentDistance 
        position = target.position - targetOffset;
        transform.position = position;
    }

    private static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }

    public void rot(bool flag)
    {
        canRotate = flag;
    }

}