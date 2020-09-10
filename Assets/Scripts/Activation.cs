using BayatGames.SaveGameFree;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Activation : MonoBehaviour
{
    // Pref
    public GameObject boardBrick;
    public GameObject boardObstacle;
    public GameObject boardWall;

    public GameObject sphere;

    // Material to apply on board game object
    public Material brickColor;
    public Material acceleratorColor;
    public Material deceleratorColor;
    public Material obsctacleColor;
    public Material shrinkerColor;
    public Material expanderColor;
    public Material shadermaterial;

    // Probability for board generation
    private const int PROBA_BRICK = 65;
    private const int PROBA_OBSTACLE = 50;
    private const int PROBA_ACCELERATOR = 10;
    private const int PROBA_DECELERATOR = 10;
    private const int PROBA_SHRINKER = 10;
    private const int PROBA_EXPANDER = 20;
    private const int PROBA_NONE = 10;
    private const int PROBA_DELETE_ANIM = 90;

    // Board parameters
    private int boardWidth = 7;
    private int boardHeight = 50;
    private const int WALL_HEIGHT = 4;

    private GameObject[,] board;

    /// <summary>
    /// Return true if the random is in the proba, return false if the random is not in the proba
    /// </summary>
    /// <param name="proba">proba beetween 0 and 100</param>
    /// <returns>true if random in proba, otherwise 0</returns>
    private bool RandomFromProba(double proba)
    {
        int rand = Random.Range(0, 100);
        return (rand < proba) ? true : false;
    }

    /// <summary>
    /// Helper to create GameObject for the board
    /// </summary>
    /// <param name="prefab">Prefab model of the object</param>
    /// <param name="x">x coordinate</param>
    /// <param name="z">z coordinate</param>
    /// <param name="tag">tag of the gameObject</param>
    /// <param name="material">material of the object</param>
    /// <returns></returns>
    private GameObject CreateBoardComponent(GameObject prefab, Vector3 pos, string tag, Material material)
    {
        GameObject go = null;
        go = Instantiate(prefab, new Vector3(pos.x, pos.y, pos.z), Quaternion.identity);
        go.tag = tag;
        go.transform.parent = this.transform;
        go.GetComponent<MeshRenderer>().material = material;

        return go;
    }

    /// <summary>
    /// Create the board 
    /// </summary>
    /// <returns>
    /// Return a array that conatain the GameObject edge of the board 
    /// [0] -> Left start of the board
    /// [1] -> Right start of the board
    /// [2] -> Left end of the board
    /// [3] -> Right end of the board
    /// </returns>
    private GameObject[] CreateBoard(int level)
    {
        board = new GameObject[boardWidth, boardHeight];

        for (int i = 0; i < boardWidth; i++)
        {
            for (int j = 0; j < boardHeight; j++)
            {
                GameObject go = null;

                //Create board brick
                if (go == null && RandomFromProba(PROBA_BRICK - level))
                {
                    go = CreateBoardComponent(boardBrick, new Vector3(i,0,j), "brick", brickColor);
                }
                //Create board obstacle
                if (go == null && RandomFromProba(PROBA_OBSTACLE - level))
                {
                    go = CreateBoardComponent(boardObstacle, new Vector3(i, 0.4f, j), "obstacle", obsctacleColor);

                    //Delete the animation component
                    if (RandomFromProba(PROBA_DELETE_ANIM - 10*level))
                    {
                        Destroy(go.GetComponent<Animator>());
                    }
                }

                if (go == null && RandomFromProba(PROBA_ACCELERATOR - level))
                {
                    go = CreateBoardComponent(boardBrick, new Vector3(i, 0, j), "accelerator", acceleratorColor);
                }

                if (go == null && RandomFromProba(PROBA_DECELERATOR + level))
                {
                    go = CreateBoardComponent(boardBrick, new Vector3(i, 0, j), "decelerator", deceleratorColor);
                }

                if(go == null && RandomFromProba(PROBA_SHRINKER + level))
                {
                    go = CreateBoardComponent(boardBrick, new Vector3(i, 0, j), "shrinker", shrinkerColor);
                }

                if(go == null && RandomFromProba(PROBA_EXPANDER - level))
                {
                    go = CreateBoardComponent(boardBrick, new Vector3(i, 0, j), "expander", expanderColor);
                }
                
                if(RandomFromProba(PROBA_NONE+ 2*level))
                {
                    Destroy(go);
                }

                board[i, j] = go;
            }
        }

        //Contains the edge coordinate of the board
        List<Tuple<int, int>> edgeCoordinate = new List<Tuple<int,int>>() {
            new Tuple<int, int>(0,0), //Left start of the board
            new Tuple<int, int>(boardWidth - 1, 0), //Right start of the board
            new Tuple<int, int>(0, boardHeight - 1), //Left end of the board
            new Tuple<int, int>(boardWidth - 1, boardHeight - 1) //Right end of the board
        };

        List<GameObject> edgeGameObject = new List<GameObject>();//contain the GameObject in the edge of the board
        foreach(Tuple<int,int> coordinate in edgeCoordinate)
        {
            //If gameobject in a edge is null, create a brick object
            //This ensures that the corners always contain objects
            if (board[coordinate.Item1, coordinate.Item2] == null)
            {
                board[coordinate.Item1, coordinate.Item2] = CreateBoardComponent(boardBrick, new Vector3(coordinate.Item1, 0, coordinate.Item2), "brick", brickColor);
            }
            edgeGameObject.Add(board[coordinate.Item1, coordinate.Item2]);//add edge to the array
        }

        return edgeGameObject.ToArray();
    }

    /// <summary>
    /// Add the wall to the board
    /// </summary>
    /// <param name="edgeGameObjects"></param>
    private void AddWalls(GameObject[] edgeGameObjects, int wallHeight)
    {
        GameObject rightWall = CreateBoardComponent(boardWall, (edgeGameObjects[1].transform.position + edgeGameObjects[3].transform.position) / 2, "wall", obsctacleColor);
        rightWall.transform.position += new Vector3(1, 0.4f, 0);
        rightWall.transform.localScale = new Vector3(1, wallHeight, boardHeight);
        rightWall.GetComponent<MeshRenderer>().material = shadermaterial;

        GameObject leftWall = CreateBoardComponent(boardWall, (edgeGameObjects[0].transform.position + edgeGameObjects[2].transform.position) / 2, "wall", obsctacleColor);
        leftWall.transform.position += new Vector3(-1, 0.4f, 0);
        leftWall.transform.localScale = new Vector3(1, wallHeight, boardHeight);
        leftWall.GetComponent<MeshRenderer>().material = shadermaterial;
    }

    /// <summary>
    /// Add the final plan at the end of the board
    /// </summary>
    /// <param name="edgeGameObjects"></param>
    private void AddEndPlane(GameObject[] edgeGameObjects)
    {
        //Get the plane and replace it to the end of the board
        Transform endPlane = gameObject.transform.GetChild(0);
        endPlane.tag = "end_plane";
        endPlane.position = (edgeGameObjects[2].transform.position + edgeGameObjects[3].transform.position) / 2;
        endPlane.localScale = new Vector3(boardWidth / 10f, 1, boardWidth / 10f);
    }

    // Start is called before the first frame update
    void Start()
    {
        int level = SaveGame.Load<int>("level");

        boardHeight = boardHeight + 5 * level;//update board length from the level number

        GameObject[] edgeGameObjects = CreateBoard(level);

        AddEndPlane(edgeGameObjects);

        //update the wall height from the level number
        if(WALL_HEIGHT - level > 0)
        {
            AddWalls(edgeGameObjects, WALL_HEIGHT - level);
        }

        //Rotate the board at the end of the generation
        this.transform.Rotate(20, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        shadermaterial.SetVector("_SpherePosition", sphere.transform.position);// Update the shader vector with the sphere position vector

        //Update board angle when arrow key is press
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            this.transform.Rotate(0.0f, 0.0f, 0.5f);
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            this.transform.Rotate(0.0f, 0.0f, -0.5f);
        }
    }
}
