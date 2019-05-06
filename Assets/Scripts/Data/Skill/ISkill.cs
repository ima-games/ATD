using UnityEngine;
/// <summary>
/// 技能接口
/// </summary>
public interface ISkill
{
    /// <summary>
    /// 技能初始化接口
    /// </summary>
    void InitSkill(GameObject master);

    /// <summary>
    /// 使用技能接口
    /// </summary>
    void ReleaseSkill(GameObject master);

    //....其余处理消息接口
}
