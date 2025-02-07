using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterDisplayName { ����, ����, ������, �ü�, ������ }

public enum CharacterTier { None, ����, ������, �����, ������, �������� }

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
    Damaged,
    Death,
    Other
}

public enum SpriteType { None, Bg, BgOutline, ImgCharacter }

public enum TabMenu { TabMenuShop, TabMenuCharacter, TabMenuLobby, TabMenuRelics, TabMenuPreferences }

public enum SelectSkill { One, Two, Three, Four, Five, Six, Seven }