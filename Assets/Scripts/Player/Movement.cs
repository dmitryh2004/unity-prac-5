using UnityEngine;

public class Movement : MonoBehaviour
{
    public CharacterController characterController;
    public Transform playerModel;
    Animator anim;
    public Transform groundcheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    bool moving = false;
    bool grounded;
    Vector3 velocity;
    Vector3 force;

    public float gravity = -9.81f;
    public float speed = 5f;

    public float jumpForce = 4f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        anim = playerModel.GetComponent<Animator>();
    }

    public void AddForce(Vector3 newForce)
    {
        force += newForce;
    }

    void DampForce()
    {
        Vector3 damping = 2 * Time.deltaTime * force;
        if (force.magnitude > 0.01) force -= damping; else force = new Vector3(0f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal"), z = Input.GetAxis("Vertical");
        moving = !((x == 0.0f) && (z == 0.0f));
        anim.SetBool("moving", moving);

        Vector3 move = transform.right * x + transform.forward * z;

        // если зажата клавиша ctrl, то игрок двигается в 2 раза быстрее
        if (Input.GetKey(KeyCode.LeftControl)) move *= 2;

        move += force;
        characterController.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime / 2);

        grounded = Physics.CheckSphere(groundcheck.position, groundDistance, groundMask);

        if (grounded && (velocity.y < 0))
        {
            velocity.y = -2f;
        }

        if (Input.GetButtonDown("Jump") && grounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }

        DampForce();
    }
}
