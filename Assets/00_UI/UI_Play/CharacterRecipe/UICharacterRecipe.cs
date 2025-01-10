using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UICharacterRecipe : MonoBehaviour
{
    private RecipeList[] arrRecipeList;

    private void Awake()
    {
        arrRecipeList = GetComponentsInChildren<RecipeList>();
    }

    public void SetReferenceRecipe(string displayName)
    {
        for (int i = 0; i < arrRecipeList.Length; i++) {
            arrRecipeList[i].gameObject.SetActive(false);
        }

        var characterRecipeData = DataManager.GetInstance().GetCharacterRecipeData();
        if (!characterRecipeData.ContainsKey(displayName)) {
            DebugLogger.Log(displayName + "�� Key�� �������� �ʽ��ϴ�.");
            return;
        }

        var dicCharacterDatas = DataManager.GetInstance().GetCharacterData();
        for (int i = 0; i < characterRecipeData[displayName].Count; i++)
        {
            List<Sprite> characterSprites = new List<Sprite>();
            var recipe = characterRecipeData[displayName][i];
            var spriteType = SpriteManager.SpriteType.ImgCharacter;

            if (!string.IsNullOrEmpty(recipe.select_name)) characterSprites.Add(SpriteManager.GetInstance().GetSprite(spriteType, recipe.select_name));
            if (!string.IsNullOrEmpty(recipe.recipe_name_a)) characterSprites.Add(SpriteManager.GetInstance().GetSprite(spriteType, recipe.recipe_name_a));
            if (!string.IsNullOrEmpty(recipe.recipe_name_b)) characterSprites.Add(SpriteManager.GetInstance().GetSprite(spriteType, recipe.recipe_name_b));
            if (!string.IsNullOrEmpty(recipe.recipe_name_c)) characterSprites.Add(SpriteManager.GetInstance().GetSprite(spriteType, recipe.recipe_name_c));
            if (!string.IsNullOrEmpty(recipe.result_name)) characterSprites.Add(SpriteManager.GetInstance().GetSprite(spriteType, recipe.result_name));

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
                if (character.name == recipe.select_name) {
                    if (!dicExistingCharacterCount.ContainsKey(character.name)) dicExistingCharacterCount.Add(character.name, 1);
                    else dicExistingCharacterCount[character.name]++;
                }
                else if (character.name == recipe.recipe_name_a) {
                    if (!dicExistingCharacterCount.ContainsKey(character.name)) dicExistingCharacterCount.Add(character.name, 1);
                    else dicExistingCharacterCount[character.name]++;
                }
                else if (character.name == recipe.recipe_name_b) {
                    if (!dicExistingCharacterCount.ContainsKey(character.name)) dicExistingCharacterCount.Add(character.name, 1);
                    else dicExistingCharacterCount[character.name]++;
                }
                else if (character.name == recipe.recipe_name_c) {
                    if (!dicExistingCharacterCount.ContainsKey(character.name)) dicExistingCharacterCount.Add(character.name, 1);
                    else dicExistingCharacterCount[character.name]++;
                }
            }

            //���� �����ǿ��� �䱸�ϴ� ��� ����
            var dicRequierdRecipeCount = new Dictionary<string, int>();
            foreach (PlayFabManager.CharacterDisplayName characterType in System.Enum.GetValues(typeof(PlayFabManager.CharacterDisplayName)))
            {
                //���� �����ǿ� �ߺ��Ǵ� ��ᰡ �����ϹǷ� ���� ��ø ��� (if - if ...)
                if (recipe.select_name == characterType.ToString())
                {
                    if (!dicRequierdRecipeCount.ContainsKey(recipe.select_name)) dicRequierdRecipeCount.Add(recipe.select_name, 1);
                    else dicRequierdRecipeCount[recipe.select_name]++;
                }
                if (recipe.recipe_name_a == characterType.ToString())
                {
                    if (!dicRequierdRecipeCount.ContainsKey(recipe.recipe_name_a)) dicRequierdRecipeCount.Add(recipe.recipe_name_a, 1);
                    else dicRequierdRecipeCount[recipe.recipe_name_a]++;
                }
                if (recipe.recipe_name_b == characterType.ToString())
                {
                    if (!dicRequierdRecipeCount.ContainsKey(recipe.recipe_name_b)) dicRequierdRecipeCount.Add(recipe.recipe_name_b, 1);
                    else dicRequierdRecipeCount[recipe.recipe_name_b]++;
                }
                if (recipe.recipe_name_c == characterType.ToString())
                {
                    if (!dicRequierdRecipeCount.ContainsKey(recipe.recipe_name_c)) dicRequierdRecipeCount.Add(recipe.recipe_name_c, 1);
                    else dicRequierdRecipeCount[recipe.recipe_name_c]++;
                }
            }

            arrRecipeList[i].SetReferenceRecipeHighlights(dicExistingCharacterCount, dicRequierdRecipeCount);
        }
    }
}