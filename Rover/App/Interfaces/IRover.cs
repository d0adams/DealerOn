using System;

namespace MarsRover.App.Interfaces
{
    public interface IRover
    {
        void RotateRight();
        void RotateLeft();
        void Move(int units);
        void Drive(string instructions);
        string GetPosition();
    }
}