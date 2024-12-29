using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using System.Linq;

public class PlayerGenerator : MonoBehaviour
{
    public static List<GameObject> ExistingPlayers { get => existingPlayers; private set => existingPlayers = value; }
    private static List<GameObject> existingPlayers = new List<GameObject>();  //존재하는 플레이어들

    private enum Rating { Common, Uncommon, Rare, Unique, Legendary }

    private Dictionary<int, PlayMapData> dicPlayMapDatas;

    [SerializeField] UIPlay uiPlay;
    [SerializeField] CameraController cameraController;
    [SerializeField] Tilemap targetTilemap;

    [Header("플레이어 랜덤 뽑기")]
    [SerializeField] Button btnDraw;
    private GameObject[] arrCommonRating, arrUncommonRating, arrRareRating, arrUniqueRating, arrLegendaryRating;
    private readonly int drawMinRatingPercentage = 0;
    private readonly int drawMaxRatingPercentage = 1000;
    private readonly int drawGold = -1;
    private readonly int drawPopulation = 5;

    [Header("플레이어 위치 정렬")]
    private readonly int originCount = 100;
    private int minusX = 0, minusY = 0, plusX = 0, plusY = 0;

    [Header("플레이어 조합")]
    [SerializeField] UICharacterRecipe uiCharacterRecipe;
    private List<GameObject> playerCombinationGos = new List<GameObject>();

    private void Awake()
    {
        dicPlayMapDatas = DataManager.instance.GetPlayMapData();

        btnDraw.onClick.AddListener(DrawRandom);
    }

    private void Start()
    {
        arrCommonRating = Resources.LoadAll<GameObject>("PlayerPrefabs/0_Common");
        arrUncommonRating = Resources.LoadAll<GameObject>("PlayerPrefabs/1_Uncommon");
        arrRareRating = Resources.LoadAll<GameObject>("PlayerPrefabs/2_Rare");
        arrUniqueRating = Resources.LoadAll<GameObject>("PlayerPrefabs/3_Unique");
        arrLegendaryRating = Resources.LoadAll<GameObject>("PlayerPrefabs/4_Legendary");

        playerCombinationGos.AddRange(arrCommonRating);
        playerCombinationGos.AddRange(arrUncommonRating);
        playerCombinationGos.AddRange(arrRareRating);
        playerCombinationGos.AddRange(arrUniqueRating);
        playerCombinationGos.AddRange(arrLegendaryRating);
    }

    #region 플레이어 랜덤 뽑기
    public void DrawRandom()
    {
        if (CanDraw()) {
            uiPlay.SetUI_Gold(drawGold);
            uiPlay.SetUI_Population(drawPopulation);
        }
        else return;

        GameObject target;
        Rating curRating;
        int randomNnumber = Random.Range(drawMinRatingPercentage, drawMaxRatingPercentage);
        if (randomNnumber >= 0 && randomNnumber < 800)
        {
            curRating = Rating.Common;
            Debug.Log("80% 확률로 Common을 뽑았습니다.");
        }
        else if (randomNnumber >= 800 && randomNnumber < 900)
        {
            curRating = Rating.Uncommon;
            Debug.Log("10% 확률로 Uncommon을 뽑았습니다.");
        }
        else if (randomNnumber >= 900 && randomNnumber <= 980)
        {
            curRating = Rating.Rare;
            Debug.Log("2% 확률로 Rare를 뽑았습니다.");
        }
        else if (randomNnumber >= 980 && randomNnumber <= 995)
        {
            curRating = Rating.Unique;
            Debug.Log("1.5% 확률로 Unique를 뽑았습니다.");
        }
        else
        {
            curRating = Rating.Legendary;
            Debug.Log("0.5% 확률로 Legendary를 뽑았습니다.");
        }

        int drawIndex;
        if (curRating == Rating.Common)
        {
            drawIndex = Random.Range(0, arrCommonRating.Length);
            target = Instantiate(arrCommonRating[drawIndex]);
            target.name = Rename(target.name);
            existingPlayers.Add(target);
        }
        else if (curRating == Rating.Uncommon)
        {
            drawIndex = Random.Range(0, arrUncommonRating.Length);
            target = Instantiate(arrUncommonRating[drawIndex]);
            target.name = Rename(target.name);
            existingPlayers.Add(target);
        }
        else if (curRating == Rating.Rare)
        {
            drawIndex = Random.Range(0, arrRareRating.Length);
            target = Instantiate(arrRareRating[drawIndex]);
            target.name = Rename(target.name);
            existingPlayers.Add(target);
        }
        else if (curRating == Rating.Unique)
        {
            drawIndex = Random.Range(0, arrUniqueRating.Length);
            target = Instantiate(arrUniqueRating[drawIndex]);
            target.name = Rename(target.name);
            existingPlayers.Add(target);
        }
        else /*if (curRating == Rating.Legendary)*/
        {
            drawIndex = Random.Range(0, arrLegendaryRating.Length);
            target = Instantiate(arrLegendaryRating[drawIndex]);
            target.name = Rename(target.name);
            existingPlayers.Add(target);
        }

        if (target == null) return;
        else target.GetComponent<PlayerController>().Init(cameraController, targetTilemap);
        Sort(target);
    }
    #endregion

    #region 플레이어 위치 정렬
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

    #region 플레이어 조합
    public void Combine(List<Image> imgCharacters, string resultName)
    {
        DeleteRecipeIngredient(imgCharacters, resultName);
    }

    private void DeleteRecipeIngredient(List<Image> imgCharacters, string resultName)
    {
        //Result 제외
        var temp = imgCharacters.Take(imgCharacters.Count - 1).ToList();
       
        List<string> characterNames = temp
            .Where(c => c.gameObject.activeSelf)
            .Select(c => c.sprite.name)
            .ToList();

        for (int i = existingPlayers.Count - 1; i >= 0; i--)
        {
            var player = existingPlayers[i];
            for (int j = 0; j < characterNames.Count; j++)
            {
                if (player.name == characterNames[j])
                {
                    characterNames.RemoveAt(j);
                    existingPlayers.RemoveAt(i);
                    Destroy(player);
                    break;
                }
            }
        }

        CreateRecipeResult(resultName);
    }

    private void CreateRecipeResult(string resultName)
    {
        foreach (GameObject playerCombinationPool in playerCombinationGos) {
            if (playerCombinationPool.name == resultName) {
                GameObject resultGo = Instantiate(playerCombinationPool);
                resultGo.name = Rename(resultGo.name);
                resultGo.GetComponent<PlayerController>().Init(cameraController, targetTilemap);
                existingPlayers.Add(resultGo);
                Sort(resultGo);
                uiCharacterRecipe.gameObject.SetActive(false);
                return;
            }
        }
    }
    #endregion

    private string Rename(string name)
    {
        name = name.Split("(")[0];
        return name;
    }

    private bool CanDraw()
    {
        if (uiPlay.GetCurGold + drawGold < 0) return false;
        if (uiPlay.GetCurPopulation >= dicPlayMapDatas[uiPlay.GetCurMapId].maximum_population) return false;
        return true;
    }
}