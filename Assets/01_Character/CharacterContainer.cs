using System;
using System.Collections;
using System.Collections.Generic;
using System.Media;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CharacterContainer : MonoBehaviour
{
    [Header("Find Target Tile and Create Transform")]
    [SerializeField] TileBase targetFindTile;  //찾을 타일 - BROWNROCK_full_tileset_v2_164
    [SerializeField] Tilemap containerTilemap;
    [SerializeField] GameObject containerTileTransformPrefab;
    private List<Transform> containerTrs = new List<Transform>();
    private List<ContainerCountText> containerCountTexts = new List<ContainerCountText>();

    [Header("Add")]
    private List<CharacterController> characters = new List<CharacterController>();
    private Dictionary<string, Transform> dicContainerTrs = new Dictionary<string, Transform>();

    private void Start()
    {
        FindTilePositions();
    }

    #region Find Target Tile and Create Transform
    private void FindTilePositions()
    {
        BoundsInt bounds = containerTilemap.cellBounds;
        TileBase[] allTiles = containerTilemap.GetTilesBlock(bounds);

        //좌하단 (시작) ~ 우상단 (끝)
        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = allTiles[x + y * bounds.size.x];
                if (tile == targetFindTile)
                {
                    Vector3Int cellPosition = new Vector3Int(x + bounds.xMin, y + bounds.yMin, 0);
                    DebugLogger.Log("Target Tile found at : " + cellPosition);

                    //특정 타일에 Transform 생성
                    Vector3 worldPosition = containerTilemap.CellToWorld(cellPosition);
                    worldPosition = new Vector3(worldPosition.x + 0.5f, worldPosition.y + 0.5f);
                    var containerTr = Instantiate(containerTileTransformPrefab, worldPosition, Quaternion.identity);
                    containerTrs.Add(containerTr.transform);
                    containerCountTexts.Add(containerTr.GetComponentInChildren<ContainerCountText>());
                }
                else DebugLogger.Log("Not Tile found");
            }
        }
    }
    #endregion

    #region Add
    public void Add(CharacterController character)
    {
        character.name = character.name.Replace(character.name, character.name + "(Container)");  //'Container에 있는 캐릭터'와 'Main에 있는 캐릭터'를 구분
        characters.Add(character);
        DebugLogger.Log("Added character display name : " + character.name);
        SetPosition(character);
        SetScale(character, Vector3.one * 4);
    }

    private void SetPosition(CharacterController character)
    {
        for (int i = 0; i < containerTrs.Count; i++)
        {
            if (!dicContainerTrs.ContainsKey(character.name))
            {
                dicContainerTrs.Add(character.name, containerTrs[i]);
                character.transform.position = containerTrs[i].position;
                containerCountTexts[i].Increase();
            }
            else
            {
                if (dicContainerTrs[character.name] == containerTrs[i])
                {
                    character.gameObject.SetActive(false);
                    character.transform.position = containerTrs[i].position;
                    containerCountTexts[i].Increase();
                    break;
                }
            }
        }
    }
    #endregion

    #region Get / Get All
    public CharacterController Get(string displayName, Action<GameObject> action)
    {
        CharacterController foundCharacter = characters.Find(character => character.name == displayName);
        if (foundCharacter == null)
        {
            DebugLogger.Log($"Character display name {displayName} not found.");
            return null;
        }
        DecreaseContainerCharacter(foundCharacter, action);
        characters.Remove(foundCharacter);
        foundCharacter.name = foundCharacter.name.Replace("(Container)", string.Empty);  //'Container에 있는 캐릭터'와 'Main에 있는 캐릭터'를 구분
        return foundCharacter;
    }

    public List<CharacterController> GetAll(string displayName, Action<GameObject> action)
    {
        List<CharacterController> foundCharacters = characters.FindAll(character => character.name == displayName);
        if (foundCharacters == null) DebugLogger.Log($"Character display name {displayName} not found.");
        for (int i = 0; i < foundCharacters.Count; i++)
        {
            DecreaseContainerCharacter(foundCharacters[i], action);
            foundCharacters[i].name = foundCharacters[i].name.Replace("(Container)", string.Empty);  //'Container에 있는 캐릭터'와 'Main에 있는 캐릭터'를 구분
        }
        characters.RemoveRange(0, foundCharacters.Count);
        return foundCharacters;
    }
    #endregion

    #region Delete
    public void Delete(string displayName)
    {
        CharacterController foundCharacter = characters.Find(p => p.name == displayName);
        if (foundCharacter == null) DebugLogger.Log($"Character display name {displayName} not found.");
        DecreaseContainerCharacter(foundCharacter, DeleteCharacter);
        characters.Remove(foundCharacter);
    }

    private void DeleteCharacter(GameObject characterGo)
    {
        var existingCharacters = CharacterGenerator.ExistingCharacters;
        for (int i = 0; i < existingCharacters.Count; i++)
        {
            if (existingCharacters[i] == characterGo)
            {
                CharacterGenerator.ExistingCharacters.Remove(characterGo);
            }
        }
        Destroy(characterGo);
    }
    #endregion

    private void DecreaseContainerCharacter(CharacterController character, Action<GameObject> action)
    {
        var containerCountText = dicContainerTrs[character.name].GetComponentInChildren<ContainerCountText>();
        var currentCount = containerCountText.GetCurrentCount();
        if (currentCount > 1)
        {
            for (int i = 0; i < characters.Count; i++)
            {
                if (characters[i].name == character.name)
                {
                    characters[i + 1].gameObject.SetActive(true);
                    action(character.gameObject);
                    containerCountText.Decrease();
                    break;
                }
            }
        }
        else
        {
            action(character.gameObject);
            containerCountText.Decrease();
            dicContainerTrs.Remove(character.name);
        }
        SetScale(character, Vector3.one);
    }

    private void SetScale(CharacterController character, Vector3 setScale)
    {
        character.transform.localScale = setScale;
    }
}