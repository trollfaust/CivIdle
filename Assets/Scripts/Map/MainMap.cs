using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

namespace trollschmiede.CivIdle.MapSys
{
    public class MainMap : MonoBehaviour, IPointerClickHandler
    {
        #region Singleton
        public static MainMap instance;
        void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
        }
        #endregion

        [SerializeField] Tilemap landMap = null;
        [SerializeField] Tilemap waterMap = null;
        [SerializeField] Tilemap overlayMap = null;
        [SerializeField] Tilemap fogMap = null;

        [SerializeField] Tile highlightTile = null;
        float zValue = 0f;

        Vector3Int lastCell = Vector3Int.zero;

        public bool isDragging;

        public delegate void OnRevealLandTile();
        public event OnRevealLandTile onRevealLandTile;

        bool isSetup = false;
        bool hasCellHighlighted = false;
        int lastValue = 0;

        void Start()
        {
            zValue = Camera.main.transform.position.z - transform.position.z;
            isSetup = true;
        }

        public void OnTick(int min, int max)
        {
            if (isSetup == false)
                return;

            if ((landMap.GetTile(lastCell) is LandTile) == false || hasCellHighlighted == false)
                return;

            LandTile tile = (LandTile)landMap.GetTile(lastCell);

            int rng = Random.Range(min, max);

            if (tile.GetPercentExplored() < 100)
            {
                lastValue = tile.ChangeExplored(rng);
                return;
            }

            List<Vector3Int> neighbors = Neighbors(lastCell);
            int neighborsIndexRng = Random.Range(0, neighbors.Count);

            if ((landMap.GetTile(neighbors[neighborsIndexRng]) is LandTile) == false)
            {
                fogMap.SetTile(neighbors[neighborsIndexRng], null);
                return;
            }

            LandTile neighbor = (LandTile)landMap.GetTile(neighbors[neighborsIndexRng]);

            lastValue = neighbor.ChangeExplored(rng);

            if (neighbor.GetPercentExplored() > 20)
            {
                fogMap.SetTile(neighbors[neighborsIndexRng], null);
            }
        }

        public int GetLastValue()
        {
            return lastValue;
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
            hasCellHighlighted = false;

            if (landMap.HasTile(cell) == true && fogMap.HasTile(cell) == false)
            {
                lastCell = cell;
                overlayMap.SetTile(cell, highlightTile);
                hasCellHighlighted = true;
            }
        }

        void UnlockTile(Vector3Int tilePosition)
        {
            if (landMap.HasTile(tilePosition))
            {
                onRevealLandTile?.Invoke();
            }

            fogMap.SetTile(tilePosition, null);
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
                output.Add(neighborPos);
            }

            return output;
        }

    }
}