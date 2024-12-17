using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;

public class PlayFabManager : MonoBehaviour
{
    public static PlayFabManager instance = null;

    public enum Characters { �ָ�, �̼���, ����, ������, �����, ������ }
    private enum CharacterTier { None, ����, ������, �����, ������, �������� }

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
                Debug.Log("����ȭ�� ���� �Ϸ� : " + result.Balance);
            }, 
            (error) => Debug.Log("����ȭ�� ���� ����"));
    }
    #endregion

    #region ĳ���� ���� �̱�
    private UIPurchaseVirtualCurrency uiPurchaseVirtualCurrency;
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

    public void DrawCharacters(UIGameResources uiGameResources, UIPurchaseVirtualCurrency uiPurchaseVirtualCurrency, string virtualCurrencyName, int amount, string catalogVersion, int drawCount)
        => GetUserInventory(uiGameResources, uiPurchaseVirtualCurrency, virtualCurrencyName, amount, catalogVersion, drawCount);

    private void GetUserInventory(UIGameResources uiGameResources, UIPurchaseVirtualCurrency uiPurchaseVirtualCurrency, string virtualCurrencyName, int amount, string catalogVersion, int drawCount)
    {
        this.uiPurchaseVirtualCurrency = uiPurchaseVirtualCurrency;

        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), (result) =>
        {
            if (result.VirtualCurrency[virtualCurrencyName] >= amount) SubtractUserVirtualCurrency(uiGameResources, virtualCurrencyName, amount, catalogVersion, drawCount);
            else Debug.Log("������ ������ ���� ȭ���� ������ ����");
        },
        (error) => { Debug.Log("���� �κ��丮 ȹ�� ����"); });
    }

    private void SubtractUserVirtualCurrency(UIGameResources uiGameResources, string virtualCurrencyName, int amount, string catalogVersion, int drawCount)
    {
        SubtractUserVirtualCurrencyRequest request = new SubtractUserVirtualCurrencyRequest
        {
            VirtualCurrency = virtualCurrencyName,
            Amount = amount
        };

        PlayFabClientAPI.SubtractUserVirtualCurrency(request,
        (result) => {
            GetCatalogItems(catalogVersion, drawCount);
            DisplayGameResources(uiGameResources, virtualCurrencyName);
        },
        (error) => Debug.Log("����ȭ�� ���� ����"));
    }

    private void GetCatalogItems(string catalogVersion, int drawCount)
    {
        List<RandomCharacter.CharacterData> characterDatas = new List<RandomCharacter.CharacterData>();

        PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest() { CatalogVersion = catalogVersion }, (result) =>
        {
            for (int i = 0; i < result.Catalog.Count; i++)
            {
                CatalogItem CatalogItem = result.Catalog[i];
                //Legendary
                if (CatalogItem.DisplayName == Characters.�ָ�.ToString()) characterDatas.Add(new RandomCharacter.CharacterData(weightLegendaryJumong, CatalogItem.DisplayName, CatalogItem.ItemId));
                else if (CatalogItem.DisplayName == Characters.�̼���.ToString()) characterDatas.Add(new RandomCharacter.CharacterData(weightLegendaryAdmiralYi, CatalogItem.DisplayName, CatalogItem.ItemId));
                //Unique
                else if (CatalogItem.DisplayName == Characters.������.ToString()) characterDatas.Add(new RandomCharacter.CharacterData(weightUniqueCharacter, CatalogItem.DisplayName, CatalogItem.ItemId));
                //Rare
                else if (CatalogItem.DisplayName == Characters.�����.ToString()) characterDatas.Add(new RandomCharacter.CharacterData(weightRareCharacter, CatalogItem.DisplayName, CatalogItem.ItemId));
                //Uncommon
                else if (CatalogItem.DisplayName == Characters.������.ToString()) characterDatas.Add(new RandomCharacter.CharacterData(weightUncommonCharacter, CatalogItem.DisplayName, CatalogItem.ItemId));
                //Common
                else if (CatalogItem.DisplayName == Characters.����.ToString()) characterDatas.Add(new RandomCharacter.CharacterData(weightCommonCharacter, CatalogItem.DisplayName, CatalogItem.ItemId));
            }

            Dictionary<string, int> dicDrawCharacterDatas = new Dictionary<string, int>();
            List<string> itemIds = new List<string>();
            Dictionary<string, DrawCharacterData> dicDrawCharacterData = new Dictionary<string, DrawCharacterData>();
            for (int i = 0; i < drawCount; i++)
            {
                RandomCharacter.CharacterData characterData = RandomCharacter.DrawRandomCharacter(characterDatas);
                itemIds.Add(characterData.itemId);
            }

            GrantItemsToUser(dicDrawCharacterData, itemIds);
        },
        (error) => { Debug.Log("���� �ҷ����� ����"); });
    }

    private void GrantItemsToUser(Dictionary<string, DrawCharacterData> dicDrawCharacterData, List<string> itemIds)
    {
        PlayFab.ServerModels.GrantItemsToUserRequest request = new PlayFab.ServerModels.GrantItemsToUserRequest()
        {
            PlayFabId = curPlayfabId,
            ItemIds = itemIds
        };

        PlayFabServerAPI.GrantItemsToUser(request, result => GetUserInventory(dicDrawCharacterData, itemIds), (error) => { Debug.Log("�������� ������ �ֱ� ����"); });
    }

    private void GetUserInventory(Dictionary<string, DrawCharacterData> dicDrawCharacterData, List<string> itemIds)
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),
        (result) => {
            for (int i = 0; i < result.Inventory.Count; i++)
            {
                for (int j = 0; j < itemIds.Count; j++)
                {
                    if (result.Inventory[i].ItemId == itemIds[j])
                    {
                        DrawCharacterData drawCharacterData = new DrawCharacterData(result.Inventory[i].DisplayName, result.Inventory[i].ItemClass, 1);
                        if (dicDrawCharacterData.ContainsKey(drawCharacterData.displayName)) dicDrawCharacterData[drawCharacterData.displayName] = new DrawCharacterData(drawCharacterData.displayName, drawCharacterData.itemClass, dicDrawCharacterData[drawCharacterData.displayName].cardCount + 1);
                        else dicDrawCharacterData.Add(drawCharacterData.displayName, drawCharacterData);
                    }
                }
            }

            uiPurchaseVirtualCurrency.DisplayDrawCharacters(dicDrawCharacterData);
        },
        (error) => { Debug.Log("���� �κ��丮 ȹ�� ����"); });
    }
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

    #region UI ĳ���� ī�� ������
    private UICharacter uiCharacter;
    private UICharacterCardDataPopup uiCharacterCardDataPopup;
    private Dictionary<string, int> dicCharacterLevelDatas = new Dictionary<string, int>();

    public void UpgradeCharacterCard(string displayName, UICharacterCardDataPopup uiCharacterCardDataPopup, UICharacter uiCharacter) => GetUserInventory(displayName, uiCharacterCardDataPopup, uiCharacter);

    private void GetUserInventory(string displayName, UICharacterCardDataPopup uiCharacterCardDataPopup, UICharacter uiCharacter)
    {
        this.uiCharacter = uiCharacter;
        this.uiCharacterCardDataPopup = uiCharacterCardDataPopup;

        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),
        (result) =>
        {
            int[] values = dicAllCharacterDatas[displayName];
            int level = values[0];
            int remainingUses = values[1];
            int tier = values[2];
            Debug.Log("���� ���õ� ĳ����" + "\n Ű : " + displayName + "\n ���� : " + values[0] + "\n ī�� ������ : " + values[1] + "\n Ƽ�� : " + values[2]);

            int levelUpRemainingUses = DataManager.instance.GetCharacterCardLevelQuentityData(level, tier);
            Debug.Log("������ �䱸�� : " + levelUpRemainingUses);

            for (int i = 0; i < result.Inventory.Count; i++)
            {
                if (result.Inventory[i].DisplayName == displayName)
                {
                    if (levelUpRemainingUses > remainingUses)
                    {
                        Debug.Log("ī�� ������ ����");
                        return;
                    }
                    
                    ConsumeItem(levelUpRemainingUses, result.Inventory[i].ItemInstanceId);

                    remainingUses -= levelUpRemainingUses;
                    level++;
                }
            }

            GetUserData(displayName, level, remainingUses, tier);
        },
        (error) => { Debug.Log("���� �κ��丮 ȹ�� ����"); });
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

        int levelUpRemainingUses = DataManager.instance.GetCharacterCardLevelQuentityData(dicCharacterLevelDatas[displayName], tier);
        uiCharacterCardDataPopup.Open(dicCharacterLevelDatas[displayName].ToString(), displayName, remainingUses, levelUpRemainingUses);
    }
    #endregion

    //// ======================================================================================================== //
    //// ======================================================================================================== //
    //// ======================================================================================================== //

    //public void PurchaseItem(string catalogVersion, string itemId, string virtualCurrency, int price)
    //{
    //    PurchaseItemRequest request = new PurchaseItemRequest()
    //    {
    //        CatalogVersion = catalogVersion,
    //        ItemId = itemId,
    //        VirtualCurrency = virtualCurrency,
    //        Price = price,
    //    };

    //    PlayFabClientAPI.PurchaseItem(request,
    //    (result) => { Debug.Log("������ ���� ����"); },
    //    (error) => { Debug.Log("������ ���� ���� : " + error.ErrorMessage); });
    //}

    //// ======================================================================================

    //public void GrantRandom(string playFabId, string tableId)
    //{
    //    PlayFab.ServerModels.EvaluateRandomResultTableRequest request = new PlayFab.ServerModels.EvaluateRandomResultTableRequest()
    //    {
    //        TableId = tableId
    //    };

    //    PlayFabServerAPI.EvaluateRandomResultTable(request, (result) => GrantItemsToUser(result, playFabId), OnGrantItemsToUserFailure);
    //}

    //private void GrantItemsToUser(PlayFab.ServerModels.EvaluateRandomResultTableResult tableResult, string playFabId)
    //{
    //    PlayFab.ServerModels.GrantItemsToUserRequest request = new PlayFab.ServerModels.GrantItemsToUserRequest()
    //    {
    //        PlayFabId = playFabId,
    //        ItemIds = new List<string> { tableResult.ResultItemId }
    //    };

    //    PlayFabServerAPI.GrantItemsToUser(request, (result) => OnGrantItemsToUserSuccess(result), (error) => OnGrantItemsToUserFailure(error));
    //}

    //private void OnGrantItemsToUserSuccess(PlayFab.ServerModels.GrantItemsToUserResult result)
    //{
    //    Debug.Log("�������� ������ �ֱ� ����");
    //}

    //public void OnGrantItemsToUserFailure(PlayFabError error)
    //{
    //    Debug.Log("�������� ������ �ֱ� ����");
    //}

    //// ======================================================================================

    //private void GrantCharacterToUser(string catalogVersion, string itemId, string setCharacterName)
    //{
    //    PlayFabClientAPI.GrantCharacterToUser(new GrantCharacterToUserRequest()
    //    {
    //        CatalogVersion = catalogVersion,
    //        ItemId = itemId,
    //        CharacterName = setCharacterName,
    //    },
    //    (result) => { Debug.Log("ĳ���� ���� ���� : " + result.CharacterId); },
    //    (error) => { Debug.Log("ĳ���� ���� ���� : " + error.ErrorMessage); });
    //}

    //// ======================================================================================
}