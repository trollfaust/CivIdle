using UnityEngine;
using UnityEngine.Tilemaps;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace trollschmiede.CivIdle.MapSys
{
    public class LandTile : Tile
    {
        public bool isLandTile = false;
        public int percentExplored = 0;

        public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
        {
            isLandTile = true;

            return base.StartUp(position, tilemap, go);
        }

        public override void RefreshTile(Vector3Int position, ITilemap tilemap)
        {
            base.RefreshTile(position, tilemap);
        }

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            base.GetTileData(position, tilemap, ref tileData);
        }

        public override bool GetTileAnimationData(Vector3Int position, ITilemap tilemap, ref TileAnimationData tileAnimationData)
        {
            return base.GetTileAnimationData(position, tilemap, ref tileAnimationData);
        }

        public int ChangeExplored(int amount)
        {
            if (percentExplored + amount > 100)
            {
                amount = 100 - percentExplored;
            }
            if (percentExplored + amount < 0)
            {
                amount = 0 - percentExplored;
            }

            percentExplored += amount;

            return amount;
        }

        public int GetPercentExplored()
        {
            return percentExplored;
        }

        #if UNITY_EDITOR
        // The following is a helper that adds a menu item to create a RoadTile Asset
        [MenuItem("Assets/Create/LandTile")]
        public static void CreateLandTile()
        {
            string path = EditorUtility.SaveFilePanelInProject("Save Land Tile", "New Land Tile", "Asset", "Save Land Tile", "Assets");
            if (path == "")
                return;
            AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<LandTile>(), path);
        }
        #endif
    }
}