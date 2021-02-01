using System;
using UnityEngine;

/// <summary>
/// 计时任务工具升级版
/// </summary>
public class TimerSvcTool : UnityInstanceBase<TimerSvcTool>
{
    private PETimer pt = null;
    protected override void Awake() {
        base.Awake();
        DontDestroyOnLoad(this.gameObject);

        pt = new PETimer();
        //设置日志输出
        pt.SetLog((string info) =>
        {
            Debug.LogError("TimerSvcTool:" + info);
        });
    }

    private void Update() {
        pt.Update();
    }

    public int AddTimeTask(Action<int> callback, double delay, PETimeUnit timeUnit = PETimeUnit.Millisecond, int count = 1) {
        return pt.AddTimeTask(callback, delay, timeUnit, count);
    }

    public double GetNowTime() {
        return pt.GetMillisecondsTime();
    }
    public UInt32 GetSecTime()
    {
        return (UInt32)(pt.GetMillisecondsTime()/1000);
    }
    public void DelTask(int tid) {
        pt.DeleteTimeTask(tid);
    }

    public void Clear() {
        pt.Clear();
    }
}