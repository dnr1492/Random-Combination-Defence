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

    public enum Characters { �ָ�, �̼���, ����, ������, �����, ������ }
    public enum CharacterTier { None, ����, ������, �����, ������, �������� }

    private void Awake()
    {
        if (instance == null)
        {
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

        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, (error) => Debug.Log("ȸ������ ���� : " + error.ErrorMessage));
    }

    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        Debug.Log("ȸ������ ����");
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

        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, (error) => Debug.Log("�α��� ���� : " + error.ErrorMessage));
    }

    public bool CheckLoginSuccess()
    {
        return isLoginSuccess;
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("�α��� ����");

        isLoginSuccess = true;
        curPlayfabId = result.PlayFabId;
    }
    #endregion

    #region ����ȭ�� ����
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
                Debug.Log($"{virtualCurrencyName}�� {amount}��ŭ ���� �Ϸ�" +
                    $"\n �� �ݾ� : {result.Balance}");
            }, 
            (error) => Debug.Log("����ȭ�� ���� ����"));
    }
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
                Debug.Log($"{virtualCurrencyName}�� {amount}��ŭ ���� �Ϸ�" +
                    $"\n �� �ݾ� : {result.Balance}");
            },
            (error) => Debug.Log($"����ȭ�� ���� ����: {error.GenerateErrorReport()}"));
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
        public string itemClass;
        public int cardCount;

        public DrawCharacterData(string displayName, string itemClass, int cardCount)
        {
            this.displayName = displayName;
            this.itemClass = itemClass;
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
    //        else Debug.Log("������ ������ ���� ȭ���� ������ ����");
    //    },
    //    (error) => { Debug.Log("���� �κ��丮 ȹ�� ����"); });
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
    //    (error) => Debug.Log("����ȭ�� ���� ����"));
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
    //    (error) => { Debug.Log("���� �ҷ����� ����"); });
    //}

    //private void GrantItemsToUser(Dictionary<string, DrawCharacterData> dicDrawCharacterData, List<string> itemIds)
    //{
    //    PlayFab.ServerModels.GrantItemsToUserRequest request = new PlayFab.ServerModels.GrantItemsToUserRequest()
    //    {
    //        PlayFabId = curPlayfabId,
    //        ItemIds = itemIds
    //    };

    //    PlayFabServerAPI.GrantItemsToUser(request, result => GetUserInventory(dicDrawCharacterData, itemIds), (error) => { Debug.Log("�������� ������ �ֱ� ����"); });
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
    //    (error) => { Debug.Log("���� �κ��丮 ȹ�� ����"); });
    //}
    #endregion

    #region �񵿱� ���� - ��� (feat.����ȭ / �ε� ���� �ʿ�) 
    public async void OnClickDrawCharactersAsync(UIGameResources uiGameResources, UIPurchaseDrawCharacters uiPurchaseVirtualCurrency, string virtualCurrencyName, int amount, string catalogVersion, int drawCount)
    {
        //loadingPanel.SetActive(true);

        await DrawCharactersAsync(uiGameResources, uiPurchaseVirtualCurrency, virtualCurrencyName, amount, catalogVersion, drawCount);

        //loadingPanel.SetActive(false);
    }

    private async Task DrawCharactersAsync(UIGameResources uiGameResources, UIPurchaseDrawCharacters uiPurchaseVirtualCurrency, string virtualCurrencyName, int amount, string catalogVersion, int drawCount)
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
            Debug.Log("������ ������ ���� ȭ���� ������ �����մϴ�.");
            return;
        }

        //����ȭ�� ����
        var subtractTask = SubtractUserVirtualCurrencyAsync(virtualCurrencyName, amount);
        await subtractTask;
        if (!subtractTask.Result)
        {
            Debug.LogError("���� ȭ�� ���� ����");
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
            Debug.LogError("������ ���� ����");
            return;
        }
        endTime = Time.realtimeSinceStartup;
        Debug.Log($"Step - Grant Items: {endTime - startTime} seconds '���Ⱑ ������...'");

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
                Debug.LogError($"���� �κ��丮 ȹ�� ����: {error.GenerateErrorReport()}");
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
                Debug.LogError($"���� ȭ�� ���� ����: {error.GenerateErrorReport()}");
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
                    if (catalogItem.DisplayName == Characters.�ָ�.ToString())
                        characterDatas.Add(new RandomCharacter.CharacterData(weightLegendaryJumong, catalogItem.DisplayName, catalogItem.ItemId));
                    else if (catalogItem.DisplayName == Characters.�̼���.ToString())
                        characterDatas.Add(new RandomCharacter.CharacterData(weightLegendaryAdmiralYi, catalogItem.DisplayName, catalogItem.ItemId));
                    else if (catalogItem.DisplayName == Characters.������.ToString())
                        characterDatas.Add(new RandomCharacter.CharacterData(weightUniqueCharacter, catalogItem.DisplayName, catalogItem.ItemId));
                    else if (catalogItem.DisplayName == Characters.�����.ToString())
                        characterDatas.Add(new RandomCharacter.CharacterData(weightRareCharacter, catalogItem.DisplayName, catalogItem.ItemId));
                    else if (catalogItem.DisplayName == Characters.������.ToString())
                        characterDatas.Add(new RandomCharacter.CharacterData(weightUncommonCharacter, catalogItem.DisplayName, catalogItem.ItemId));
                    else if (catalogItem.DisplayName == Characters.����.ToString())
                        characterDatas.Add(new RandomCharacter.CharacterData(weightCommonCharacter, catalogItem.DisplayName, catalogItem.ItemId));
                }
                tcs.SetResult(characterDatas);
            },
            error =>
            {
                Debug.LogError($"īŻ�α� ������ �������� ����: {error.GenerateErrorReport()}");
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
                    Debug.LogError($"�������� ������ ���� ����: {error.GenerateErrorReport()}");
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
                        existingData.itemClass,
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

    #region �ű� �÷��̾� ������ ����
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
        (error) => { Debug.Log("���� �ҷ����� ����"); });
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
        error => Debug.Log("���� ������ �ҷ����� ����"));
    }

    private void UpdateUserData(string displayName, int level)
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest() {
            Data = new Dictionary<string, string>() { { displayName, level.ToString() } }
        },
        null,
        error => Debug.Log("���� ������ ���� ����"));
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
        (error) => { Debug.Log("���� �κ��丮 ȹ�� ����"); });
    }
    #endregion

    #region UI ĳ���� ǥ��
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
                if (result.Inventory[i].ItemClass == CharacterTier.��������.ToString())
                {
                    if (!dicLegendaryCharacterDatas.ContainsKey(result.Inventory[i].DisplayName)) dicLegendaryCharacterDatas.Add(result.Inventory[i].DisplayName, new int[] { dicCharacterLevelDatas[result.Inventory[i].DisplayName], (int)result.Inventory[i].RemainingUses, (int)CharacterTier.�������� });
                    else dicLegendaryCharacterDatas[result.Inventory[i].DisplayName] = new int[] { dicCharacterLevelDatas[result.Inventory[i].DisplayName], (int)result.Inventory[i].RemainingUses, (int)CharacterTier.�������� };
                }
                if (result.Inventory[i].ItemClass == CharacterTier.������.ToString())
                {
                    if (!dicUniqueCharacterDatas.ContainsKey(result.Inventory[i].DisplayName)) dicUniqueCharacterDatas.Add(result.Inventory[i].DisplayName, new int[] { dicCharacterLevelDatas[result.Inventory[i].DisplayName], (int)result.Inventory[i].RemainingUses, (int)CharacterTier.������ });
                    else dicUniqueCharacterDatas[result.Inventory[i].DisplayName] = new int[] { dicCharacterLevelDatas[result.Inventory[i].DisplayName], (int)result.Inventory[i].RemainingUses, (int)CharacterTier.������ };
                }
                if (result.Inventory[i].ItemClass == CharacterTier.�����.ToString())
                {
                    if (!dicRareCharacterDatas.ContainsKey(result.Inventory[i].DisplayName)) dicRareCharacterDatas.Add(result.Inventory[i].DisplayName, new int[] { dicCharacterLevelDatas[result.Inventory[i].DisplayName], (int)result.Inventory[i].RemainingUses, (int)CharacterTier.����� });
                    else dicRareCharacterDatas[result.Inventory[i].DisplayName] = new int[] { dicCharacterLevelDatas[result.Inventory[i].DisplayName], (int)result.Inventory[i].RemainingUses, (int)CharacterTier.����� };
                }
                if (result.Inventory[i].ItemClass == CharacterTier.������.ToString())
                {
                    if (!dicUncommonCharacterDatas.ContainsKey(result.Inventory[i].DisplayName)) dicUncommonCharacterDatas.Add(result.Inventory[i].DisplayName, new int[] { dicCharacterLevelDatas[result.Inventory[i].DisplayName], (int)result.Inventory[i].RemainingUses, (int)CharacterTier.������ });
                    else dicUncommonCharacterDatas[result.Inventory[i].DisplayName] = new int[] { dicCharacterLevelDatas[result.Inventory[i].DisplayName], (int)result.Inventory[i].RemainingUses, (int)CharacterTier.������ };
                }
                if (result.Inventory[i].ItemClass == CharacterTier.����.ToString())
                {
                    if (!dicCommonCharacterDatas.ContainsKey(result.Inventory[i].DisplayName)) dicCommonCharacterDatas.Add(result.Inventory[i].DisplayName, new int[] { dicCharacterLevelDatas[result.Inventory[i].DisplayName], (int)result.Inventory[i].RemainingUses, (int)CharacterTier.���� });
                    else dicCommonCharacterDatas[result.Inventory[i].DisplayName] = new int[] { dicCharacterLevelDatas[result.Inventory[i].DisplayName], (int)result.Inventory[i].RemainingUses, (int)CharacterTier.���� };
                }
            }

            uiCharacter.DisplayCharacters(CombineDictionary());
        },
        (error) => { Debug.Log("���� �κ��丮 ȹ�� ����"); });
    }

    private Dictionary<string, int[]> CombineDictionary()
    {
        foreach (var data in dicLegendaryCharacterDatas)
        {
            if (!dicAllCharacterDatas.ContainsKey(data.Key)) dicAllCharacterDatas.Add(data.Key, data.Value);
            else dicAllCharacterDatas[data.Key] = data.Value;
            Debug.Log("Ű : " + data.Key + "\n ���� : " + data.Value[0] + "\n ī�� ������ : " + data.Value[1] + "\n Ƽ�� : " + data.Value[2]);
        }
        foreach (var data in dicUniqueCharacterDatas)
        {
            if (!dicAllCharacterDatas.ContainsKey(data.Key)) dicAllCharacterDatas.Add(data.Key, data.Value);
            else dicAllCharacterDatas[data.Key] = data.Value;
            Debug.Log("Ű : " + data.Key + "\n ���� : " + data.Value[0] + "\n ī�� ������ : " + data.Value[1] + "\n Ƽ�� : " + data.Value[2]);
        }
        foreach (var data in dicRareCharacterDatas)
        {
            if (!dicAllCharacterDatas.ContainsKey(data.Key)) dicAllCharacterDatas.Add(data.Key, data.Value);
            else dicAllCharacterDatas[data.Key] = data.Value;
            Debug.Log("Ű : " + data.Key + "\n ���� : " + data.Value[0] + "\n ī�� ������ : " + data.Value[1] + "\n Ƽ�� : " + data.Value[2]);
        }
        foreach (var data in dicUncommonCharacterDatas)
        {
            if (!dicAllCharacterDatas.ContainsKey(data.Key)) dicAllCharacterDatas.Add(data.Key, data.Value);
            else dicAllCharacterDatas[data.Key] = data.Value;
            Debug.Log("Ű : " + data.Key + "\n ���� : " + data.Value[0] + "\n ī�� ������ : " + data.Value[1] + "\n Ƽ�� : " + data.Value[2]);
        }
        foreach (var data in dicCommonCharacterDatas)
        {
            if (!dicAllCharacterDatas.ContainsKey(data.Key)) dicAllCharacterDatas.Add(data.Key, data.Value);
            else dicAllCharacterDatas[data.Key] = data.Value;
            Debug.Log("Ű : " + data.Key + "\n ���� : " + data.Value[0] + "\n ī�� ������ : " + data.Value[1] + "\n Ƽ�� : " + data.Value[2]);
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
            Debug.Log("������ ������ ���� ȭ���� ������ �����մϴ�.");
            return;
        }

        //����ȭ�� ����
        var subtractTask = SubtractUserVirtualCurrencyAsync(subtractVirtualCurrencyName, price);
        await subtractTask;
        if (!subtractTask.Result)
        {
            Debug.LogError("���� ȭ�� ���� ����");
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

        int[] values = dicAllCharacterDatas[displayName];
        int level = values[0];  
        int remainingUses = values[1];
        int tier = values[2];
        //Debug.Log("���� ���õ� ĳ����" + "\n Ű : " + displayName + "\n ���� : " + values[0] + "\n ī�� ������ : " + values[1] + "\n Ƽ�� : " + values[2]);

        //���� �κ��丮 ���ķ� ��������
        var inventoryTask = GetUserInventoryAsync();
        await Task.WhenAll(inventoryTask);
        var inventoryResult = inventoryTask.Result;

        //���� �κ��丮 Ȯ��
        if (inventoryResult == null ||
            !inventoryResult.VirtualCurrency.ContainsKey(subtractVirtualCurrencyName) ||
            inventoryResult.VirtualCurrency[subtractVirtualCurrencyName] < price)
        {
            Debug.Log("������ ������ ���� ȭ���� ������ �����մϴ�.");
            return;
        }
        else if (levelUpRemainingUses > remainingUses)
        {
            Debug.Log("������ ������ ĳ���� ī���� ������ ����");
            return;
        }

        //����ȭ�� ����
        var subtractTask = SubtractUserVirtualCurrencyAsync(subtractVirtualCurrencyName, price);
        await subtractTask;
        if (!subtractTask.Result)
        {
            Debug.LogError("���� ȭ�� ���� ����");
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
        (error) => { Debug.Log("�Ҹ� ������ ��� ����"); });
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
        error => Debug.Log("���� ������ �ҷ����� ����"));
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