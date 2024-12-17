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

        var characterRecipeData = DataManager.instance.GetCharacterRecipeData();
        if (!characterRecipeData.ContainsKey(selectionName)) {
            Debug.Log(selectionName + "�� Key�� �������� �ʽ��ϴ�.");
            return;
        }

        var dicCharacterDatas = DataManager.instance.GetCharacterData();
        for (int i = 0; i < characterRecipeData[selectionName].Count; i++)
        {
            List<string> characterImagePaths = new List<string>();

            if (!string.IsNullOrEmpty(characterRecipeData[selectionName][i].select_name)) characterImagePaths.Add(dicCharacterDatas[characterRecipeData[selectionName][i].select_name].character_image_path);
            if (!string.IsNullOrEmpty(characterRecipeData[selectionName][i].recipe_name_a)) characterImagePaths.Add(dicCharacterDatas[characterRecipeData[selectionName][i].recipe_name_a].character_image_path);
            if (!string.IsNullOrEmpty(characterRecipeData[selectionName][i].recipe_name_b)) characterImagePaths.Add(dicCharacterDatas[characterRecipeData[selectionName][i].recipe_name_b].character_image_path);
            if (!string.IsNullOrEmpty(characterRecipeData[selectionName][i].recipe_name_c)) characterImagePaths.Add(dicCharacterDatas[characterRecipeData[selectionName][i].recipe_name_c].character_image_path);
            if (!string.IsNullOrEmpty(characterRecipeData[selectionName][i].result_name)) characterImagePaths.Add(dicCharacterDatas[characterRecipeData[selectionName][i].result_name].character_image_path);

            arrRecipeList[i].gameObject.SetActive(true);
            arrRecipeList[i].SetReferenceRecipe(characterImagePaths);
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

            //���� �����ϴ� �÷��̾� ����
            var dicExistingPlayerCount = new Dictionary<string, int>();
            foreach (var player in existingPlayers)
            {
                //���� �����ǿ� �ߺ��Ǵ� ��ᰡ �����ϹǷ� ���� ��ø ���� (if - else if ...)
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

            //���� �����ǿ��� �䱸�ϴ� ��� ����
            var dicRequierdRecipeCount = new Dictionary<string, int>();
            foreach (PlayFabManager.Characters characterType in System.Enum.GetValues(typeof(PlayFabManager.Characters)))
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

            arrRecipeList[i].SetReferenceRecipeHighlights(dicExistingPlayerCount, dicRequierdRecipeCount);
        }
    }
}