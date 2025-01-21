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

    public enum CharacterDisplayName { 검병, 창병, 궁병, 보급병, 광전사, 군사, 책사, 안흔한, 희귀한, 유일한, 주몽, 이순신 }
    public enum CharacterTier { None, 흔한, 안흔한, 희귀한, 유일한, 전설적인 }

    private void Awake()
    {
        if (instance == null) {
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

        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, (error) => DebugLogger.Log("회원가입 실패 : " + error.ErrorMessage));
    }

    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        DebugLogger.Log("회원가입 성공");
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

        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, (error) => DebugLogger.Log("로그인 실패 : " + error.ErrorMessage));
    }

    public bool CheckLoginSuccess()
    {
        return isLoginSuccess;
    }

    private void OnLoginSuccess(LoginResult result)
    {
        DebugLogger.Log("로그인 성공");

        isLoginSuccess = true;
        curPlayfabId = result.PlayFabId;
    }
    #endregion

    #region 가상화폐 증가
    #region 동기 로직
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
                DebugLogger.Log($"{virtualCurrencyName}가 {amount}만큼 증가 완료" +
                    $"\n 총 금액 : {result.Balance}");
            }, 
            (error) => DebugLogger.Log("가상화폐 증가 실패"));
    }
    #endregion

    #region 비동기 로직
    public Task<bool> AddUserVirtualCurrencyAsync(UIGameResources uiGameResources, string virtualCurrencyName, int amount)
    {
        var tcs = new TaskCompletionSource<bool>();
        var request = new AddUserVirtualCurrencyRequest
        {
            VirtualCurrency = virtualCurrencyName,
            Amount = amount
        };

        PlayFabClientAPI.AddUserVirtualCurrency(request, 
            (result) => {
                tcs.SetResult(true);
                DisplayGameResources(uiGameResources, virtualCurrencyName);
                DebugLogger.Log($"{virtualCurrencyName}가 {amount}만큼 증가 완료" +
                    $"\n 총 금액 : {result.Balance}");
            }, 
            (error) => {
                tcs.SetResult(false);
                DebugLogger.Log("가상화폐 증가 실패");
            });

        return tcs.Task;
    }
    #endregion
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
                DebugLogger.Log($"{virtualCurrencyName}가 {amount}만큼 감소 완료" +
                    $"\n 총 금액 : {result.Balance}");
            },
            (error) => DebugLogger.Log($"가상화폐 감소 실패: {error.GenerateErrorReport()}"));
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
        public string tierName;
        public int cardCount;

        public DrawCharacterData(string displayName, string tierName, int cardCount)
        {
            this.displayName = displayName;
            this.tierName = tierName;
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
    //        else DebugLogger.DebugLog("유저가 보유한 가상 화폐의 수량이 부족");
    //    },
    //    (error) => { DebugLogger.DebugLog("유저 인벤토리 획득 실패"); });
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
    //    (error) => DebugLogger.DebugLog("가상화폐 감소 실패"));
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
    //    (error) => { DebugLogger.DebugLog("상점 불러오기 실패"); });
    //}

    //private void GrantItemsToUser(Dictionary<string, DrawCharacterData> dicDrawCharacterData, List<string> itemIds)
    //{
    //    PlayFab.ServerModels.GrantItemsToUserRequest request = new PlayFab.ServerModels.GrantItemsToUserRequest()
    //    {
    //        PlayFabId = curPlayfabId,
    //        ItemIds = itemIds
    //    };

    //    PlayFabServerAPI.GrantItemsToUser(request, result => GetUserInventory(dicDrawCharacterData, itemIds), (error) => { DebugLogger.DebugLog("유저에게 아이템 주기 실패"); });
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
    //    (error) => { DebugLogger.DebugLog("유저 인벤토리 획득 실패"); });
    //}
    #endregion

    #region 비동기 로직 - 사용 (feat.최적화 필요) 
    public async Task DrawCharactersAsync(UIGameResources uiGameResources, UIPurchaseDrawCharacters uiPurchaseVirtualCurrency, string virtualCurrencyName, int amount, string catalogVersion, int drawCount)
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
            DebugLogger.Log("유저가 보유한 가상 화폐의 수량이 부족합니다.");
            return;
        }

        //가상화폐 차감
        var subtractTask = SubtractUserVirtualCurrencyAsync(virtualCurrencyName, amount);
        await subtractTask;
        if (!subtractTask.Result)
        {
            DebugLogger.Log("가상 화폐 차감 실패");
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
            DebugLogger.Log("아이템 지급 실패");
            return;
        }
        endTime = Time.realtimeSinceStartup;
        DebugLogger.Log($"Step - Grant Items: {endTime - startTime} seconds '여기가 문제다...'");

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
                DebugLogger.Log($"유저 인벤토리 획득 실패: {error.GenerateErrorReport()}");
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
                DebugLogger.Log($"가상 화폐 차감 실패: {error.GenerateErrorReport()}");
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
                    if (catalogItem.DisplayName == CharacterDisplayName.주몽.ToString())
                        characterDatas.Add(new RandomCharacter.CharacterData(weightLegendaryJumong, catalogItem.DisplayName, catalogItem.ItemId));
                    else if (catalogItem.DisplayName == CharacterDisplayName.이순신.ToString())
                        characterDatas.Add(new RandomCharacter.CharacterData(weightLegendaryAdmiralYi, catalogItem.DisplayName, catalogItem.ItemId));
                    else if (catalogItem.DisplayName == CharacterDisplayName.유일한.ToString())
                        characterDatas.Add(new RandomCharacter.CharacterData(weightUniqueCharacter, catalogItem.DisplayName, catalogItem.ItemId));
                    else if (catalogItem.DisplayName == CharacterDisplayName.희귀한.ToString())
                        characterDatas.Add(new RandomCharacter.CharacterData(weightRareCharacter, catalogItem.DisplayName, catalogItem.ItemId));
                    else if (catalogItem.DisplayName == CharacterDisplayName.안흔한.ToString())
                        characterDatas.Add(new RandomCharacter.CharacterData(weightUncommonCharacter, catalogItem.DisplayName, catalogItem.ItemId));
                    #region Tier: 흔한
                    else if (catalogItem.DisplayName == CharacterDisplayName.검병.ToString())
                        characterDatas.Add(new RandomCharacter.CharacterData(weightCommonCharacter, catalogItem.DisplayName, catalogItem.ItemId));
                    else if (catalogItem.DisplayName == CharacterDisplayName.창병.ToString())
                        characterDatas.Add(new RandomCharacter.CharacterData(weightCommonCharacter, catalogItem.DisplayName, catalogItem.ItemId));
                    else if (catalogItem.DisplayName == CharacterDisplayName.궁병.ToString())
                        characterDatas.Add(new RandomCharacter.CharacterData(weightCommonCharacter, catalogItem.DisplayName, catalogItem.ItemId));
                    else if (catalogItem.DisplayName == CharacterDisplayName.보급병.ToString())
                        characterDatas.Add(new RandomCharacter.CharacterData(weightCommonCharacter, catalogItem.DisplayName, catalogItem.ItemId));
                    else if (catalogItem.DisplayName == CharacterDisplayName.광전사.ToString())
                        characterDatas.Add(new RandomCharacter.CharacterData(weightCommonCharacter, catalogItem.DisplayName, catalogItem.ItemId));
                    else if (catalogItem.DisplayName == CharacterDisplayName.군사.ToString())
                        characterDatas.Add(new RandomCharacter.CharacterData(weightCommonCharacter, catalogItem.DisplayName, catalogItem.ItemId));
                    else if (catalogItem.DisplayName == CharacterDisplayName.책사.ToString())
                        characterDatas.Add(new RandomCharacter.CharacterData(weightCommonCharacter, catalogItem.DisplayName, catalogItem.ItemId));
                    #endregion
                }
                tcs.SetResult(characterDatas);
            },
            error =>
            {
                DebugLogger.Log($"카탈로그 아이템 가져오기 실패: {error.GenerateErrorReport()}");
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
                    DebugLogger.Log($"유저에게 아이템 지급 실패: {error.GenerateErrorReport()}");
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
                        existingData.tierName,
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

    #region 신규 캐릭터 데이터 설정
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
        (error) => { DebugLogger.Log("상점 불러오기 실패"); });
    }

    private void GetUserData(List<string> displayNames)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest() { PlayFabId = curPlayfabId },
        result =>
        {
            var characterNames = Enum.GetNames(typeof(CharacterDisplayName));
            foreach (var name in characterNames)
            {
                if (result.Data.ContainsKey(name)) {
                    int curLevel = int.Parse(result.Data[name].Value);
                    InitCharacterLevelData(name, curLevel);
                }
                else {
                    int initLevel = 1;
                    UpdateUserData(name, initLevel);
                    InitCharacterLevelData(name, initLevel);
                }
            }
        },
        error => DebugLogger.Log("유저 데이터 불러오기 실패"));
    }

    private void UpdateUserData(string displayName, int level)
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest() {
            Data = new Dictionary<string, string>() { { displayName, level.ToString() } }
        },
        null,
        error => DebugLogger.Log("유저 데이터 저장 실패"));
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
        (error) => { DebugLogger.Log("유저 인벤토리 획득 실패"); });
    }
    #endregion

    #region UI 캐릭터 표시
    private Dictionary<string, StructCharacterCardData> dicLegendaryCharacterDatas = new Dictionary<string, StructCharacterCardData>();
    private Dictionary<string, StructCharacterCardData> dicUniqueCharacterDatas = new Dictionary<string, StructCharacterCardData>();
    private Dictionary<string, StructCharacterCardData> dicRareCharacterDatas = new Dictionary<string, StructCharacterCardData>();
    private Dictionary<string, StructCharacterCardData> dicUncommonCharacterDatas = new Dictionary<string, StructCharacterCardData>();
    private Dictionary<string, StructCharacterCardData> dicCommonCharacterDatas = new Dictionary<string, StructCharacterCardData>();
    private Dictionary<string, StructCharacterCardData> dicAllCharacterDatas = new Dictionary<string, StructCharacterCardData>();

    public struct StructCharacterCardData
    {
        public int Level;        //레벨
        public int quantity;     //카드 개수
        public int TierNum;      //티어 
        public string itemId;    //아이템 ID (DisplayName과 동일)
    }

    public void DisplayCharacterCard(UICharacter uiCharacter) => GetUserInventory(uiCharacter);

    private void GetUserInventory(UICharacter uiCharacter)
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),
        (result) =>
        {
            //Tier를 추가할 시 해당 딕셔너리 안에 Enum 및 딕셔너리 추가
            var allTierDictionaries = new Dictionary<string, Dictionary<string, StructCharacterCardData>> {
     { CharacterTier.전설적인.ToString(), new Dictionary<string, StructCharacterCardData>() },
    { CharacterTier.유일한.ToString(), new Dictionary<string, StructCharacterCardData>() },
    { CharacterTier.희귀한.ToString(), new Dictionary<string, StructCharacterCardData>() },
    { CharacterTier.안흔한.ToString(), new Dictionary<string, StructCharacterCardData>() },
    { CharacterTier.흔한.ToString(), new Dictionary<string, StructCharacterCardData>() },
};

            for (int i = 0; i < result.Inventory.Count; i++)
            {
                var item = result.Inventory[i];
                if (allTierDictionaries.ContainsKey(item.ItemClass))
                {
                    var targetDictionary = allTierDictionaries[item.ItemClass];
                    if (!targetDictionary.ContainsKey(item.DisplayName)) {
                        targetDictionary.Add(item.DisplayName, new StructCharacterCardData
                        {
                            Level = dicCharacterLevelDatas[item.DisplayName],
                            quantity = (int)item.RemainingUses,
                            TierNum = (int)Enum.Parse(typeof(CharacterTier), item.ItemClass),
                            itemId = item.DisplayName
                        });
                    }
                    else {
                        targetDictionary[item.DisplayName] = new StructCharacterCardData
                        {
                            Level = dicCharacterLevelDatas[item.DisplayName],
                            quantity = (int)item.RemainingUses,
                            TierNum = (int)Enum.Parse(typeof(CharacterTier), item.ItemClass),
                            itemId = item.DisplayName
                        };
                    }
                }
            }

            //캐릭터 카드가 0개일 경우 레벨1로 데이터 추가
            //단, 전설적인 Tier일 경우 레벨0으로 데이터 추가
            var displayNames = Enum.GetNames(typeof(CharacterDisplayName));
            foreach (var tier in allTierDictionaries) {
                var targetDictionary = tier.Value;
                foreach (var displayName in displayNames) {
                    if (!targetDictionary.ContainsKey(displayName)) {
                        int defaultLevel = tier.Key == CharacterTier.전설적인.ToString() ? 0 : 1;
                        targetDictionary.Add(displayName, new StructCharacterCardData
                        {
                            Level = defaultLevel,
                            quantity = 0,
                            TierNum = (int)Enum.Parse(typeof(CharacterTier), tier.Key),
                            itemId = displayName
                        });
                    }
                }
            }

            // ===== itemIdList를 유저에게 아이템 지급하는 로직을 호출해서 전달하기 ===== //
            // ===== itemIdList를 유저에게 아이템 지급하는 로직을 호출해서 전달하기 ===== //
            // ===== itemIdList를 유저에게 아이템 지급하는 로직을 호출해서 전달하기 ===== //
            //itemId 리스트 생성
            var itemIdList = new List<string>();
            foreach (var tier in allTierDictionaries) {
                foreach (var entry in tier.Value) {
                    itemIdList.Add(entry.Value.itemId);
                }
            }

            uiCharacter.DisplayCharacters(GetAllCharacterDatas());
        },
        (error) => { DebugLogger.Log("유저 인벤토리 획득 실패"); });
    }

    private Dictionary<string, StructCharacterCardData> GetAllCharacterDatas()
    {
        //Tier를 추가할 시 해당 리스트 안에 딕셔너리 추가
        var allCharacterDictionaries = new List<Dictionary<string, StructCharacterCardData>> {
            dicLegendaryCharacterDatas,
            dicUniqueCharacterDatas,
            dicRareCharacterDatas,
            dicUncommonCharacterDatas,
            dicCommonCharacterDatas
        };

        foreach (var dictionary in allCharacterDictionaries) {
            foreach (var data in dictionary) {
                //키가 없으면 추가, 있으면 업데이트
                dicAllCharacterDatas[data.Key] = data.Value;  
                DebugLogger.Log($"키 : {data.Key}\n레벨 : {data.Value.Level}\n카드 보유량 : {data.Value.quantity}\n티어 : {data.Value.TierNum}\n아이템 ID : {data.Value.itemId}");
            }
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
            DebugLogger.Log("유저가 보유한 가상 화폐의 수량이 부족합니다.");
            return;
        }

        //가상화폐 차감
        var subtractTask = SubtractUserVirtualCurrencyAsync(subtractVirtualCurrencyName, price);
        await subtractTask;
        if (!subtractTask.Result)
        {
            DebugLogger.Log("가상 화폐 차감 실패");
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

        StructCharacterCardData data = dicAllCharacterDatas[displayName];
        int level = data.Level;  
        int remainingUses = data.quantity;
        int tierNum = data.TierNum;
        string itemId = data.itemId;
        DebugLogger.Log($"현재 선택된 캐릭터 (key) : {displayName}\n레벨 : {data.Level}\n카드 보유량 : {data.quantity}\n티어 : {data.TierNum}\n아이템 ID : {data.itemId}");

        //유저 인벤토리 병렬로 가져오기
        var inventoryTask = GetUserInventoryAsync();
        await Task.WhenAll(inventoryTask);
        var inventoryResult = inventoryTask.Result;

        //유저 인벤토리 확인
        if (inventoryResult == null ||
            !inventoryResult.VirtualCurrency.ContainsKey(subtractVirtualCurrencyName) ||
            inventoryResult.VirtualCurrency[subtractVirtualCurrencyName] < price)
        {
            DebugLogger.Log("유저가 보유한 가상 화폐의 수량이 부족합니다.");
            return;
        }
        else if (levelUpRemainingUses > remainingUses)
        {
            DebugLogger.Log("유저가 보유한 캐릭터 카드의 보유량 부족");
            return;
        }

        //가상화폐 차감
        var subtractTask = SubtractUserVirtualCurrencyAsync(subtractVirtualCurrencyName, price);
        await subtractTask;
        if (!subtractTask.Result)
        {
            DebugLogger.Log("가상 화폐 차감 실패");
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

        GetUserData(displayName, level, remainingUses, tierNum, itemId);
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
        (error) => { DebugLogger.Log("소모성 아이템 사용 실패"); });
    }

    private void GetUserData(string displayName, int level, int remainingUses, int tierNum, string itemId)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest() { PlayFabId = curPlayfabId },
        result =>
        {
            if (result.Data.ContainsKey(displayName))
            {
                SaveCharacterLevelData(displayName, remainingUses, tierNum, itemId);
                UpdateUserData(displayName, level);
            }
        },
        error => DebugLogger.Log("유저 데이터 불러오기 실패"));
    }

    private void SaveCharacterLevelData(string displayName, int remainingUses, int tierNum, string itemId)
    {
        if (dicCharacterLevelDatas.ContainsKey(displayName))
        {
            dicCharacterLevelDatas[displayName] += 1;
            dicAllCharacterDatas[displayName] = new StructCharacterCardData {
                Level = dicCharacterLevelDatas[displayName],
                quantity = remainingUses,
                TierNum = tierNum,
                itemId = itemId
            };
        }
        else return;

        DisplayCharacterCard(uiCharacter);

        int levelUpRemainingUses = DataManager.GetInstance().GetCharacterCardLevelQuentityData(dicCharacterLevelDatas[displayName], tierNum);
        uiCharacterCardDataPopup.Open(dicCharacterLevelDatas[displayName].ToString(), displayName, remainingUses, levelUpRemainingUses);
    }
    #endregion
}