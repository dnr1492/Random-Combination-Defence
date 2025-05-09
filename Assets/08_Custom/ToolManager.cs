using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public class ToolManager
{
    private static void LoadDataFromJSON<T>(T data, string fileName)
    {
        //Resources/Datas 경로
        string path = Path.Combine(Application.dataPath, "Resources/Datas");
        string jsonPath = Path.Combine(path, fileName);

        //JSON 파일 저장
        string jsonData = JsonUtility.ToJson(data, true);
        File.WriteAllText(jsonPath, jsonData);

        Debug.Log($"JSON data saved at: {jsonPath}");
        AssetDatabase.Refresh();
    }

    #region CharacterCardLevelInfoData
    private static string[] characterDisplayNames = Enum.GetNames(typeof(CharacterDisplayName));
    private static readonly Dictionary<string, Dictionary<int, CharacterCardLevelInfoData>> characterConfigs = new Dictionary<string, Dictionary<int, CharacterCardLevelInfoData>>()
    { 
            #region Tier: 흔한
            {
                "전사", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(전사) 테스트 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(전사) 테스트 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            {
                "도적", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(도적) 테스트 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(도적) 테스트 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            {
                "마법사", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(마법사) 테스트 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(마법사) 테스트 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            {
                "궁수", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(궁수) 테스트 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(궁수) 테스트 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            {
                "격투가", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(격투가) 테스트 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(격투가) 테스트 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            {
                "광전사", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(광전사) 테스트 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(광전사) 테스트 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            {
                "창술사", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(창술사) 테스트 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(창술사) 테스트 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            {
                "주술사", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(주술사) 테스트 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(주술사) 테스트 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            {
                "사제", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(사제) 테스트 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(사제) 테스트 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            #endregion
            #region Tier: 안흔한
            {
                "성기사", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(성기사) 테스트 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(성기사) 테스트 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            {
                "마검사", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(마검사) 테스트 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(마검사) 테스트 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            {
                "닌자", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(닌자) 테스트 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(닌자) 테스트 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            {
                "어쌔신", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(어쌔신) 테스트 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(어쌔신) 테스트 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            {
                "레인저", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(레인저) 테스트 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(레인저) 테스트 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            {
                "스나이퍼", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(스나이퍼) 테스트 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(스나이퍼) 테스트 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            {
                "파이터", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(파이터) 테스트 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(파이터) 테스트 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            {
                "버서커", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(버서커) 테스트 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(버서커) 테스트 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            {
                "파이크맨", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(파이크맨) 테스트 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(파이크맨) 테스트 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            {
                "마창사", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(마창사) 테스트 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(마창사) 테스트 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            {
                "흑마술사", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(흑마술사) 테스트 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(흑마술사) 테스트 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            {
                "소서러", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(소서러) 테스트 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(소서러) 테스트 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            {
                "드루이드", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(드루이드) 테스트 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(드루이드) 테스트 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            {
                "바드", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(바드) 테스트 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(바드) 테스트 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            #endregion
            #region Tier: 희귀한
            {
                "룬나이트", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(룬나이트) 테스트 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(룬나이트) 테스트 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            {
                "워든", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(워든) 테스트 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(워든) 테스트 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            {
                "워로드", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(워로드) 테스트 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(워로드) 테스트 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            {
                "크루세이더", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(크루세이더) 테스트 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(크루세이더) 테스트 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            {
                "어벤저", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(어벤저) 테스트 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(어벤저) 테스트 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            {
                "데스리퍼", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(데스리퍼) 테스트 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(데스리퍼) 테스트 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            {
                "나이트스토커", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(나이트스토커) 테스트 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(나이트스토커) 테스트 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            {
                "사무라이", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(사무라이) 테스트 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(사무라이) 테스트 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            {
                "헌터", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(헌터) 테스트 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(헌터) 테스트 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            {
                "스카우트", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(스카우트) 테스트 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(스카우트) 테스트 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            {
                "트래퍼", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(트래퍼) 테스트 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(트래퍼) 테스트 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            {
                "데드아이", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(데드아이) 테스트 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(데드아이) 테스트 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            {
                "슬레이어", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(슬레이어) 테스트 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(슬레이어) 테스트 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            {
                "스트라이커", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(스트라이커) 테스트 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(스트라이커) 테스트 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            {
                "수도사", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(수도사) 테스트 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(수도사) 테스트 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            {
                "워마스터", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(워마스터) 테스트 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(워마스터) 테스트 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            {
                "블러드나이트", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(블러드나이트) 테스트 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(블러드나이트) 테스트 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            {
                "블레이드댄서", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(블레이드댄서) 테스트 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(블레이드댄서) 테스트 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            {
                "팔랑크스", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(팔랑크스) 테스트 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(팔랑크스) 테스트 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            {
                "하이랜서", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(하이랜서) 테스트 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(하이랜서) 테스트 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            {
                "헤이스트랜서", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(헤이스트랜서) 테스트 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(헤이스트랜서) 테스트 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            {
                "섀도우랜서", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(섀도우랜서) 테스트 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(섀도우랜서) 테스트 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            {
                "카오스챔피언", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(카오스챔피언) 테스트 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(카오스챔피언) 테스트 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            {
                "워록", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(워록) 테스트 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(워록) 테스트 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            {
                "둠콜러", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(둠콜러) 테스트 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(둠콜러) 테스트 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            {
                "아크메이지", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(아크메이지) 테스트 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(아크메이지) 테스트 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            {
                "배틀메이지", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(배틀메이지) 테스트 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(배틀메이지) 테스트 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            {
                "엘리멘탈리스트", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(엘리멘탈리스트) 테스트 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(엘리멘탈리스트) 테스트 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            {
                "스펠듀얼리스트", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(스펠듀얼리스트) 테스트 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(스펠듀얼리스트) 테스트 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            {
                "프리스트", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(프리스트) 테스트 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(프리스트) 테스트 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            {
                "와일드워든", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(와일드워든) 테스트 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(와일드워든) 테스트 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            {
                "비숍", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(비숍) 테스트 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(비숍) 테스트 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            {
                "디바인섀도우", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(디바인섀도우) 테스트 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(디바인섀도우) 테스트 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            }
            #endregion
    };

    [MenuItem("정재욱/Generate characterCardLevelInfo_data")]
    private static void GenerateCharacterCardLevelInfoData()
    {
        //JSON 데이터 생성
        CharacterCardDataList list = new CharacterCardDataList
        {
            characterCardDatas = new List<CharacterCardData>()
        };

        //데이터 추가
        foreach (string displayName in characterDisplayNames)
        {
            CharacterCardData character = new CharacterCardData
            {
                display_name = displayName,
                levels = new List<CharacterCardLevelInfoData>()
            };

            if (characterConfigs.TryGetValue(displayName, out var levelDataMap))
            {
                foreach (var levelDataEntry in levelDataMap)
                {
                    int level = levelDataEntry.Key;
                    CharacterCardLevelInfoData data = levelDataEntry.Value;

                    character.levels.Add(new CharacterCardLevelInfoData
                    {
                        level = level,
                        damage = data.damage,
                        attackSpeed = data.attackSpeed,
                        moveSpeed = data.moveSpeed,
                        skill = data.skill,
                        description = GenerateDescription(data.damage, data.attackSpeed, data.moveSpeed, data.skill)
                    });
                }
            }

            list.characterCardDatas.Add(character);
        }

        LoadDataFromJSON(list, "characterCardLevelInfo_data.json");
    }

    private static string GenerateDescription(int damage, float attackSpeed, float moveSpeed, string skill)
    {
        List<string> parts = new List<string>();

        if (damage > 0) parts.Add($"공격력이 {damage}만큼 증가");
        if (attackSpeed > 0) parts.Add($"공격속도가 {attackSpeed * 100}%만큼 증가");
        if (moveSpeed > 0) parts.Add($"이동속도가 {moveSpeed * 100}%만큼 증가");
        if (!string.IsNullOrEmpty(skill)) parts.Add($"{skill} 획득");

        return string.Join(", ", parts);
    }
    #endregion

    #region CharacterCardLevelData
    private static readonly int maxLevel = 12;

    [MenuItem("정재욱/Generate characterCardLevel_data")]
    private static void GenerateCharacterCardLevelData()
    {
        //JSON 데이터 생성
        CharacterCardLevelDataList list = new CharacterCardLevelDataList
        {
            characterCardLevelDatas = new List<CharacterCardLevelData>()
        };

        //데이터 추가
        for (int i = 1; i < maxLevel; i++)
        {
            list.characterCardLevelDatas.Add(new CharacterCardLevelData
            {
                level = i,
                quantityTierCommon = 10 * i,
                quantityTierUncommon = 8 * i,
                quantityTierRare = 6 * i,
                quantityTierUniqe = 4 * i,
                quantityTierLegendary = 2 * i
            });
        }

        LoadDataFromJSON(list, "characterCardLevel_data.json");
    }
    #endregion

    #region CharacterData
    [MenuItem("정재욱/Generate character_data")]
    private static void GenerateCharacterData()
    {
        //JSON 데이터 생성
        CharacterDataList list = new CharacterDataList
        {
            characterDatas = new List<CharacterData>()
        };

        //데이터 추가
        #region Tier: 흔한
        list.characterDatas.Add(new CharacterData { displayName = "전사", damage = 30f, attackSpeed = 1f, attackRange = 1f, moveSpeed = 2f, tierNum = 1, attackType = CharacterAttackType.Normal, damageType = CharacterDamageType.Melee, skill_1_name = "", skill_2_name = "", skill_3_name = "" });
        list.characterDatas.Add(new CharacterData { displayName = "도적", damage = 20f, attackSpeed = 0.5f, attackRange = 0.8f, moveSpeed = 2f, tierNum = 1, attackType = CharacterAttackType.Normal, damageType = CharacterDamageType.Melee, skill_1_name = "", skill_2_name = "", skill_3_name = "" });
        list.characterDatas.Add(new CharacterData { displayName = "격투가", damage = 25f, attackSpeed = 0.8f, attackRange = 1f, moveSpeed = 2f, tierNum = 1, attackType = CharacterAttackType.Normal, damageType = CharacterDamageType.Melee, skill_1_name = "", skill_2_name = "", skill_3_name = "" });
        list.characterDatas.Add(new CharacterData { displayName = "궁수", damage = 15f, attackSpeed = 0.8f, attackRange = 4f, moveSpeed = 2f, tierNum = 1, attackType = CharacterAttackType.Bow, damageType = CharacterDamageType.Melee, skill_1_name = "", skill_2_name = "", skill_3_name = "" });
        list.characterDatas.Add(new CharacterData { displayName = "마법사", damage = 15f, attackSpeed = 1f, attackRange = 3f, moveSpeed = 2f, tierNum = 1, attackType = CharacterAttackType.Magic, damageType = CharacterDamageType.Magic, skill_1_name = "", skill_2_name = "", skill_3_name = "" });
        list.characterDatas.Add(new CharacterData { displayName = "광전사", damage = 36f, attackSpeed = 1f, attackRange = 2f, moveSpeed = 2f, tierNum = 1, attackType = CharacterAttackType.Normal, damageType = CharacterDamageType.Melee, skill_1_name = "깡공격력(+20%)", skill_2_name = "", skill_3_name = "" });
        list.characterDatas.Add(new CharacterData { displayName = "창술사", damage = 20f, attackSpeed = 1.2f, attackRange = 3f, moveSpeed = 2f, tierNum = 1, attackType = CharacterAttackType.LongSpear, damageType = CharacterDamageType.Melee, skill_1_name = "다중타격", skill_2_name = "", skill_3_name = "" });
        list.characterDatas.Add(new CharacterData { displayName = "주술사", damage = 10f, attackSpeed = 1.5f, attackRange = 3f, moveSpeed = 2f, tierNum = 1, attackType = CharacterAttackType.Magic, damageType = CharacterDamageType.Magic, skill_1_name = "디버프", skill_2_name = "", skill_3_name = "" });
        list.characterDatas.Add(new CharacterData { displayName = "사제", damage = 10f, attackSpeed = 1.5f, attackRange = 3f, moveSpeed = 2f, tierNum = 1, attackType = CharacterAttackType.Magic, damageType = CharacterDamageType.Magic, skill_1_name = "버프", skill_2_name = "", skill_3_name = "" });
        #endregion
        #region Tier: 안흔한
        //전사 계열
        list.characterDatas.Add(new CharacterData { displayName = "성기사", damage = 35f, attackSpeed = 1f, attackRange = 1f, moveSpeed = 2f, tierNum = 2, attackType = CharacterAttackType.Normal, damageType = CharacterDamageType.Magic, skill_1_name = "", skill_2_name = "", skill_3_name = "" });
        list.characterDatas.Add(new CharacterData { displayName = "마검사", damage = 25f, attackSpeed = 1f, attackRange = 2f, moveSpeed = 2f, tierNum = 2, attackType = CharacterAttackType.Normal, damageType = CharacterDamageType.Magic, skill_1_name = "", skill_2_name = "", skill_3_name = "" });
        //도적 계열
        list.characterDatas.Add(new CharacterData { displayName = "닌자", damage = 20f, attackSpeed = 0.8f, attackRange = 1f, moveSpeed = 3f, tierNum = 2, attackType = CharacterAttackType.Normal, damageType = CharacterDamageType.Melee, skill_1_name = "", skill_2_name = "", skill_3_name = "" });
        list.characterDatas.Add(new CharacterData { displayName = "어쌔신", damage = 40f, attackSpeed = 1f, attackRange = 0.8f, moveSpeed = 2.5f, tierNum = 2, attackType = CharacterAttackType.Normal, damageType = CharacterDamageType.Melee, skill_1_name = "", skill_2_name = "", skill_3_name = "" });
        //궁수 계열
        list.characterDatas.Add(new CharacterData { displayName = "레인저", damage = 20f, attackSpeed = 0.8f, attackRange = 3f, moveSpeed = 2f, tierNum = 2, attackType = CharacterAttackType.Bow, damageType = CharacterDamageType.Melee, skill_1_name = "", skill_2_name = "", skill_3_name = "" });
        list.characterDatas.Add(new CharacterData { displayName = "스나이퍼", damage = 40f, attackSpeed = 1.5f, attackRange = 4f, moveSpeed = 1.5f, tierNum = 2, attackType = CharacterAttackType.Bow, damageType = CharacterDamageType.Melee, skill_1_name = "", skill_2_name = "", skill_3_name = "" });
        //격투가 계열
        list.characterDatas.Add(new CharacterData { displayName = "파이터", damage = 30f, attackSpeed = 0.5f, attackRange = 0.8f, moveSpeed = 2f, tierNum = 2, attackType = CharacterAttackType.Normal, damageType = CharacterDamageType.Melee, skill_1_name = "", skill_2_name = "", skill_3_name = "" });
        //광전사 계열
        list.characterDatas.Add(new CharacterData { displayName = "버서커", damage = 48f, attackSpeed = 1.2f, attackRange = 1f, moveSpeed = 2f, tierNum = 2, attackType = CharacterAttackType.Axe, damageType = CharacterDamageType.Melee, skill_1_name = "깡공격력(+20%)", skill_2_name = "", skill_3_name = "" });
        //창술사 계열
        list.characterDatas.Add(new CharacterData { displayName = "파이크맨", damage = 20f, attackSpeed = 1f, attackRange = 2f, moveSpeed = 2f, tierNum = 2, attackType = CharacterAttackType.LongSpear, damageType = CharacterDamageType.Melee, skill_1_name = "다중타격", skill_2_name = "", skill_3_name = "" });
        list.characterDatas.Add(new CharacterData { displayName = "마창사", damage = 30f, attackSpeed = 1f, attackRange = 2f, moveSpeed = 2f, tierNum = 2, attackType = CharacterAttackType.LongSpear, damageType = CharacterDamageType.Magic, skill_1_name = "", skill_2_name = "", skill_3_name = "" });
        //주술사 계열
        list.characterDatas.Add(new CharacterData { displayName = "흑마술사", damage = 20f, attackSpeed = 1.5f, attackRange = 3f, moveSpeed = 2f, tierNum = 2, attackType = CharacterAttackType.Magic, damageType = CharacterDamageType.Magic, skill_1_name = "디버프", skill_2_name = "", skill_3_name = "" });
        //마법사 계열
        list.characterDatas.Add(new CharacterData { displayName = "소서러", damage = 25f, attackSpeed = 1f, attackRange = 3f, moveSpeed = 2f, tierNum = 2, attackType = CharacterAttackType.Magic, damageType = CharacterDamageType.Magic, skill_1_name = "", skill_2_name = "", skill_3_name = "" });
        //사제 계열
        list.characterDatas.Add(new CharacterData { displayName = "드루이드", damage = 20f, attackSpeed = 1.5f, attackRange = 3f, moveSpeed = 2f, tierNum = 2, attackType = CharacterAttackType.Magic, damageType = CharacterDamageType.Magic, skill_1_name = "버프", skill_2_name = "", skill_3_name = "" });
        list.characterDatas.Add(new CharacterData { displayName = "바드", damage = 20f, attackSpeed = 1.5f, attackRange = 3f, moveSpeed = 2f, tierNum = 2, attackType = CharacterAttackType.Magic, damageType = CharacterDamageType.Magic, skill_1_name = "상태이상", skill_2_name = "", skill_3_name = "" });
        #endregion
        #region Tier: 희귀한
        //전사 계열
        list.characterDatas.Add(new CharacterData { displayName = "룬나이트", damage = 60f, attackSpeed = 1f, attackRange = 4.5f, moveSpeed = 3f, tierNum = 3, attackType = CharacterAttackType.Normal, damageType = CharacterDamageType.Magic, skill_1_name = "", skill_2_name = "", skill_3_name = "" });
        list.characterDatas.Add(new CharacterData { displayName = "워든", damage = 40f, attackSpeed = 1f, attackRange = 3.5f, moveSpeed = 3f, tierNum = 3, attackType = CharacterAttackType.Axe, damageType = CharacterDamageType.Magic, skill_1_name = "상태이상", skill_2_name = "", skill_3_name = "" });
        list.characterDatas.Add(new CharacterData { displayName = "워로드", damage = 80f, attackSpeed = 0.8f, attackRange = 2.5f, moveSpeed = 3.5f, tierNum = 3, attackType = CharacterAttackType.Normal, damageType = CharacterDamageType.Melee, skill_1_name = "", skill_2_name = "", skill_3_name = "" });
        list.characterDatas.Add(new CharacterData { displayName = "크루세이더", damage = 50f, attackSpeed = 1.2f, attackRange = 2.5f, moveSpeed = 3f, tierNum = 3, attackType = CharacterAttackType.Axe, damageType = CharacterDamageType.Magic, skill_1_name = "다중타격", skill_2_name = "", skill_3_name = "" });
        //도적 계열
        list.characterDatas.Add(new CharacterData { displayName = "어벤저", damage = 50f, attackSpeed = 0.5f, attackRange = 1f, moveSpeed = 4f, tierNum = 3, attackType = CharacterAttackType.Normal, damageType = CharacterDamageType.Melee, skill_1_name = "", skill_2_name = "", skill_3_name = "" });
        list.characterDatas.Add(new CharacterData { displayName = "데스리퍼", damage = 40f, attackSpeed = 0.8f, attackRange = 1.5f, moveSpeed = 3f, tierNum = 3, attackType = CharacterAttackType.Normal, damageType = CharacterDamageType.Melee, skill_1_name = "디버프", skill_2_name = "", skill_3_name = "" });
        list.characterDatas.Add(new CharacterData { displayName = "나이트스토커", damage = 104f, attackSpeed = 1.5f, attackRange = 1f, moveSpeed = 4f, tierNum = 3, attackType = CharacterAttackType.Normal, damageType = CharacterDamageType.Melee, skill_1_name = "깡공격력(+30%)", skill_2_name = "", skill_3_name = "" });
        list.characterDatas.Add(new CharacterData { displayName = "사무라이", damage = 60f, attackSpeed = 1f, attackRange = 2.5f, moveSpeed = 3f, tierNum = 3, attackType = CharacterAttackType.Normal, damageType = CharacterDamageType.Melee, skill_1_name = "", skill_2_name = "", skill_3_name = "" });
        //궁수 계열
        list.characterDatas.Add(new CharacterData { displayName = "헌터", damage = 60f, attackSpeed = 1f, attackRange = 3.5f, moveSpeed = 2.5f, tierNum = 3, attackType = CharacterAttackType.Bow, damageType = CharacterDamageType.Melee, skill_1_name = "", skill_2_name = "", skill_3_name = "" });
        list.characterDatas.Add(new CharacterData { displayName = "스카우트", damage = 50f, attackSpeed = 1.2f, attackRange = 4.5f, moveSpeed = 3f, tierNum = 3, attackType = CharacterAttackType.Bow, damageType = CharacterDamageType.Melee, skill_1_name = "상태이상", skill_2_name = "", skill_3_name = "" });
        list.characterDatas.Add(new CharacterData { displayName = "트래퍼", damage = 80f, attackSpeed = 1.5f, attackRange = 2.5f, moveSpeed = 2f, tierNum = 3, attackType = CharacterAttackType.Bow, damageType = CharacterDamageType.Melee, skill_1_name = "다중타격", skill_2_name = "", skill_3_name = "" });
        list.characterDatas.Add(new CharacterData { displayName = "데드아이", damage = 70f, attackSpeed = 1.5f, attackRange = 4.5f, moveSpeed = 2f, tierNum = 3, attackType = CharacterAttackType.Bow, damageType = CharacterDamageType.Magic, skill_1_name = "", skill_2_name = "", skill_3_name = "" });
        //격투가 계열
        list.characterDatas.Add(new CharacterData { displayName = "슬레이어", damage = 60f, attackSpeed = 1f, attackRange = 2.5f, moveSpeed = 3f, tierNum = 3, attackType = CharacterAttackType.Normal, damageType = CharacterDamageType.Melee, skill_1_name = "", skill_2_name = "", skill_3_name = "" });
        list.characterDatas.Add(new CharacterData { displayName = "스트라이커", damage = 40f, attackSpeed = 0.5f, attackRange = 1.5f, moveSpeed = 4f, tierNum = 3, attackType = CharacterAttackType.Normal, damageType = CharacterDamageType.Melee, skill_1_name = "", skill_2_name = "", skill_3_name = "" });
        list.characterDatas.Add(new CharacterData { displayName = "수도사", damage = 80f, attackSpeed = 1f, attackRange = 1f, moveSpeed = 4f, tierNum = 3, attackType = CharacterAttackType.Normal, damageType = CharacterDamageType.Magic, skill_1_name = "", skill_2_name = "", skill_3_name = "" });
        //광전사 계열
        list.characterDatas.Add(new CharacterData { displayName = "워마스터", damage = 60f, attackSpeed = 1f, attackRange = 2.5f, moveSpeed = 3f, tierNum = 3, attackType = CharacterAttackType.Axe, damageType = CharacterDamageType.Melee, skill_1_name = "", skill_2_name = "", skill_3_name = "" });
        list.characterDatas.Add(new CharacterData { displayName = "블러드나이트", damage = 80f, attackSpeed = 1.2f, attackRange = 2.5f, moveSpeed = 3f, tierNum = 3, attackType = CharacterAttackType.Normal, damageType = CharacterDamageType.Melee, skill_1_name = "", skill_2_name = "", skill_3_name = "" });
        list.characterDatas.Add(new CharacterData { displayName = "블레이드댄서", damage = 40f, attackSpeed = 0.5f, attackRange = 1f, moveSpeed = 2.5f, tierNum = 3, attackType = CharacterAttackType.Normal, damageType = CharacterDamageType.Melee, skill_1_name = "상태이상", skill_2_name = "", skill_3_name = "" });
        //창술사 계열
        list.characterDatas.Add(new CharacterData { displayName = "팔랑크스", damage = 60f, attackSpeed = 1.5f, attackRange = 4.5f, moveSpeed = 2f, tierNum = 3, attackType = CharacterAttackType.LongSpear, damageType = CharacterDamageType.Melee, skill_1_name = "다중타격(+1)", skill_2_name = "", skill_3_name = "" });
        list.characterDatas.Add(new CharacterData { displayName = "하이랜서", damage = 80f, attackSpeed = 1.5f, attackRange = 3.5f, moveSpeed = 3f, tierNum = 3, attackType = CharacterAttackType.LongSpear, damageType = CharacterDamageType.Melee, skill_1_name = "다중타격", skill_2_name = "", skill_3_name = "" });
        list.characterDatas.Add(new CharacterData { displayName = "헤이스트랜서", damage = 60f, attackSpeed = 0.8f, attackRange = 3.5f, moveSpeed = 3f, tierNum = 3, attackType = CharacterAttackType.LongSpear, damageType = CharacterDamageType.Magic, skill_1_name = "", skill_2_name = "", skill_3_name = "" });
        list.characterDatas.Add(new CharacterData { displayName = "섀도우랜서", damage = 40f, attackSpeed = 1.2f, attackRange = 3.5f, moveSpeed = 3.5f, tierNum = 3, attackType = CharacterAttackType.LongSpear, damageType = CharacterDamageType.Magic, skill_1_name = "디버프", skill_2_name = "", skill_3_name = "" });
        //주술사 계열
        list.characterDatas.Add(new CharacterData { displayName = "카오스챔피언", damage = 84f, attackSpeed = 1f, attackRange = 2.5f, moveSpeed = 3f, tierNum = 3, attackType = CharacterAttackType.Normal, damageType = CharacterDamageType.Magic, skill_1_name = "깡공격력(+20%)", skill_2_name = "", skill_3_name = "" });
        list.characterDatas.Add(new CharacterData { displayName = "워록", damage = 50f, attackSpeed = 1.2f, attackRange = 3.5f, moveSpeed = 3f, tierNum = 3, attackType = CharacterAttackType.Magic, damageType = CharacterDamageType.Magic, skill_1_name = "디버프", skill_2_name = "", skill_3_name = "" });
        list.characterDatas.Add(new CharacterData { displayName = "둠콜러", damage = 60f, attackSpeed = 1f, attackRange = 3.5f, moveSpeed = 3f, tierNum = 3, attackType = CharacterAttackType.Magic, damageType = CharacterDamageType.Magic, skill_1_name = "다중타격", skill_2_name = "", skill_3_name = "" });
        //마법사 계열
        list.characterDatas.Add(new CharacterData { displayName = "아크메이지", damage = 80f, attackSpeed = 1.2f, attackRange = 4.5f, moveSpeed = 2.5f, tierNum = 3, attackType = CharacterAttackType.Magic, damageType = CharacterDamageType.Magic, skill_1_name = "", skill_2_name = "", skill_3_name = "" });
        list.characterDatas.Add(new CharacterData { displayName = "배틀메이지", damage = 80f, attackSpeed = 0.8f, attackRange = 2.5f, moveSpeed = 3f, tierNum = 3, attackType = CharacterAttackType.Normal, damageType = CharacterDamageType.Magic, skill_1_name = "", skill_2_name = "", skill_3_name = "" });
        list.characterDatas.Add(new CharacterData { displayName = "엘리멘탈리스트", damage = 60f, attackSpeed = 1.5f, attackRange = 3.5f, moveSpeed = 2.5f, tierNum = 3, attackType = CharacterAttackType.Magic, damageType = CharacterDamageType.Magic, skill_1_name = "버프", skill_2_name = "", skill_3_name = "" });
        list.characterDatas.Add(new CharacterData { displayName = "스펠듀얼리스트", damage = 40f, attackSpeed = 0.5f, attackRange = 3.5f, moveSpeed = 2.5f, tierNum = 3, attackType = CharacterAttackType.Magic, damageType = CharacterDamageType.Magic, skill_1_name = "다중타격", skill_2_name = "", skill_3_name = "" });
        //사제 계열
        list.characterDatas.Add(new CharacterData { displayName = "프리스트", damage = 40f, attackSpeed = 1f, attackRange = 3.5f, moveSpeed = 3f, tierNum = 3, attackType = CharacterAttackType.Magic, damageType = CharacterDamageType.Magic, skill_1_name = "버프", skill_2_name = "", skill_3_name = "" });
        list.characterDatas.Add(new CharacterData { displayName = "와일드워든", damage = 96f, attackSpeed = 1.5f, attackRange = 2.5f, moveSpeed = 2f, tierNum = 3, attackType = CharacterAttackType.Magic, damageType = CharacterDamageType.Magic, skill_1_name = "깡공격력(+20%)", skill_2_name = "", skill_3_name = "" });
        list.characterDatas.Add(new CharacterData { displayName = "비숍", damage = 50f, attackSpeed = 1f, attackRange = 3.5f, moveSpeed = 3f, tierNum = 3, attackType = CharacterAttackType.Magic, damageType = CharacterDamageType.Magic, skill_1_name = "상태이상", skill_2_name = "", skill_3_name = "" });
        list.characterDatas.Add(new CharacterData { displayName = "디바인섀도우", damage = 60f, attackSpeed = 1.2f, attackRange = 2.5f, moveSpeed = 3.5f, tierNum = 3, attackType = CharacterAttackType.LongSpear, damageType = CharacterDamageType.Magic, skill_1_name = "다중타격", skill_2_name = "", skill_3_name = "" });
        #endregion
        LoadDataFromJSON(list, "character_data.json");
    }
    #endregion

    #region CharacterRecipeData
    [MenuItem("정재욱/Generate characterRecipe_data")]
    private static void GenerateCharacterRecipeData()
    {
        //JSON 데이터 생성
        CharacterRecipeDataList list = new CharacterRecipeDataList
        {
            characterRecipeDatas = new List<CharacterRecipeData>()
        };

        //데이터 추가
        list.characterRecipeDatas.AddRange(new List<CharacterRecipeData>
        {
            #region Tier: 흔한
            new CharacterRecipeData { selectName = "전사", recipeNameA = "사제", recipeNameB = "", recipeNameC = "", resultName = "성기사" },
            new CharacterRecipeData { selectName = "전사", recipeNameA = "마법사", recipeNameB = "", recipeNameC = "", resultName = "마검사" },
            new CharacterRecipeData { selectName = "도적", recipeNameA = "격투가", recipeNameB = "", recipeNameC = "", resultName = "닌자" },
            new CharacterRecipeData { selectName = "도적", recipeNameA = "도적", recipeNameB = "", recipeNameC = "", resultName = "어쌔신" },
            new CharacterRecipeData { selectName = "궁수", recipeNameA = "도적", recipeNameB = "", recipeNameC = "", resultName = "레인저" },
            new CharacterRecipeData { selectName = "궁수", recipeNameA = "궁수", recipeNameB = "", recipeNameC = "", resultName = "스나이퍼" },
            new CharacterRecipeData { selectName = "격투가", recipeNameA = "광전사", recipeNameB = "", recipeNameC = "", resultName = "파이터" },
            new CharacterRecipeData { selectName = "광전사", recipeNameA = "광전사", recipeNameB = "", recipeNameC = "", resultName = "버서커" },
            new CharacterRecipeData { selectName = "창술사", recipeNameA = "창술사", recipeNameB = "", recipeNameC = "", resultName = "파이크맨" },
            new CharacterRecipeData { selectName = "창술사", recipeNameA = "마법사", recipeNameB = "", recipeNameC = "", resultName = "마창사" },
            new CharacterRecipeData { selectName = "주술사", recipeNameA = "주술사", recipeNameB = "", recipeNameC = "", resultName = "흑마술사" },
            new CharacterRecipeData { selectName = "마법사", recipeNameA = "마법사", recipeNameB = "", recipeNameC = "", resultName = "소서러" },
            new CharacterRecipeData { selectName = "사제", recipeNameA = "주술사", recipeNameB = "", recipeNameC = "", resultName = "드루이드" },
            new CharacterRecipeData { selectName = "사제", recipeNameA = "궁수", recipeNameB = "", recipeNameC = "", resultName = "바드" },
            #endregion
            #region Tier: 안흔한
            new CharacterRecipeData { selectName = "마검사", recipeNameA = "소서러", recipeNameB = "궁수", recipeNameC = "", resultName = "룬나이트" },
            new CharacterRecipeData { selectName = "마검사", recipeNameA = "드루이드", recipeNameB = "주술사", recipeNameC = "", resultName = "워든" },
            new CharacterRecipeData { selectName = "성기사", recipeNameA = "버서커", recipeNameB = "격투가", recipeNameC = "", resultName = "워로드" },
            new CharacterRecipeData { selectName = "성기사", recipeNameA = "마창사", recipeNameB = "사제", recipeNameC = "", resultName = "크루세이더" },
            new CharacterRecipeData { selectName = "어쌔신", recipeNameA = "파이터", recipeNameB = "광전사", recipeNameC = "", resultName = "어벤저" },
            new CharacterRecipeData { selectName = "어쌔신", recipeNameA = "파이크맨", recipeNameB = "격투가", recipeNameC = "", resultName = "데스리퍼" },
            new CharacterRecipeData { selectName = "닌자", recipeNameA = "스나이퍼", recipeNameB = "도적", recipeNameC = "", resultName = "나이트스토커" },
            new CharacterRecipeData { selectName = "닌자", recipeNameA = "닌자", recipeNameB = "전사", recipeNameC = "", resultName = "사무라이" },
            new CharacterRecipeData { selectName = "레인저", recipeNameA = "스나이퍼", recipeNameB = "궁수", recipeNameC = "", resultName = "헌터" },
            new CharacterRecipeData { selectName = "레인저", recipeNameA = "바드", recipeNameB = "격투가", recipeNameC = "", resultName = "스카우트" },
            new CharacterRecipeData { selectName = "스나이퍼", recipeNameA = "파이크맨", recipeNameB = "마법사", recipeNameC = "", resultName = "트래퍼" },
            new CharacterRecipeData { selectName = "스나이퍼", recipeNameA = "흑마술사", recipeNameB = "궁수", recipeNameC = "", resultName = "데드아이" },
            new CharacterRecipeData { selectName = "파이터", recipeNameA = "버서커", recipeNameB = "광전사", recipeNameC = "", resultName = "슬레이어" },
            new CharacterRecipeData { selectName = "파이터", recipeNameA = "닌자", recipeNameB = "광전사", recipeNameC = "", resultName = "스트라이커" },
            new CharacterRecipeData { selectName = "파이터", recipeNameA = "드루이드", recipeNameB = "격투가", recipeNameC = "", resultName = "수도사" },
            new CharacterRecipeData { selectName = "버서커", recipeNameA = "버서커", recipeNameB = "전사", recipeNameC = "", resultName = "워마스터" },
            new CharacterRecipeData { selectName = "버서커", recipeNameA = "마검사", recipeNameB = "주술사", recipeNameC = "", resultName = "블러드나이트" },
            new CharacterRecipeData { selectName = "버서커", recipeNameA = "바드", recipeNameB = "도적", recipeNameC = "", resultName = "블레이드댄서" },
            new CharacterRecipeData { selectName = "파이크맨", recipeNameA = "성기사", recipeNameB = "창술사", recipeNameC = "", resultName = "팔랑크스" },
            new CharacterRecipeData { selectName = "파이크맨", recipeNameA = "파이크맨", recipeNameB = "전사", recipeNameC = "", resultName = "하이랜서" },
            new CharacterRecipeData { selectName = "마창사", recipeNameA = "바드", recipeNameB = "사제", recipeNameC = "", resultName = "헤이스트랜서" },
            new CharacterRecipeData { selectName = "마창사", recipeNameA = "어쌔신", recipeNameB = "마법사", recipeNameC = "", resultName = "섀도우랜서" },
            new CharacterRecipeData { selectName = "흑마술사", recipeNameA = "마검사", recipeNameB = "마법사", recipeNameC = "", resultName = "카오스챔피언" },
            new CharacterRecipeData { selectName = "흑마술사", recipeNameA = "어쌔신", recipeNameB = "마법사", recipeNameC = "", resultName = "워록" },
            new CharacterRecipeData { selectName = "흑마술사", recipeNameA = "마창사", recipeNameB = "사제", recipeNameC = "", resultName = "둠콜러" },
            new CharacterRecipeData { selectName = "소서러", recipeNameA = "소서러", recipeNameB = "주술사", recipeNameC = "", resultName = "아크메이지" },
            new CharacterRecipeData { selectName = "소서러", recipeNameA = "파이터", recipeNameB = "전사", recipeNameC = "", resultName = "배틀메이지" },
            new CharacterRecipeData { selectName = "소서러", recipeNameA = "드루이드", recipeNameB = "사제", recipeNameC = "", resultName = "엘리멘탈리스트" },
            new CharacterRecipeData { selectName = "소서러", recipeNameA = "레인저", recipeNameB = "창술사", recipeNameC = "", resultName = "스펠듀얼리스트" },
            new CharacterRecipeData { selectName = "드루이드", recipeNameA = "성기사", recipeNameB = "광전사", recipeNameC = "", resultName = "프리스트" },
            new CharacterRecipeData { selectName = "드루이드", recipeNameA = "레인저", recipeNameB = "도적", recipeNameC = "", resultName = "와일드워든" },
            new CharacterRecipeData { selectName = "바드", recipeNameA = "흑마술사", recipeNameB = "궁수", recipeNameC = "", resultName = "비숍" },
            new CharacterRecipeData { selectName = "바드", recipeNameA = "닌자", recipeNameB = "창술사", recipeNameC = "", resultName = "디바인섀도우" },
            #endregion
            //#region Tier: 희귀한
            //#endregion
        });

        LoadDataFromJSON(list, "characterRecipe_data.json");
    }
    #endregion

    #region CharacterSkillData
    [MenuItem("정재욱/Generate characterSkill_data")]
    private static void GenerateCharacterSkillData()
    {
        //JSON 데이터 생성
        CharacterSkillDataList list = new CharacterSkillDataList
        {
            characterSkillDatas = new List<CharacterSkillData>()
        };

        //데이터 추가
        list.characterSkillDatas.Add(new CharacterSkillData
        {
            skillName = "화살 날리기",
            characterDisplayName = "전사",
            skillDescription = "화살을 날려서 {0}데미지를 입힙니다.",
            skillImagePath = "image/skill/전사/bow",
            skillDamage = "{0} * 5",
            skillTriggerChance = 10f,
            skillEffect = "",
            skillBasic = true
        });
        list.characterSkillDatas.Add(new CharacterSkillData
        {
            skillName = "불화살 날리기",
            characterDisplayName = "전사",
            skillDescription = "불화살을 날려서 {0}데미지를 입힙니다.",
            skillImagePath = "image/skill/전사/fireBow",
            skillDamage = "{0} * 10",
            skillTriggerChance = 5f,
            skillEffect = "추가적으로 5초간 스킬 데미지의 200%만큼 지속 피해를 입힙니다.",
            skillBasic = false
        });

        LoadDataFromJSON(list, "characterSkill_data.json");
    }
    #endregion

    #region PlayWaveData
    [MenuItem("정재욱/Generate playWave_data")]
    private static void GeneratePlayWaveData()
    {
        //JSON 데이터 생성
        PlayWaveDataList list = new PlayWaveDataList
        {
            playWaveDatas = new List<PlayWaveData>()
        };

        //데이터 추가
        List<PlayWaveData> playWaveDatas = new List<PlayWaveData> {
            new PlayWaveData { wave = 1, waveEnemyCount = 35, waveTimer = 20, enemyHp = 112, enemyDefense = 5.3f, enemySpeed = 3f, enemyType = EnemyType.일반, characterDrawCount = 5 },
            new PlayWaveData { wave = 2, waveEnemyCount = 35, waveTimer = 40, enemyHp = 125, enemyDefense = 5.6f, enemySpeed = 3f, enemyType = EnemyType.일반, characterDrawCount = 2 },
            new PlayWaveData { wave = 3, waveEnemyCount = 35, waveTimer = 40, enemyHp = 140, enemyDefense = 6.5f, enemySpeed = 5.5f, enemyType = EnemyType.속도형, characterDrawCount = 2 },
            new PlayWaveData { wave = 4, waveEnemyCount = 35, waveTimer = 40, enemyHp = 157, enemyDefense = 6.3f, enemySpeed = 3f, enemyType = EnemyType.일반, characterDrawCount = 2 },
            new PlayWaveData { wave = 5, waveEnemyCount = 35, waveTimer = 40, enemyHp = 176, enemyDefense = 6.7f, enemySpeed = 3f, enemyType = EnemyType.일반, characterDrawCount = 2 },
            new PlayWaveData { wave = 6, waveEnemyCount = 35, waveTimer = 40, enemyHp = 212, enemyDefense = 11.6f, enemySpeed = 1.5f, enemyType = EnemyType.방어형, characterDrawCount = 2 },
            new PlayWaveData { wave = 7, waveEnemyCount = 35, waveTimer = 40, enemyHp = 221, enemyDefense = 7.5f, enemySpeed = 3f, enemyType = EnemyType.일반, characterDrawCount = 2 },
            new PlayWaveData { wave = 8, waveEnemyCount = 35, waveTimer = 40, enemyHp = 247, enemyDefense = 8.0f, enemySpeed = 3f, enemyType = EnemyType.일반, characterDrawCount = 2 },
            new PlayWaveData { wave = 9, waveEnemyCount = 35, waveTimer = 40, enemyHp = 277, enemyDefense = 8.4f, enemySpeed = 3f, enemyType = EnemyType.일반, characterDrawCount = 2 },
            new PlayWaveData { wave = 10, waveEnemyCount = 1, waveTimer = 40, enemyHp = 930, enemyDefense = 18.0f, enemySpeed = 2.25f, enemyType = EnemyType.보스, characterDrawCount = 2 },
            new PlayWaveData { wave = 11, waveEnemyCount = 35, waveTimer = 40, enemyHp = 347, enemyDefense = 9.5f, enemySpeed = 3f, enemyType = EnemyType.일반, characterDrawCount = 2 },
            new PlayWaveData { wave = 12, waveEnemyCount = 35, waveTimer = 40, enemyHp = 389, enemyDefense = 10.1f, enemySpeed = 3f, enemyType = EnemyType.일반, characterDrawCount = 2 },
            new PlayWaveData { wave = 13, waveEnemyCount = 35, waveTimer = 40, enemyHp = 408, enemyDefense = 10.0f, enemySpeed = 5.5f, enemyType = EnemyType.속도형, characterDrawCount = 2 },
            new PlayWaveData { wave = 14, waveEnemyCount = 35, waveTimer = 40, enemyHp = 488, enemyDefense = 11.3f, enemySpeed = 3f, enemyType = EnemyType.일반, characterDrawCount = 2 },
            new PlayWaveData { wave = 15, waveEnemyCount = 35, waveTimer = 40, enemyHp = 547, enemyDefense = 12.0f, enemySpeed = 3f, enemyType = EnemyType.일반, characterDrawCount = 2 },
            new PlayWaveData { wave = 16, waveEnemyCount = 35, waveTimer = 40, enemyHp = 1002, enemyDefense = 12.9f, enemySpeed = 1.5f, enemyType = EnemyType.체력형, characterDrawCount = 2 },
            new PlayWaveData { wave = 17, waveEnemyCount = 35, waveTimer = 40, enemyHp = 686, enemyDefense = 13.5f, enemySpeed = 3f, enemyType = EnemyType.일반, characterDrawCount = 2 },
            new PlayWaveData { wave = 18, waveEnemyCount = 35, waveTimer = 40, enemyHp = 768, enemyDefense = 14.3f, enemySpeed = 3f, enemyType = EnemyType.일반, characterDrawCount = 2 },
            new PlayWaveData { wave = 19, waveEnemyCount = 35, waveTimer = 40, enemyHp = 861, enemyDefense = 15.1f, enemySpeed = 3f, enemyType = EnemyType.일반, characterDrawCount = 2 },
            new PlayWaveData { wave = 20, waveEnemyCount = 1, waveTimer = 40, enemyHp = 2892, enemyDefense = 32.0f, enemySpeed = 2.25f, enemyType = EnemyType.보스, characterDrawCount = 2 },
            new PlayWaveData { wave = 21, waveEnemyCount = 35, waveTimer = 40, enemyHp = 1080, enemyDefense = 17.0f, enemySpeed = 3f, enemyType = EnemyType.일반, characterDrawCount = 2 },
            new PlayWaveData { wave = 22, waveEnemyCount = 35, waveTimer = 40, enemyHp = 1210, enemyDefense = 18.0f, enemySpeed = 3f, enemyType = EnemyType.일반, characterDrawCount = 2 },
            new PlayWaveData { wave = 23, waveEnemyCount = 35, waveTimer = 40, enemyHp = 1477, enemyDefense = 25.0f, enemySpeed = 1.5f, enemyType = EnemyType.방어형, characterDrawCount = 2 },
            new PlayWaveData { wave = 24, waveEnemyCount = 35, waveTimer = 40, enemyHp = 1517, enemyDefense = 20.2f, enemySpeed = 3f, enemyType = EnemyType.일반, characterDrawCount = 2 },
            new PlayWaveData { wave = 25, waveEnemyCount = 35, waveTimer = 40, enemyHp = 1700, enemyDefense = 21.5f, enemySpeed = 3f, enemyType = EnemyType.일반, characterDrawCount = 2 },
            new PlayWaveData { wave = 26, waveEnemyCount = 35, waveTimer = 40, enemyHp = 1830, enemyDefense = 30.4f, enemySpeed = 1.5f, enemyType = EnemyType.방어형, characterDrawCount = 2 },
            new PlayWaveData { wave = 27, waveEnemyCount = 35, waveTimer = 40, enemyHp = 2132, enemyDefense = 24.1f, enemySpeed = 3f, enemyType = EnemyType.일반, characterDrawCount = 2 },
            new PlayWaveData { wave = 28, waveEnemyCount = 35, waveTimer = 40, enemyHp = 2388, enemyDefense = 25.6f, enemySpeed = 3f, enemyType = EnemyType.일반, characterDrawCount = 2 },
            new PlayWaveData { wave = 29, waveEnemyCount = 35, waveTimer = 40, enemyHp = 2674, enemyDefense = 27.1f, enemySpeed = 3f, enemyType = EnemyType.일반, characterDrawCount = 2 },
            new PlayWaveData { wave = 30, waveEnemyCount = 1, waveTimer = 40, enemyHp = 8985, enemyDefense = 57.4f, enemySpeed = 2.25f, enemyType = EnemyType.보스, characterDrawCount = 2 },
            new PlayWaveData { wave = 31, waveEnemyCount = 35, waveTimer = 40, enemyHp = 3355, enemyDefense = 30.4f, enemySpeed = 3f, enemyType = EnemyType.일반, characterDrawCount = 2 },
            new PlayWaveData { wave = 32, waveEnemyCount = 35, waveTimer = 40, enemyHp = 3758, enemyDefense = 32.3f, enemySpeed = 3f, enemyType = EnemyType.일반, characterDrawCount = 2 },
            new PlayWaveData { wave = 33, waveEnemyCount = 35, waveTimer = 40, enemyHp = 4391, enemyDefense = 52.6f, enemySpeed = 1.5f, enemyType = EnemyType.방어형, characterDrawCount = 2 },
            new PlayWaveData { wave = 34, waveEnemyCount = 35, waveTimer = 40, enemyHp = 4714, enemyDefense = 36.3f, enemySpeed = 3f, enemyType = EnemyType.일반, characterDrawCount = 2 },
            new PlayWaveData { wave = 35, waveEnemyCount = 35, waveTimer = 40, enemyHp = 5279, enemyDefense = 38.4f, enemySpeed = 3f, enemyType = EnemyType.일반, characterDrawCount = 2 },
            new PlayWaveData { wave = 36, waveEnemyCount = 35, waveTimer = 40, enemyHp = 6095, enemyDefense = 62.4f, enemySpeed = 1.5f, enemyType = EnemyType.방어형, characterDrawCount = 2 },
            new PlayWaveData { wave = 37, waveEnemyCount = 35, waveTimer = 40, enemyHp = 6623, enemyDefense = 43.2f, enemySpeed = 3f, enemyType = EnemyType.일반, characterDrawCount = 2 },
            new PlayWaveData { wave = 38, waveEnemyCount = 35, waveTimer = 40, enemyHp = 7417, enemyDefense = 45.8f, enemySpeed = 3f, enemyType = EnemyType.일반, characterDrawCount = 2 },
            new PlayWaveData { wave = 39, waveEnemyCount = 35, waveTimer = 40, enemyHp = 8308, enemyDefense = 48.5f, enemySpeed = 3f, enemyType = EnemyType.일반, characterDrawCount = 2 },
            new PlayWaveData { wave = 40, waveEnemyCount = 1, waveTimer = 40, enemyHp = 27915, enemyDefense = 102.8f, enemySpeed = 2.25f, enemyType = EnemyType.보스, characterDrawCount = 2 },
            new PlayWaveData { wave = 41, waveEnemyCount = 35, waveTimer = 40, enemyHp = 10421, enemyDefense = 54.5f, enemySpeed = 3f, enemyType = EnemyType.일반, characterDrawCount = 2 },
            new PlayWaveData { wave = 42, waveEnemyCount = 35, waveTimer = 40, enemyHp = 11672, enemyDefense = 57.8f, enemySpeed = 3f, enemyType = EnemyType.일반, characterDrawCount = 2 },
            new PlayWaveData { wave = 43, waveEnemyCount = 35, waveTimer = 40, enemyHp = 17561, enemyDefense = 58.3f, enemySpeed = 1.5f, enemyType = EnemyType.체력형, characterDrawCount = 2 },
            new PlayWaveData { wave = 44, waveEnemyCount = 35, waveTimer = 40, enemyHp = 14641, enemyDefense = 64.9f, enemySpeed = 3f, enemyType = EnemyType.일반, characterDrawCount = 2 },
            new PlayWaveData { wave = 45, waveEnemyCount = 35, waveTimer = 40, enemyHp = 16398, enemyDefense = 68.8f, enemySpeed = 3f, enemyType = EnemyType.일반, characterDrawCount = 2 },
            new PlayWaveData { wave = 46, waveEnemyCount = 35, waveTimer = 40, enemyHp = 17114, enemyDefense = 68.0f, enemySpeed = 5.5f, enemyType = EnemyType.속도형, characterDrawCount = 2 },
            new PlayWaveData { wave = 47, waveEnemyCount = 35, waveTimer = 40, enemyHp = 20570, enemyDefense = 77.3f, enemySpeed = 3f, enemyType = EnemyType.일반, characterDrawCount = 2 },
            new PlayWaveData { wave = 48, waveEnemyCount = 35, waveTimer = 40, enemyHp = 23039, enemyDefense = 82.0f, enemySpeed = 3f, enemyType = EnemyType.일반, characterDrawCount = 2 },
            new PlayWaveData { wave = 49, waveEnemyCount = 35, waveTimer = 40, enemyHp = 25803, enemyDefense = 86.9f, enemySpeed = 3f, enemyType = EnemyType.일반, characterDrawCount = 2 },
            new PlayWaveData { wave = 50, waveEnemyCount = 1, waveTimer = 40, enemyHp = 86700, enemyDefense = 184.2f, enemySpeed = 2.25f, enemyType = EnemyType.보스, characterDrawCount = 2 },
            new PlayWaveData { wave = 51, waveEnemyCount = 35, waveTimer = 40, enemyHp = 32368, enemyDefense = 97.6f, enemySpeed = 3f, enemyType = EnemyType.일반, characterDrawCount = 2 },
            new PlayWaveData { wave = 52, waveEnemyCount = 35, waveTimer = 40, enemyHp = 36252, enemyDefense = 103.5f, enemySpeed = 3f, enemyType = EnemyType.일반, characterDrawCount = 2 },
            new PlayWaveData { wave = 53, waveEnemyCount = 35, waveTimer = 40, enemyHp = 42916, enemyDefense = 100.0f, enemySpeed = 5.5f, enemyType = EnemyType.속도형, characterDrawCount = 2 },
            new PlayWaveData { wave = 54, waveEnemyCount = 35, waveTimer = 40, enemyHp = 45475, enemyDefense = 116.3f, enemySpeed = 3f, enemyType = EnemyType.일반, characterDrawCount = 2 },
            new PlayWaveData { wave = 55, waveEnemyCount = 35, waveTimer = 40, enemyHp = 50932, enemyDefense = 123.3f, enemySpeed = 3f, enemyType = EnemyType.일반, characterDrawCount = 2 },
            new PlayWaveData { wave = 56, waveEnemyCount = 35, waveTimer = 40, enemyHp = 52715, enemyDefense = 183.1f, enemySpeed = 1.5f, enemyType = EnemyType.방어형, characterDrawCount = 2 },
            new PlayWaveData { wave = 57, waveEnemyCount = 35, waveTimer = 40, enemyHp = 63889, enemyDefense = 138.5f, enemySpeed = 3f, enemyType = EnemyType.일반, characterDrawCount = 2 },
            new PlayWaveData { wave = 58, waveEnemyCount = 35, waveTimer = 40, enemyHp = 71555, enemyDefense = 146.8f, enemySpeed = 3f, enemyType = EnemyType.일반, characterDrawCount = 2 },
            new PlayWaveData { wave = 59, waveEnemyCount = 35, waveTimer = 40, enemyHp = 80142, enemyDefense = 155.6f, enemySpeed = 3f, enemyType = EnemyType.일반, characterDrawCount = 2 },
            new PlayWaveData { wave = 60, waveEnemyCount = 1, waveTimer = 40, enemyHp = 269277, enemyDefense = 329.8f, enemySpeed = 2.25f, enemyType = EnemyType.보스, characterDrawCount = 2 }
        };

        list.playWaveDatas = playWaveDatas;

        LoadDataFromJSON(list, "playWave_data.json");
    }
    #endregion
}
#endif