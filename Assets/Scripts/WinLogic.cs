using System;
using System.Drawing;
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

    //int GetIndexFromIntsLR(int diagonalIndex, int row, int column)
    //{
    //    // Combines row first, then column (e.g., row = 0, column = 2 becomes "02")
    //    string targetCoords = $"{row}{column}";

    //    DiagonalCell[] cellsInLine = LToRDiagonals[diagonalIndex].cells;

    //    // Returns the second-level index where cellCoords matches, or -1 if not found
    //    int foundIndex = Array.FindIndex(cellsInLine, cell => cell.cellCoords == targetCoords);

    //    return foundIndex;
    //}
    //int GetIndexFromIntsRL(int diagonalIndex, int row, int column)
    //{
    //    // Combines row first, then column (e.g., row = 0, column = 2 becomes "02")
    //    string targetCoords = $"{row}{column}";

    //    DiagonalCell[] cellsInLine = RToLDiagonals[diagonalIndex].cells;

    //    // Returns the second-level index where cellCoords matches, or -1 if not found
    //    int foundIndex = Array.FindIndex(cellsInLine, cell => cell.cellCoords == targetCoords);

    //    return foundIndex; 
    //}

    public void UpdateChip(int chipRow, int chipColumn, string fill, bool Lrd, int LrdI, bool Rld, int RldI)
    {
        rowsArray[chipRow].cells[chipColumn].fillValue = fill;
        columnsArray[chipColumn].cells[chipRow].fillValue = fill;

        if (chipRow == 0)
        {
            chipInstantiationScript.columnsFilled[chipColumn] = true;
        }

        int LrdCIndex = 0;
        int RldCIndex = 0;

        //Debug.Log(Lrd);
        //Debug.Log(Rld);
        //Debug.Log(LrdI);
        //Debug.Log(RldI);

        if (Lrd)
        {
            //LrdCIndex = GetIndexFromIntsLR(LrdI, chipRow, chipColumn);
            //Debug.Log(chipColumn - LToRDiagonals[LrdI].cells[0].columnNumber);
            //Debug.Log(LToRDiagonals[LrdI].cells[0].columnNumber);
            LToRDiagonals[LrdI].cells[chipColumn - LToRDiagonals[LrdI].cells[0].columnNumber].fillValue = fill;
        }
        if (Rld)
        {
            //RldCIndex = GetIndexFromIntsRL(RldI, chipRow, chipColumn);
            //Debug.Log(chipColumn - RToLDiagonals[RldI].cells[0].columnNumber);
            //Debug.Log(RToLDiagonals[RldI].cells[0].columnNumber);
            RToLDiagonals[RldI].cells[chipColumn - RToLDiagonals[RldI].cells[0].columnNumber].fillValue = fill;
        }

        numberOfMoves++;
        StartCheckingForWin(chipRow, chipColumn, fill, Lrd, LrdI, LrdCIndex, Rld, RldI, RldCIndex);
    }

    void StartCheckingForWin(int chipRow, int chipColumn, string fill, bool Lrd, int LrdI, int LrdCIndex, bool Rld, int RldI, int RldCIndex)
    {
        HorizontalWinLogic(chipRow, fill);
        VerticalWinLogic(chipColumn, fill);

        if (Lrd)
        {
            if (LrdI == 0 || LrdI == 5)
            {
                Diagonal4Guar(Lrd, LrdI, false, 0, fill);
            }
            else if (LrdI == 1 || LrdI == 4)
            {
                Diagonal3Guar(Lrd, LrdI, false, 0, fill);
            }
            else if (LrdI == 2 || LrdI == 3)
            {
                Diagonal2Guar(Lrd, LrdI, false, 0, fill);
            }
        }
        if (Rld)
        {
            if (RldI == 0 || RldI == 5)
            {
                Diagonal4Guar(false, 0, Rld, RldI, fill);
            }
            else if (RldI == 1 || RldI == 4)
            {
                Diagonal3Guar(false, 0, Rld, RldI, fill);
            }
            else if (RldI == 2 || RldI   == 3)
            {
                Diagonal2Guar(false, 0, Rld, RldI, fill);
            }
        }
        
        if (numberOfMoves == 42)
        {
            Debug.Log("How in the world did you draw in Connect 4?");
        }

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
        int rowsFilled = RowsFilledInColumn(column);
        if (!(rowsFilled < 4))
        {
            VerticalCell[] colorInhabitingArray = GetMatchingCellsInColumn(column, fill);

            //want to not make this for loop, this is just quick fix
            int[] rowNumbersArray = new int[colorInhabitingArray.Length];

            for (int i = colorInhabitingArray.Length - 1; i > -1; i--)
            {
                int l = colorInhabitingArray.Length - 1;
                //5 4 3 2 1 0 i values/color array rows
                //0 1 2 3 4 5 what I want
                rowNumbersArray[l - i] = colorInhabitingArray[l - i].rowNumber;
            }

            //Debug.Log(rowsFilled);
            //Debug.Log(string.Join(", ", rowNumbersArray));

            if (!(rowNumbersArray.Length < 4))
            {
                if (rowNumbersArray.Contains(2) && rowNumbersArray.Contains(3))
                {
                    int twoIndex = Array.IndexOf(rowNumbersArray, 2);
                    int threeIndex = Array.IndexOf(rowNumbersArray, 3);

                    bool below = false;
                    bool above = false;

                    if (twoIndex == 2)
                    {
                        Debug.Log("Vert Win");
                    }
                    else if (twoIndex == 1)
                    {
                        below = true;
                    }
                    else
                    {
                        below = false;
                    }

                    if (below)
                    {
                        if (!((threeIndex + 1) >= rowNumbersArray.Length))
                        {
                            above = (rowNumbersArray[threeIndex + 1] == 4);
                        }
                        else
                        {
                            above = false;
                        }

                        if (above)
                        {
                            Debug.Log("Vert Win");
                        }
                    }
                    else
                    {
                        bool doubleAbove = false;

                        if (!((threeIndex + 1) >= rowNumbersArray.Length))
                        {
                            above = (rowNumbersArray[threeIndex + 1] == 4);
                            if (!((threeIndex + 2) >= rowNumbersArray.Length))
                            {
                                doubleAbove = (rowNumbersArray[threeIndex + 2] == 5);
                            }

                            if (above && doubleAbove)
                            {
                                Debug.Log("Vert Win");
                            }
                        }
                    }
                }
            }
        }
    }

    public VerticalCell[] GetMatchingCellsInColumn(int columnIndex, string targetChipValue)
    {
        VerticalCell[] cellsInLine = columnsArray[columnIndex].cells;

        // Creates a brand new array containing ONLY cells that match targetChipValue
        VerticalCell[] matchingCells = Array.FindAll(cellsInLine, cell => cell.fillValue == targetChipValue);

        return matchingCells;
    }

    public int RowsFilledInColumn(int columnIndex)
    {
        VerticalCell[] cellsInLine = columnsArray[columnIndex].cells;

        // Creates a brand new array containing ONLY cells that match targetChipValue
        VerticalCell[] matchingCells = Array.FindAll(cellsInLine, cell => cell.fillValue != "e");

        return matchingCells.Length;
    }

    void Diagonal4Guar(bool Lr, int LrI, bool Rl, int RlI, string fill)
    {
        if (Lr)
        {
            DiagonalCell[] colorInhabitingArray = GetMatchingCellsInDiagonalLR(LrI, fill);

            Debug.Log(colorInhabitingArray.Length);

            //int[] columnNumbersArray = new int[colorInhabitingArray.Length];

            //for (int i = 0; i < colorInhabitingArray.Length; i++)
            //{
            //    columnNumbersArray[i] = colorInhabitingArray[i].columnNumber;
            //}

            //Debug.Log(string.Join(", ", columnNumbersArray));

            if  (colorInhabitingArray.Length == 4)
            {
                Debug.Log("LR 4 Guar Diagonal Win");
            }
        }
        if (Rl)
        {
            DiagonalCell[] colorInhabitingArray = GetMatchingCellsInDiagonalRL(RlI, fill);

            Debug.Log(colorInhabitingArray.Length);

            //int[] columnNumbersArray = new int[colorInhabitingArray.Length];

            //for (int i = 0; i < colorInhabitingArray.Length; i++)
            //{
            //    columnNumbersArray[i] = colorInhabitingArray[i].columnNumber;
            //}

            //Debug.Log(string.Join(", ", columnNumbersArray));

            if (colorInhabitingArray.Length == 4)
            {
                Debug.Log("RL 4 Guar Diagonal Win");
            }
        }
    }

    void Diagonal3Guar(bool Lr, int LrI, bool Rl, int RlI, string fill)
    {
        if (Lr)
        {
            DiagonalCell[] colorInhabitingArray = GetMatchingCellsInDiagonalLR(LrI, fill);

            //Debug.Log(colorInhabitingArray.Length);

            int[] columnNumbersArray = new int[colorInhabitingArray.Length];

            for (int i = 0; i < colorInhabitingArray.Length; i++)
            {
                columnNumbersArray[i] = colorInhabitingArray[i].columnNumber;
            }

            //Debug.Log(string.Join(", ", columnNumbersArray));

            if (!(columnNumbersArray.Length < 4))
            {
                if (LrI == 1)
                {
                    if (columnNumbersArray.Contains(1) && columnNumbersArray.Contains(2) && columnNumbersArray.Contains(3))
                    {
                        Debug.Log("LR 3 Guar Diagonal Win");
                    }
                }
                else if (LrI == 4)
                {
                    if (columnNumbersArray.Contains(3) && columnNumbersArray.Contains(4) && columnNumbersArray.Contains(5))
                    {
                        Debug.Log("LR 3 Guar Diagonal Win");
                    }
                }
            }
        }
        if (Rl)
        {
            DiagonalCell[] colorInhabitingArray = GetMatchingCellsInDiagonalRL(RlI, fill);

            //Debug.Log(colorInhabitingArray.Length);

            int[] columnNumbersArray = new int[colorInhabitingArray.Length];

            for (int i = 0; i < colorInhabitingArray.Length; i++)
            {
                columnNumbersArray[i] = colorInhabitingArray[i].columnNumber;
            }

            //Debug.Log(string.Join(", ", columnNumbersArray));

            if (!(columnNumbersArray.Length < 4))
            {
                if (RlI == 1)
                {
                    if (columnNumbersArray.Contains(1) && columnNumbersArray.Contains(2) && columnNumbersArray.Contains(3))
                    {
                        Debug.Log("RL 3 Guar Diagonal Win");
                    }
                }
                else if (RlI == 4)
                {
                    if (columnNumbersArray.Contains(3) && columnNumbersArray.Contains(4) && columnNumbersArray.Contains(5))
                    {
                        Debug.Log("RL 3 Guar Diagonal Win");
                    }
                }
            }
        }
    }

    void Diagonal2Guar(bool Lr, int LrI, bool Rl, int RlI, string fill)
    {
        if (Lr)
        {
            DiagonalCell[] colorInhabitingArray = GetMatchingCellsInDiagonalLR(LrI, fill);

            //Debug.Log(colorInhabitingArray.Length);

            int[] columnNumbersArray = new int[colorInhabitingArray.Length];

            for (int i = 0; i < colorInhabitingArray.Length; i++)
            {
                columnNumbersArray[i] = colorInhabitingArray[i].columnNumber;
            }

            //Debug.Log(string.Join(", ", columnNumbersArray));

            if (!(columnNumbersArray.Length < 4))
            {
                if (LrI == 2)
                {
                    int twoIndex = Array.IndexOf(columnNumbersArray, 2);
                    int threeIndex = Array.IndexOf(columnNumbersArray, 3);

                    bool leftSide = false;
                    bool rightSide = false;

                    if (twoIndex != 0)
                    {
                        leftSide = (columnNumbersArray[twoIndex - 1] == 1);
                    }
                    else
                    {
                        leftSide = false;
                    }

                    if (threeIndex != 5 && !((threeIndex + 1) >= columnNumbersArray.Length))
                    {
                        rightSide = (columnNumbersArray[threeIndex + 1] == 4);
                    }
                    else
                    {
                        rightSide = false;
                    }

                    if (leftSide && rightSide)
                    {
                        Debug.Log("LR 2 Guar Diagonal Win");
                    }
                    else if (leftSide)
                    {
                        if (twoIndex == 2)
                        {
                            Debug.Log("LR 2 Guar Diagonal Win");
                        }
                    }
                    else if (rightSide)
                    {
                        if (columnNumbersArray.Contains(5))
                        {
                            Debug.Log("LR 2 Guar Diagonal Win");
                        }
                    }
                }
                else if (LrI == 3)
                {
                    int threeIndex = Array.IndexOf(columnNumbersArray, 3);
                    int fourIndex = Array.IndexOf(columnNumbersArray, 4);

                    bool leftSide = false;
                    bool rightSide = false;

                    if (threeIndex != 0)
                    {
                        leftSide = (columnNumbersArray[threeIndex - 1] == 2);
                    }
                    else
                    {
                        leftSide = false;
                    }

                    if (fourIndex != 5 && !((fourIndex + 1) >= columnNumbersArray.Length))
                    {
                        rightSide = (columnNumbersArray[fourIndex + 1] == 5);
                    }
                    else
                    {
                        rightSide = false;
                    }

                    if (leftSide && rightSide)
                    {
                        Debug.Log("LR 2 Guar Diagonal Win");
                    }
                    else if (leftSide)
                    {
                        if (threeIndex == 3)
                        {
                            Debug.Log("LR 2 Guar Diagonal Win");
                        }
                    }
                    else if (rightSide)
                    {
                        if (columnNumbersArray.Contains(6))
                        {
                            Debug.Log("LR 2 Guar Diagonal Win");
                        }
                    }
                }
            }
        }
        if (Rl)
        {
            DiagonalCell[] colorInhabitingArray = GetMatchingCellsInDiagonalRL(RlI, fill);

            //Debug.Log(colorInhabitingArray.Length);

            int[] columnNumbersArray = new int[colorInhabitingArray.Length];

            for (int i = 0; i < colorInhabitingArray.Length; i++)
            {
                columnNumbersArray[i] = colorInhabitingArray[i].columnNumber;
            }

            //Debug.Log(string.Join(", ", columnNumbersArray));

            if (!(columnNumbersArray.Length < 4))
            {
                if (RlI == 2)
                {
                    int twoIndex = Array.IndexOf(columnNumbersArray, 2);
                    int threeIndex = Array.IndexOf(columnNumbersArray, 3);

                    bool leftSide = false;
                    bool rightSide = false;

                    if (twoIndex != 0)
                    {
                        leftSide = (columnNumbersArray[twoIndex - 1] == 1);
                    }
                    else
                    {
                        leftSide = false;
                    }

                    if (threeIndex != 5 && !((threeIndex + 1) >= columnNumbersArray.Length))
                    {
                        rightSide = (columnNumbersArray[threeIndex + 1] == 4);
                    }
                    else
                    {
                        rightSide = false;
                    }

                    if (leftSide && rightSide)
                    {
                        Debug.Log("RL 2 Guar Diagonal Win");
                    }
                    else if (leftSide)
                    {
                        if (twoIndex == 2)
                        {
                            Debug.Log("RL 2 Guar Diagonal Win");
                        }
                    }
                    else if (rightSide)
                    {
                        if (columnNumbersArray.Contains(5))
                        {
                            Debug.Log("RL 2 Guar Diagonal Win");
                        }
                    }
                }
                else if (RlI == 3)
                {
                    int threeIndex = Array.IndexOf(columnNumbersArray, 3);
                    int fourIndex = Array.IndexOf(columnNumbersArray, 4);

                    bool leftSide = false;
                    bool rightSide = false;

                    if (threeIndex != 0)
                    {
                        leftSide = (columnNumbersArray[threeIndex - 1] == 2);
                    }
                    else
                    {
                        leftSide = false;
                    }

                    if (fourIndex != 5 && !((fourIndex + 1) >= columnNumbersArray.Length))
                    {
                        rightSide = (columnNumbersArray[fourIndex + 1] == 5);
                    }
                    else
                    {
                        rightSide = false;
                    }

                    if (leftSide && rightSide)
                    {
                        Debug.Log("RL 2 Guar Diagonal Win");
                    }
                    else if (leftSide)
                    {
                        if (threeIndex == 3)
                        {
                            Debug.Log("RL 2 Guar Diagonal Win");
                        }
                    }
                    else if (rightSide)
                    {
                        if (columnNumbersArray.Contains(6))
                        {
                            Debug.Log("RL 2 Guar Diagonal Win");
                        }
                    }
                }
            }
        }
    }

    public DiagonalCell[] GetMatchingCellsInDiagonalLR(int diagonalIndex, string targetChipValue)
    {
        DiagonalCell[] cellsInLine = LToRDiagonals[diagonalIndex].cells;

        // Creates a brand new array containing ONLY cells that match targetChipValue
        DiagonalCell[] matchingCells = Array.FindAll(cellsInLine, cell => cell.fillValue == targetChipValue);

        return matchingCells;
    }
    public DiagonalCell[] GetMatchingCellsInDiagonalRL(int diagonalIndex, string targetChipValue)
    {
        DiagonalCell[] cellsInLine = RToLDiagonals[diagonalIndex].cells;

        // Creates a brand new array containing ONLY cells that match targetChipValue
        DiagonalCell[] matchingCells = Array.FindAll(cellsInLine, cell => cell.fillValue == targetChipValue);

        return matchingCells;
    }
}