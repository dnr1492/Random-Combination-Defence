using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Threading.Tasks;
using System.Linq;

public class PlayFabManager : MonoBehaviour
{
    #region *유저 인벤토리 가져오기 - 비동기
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
    #endregion

    #region *유저에게 아이템 지급 - 비동기
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
    #endregion

    #region *가상화폐 증가 - 비동기
    private Task AddUserVirtualCurrencyAsync(UIGameResources uiGameResources, string virtualCurrencyName, int amount)
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

    #region *가상화폐 감소 - 비동기
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
    #endregion

    public static PlayFabManager instance = null;

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

    #region 캐릭터 랜덤 뽑기 - 비동기 (feat.최적화 필요) 
    private readonly float weightCommonCharacter = 900f;  //70.423%
    private readonly float weightUncommonCharacter = 300f;  //23.474%
    private readonly float weightRareCharacter = 50f;  //3.912%
    private readonly float weightUniqueCharacter = 25f;  //1.956%
    private readonly float weightLegendaryCharacter = 3.0f;  //0.235%

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

    public async Task DrawCharactersAsync(UIGameResources uiGameResources, UIPurchaseDrawCharacters uiPurchaseVirtualCurrency, string virtualCurrencyName, int amount, string catalogVersion, int drawCount)
    {
        //유저 인벤토리와 카탈로그 데이터를 뽑기 캐릭터 데이터로 변환하여 병렬로 가져오기
        var inventoryTask = GetUserInventoryAsync();
        var catalogTask = GetCatalogItemToDrawCharacterDataAsync(catalogVersion);

        await Task.WhenAll(inventoryTask, catalogTask);

        var inventoryResult = inventoryTask.Result;
        var allCatalogCharacterDatas = catalogTask.Result;
        
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
                var characterData = RandomCharacter.DrawRandomCharacter(allCatalogCharacterDatas, randomValues[i]);
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
        UpdateInventoryAndUIAsync(uiGameResources, uiPurchaseVirtualCurrency, itemIds, virtualCurrencyName, allCatalogCharacterDatas);
    }

    //카탈로그 아이템을 뽑기 캐릭터 데이터로 변환해서 가져오기 비동기
    private Task<List<RandomCharacter.CharacterData>> GetCatalogItemToDrawCharacterDataAsync(string catalogVersion)
    {
        var tcs = new TaskCompletionSource<List<RandomCharacter.CharacterData>>();

        PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest { CatalogVersion = catalogVersion },
            result =>
            {
                var characterDatas = new List<RandomCharacter.CharacterData>();

                //Enum에서 유효한 DisplayName 목록 가져오기
                var validDisplayNames = Enum.GetValues(typeof(CharacterDisplayName))
                    .Cast<CharacterDisplayName>()
                    .Select(name => name.ToString())
                    .ToHashSet();

                foreach (var catalogItem in result.Catalog)
                {
                    //현재 아이템의 DisplayName이 Enum에 없거나 이미 리스트에 존재하면 스킵
                    if (!validDisplayNames.Contains(catalogItem.DisplayName) ||
                        characterDatas.Exists(data => data.displayName == catalogItem.DisplayName))
                        continue;

                    //유효한 아이템을 리스트에 추가
                    characterDatas.Add(new RandomCharacter.CharacterData(
                        SetWeight(catalogItem.ItemClass),
                        catalogItem.DisplayName,
                        catalogItem.ItemId,
                        catalogItem.ItemClass
                    ));
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

    //Tier 가중치 설정
    private float SetWeight(string itemClass)
    {
        if (itemClass == CharacterTier.흔한.ToString()) return weightCommonCharacter;
        else if (itemClass == CharacterTier.안흔한.ToString()) return weightUncommonCharacter;
        else if (itemClass == CharacterTier.희귀한.ToString()) return weightRareCharacter;
        else if (itemClass == CharacterTier.유일한.ToString()) return weightUniqueCharacter;
        else if (itemClass == CharacterTier.전설적인.ToString()) return weightLegendaryCharacter;
        return 0;
    }

    //유저의 인벤토리 및 UI 갱신
    private void UpdateInventoryAndUIAsync(UIGameResources uiGameResources, UIPurchaseDrawCharacters uiPurchaseVirtualCurrency, List<string> itemIds, string virtualCurrencyName, List<RandomCharacter.CharacterData> allCatalogCharacterDatas)
    {
        var dicDrawCharacterData = new Dictionary<string, DrawCharacterData>();

        foreach (var itemId in itemIds)
        {
            var matchingItems = allCatalogCharacterDatas.Where(item => item.itemId == itemId);
            foreach (var item in matchingItems)
            {
                var drawCharacterData = new DrawCharacterData(item.displayName, item.itemClass, 1);
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

    #region 신규 캐릭터 데이터 설정
    public void InitUserData()
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

    #region UI 캐릭터 표시 - 비동기
    private Dictionary<string, StructCharacterCardData> dicAllCharacterCardDatas = new Dictionary<string, StructCharacterCardData>();
    private List<UICharacterCard> uiCharacterCards;

    public struct StructCharacterCardData
    {
        public int level;        //레벨
        public int quantity;     //카드 개수
        public int tierNum;      //티어 
        public string itemId;    //아이템 ID (DisplayName과 동일)
    }

    public async Task SetCharacterCardData(List<UICharacterCard> uiCharacterCards)
    {
        this.uiCharacterCards = uiCharacterCards;

        var result = await GetUserInventoryAsync();
        if (result == null)
        {
            DebugLogger.Log("유저 인벤토리 획득 실패");
            return;
        }

        var characterNames = Enum.GetNames(typeof(CharacterDisplayName));
        foreach (var name in characterNames)
        {
            var item = result.Inventory.FirstOrDefault(i => i.DisplayName == name);

            //유저 인벤토리에 캐릭터가 있는 경우
            if (item != null) 
            {
                var info = InfoManager.instance.LoadCharacterInfo(item.DisplayName);
                dicAllCharacterCardDatas[name] = new StructCharacterCardData
                {
                    level = info.level,
                    quantity = (int)item.RemainingUses,
                    tierNum = Enum.TryParse(typeof(CharacterTier), item.ItemClass, out var enumValue)
                        ? (int)enumValue
                        : 0,
                    itemId = item.ItemId
                };
            }
            //유저 인벤토리에 캐릭터가 없는 경우
            else
            {
                var info = InfoManager.instance.LoadCharacterInfo(name);
                dicAllCharacterCardDatas[name] = new StructCharacterCardData
                {
                    level = info.level,
                    quantity = 0,
                    tierNum = info.tierNum,
                    itemId = info.displayName
                };
            }
        }

        DisplayCharacterCard();
    }

    private void DisplayCharacterCard()
    {
        string displayName;
        int level;
        int quantity;
        int tierNum;
        string tierName = null;

        Sprite bgSprtie;
        Sprite bgOutlineSprite;
        Sprite imgCharacterSprite;

        int index = 0;
        foreach (var data in dicAllCharacterCardDatas)
        {
            if (index < uiCharacterCards.Count)
            {
                displayName = data.Key;
                level = data.Value.level;
                quantity = data.Value.quantity;
                tierNum = data.Value.tierNum;

                foreach (CharacterTier tier in Enum.GetValues(typeof(CharacterTier)))
                {
                    if ((int)tier == tierNum) tierName = tier.ToString();
                }

                bgSprtie = SpriteManager.GetInstance().GetSprite(SpriteType.Bg, tierName);
                bgOutlineSprite = SpriteManager.GetInstance().GetSprite(SpriteType.BgOutline, tierName);
                imgCharacterSprite = SpriteManager.GetInstance().GetSprite(SpriteType.ImgCharacter, displayName);

                uiCharacterCards[index].gameObject.SetActive(true);
                uiCharacterCards[index].Set(
                    displayName,
                    level,
                    quantity,
                    DataManager.GetInstance().GetCharacterCardLevelQuentityData(level, tierNum),
                    bgSprtie,
                    bgOutlineSprite,
                    imgCharacterSprite
                );
                index++;
            }
        }
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

        await AddUserVirtualCurrencyAsync(uiGold, addVirtualCurrencyName, amount);
        DisplayGameResources(uiJewel, subtractVirtualCurrencyName);
    }
    #endregion

    #region 골드로 캐릭터 카드 레벨업 - 비동기
    private UICharacterCardDataPopup uiCharacterCardDataPopup;
    private Dictionary<string, int> dicCharacterLevelDatas = new Dictionary<string, int>();

    public async Task UpgradeCharacterCardLevelToGoldAsync(UICharacterCardDataPopup uiCharacterCardDataPopup, UICharacter uiCharacter, UIGameResources uiGold, string displayName, int price, int levelUpRemainingUses)
    {
        this.uiCharacterCardDataPopup = uiCharacterCardDataPopup;

        string subtractVirtualCurrencyName = "GD";

        StructCharacterCardData data = dicAllCharacterCardDatas[displayName];
        int level = data.level;  
        int remainingUses = data.quantity;
        int tierNum = data.tierNum;
        string itemId = data.itemId;
        DebugLogger.Log($"현재 선택된 캐릭터 (key) : {displayName}\n레벨 : {data.level}\n카드 보유량 : {data.quantity}\n티어 : {data.tierNum}\n아이템 ID : {data.itemId}");

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
            dicAllCharacterCardDatas[displayName] = new StructCharacterCardData {
                level = dicCharacterLevelDatas[displayName],
                quantity = remainingUses,
                tierNum = tierNum,
                itemId = itemId
            };
        }
        else return;

        DisplayCharacterCard();

        int levelUpRemainingUses = DataManager.GetInstance().GetCharacterCardLevelQuentityData(dicCharacterLevelDatas[displayName], tierNum);
        uiCharacterCardDataPopup.Open(dicCharacterLevelDatas[displayName].ToString(), displayName, remainingUses, levelUpRemainingUses);
    }
    #endregion

    #region 현금으로 Gem 구매 - 비동기
    public async Task PurchaseGemToCashAsync(UIGameResources uiGameResources, string virtualCurrencyName, int amount)
    {
        await AddUserVirtualCurrencyAsync(uiGameResources, virtualCurrencyName, amount);
    }
    #endregion
}