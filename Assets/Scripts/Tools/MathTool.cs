using UnityEngine;

public struct MathTool
{
    public const float MPH2KMH = 1.609344F;
    public const float KMH2MPH = 0.6213712F;

    /// <summary>
    ///  每帧移动距离/米
    /// </summary>
    /// <returns></returns>
    public static float GetMeterPerFrame(float mph) {

        return mph * MPH2KMH / 3.6f * Time.fixedDeltaTime;
    }

    #region 时速等级
    public const float BaseMPH = 70f * 1.15f;
    public static float GetMPHForShow(float lv) {
        return BaseMPH + GetTotalMPHGrowth(lv);
    }
    public static float GetMPHForUse(float lv) {
        ////纪子易(纪子易) 09-16 11:01:36
        ////但是实际给他的速度数值。到60级就不加了
        //lv = (lv > 60 ? 60 : lv);
        return BaseMPH + GetTotalMPHGrowth(lv);
    }

    public static float GetTotalMPHGrowth(float lv) {
        return (5 / 6f) * (lv - 1);// Mathf.Pow(1.01f, lv - 1);
    }
    #endregion

    /// <summary>
    ///  最大时速耗金
    /// </summary>
    /// <param name="lv"></param>
    /// <returns></returns>
    public static int GetUpgradeGoldForMPH(int lv) {
        return (int)(240f * Mathf.Pow(1.112f, lv - 1) / 10) * 10;
    }

    /// <summary>
    ///  加速度耗金
    /// </summary>
    /// <param name="lv"></param>
    /// <returns></returns>
    public static int GetUpgradeGoldForG(int lv) {
        return (int)(240f * Mathf.Pow(1.112f, lv - 1) / 10) * 10;
    }

    /// <summary>
    ///  金币获取耗金
    /// </summary>
    /// <param name="lv"></param>
    /// <returns></returns>
    public static int GetUpgradeGoldForEarnings(int lv) {
        return (int)(240f * Mathf.Pow(1.122f, lv - 1) / 10) * 10;
    }

    #region 加速等级
    public const float BaseG = 80f;
    public static float GetSpeedUpG(float lv) {
        return BaseG * GetTotalSpeedUpGGrowth(lv);
    }

    public static float GetTotalSpeedUpGGrowth(float lv) {
        return Mathf.Pow(1.02f, lv - 1);
    }
    #endregion

    #region 离线收益等级 /每小时
    public const float BaseX = 530f;
    public static float GetXLv(float lv) {
        return BaseX * GetTotalXGrowth(lv);
    }

    public static float GetTotalXGrowth(float lv) {
        return Mathf.Pow(1.065f, lv - 1);
    }
    #endregion

    /// <summary>
    /// 获取两点Z轴距离
    /// </summary>
    /// <param name="point1"></param>
    /// <param name="point2"></param>
    /// <returns></returns>
    public static float GetZDis(Vector3 point1, Vector3 point2) {
        return Mathf.Abs(point1.z - point2.z);
    }

    /// <summary>
    /// 获取钻圈金币倍数
    /// </summary>
    /// <param name="PassStageCount"></param>
    /// <returns></returns>
    public static int GetSpeedUpGold(int PassStageCount) {
        return (int)Mathf.Pow(1.15f, ((PassStageCount + 1) / 10f));
    }

    /// <summary>
    ///  通关收益基础
    /// </summary>
    /// <param name="passStageCount"></param>
    /// <returns></returns>
    public static int GetPassStageGold(int passStageCount) {
        return (int)(180 * Mathf.Pow(1.019f, passStageCount + 1) / 10) * 10;
    }
}
