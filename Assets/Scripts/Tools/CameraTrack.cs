using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrack : MonoBehaviour
{
    public float EndMoveSpeed;

    // The target we are following
    Transform target;

    // The distance in the x-z plane to the target
    public float distance = 2.0f;

    // the height we want the camera to be above the target
    public float height = 4.0f;

    // How much we 
    public float heightDamping = 2.0f;
    public float rotationDamping = 3.0f;

    //public int m_nStartFrm = 0;

    private Vector3[] vPassPt;

    private ArrayList oldPostions = new ArrayList();

    //private int m_nRunState = 0;    //0准备开始  1飞行中  2死亡
    float curDistance;
    private float curHeight;
    private Queue<Vector3> oldVelocityQueue = new Queue<Vector3>();

    private void FixedUpdate()
    {
        // Early out if we don't have a target
        //if(Player.Instance.isStart)
        //m_nStartFrm++;
        var target = Player.Instance.transform;
        if (!target)
            return;

        // Calculate the current rotation angles
        float wantedRotationAngle = target.eulerAngles.y;
        float wantedHeight = target.position.y + height;

        float currentRotationAngle = transform.eulerAngles.y;
        float currentHeight = transform.position.y;

        // Damp the rotation around the y-axis
        float dt = Time.deltaTime;
        currentRotationAngle =
            Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * dt); //Time.GetMyDeltaTime());

        // Damp the height
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * dt); //Time.GetMyDeltaTime());
        //System.Console.WriteLine("dt: {0}", dt);//Time.GetMyDeltaTime());
        //Debug.Log("dt: " + Time.deltaTime);

        // Convert the angle into a rotation
        //Quaternion currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);


        // Set the position of the camera on the x-z plane to:
        // distance meters behind the target
        //transform.position = target.position;
        if (!Player.Instance.isGameOver || true)
        {
            oldPostions.Add(target.position);

            //冲锋额外拉远
            curDistance += 0; //1.3f * Player.Instance.curSpeedUpTime / Player.Instance.speedUpTime;

            while (oldPostions.Count >= 6)
                oldPostions.RemoveAt(0);

            if (Player.Instance.isStart)
            {
                curDistance += (distance - curDistance) * 0.1f;
                curHeight += (height - curHeight) * 0.1f;
            }
            else
            {
                curDistance = 20;
                curHeight = 10;
            }

            Vector3 pos = (Vector3) oldPostions[0] + Vector3.back * curDistance + Vector3.up * curHeight;
            //pos.y = currentHeight;

            // Set the height of the camera
            // transform.position = pos;


            // var tmpT = target.position;
            // // Always look at the target
            // if (Player.Instance.m_nRunFrm < 30)
            // {
            //     tmpT = new Vector3(transform.position.x, transform.position.y - curHeight, tmpT.z);
            //     tmpT += new Vector3(0, (30 - Player.Instance.m_nRunFrm) * 0.15f, 0);
            //     transform.LookAt(tmpT);
            // }
            // else
            // {
            //     tmpT = new Vector3(transform.position.x, transform.position.y - curHeight, tmpT.z);
            //     transform.LookAt(tmpT);
            // }

            #region 滑雪跟拍效果

            var curVelocity = Player.Instance.curVelocity;
            oldVelocityQueue.Enqueue(curVelocity);
            while (oldVelocityQueue.Count > 6)
            {
                oldVelocityQueue.Dequeue();
            }

            var oldVelocity = oldVelocityQueue.Peek();

            if (Player.Instance.curCamRotateAngle < 360)
            {
                #region 玩家起跳离地后的镜头环绕效果

                Player.Instance.curCamRotateAngle += Player.Instance.camRotateSpeed;
                var dir = Quaternion.AngleAxis(Player.Instance.curCamRotateAngle, Vector3.up) * Vector3.back;

                var newPos = target.position + dir * curDistance + Vector3.up * curHeight / 2;
                transform.position = newPos;

                #endregion
            }
            else
            {
                if (oldVelocity.magnitude < 5f)
                {
                    transform.position = pos;
                }
                else
                {
                    var newPos =  target.position - oldVelocity.normalized * curDistance +
                                  Vector3.up * curHeight / 2;

                    transform.position = Vector3.Lerp(transform.position, newPos, dt * 6);
                }
            }

            transform.LookAt(target);

            #endregion
        }

        // //玩家到终点后镜头缓慢向前
        // if( Player.Instance.isGameOver && !Player.Instance.isDie) {
        //     var curPos = transform.localPosition;
        //     curPos.z += EndMoveSpeed;
        //     transform.localPosition = curPos;
        // }
    }
}