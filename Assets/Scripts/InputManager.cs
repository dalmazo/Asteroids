using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public InputManagerSystem inputManagerSystem;

    public KeyCode pcKeycodePause;
    public KeyCode pcKeycodeShoot;
    public KeyCode pcKeycodeForward;
    public KeyCode pcKeycodeSteerLeft;
    public KeyCode pcKeycodeSteerRight;
    public KeyCode pcKeycodeHyperspace;

    void Update()
    {

        switch (inputManagerSystem)
        {
            case InputManagerSystem.pc:

                if (Input.GetKeyDown(pcKeycodePause))
                {
                    GameManager.instance.pauseGame.Pause();
                }

                if (Input.GetKeyDown(pcKeycodeShoot))
                {
                    GameManager.instance.playerScript.Fire();
                }

                if (Input.GetKey(pcKeycodeSteerLeft))
                {
                    GameManager.instance.playerScript.Steer(1);
                }
                if (Input.GetKey(pcKeycodeSteerRight))
                {
                    GameManager.instance.playerScript.Steer(-1);
                }

                if (Input.GetKey(pcKeycodeForward))
                {
                    GameManager.instance.playerScript.MoveForward();
                }

                if (!Input.GetKey(pcKeycodeSteerRight) && !Input.GetKey(pcKeycodeSteerLeft) && !Input.GetKey(pcKeycodeForward))
                {
                    GameManager.instance.playerScript.Steer(0);
                }

                if (Input.GetKeyDown(pcKeycodeHyperspace))
                {
                    GameManager.instance.playerScript.GoHyperspace();
                }



                break;
        }

    }
}
