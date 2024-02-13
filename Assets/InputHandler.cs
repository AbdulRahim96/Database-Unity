using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class InputHandler : MonoBehaviour
{
    public Vector2 InputVector { get; private set; }
    public Animator animator;

    public static float horizontalMotion;
    public static float verticalMotion;

    public bool canJump = true;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundRadius = 0.17f, jumpHeight;

    private InputHandler _input;
    [SerializeField]
    private float MovementSpeed;
    [SerializeField]
    private float RotationSpeed;

    [SerializeField]
    private Camera Camera;


    private void OnEnable()
    {
        gameObject.tag = "Player";
        _input = GetComponent<InputHandler>();
        Camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        FindObjectOfType<CameraMovementMobile>().target = transform;
        FindObjectOfType<Database>().currentPlayer = GetComponent<List>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        horizontalMotion = CrossPlatformInputManager.GetAxis("Horizontal");
        verticalMotion = CrossPlatformInputManager.GetAxis("Vertical");
        InputVector = new Vector2(horizontalMotion, verticalMotion);

        var targetVector = new Vector3(_input.InputVector.x, 0, _input.InputVector.y);
        var movementVector = MoveTowardTarget(targetVector);
        RotateTowardMovementVector(movementVector);

        //canJump = Physics.CheckSphere(groundCheck.position, groundRadius, groundLayer);

        if (horizontalMotion != 0 || verticalMotion != 0)
            animator.SetBool("moving", true);
        else
            animator.SetBool("moving", false);

        if (Users.fast)
            animator.SetBool("fast", true);
        else
            animator.SetBool("fast", false);
    }

    
    IEnumerator animate(string name, float time)
    {
        animator.SetBool(name, true);
        yield return new WaitForSeconds(time);
        animator.SetBool(name, false);
    }

    
    

    public void footSound()
    {
        GetComponent<AudioSource>().Play();
    }

    public void jump()
    {
        if(canJump)
        {
            StartCoroutine(animate("jump", 0.1f));
            GetComponent<Rigidbody>().AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);

        }
    }


    private Vector3 MoveTowardTarget(Vector3 targetVector)
    {
        var speed = 1f;
        if (Users.fast)
            speed = MovementSpeed * 2 * Time.deltaTime;
        else
            speed = MovementSpeed * Time.deltaTime;

        // transform.Translate(targetVector * (MovementSpeed * Time.deltaTime)); Demonstrate why this doesn't work
        //transform.Translate(targetVector * (MovementSpeed * Time.deltaTime), Camera.gameObject.transform);

        targetVector = Quaternion.Euler(0, Camera.gameObject.transform.rotation.eulerAngles.y, 0) * targetVector;
        var targetPosition = transform.position + targetVector * speed;
        transform.position = targetPosition;
        return targetVector;
    }

    private void RotateTowardMovementVector(Vector3 movementDirection)
    {
        if (movementDirection.magnitude == 0) { return; }
        var rotation = Quaternion.LookRotation(movementDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, RotationSpeed);
    }

    private void OnDisable()
    {
        gameObject.tag = "Untagged";
    }

    
}
