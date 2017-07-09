using UnityEngine;
using System.Collections.Generic;

public class TileMapGenerator : MonoBehaviour {
    public Transform tilePrefab;
    public Transform obstaclePrefab;

    [Range(0, 1)]
    public float outlinePercent;

    public float tileSize;

    //Collection of Tile Coords
    List<Coords> m_Collection;
    Queue<Coords> m_ShuffledCollection;
    Queue<Coords> m_ShuffledOpenCollection;

    public Map[] maps;
    public int mapIndex;

    Map m_CurrentMap;

    Transform[,] tileMap;

    [System.Serializable]
    public class Coords
    {
        public int x;
        public int y;

        public Coords(int _x, int _y)
        {
            x = _x;
            y = _y;
        }

        public static bool operator ==(Coords c1, Coords c2) {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(c1, c2))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)c1 == null) || ((object)c2 == null))
            {
                return false;
            }

            // Return true if the fields match:
            return c1.x == c2.x && c1.y == c2.y;
        }

        public static bool operator !=(Coords c1, Coords c2)
        {
            return !(c1 == c2);
        }

        public override bool Equals(object obj)
        {
            // If parameter is null return false:
            if (obj != null)
                return false;

            // If parameter cannot be cast to Point return false.
            Coords c = obj as Coords;
            if ((object)c == null)
                return false;
            
            // Return true if the fields match:
            return (x == c.x) && (y == c.y);
        }

        public bool Equals(Coords c)
        {
            // If parameter is null return false:
            if ((object)c != null)
                return false;

            // Return true if the fields match:
            return (x == c.x) && (y == c.y);
        }

        public override int GetHashCode()
        {
            return x ^ y;
        }
    }

    [System.Serializable]
    public class Map
    {
        public int m_Seed;
        [Range(0, 1)]
        public float m_ObstaclePercent;
        public float minObstacleHeight, maxObstacleHeight;
        public Coords mapSize;
        public Coords mapCentre
        {
            get { return new Coords((int)(mapSize.x * 0.5f), (int)(mapSize.y * 0.5f)); }
        }
    }

    void Start() {
        GenerateMap();
    }

    public Coords GetRandCoord()
    {
        Coords randCoord = m_ShuffledCollection.Dequeue();
        m_ShuffledCollection.Enqueue(randCoord);
        return randCoord;
    }

    public Transform GetRandomOpenTile()
    {
        Coords randCoord = m_ShuffledOpenCollection.Dequeue();
        if (tileMap[randCoord.x, randCoord.y] == null)
            return GetRandomOpenTile();

        m_ShuffledOpenCollection.Enqueue(randCoord);
        
        return tileMap[randCoord.x, randCoord.y];
    }

    ////////////////////////////////////////////////////////////////////////////////////
    // Algorithm: Flood Fill Algorithm
    // Desc: Starting at centre of Obstacle Map
    // Search all tiles in an outward expanding radius
    // Counting Obstacles and Tiles
    // If number of tiles != count of Flood Fill Algorithm: Flood Fill failed
    // In this case Map isn't accessible, return false.
    ////////////////////////////////////////////////////////////////////////////////////
    bool MapIsFullyAccessible(bool[,] obstacleMap, int currentObstacleCount) {
        bool[,] visitedList = new bool[obstacleMap.GetLength(0), obstacleMap.GetLength(1)];
        Queue<Coords> queue = new Queue<Coords>();
        queue.Enqueue(m_CurrentMap.mapCentre);
        visitedList[m_CurrentMap.mapCentre.x, m_CurrentMap.mapCentre.y] = true;

        //Centre Tile + 1
        int accessibleTileCount = 1;
        while (queue.Count > 0) {
            Coords tile = queue.Dequeue();

            //Loop through Neighboring tiles
            for(int x = -1; x <= 1; x++) {
                for (int y = -1; y <= 1; y++) {
                    int neighborX = tile.x + x;
                    int neighborY = tile.y + y;

                    //If Edge Tile
                    if (x == 0 || y == 0)
                    {
                        //Array out of Bounds Check
                        if (neighborX >= 0 && neighborX < obstacleMap.GetLength(0) && neighborY >= 0 && neighborY < obstacleMap.GetLength(1))
                        {
                            //If never visited
                            if (!visitedList[neighborX, neighborY] && !obstacleMap[neighborX, neighborY])
                            {
                                visitedList[neighborX, neighborY] = true;
                                queue.Enqueue(new Coords(neighborX, neighborY));
                                accessibleTileCount++;
                            }
                        }
                    }
                }
            }
        }

        //Number of Tiles that should exist - Obstacles amount
        int targetAccessibleTileCount = (m_CurrentMap.mapSize.x * m_CurrentMap.mapSize.y) - currentObstacleCount;

        //If true, Map is fully accessible
        //Else, inaccessible
        return targetAccessibleTileCount == accessibleTileCount;
    }

    private Vector3 CoordToPosition(int x, int y) {
        return new Vector3 (-m_CurrentMap.mapSize.x / 2 + x, 0, -m_CurrentMap.mapSize.y / 2 + y) * tileSize;
    }

    private bool isEdge(int row, int column)
    {
        if (row == 0 || column == 0 || row == (m_CurrentMap.mapSize.x - 1) || column == (m_CurrentMap.mapSize.y - 1))
            return true;
        
        return false;
    }

    //Creates Tile Map
    public void GenerateMap()
    {
        m_CurrentMap = maps[mapIndex];
        tileMap = new Transform[m_CurrentMap.mapSize.x, m_CurrentMap.mapSize.y];
        System.Random prng = new System.Random(m_CurrentMap.m_Seed);
        m_Collection = new List<Coords>();

        //Generating Coords
        for (int x = 0; x < m_CurrentMap.mapSize.x; x++) {
            for (int y = 0; y < m_CurrentMap.mapSize.y; y++) {
                m_Collection.Add(new Coords(x, y));
            }
        }

        //Shuffle Collection of Coords
        m_ShuffledCollection = new Queue<Coords>(Utility.ShuffleArray(m_Collection.ToArray(), m_CurrentMap.m_Seed));

        //Clear list on function call
        string name = "Generated Map";
        if (transform.Find(name)) {
            DestroyImmediate(transform.Find(name).gameObject);
        }

        Transform mapHolder = new GameObject(name).transform;
        mapHolder.parent = transform;

        bool[,] obstacleMap = new bool[m_CurrentMap.mapSize.x, m_CurrentMap.mapSize.y];
        List<Coords> openCollection = new List<Coords>(m_Collection);

        //Set Obstacle positions
        int currObstCount = 0;
        int numObstacles = (int)((m_CurrentMap.mapSize.x * m_CurrentMap.mapSize.y) * m_CurrentMap.m_ObstaclePercent);
        for (int i = 0; i < numObstacles; i++)
        {
            Coords randCoord = GetRandCoord();
            obstacleMap[randCoord.x, randCoord.y] = true;
            currObstCount++;

            if (randCoord != m_CurrentMap.mapCentre && !isEdge(randCoord.x, randCoord.y) && MapIsFullyAccessible(obstacleMap, currObstCount)) {
                float obstacleHeight = Mathf.Lerp(m_CurrentMap.minObstacleHeight, m_CurrentMap.maxObstacleHeight, (float)prng.NextDouble());
                Vector3 obstaclePosition = CoordToPosition(randCoord.x, randCoord.y);

                Transform obstacle = Instantiate(obstaclePrefab, obstaclePosition + Vector3.up * (obstacleHeight * 0.5f), Quaternion.identity) as Transform;
                obstacle.localScale = new Vector3((1 - outlinePercent) * tileSize, obstacleHeight, (1 - outlinePercent) * tileSize);
                obstacle.parent = mapHolder;

                openCollection.Remove(randCoord);
            }
            else {
                obstacleMap[randCoord.x, randCoord.y] = false;
                currObstCount--;
            }
        }

        m_ShuffledOpenCollection = new Queue<Coords>(Utility.ShuffleArray(openCollection.ToArray(), m_CurrentMap.m_Seed));

        //Spawn Tiles
        for (int x = 0; x < m_CurrentMap.mapSize.x; x++)
        {
            for (int y = 0; y < m_CurrentMap.mapSize.y; y++)
            {
                Vector3 tilePosition = CoordToPosition(x, y);
                //Only Spawn Tile if obstacle doesn't exist in current position
                if (obstacleMap[x, y] == true)
                    continue;
                else if (isEdge(x, y)) {
                    float obstacleHeight = Mathf.Lerp(m_CurrentMap.minObstacleHeight, m_CurrentMap.maxObstacleHeight, (float)prng.NextDouble());
                    Transform obstacle = Instantiate(obstaclePrefab, tilePosition + Vector3.up * (obstacleHeight * 0.5f), Quaternion.identity) as Transform;
                    obstacle.localScale = Vector3.one * (1 - outlinePercent) * tileSize;
                    obstacle.parent = mapHolder;

                    obstacleMap[x, y] = true;
                } else {
                    Transform tileinstance = Instantiate(tilePrefab, tilePosition, Quaternion.Euler(Vector3.right * 90)) as Transform;
                    tileinstance.localScale = Vector3.one * (1 - outlinePercent) * tileSize;
                    tileinstance.parent = mapHolder;

                    tileMap[x, y] = tileinstance;
                }
            }
        }
    }
}
