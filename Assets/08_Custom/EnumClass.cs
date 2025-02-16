using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterDisplayName { 전사, 도적, 마법사, 궁수, 격투가 }

public enum CharacterTier { None, 흔한, 안흔한, 희귀한, 유일한, 전설적인 }

public enum CharacterAttackType { Normal, Arrow, Magic }

public enum AnimatorState
{
    Idle,
    Idle_LongSpear,
    Move,
    Move_LongSpear,
    Attack_Normal,
    Attack_Bow,
    Attack_Magic,
    Attack_Axe,
    Attack_ShotSword,
    Concentrate,
    Buff,
    Debuff,
    Skill_Normal,
    Skill_Bow,
    Skill_Magic,
    Hit,
    Death,
    Other
}

public enum SpriteType { None, Bg, BgOutline, ImgCharacter }

public enum TabMenu { TabMenuShop, TabMenuCharacter, TabMenuLobby, TabMenuRelics, TabMenuPreferences }

public enum SelectSkill { One, Two, Three, Four, Five, Six, Seven }

public enum EnemyType { 일반, 체력형, 방어형, 속도형, 보스 }