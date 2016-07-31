using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Input;

namespace BoardGame
{
    class MouseFilter
    {
        //Mouse Filter class adds to queue when it registers mouse commands.  Inputs are accessed and removed by parent game class.
        internal List<ClickInfo> ActionQueue;

        //internal bool LeftDoubleClick
        //{
        //    get { return LeftButtonInfo.DoubleClick; }
        //}
        //internal bool LeftSingleClick
        //{
        //    get { return LeftButtonInfo.SingleClick; }
        //}
        //internal bool RightDoubleClick
        //{
        //    get { return RightButtonInfo.DoubleClick; }
        //}
        //internal bool RightSingleClick
        //{
        //    get { return RightButtonInfo.SingleClick; }
        //}
        //internal Tuple<int, int> LeftStartingCoords
        //{
        //    get { return LeftButtonInfo.StartingCoordinates; }
        //}
        //internal Tuple<int, int> LeftEndingCoordinates
        //{
        //    get { return LeftButtonInfo.EndingCoordinates; }
        //}
        //internal Tuple<int, int> RightStartingCoords
        //{
        //    get { return RightButtonInfo.StartingCoordinates; }
        //}
        //internal Tuple<int, int> RightEndingCoords
        //{
        //    get { return RightButtonInfo.EndingCoordinates; }
        //}
        //TODO make this shit private yo!
        LeftClickInfo leftButtonInfo;
        RightClickInfo rightButtonInfo;

        internal MouseFilter()
        {
            leftButtonInfo = new LeftClickInfo(g.InputType.LeftClick, g.InputType.LeftDoubleClick);
            rightButtonInfo = new RightClickInfo(g.InputType.RightClick, g.InputType.RightDoubleClick);
        }

        internal void Update(MouseState _mouseState, ref List<ClickInfo> _actionQueue)
        {
            leftButtonInfo.Update(_mouseState, ref _actionQueue);
            rightButtonInfo.Update(_mouseState, ref _actionQueue);
        }
    }

    //TODO Set private.  Made public for debugging.
    internal abstract class ButtonInfo
    {
        g.InputType inputTypeSingle;
        g.InputType inputTypeDouble;
        bool lastUpdateIsDown;
        int stateChangeCount;
        //internal bool DoubleClick;
        //internal bool SingleClick;
        Tuple<int, int> startingCoordinates;
        Tuple<int, int> endingCoordinates;
        const int resetTimerInTicks = 10;
        private int currentTimer;

        internal ButtonInfo(g.InputType _inputTypeSingle, g.InputType _inputTypeDouble)
        {
            inputTypeSingle = _inputTypeSingle;
            inputTypeDouble = _inputTypeDouble;
        }

        internal abstract bool isButtonDown(MouseState _mouseState);

        internal void Update(MouseState _mouseState, ref List<ClickInfo> _actionQueue)
        {
            bool currentIsDown = isButtonDown(_mouseState);

            //Start of tracking condition
            if (currentIsDown && !lastUpdateIsDown && stateChangeCount == 0)
            {
                stateChangeCount = 1;
                currentTimer = resetTimerInTicks;
                startingCoordinates = new Tuple<int, int>(_mouseState.X, _mouseState.Y);
            }
            //Tracks changes in up/down states
            else if (currentIsDown != lastUpdateIsDown)
            {
                ++stateChangeCount;
                currentTimer += 5;
                //Ending coords of single click
                if (stateChangeCount == 2)
                {
                    endingCoordinates = new Tuple<int, int>(_mouseState.X, _mouseState.Y);
                }
            }
            lastUpdateIsDown = currentIsDown;
            --currentTimer;
            //End of tracking condition
            if (currentTimer <= 0 || stateChangeCount >= 4)
            {
                if (stateChangeCount >= 4)
                {
                    //Ending coords of double click. Overrides single click ending coords.
                    endingCoordinates = new Tuple<int, int>(_mouseState.X, _mouseState.Y);

                    _actionQueue.Add(new ClickInfo(inputTypeDouble, startingCoordinates, endingCoordinates));
                }
                else if (stateChangeCount >= 2)
                {
                    _actionQueue.Add(new ClickInfo(inputTypeSingle, startingCoordinates, endingCoordinates));
                }
                stateChangeCount = 0;
                currentTimer = 0;
            }
        }
    }

    class LeftClickInfo : ButtonInfo
    {
        public LeftClickInfo(g.InputType _inputTypeSingle, g.InputType _inputTypeDouble) : base(_inputTypeSingle, _inputTypeDouble)
        {
        }

        internal override bool isButtonDown(MouseState _mouseState)
        {
            return _mouseState.LeftButton == ButtonState.Pressed;
        }
    }

    class RightClickInfo : ButtonInfo
    {
        public RightClickInfo(g.InputType _inputTypeSingle, g.InputType _inputTypeDouble) : base(_inputTypeSingle, _inputTypeDouble)
        {
        }

        internal override bool isButtonDown(MouseState _mouseState)
        {
            return _mouseState.RightButton == ButtonState.Pressed;
        }
    }

    class ClickInfo
    {
        internal g.InputType ClickType;
        internal Tuple<int, int> StartingCoordinates;
        internal Tuple<int, int> EndingCoordinates;

        internal ClickInfo(g.InputType _clickType, Tuple<int,int> _startingCoordinates, Tuple<int,int> _endingCoordinates)
        {
            ClickType = _clickType;
            StartingCoordinates = _startingCoordinates;
            EndingCoordinates = _endingCoordinates;
        }
    }
}
