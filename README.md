# Imitation Learning using ML-Agents (Under Development)

This project is based on the Imitation learning which uses the interaction between Teacher and Student rather than depending on reward/punishment mechanism accomodated by the Reinforcement learning. </br>

I am using the Hover Racer assets developed by Unity for the application of Imitation Learning using ML-Agents toolkit.

Objective - To automate the vehicle's movement such that it imitates the behaviour of the player.

This project is under development. </br>

## Work

* The student agent needs to understand Observations and Actions in order to be trained properly.

* The vehicle sends out a series of raycasts to detect the nearby objects around. The five raycasts are used around the agent which returns the value of the distance from the object it overlaps with.

* The value of the distance is passed if it hits any object otherwise -1 value is passed if it hits nothing.


![img](https://github.com/pkunjam/Imitation-Learning-using-mlagents/blob/master/Assets/raycast.PNG)


        Debug.DrawRay(transform.position, this.transform.forward * visibleDistance, Color.red);
        Debug.DrawRay(transform.position, this.transform.right * visibleDistance, Color.red);
        Debug.DrawRay(transform.position, -this.transform.right * visibleDistance, Color.red);
        Debug.DrawRay(transform.position, Quaternion.AngleAxis(45, Vector3.down) * this.transform.right * visibleDistance, Color.red);
        Debug.DrawRay(transform.position, Quaternion.AngleAxis(45, Vector3.up) * -this.transform.right * visibleDistance, Color.red);
        
        RaycastHit hit;

        //forward 
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
        if (Physics.Raycast(transform.position, Quaternion.AngleAxis(45, Vector3.down) * this.transform.right, out hit,visibleDistance))
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
        
* The CollectObservations() method for the agent is responsible for adding information of the nearby surrounding objects to avoid it. 

      public override void CollectObservations()
        {
       
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
        
          Vector3 localVelocity = transform.InverseTransformVector(rigidBody.velocity);
          Vector3 localAngularVelocity = transform.InverseTransformVector(rigidBody.angularVelocity);
          AddVectorObs(localVelocity.x);
          AddVectorObs(localVelocity.y);
          AddVectorObs(localVelocity.z);
          AddVectorObs(localAngularVelocity.y);

          print(localVelocity.x);
       }

* Further this information is sent to the Brain as it needs to know how many observations has been collected.

![img1](https://github.com/pkunjam/Imitation-Learning-using-mlagents/blob/master/Assets/brain.PNG)

* The AgentAction() method is responsible for performing actions during both training and testing mode.

      public override void AgentAction(float[] act, string txt)
       {
         float propulsion = driveForce * input.thruster - drag * Mathf.Clamp(speed, 0f, terminalVelocity);
         rigidBody.AddForce(transform.forward * propulsion, ForceMode.Acceleration);
         input.rudder = Mathf.Clamp(act[0], -1, 1);
         AddReward(.1f);
      }

* The observations during testing mode is saved in a .demo file which is further used for the training of the agent.

![img2](https://github.com/pkunjam/Imitation-Learning-using-mlagents/blob/master/Assets/training.PNG)

* Training Process - 

![img3](https://github.com/pkunjam/Imitation-Learning-using-mlagents/blob/master/Assets/mlagents.PNG)

## Tools used -

* [Unity](https://unity.com/) - Game Engine used
* [Visual Studio](https://visualstudio.microsoft.com/) - IDE used for scripting
* [ML-Agents tookit](https://github.com/Unity-Technologies/ml-agents) - Machine Learning Applications
