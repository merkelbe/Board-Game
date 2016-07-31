using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BoardGame
{
    class GameBoard
    {
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //// FIELDS
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // View 
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        int x;
        int y;

        List<double> zoomOptions = new List<double>(){.05,.1,.15,.2,.25,.3,.35,.4,.45,.5,.55,.6,.65,.7,.75,.8,.85,.9,.95,1,
            1.05,1.1,1.15,1.2,1.25,1.3,1.35,1.4,1.45,1.5,1.55,1.6,1.65,1.7,1.75,1.8,1.85,1.9,1.95,2};
        double currentZoomOption;

        int selectedSpaceRowIndex;
        int selectedSpaceColIndex;

        int baseHexSpaceWidth;
        int baseHexSpaceHeight;

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Data
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        internal enum BoardSpaceOffsetType { ToggleWithUpStart, ToggleWithDownStart, Ascending, Descending }
        BoardSpaceOffsetType boardSpaceOffsetType;
               
        BoardSpace[,] gameBoard;
        int[,] gameBoardXCoords;
        int[,] gameBoardYCoords;

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //// CONSTRUCTOR
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        internal GameBoard(BoardSpaceOffsetType _boardSpaceOffsetType, int _x, int _y, int _hexSpaceWidth, int _hexSpaceHeight)
        {

            ClearBoardOfSpaces();
            boardSpaceOffsetType = _boardSpaceOffsetType;
            x = _x;
            y = _y;
            baseHexSpaceWidth = _hexSpaceWidth;
            baseHexSpaceHeight = _hexSpaceHeight;
            currentZoomOption = .2;

            selectedSpaceRowIndex = 0;
            selectedSpaceColIndex = 0;

            updateZoomOfBoardSpaces();
            setBoardXYCoords();
            
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //// METHODS
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Private Methods

        

        private void growBoardHorizontally<T>(ref T[,] _board)
        {
            T[,] newBoard = new T[_board.GetLength(0), _board.GetLength(1) * 2];
            for (int rowNum = 0; rowNum < _board.GetLength(0); ++rowNum)
            {
                for (int colNum = 0; colNum < _board.GetLength(1); ++colNum)
                {
                    if (_board[rowNum, colNum] != null)
                    {
                        newBoard[rowNum, colNum] = _board[rowNum, colNum];
                    }
                }
            }
            _board = newBoard;
        }

        private void growBoardVertically<T>(ref T[,] _board)
        {
            T[,] newBoard = new T[_board.GetLength(0) * 2, _board.GetLength(1)];
            for (int rowNum = 0; rowNum < _board.GetLength(0); ++rowNum)
            {
                for (int colNum = 0; colNum < _board.GetLength(1); ++colNum)
                {
                    if (_board[rowNum, colNum] != null)
                    {
                        newBoard[rowNum, colNum] = _board[rowNum, colNum];
                    }
                }
            }
            _board = newBoard;
        }

        internal void ClearBoardOfSpaces()
        {
            gameBoard = new BoardSpace[1, 1];
            gameBoardXCoords = new int[1, 1];
            gameBoardYCoords = new int[1, 1];
        }
        
        #endregion

        private void setBoardXYCoords()
        {
            switch (boardSpaceOffsetType)
            {
                case BoardSpaceOffsetType.ToggleWithUpStart:
                    {
                        int offset = baseHexSpaceHeight / 2;
                        for (int colNum = 0; colNum < gameBoard.GetLength(1); ++colNum)
                        {
                            offset = offset == 0 ? baseHexSpaceHeight / 2 : 0;
                            for (int rowNum = 0; rowNum < gameBoard.GetLength(0); ++rowNum)
                            {
                                if (gameBoard[rowNum, colNum] != null)
                                {
                                    //int casting where it is to make uniform spaces between board pieces
                                    gameBoard[rowNum, colNum].X = x + colNum * (int)(baseHexSpaceWidth * 3 / 4 * currentZoomOption);
                                    gameBoard[rowNum, colNum].Y = y + rowNum * (int)(baseHexSpaceHeight * currentZoomOption) + (int)(offset * currentZoomOption);
                                }
                            }
                        }
                        break;
                    }
                case BoardSpaceOffsetType.ToggleWithDownStart:
                    {
                        int offset = 0;
                        for (int colNum = 0; colNum < gameBoard.GetLength(1); ++colNum)
                        {
                            offset = offset == 0 ? baseHexSpaceHeight / 2 : 0;
                            for (int rowNum = 0; rowNum < gameBoard.GetLength(0); ++rowNum)
                            {
                                if (gameBoard[rowNum, colNum] != null)
                                {
                                    //int casting where it is to make uniform spaces between board pieces
                                    gameBoard[rowNum, colNum].X = x + colNum * (int)(baseHexSpaceWidth * 3 / 4 * currentZoomOption);
                                    gameBoard[rowNum, colNum].Y = y + rowNum * (int)(baseHexSpaceHeight * currentZoomOption) + (int)(offset * currentZoomOption);
                                }
                            }
                        }
                        break;
                    }
                case BoardSpaceOffsetType.Ascending:
                    {

                        break;
                    }
                case BoardSpaceOffsetType.Descending:
                    {

                        break;
                    }
                default:
                    {
                        throw new Exception("Unknown board space offset type: " + boardSpaceOffsetType);
                    }
            }
        }

        private void updateZoomOfBoardSpaces()
        {
            for (int rowNum = 0; rowNum < gameBoard.GetLength(0); ++rowNum)
            {
                for (int colNum = 0; colNum < gameBoard.GetLength(1); ++colNum)
                {
                    if (gameBoard[rowNum, colNum] != null)
                    {
                        gameBoard[rowNum, colNum].UpdateZoom(currentZoomOption);
                    }
                }
            }
        }

        private void updateSelectedSpace()
        {
            for (int rowIndex = 0; rowIndex < gameBoard.GetLength(0); ++rowIndex)
            {
                for (int colIndex = 0; colIndex < gameBoard.GetLength(1); ++colIndex)
                {
                    if (gameBoard[rowIndex, colIndex] != null)
                    {
                        if (rowIndex == selectedSpaceRowIndex && colIndex == selectedSpaceColIndex)
                        {
                            gameBoard[rowIndex, colIndex].Selected = true;
                        }
                        else
                        {
                            gameBoard[rowIndex, colIndex].Selected = false;
                        }
                    }
                }
            }
        }




        internal void Draw(SpriteBatch _spriteBatch)
        {
            for (int rowNum = 0; rowNum < gameBoard.GetLength(0); ++rowNum)
            {
                for (int colNum = 0; colNum < gameBoard.GetLength(1); ++colNum)
                {
                    if (gameBoard[rowNum, colNum] != null)
                    {
                        gameBoard[rowNum, colNum].Draw(_spriteBatch);
                    }
                }
            }
        }



        #region Private Methods

        //Logic for getting all spaces connected to an input space.  Currently only set up for one board game space type (hexes toggle starting with higher position)
        private List<Tuple<int, int>> getConnectedSpaces(Tuple<int, int> _gameBoardSpace)
        {
            List<Tuple<int, int>> connectedSpaces = new List<Tuple<int, int>>();
            int row = _gameBoardSpace.Item1;
            int col = _gameBoardSpace.Item2;
            switch (boardSpaceOffsetType)
            {
                case BoardSpaceOffsetType.ToggleWithUpStart:
                    {
                        if (row - 1 >= 0 && gameBoard[row-1,col] != null)
                        {
                            connectedSpaces.Add(new Tuple<int, int>(row - 1, col));
                        }
                        if (row + 1 < gameBoard.GetLength(0) && gameBoard[row + 1, col] != null)
                        {
                            connectedSpaces.Add(new Tuple<int, int>(row + 1, col));
                        }
                        if (col % 2 == 0)
                        {
                            if (col - 1 >= 0)
                            {
                                if (row - 1 >= 0)
                                {
                                    if (gameBoard[row - 1, col - 1] != null)
                                    {
                                        connectedSpaces.Add(new Tuple<int, int>(row - 1, col - 1));
                                    }
                                }
                                if (gameBoard[row, col - 1] != null)
                                {
                                    connectedSpaces.Add(new Tuple<int, int>(row, col - 1));
                                }
                            }
                            if (col + 1 < gameBoard.GetLength(1))
                            {
                                if (row - 1 >= 0)
                                {
                                    if (gameBoard[row - 1, col + 1] != null)
                                    {
                                        connectedSpaces.Add(new Tuple<int, int>(row - 1, col + 1));
                                    }
                                }
                                if (gameBoard[row, col + 1] != null)
                                {
                                    connectedSpaces.Add(new Tuple<int, int>(row, col + 1));
                                }
                            }
                        }
                        else
                        {
                            if (gameBoard[row, col - 1] != null)
                            {
                                connectedSpaces.Add(new Tuple<int, int>(row, col - 1));
                            }
                            if (row + 1 < gameBoard.GetLength(0))
                            {
                                if (gameBoard[row + 1, col - 1] != null)
                                {
                                    connectedSpaces.Add(new Tuple<int, int>(row + 1, col - 1));
                                }
                            }
                            if (col + 1 < gameBoard.GetLength(1))
                            {
                                if (gameBoard[row, col + 1] != null)
                                {
                                    connectedSpaces.Add(new Tuple<int,int>(row,col+1));
                                }
                                if (row + 1 < gameBoard.GetLength(0))
                                {
                                    if (gameBoard[row + 1, col + 1] != null)
                                    {
                                        connectedSpaces.Add(new Tuple<int, int>(row + 1, col + 1));
                                    }
                                }
                            }
                        }
                        break;
                    }
                default:
                    {
                        throw new Exception("Current board game type is unsupported for funtion.");
                    }
            }
            return connectedSpaces;
        }

        //Calculates the distance between any 
        private double getDistanceBetweenPoints(Tuple<int, int> _point1, Tuple<int, int> _point2)
        {
            return Math.Sqrt(Math.Pow(_point1.Item1 - _point2.Item1, 2) + Math.Pow(_point1.Item2 - _point2.Item2, 2));
        }

        #endregion

        #region Internal Methods

        internal void HandleClick(ClickInfo _clickInfo)
        {
            switch (_clickInfo.ClickType)
            {
                case g.InputType.LeftClick:
                    {
                        BoardSpace nearestBoardSpace = this.getClosestBoardSpace(_clickInfo.EndingCoordinates);
                        if (nearestBoardSpace.CollisionRectangle.Contains(_clickInfo.EndingCoordinates.Item1, _clickInfo.EndingCoordinates.Item2))
                        {
                            this.moveSelectedSpace(nearestBoardSpace.RowIndex, nearestBoardSpace.ColIndex);
                        }
                        break;
                    }
                case g.InputType.LeftDoubleClick:
                    {
                        BoardSpace nearestBoardSpace = this.getClosestBoardSpace(_clickInfo.EndingCoordinates);
                        if (nearestBoardSpace.CollisionRectangle.Contains(_clickInfo.EndingCoordinates.Item1, _clickInfo.EndingCoordinates.Item2))
                        {
                            this.selectConnectedSpaces(nearestBoardSpace);
                        }
                        break;
                    }
                default:
                    {
                        throw new Exception("Unknown click type: " + _clickInfo.ClickType);
                    }
            }
        }

        internal void ProcessPressedKeys(List<Keys> _pressedKeys, KeyboardState _keyboardState)
        {
            foreach (Keys pressedKey in _pressedKeys)
            {
                switch (pressedKey)
                {
                    case Keys.Up:
                        {
                            if ((_keyboardState.IsKeyDown(Keys.LeftControl) || _keyboardState.IsKeyDown(Keys.RightControl)) &&
                                !_keyboardState.IsKeyDown(Keys.LeftShift) && !_keyboardState.IsKeyDown(Keys.RightShift))
                            {
                                this.increaseZoom();
                            }
                            else if ((_keyboardState.IsKeyDown(Keys.LeftShift) || _keyboardState.IsKeyDown(Keys.RightShift)) &&
                                !_keyboardState.IsKeyDown(Keys.LeftControl) && !_keyboardState.IsKeyDown(Keys.RightControl))
                            {
                                this.shiftBoardUp();
                            }
                            else if (!_keyboardState.IsKeyDown(Keys.LeftControl) && !_keyboardState.IsKeyDown(Keys.RightControl) &&
                                !_keyboardState.IsKeyDown(Keys.LeftShift) && !_keyboardState.IsKeyDown(Keys.RightShift))
                            {
                                this.moveSelectedSpaceUp();
                            }
                            break;
                        }
                    case Keys.Right:
                        {
                            if (_keyboardState.IsKeyDown(Keys.LeftShift) || _keyboardState.IsKeyDown(Keys.RightShift))
                            {
                                this.shiftBoardRight();
                            }
                            else
                            {
                                this.moveSelectedSpaceRight();
                            }
                            break;
                        }
                    case Keys.Down:
                        {
                            if ((_keyboardState.IsKeyDown(Keys.LeftControl) || _keyboardState.IsKeyDown(Keys.RightControl)) &&
                                !_keyboardState.IsKeyDown(Keys.LeftShift) && !_keyboardState.IsKeyDown(Keys.RightShift))
                            {
                                this.decreaseZoom();
                            }
                            else if ((_keyboardState.IsKeyDown(Keys.LeftShift) || _keyboardState.IsKeyDown(Keys.RightShift)) &&
                                !_keyboardState.IsKeyDown(Keys.LeftControl) && !_keyboardState.IsKeyDown(Keys.RightShift))
                            {
                                this.shiftBoardDown();
                            }
                            else if (!_keyboardState.IsKeyDown(Keys.LeftControl) && !_keyboardState.IsKeyDown(Keys.RightControl) &&
                                !_keyboardState.IsKeyDown(Keys.LeftShift) && !_keyboardState.IsKeyDown(Keys.RightShift))
                            {
                                this.moveSelectedSpaceDown();
                            }
                            break;
                        }
                    case Keys.Left:
                        {
                            if (_keyboardState.IsKeyDown(Keys.LeftShift) || _keyboardState.IsKeyDown(Keys.RightShift))
                            {
                                this.shiftBoardLeft();
                            }
                            else
                            {
                                this.moveSelectedSpaceLeft();
                            }
                            break;
                        }
                }
            }
        }

        //Selects all spaces adjacent to the input board space
        private void selectConnectedSpaces(Tuple<int, int> _gameBoardSpace)
        {
            foreach (Tuple<int, int> boardSpaceCoords in getConnectedSpaces(_gameBoardSpace))
            {
                int row = boardSpaceCoords.Item1;
                int col = boardSpaceCoords.Item2;
                gameBoard[row, col].Selected = true;
            }
        }

        //Selects all spaces adjacent to the input board space
        private void selectConnectedSpaces(BoardSpace _gameBoardSpace)
        {
            int row;
            int col;
            for (row = 0; row < gameBoard.GetLength(0); ++row)
            {
                for (col = 0; col < gameBoard.GetLength(1); ++col)
                {
                    if (gameBoard[row, col] != null)
                    {
                        if (gameBoard[row, col].X == _gameBoardSpace.X && gameBoard[row, col].Y == _gameBoardSpace.Y)
                        {
                            selectConnectedSpaces(new Tuple<int, int>(row, col));
                            return;
                        }
                    }
                }
            }
            
        }

        //Returns the closest space to a set of coordinates.  Can be used to figure out if a space has been clicked on.
        private BoardSpace getClosestBoardSpace(Tuple<int, int> _coordinate)
        {
            double currentClosestDistance = double.MaxValue;
            BoardSpace currentClosestSpace = null;

            for (int row = 0; row < gameBoard.GetLength(0); ++row)
            {
                for (int col = 0; col < gameBoard.GetLength(1); ++col)
                {
                    if (gameBoard[row, col] != null)
                    {
                        double dist = getDistanceBetweenPoints(_coordinate, gameBoard[row, col].Center);
                        if (dist < currentClosestDistance)
                        {
                            currentClosestDistance = dist;
                            currentClosestSpace = gameBoard[row, col];
                        }
                    }
                }
            }
            return currentClosestSpace;
        }

        private BoardSpace getClosestBoardSpace(Tuple<int, int> _coordinate, out bool _coordinateContainedInSpace)
        {
            double currentClosestDistance = double.MaxValue;
            BoardSpace currentClosestSpace = null;
            _coordinateContainedInSpace = false;

            for (int row = 0; row < gameBoard.GetLength(0); ++row)
            {
                for (int col = 0; col < gameBoard.GetLength(1); ++col)
                {
                    if (gameBoard[row, col] != null)
                    {
                        double dist = getDistanceBetweenPoints(_coordinate, gameBoard[row, col].Center);
                        if (dist < currentClosestDistance)
                        {
                            currentClosestDistance = dist;
                            currentClosestSpace = gameBoard[row, col];
                            _coordinateContainedInSpace = currentClosestSpace.CollisionRectangle.Contains(_coordinate.Item1, _coordinate.Item2);
                        }
                    }
                }
            }
            return currentClosestSpace;
        }

        //Logic for shifting the game board around the view screen.  Used mostly to figure out initial xy-coords for setting up the board game.
        private void shiftBoardRight()
        {
            this.x += (int)(1200 * currentZoomOption);
            setBoardXYCoords();
        }

        private void shiftBoardLeft()
        {
            this.x -= (int)(1200 * currentZoomOption);
            setBoardXYCoords();
        }

        private void shiftBoardDown()
        {
            this.y += (int)(800 * currentZoomOption);
            setBoardXYCoords();
        }

        private void shiftBoardUp()
        {
            this.y -= (int)(800 * currentZoomOption);
            setBoardXYCoords();
        }

        //Logic for increasing/decreasing zoom of game board.  Used mostly to figure out initial zoom for setting up the board game.

        private void increaseZoom()
        {
            currentZoomOption = zoomOptions[Math.Min(zoomOptions.IndexOf(currentZoomOption) + 1, zoomOptions.Count - 1)];
            setBoardXYCoords();
            updateZoomOfBoardSpaces();

        }

        private void decreaseZoom()
        {
            currentZoomOption = zoomOptions[Math.Max(zoomOptions.IndexOf(currentZoomOption) - 1, 0)];
            setBoardXYCoords();
            updateZoomOfBoardSpaces();
        }

        //Logic for moving "selected space" around.
        private void moveSelectedSpaceRight()
        {
            switch (boardSpaceOffsetType)
            {
                case BoardSpaceOffsetType.ToggleWithUpStart:
                    {
                        if (selectedSpaceColIndex + 1 < gameBoard.GetLength(1))
                        {
                            if (gameBoard[selectedSpaceRowIndex, selectedSpaceColIndex + 1] != null)
                            {
                                ++selectedSpaceColIndex;
                                updateSelectedSpace();
                            }
                            else
                            {
                                if (selectedSpaceRowIndex % 2 != 0)
                                {
                                    if (selectedSpaceRowIndex + 1 < gameBoard.GetLength(0))
                                    {
                                        if (gameBoard[selectedSpaceRowIndex + 1, selectedSpaceColIndex + 1] != null)
                                        {
                                            ++selectedSpaceRowIndex;
                                            ++selectedSpaceColIndex;
                                            updateSelectedSpace();
                                        }
                                    }
                                }
                                else
                                {
                                    if (selectedSpaceRowIndex - 1 >= 0)
                                    {
                                        if (gameBoard[selectedSpaceRowIndex - 1, selectedSpaceColIndex + 1] != null)
                                        {
                                            --selectedSpaceRowIndex;
                                            ++selectedSpaceColIndex;
                                            updateSelectedSpace();
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    }
                default:
                    {
                        throw new Exception("Behavior not implemented yet.");
                    }
            }
        }

        private void moveSelectedSpaceLeft()
        {
            switch (boardSpaceOffsetType)
            {
                case BoardSpaceOffsetType.ToggleWithUpStart:
                    {
                        if (selectedSpaceColIndex - 1 >= 0)
                        {
                            if (gameBoard[selectedSpaceRowIndex, selectedSpaceColIndex - 1] != null)
                            {
                                --selectedSpaceColIndex;
                                updateSelectedSpace();
                            }
                            else
                            {
                                if (selectedSpaceRowIndex % 2 != 0)
                                {
                                    if (selectedSpaceRowIndex + 1 < gameBoard.GetLength(0))
                                    {
                                        if (gameBoard[selectedSpaceRowIndex + 1, selectedSpaceColIndex - 1] != null)
                                        {
                                            ++selectedSpaceRowIndex;
                                            --selectedSpaceColIndex;
                                            updateSelectedSpace();
                                        }
                                    }
                                }
                                else
                                {
                                    if (selectedSpaceRowIndex - 1 >= 0)
                                    {
                                        if (gameBoard[selectedSpaceRowIndex - 1, selectedSpaceColIndex - 1] != null)
                                        {
                                            --selectedSpaceRowIndex;
                                            --selectedSpaceColIndex;
                                            updateSelectedSpace();
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    }
                default:
                    {
                        throw new Exception("Behavior not implemented yet.");
                    }
            }
        }

        private void moveSelectedSpaceUp()
        {
            switch (boardSpaceOffsetType)
            {
                case BoardSpaceOffsetType.ToggleWithUpStart:
                    {
                        if (selectedSpaceRowIndex - 1 >= 0)
                        {
                            if (gameBoard[selectedSpaceRowIndex - 1, selectedSpaceColIndex] != null)
                            {
                                --selectedSpaceRowIndex;
                                updateSelectedSpace();
                            }
                        }
                        break;
                    }
                default:
                    {
                        throw new Exception("Behavior not implemented yet.");
                    }
            }
        }

        private void moveSelectedSpaceDown()
        {
            switch (boardSpaceOffsetType)
            {
                case BoardSpaceOffsetType.ToggleWithUpStart:
                    {
                        if (selectedSpaceRowIndex + 1 < gameBoard.GetLength(0))
                        {
                            if (gameBoard[selectedSpaceRowIndex + 1, selectedSpaceColIndex] != null)
                            {
                                ++selectedSpaceRowIndex;
                                updateSelectedSpace();
                            }
                        }
                        break;
                    }
                default:
                    {
                        throw new Exception("Behavior not implemented yet.");
                    }
            }
        }

        private void moveSelectedSpace(int _row, int _col)
        {
            if (this.gameBoard[_row, _col] != null)
            {
                this.selectedSpaceRowIndex = _row;
                this.selectedSpaceColIndex = _col;
                this.updateSelectedSpace();
            }
        }

        private void selectSpace(int _rowIndex, int _colIndex)
        {
            gameBoard[_rowIndex, _colIndex].Selected = true;
        }

        private void deselectSpace(int _rowIndex, int _colIndex)
        {
            gameBoard[_rowIndex, _colIndex].Selected = false;
        }

        //Updates game board data, growing the game board if necessary. 
        internal void AddSpace(BoardSpace _boardGameSpace)
        {
            int rowIndex = _boardGameSpace.RowIndex;
            int colIndex = _boardGameSpace.ColIndex;
            if (colIndex >= gameBoard.GetLength(1))
            {
                growBoardHorizontally(ref gameBoard);
                growBoardHorizontally(ref gameBoardXCoords);
                growBoardHorizontally(ref gameBoardYCoords);
            }
            if (rowIndex >= gameBoard.GetLength(0))
            {
                growBoardVertically(ref gameBoard);
                growBoardVertically(ref gameBoardXCoords);
                growBoardVertically(ref gameBoardYCoords);
            }
            gameBoard[rowIndex, colIndex] = _boardGameSpace;
            //Primarily used so the selected space is shown on start up.
            if (rowIndex == selectedSpaceRowIndex && colIndex == selectedSpaceColIndex) gameBoard[rowIndex, colIndex].Selected = true;
            setBoardXYCoords();
            _boardGameSpace.UpdateZoom(currentZoomOption);
        }

        #endregion
    }
}
