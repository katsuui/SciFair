using UnityEngine;

public class Grid : MonoBehaviour
{
    public int numRows; // number of rows in the grid
    public int numCols; // number of columns in the grid
    public float cellSize; // size of each cell in Unity units
    public Vector2 originPosition; // position of the bottom-left corner of the grid

    private GameObject[,] gridArray; // 2D array to store the game objects in the grid

    // Start is called before the first frame update
    void Start()
    {
        // initialize the grid array
        gridArray = new GameObject[numRows, numCols];

        // fill the grid with game objects
        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < numCols; col++)
            {
                // create a new game object
                GameObject newCell = new GameObject("Cell [" + row + ", " + col + "]");

                // set the position of the game object
                newCell.transform.position = new Vector3(
                    originPosition.x + (cellSize * col),
                    originPosition.y + (cellSize * row),
                    0);

                // set the game object as a child of the grid object
                newCell.transform.parent = transform;

                // add the game object to the grid array
                gridArray[row, col] = newCell;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    // example method to get the game object at a particular cell in the grid
    public GameObject GetCell(int row, int col)
    {
        if (row >= 0 && row < numRows && col >= 0 && col < numCols)
        {
            return gridArray[row, col];
        }
        else
        {
            return null;
        }
    }
}
