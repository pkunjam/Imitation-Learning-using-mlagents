using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Vehicles.Car;
using MLAgents;
using UnityEngine.UI;
public class Car : Agent
{

    Rigidbody rigidBody;
    private CarController m_Car;
    WheelCollider WheelFrontRight;
    private Vector3 reSpawnPositon;
    private Quaternion reSpawnRotation;

    public float visibleDistance = 20.0f;
    private float fDist = 20.0f;
    private float rDist = 20.0f;
    private float lDist = 20.0f;
    private float r45Dist = 20.0f;
    private float l45Dist = 20.0f;


    public Text rewardText;

    public Text forwardDistance;
    public Text rightDistance;
    public Text leftDistance;
    public Text right45Distance;
    public Text left45Distance;

    public Text veloX;
    public Text veloY;
    public Text veloZ;

    private void Awake()
    {
        m_Car = GetComponent<CarController>();
        rigidBody = GetComponent<Rigidbody>();
        WheelFrontRight = GameObject.FindGameObjectWithTag("carWheelTrainer").GetComponent<WheelCollider>();
        reSpawnPositon = GameObject.FindGameObjectWithTag("startPosition").transform.position;
        reSpawnRotation = GameObject.FindGameObjectWithTag("startPosition").transform.rotation;
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        float h = vectorAction[0];
        float v = vectorAction[1];
        m_Car.Move(h, v, v, 0);
    }

    public override void CollectObservations()
    {
        //1
        if (fDist != -1f)
        {
            AddVectorObs(fDist);
            AddVectorObs(1f);
        }
        else
        {
            AddVectorObs(1f);
            AddVectorObs(0f);
        }
        //2
        if (rDist != -1f)
        {
            AddVectorObs(rDist);
            AddVectorObs(1f);
        }
        else
        {
            AddVectorObs(1f);
            AddVectorObs(0f);
        }

        //3
        if (lDist != -1f)
        {
            AddVectorObs(lDist);
            AddVectorObs(1f);
        }
        else
        {
            AddVectorObs(1f);
            AddVectorObs(0f);
        }

        //4
        if (r45Dist != -1f)
        {
            AddVectorObs(r45Dist);
            AddVectorObs(1f);
        }
        else
        {
            AddVectorObs(1f);
            AddVectorObs(0f);
        }

        //5
        if (l45Dist != -1f)
        {
            AddVectorObs(l45Dist);
            AddVectorObs(1f);
        }
        else
        {
            AddVectorObs(1f);
            AddVectorObs(0f);
        }


        Vector3 localVelocity = transform.InverseTransformVector(rigidBody.velocity);
        Vector3 localAngularVelocity = transform.InverseTransformVector(rigidBody.angularVelocity);
        AddVectorObs(localVelocity.x);
        AddVectorObs(localVelocity.y);
        AddVectorObs(localVelocity.z);
        AddVectorObs(localAngularVelocity.y);

        print(localVelocity.x);
    }


    void Update()
    {

        Vector3 localVelocity = transform.InverseTransformVector(rigidBody.velocity);
        Vector3 localAngularVelocity = transform.InverseTransformVector(rigidBody.angularVelocity);
        AddVectorObs(localVelocity.x);
        AddVectorObs(localVelocity.y);
        AddVectorObs(localVelocity.z);
        AddVectorObs(localAngularVelocity.y);

        float h = CrossPlatformInputManager.GetAxis("Horizontal");
        float v = CrossPlatformInputManager.GetAxis("Vertical");
        m_Car.Move(h, v, v, 0);
        AddReward(.001f);
        //rewardText.text = "Reward: " + GetCumulativeReward().ToString();


        //forwardDistance.text = "forward Distance: " + fDist;
        //rightDistance.text = "right Distance: " + fDist;
        //leftDistance.text = "left Distance: " + fDist;
        //right45Distance.text = "right 45 Distance: " + fDist;
        //left45Distance.text = "left 45 Distance: " + fDist;
        //veloX.text = "Velocity X: " + localVelocity.x;
        //veloY.text = "Velocity Y: " + localVelocity.y;
        //veloZ.text = "Velocity Z: " + localVelocity.z;


        Debug.DrawRay(transform.position, this.transform.forward * visibleDistance, Color.red);
        Debug.DrawRay(transform.position, this.transform.right * visibleDistance, Color.red);
        Debug.DrawRay(transform.position, -this.transform.right * visibleDistance, Color.red);
        Debug.DrawRay(transform.position, Quaternion.AngleAxis(45, Vector3.down) * this.transform.right * visibleDistance, Color.red);
        Debug.DrawRay(transform.position, Quaternion.AngleAxis(45, Vector3.up) * -this.transform.right * visibleDistance, Color.red);


        RaycastHit hit;

        //forward Distance
        if (Physics.Raycast(transform.position, this.transform.forward, out hit, visibleDistance))
        {
            fDist = hit.distance / visibleDistance;
        }
        else { fDist = -1f; }
        //right
        if (Physics.Raycast(transform.position, this.transform.right, out hit, visibleDistance))
        {
            rDist = hit.distance / visibleDistance;
        }
        else { rDist = -1f; }
        //left
        if (Physics.Raycast(transform.position, -this.transform.right, out hit, visibleDistance))
        {
            lDist = hit.distance / visibleDistance;
        }
        else { lDist = -1f; }

        //right 45
        if (Physics.Raycast(transform.position, Quaternion.AngleAxis(45, Vector3.down) * this.transform.right, out hit, visibleDistance))
        {
            r45Dist = hit.distance / visibleDistance;
        }
        else { r45Dist = -1f; }

        //left 45
        if (Physics.Raycast(transform.position, Quaternion.AngleAxis(45, Vector3.up) * -this.transform.right, out hit, visibleDistance))
        {
            l45Dist = hit.distance / visibleDistance;
        }
        else { l45Dist = -1f; }

    }


    void OnCollisionEnter(Collision collision)
    {
        Vector3 colPosition = gameObject.transform.position;
        float positionX = gameObject.transform.position.x;
        float positionY = gameObject.transform.position.y;
        float positionZ = gameObject.transform.position.z;
        WheelHit hitGround;
        bool grounded = WheelFrontRight.GetGroundHit(out hitGround);

        if (collision.gameObject.tag == "wallOutside")
        {
 
            if (grounded)
            {
                if (hitGround.collider.tag == "plane1")
                {
                    reSpawnPositon = new Vector3(positionX + 5, positionY, positionZ);
                    reSpawnRotation = Quaternion.Euler(0, 0, 0);
                }
                if (hitGround.collider.tag == "plane2")
                {
                    reSpawnPositon = new Vector3(positionX, positionY, positionZ - 5);
                    reSpawnRotation = Quaternion.Euler(0, 90, 0);
                }

                if (hitGround.collider.tag == "plane3")
                {
                    reSpawnPositon = new Vector3(positionX - 5, positionY, positionZ);
                    reSpawnRotation = Quaternion.Euler(0, 180, 0);
                }

                if (hitGround.collider.tag == "plane4")
                {
                    reSpawnPositon = new Vector3(positionX, positionY, positionZ + 5);
                    reSpawnRotation = Quaternion.Euler(0, -90, 0);
                }
                Done();
            }
        }

        if (collision.gameObject.tag == "wallInside")
        {
            if (grounded)
            {
                if (hitGround.collider.tag == "plane1")
                {
                    reSpawnPositon = new Vector3(positionX - 5, positionY, positionZ);
                    reSpawnRotation = Quaternion.Euler(0, 0, 0);
                }
                if (hitGround.collider.tag == "plane2")
                {
                    reSpawnPositon = new Vector3(positionX, positionY, positionZ + 5);
                    reSpawnRotation = Quaternion.Euler(0, 90, 0);
                }

                if (hitGround.collider.tag == "plane3")
                {
                    reSpawnPositon = new Vector3(positionX + 5, positionY, positionZ);
                    reSpawnRotation = Quaternion.Euler(0, 180, 0);
                }

                if (hitGround.collider.tag == "plane4")
                {
                    reSpawnPositon = new Vector3(positionX, positionY, positionZ - 5);
                    reSpawnRotation = Quaternion.Euler(0, -90, 0);
                }
                Done();
            }
        }
    }


    public  override void AgentReset()
    {
        transform.position = reSpawnPositon;
        transform.rotation = reSpawnRotation;
        rigidBody.velocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;
    }

}
