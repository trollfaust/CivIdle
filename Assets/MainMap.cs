using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class MainMap : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] Tilemap landMap = null;
    [SerializeField] Tilemap waterMap = null;
    [SerializeField] Tilemap overlayMap = null;
    [SerializeField] Tilemap fogMap = null;

    [SerializeField] Tile highlightTile = null;
    float zValue = 0f;

    Vector3Int lastCell = Vector3Int.zero;

    public bool isDragging;

    void Start()
    {
        zValue = Camera.main.transform.position.z - transform.position.z;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isDragging)
        {
            return;
        }
        Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, zValue));
        Vector3Int cell = landMap.WorldToCell(new Vector3(-pos.x, -pos.y, pos.z));

        overlayMap.SetTile(lastCell, null);
        lastCell = cell;

        if (landMap.HasTile(cell) == true && fogMap.HasTile(cell) == false)
        {
            overlayMap.SetTile(cell, highlightTile);

            List<Vector3Int> neighbors = Neighbors(cell);
            foreach (Vector3Int neighbor in neighbors)
            {
                if (fogMap.HasTile(neighbor))
                {
                    fogMap.SetTile(neighbor, null);
                }
            }
        }
    }


    static Vector3Int
    LEFT = new Vector3Int(-1, 0, 0),
    RIGHT = new Vector3Int(1, 0, 0),
    DOWN = new Vector3Int(0, -1, 0),
    DOWNLEFT = new Vector3Int(-1, -1, 0),
    DOWNRIGHT = new Vector3Int(1, -1, 0),
    UP = new Vector3Int(0, 1, 0),
    UPLEFT = new Vector3Int(-1, 1, 0),
    UPRIGHT = new Vector3Int(1, 1, 0);

    static Vector3Int[] directions_when_y_is_even =
          { LEFT, RIGHT, DOWN, DOWNLEFT, UP, UPLEFT };
    static Vector3Int[] directions_when_y_is_odd =
          { LEFT, RIGHT, DOWN, DOWNRIGHT, UP, UPRIGHT };

    public List<Vector3Int> Neighbors(Vector3Int node)
    {
        List<Vector3Int> output = new List<Vector3Int>();
        Vector3Int[] directions = (node.y % 2) == 0 ?
             directions_when_y_is_even :
             directions_when_y_is_odd;
        foreach (var direction in directions)
        {
            Vector3Int neighborPos = node + direction;
            output.Add( neighborPos);
        }

        return output;
    }

}
