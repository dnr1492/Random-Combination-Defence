using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Threading.Tasks;
using System.Linq;

public class PlayFabManager : MonoBehaviour
{
    public static PlayFabManager instance = null;

    public enum Characters { 주몽, 이순신, 흔한, 안흔한, 희귀한, 유일한 }
    public enum CharacterTier { None, 흔한, 안흔한, 희귀한, 유일한, 전설적인 }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this) Destroy(gameObject);
    }

    #region 회원가입
    public void Regist(string email, string password, string username)
    {
        RegisterPlayFabUserRequest request = new RegisterPlayFabUserRequest();
        request.Email = email;
        request.Password = password;
        request.Username = username;

        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, (error) => Debug.Log("회원가입 실패 : " + error.ErrorMessage));
    }

    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        Debug.Log("회원가입 성공");
    }
    #endregion

    #region 로그인
    private bool isLoginSuccess = false;
    private string curPlayfabId = null;

    public void Login(string email, string password)
    {
        LoginWithEmailAddressRequest request = new LoginWithEmailAddressRequest();
        request.Email = email;
        request.Password = password;

        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, (error) => Debug.Log("로그인 실패 : " + error.ErrorMessage));
    }

    public bool CheckLoginSuccess()
    {
        return isLoginSuccess;
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("로그인 성공");

        isLoginSuccess = true;
        curPlayfabId = result.PlayFabId;
    }
    #endregion

    #region 가상화폐 증가
    public void AddUserVirtualCurrency(UIGameResources uiGameResources, string virtualCurrencyName, int amount)
    {
        AddUserVirtualCurrencyRequest request = new AddUserVirtualCurrencyRequest
        {
            VirtualCurrency = virtualCurrencyName,
            Amount = amount
        };

        PlayFabClientAPI.AddUserVirtualCurrency(request, 
            (result) => {
                DisplayGameResources(uiGameResources, virtualCurrencyName);
                Debug.Log($"{virtualCurrencyName}가 {amount}만큼 증가 완료" +
                    $"\n 총 금액 : {result.Balance}");
            }, 
            (error) => Debug.Log("가상화폐 증가 실패"));
    }
    #endregion

    #region 가상화폐 감소
    public void SubtractUserVirtualCurrency(UIGameResources uiGameResources, string virtualCurrencyName, int amount)
    {
        SubtractUserVirtualCurrencyRequest request = new SubtractUserVirtualCurrencyRequest
        {
            VirtualCurrency = virtualCurrencyName,
            Amount = amount
        };

        PlayFabClientAPI.SubtractUserVirtualCurrency(request,
            (result) =>
            {
                DisplayGameResources(uiGameResources, virtualCurrencyName);
                Debug.Log($"{virtualCurrencyName}가 {amount}만큼 감소 완료" +
                    $"\n 총 금액 : {result.Balance}");
            },
            (error) => Debug.Log($"가상화폐 감소 실패: {error.GenerateErrorReport()}"));
    }
    #endregion

    #region 캐릭터 랜덤 뽑기
    private readonly float weightCommonCharacter = 900f;  //70.423%
    private readonly float weightUncommonCharacter = 300f;  //23.474%
    private readonly float weightRareCharacter = 50f;  //3.912%
    private readonly float weightUniqueCharacter = 25f;  //1.956%
    private readonly float weightLegendaryJumong = 1.5f;  //0.117%
    private readonly float weightLegendaryAdmiralYi = 1.5f;  //0.117%

    public struct DrawCharacterData
    {
        public string displayName;
        public string itemClass;
        public int cardCount;

        public DrawCharacterData(string displayName, string itemClass, int cardCount)
        {
            this.displayName = displayName;
            this.itemClass = itemClass;
            this.cardCount = cardCount;
        }
    }

    #region 동기 로직 - 미사용
    //public void DrawCharacters(UIGameResources uiGameResources, UIPurchaseVirtualCurrency uiPurchaseVirtualCurrency, string virtualCurrencyName, int amount, string catalogVersion, int drawCount)
    //    => GetUserInventory(uiGameResources, uiPurchaseVirtualCurrency, virtualCurrencyName, amount, catalogVersion, drawCount);

    //private void GetUserInventory(UIGameResources uiGameResources, UIPurchaseVirtualCurrency uiPurchaseVirtualCurrency, string virtualCurrencyName, int amount, string catalogVersion, int drawCount)
    //{
    //    this.uiPurchaseVirtualCurrency = uiPurchaseVirtualCurrency;

    //    PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), (result) =>
    //    {
    //        if (result.VirtualCurrency[virtualCurrencyName] >= amount) SubtractUserVirtualCurrency(uiGameResources, virtualCurrencyName, amount, catalogVersion, drawCount);
    //        else Debug.Log("유저가 보유한 가상 화폐의 수량이 부족");
    //    },
    //    (error) => { Debug.Log("유저 인벤토리 획득 실패"); });
    //}

    //private void SubtractUserVirtualCurrency(UIGameResources uiGameResources, string virtualCurrencyName, int amount, string catalogVersion, int drawCount)
    //{
    //    SubtractUserVirtualCurrencyRequest request = new SubtractUserVirtualCurrencyRequest
    //    {
    //        VirtualCurrency = virtualCurrencyName,
    //        Amount = amount
    //    };

    //    PlayFabClientAPI.SubtractUserVirtualCurrency(request,
    //    (result) =>
    //    {
    //        GetCatalogItems(catalogVersion, drawCount);
    //        DisplayGameResources(uiGameResources, virtualCurrencyName);
    //    },
    //    (error) => Debug.Log("가상화폐 감소 실패"));
    //}

    //private void GetCatalogItems(string catalogVersion, int drawCount)
    //{
    //    List<RandomCharacter.CharacterData> characterDatas = new List<RandomCharacter.CharacterData>();

    //    PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest() { CatalogVersion = catalogVersion }, (result) =>
    //    {
    //        for (int i = 0; i < result.Catalog.Count; i++)
    //        {
    //            CatalogItem CatalogItem = result.Catalog[i];
    //            //Legendary
    //            if (CatalogItem.DisplayName == Characters.주몽.ToString()) characterDatas.Add(new RandomCharacter.CharacterData(weightLegendaryJumong, CatalogItem.DisplayName, CatalogItem.ItemId));
    //            else if (CatalogItem.DisplayName == Characters.이순신.ToString()) characterDatas.Add(new RandomCharacter.CharacterData(weightLegendaryAdmiralYi, CatalogItem.DisplayName, CatalogItem.ItemId));
    //            //Unique
    //            else if (CatalogItem.DisplayName == Characters.유일한.ToString()) characterDatas.Add(new RandomCharacter.CharacterData(weightUniqueCharacter, CatalogItem.DisplayName, CatalogItem.ItemId));
    //            //Rare
    //            else if (CatalogItem.DisplayName == Characters.희귀한.ToString()) characterDatas.Add(new RandomCharacter.CharacterData(weightRareCharacter, CatalogItem.DisplayName, CatalogItem.ItemId));
    //            //Uncommon
    //            else if (CatalogItem.DisplayName == Characters.안흔한.ToString()) characterDatas.Add(new RandomCharacter.CharacterData(weightUncommonCharacter, CatalogItem.DisplayName, CatalogItem.ItemId));
    //            //Common
    //            else if (CatalogItem.DisplayName == Characters.흔한.ToString()) characterDatas.Add(new RandomCharacter.CharacterData(weightCommonCharacter, CatalogItem.DisplayName, CatalogItem.ItemId));
    //        }

    //        Dictionary<string, int> dicDrawCharacterDatas = new Dictionary<string, int>();
    //        List<string> itemIds = new List<string>();
    //        Dictionary<string, DrawCharacterData> dicDrawCharacterData = new Dictionary<string, DrawCharacterData>();  //여기서 초기화하지 않고 GetUserInventory()에서 초기화하면 되지 않나?
    //        for (int i = 0; i < drawCount; i++)
    //        {
    //            RandomCharacter.CharacterData characterData = RandomCharacter.DrawRandomCharacter(characterDatas);
    //            itemIds.Add(characterData.itemId);
    //        }

    //        GrantItemsToUser(dicDrawCharacterData, itemIds);
    //    },
    //    (error) => { Debug.Log("상점 불러오기 실패"); });
    //}

    //private void GrantItemsToUser(Dictionary<string, DrawCharacterData> dicDrawCharacterData, List<string> itemIds)
    //{
    //    PlayFab.ServerModels.GrantItemsToUserRequest request = new PlayFab.ServerModels.GrantItemsToUserRequest()
    //    {
    //        PlayFabId = curPlayfabId,
    //        ItemIds = itemIds
    //    };

    //    PlayFabServerAPI.GrantItemsToUser(request, result => GetUserInventory(dicDrawCharacterData, itemIds), (error) => { Debug.Log("유저에게 아이템 주기 실패"); });
    //}

    //private void GetUserInventory(Dictionary<string, DrawCharacterData> dicDrawCharacterData, List<string> itemIds)
    //{
    //    PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),
    //    (result) =>
    //    {
    //        for (int i = 0; i < result.Inventory.Count; i++)
    //        {
    //            for (int j = 0; j < itemIds.Count; j++)
    //            {
    //                if (result.Inventory[i].ItemId == itemIds[j])
    //                {
    //                    DrawCharacterData drawCharacterData = new DrawCharacterData(result.Inventory[i].DisplayName, result.Inventory[i].ItemClass, 1);
    //                    if (dicDrawCharacterData.ContainsKey(drawCharacterData.displayName)) dicDrawCharacterData[drawCharacterData.displayName] = new DrawCharacterData(drawCharacterData.displayName, drawCharacterData.itemClass, dicDrawCharacterData[drawCharacterData.displayName].cardCount + 1);
    //                    else dicDrawCharacterData.Add(drawCharacterData.displayName, drawCharacterData);
    //                }
    //            }
    //        }

    //        uiPurchaseVirtualCurrency.DisplayDrawCharacters(dicDrawCharacterData);
    //    },
    //    (error) => { Debug.Log("유저 인벤토리 획득 실패"); });
    //}
    #endregion

    #region 비동기 로직 - 사용 (feat.최적화 / 로딩 구현 필요) 
    public async void OnClickDrawCharactersAsync(UIGameResources uiGameResources, UIPurchaseDrawCharacters uiPurchaseVirtualCurrency, string virtualCurrencyName, int amount, string catalogVersion, int drawCount)
    {
        //loadingPanel.SetActive(true);

        await DrawCharactersAsync(uiGameResources, uiPurchaseVirtualCurrency, virtualCurrencyName, amount, catalogVersion, drawCount);

        //loadingPanel.SetActive(false);
    }

    private async Task DrawCharactersAsync(UIGameResources uiGameResources, UIPurchaseDrawCharacters uiPurchaseVirtualCurrency, string virtualCurrencyName, int amount, string catalogVersion, int drawCount)
    {
        //유저 인벤토리와 카탈로그 데이터를 뽑기 캐릭터 데이터로 변환하여 병렬로 가져오기
        var inventoryTask = GetUserInventoryAsync();
        var catalogTask = GetCatalogItemToDrawCharacterDataAsync(catalogVersion);

        await Task.WhenAll(inventoryTask, catalogTask);

        var inventoryResult = inventoryTask.Result;
        var characterDatas = catalogTask.Result;
        
        //유저 인벤토리 확인
        if (inventoryResult == null ||
            !inventoryResult.VirtualCurrency.ContainsKey(virtualCurrencyName) ||
            inventoryResult.VirtualCurrency[virtualCurrencyName] < amount)
        {
            Debug.Log("유저가 보유한 가상 화폐의 수량이 부족합니다.");
            return;
        }

        //가상화폐 차감
        var subtractTask = SubtractUserVirtualCurrencyAsync(virtualCurrencyName, amount);
        await subtractTask;
        if (!subtractTask.Result)
        {
            Debug.LogError("가상 화폐 차감 실패");
            return;
        }

        //캐릭터 랜덤 뽑기 (병렬 처리)
        //메인 스레드에서 랜덤 값을 미리 생성
        float[] randomValues = new float[drawCount];
        for (int i = 0; i < drawCount; i++) {
            randomValues[i] = UnityEngine.Random.value;
        }
        var itemIds = await Task.Run(() =>
        {
            var drawnItems = new List<string>();
            for (int i = 0; i < drawCount; i++)
            {
                var characterData = RandomCharacter.DrawRandomCharacter(characterDatas, randomValues[i]);
                drawnItems.Add(characterData.itemId);
            }
            return drawnItems;
        });

        //유저에게 아이템 지급
        // =============== *** 최적화 요망 *** =============== //
        // =============== *** 최적화 요망 *** =============== //
        // =============== *** 최적화 요망 *** =============== //
        float startTime, endTime;
        startTime = Time.realtimeSinceStartup;
        bool isItemsGranted = await GrantItemsToUserAsync(itemIds);
        if (!isItemsGranted)
        {
            Debug.LogError("아이템 지급 실패");
            return;
        }
        endTime = Time.realtimeSinceStartup;
        Debug.Log($"Step - Grant Items: {endTime - startTime} seconds '여기가 문제다...'");

        //유저 인벤토리 갱신 및 UI 업데이트
        UpdateInventoryAndUIAsync(uiGameResources, uiPurchaseVirtualCurrency, itemIds, virtualCurrencyName, inventoryResult);
    }

    //유저 인벤토리 가져오기 비동기
    private Task<GetUserInventoryResult> GetUserInventoryAsync()
    {
        var tcs = new TaskCompletionSource<GetUserInventoryResult>();

        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),
            result => tcs.SetResult(result),
            error =>
            {
                Debug.LogError($"유저 인벤토리 획득 실패: {error.GenerateErrorReport()}");
                tcs.SetResult(null);
            });

        return tcs.Task;
    }

    //가상화폐 차감 비동기
    private Task<bool> SubtractUserVirtualCurrencyAsync(string virtualCurrencyName, int amount)
    {
        var tcs = new TaskCompletionSource<bool>();
        var request = new SubtractUserVirtualCurrencyRequest
        {
            VirtualCurrency = virtualCurrencyName,
            Amount = amount
        };

        PlayFabClientAPI.SubtractUserVirtualCurrency(request,
            result => tcs.SetResult(true),
            error =>
            {
                Debug.LogError($"가상 화폐 차감 실패: {error.GenerateErrorReport()}");
                tcs.SetResult(false);
            });

        return tcs.Task;
    }

    //카탈로그 아이템을 뽑기 캐릭터 데이터로 변환해서 가져오기 비동기
    private Task<List<RandomCharacter.CharacterData>> GetCatalogItemToDrawCharacterDataAsync(string catalogVersion)
    {
        var tcs = new TaskCompletionSource<List<RandomCharacter.CharacterData>>();

        PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest { CatalogVersion = catalogVersion },
            result =>
            {
                var characterDatas = new List<RandomCharacter.CharacterData>();
                foreach (var catalogItem in result.Catalog)
                {
                    if (catalogItem.DisplayName == Characters.주몽.ToString())
                        characterDatas.Add(new RandomCharacter.CharacterData(weightLegendaryJumong, catalogItem.DisplayName, catalogItem.ItemId));
                    else if (catalogItem.DisplayName == Characters.이순신.ToString())
                        characterDatas.Add(new RandomCharacter.CharacterData(weightLegendaryAdmiralYi, catalogItem.DisplayName, catalogItem.ItemId));
                    else if (catalogItem.DisplayName == Characters.유일한.ToString())
                        characterDatas.Add(new RandomCharacter.CharacterData(weightUniqueCharacter, catalogItem.DisplayName, catalogItem.ItemId));
                    else if (catalogItem.DisplayName == Characters.희귀한.ToString())
                        characterDatas.Add(new RandomCharacter.CharacterData(weightRareCharacter, catalogItem.DisplayName, catalogItem.ItemId));
                    else if (catalogItem.DisplayName == Characters.안흔한.ToString())
                        characterDatas.Add(new RandomCharacter.CharacterData(weightUncommonCharacter, catalogItem.DisplayName, catalogItem.ItemId));
                    else if (catalogItem.DisplayName == Characters.흔한.ToString())
                        characterDatas.Add(new RandomCharacter.CharacterData(weightCommonCharacter, catalogItem.DisplayName, catalogItem.ItemId));
                }
                tcs.SetResult(characterDatas);
            },
            error =>
            {
                Debug.LogError($"카탈로그 아이템 가져오기 실패: {error.GenerateErrorReport()}");
                tcs.SetResult(null);
            });

        return tcs.Task;
    }

    //유저에게 아이템 지급 비동기
    private Task<bool> GrantItemsToUserAsync(List<string> itemIds)
    {
        var tcs = new TaskCompletionSource<bool>();
        var grantItemsRequest = new PlayFab.ServerModels.GrantItemsToUserRequest
        {
            PlayFabId = curPlayfabId,
            ItemIds = itemIds
        };

        PlayFabServerAPI.GrantItemsToUser(grantItemsRequest,
                success => tcs.SetResult(true),
                error =>
                {
                    Debug.LogError($"유저에게 아이템 지급 실패: {error.GenerateErrorReport()}");
                    tcs.SetResult(false);
                });

        return tcs.Task;
    }

    //유저의 인벤토리 및 UI 갱신
    private void UpdateInventoryAndUIAsync(UIGameResources uiGameResources, UIPurchaseDrawCharacters uiPurchaseVirtualCurrency, List<string> itemIds, string virtualCurrencyName, GetUserInventoryResult inventoryResult)
    {
        var dicDrawCharacterData = new Dictionary<string, DrawCharacterData>();

        foreach (var itemId in itemIds)
        {
            var matchingItems = inventoryResult.Inventory.Where(item => item.ItemId == itemId);
            foreach (var item in matchingItems)
            {
                var drawCharacterData = new DrawCharacterData(item.DisplayName, item.ItemClass, 1);
                if (dicDrawCharacterData.ContainsKey(drawCharacterData.displayName))
                {
                    var existingData = dicDrawCharacterData[drawCharacterData.displayName];
                    dicDrawCharacterData[drawCharacterData.displayName] = new DrawCharacterData(
                        existingData.displayName,
                        existingData.itemClass,
                        existingData.cardCount + 1
                    );
                }
                else dicDrawCharacterData.Add(drawCharacterData.displayName, drawCharacterData);
            }
        }

        //UI 업데이트
        uiPurchaseVirtualCurrency.DisplayDrawCharacters(dicDrawCharacterData);
        DisplayGameResources(uiGameResources, virtualCurrencyName);
    }
    #endregion
    #endregion

    #region 신규 플레이어 데이터 설정
    public void InitUserData(string catalogVersion) => GetCatalogItems(catalogVersion);

    private void GetCatalogItems(string catalogVersion)
    {
        PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest() { CatalogVersion = catalogVersion }, (result) =>
        {
            List<string> displayNames = new List<string>();

            for (int i = 0; i < result.Catalog.Count; i++)
            {
                CatalogItem CatalogItem = result.Catalog[i];
                displayNames.Add(CatalogItem.DisplayName);
            }

            GetUserData(displayNames);
        },
        (error) => { Debug.Log("상점 불러오기 실패"); });
    }

    private void GetUserData(List<string> displayNames)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest() { PlayFabId = curPlayfabId },
        result =>
        {
            for (int i = 0; i < Enum.GetNames(typeof(Characters)).Length; i++)
            {
                if (result.Data.ContainsKey(Enum.GetNames(typeof(Characters))[i]))
                {
                    InitCharacterLevelData(displayNames[i], int.Parse(result.Data[displayNames[i]].Value));
                    continue;
                }
                else
                {
                    int initLevel = 1;
                    UpdateUserData(displayNames[i], initLevel);
                    InitCharacterLevelData(displayNames[i], initLevel);
                }
            }
        },
        error => Debug.Log("유저 데이터 불러오기 실패"));
    }

    private void UpdateUserData(string displayName, int level)
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest() {
            Data = new Dictionary<string, string>() { { displayName, level.ToString() } }
        },
        null,
        error => Debug.Log("유저 데이터 저장 실패"));
    }

    private void InitCharacterLevelData(string displayName, int curLevel)
    {
        if (dicCharacterLevelDatas.ContainsKey(displayName)) dicCharacterLevelDatas[displayName] = curLevel;
        else dicCharacterLevelDatas.Add(displayName, curLevel);

        InfoManager.GetInstance().SaveCharacterInfo(displayName, curLevel);
    }
    #endregion

    #region UI 게임 자원 표시
    public void DisplayGameResources(UIGameResources uiGameResources, string virtualCurrencyName)
        => GetUserInventory(uiGameResources, virtualCurrencyName);

    private void GetUserInventory(UIGameResources uiGameResources, string virtualCurrencyName)
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),
        (result) => {
            uiGameResources.DisplayGameResourceAmount(result.VirtualCurrency[virtualCurrencyName]);
        },
        (error) => { Debug.Log("유저 인벤토리 획득 실패"); });
    }
    #endregion

    #region UI 캐릭터 표시
    private Dictionary<string, int[]> dicLegendaryCharacterDatas = new Dictionary<string, int[]>();
    private Dictionary<string, int[]> dicUniqueCharacterDatas = new Dictionary<string, int[]>();
    private Dictionary<string, int[]> dicRareCharacterDatas = new Dictionary<string, int[]>();
    private Dictionary<string, int[]> dicUncommonCharacterDatas = new Dictionary<string, int[]>();
    private Dictionary<string, int[]> dicCommonCharacterDatas = new Dictionary<string, int[]>();
    private Dictionary<string, int[]> dicAllCharacterDatas = new Dictionary<string, int[]>();

    public void DisplayCharacterCard(UICharacter uiCharacter) => GetUserInventory(uiCharacter);

    private void GetUserInventory(UICharacter uiCharacter)
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),
        (result) =>
        {
            for (int i = 0; i < result.Inventory.Count; i++)
            {
                if (result.Inventory[i].ItemClass == CharacterTier.전설적인.ToString())
                {
                    if (!dicLegendaryCharacterDatas.ContainsKey(result.Inventory[i].DisplayName)) dicLegendaryCharacterDatas.Add(result.Inventory[i].DisplayName, new int[] { dicCharacterLevelDatas[result.Inventory[i].DisplayName], (int)result.Inventory[i].RemainingUses, (int)CharacterTier.전설적인 });
                    else dicLegendaryCharacterDatas[result.Inventory[i].DisplayName] = new int[] { dicCharacterLevelDatas[result.Inventory[i].DisplayName], (int)result.Inventory[i].RemainingUses, (int)CharacterTier.전설적인 };
                }
                if (result.Inventory[i].ItemClass == CharacterTier.유일한.ToString())
                {
                    if (!dicUniqueCharacterDatas.ContainsKey(result.Inventory[i].DisplayName)) dicUniqueCharacterDatas.Add(result.Inventory[i].DisplayName, new int[] { dicCharacterLevelDatas[result.Inventory[i].DisplayName], (int)result.Inventory[i].RemainingUses, (int)CharacterTier.유일한 });
                    else dicUniqueCharacterDatas[result.Inventory[i].DisplayName] = new int[] { dicCharacterLevelDatas[result.Inventory[i].DisplayName], (int)result.Inventory[i].RemainingUses, (int)CharacterTier.유일한 };
                }
                if (result.Inventory[i].ItemClass == CharacterTier.희귀한.ToString())
                {
                    if (!dicRareCharacterDatas.ContainsKey(result.Inventory[i].DisplayName)) dicRareCharacterDatas.Add(result.Inventory[i].DisplayName, new int[] { dicCharacterLevelDatas[result.Inventory[i].DisplayName], (int)result.Inventory[i].RemainingUses, (int)CharacterTier.희귀한 });
                    else dicRareCharacterDatas[result.Inventory[i].DisplayName] = new int[] { dicCharacterLevelDatas[result.Inventory[i].DisplayName], (int)result.Inventory[i].RemainingUses, (int)CharacterTier.희귀한 };
                }
                if (result.Inventory[i].ItemClass == CharacterTier.안흔한.ToString())
                {
                    if (!dicUncommonCharacterDatas.ContainsKey(result.Inventory[i].DisplayName)) dicUncommonCharacterDatas.Add(result.Inventory[i].DisplayName, new int[] { dicCharacterLevelDatas[result.Inventory[i].DisplayName], (int)result.Inventory[i].RemainingUses, (int)CharacterTier.안흔한 });
                    else dicUncommonCharacterDatas[result.Inventory[i].DisplayName] = new int[] { dicCharacterLevelDatas[result.Inventory[i].DisplayName], (int)result.Inventory[i].RemainingUses, (int)CharacterTier.안흔한 };
                }
                if (result.Inventory[i].ItemClass == CharacterTier.흔한.ToString())
                {
                    if (!dicCommonCharacterDatas.ContainsKey(result.Inventory[i].DisplayName)) dicCommonCharacterDatas.Add(result.Inventory[i].DisplayName, new int[] { dicCharacterLevelDatas[result.Inventory[i].DisplayName], (int)result.Inventory[i].RemainingUses, (int)CharacterTier.흔한 });
                    else dicCommonCharacterDatas[result.Inventory[i].DisplayName] = new int[] { dicCharacterLevelDatas[result.Inventory[i].DisplayName], (int)result.Inventory[i].RemainingUses, (int)CharacterTier.흔한 };
                }
            }

            uiCharacter.DisplayCharacters(CombineDictionary());
        },
        (error) => { Debug.Log("유저 인벤토리 획득 실패"); });
    }

    private Dictionary<string, int[]> CombineDictionary()
    {
        foreach (var data in dicLegendaryCharacterDatas)
        {
            if (!dicAllCharacterDatas.ContainsKey(data.Key)) dicAllCharacterDatas.Add(data.Key, data.Value);
            else dicAllCharacterDatas[data.Key] = data.Value;
            Debug.Log("키 : " + data.Key + "\n 레벨 : " + data.Value[0] + "\n 카드 보유량 : " + data.Value[1] + "\n 티어 : " + data.Value[2]);
        }
        foreach (var data in dicUniqueCharacterDatas)
        {
            if (!dicAllCharacterDatas.ContainsKey(data.Key)) dicAllCharacterDatas.Add(data.Key, data.Value);
            else dicAllCharacterDatas[data.Key] = data.Value;
            Debug.Log("키 : " + data.Key + "\n 레벨 : " + data.Value[0] + "\n 카드 보유량 : " + data.Value[1] + "\n 티어 : " + data.Value[2]);
        }
        foreach (var data in dicRareCharacterDatas)
        {
            if (!dicAllCharacterDatas.ContainsKey(data.Key)) dicAllCharacterDatas.Add(data.Key, data.Value);
            else dicAllCharacterDatas[data.Key] = data.Value;
            Debug.Log("키 : " + data.Key + "\n 레벨 : " + data.Value[0] + "\n 카드 보유량 : " + data.Value[1] + "\n 티어 : " + data.Value[2]);
        }
        foreach (var data in dicUncommonCharacterDatas)
        {
            if (!dicAllCharacterDatas.ContainsKey(data.Key)) dicAllCharacterDatas.Add(data.Key, data.Value);
            else dicAllCharacterDatas[data.Key] = data.Value;
            Debug.Log("키 : " + data.Key + "\n 레벨 : " + data.Value[0] + "\n 카드 보유량 : " + data.Value[1] + "\n 티어 : " + data.Value[2]);
        }
        foreach (var data in dicCommonCharacterDatas)
        {
            if (!dicAllCharacterDatas.ContainsKey(data.Key)) dicAllCharacterDatas.Add(data.Key, data.Value);
            else dicAllCharacterDatas[data.Key] = data.Value;
            Debug.Log("키 : " + data.Key + "\n 레벨 : " + data.Value[0] + "\n 카드 보유량 : " + data.Value[1] + "\n 티어 : " + data.Value[2]);
        }

        return dicAllCharacterDatas;
    }
    #endregion

    #region 보석으로 골드를 구매 - 비동기
    public async Task PurchaseJewelToGoldAsync(UIGameResources uiJewel, UIGameResources uiGold, int price, int amount)
    {
        string addVirtualCurrencyName = "GD";
        string subtractVirtualCurrencyName = "JE";

        //유저 인벤토리 병렬로 가져오기
        var inventoryTask = GetUserInventoryAsync();
        await Task.WhenAll(inventoryTask);
        var inventoryResult = inventoryTask.Result;

        //유저 인벤토리 확인
        if (inventoryResult == null ||
            !inventoryResult.VirtualCurrency.ContainsKey(subtractVirtualCurrencyName) ||
            inventoryResult.VirtualCurrency[subtractVirtualCurrencyName] < price)
        {
            Debug.Log("유저가 보유한 가상 화폐의 수량이 부족합니다.");
            return;
        }

        //가상화폐 차감
        var subtractTask = SubtractUserVirtualCurrencyAsync(subtractVirtualCurrencyName, price);
        await subtractTask;
        if (!subtractTask.Result)
        {
            Debug.LogError("가상 화폐 차감 실패");
            return;
        }

        AddUserVirtualCurrency(uiGold, addVirtualCurrencyName, amount);
        DisplayGameResources(uiJewel, subtractVirtualCurrencyName);
    }
    #endregion

    #region 골드로 캐릭터 카드 레벨업 - 비동기
    private UICharacter uiCharacter;
    private UICharacterCardDataPopup uiCharacterCardDataPopup;
    private Dictionary<string, int> dicCharacterLevelDatas = new Dictionary<string, int>();

    public async Task UpgradeCharacterCardLevelToGoldAsync(UICharacterCardDataPopup uiCharacterCardDataPopup, UICharacter uiCharacter, UIGameResources uiGold, string displayName, int price, int levelUpRemainingUses)
    {
        this.uiCharacter = uiCharacter;
        this.uiCharacterCardDataPopup = uiCharacterCardDataPopup;

        string subtractVirtualCurrencyName = "GD";

        int[] values = dicAllCharacterDatas[displayName];
        int level = values[0];  
        int remainingUses = values[1];
        int tier = values[2];
        //Debug.Log("현재 선택된 캐릭터" + "\n 키 : " + displayName + "\n 레벨 : " + values[0] + "\n 카드 보유량 : " + values[1] + "\n 티어 : " + values[2]);

        //유저 인벤토리 병렬로 가져오기
        var inventoryTask = GetUserInventoryAsync();
        await Task.WhenAll(inventoryTask);
        var inventoryResult = inventoryTask.Result;

        //유저 인벤토리 확인
        if (inventoryResult == null ||
            !inventoryResult.VirtualCurrency.ContainsKey(subtractVirtualCurrencyName) ||
            inventoryResult.VirtualCurrency[subtractVirtualCurrencyName] < price)
        {
            Debug.Log("유저가 보유한 가상 화폐의 수량이 부족합니다.");
            return;
        }
        else if (levelUpRemainingUses > remainingUses)
        {
            Debug.Log("유저가 보유한 캐릭터 카드의 보유량 부족");
            return;
        }

        //가상화폐 차감
        var subtractTask = SubtractUserVirtualCurrencyAsync(subtractVirtualCurrencyName, price);
        await subtractTask;
        if (!subtractTask.Result)
        {
            Debug.LogError("가상 화폐 차감 실패");
            return;
        }

        //유저 캐릭터 카드 레벨업
        for (int i = 0; i < inventoryResult.Inventory.Count; i++)
        {
            if (inventoryResult.Inventory[i].DisplayName == displayName) 
            {
                ConsumeItem(levelUpRemainingUses, inventoryResult.Inventory[i].ItemInstanceId);
                remainingUses -= levelUpRemainingUses;
                level++;
                break;
            }
        }

        GetUserData(displayName, level, remainingUses, tier);
        DisplayGameResources(uiGold, subtractVirtualCurrencyName);
    }

    private void ConsumeItem(int consumeCount, string itemInstanceId)
    {
        PlayFabClientAPI.ConsumeItem(new ConsumeItemRequest
        {
            ConsumeCount = consumeCount,
            ItemInstanceId = itemInstanceId
        },
        null,
        (error) => { Debug.Log("소모성 아이템 사용 실패"); });
    }

    private void GetUserData(string displayName, int level, int remainingUses, int tier)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest() { PlayFabId = curPlayfabId },
        result =>
        {
            if (result.Data.ContainsKey(displayName))
            {
                SaveCharacterLevelData(displayName, remainingUses, tier);
                UpdateUserData(displayName, level);
            }
        },
        error => Debug.Log("유저 데이터 불러오기 실패"));
    }

    private void SaveCharacterLevelData(string displayName, int remainingUses, int tier)
    {
        if (dicCharacterLevelDatas.ContainsKey(displayName))
        {
            dicCharacterLevelDatas[displayName] += 1;
            dicAllCharacterDatas[displayName] = new int[] { dicCharacterLevelDatas[displayName], remainingUses, tier };
        }
        else return;

        DisplayCharacterCard(uiCharacter);

        int levelUpRemainingUses = DataManager.GetInstance().GetCharacterCardLevelQuentityData(dicCharacterLevelDatas[displayName], tier);
        uiCharacterCardDataPopup.Open(dicCharacterLevelDatas[displayName].ToString(), displayName, remainingUses, levelUpRemainingUses);
    }
    #endregion
}