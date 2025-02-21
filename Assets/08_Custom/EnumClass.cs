using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterDisplayName { ����, ����, ������, �ü�, ������ }

public enum CharacterTier { None, ����, ������, �����, ������, ��������, �ʿ��� }

public enum CharacterAttackType { Normal, Bow, Magic, LongSpear, Axe, ShotSword }

public enum CharacterDamageType { Melee, Chaos, Blood }

public enum SpriteType { None, Bg, BgOutline, ImgCharacter }

public enum SelectSkill { One, Two, Three, Four, Five, Six, Seven }

public enum EnemyType { �Ϲ�, ü����, �����, �ӵ���, ���� }

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

public enum PlaySpeed { X1, X2, X3 }