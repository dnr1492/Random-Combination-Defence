using System.Collections.Generic;
using UnityEngine;

public class UICharacterRecipe : MonoBehaviour
{
    private UIRecipeList[] arrRecipeList;

    private void Awake()
    {
        arrRecipeList = GetComponentsInChildren<UIRecipeList>();
    }

    public void SetReferenceRecipe(string displayName)
    {
        for (int i = 0; i < arrRecipeList.Length; i++) {
            arrRecipeList[i].gameObject.SetActive(false);
        }

        var characterRecipeData = DataManager.GetInstance().GetCharacterRecipeData();
        if (!characterRecipeData.ContainsKey(displayName)) {
            DebugLogger.Log(displayName + "�� ���չ��� �������� �ʽ��ϴ�.");
            return;
        }

        for (int i = 0; i < characterRecipeData[displayName].Count; i++)
        {   
            List<Sprite> characterSprites = new List<Sprite>();
            var recipe = characterRecipeData[displayName][i];
            var spriteType = SpriteManager.SpriteType.ImgCharacter;

            if (!string.IsNullOrEmpty(recipe.selectName)) characterSprites.Add(SpriteManager.GetInstance().GetSprite(spriteType, recipe.selectName));
            if (!string.IsNullOrEmpty(recipe.recipeNameA)) characterSprites.Add(SpriteManager.GetInstance().GetSprite(spriteType, recipe.recipeNameA));
            if (!string.IsNullOrEmpty(recipe.recipeNameB)) characterSprites.Add(SpriteManager.GetInstance().GetSprite(spriteType, recipe.recipeNameB));
            if (!string.IsNullOrEmpty(recipe.recipeNameC)) characterSprites.Add(SpriteManager.GetInstance().GetSprite(spriteType, recipe.recipeNameC));
            if (!string.IsNullOrEmpty(recipe.resultName)) characterSprites.Add(SpriteManager.GetInstance().GetSprite(spriteType, recipe.resultName));

            arrRecipeList[i].gameObject.SetActive(true);
            arrRecipeList[i].SetReferenceRecipe(characterSprites);
        }

        SetReferenceRecipeHighlights(characterRecipeData, displayName);
    }

    private void SetReferenceRecipeHighlights(Dictionary<string, List<CharacterRecipeData>> characterRecipeData, string displayName)
    {
        var recipeCount = characterRecipeData[displayName].Count;
        var existingCharacters = CharacterGenerator.ExistingCharacters;

        for (int i = 0; i < recipeCount; i++)
        {
            var recipe = characterRecipeData[displayName][i];

            //���� �����ϴ� ĳ���� ����
            var dicExistingCharacterCount = new Dictionary<string, int>();
            foreach (var character in existingCharacters)
            {
                //���� �����ǿ� �ߺ��Ǵ� ��ᰡ �����ϹǷ� ���� ��ø ���� (if - else if ...)
                if (character.name == recipe.selectName) {
                    if (!dicExistingCharacterCount.ContainsKey(character.name)) dicExistingCharacterCount.Add(character.name, 1);
                    else dicExistingCharacterCount[character.name]++;
                }
                else if (character.name == recipe.recipeNameA) {
                    if (!dicExistingCharacterCount.ContainsKey(character.name)) dicExistingCharacterCount.Add(character.name, 1);
                    else dicExistingCharacterCount[character.name]++;
                }
                else if (character.name == recipe.recipeNameB) {
                    if (!dicExistingCharacterCount.ContainsKey(character.name)) dicExistingCharacterCount.Add(character.name, 1);
                    else dicExistingCharacterCount[character.name]++;
                }
                else if (character.name == recipe.recipeNameC) {
                    if (!dicExistingCharacterCount.ContainsKey(character.name)) dicExistingCharacterCount.Add(character.name, 1);
                    else dicExistingCharacterCount[character.name]++;
                }
            }

            //���� �����ǿ��� �䱸�ϴ� ��� ����
            var dicRequierdRecipeCount = new Dictionary<string, int>();
            foreach (PlayFabManager.CharacterDisplayName characterType in System.Enum.GetValues(typeof(PlayFabManager.CharacterDisplayName)))
            {
                //���� �����ǿ� �ߺ��Ǵ� ��ᰡ �����ϹǷ� ���� ��ø ��� (if - if ...)
                if (recipe.selectName == characterType.ToString())
                {
                    if (!dicRequierdRecipeCount.ContainsKey(recipe.selectName)) dicRequierdRecipeCount.Add(recipe.selectName, 1);
                    else dicRequierdRecipeCount[recipe.selectName]++;
                }
                if (recipe.recipeNameA == characterType.ToString())
                {
                    if (!dicRequierdRecipeCount.ContainsKey(recipe.recipeNameA)) dicRequierdRecipeCount.Add(recipe.recipeNameA, 1);
                    else dicRequierdRecipeCount[recipe.recipeNameA]++;
                }
                if (recipe.recipeNameB == characterType.ToString())
                {
                    if (!dicRequierdRecipeCount.ContainsKey(recipe.recipeNameB)) dicRequierdRecipeCount.Add(recipe.recipeNameB, 1);
                    else dicRequierdRecipeCount[recipe.recipeNameB]++;
                }
                if (recipe.recipeNameC == characterType.ToString())
                {
                    if (!dicRequierdRecipeCount.ContainsKey(recipe.recipeNameC)) dicRequierdRecipeCount.Add(recipe.recipeNameC, 1);
                    else dicRequierdRecipeCount[recipe.recipeNameC]++;
                }
            }

            arrRecipeList[i].SetReferenceRecipeHighlights(dicExistingCharacterCount, dicRequierdRecipeCount);
        }
    }
}