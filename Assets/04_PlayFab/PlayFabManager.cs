using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Threading.Tasks;
using System.Linq;

public class PlayFabManager : MonoBehaviour
{
    #region *���� �κ��丮 �������� - �񵿱�
    private Task<GetUserInventoryResult> GetUserInventoryAsync()
    {
        var tcs = new TaskCompletionSource<GetUserInventoryResult>();

        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),
            result => tcs.SetResult(result),
            error =>
            {
                DebugLogger.Log($"���� �κ��丮 ȹ�� ����: {error.GenerateErrorReport()}");
                tcs.SetResult(null);
            });

        return tcs.Task;
    }
    #endregion

    #region *�������� ������ ���� - �񵿱�
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
                    DebugLogger.Log($"�������� ������ ���� ����: {error.GenerateErrorReport()}");
                    tcs.SetResult(false);
                });

        return tcs.Task;
    }
    #endregion

    #region *����ȭ�� ���� - �񵿱�
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
                DebugLogger.Log($"{virtualCurrencyName}�� {amount}��ŭ ���� �Ϸ�" +
                    $"\n �� �ݾ� : {result.Balance}");
            },
            (error) => {
                tcs.SetResult(false);
                DebugLogger.Log("����ȭ�� ���� ����");
            });

        return tcs.Task;
    }
    #endregion

    #region *����ȭ�� ���� - �񵿱�
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
                DebugLogger.Log($"���� ȭ�� ���� ����: {error.GenerateErrorReport()}");
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

    #region ȸ������
    public void Regist(string email, string password, string username)
    {
        RegisterPlayFabUserRequest request = new RegisterPlayFabUserRequest();
        request.Email = email;
        request.Password = password;
        request.Username = username;

        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, (error) => DebugLogger.Log("ȸ������ ���� : " + error.ErrorMessage));
    }

    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        DebugLogger.Log("ȸ������ ����");
    }
    #endregion

    #region �α���
    private bool isLoginSuccess = false;
    private string curPlayfabId = null;

    public void Login(string email, string password)
    {
        LoginWithEmailAddressRequest request = new LoginWithEmailAddressRequest();
        request.Email = email;
        request.Password = password;

        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, (error) => DebugLogger.Log("�α��� ���� : " + error.ErrorMessage));
    }

    public bool CheckLoginSuccess()
    {
        return isLoginSuccess;
    }

    private void OnLoginSuccess(LoginResult result)
    {
        DebugLogger.Log("�α��� ����");

        isLoginSuccess = true;
        curPlayfabId = result.PlayFabId;
    }
    #endregion

    #region ĳ���� ���� �̱� - �񵿱� (feat.����ȭ �ʿ�) 
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
        //���� �κ��丮�� īŻ�α� �����͸� �̱� ĳ���� �����ͷ� ��ȯ�Ͽ� ���ķ� ��������
        var inventoryTask = GetUserInventoryAsync();
        var catalogTask = GetCatalogItemToDrawCharacterDataAsync(catalogVersion);

        await Task.WhenAll(inventoryTask, catalogTask);

        var inventoryResult = inventoryTask.Result;
        var allCatalogCharacterDatas = catalogTask.Result;
        
        //���� �κ��丮 Ȯ��
        if (inventoryResult == null ||
            !inventoryResult.VirtualCurrency.ContainsKey(virtualCurrencyName) ||
            inventoryResult.VirtualCurrency[virtualCurrencyName] < amount)
        {
            DebugLogger.Log("������ ������ ���� ȭ���� ������ �����մϴ�.");
            return;
        }

        //����ȭ�� ����
        var subtractTask = SubtractUserVirtualCurrencyAsync(virtualCurrencyName, amount);
        await subtractTask;
        if (!subtractTask.Result)
        {
            DebugLogger.Log("���� ȭ�� ���� ����");
            return;
        }

        //ĳ���� ���� �̱� (���� ó��)
        //���� �����忡�� ���� ���� �̸� ����
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

        //�������� ������ ����
        // =============== *** ����ȭ ��� *** =============== //
        // =============== *** ����ȭ ��� *** =============== //
        // =============== *** ����ȭ ��� *** =============== //
        float startTime, endTime;
        startTime = Time.realtimeSinceStartup;
        bool isItemsGranted = await GrantItemsToUserAsync(itemIds);
        if (!isItemsGranted)
        {
            DebugLogger.Log("������ ���� ����");
            return;
        }
        endTime = Time.realtimeSinceStartup;
        DebugLogger.Log($"Step - Grant Items: {endTime - startTime} seconds '���Ⱑ ������...'");

        //���� �κ��丮 ���� �� UI ������Ʈ
        UpdateInventoryAndUIAsync(uiGameResources, uiPurchaseVirtualCurrency, itemIds, virtualCurrencyName, allCatalogCharacterDatas);
    }

    //īŻ�α� �������� �̱� ĳ���� �����ͷ� ��ȯ�ؼ� �������� �񵿱�
    private Task<List<RandomCharacter.CharacterData>> GetCatalogItemToDrawCharacterDataAsync(string catalogVersion)
    {
        var tcs = new TaskCompletionSource<List<RandomCharacter.CharacterData>>();

        PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest { CatalogVersion = catalogVersion },
            result =>
            {
                var characterDatas = new List<RandomCharacter.CharacterData>();

                //Enum���� ��ȿ�� DisplayName ��� ��������
                var validDisplayNames = Enum.GetValues(typeof(CharacterDisplayName))
                    .Cast<CharacterDisplayName>()
                    .Select(name => name.ToString())
                    .ToHashSet();

                foreach (var catalogItem in result.Catalog)
                {
                    //���� �������� DisplayName�� Enum�� ���ų� �̹� ����Ʈ�� �����ϸ� ��ŵ
                    if (!validDisplayNames.Contains(catalogItem.DisplayName) ||
                        characterDatas.Exists(data => data.displayName == catalogItem.DisplayName))
                        continue;

                    //��ȿ�� �������� ����Ʈ�� �߰�
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
                DebugLogger.Log($"īŻ�α� ������ �������� ����: {error.GenerateErrorReport()}");
                tcs.SetResult(null);
            });

        return tcs.Task;
    }

    //Tier ����ġ ����
    private float SetWeight(string itemClass)
    {
        if (itemClass == CharacterTier.����.ToString()) return weightCommonCharacter;
        else if (itemClass == CharacterTier.������.ToString()) return weightUncommonCharacter;
        else if (itemClass == CharacterTier.�����.ToString()) return weightRareCharacter;
        else if (itemClass == CharacterTier.������.ToString()) return weightUniqueCharacter;
        else if (itemClass == CharacterTier.��������.ToString()) return weightLegendaryCharacter;
        return 0;
    }

    //������ �κ��丮 �� UI ����
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

        //UI ������Ʈ
        uiPurchaseVirtualCurrency.DisplayDrawCharacters(dicDrawCharacterData);
        DisplayGameResources(uiGameResources, virtualCurrencyName);
    }
    #endregion

    #region �ű� ĳ���� ������ ����
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
        error => DebugLogger.Log("���� ������ �ҷ����� ����"));
    }

    private void UpdateUserData(string displayName, int level)
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest() {
            Data = new Dictionary<string, string>() { { displayName, level.ToString() } }
        },
        null,
        error => DebugLogger.Log("���� ������ ���� ����"));
    }

    private void InitCharacterLevelData(string displayName, int curLevel)
    {
        if (dicCharacterLevelDatas.ContainsKey(displayName)) dicCharacterLevelDatas[displayName] = curLevel;
        else dicCharacterLevelDatas.Add(displayName, curLevel);

        InfoManager.GetInstance().SaveCharacterInfo(displayName, curLevel);
    }
    #endregion

    #region UI ���� �ڿ� ǥ��
    public void DisplayGameResources(UIGameResources uiGameResources, string virtualCurrencyName)
        => GetUserInventory(uiGameResources, virtualCurrencyName);

    private void GetUserInventory(UIGameResources uiGameResources, string virtualCurrencyName)
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),
        (result) => {
            uiGameResources.DisplayGameResourceAmount(result.VirtualCurrency[virtualCurrencyName]);
        },
        (error) => { DebugLogger.Log("���� �κ��丮 ȹ�� ����"); });
    }
    #endregion

    #region UI ĳ���� ǥ�� - �񵿱�
    private Dictionary<string, StructCharacterCardData> dicAllCharacterCardDatas = new Dictionary<string, StructCharacterCardData>();
    private List<UICharacterCard> uiCharacterCards;

    public struct StructCharacterCardData
    {
        public int level;        //����
        public int quantity;     //ī�� ����
        public int tierNum;      //Ƽ�� 
        public string itemId;    //������ ID (DisplayName�� ����)
    }

    public async Task SetCharacterCardData(List<UICharacterCard> uiCharacterCards)
    {
        this.uiCharacterCards = uiCharacterCards;

        var result = await GetUserInventoryAsync();
        if (result == null)
        {
            DebugLogger.Log("���� �κ��丮 ȹ�� ����");
            return;
        }

        var characterNames = Enum.GetNames(typeof(CharacterDisplayName));
        foreach (var name in characterNames)
        {
            var item = result.Inventory.FirstOrDefault(i => i.DisplayName == name);

            //���� �κ��丮�� ĳ���Ͱ� �ִ� ���
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
            //���� �κ��丮�� ĳ���Ͱ� ���� ���
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

    #region �������� ��带 ���� - �񵿱�
    public async Task PurchaseJewelToGoldAsync(UIGameResources uiJewel, UIGameResources uiGold, int price, int amount)
    {
        string addVirtualCurrencyName = "GD";
        string subtractVirtualCurrencyName = "JE";

        //���� �κ��丮 ���ķ� ��������
        var inventoryTask = GetUserInventoryAsync();
        await Task.WhenAll(inventoryTask);
        var inventoryResult = inventoryTask.Result;

        //���� �κ��丮 Ȯ��
        if (inventoryResult == null ||
            !inventoryResult.VirtualCurrency.ContainsKey(subtractVirtualCurrencyName) ||
            inventoryResult.VirtualCurrency[subtractVirtualCurrencyName] < price)
        {
            DebugLogger.Log("������ ������ ���� ȭ���� ������ �����մϴ�.");
            return;
        }

        //����ȭ�� ����
        var subtractTask = SubtractUserVirtualCurrencyAsync(subtractVirtualCurrencyName, price);
        await subtractTask;
        if (!subtractTask.Result)
        {
            DebugLogger.Log("���� ȭ�� ���� ����");
            return;
        }

        await AddUserVirtualCurrencyAsync(uiGold, addVirtualCurrencyName, amount);
        DisplayGameResources(uiJewel, subtractVirtualCurrencyName);
    }
    #endregion

    #region ���� ĳ���� ī�� ������ - �񵿱�
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
        DebugLogger.Log($"���� ���õ� ĳ���� (key) : {displayName}\n���� : {data.level}\nī�� ������ : {data.quantity}\nƼ�� : {data.tierNum}\n������ ID : {data.itemId}");

        //���� �κ��丮 ���ķ� ��������
        var inventoryTask = GetUserInventoryAsync();
        await Task.WhenAll(inventoryTask);
        var inventoryResult = inventoryTask.Result;

        //���� �κ��丮 Ȯ��
        if (inventoryResult == null ||
            !inventoryResult.VirtualCurrency.ContainsKey(subtractVirtualCurrencyName) ||
            inventoryResult.VirtualCurrency[subtractVirtualCurrencyName] < price)
        {
            DebugLogger.Log("������ ������ ���� ȭ���� ������ �����մϴ�.");
            return;
        }
        else if (levelUpRemainingUses > remainingUses)
        {
            DebugLogger.Log("������ ������ ĳ���� ī���� ������ ����");
            return;
        }

        //����ȭ�� ����
        var subtractTask = SubtractUserVirtualCurrencyAsync(subtractVirtualCurrencyName, price);
        await subtractTask;
        if (!subtractTask.Result)
        {
            DebugLogger.Log("���� ȭ�� ���� ����");
            return;
        }

        //���� ĳ���� ī�� ������
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
        (error) => { DebugLogger.Log("�Ҹ� ������ ��� ����"); });
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
        error => DebugLogger.Log("���� ������ �ҷ����� ����"));
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

    #region �������� Gem ���� - �񵿱�
    public async Task PurchaseGemToCashAsync(UIGameResources uiGameResources, string virtualCurrencyName, int amount)
    {
        await AddUserVirtualCurrencyAsync(uiGameResources, virtualCurrencyName, amount);
    }
    #endregion
}