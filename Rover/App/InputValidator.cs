using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MarsRover.App.Interfaces;

namespace MarsRover.App
{
    public class InputValidator : IInputValidator
    {

        /// <summary>This method validates the instructions sent by the users</summary>
        /// <param><c>strToValidate</c>String made up of only L R and M</param>
        public bool ValidateInstructions(string strToValidate)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(strToValidate))
                {
                    return false;
                }

                //we know there's some instruction so turn them to uppercase and
                //make sure they are all Chars and L, R, or M
                return strToValidate.ToUpper().All(x => char.IsLetter(x) || x == 'L' || x == 'R' || x == 'M');

            }
            catch //we don't care what happened...its invalid
            {
                return false;
            }
        }

        /// <summary>This method validates the the landing position sent by the users</summary>
        /// <param><c>strToValidate</c>String made up of 2 Ints seperated by spaces</param>
        public bool ValidateLandingPosition(string strToValidate, int grid_x, int grid_y)
        {
            try
            {
                //trim whitespace 
                strToValidate = strToValidate.Trim();

                //split by space into array
                var ary = strToValidate.Split(' ');

                //make sure there are two values
                if (ary.Length == 3)
                {
                    var x = int.Parse(ary[0]); //parse the values to ints... if they are not ints they will throw ex.
                    var y = int.Parse(ary[1]);

                    //make sure the rover landed in the grid
                    if (x < 0 || x > grid_x || y < 0 || y > grid_y)
                    {
                        return false;
                    }

                    //if were here its all up to this last check
                    //validate the last char must one of N E S W
                    var dir = ary[2].ToUpper();
                    return (dir == "N" || dir == "E" || dir == "S" || dir == "W");

                }
                else
                {
                    return false;
                }

            }
            catch //we don't care what happened...its invalid
            {
                return false;
            }
        }
        
        /// <summary>This method validates the grid boundaries sent by the users</summary>
        /// <param><c>strToValidate</c>String made up of 2 Ints and a Char of of N,E,S,W 
        /// seperated by spaces</param>
        public bool ValidateBoundaries(string strToValidate)
        {
            try
            {
                //trim whitespace 
                strToValidate = strToValidate.Trim();

                //split by space into array
                var ary = strToValidate.Split(' ');

                //make sure there are two values
                if (ary.Length == 2)
                {
                    //parse the values to ints... if they are not ints they will throw ex.
                    var x = int.Parse(ary[0]);
                    var y = int.Parse(ary[1]);

                    //make sure the grid x and y > 0
                    return x > 0 && y > 0;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception) //we don't care which exception is caught... it still fails validation
            {
                return false;
            }
        }
    }

}
