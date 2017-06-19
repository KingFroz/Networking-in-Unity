using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileMapGenerator : MonoBehaviour {
    public Transform tilePrefab;
    public Transform obstaclePrefab;
    public Vector2 mapSize;

    [Range(0, 1)]
    public float outlinePercent;

    private float Offset;

    //Collection of Tile Coords
    List<Coords> m_Collection;
    Queue<Coords> m_ShuffledCollection;

    public int seed = 5;

    [Range(0, 1)]
    public float percentage;

    Coords mapCentre;

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

    void Start()
    {
        Offset = 0.5f;
        GenerateMap();
    }
    
    public Coords GetRandCoord()
    {
        Coords randCoord = m_ShuffledCollection.Dequeue();
        m_ShuffledCollection.Enqueue(randCoord);
        return randCoord;
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
        queue.Enqueue(mapCentre);
        visitedList[mapCentre.x, mapCentre.y] = true;

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
        int targetAccessibleTileCount = (int)(mapSize.x * mapSize.y) - currentObstacleCount;

        //If true, Map is fully accessible
        //Else, inaccessible
        return targetAccessibleTileCount == accessibleTileCount;
    }

    private Vector3 CoordToPosition(int x, int y) {
        return new Vector3 (-mapSize.x / 2 + Offset + x, 0, -mapSize.y / 2 + Offset + y);
    }

    //Creates Tile Map
    public void GenerateMap()
    {
        m_Collection = new List<Coords>();

        for (int x = 0; x < mapSize.x; x++) {
            for (int y = 0; y < mapSize.y; y++) {
                m_Collection.Add(new Coords(x, y));
            }
        }

        m_ShuffledCollection = new Queue<Coords>(Utility.ShuffleArray(m_Collection.ToArray(), seed));
        mapCentre = new Coords((int)(mapSize.x * 0.5f), (int)(mapSize.y * 0.5f));

        string name = "Generated Map";
        if (transform.FindChild(name)) {
            DestroyImmediate(transform.FindChild(name).gameObject);
        }

        Transform mapHolder = new GameObject(name).transform;
        mapHolder.parent = transform;

        //Position tiles by dividing -Map X and -Map Y by 2 + Offset
        for (int x = 0; x < mapSize.x; x++) {
            for (int y = 0; y < mapSize.y; y++) {
                Vector3 tilePosition = CoordToPosition(x, y);
                Transform tileinstance = Instantiate(tilePrefab, tilePosition, Quaternion.Euler(Vector3.right * 90)) as Transform;
                tileinstance.localScale = Vector3.one * (1 - outlinePercent);
                tileinstance.parent = mapHolder;
            }
        }

        bool[,] obstacleMap = new bool[(int)mapSize.x, (int)mapSize.y];

        int currObstCount = 0;
        int numObstacles = (int)((mapSize.x * mapSize.y) * percentage);
        for (int i = 0; i < numObstacles; i++)
        {
            Coords randCoord = GetRandCoord();
            obstacleMap[randCoord.x, randCoord.y] = true;
            currObstCount++;

            if (randCoord != mapCentre && MapIsFullyAccessible(obstacleMap, currObstCount)) {
                Vector3 obstaclePosition = CoordToPosition(randCoord.x, randCoord.y);
                Transform obstacle = Instantiate(obstaclePrefab, obstaclePosition + Vector3.up * 0.5f, Quaternion.identity) as Transform;
                obstacle.parent = mapHolder;
            }
            else {
                obstacleMap[randCoord.x, randCoord.y] = false;
                currObstCount--;
            }
        }
    }
}
