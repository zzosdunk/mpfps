using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float speed = 5f;

    [SerializeField]
    private float lookSensivity = 3f;

    [SerializeField]
    private float thrusterForce = 1000f;

 

    //Making own settings in the inspector
    [Header("Spring settings")]
    [SerializeField]
    private JointDriveMode jointMode = JointDriveMode.Position;
    [SerializeField]
    private float jointSpring = 20f;
    [SerializeField]
    private float jointMaxForce = 40f;


    //Component caching
    private PlayerMotor motor;
    private ConfigurableJoint joint;
    private Animator animator;


    // Use this for initialization
    void Start () {
        motor = GetComponent<PlayerMotor>();
        joint = GetComponent<ConfigurableJoint>();
        animator = GetComponent<Animator>();

        SetJointSettings(jointSpring);

    }
	
	// Update is called once per frame
	void Update () {
        //Calculate movement velocity
        float _xMov = Input.GetAxis("Horizontal");
        float _zMov = Input.GetAxis("Vertical");

        Vector3 _movHorizontal = transform.right * _xMov;
        Vector3 _movVertical = transform.forward * _zMov;

        //Fival movement vector
        Vector3 _velocity = (_movHorizontal + _movVertical) * speed;

        //Animate movement
        animator.SetFloat("ForwardVelocity", _zMov);    

        //Apply movement
        motor.Move(_velocity);

        //Calculate rotation as 3D vector (turning around)
        float _yRot = Input.GetAxisRaw("Mouse X");
        Vector3 _rotation = new Vector3(0f, _yRot, 0f) * lookSensivity;
        //Apply rotation
        motor.Rotate(_rotation);

        //Calculate camera rotation
        float _xRot = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRot * lookSensivity;
        motor.CameraRotate(_cameraRotationX);

        //calculate the thrusterforce based on input
        Vector3 _thrusterForce = Vector3.zero;

        if(Input.GetButton("Jump"))
        {
            _thrusterForce = Vector3.up * thrusterForce;
            SetJointSettings(0f);
        } else
        {
            SetJointSettings(jointSpring);
        }

        motor.ApplyThruster(_thrusterForce);
	}
    private void SetJointSettings (float _jointSpring)
    {
        joint.yDrive = new JointDrive
        {
            mode = jointMode,
            positionSpring = _jointSpring,
            maximumForce = jointMaxForce
        };   
    }
}
