using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterDisplayName { 전사, 도적, 마법사, 궁수, 격투가 }

public enum CharacterTier { None, 흔한, 안흔한, 희귀한, 유일한, 전설적인 }

public enum CharacterAnimationState
{
    Idle,
    Running,
    Jumping,
    Attacking,
    Damaged,
    Dead
}

public enum SpriteType { None, Bg, BgOutline, ImgCharacter }

public enum TabMenu { TabMenuShop, TabMenuCharacter, TabMenuLobby, TabMenuRelics, TabMenuPreferences }

public enum SelectSkill { One, Two, Three, Four, Five, Six, Seven }