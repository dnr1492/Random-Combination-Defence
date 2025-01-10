using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using System.Linq;

public class CharacterGenerator : MonoBehaviour
{
    public static List<GameObject> ExistingCharacters { get => existingCharacters; private set => existingCharacters = value; }
    private static List<GameObject> existingCharacters;  //�����ϴ� ĳ���͵�

    private enum Tier { Common, Uncommon, Rare, Unique, Legendary }

    private Dictionary<int, PlayMapData> dicPlayMapDatas;

    [SerializeField] UIPlay uiPlay;
    [SerializeField] CameraController cameraController;
    [SerializeField] Tilemap mainTilemap;

    [Header("ĳ���� ���� �̱�")]
    [SerializeField] Button btnDraw;
    private GameObject[] arrCommonRating, arrUncommonRating, arrRareRating, arrUniqueRating, arrLegendaryRating;
    private readonly int drawMinRatingPercentage = 0;
    private readonly int drawMaxRatingPercentage = 1000;
    private readonly int drawGold = -1;
    private readonly int drawPopulation = 5;

    [Header("ĳ���� ��ġ ����")]
    private readonly int originCount = 100;
    private int minusX = 0, minusY = 0, plusX = 0, plusY = 0;

    [Header("ĳ���� ����")]
    [SerializeField] UICharacterRecipe uiCharacterRecipe;
    private List<GameObject> characterCombinationGos = new List<GameObject>();

    private void Awake()
    {
        existingCharacters = new List<GameObject>();

        dicPlayMapDatas = DataManager.GetInstance().GetPlayMapData();

        btnDraw.onClick.AddListener(DrawRandom);
    }

    private void Start()
    {
        arrCommonRating = Resources.LoadAll<GameObject>("CharacterPrefabs/0_Common");
        arrUncommonRating = Resources.LoadAll<GameObject>("CharacterPrefabs/1_Uncommon");
        arrRareRating = Resources.LoadAll<GameObject>("CharacterPrefabs/2_Rare");
        arrUniqueRating = Resources.LoadAll<GameObject>("CharacterPrefabs/3_Unique");
        arrLegendaryRating = Resources.LoadAll<GameObject>("CharacterPrefabs/4_Legendary");

        characterCombinationGos.AddRange(arrCommonRating);
        characterCombinationGos.AddRange(arrUncommonRating);
        characterCombinationGos.AddRange(arrRareRating);
        characterCombinationGos.AddRange(arrUniqueRating);
        characterCombinationGos.AddRange(arrLegendaryRating);
    }

    #region ĳ���� ���� �̱�
    public void DrawRandom()
    {
        if (CanDraw()) {
            uiPlay.SetUI_Gold(drawGold);
            uiPlay.SetUI_Population(drawPopulation);
        }
        else return;

        GameObject target;
        Tier curRating;
        int randomNnumber = Random.Range(drawMinRatingPercentage, drawMaxRatingPercentage);
        if (randomNnumber >= 0 && randomNnumber < 800)
        {
            curRating = Tier.Common;
            DebugLogger.Log("80% Ȯ���� Common�� �̾ҽ��ϴ�.");
        }
        else if (randomNnumber >= 800 && randomNnumber < 900)
        {
            curRating = Tier.Uncommon;
            DebugLogger.Log("10% Ȯ���� Uncommon�� �̾ҽ��ϴ�.");
        }
        else if (randomNnumber >= 900 && randomNnumber <= 980)
        {
            curRating = Tier.Rare;
            DebugLogger.Log("2% Ȯ���� Rare�� �̾ҽ��ϴ�.");
        }
        else if (randomNnumber >= 980 && randomNnumber <= 995)
        {
            curRating = Tier.Unique;
            DebugLogger.Log("1.5% Ȯ���� Unique�� �̾ҽ��ϴ�.");
        }
        else
        {
            curRating = Tier.Legendary;
            DebugLogger.Log("0.5% Ȯ���� Legendary�� �̾ҽ��ϴ�.");
        }

        int drawIndex;
        if (curRating == Tier.Common)
        {
            drawIndex = Random.Range(0, arrCommonRating.Length);
            target = Instantiate(arrCommonRating[drawIndex]);
            target.name = Rename(target.name);
            existingCharacters.Add(target);
        }
        else if (curRating == Tier.Uncommon)
        {
            drawIndex = Random.Range(0, arrUncommonRating.Length);
            target = Instantiate(arrUncommonRating[drawIndex]);
            target.name = Rename(target.name);
            existingCharacters.Add(target);
        }
        else if (curRating == Tier.Rare)
        {
            drawIndex = Random.Range(0, arrRareRating.Length);
            target = Instantiate(arrRareRating[drawIndex]);
            target.name = Rename(target.name);
            existingCharacters.Add(target);
        }
        else if (curRating == Tier.Unique)
        {
            drawIndex = Random.Range(0, arrUniqueRating.Length);
            target = Instantiate(arrUniqueRating[drawIndex]);
            target.name = Rename(target.name);
            existingCharacters.Add(target);
        }
        else /*if (curRating == Rating.Legendary)*/
        {
            drawIndex = Random.Range(0, arrLegendaryRating.Length);
            target = Instantiate(arrLegendaryRating[drawIndex]);
            target.name = Rename(target.name);
            existingCharacters.Add(target);
        }

        if (target == null) return;
        else target.GetComponent<CharacterController>().Init(cameraController, mainTilemap);
        Sort(target);
    }
    #endregion

    #region ĳ���� ��ġ ����
    public void Sort(GameObject target)
    {
        for (int i = 0; i < originCount; i++)
        {
            float randomX = Random.Range(-0.5f + minusX, 0.5f + plusX);
            float randomY = Random.Range(-0.5f + minusY, 0.5f + plusY);
            Vector3 randomVector = new Vector3(randomX, randomY, 0);
            Collider2D col = Physics2D.OverlapBox(randomVector, target.GetComponent<BoxCollider2D>().size / 2, 0);
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

    #region ĳ���� ����
    public void Combine(List<Image> imgCharacters, string resultName)
    {
        DeleteRecipeIngredient(imgCharacters, resultName);
    }

    private void DeleteRecipeIngredient(List<Image> imgCharacters, string resultName)
    {
        //Result ����
        var temp = imgCharacters.Take(imgCharacters.Count - 1).ToList();
       
        List<string> characterDisplayNames = temp
            .Where(c => c.gameObject.activeSelf)
            .Select(c => c.sprite.name)
            .ToList();

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
                GameObject resultGo = Instantiate(characterCombinationPool);
                resultGo.name = Rename(resultGo.name);
                resultGo.GetComponent<CharacterController>().Init(cameraController, mainTilemap);
                existingCharacters.Add(resultGo);
                Sort(resultGo);
                uiCharacterRecipe.gameObject.SetActive(false);
                return;
            }
        }

        DebugLogger.Log("ĳ���� ���� ����");
    }
    #endregion

    private string Rename(string name)
    {
        name = name.Split("(")[0];
        return name;
    }

    private bool CanDraw()
    {
        //��� ����
        if (uiPlay.GetCurGold + drawGold < 0) return false;
        //�α��� ����
        if (uiPlay.GetCurPopulation >= dicPlayMapDatas[uiPlay.GetCurMapId].maximum_population) return false;
        //�̱� ����
        return true;
    }
}