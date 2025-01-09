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

    public void SetReferenceRecipe(string selectionName)
    {
        for (int i = 0; i < arrRecipeList.Length; i++) {
            arrRecipeList[i].gameObject.SetActive(false);
        }

        var characterRecipeData = DataManager.GetInstance().GetCharacterRecipeData();
        if (!characterRecipeData.ContainsKey(selectionName)) {
            DebugLogger.Log(selectionName + "의 Key가 존재하지 않습니다.");
            return;
        }

        var dicCharacterDatas = DataManager.GetInstance().GetCharacterData();
        for (int i = 0; i < characterRecipeData[selectionName].Count; i++)
        {
            List<Sprite> characterSprites = new List<Sprite>();
            var recipe = characterRecipeData[selectionName][i];
            var spriteType = SpriteManager.SpriteType.ImgCharacter;

            if (!string.IsNullOrEmpty(recipe.select_name)) characterSprites.Add(SpriteManager.GetInstance().GetSprite(spriteType, recipe.select_name));
            if (!string.IsNullOrEmpty(recipe.recipe_name_a)) characterSprites.Add(SpriteManager.GetInstance().GetSprite(spriteType, recipe.recipe_name_a));
            if (!string.IsNullOrEmpty(recipe.recipe_name_b)) characterSprites.Add(SpriteManager.GetInstance().GetSprite(spriteType, recipe.recipe_name_b));
            if (!string.IsNullOrEmpty(recipe.recipe_name_c)) characterSprites.Add(SpriteManager.GetInstance().GetSprite(spriteType, recipe.recipe_name_c));
            if (!string.IsNullOrEmpty(recipe.result_name)) characterSprites.Add(SpriteManager.GetInstance().GetSprite(spriteType, recipe.result_name));

            arrRecipeList[i].gameObject.SetActive(true);
            arrRecipeList[i].SetReferenceRecipe(characterSprites);
        }

        SetReferenceRecipeHighlights(characterRecipeData, selectionName);
    }

    private void SetReferenceRecipeHighlights(Dictionary<string, List<CharacterRecipeData>> characterRecipeData, string selectionName)
    {
        var recipeCount = characterRecipeData[selectionName].Count;
        var existingPlayers = PlayerGenerator.ExistingPlayers;

        for (int i = 0; i < recipeCount; i++)
        {
            var recipe = characterRecipeData[selectionName][i];

            //현재 존재하는 플레이어 개수
            var dicExistingPlayerCount = new Dictionary<string, int>();
            foreach (var player in existingPlayers)
            {
                //조합 레시피에 중복되는 재료가 존재하므로 개수 중첩 방지 (if - else if ...)
                if (player.name == recipe.select_name) {
                    if (!dicExistingPlayerCount.ContainsKey(player.name)) dicExistingPlayerCount.Add(player.name, 1);
                    else dicExistingPlayerCount[player.name]++;
                }
                else if (player.name == recipe.recipe_name_a) {
                    if (!dicExistingPlayerCount.ContainsKey(player.name)) dicExistingPlayerCount.Add(player.name, 1);
                    else dicExistingPlayerCount[player.name]++;
                }
                else if (player.name == recipe.recipe_name_b) {
                    if (!dicExistingPlayerCount.ContainsKey(player.name)) dicExistingPlayerCount.Add(player.name, 1);
                    else dicExistingPlayerCount[player.name]++;
                }
                else if (player.name == recipe.recipe_name_c) {
                    if (!dicExistingPlayerCount.ContainsKey(player.name)) dicExistingPlayerCount.Add(player.name, 1);
                    else dicExistingPlayerCount[player.name]++;
                }
            }

            //조합 레시피에서 요구하는 재료 개수
            var dicRequierdRecipeCount = new Dictionary<string, int>();
            foreach (PlayFabManager.Characters characterType in System.Enum.GetValues(typeof(PlayFabManager.Characters)))
            {
                //조합 레시피에 중복되는 재료가 존재하므로 개수 중첩 허용 (if - if ...)
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

            arrRecipeList[i].SetReferenceRecipeHighlights(dicExistingPlayerCount, dicRequierdRecipeCount);
        }
    }
}