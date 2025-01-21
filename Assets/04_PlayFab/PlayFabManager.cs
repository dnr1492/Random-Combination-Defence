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

    public enum CharacterDisplayName { �˺�, â��, �ú�, ���޺�, ������, ����, å��, ������, �����, ������, �ָ�, �̼��� }
    public enum CharacterTier { None, ����, ������, �����, ������, �������� }

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

    #region ����ȭ�� ����
    #region ���� ����
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
                DebugLogger.Log($"{virtualCurrencyName}�� {amount}��ŭ ���� �Ϸ�" +
                    $"\n �� �ݾ� : {result.Balance}");
            }, 
            (error) => DebugLogger.Log("����ȭ�� ���� ����"));
    }
    #endregion

    #region �񵿱� ����
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
    #endregion

    #region ����ȭ�� ����
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
                DebugLogger.Log($"{virtualCurrencyName}�� {amount}��ŭ ���� �Ϸ�" +
                    $"\n �� �ݾ� : {result.Balance}");
            },
            (error) => DebugLogger.Log($"����ȭ�� ���� ����: {error.GenerateErrorReport()}"));
    }
    #endregion

    #region ĳ���� ���� �̱�
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

    #region ���� ���� - �̻��
    //public void DrawCharacters(UIGameResources uiGameResources, UIPurchaseVirtualCurrency uiPurchaseVirtualCurrency, string virtualCurrencyName, int amount, string catalogVersion, int drawCount)
    //    => GetUserInventory(uiGameResources, uiPurchaseVirtualCurrency, virtualCurrencyName, amount, catalogVersion, drawCount);

    //private void GetUserInventory(UIGameResources uiGameResources, UIPurchaseVirtualCurrency uiPurchaseVirtualCurrency, string virtualCurrencyName, int amount, string catalogVersion, int drawCount)
    //{
    //    this.uiPurchaseVirtualCurrency = uiPurchaseVirtualCurrency;

    //    PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), (result) =>
    //    {
    //        if (result.VirtualCurrency[virtualCurrencyName] >= amount) SubtractUserVirtualCurrency(uiGameResources, virtualCurrencyName, amount, catalogVersion, drawCount);
    //        else DebugLogger.DebugLog("������ ������ ���� ȭ���� ������ ����");
    //    },
    //    (error) => { DebugLogger.DebugLog("���� �κ��丮 ȹ�� ����"); });
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
    //    (error) => DebugLogger.DebugLog("����ȭ�� ���� ����"));
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
    //            if (CatalogItem.DisplayName == Characters.�ָ�.ToString()) characterDatas.Add(new RandomCharacter.CharacterData(weightLegendaryJumong, CatalogItem.DisplayName, CatalogItem.ItemId));
    //            else if (CatalogItem.DisplayName == Characters.�̼���.ToString()) characterDatas.Add(new RandomCharacter.CharacterData(weightLegendaryAdmiralYi, CatalogItem.DisplayName, CatalogItem.ItemId));
    //            //Unique
    //            else if (CatalogItem.DisplayName == Characters.������.ToString()) characterDatas.Add(new RandomCharacter.CharacterData(weightUniqueCharacter, CatalogItem.DisplayName, CatalogItem.ItemId));
    //            //Rare
    //            else if (CatalogItem.DisplayName == Characters.�����.ToString()) characterDatas.Add(new RandomCharacter.CharacterData(weightRareCharacter, CatalogItem.DisplayName, CatalogItem.ItemId));
    //            //Uncommon
    //            else if (CatalogItem.DisplayName == Characters.������.ToString()) characterDatas.Add(new RandomCharacter.CharacterData(weightUncommonCharacter, CatalogItem.DisplayName, CatalogItem.ItemId));
    //            //Common
    //            else if (CatalogItem.DisplayName == Characters.����.ToString()) characterDatas.Add(new RandomCharacter.CharacterData(weightCommonCharacter, CatalogItem.DisplayName, CatalogItem.ItemId));
    //        }

    //        Dictionary<string, int> dicDrawCharacterDatas = new Dictionary<string, int>();
    //        List<string> itemIds = new List<string>();
    //        Dictionary<string, DrawCharacterData> dicDrawCharacterData = new Dictionary<string, DrawCharacterData>();  //���⼭ �ʱ�ȭ���� �ʰ� GetUserInventory()���� �ʱ�ȭ�ϸ� ���� �ʳ�?
    //        for (int i = 0; i < drawCount; i++)
    //        {
    //            RandomCharacter.CharacterData characterData = RandomCharacter.DrawRandomCharacter(characterDatas);
    //            itemIds.Add(characterData.itemId);
    //        }

    //        GrantItemsToUser(dicDrawCharacterData, itemIds);
    //    },
    //    (error) => { DebugLogger.DebugLog("���� �ҷ����� ����"); });
    //}

    //private void GrantItemsToUser(Dictionary<string, DrawCharacterData> dicDrawCharacterData, List<string> itemIds)
    //{
    //    PlayFab.ServerModels.GrantItemsToUserRequest request = new PlayFab.ServerModels.GrantItemsToUserRequest()
    //    {
    //        PlayFabId = curPlayfabId,
    //        ItemIds = itemIds
    //    };

    //    PlayFabServerAPI.GrantItemsToUser(request, result => GetUserInventory(dicDrawCharacterData, itemIds), (error) => { DebugLogger.DebugLog("�������� ������ �ֱ� ����"); });
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
    //    (error) => { DebugLogger.DebugLog("���� �κ��丮 ȹ�� ����"); });
    //}
    #endregion

    #region �񵿱� ���� - ��� (feat.����ȭ �ʿ�) 
    public async Task DrawCharactersAsync(UIGameResources uiGameResources, UIPurchaseDrawCharacters uiPurchaseVirtualCurrency, string virtualCurrencyName, int amount, string catalogVersion, int drawCount)
    {
        //���� �κ��丮�� īŻ�α� �����͸� �̱� ĳ���� �����ͷ� ��ȯ�Ͽ� ���ķ� ��������
        var inventoryTask = GetUserInventoryAsync();
        var catalogTask = GetCatalogItemToDrawCharacterDataAsync(catalogVersion);

        await Task.WhenAll(inventoryTask, catalogTask);

        var inventoryResult = inventoryTask.Result;
        var characterDatas = catalogTask.Result;
        
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
                var characterData = RandomCharacter.DrawRandomCharacter(characterDatas, randomValues[i]);
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
        UpdateInventoryAndUIAsync(uiGameResources, uiPurchaseVirtualCurrency, itemIds, virtualCurrencyName, inventoryResult);
    }

    //���� �κ��丮 �������� �񵿱�
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

    //����ȭ�� ���� �񵿱�
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

    //īŻ�α� �������� �̱� ĳ���� �����ͷ� ��ȯ�ؼ� �������� �񵿱�
    private Task<List<RandomCharacter.CharacterData>> GetCatalogItemToDrawCharacterDataAsync(string catalogVersion)
    {
        var tcs = new TaskCompletionSource<List<RandomCharacter.CharacterData>>();

        PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest { CatalogVersion = catalogVersion },
            result =>
            {
                var characterDatas = new List<RandomCharacter.CharacterData>();
                foreach (var catalogItem in result.Catalog)
                {
                    if (catalogItem.DisplayName == CharacterDisplayName.�ָ�.ToString())
                        characterDatas.Add(new RandomCharacter.CharacterData(weightLegendaryJumong, catalogItem.DisplayName, catalogItem.ItemId));
                    else if (catalogItem.DisplayName == CharacterDisplayName.�̼���.ToString())
                        characterDatas.Add(new RandomCharacter.CharacterData(weightLegendaryAdmiralYi, catalogItem.DisplayName, catalogItem.ItemId));
                    else if (catalogItem.DisplayName == CharacterDisplayName.������.ToString())
                        characterDatas.Add(new RandomCharacter.CharacterData(weightUniqueCharacter, catalogItem.DisplayName, catalogItem.ItemId));
                    else if (catalogItem.DisplayName == CharacterDisplayName.�����.ToString())
                        characterDatas.Add(new RandomCharacter.CharacterData(weightRareCharacter, catalogItem.DisplayName, catalogItem.ItemId));
                    else if (catalogItem.DisplayName == CharacterDisplayName.������.ToString())
                        characterDatas.Add(new RandomCharacter.CharacterData(weightUncommonCharacter, catalogItem.DisplayName, catalogItem.ItemId));
                    #region Tier: ����
                    else if (catalogItem.DisplayName == CharacterDisplayName.�˺�.ToString())
                        characterDatas.Add(new RandomCharacter.CharacterData(weightCommonCharacter, catalogItem.DisplayName, catalogItem.ItemId));
                    else if (catalogItem.DisplayName == CharacterDisplayName.â��.ToString())
                        characterDatas.Add(new RandomCharacter.CharacterData(weightCommonCharacter, catalogItem.DisplayName, catalogItem.ItemId));
                    else if (catalogItem.DisplayName == CharacterDisplayName.�ú�.ToString())
                        characterDatas.Add(new RandomCharacter.CharacterData(weightCommonCharacter, catalogItem.DisplayName, catalogItem.ItemId));
                    else if (catalogItem.DisplayName == CharacterDisplayName.���޺�.ToString())
                        characterDatas.Add(new RandomCharacter.CharacterData(weightCommonCharacter, catalogItem.DisplayName, catalogItem.ItemId));
                    else if (catalogItem.DisplayName == CharacterDisplayName.������.ToString())
                        characterDatas.Add(new RandomCharacter.CharacterData(weightCommonCharacter, catalogItem.DisplayName, catalogItem.ItemId));
                    else if (catalogItem.DisplayName == CharacterDisplayName.����.ToString())
                        characterDatas.Add(new RandomCharacter.CharacterData(weightCommonCharacter, catalogItem.DisplayName, catalogItem.ItemId));
                    else if (catalogItem.DisplayName == CharacterDisplayName.å��.ToString())
                        characterDatas.Add(new RandomCharacter.CharacterData(weightCommonCharacter, catalogItem.DisplayName, catalogItem.ItemId));
                    #endregion
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

    //�������� ������ ���� �񵿱�
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

    //������ �κ��丮 �� UI ����
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

        //UI ������Ʈ
        uiPurchaseVirtualCurrency.DisplayDrawCharacters(dicDrawCharacterData);
        DisplayGameResources(uiGameResources, virtualCurrencyName);
    }
    #endregion
    #endregion

    #region �ű� ĳ���� ������ ����
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
        (error) => { DebugLogger.Log("���� �ҷ����� ����"); });
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

    #region UI ĳ���� ǥ��
    private Dictionary<string, StructCharacterCardData> dicLegendaryCharacterDatas = new Dictionary<string, StructCharacterCardData>();
    private Dictionary<string, StructCharacterCardData> dicUniqueCharacterDatas = new Dictionary<string, StructCharacterCardData>();
    private Dictionary<string, StructCharacterCardData> dicRareCharacterDatas = new Dictionary<string, StructCharacterCardData>();
    private Dictionary<string, StructCharacterCardData> dicUncommonCharacterDatas = new Dictionary<string, StructCharacterCardData>();
    private Dictionary<string, StructCharacterCardData> dicCommonCharacterDatas = new Dictionary<string, StructCharacterCardData>();
    private Dictionary<string, StructCharacterCardData> dicAllCharacterDatas = new Dictionary<string, StructCharacterCardData>();

    public struct StructCharacterCardData
    {
        public int Level;        //����
        public int quantity;     //ī�� ����
        public int TierNum;      //Ƽ�� 
        public string itemId;    //������ ID (DisplayName�� ����)
    }

    public void DisplayCharacterCard(UICharacter uiCharacter) => GetUserInventory(uiCharacter);

    private void GetUserInventory(UICharacter uiCharacter)
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),
        (result) =>
        {
            //Tier�� �߰��� �� �ش� ��ųʸ� �ȿ� Enum �� ��ųʸ� �߰�
            var allTierDictionaries = new Dictionary<string, Dictionary<string, StructCharacterCardData>> {
     { CharacterTier.��������.ToString(), new Dictionary<string, StructCharacterCardData>() },
    { CharacterTier.������.ToString(), new Dictionary<string, StructCharacterCardData>() },
    { CharacterTier.�����.ToString(), new Dictionary<string, StructCharacterCardData>() },
    { CharacterTier.������.ToString(), new Dictionary<string, StructCharacterCardData>() },
    { CharacterTier.����.ToString(), new Dictionary<string, StructCharacterCardData>() },
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

            //ĳ���� ī�尡 0���� ��� ����1�� ������ �߰�
            //��, �������� Tier�� ��� ����0���� ������ �߰�
            var displayNames = Enum.GetNames(typeof(CharacterDisplayName));
            foreach (var tier in allTierDictionaries) {
                var targetDictionary = tier.Value;
                foreach (var displayName in displayNames) {
                    if (!targetDictionary.ContainsKey(displayName)) {
                        int defaultLevel = tier.Key == CharacterTier.��������.ToString() ? 0 : 1;
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

            // ===== itemIdList�� �������� ������ �����ϴ� ������ ȣ���ؼ� �����ϱ� ===== //
            // ===== itemIdList�� �������� ������ �����ϴ� ������ ȣ���ؼ� �����ϱ� ===== //
            // ===== itemIdList�� �������� ������ �����ϴ� ������ ȣ���ؼ� �����ϱ� ===== //
            //itemId ����Ʈ ����
            var itemIdList = new List<string>();
            foreach (var tier in allTierDictionaries) {
                foreach (var entry in tier.Value) {
                    itemIdList.Add(entry.Value.itemId);
                }
            }

            uiCharacter.DisplayCharacters(GetAllCharacterDatas());
        },
        (error) => { DebugLogger.Log("���� �κ��丮 ȹ�� ����"); });
    }

    private Dictionary<string, StructCharacterCardData> GetAllCharacterDatas()
    {
        //Tier�� �߰��� �� �ش� ����Ʈ �ȿ� ��ųʸ� �߰�
        var allCharacterDictionaries = new List<Dictionary<string, StructCharacterCardData>> {
            dicLegendaryCharacterDatas,
            dicUniqueCharacterDatas,
            dicRareCharacterDatas,
            dicUncommonCharacterDatas,
            dicCommonCharacterDatas
        };

        foreach (var dictionary in allCharacterDictionaries) {
            foreach (var data in dictionary) {
                //Ű�� ������ �߰�, ������ ������Ʈ
                dicAllCharacterDatas[data.Key] = data.Value;  
                DebugLogger.Log($"Ű : {data.Key}\n���� : {data.Value.Level}\nī�� ������ : {data.Value.quantity}\nƼ�� : {data.Value.TierNum}\n������ ID : {data.Value.itemId}");
            }
        }

        return dicAllCharacterDatas;
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

        AddUserVirtualCurrency(uiGold, addVirtualCurrencyName, amount);
        DisplayGameResources(uiJewel, subtractVirtualCurrencyName);
    }
    #endregion

    #region ���� ĳ���� ī�� ������ - �񵿱�
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
        DebugLogger.Log($"���� ���õ� ĳ���� (key) : {displayName}\n���� : {data.Level}\nī�� ������ : {data.quantity}\nƼ�� : {data.TierNum}\n������ ID : {data.itemId}");

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