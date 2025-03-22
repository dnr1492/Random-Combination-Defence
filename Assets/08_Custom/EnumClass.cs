using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterDisplayName { ����, ����, ������, �ü�, ������, ������, â����, �ּ���, ����, �����, ���˻�, ����, ��ؽ�, ������, ��������, ������, ����Ŀ, ����ũ��, ��â��, �渶����, �Ҽ���, ����̵�, �ٵ�,
                                   �鳪��Ʈ, ����, ���ε�, ũ�缼�̴�, ���, ��������, ����Ʈ����Ŀ, �繫����, ����, ��ī��Ʈ, Ʈ����, �������, �����̾�, ��Ʈ����Ŀ, ������, ��������, ���峪��Ʈ, ���̵��, 
                                   �ȶ�ũ��, ���̷���, ���̽�Ʈ����, �����췣��, ī����è�Ǿ�, ����, ���ݷ�, ��ũ������, ��Ʋ������, ������Ż����Ʈ, �����󸮽�Ʈ, ������Ʈ, ���ϵ����, ���, ����μ����� }

public enum CharacterTier { None, ����, ������, �����, ������, ��������, �ʿ��� }

public enum CharacterAttackType { Normal, Bow, Magic, LongSpear, Axe, ShotSword }

public enum CharacterDamageType { Melee, Magic, Blood }

public enum SpriteType { None, Bg, BgOutline, ImgCharacter }

public enum SelectSkill { One, Two, Three, Four, Five, Six, Seven }

public enum EnemyType { �Ϲ�, ü����, �����, �ӵ���, ���� }

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
    Summon,  //��ȯ
    Combination  //����
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