using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class WinLogic : MonoBehaviour
{
    public bool redTurn;
    public int numberOfMoves;
    public bool win;

    [System.Serializable]
    public class HorizontalCell
    {
        public string cellCoords;
        public GameObject spaceObject;
        public int rowNumber;
        public int columnNumber;
        public string fillValue;
    }

    [System.Serializable]
    public class Row
    {
        public string name;
        public HorizontalCell[] cells;
    }

    [Header("Rows")]
    public Row[] rowsArray = new Row[6];

    [System.Serializable]
    public class VerticalCell
    {
        public string cellCoords;
        public GameObject spaceObject;
        public int rowNumber;
        public int columnNumber;
        public string fillValue;
    }

    [System.Serializable]
    public class Column
    {
        public string name;
        public VerticalCell[] cells;
    }

    public Column[] columnsArray = new Column[7];

    [System.Serializable]
    public class DiagonalCell
    {
        public string cellCoords;
        public GameObject spaceObject;
        public int rowNumber;
        public int columnNumber;
        public string fillValue;
    }

    [System.Serializable]
    public class DiagonalLine
    {
        public string name;
        public DiagonalCell[] cells;
    }

    [Header("Left to Right Diagonals")]
    //public DiagonalLine LToRDiagonal0;
    //public DiagonalLine LToRDiagonal1;
    //public DiagonalLine LToRDiagonal2;
    //public DiagonalLine LToRDiagonal3;
    //public DiagonalLine LToRDiagonal4;
    //public DiagonalLine LToRDiagonal5;
    ////0 in these arrays is top left
    public DiagonalLine[] LToRDiagonals = new DiagonalLine[6] /*{ LToRDiagonal0, LToRDiagonal1, LToRDiagonal2, LToRDiagonal3, LToRDiagonal4, LToRDiagonal5 }*/;
    //array 0 is bottom left, array 5 is top right

    [Header("Right to Left Diagonals")]
    //public DiagonalLine RToLDigaonal0;
    //public DiagonalLine RToLDigaonal1;
    //public DiagonalLine RToLDigaonal2;
    //public DiagonalLine RToLDigaonal3;
    //public DiagonalLine RToLDigaonal4;
    //public DiagonalLine RToLDigaonal5;
    ////0 in these arrays is top right
    public DiagonalLine[] RToLDiagonals = new DiagonalLine[6] /*{ RToLDigaonal0, RToLDigaonal1, RToLDigaonal2, RToLDigaonal3, RToLDigaonal4, RToLDigaonal5 }*/;
    //array 0 is top left, array 5 is bot right

    [Header("Chip Instantiation")]
    public GameObject chipInstantiator;
    public ChipInstantiation chipInstantiationScript;

    void Start()
    {
        redTurn = true;
        numberOfMoves = 0;
        chipInstantiationScript = chipInstantiator.GetComponent<ChipInstantiation>();
        chipInstantiationScript.redTurnCI = redTurn;
        chipInstantiationScript.readyToInstantiate = true;
    }

    int GetIndexFromIntsLR(int diagonalIndex, int row, int column)
    {
        // Combines row first, then column (e.g., row = 0, column = 2 becomes "02")
        string targetCoords = $"{row}{column}";

        DiagonalCell[] cellsInLine = LToRDiagonals[diagonalIndex].cells;

        // Returns the second-level index where cellCoords matches, or -1 if not found
        int foundIndex = Array.FindIndex(cellsInLine, cell => cell.cellCoords == targetCoords);

        return foundIndex;
    }
    int GetIndexFromIntsRL(int diagonalIndex, int row, int column)
    {
        // Combines row first, then column (e.g., row = 0, column = 2 becomes "02")
        string targetCoords = $"{row}{column}";

        DiagonalCell[] cellsInLine = RToLDiagonals[diagonalIndex].cells;

        // Returns the second-level index where cellCoords matches, or -1 if not found
        int foundIndex = Array.FindIndex(cellsInLine, cell => cell.cellCoords == targetCoords);

        return foundIndex;
    }

    public void UpdateChip(int chipRow, int chipColumn, string fill, bool Lrd, int LrdI, bool Rld, int RldI)
    {
        rowsArray[chipRow].cells[chipColumn].fillValue = fill;
        columnsArray[chipColumn].cells[chipRow].fillValue = fill;

        int LrdCIndex = 0;
        int RldCIndex = 0;
        if (Lrd)
        {
            LrdCIndex = GetIndexFromIntsLR(LrdI, chipRow, chipColumn);
            LToRDiagonals[LrdI].cells[LrdCIndex].fillValue = fill;
        }
        if (Rld)
        {
            RldCIndex = GetIndexFromIntsRL(RldI, chipRow, chipColumn);
            RToLDiagonals[LrdI].cells[RldCIndex].fillValue = fill;
        }

        numberOfMoves++;
        StartCheckingForWin(chipRow, chipColumn, fill, Lrd, LrdI, LrdCIndex, Rld, RldI, RldCIndex);
    }

    void StartCheckingForWin(int chipRow, int chipColumn, string fill, bool Lrd, int LrdI, int LrdCIndex, bool Rld, int RldI, int RldCIndex)
    {
        HorizontalWinLogic(chipRow, fill);
        VerticalWinLogic(chipColumn, fill);




        Debug.Log("finished checking");

        redTurn = !redTurn;

        chipInstantiationScript.redTurnCI = redTurn;
        chipInstantiationScript.readyToInstantiate = true;
    }

    void HorizontalWinLogic(int row, string fill)
    {
        HorizontalCell[] colorInhabitingArray = GetMatchingCellsInRow(row, fill);
        
        //want to not make this for loop, this is just quick fix
        int[] columnNumbersArray = new int[colorInhabitingArray.Length];

        for (int i = 0; i < colorInhabitingArray.Length; i++)
        {
            columnNumbersArray[i] = colorInhabitingArray[i].columnNumber;
        }
        
        if (!(colorInhabitingArray.Length < 4))
        {
            if (columnNumbersArray.Contains(3))
            {
                int threeIndex = Array.IndexOf(columnNumbersArray, 3);
                bool leftSide;
                bool rightSide;

                if (threeIndex != 0)
                {
                    leftSide = (columnNumbersArray[threeIndex - 1] == 2);
                }
                else
                {
                    leftSide = false;
                }

                if (threeIndex != 6 && !((threeIndex + 1) >= columnNumbersArray.Length))
                {
                    rightSide = (columnNumbersArray[threeIndex + 1] == 4);
                }
                else
                {
                    rightSide = false;
                }

                if (leftSide && rightSide)
                {
                    if (columnNumbersArray.Contains(1) || columnNumbersArray.Contains(5))
                    {
                        Debug.Log("Horiz Win");
                    }
                }
                else if (leftSide)
                {
                    if (columnNumbersArray.Contains(0))
                    {
                        int zeroIndex = Array.IndexOf(columnNumbersArray, 0);
                        if ((threeIndex - zeroIndex) == 3)
                        {
                            Debug.Log("Horiz Win");
                        }
                    }
                }
                else if (rightSide)
                {
                    if (columnNumbersArray.Contains(6))
                    {
                        int sixIndex = Array.IndexOf(columnNumbersArray, 6);
                        if ((sixIndex - threeIndex) == 3)
                        {
                            Debug.Log("Horiz Win");
                        }
                    }
                }
            }
        }
    }

    public HorizontalCell[] GetMatchingCellsInRow(int rowIndex, string targetChipValue)
    {
        HorizontalCell[] cellsInLine = rowsArray[rowIndex].cells;

        // Creates a brand new array containing ONLY cells that match targetChipValue
        HorizontalCell[] matchingCells = Array.FindAll(cellsInLine, cell => cell.fillValue == targetChipValue);

        return matchingCells;
    }

    void VerticalWinLogic(int column, string fill)
    {
        VerticalCell[] colorInhabitingArray = GetMatchingCellsInColumn(column, fill);
        if (colorInhabitingArray.Length == 2 && (colorInhabitingArray[1].rowNumber - colorInhabitingArray[0].rowNumber) == 1)
        {
            Debug.Log("Win");
        }
    }

    public VerticalCell[] GetMatchingCellsInColumn(int columnIndex, string targetChipValue)
    {
        VerticalCell[] cellsInLine = columnsArray[columnIndex].cells;

        // Creates a brand new array containing ONLY cells that match targetChipValue
        VerticalCell[] matchingCells = Array.FindAll(cellsInLine, cell => cell.fillValue == targetChipValue);

        return matchingCells;
    }

    //void Diagonal4Guar()

    //void Diagonal3Guar()

    //void Diagonal2Guar()
}