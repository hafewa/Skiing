using System.Runtime.InteropServices;
using UnityEngine;

public class VibrateTool
{

    [DllImport("__Internal")]  
    private  static extern void CallIosFun(int val,string strParam1);

    public static void Vibrate() {
        if (GameFacade.Instance.isCanVibrate)
        {
#if UNITY_IPHONE
     
        CallIosFun(9,"aa");
#endif
            Handheld.Vibrate();

        }
            
    }
}