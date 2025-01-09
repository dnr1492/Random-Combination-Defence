using System;
using System.Collections;
using System.Collections.Generic;
using System.Media;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerContainer : MonoBehaviour
{
    [Header("Find Target Tile and Create Transform")]
    [SerializeField] TileBase targetFindTile;  //찾을 타일 - BROWNROCK_full_tileset_v2_164
    [SerializeField] Tilemap containerTilemap;
    [SerializeField] GameObject containerTileTransformPrefab;
    private List<Transform> containerTrs = new List<Transform>();
    private List<ContainerCountText> containerCountTexts = new List<ContainerCountText>();

    [Header("Add")]
    private List<PlayerController> players = new List<PlayerController>();
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
    public void Add(PlayerController player)
    {
        player.name = player.name.Replace(player.name, player.name + "(Container)");  //'Container에 있는 플레이어'와 'Main에 있는 플레이어'를 구분
        players.Add(player);
        DebugLogger.Log("Added player name : " + player.name);
        SetPosition(player);
        SetScale(player, Vector3.one * 4);
    }

    private void SetPosition(PlayerController player)
    {
        for (int i = 0; i < containerTrs.Count; i++)
        {
            if (!dicContainerTrs.ContainsKey(player.name))
            {
                dicContainerTrs.Add(player.name, containerTrs[i]);
                player.transform.position = containerTrs[i].position;
                containerCountTexts[i].Increase();
            }
            else
            {
                if (dicContainerTrs[player.name] == containerTrs[i])
                {
                    player.gameObject.SetActive(false);
                    player.transform.position = containerTrs[i].position;
                    containerCountTexts[i].Increase();
                    break;
                }
            }
        }
    }
    #endregion

    #region Get / Get All
    public PlayerController Get(string name, Action<GameObject> action)
    {
        PlayerController foundPlayer = players.Find(player => player.name == name);
        if (foundPlayer == null)
        {
            DebugLogger.Log($"Player name {name} not found.");
            return null;
        }
        DecreaseContainerPlayer(foundPlayer, action);
        players.Remove(foundPlayer);
        foundPlayer.name = foundPlayer.name.Replace("(Container)", string.Empty);  //'Container에 있는 플레이어'와 'Main에 있는 플레이어'를 구분
        return foundPlayer;
    }

    public List<PlayerController> GetAll(string name, Action<GameObject> action)
    {
        List<PlayerController> foundPlayers = players.FindAll(player => player.name == name);
        if (foundPlayers == null) DebugLogger.Log($"Player name {name} not found.");
        for (int i = 0; i < foundPlayers.Count; i++)
        {
            DecreaseContainerPlayer(foundPlayers[i], action);
            foundPlayers[i].name = foundPlayers[i].name.Replace("(Container)", string.Empty);  //'Container에 있는 플레이어'와 'Main에 있는 플레이어'를 구분
        }
        players.RemoveRange(0, foundPlayers.Count);
        return foundPlayers;
    }
    #endregion

    #region Delete
    public void Delete(string name)
    {
        PlayerController foundPlayer = players.Find(p => p.name == name);
        if (foundPlayer == null) DebugLogger.Log($"Player name {name} not found.");
        DecreaseContainerPlayer(foundPlayer, DeletePlayer);
        players.Remove(foundPlayer);
    }

    private void DeletePlayer(GameObject playerGo)
    {
        var existingPlayers = PlayerGenerator.ExistingPlayers;
        for (int i = 0; i < existingPlayers.Count; i++)
        {
            if (existingPlayers[i] == playerGo)
            {
                PlayerGenerator.ExistingPlayers.Remove(playerGo);
            }
        }
        Destroy(playerGo);
    }
    #endregion

    private void DecreaseContainerPlayer(PlayerController player, Action<GameObject> action)
    {
        var containerCountText = dicContainerTrs[player.name].GetComponentInChildren<ContainerCountText>();
        var currentCount = containerCountText.GetCurrentCount();
        if (currentCount > 1)
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].name == player.name)
                {
                    players[i + 1].gameObject.SetActive(true);
                    action(player.gameObject);
                    containerCountText.Decrease();
                    break;
                }
            }
        }
        else
        {
            action(player.gameObject);
            containerCountText.Decrease();
            dicContainerTrs.Remove(player.name);
        }
        SetScale(player, Vector3.one);
    }

    private void SetScale(PlayerController player, Vector3 setScale)
    {
        player.transform.localScale = setScale;
    }
}