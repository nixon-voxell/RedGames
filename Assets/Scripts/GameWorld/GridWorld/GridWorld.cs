using UnityEngine;
using GameWorld.Util;
using Unity.Mathematics;

public class GridWorld : MonoBehaviour
{
    [SerializeField] private Pool<Tile> m_TilePool;
    [SerializeField] private int2 m_Size;

    [SerializeField] private int2 m_Center;
    [SerializeField] private Transform m_TargetTransform;

    private int2 m_HalfSize;

    private void UpdateCenter(Vector3 center)
    {
        int2 currCenter = new int2((int)center.x, (int)center.z);

        // if target transform move out of range, regenerate tiles
        if (!this.m_Center.Equals(currCenter))
        {
            this.m_Center = currCenter;
            this.m_TilePool.SetAllObjectActive(false);
            this.GenerateTileArea();
        }
    }

    private void GenerateTileArea()
    {
        for (int x = 0; x < this.m_Size.x; x++)
        {
            for (int y = 0; y < this.m_Size.y; y++)
            {
                int2 position = new int2(x, y);
                position = position - this.m_HalfSize + this.m_Center;
                this.SpawnTile(position);
            }
        }
    }

    private void SpawnTile(int2 position)
    {
        Tile nextTile = this.m_TilePool.GetNextObject();
        nextTile.gameObject.SetActive(true);
        nextTile.transform.position = new Vector3(position.x, 0.0f, position.y);

        nextTile.Initialize(GridUtil.GetTileRandIndex(position, 9));
    }

    private void Start()
    {
        this.m_TilePool.Initialize(this.transform);
        this.m_HalfSize = this.m_Size / 2;

        this.UpdateCenter(this.m_TargetTransform.position);
        this.GenerateTileArea();
    }

    private void Update()
    {
        Vector3 position = this.m_TargetTransform.position;
        this.UpdateCenter(position);
    }

    private void OnDestroy()
    {
        this.m_TilePool.Dispose();
    }
}