using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterDisplayName { ����, ����, ������, �ü�, ������ }

public enum CharacterTier { None, ����, ������, �����, ������, �������� }

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