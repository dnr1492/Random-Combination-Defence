using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using System.Linq;

public class CharacterGenerator : MonoBehaviour
{
    public static List<GameObject> ExistingCharacters { get => existingCharacters; private set => existingCharacters = value; }
    private static List<GameObject> existingCharacters;  //존재하는 캐릭터들

    [SerializeField] UIPlay uiPlay;
    [SerializeField] CameraController cameraController;
    [SerializeField] Tilemap mainTilemap;
    [SerializeField] Transform characterParant;

    [Header("캐릭터 랜덤 뽑기")]
    private GameObject[] arrCommonRating, arrUncommonRating, arrRareRating, arrUniqueRating, arrLegendaryRating;
    private readonly string drawEffectName = "소환";
    private readonly float effectDuration = 1;

    [Header("캐릭터 위치 정렬")]
    private readonly int originCount = 100;
    private int minusX = 0, minusY = 0, plusX = 0, plusY = 0;

    [Header("캐릭터 조합")]
    [SerializeField] UICharacterRecipe uiCharacterRecipe;
    private List<GameObject> characterCombinationGos = new List<GameObject>();

    private void Awake()
    {
        existingCharacters = new List<GameObject>();

        arrCommonRating = Resources.LoadAll<GameObject>("CharacterPrefabs/00_Common");
        arrUncommonRating = Resources.LoadAll<GameObject>("CharacterPrefabs/01_Uncommon");
        arrRareRating = Resources.LoadAll<GameObject>("CharacterPrefabs/02_Rare");
        arrUniqueRating = Resources.LoadAll<GameObject>("CharacterPrefabs/03_Unique");
        arrLegendaryRating = Resources.LoadAll<GameObject>("CharacterPrefabs/04_Legendary");

        characterCombinationGos.AddRange(arrCommonRating);
        characterCombinationGos.AddRange(arrUncommonRating);
        characterCombinationGos.AddRange(arrRareRating);
        characterCombinationGos.AddRange(arrUniqueRating);
        characterCombinationGos.AddRange(arrLegendaryRating);
    }

    #region 캐릭터 랜덤 뽑기
    public IEnumerator DrawRandom(int drawCount, float spawnDelay)
    {
        for (int i = 0; i < drawCount; i++) {
            GameObject target = null;
            CharacterTier tier = CharacterTier.흔한;

            int drawIndex;
            if (tier == CharacterTier.흔한) {
                drawIndex = Random.Range(0, arrCommonRating.Length);
                target = Instantiate(arrCommonRating[drawIndex], characterParant);
                target.name = Rename(target.name);
            }

            if (target == null) yield return null;
            else target.GetComponent<CharacterController>().Init(cameraController, mainTilemap);
            Sort(target);

            existingCharacters.Add(target);

            DrawEffect(target.transform.position);

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private void DrawEffect(Vector3 position)
    {
        PlayActionData effectData = ActionManager.Instance.GetPlayActionData(drawEffectName);
        if (effectData == null) return;

        ActionManager.Instance.PlayPlayAction(position, effectData, effectDuration);
    }
    #endregion

    #region 캐릭터 위치 정렬
    public void Sort(GameObject target)
    {
        for (int i = 0; i < originCount; i++)
        {
            float randomX = Random.Range(-0.5f + minusX, 0.5f + plusX);
            float randomY = Random.Range(-0.5f + minusY, 0.5f + plusY);
            Vector3 randomVector = new Vector3(randomX, randomY, 0);
            Collider2D col = Physics2D.OverlapBox(randomVector, target.GetComponent<Collider2D>().bounds.size / 2, 0);
            if (col == null)
            {
                target.transform.position = randomVector;
                minusX = minusY = plusX = plusY = 0;
                break;
            }

            if (i == originCount - 1 && col != null)
            {
                minusX = minusY -= 1;
                plusX = plusY += 1;
                Sort(target);
                break;
            }
        }
    }
    #endregion

    #region 캐릭터 조합
    public void Combine(List<Image> imgCharacters, string resultName)
    {
        DeleteRecipeIngredient(imgCharacters, resultName);
    }

    private void DeleteRecipeIngredient(List<Image> imgCharacters, string resultName)
    {
        //Result 제외
        var temp = imgCharacters.Take(imgCharacters.Count - 1).ToList();

        //1. 가독성 중시
        List<string> characterDisplayNames = temp
            .Where(c => c.gameObject.activeSelf)
            .Select(c => c.sprite.name)
            .ToList();

        ////2. 성능 중시
        //foreach (var displayName in temp) {
        //    if (displayName.gameObject.activeSelf) {
        //        characterDisplayNames.Add(displayName.sprite.name);
        //    }
        //}

        for (int i = existingCharacters.Count - 1; i >= 0; i--)
        {
            var character = existingCharacters[i];
            for (int j = 0; j < characterDisplayNames.Count; j++)
            {
                if (character.name == characterDisplayNames[j])
                {
                    characterDisplayNames.RemoveAt(j);
                    existingCharacters.RemoveAt(i);
                    Destroy(character);
                    break;
                }
            }
        }

        CreateRecipeResult(resultName);
    }

    private void CreateRecipeResult(string resultName)
    {
        foreach (GameObject characterCombinationPool in characterCombinationGos) {
            if (characterCombinationPool.name == resultName) {
                GameObject resultGo = Instantiate(characterCombinationPool, characterParant);
                resultGo.name = Rename(resultGo.name);
                resultGo.GetComponent<CharacterController>().Init(cameraController, mainTilemap);
                existingCharacters.Add(resultGo);
                Sort(resultGo);
                uiCharacterRecipe.gameObject.SetActive(false);
                return;
            }
        }

        DebugLogger.Log("캐릭터 조합 성공");
    }
    #endregion

    private string Rename(string name)
    {
        name = name.Split("(")[0];
        return name;
    }
}