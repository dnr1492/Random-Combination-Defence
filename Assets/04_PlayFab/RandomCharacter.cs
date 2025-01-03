using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RandomCharacter
{
    [Serializable]
    public class CharacterData
    {
        public float weight;  //CharacterData ��ü ���ο� ����ġ�� �����մϴ�.
        public string displayName;
        public string itemId;

        public CharacterData(float weight, string displayName, string itemId)
        {
            this.weight = weight;
            this.displayName = displayName;
            this.itemId = itemId;
        }
    }

    public static CharacterData DrawRandomCharacter(List<CharacterData> characterList, float randomValue)
    {
        ////CharacterData ��ü�� ��� ����Ʈ ���� ����
        //List<CharacterData> characterList = new List<CharacterData>
        //{
        //    new CharacterData(0.4f, "�ָ�", "1"),
        //    new CharacterData(0.3f, "�̼���", "2"),
        //    new CharacterData(0.3f, "��Ʋ��", "3")
        //};

        //CharacterData ����Ʈ�� WeightedItem ����Ʈ�� ��ȯ
        List<WeightedItem<CharacterData>> weightedCharacterList = characterList.ToWeightedItemList(character => character.weight);

        //����ġ�� ���� �������� CharacterData ��ü�� �����մϴ�.
        CharacterData selectedCharacter = WeightedRandomUtility.GetWeightedRandom(weightedCharacterList, randomValue);

        //���õ� CharacterData ��ü ���� ���
        Debug.Log("Selected Character: " + selectedCharacter.displayName);

        return selectedCharacter;
    }
}

[Serializable]
public class WeightedItem<T>
{
    public T item;
    public float weight;
}

public class WeightedRandomUtility
{
    public static T GetWeightedRandom<T>(List<WeightedItem<T>> weightedItems, float randomValue)
    {
        //���� �׸��� ������ �⺻�� ��ȯ
        if (weightedItems.Count == 0) return default(T);

        //��� ����ġ�� ���Ͽ� ������ ����
        float totalWeight = 0f;
        foreach (var weightedItem in weightedItems)
        {
            totalWeight += weightedItem.weight;
        }

        //���� ���� ����ġ ���տ� ���� ����
        randomValue *= totalWeight;

        //���� ���� ��� ������ ���ϴ��� Ȯ���Ͽ� �׸� ����
        foreach (var weightedItem in weightedItems)
        {
            randomValue -= weightedItem.weight;
            if (randomValue <= 0) return weightedItem.item;
        }

        //������� �Դٸ� ���� �߸��� ���� �ƴϹǷ� ������ �׸� ��ȯ
        return weightedItems[weightedItems.Count - 1].item;
    }
}

public static class WeightedItemExtensions
{
    public static List<WeightedItem<T>> ToWeightedItemList<T>(this List<T> dataList, Func<T, float> weightSelector)
    {
        List<WeightedItem<T>> weightedItemList = new List<WeightedItem<T>>();
        foreach (T data in dataList)
        {
            weightedItemList.Add(new WeightedItem<T> { item = data, weight = weightSelector(data) });
        }
        return weightedItemList;
    }
}
