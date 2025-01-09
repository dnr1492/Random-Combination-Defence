using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeList : MonoBehaviour
{
    [SerializeField] PlayerGenerator playerGenerator;
    [SerializeField] PlayerClickController playerClickController;
    [SerializeField] List<Image> imgCharacters = new List<Image>();  //Result 포함
    private Color highlightsColor = new Color(1, 1, 1, 1);
    private Color noneColor = new Color(1, 1, 1, 0.2f);
    private Button btnCombine;

    private void Awake()
    {
        for (int i = 0; i < imgCharacters.Count; i++) imgCharacters[i].gameObject.SetActive(false);

        btnCombine = imgCharacters[imgCharacters.Count - 1].GetComponent<Button>();
        btnCombine.interactable = false;
        btnCombine.onClick.AddListener(() => {
            playerClickController.CancelObjects();
            string resultName = btnCombine.GetComponent<Image>().sprite.name;
            playerGenerator.Combine(imgCharacters, resultName);
        });
    }

    /// <summary>
    /// 조합 레시피에 알맞게 이미지 할당
    /// </summary>
    /// <param name="characterSprites"></param>
    public void SetReferenceRecipe(List<Sprite> characterSprites)
    {
        for (int i = 0; i < imgCharacters.Count; i++)
        {
            //Result의 경우
            if (i == imgCharacters.Count - 1)
            {
                imgCharacters[i].sprite = characterSprites[characterSprites.Count - 1];
                imgCharacters[i].gameObject.SetActive(true);
            }
            //그 외
            else
            {
                if (i + 1 >= characterSprites.Count)
                {
                    imgCharacters[i].sprite = null;
                    imgCharacters[i].gameObject.SetActive(false);
                    continue;
                }
                imgCharacters[i].sprite = characterSprites[i];
                imgCharacters[i].gameObject.SetActive(true);
            }
        }
    }

    /// <summary>
    /// 조합 레시피의 재료가 존재할 경우 하이라이트
    /// </summary>
    /// <param name="dicExistingPlayerCount">조합 레시피에 필요한 재료의 개수만큼 존재하는지 확인하기 위한 현재 존재하는 플레이어 개수</param>
    /// <param name="dicRequierdRecipeCount">조합 레시피에 필요한 재료가 모두 존재하는지 확인하기 위한 조합 레시피에서 요구하는 재료 개수</param>
    public void SetReferenceRecipeHighlights(Dictionary<string, int> dicExistingPlayerCount, Dictionary<string, int> dicRequierdRecipeCount)
    {
        Image characterImg;
        string spriteName;
        var dicTemp = new Dictionary<string, int>(dicExistingPlayerCount);

        for (int i = 0; i < imgCharacters.Count; i++)
        {
            characterImg = imgCharacters[i];
            if (characterImg.sprite == null) continue;
            spriteName = characterImg.sprite.name;

            if (dicTemp.ContainsKey(spriteName) && dicTemp[spriteName] > 0) {
                characterImg.color = highlightsColor;
                dicTemp[spriteName]--;
            }
            else characterImg.color = noneColor;
        }

        SetCombinationButton(dicExistingPlayerCount, dicRequierdRecipeCount);
    }

    /// <summary>
    /// 조합 레시피의 재료가 모두 존재할 경우 조합 버튼 활성화
    /// </summary>
    /// <param name="dicExistingPlayerCount">조합 레시피에 필요한 재료의 개수만큼 존재하는지 확인하기 위한 현재 존재하는 플레이어 개수</param>
    /// <param name="dicRequierdRecipeCount">조합 레시피에 필요한 재료가 모두 존재하는지 확인하기 위한 조합 레시피에서 요구하는 재료 개수</param>
    private void SetCombinationButton(Dictionary<string, int> dicExistingPlayerCount, Dictionary<string, int> dicRequierdRecipeCount)
    {
        foreach (var recipe in dicRequierdRecipeCount) {
            dicExistingPlayerCount.TryGetValue(recipe.Key, out int value);
            if (recipe.Value > value) {
                btnCombine.interactable = false;
                btnCombine.GetComponent<Image>().color = noneColor;
                return;
            }
        }

        btnCombine.interactable = true;
        btnCombine.GetComponent<Image>().color = highlightsColor;
    }
}