using System;

namespace MarsRover.App.Interfaces
{
    public interface IInputValidator
    {
        bool ValidateBoundaries(string strToValidate);
        bool ValidateLandingPosition(string strToValidate, int grid_x, int grid_y);
        bool ValidateInstructions(string strToValidate);
    }
}