using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Simulation : MonoBehaviour
{
    private UIManager uiManager;
    private TileMapVisualizer tileMapVisualizer;

    private int width = 100;
    private int height = 100;
    private int minSize = 10;
    private int maxSize = 100;

    private bool[,] cells;
    private int offset = 1;

    private float updateTimer = 0.3f;
    private float counterForUpdateTimer;

    private int chanceOfLifePercentageValue = 5;

    // Start is called before the first frame update
    private void Awake()
    {
        cells = new bool[width + 2, height + 2];
    }
    void Start()
    {
        uiManager = UIManager.Instance;
        tileMapVisualizer = TileMapVisualizer.Instance;

        uiManager.SetChanceOfLifeSlider(chanceOfLifePercentageValue);
        uiManager.SetFieldRangeUI(minSize,maxSize);
        uiManager.SetFieldSizeUI(width, height);

        counterForUpdateTimer = updateTimer;

        tileMapVisualizer.CreateLivingAreaFrame(width, height);
    }

    // Update is called once per frame
    void Update()
    {
        CreateLifeOnMouseClickPosition();
        SimulationMainProcess();
    }

    private void CreateCellsOnCLickCoordinates(Vector3Int coordinate)
    {
        cells[coordinate.x, coordinate.y] = true;           // Center of click
        cells[coordinate.x - 1, coordinate.y] = true;       // Left of click
        cells[coordinate.x + 1, coordinate.y] = true;       // Right of click
        cells[coordinate.x, coordinate.y - 1] = true;       // Top of click
        cells[coordinate.x, coordinate.y + 1] = true;       // Down of click

        cells[coordinate.x - 1, coordinate.y - 1] = true;   // Top Left of click
        cells[coordinate.x + 1, coordinate.y - 1] = true;   // Top Right of click
        cells[coordinate.x - 1, coordinate.y + 1] = true;   // Down Left of click
        cells[coordinate.x + 1, coordinate.y + 1] = true;   // Down Right of click
    }


    

    // Main Functions
    private void SimulationMainProcess()
    {
        counterForUpdateTimer -= Time.deltaTime;
        if (counterForUpdateTimer <= 0)
        {
            for (int y = 1; y <= height; y++)
            {
                for (int x = 1; x <= width; x++)
                {
                    cells[x, y] = CheckLifeRules(CountNeighbours(x, y), cells[x, y]);
                    tileMapVisualizer.PaintLivingCellTile(new Vector3Int(x, y, 0), cells[x, y]);
                }
            }
            counterForUpdateTimer = updateTimer;
        }
    }

    private int CountNeighbours(int x, int y)
    {
        int neighboursCount = 0;

        if (cells[x - 1, y]) // Left
        {
            neighboursCount++;
        }
        if (cells[x + 1, y]) // Rigth
        {
            neighboursCount++;
        }
        if (cells[x, y - 1]) // Top
        {
            neighboursCount++;
        }
        if (cells[x, y + 1]) // Down
        {
            neighboursCount++;
        }
        // Diagonal
        if (cells[x - 1, y - 1]) // Left Top
        {
            neighboursCount++;
        }
        if (cells[x - 1, y + 1]) // Left Down
        {
            neighboursCount++;
        }
        if (cells[x + 1, y - 1]) // Right Top
        {
            neighboursCount++;
        }
        if (cells[x + 1, y + 1]) // Right Down
        {
            neighboursCount++;
        }

        return neighboursCount;
    }

    private bool CheckLifeRules(int countOfNeighbourCells, bool stateOfCurrentCell)
    {
        if (countOfNeighbourCells > 3) // Overpopulation -> die
        {
            return false;
        }
        else if (countOfNeighbourCells == 3) // Born -> alive
        {
            return true;
        }
        else if (countOfNeighbourCells == 2 && stateOfCurrentCell) // Stay -> alive
        {
            return true;
        }
        else if (countOfNeighbourCells < 2) // too lonely -> die
        {
            return false;
        }
        else // default value if something went wrong
        {
            return false;
        }
    }


    private void RebuildField()
    {
        tileMapVisualizer.ClearTiles();
        width = uiManager.GetFieldSizeUI()[0];
        height = uiManager.GetFieldSizeUI()[1];
        tileMapVisualizer.CreateLivingAreaFrame(width, height);
    }
    // Creation types
    private void CreateLifeOnMouseClickPosition()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int coordinate = tileMapVisualizer.GetTileMap().WorldToCell(mouseWorldPos);

            if (coordinate.x > offset && coordinate.y > offset && coordinate.x <= width - offset && coordinate.y <= height - offset)
            {
                CreateCellsOnCLickCoordinates(coordinate);
                tileMapVisualizer.PaintLivingCellTile(new Vector3Int(coordinate.x, coordinate.y, 0), cells[coordinate.x, coordinate.y]);
            }
        }
    }

    public void CreateRandomCells(int probabilityToCreateNewLife)
    {
        for (int y = 1; y <= height; y++)
        {
            for (int x = 1; x <= width; x++)
            {
                int probability = UnityEngine.Random.Range(0, 100);
                if (probability <= probabilityToCreateNewLife)
                {
                    cells[x, y] = true;
                }
                else
                {
                    cells[x, y] = false;
                }
            }
        }
    }

    // Buttons
    public void RestartSimulation()
    {
        if (width != uiManager.GetFieldSizeUI()[0] || height != uiManager.GetFieldSizeUI()[1])
        {
            RebuildField();
        }
        CreateRandomCells(uiManager.GetLifePercentageValue());
    }

    public void KillAllLife()
    {
        for (int y = 1; y <= height; y++)
        {
            for (int x = 1; x <= width; x++)
            {
                cells[x, y] = false;
            }
        }
    }

}
