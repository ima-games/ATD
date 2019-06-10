
public class BuffData
{
    public int ID;
    public string Name;
    public int HpChange;//血量变化
    public double HpChange_p;//血量百分比变化
    public int AttackChange;//攻击力变化
    public double AttackChange_p;//攻击力百分比变化
    public double AttSpeedChange_p;//攻击速度百分比变化
    public double SpeedChange_p;//速度百分比变化
    public int HpReturnChange;//血量恢复数值
    public double HpReturnChange_p;//血量百分比恢复数值
    public int AddReviveCount;//增加复活次数

    public bool isTrigger;//是否是触发类型
    public double Time;//持续时间
    public int Count;//触发次数

    public bool isDecelerate;//减速
    public bool isVertigo;//眩晕
    public bool isParalysis;//麻痹
    public bool isSleep;//睡眠
    public bool isBound;//束缚
    public bool isBurn;//点燃
    public bool isCharm;//魅惑
    public bool isIncreaseAttSpeed;//攻速提高
    public bool isPoisoning;//中毒
    public bool isImmuneControl;//免疫控制
    public bool isRevenge;//复仇
    public bool isTaunt;//嘲讽
    public bool isIncreaseHpReturn;//回血速度提高
    public bool isIncreaseAttack;//攻击力提高
}
