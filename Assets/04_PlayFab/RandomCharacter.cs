using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RandomCharacter
{
    [Serializable]
    public class CharacterData
    {
        public float weight;  //CharacterData 객체 내부에 가중치를 정의합니다.
        public string displayName;
        public string itemId;
        public string itemClass;

        public CharacterData(float weight, string displayName, string itemId, string itemClass)
        {
            this.weight = weight;
            this.displayName = displayName;
            this.itemId = itemId;
            this.itemClass = itemClass;
        }
    }

    public static CharacterData DrawRandomCharacter(List<CharacterData> characterList, float randomValue)
    {
        ////CharacterData 객체를 담는 리스트 생성 예제
        //List<CharacterData> characterList = new List<CharacterData>
        //{
        //    new CharacterData(0.4f, "주몽", "1", "전설적인"),
        //    new CharacterData(0.3f, "이순신", "2", "전설적인"),
        //    new CharacterData(0.3f, "히틀러", "3", "전설적인")
        //};

        //CharacterData 리스트를 WeightedItem 리스트로 변환
        List<WeightedItem<CharacterData>> weightedCharacterList = characterList.ToWeightedItemList(character => character.weight);

        //가중치에 따라 무작위로 CharacterData 객체를 뽑습니다.
        CharacterData drawnCharacter = WeightedRandomUtility.GetWeightedRandom(weightedCharacterList, randomValue);

        //뽑힌 CharacterData 객체 정보 출력
        DebugLogger.Log("Draw Character: " + drawnCharacter.displayName);

        return drawnCharacter;
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
        //만약 항목이 없으면 기본값 반환
        if (weightedItems.Count == 0) return default(T);

        //모든 가중치를 더하여 총합을 구함
        float totalWeight = 0f;
        foreach (var weightedItem in weightedItems)
        {
            totalWeight += weightedItem.weight;
        }

        //랜덤 값을 가중치 총합에 맞춰 조정
        randomValue *= totalWeight;

        //랜덤 값이 어느 범위에 속하는지 확인하여 항목 선택
        foreach (var weightedItem in weightedItems)
        {
            randomValue -= weightedItem.weight;
            if (randomValue <= 0) return weightedItem.item;
        }

        //여기까지 왔다면 뭔가 잘못된 것이 아니므로 마지막 항목 반환
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
