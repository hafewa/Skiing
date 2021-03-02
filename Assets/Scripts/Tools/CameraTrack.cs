using System.Collections;
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
    private void FixedUpdate() {
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
        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * dt);//Time.GetMyDeltaTime());

        // Damp the height
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * dt);//Time.GetMyDeltaTime());
                                                                                    //System.Console.WriteLine("dt: {0}", dt);//Time.GetMyDeltaTime());
                                                                                    //Debug.Log("dt: " + Time.deltaTime);

        // Convert the angle into a rotation
        //Quaternion currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

        
        // Set the position of the camera on the x-z plane to:
        // distance meters behind the target
        //transform.position = target.position;
        if(!Player.Instance.isGameOver || true)
        {
            oldPostions.Add(target.position);

            //冲锋额外拉远
            curDistance += 0;//1.3f * Player.Instance.curSpeedUpTime / Player.Instance.speedUpTime;

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

            Vector3 pos = (Vector3)oldPostions[0] - Vector3.forward * curDistance + new Vector3(0, curHeight,0);
            //pos.y = currentHeight;

            // Set the height of the camera
            transform.position = pos;

            // Always look at the target
            // if (Player.Instance.m_nRunFrm < 30)
            // {
            //     
            //     var tmpT = Player.Instance.transform.position;
            //     tmpT = new Vector3(transform.position.x, transform.position.y - height, tmpT.z);
            //     tmpT += new Vector3(0, (30 - Player.Instance.m_nRunFrm) * 0.15f, 0);
            //     transform.LookAt(tmpT);
            // }
            // else
            {
                var tmpT = Player.Instance.transform.position;

                tmpT = new Vector3(transform.position.x, transform.position.y - curHeight, tmpT.z);
                transform.LookAt(tmpT);
            }
        }

        // //玩家到终点后镜头缓慢向前
        // if( Player.Instance.isGameOver && !Player.Instance.isDie) {
        //     var curPos = transform.localPosition;
        //     curPos.z += EndMoveSpeed;
        //     transform.localPosition = curPos;
        // }
        
    }

}
