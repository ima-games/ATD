using UnityEngine;
/// <summary>
/// 技能接口
/// </summary>
public interface ISkill
{
    /// <summary>
    /// 技能初始化接口
    /// </summary>
    void InitSkill(Individual master);

    /// <summary>
    /// 使用技能接口
    /// </summary>
    void ReleaseSkill(Individual master);

    /// <summary>
    /// 技能每帧更新
    /// </summary>
    /// <param name="master"></param>
    void UpdateSkill(Individual master);

    /// <summary>
    /// 技能是否冷却
    /// </summary>
    /// <returns></returns>
    bool IsColdTimeEnd();

    /// <summary>
    /// 技能冷却百分比
    /// </summary>
    /// <returns></returns>
    float GetColdTimePercent();
}
