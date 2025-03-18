using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterDisplayName { 전사, 도적, 마법사, 궁수, 격투가, 광전사, 창술사, 주술사, 사제, 성기사, 마검사, 닌자, 어쌔신, 레인저, 스나이퍼, 파이터, 버서커, 파이크맨, 마창사, 흑마술사, 소서러, 드루이드, 바드 }

public enum CharacterTier { None, 흔한, 안흔한, 희귀한, 유일한, 전설적인, 초월한 }

public enum CharacterAttackType { Normal, Bow, Magic, LongSpear, Axe, ShotSword }

public enum CharacterDamageType { Melee, Magic, Blood }

public enum SpriteType { None, Bg, BgOutline, ImgCharacter }

public enum SelectSkill { One, Two, Three, Four, Five, Six, Seven }

public enum EnemyType { 일반, 체력형, 방어형, 속도형, 보스 }

public enum CombatEffectType
{
    None,
    Hit_Attack_Normal,
    Hit_Attack_Bow,
    Hit_Attack_Magic,
    Hit_Attack_LongSpear,
    Hit_Attack_Axe,
    Hit_Attack_ShotSword,
    Concentrate,
    Buff,
    Debuff,
    Hit_Skill_Normal,
    Hit_Skill_Bow,
    Hit_Skill_Magic
}

public enum PlayEffectType
{
    None,
    Summon,  //소환
    Combination  //조합
}

public enum AnimatorState
{
    Idle,
    Idle_LongSpear,
    Move,
    Move_LongSpear,
    Attack_Normal,
    Attack_Bow,
    Attack_Magic,
    Attack_LongSpear,
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

public enum TabMenu { TabMenuShop, TabMenuCharacter, TabMenuLobby, TabMenuRelics, TabMenuPreferences }

public enum FastForward { X1, X2, X3 }