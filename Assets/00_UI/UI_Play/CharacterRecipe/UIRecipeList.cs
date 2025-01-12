using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRecipeList : MonoBehaviour
{
    [SerializeField] CharacterGenerator characterGenerator;
    [SerializeField] CharacterClickController characterClickController;
    [SerializeField] UIPlay uiPlay;
    [SerializeField] List<Image> imgCharacters = new List<Image>();  //Result ����
    private Color highlightsColor = new Color(1, 1, 1, 1);
    private Color noneColor = new Color(1, 1, 1, 0.2f);
    private Button btnCombine;

    private void Awake()
    {
        for (int i = 0; i < imgCharacters.Count; i++) imgCharacters[i].gameObject.SetActive(false);

        btnCombine = imgCharacters[imgCharacters.Count - 1].GetComponent<Button>();
        btnCombine.interactable = false;
        btnCombine.onClick.AddListener(() => {
            characterClickController.CancelObjects();
            string resultName = btnCombine.GetComponent<Image>().sprite.name;
            characterGenerator.Combine(imgCharacters, resultName);
            uiPlay.SetUI_Population(false);
        });
    }

    /// <summary>
    /// ���� �����ǿ� �˸°� �̹��� �Ҵ�
    /// </summary>
    /// <param name="characterSprites"></param>
    public void SetReferenceRecipe(List<Sprite> characterSprites)
    {
        for (int i = 0; i < imgCharacters.Count; i++)
        {
            //Result�� ���
            if (i == imgCharacters.Count - 1)
            {
                imgCharacters[i].sprite = characterSprites[characterSprites.Count - 1];
                imgCharacters[i].gameObject.SetActive(true);
            }
            //�� ��
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
    /// ���� �������� ��ᰡ ������ ��� ���̶���Ʈ
    /// </summary>
    /// <param name="dicExistingCharacterCount">���� �����ǿ� �ʿ��� ����� ������ŭ �����ϴ��� Ȯ���ϱ� ���� ���� �����ϴ� ĳ���� ����</param>
    /// <param name="dicRequierdRecipeCount">���� �����ǿ� �ʿ��� ��ᰡ ��� �����ϴ��� Ȯ���ϱ� ���� ���� �����ǿ��� �䱸�ϴ� ��� ����</param>
    public void SetReferenceRecipeHighlights(Dictionary<string, int> dicExistingCharacterCount, Dictionary<string, int> dicRequierdRecipeCount)
    {
        Image characterImg;
        string spriteName;
        var dicTemp = new Dictionary<string, int>(dicExistingCharacterCount);

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

        SetCombinationButton(dicExistingCharacterCount, dicRequierdRecipeCount);
    }

    /// <summary>
    /// ���� �������� ��ᰡ ��� ������ ��� ���� ��ư Ȱ��ȭ
    /// </summary>
    /// <param name="dicExistingCharacterCount">���� �����ǿ� �ʿ��� ����� ������ŭ �����ϴ��� Ȯ���ϱ� ���� ���� �����ϴ� ĳ���� ����</param>
    /// <param name="dicRequierdRecipeCount">���� �����ǿ� �ʿ��� ��ᰡ ��� �����ϴ��� Ȯ���ϱ� ���� ���� �����ǿ��� �䱸�ϴ� ��� ����</param>
    private void SetCombinationButton(Dictionary<string, int> dicExistingCharacterCount, Dictionary<string, int> dicRequierdRecipeCount)
    {
        foreach (var recipe in dicRequierdRecipeCount) {
            dicExistingCharacterCount.TryGetValue(recipe.Key, out int value);
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