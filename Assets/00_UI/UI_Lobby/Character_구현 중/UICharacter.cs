using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICharacter : MonoBehaviour
{
    [SerializeField] List<UICharacterCard> uiCharacterCards = new List<UICharacterCard>();
    [SerializeField] Transform content;

    private void OnEnable()
    {
        StartCoroutine(Delay());
    }

    public void DisplayCharacters(Dictionary<string, int[]> dicCombineCharacterDatas)
    {
        foreach (var data in dicCombineCharacterDatas)
        {
            for (int i = 0; i < uiCharacterCards.Count; i++)
            {
                if (data.Key == uiCharacterCards[i].GetName()) uiCharacterCards[i].Set(data.Value[0], data.Value[1], DataManager.instance.GetCharacterCardLevelQuentityData(data.Value[0], data.Value[2]));
            }
        }
    }

    private IEnumerator Delay()
    {
        yield return Time.deltaTime;
        PlayFabManager.instance.DisplayCharacterCard(this);
    }
}