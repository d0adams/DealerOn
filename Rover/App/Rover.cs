using System;
using MarsRover.App.Interfaces;
using MarsRover.App;

namespace MarsRover.App
{

    public class Rover : IRover
    {
        
        ///The rover's current x position
        private int Current_X { get; set; }

        ///The rover's current y position
        private int Current_Y { get; set; }

        ///The rover's current z position
        private char Current_Heading { get; set; }

        private int Max_East { get; set; }
        private int Max_North { get; set; }

        /// <summary>Contructor for Rover Class setting its heading and location
        /// it also sets an internal grid of boundaries</summary>
        public Rover(int x, int y, char z, int max_east, int max_north)
        {
            //set the initial landing position
            this.Current_X = x;
            this.Current_Y = y;
            this.Current_Heading = z;

            //set the maximum distance the rover can travel east
            this.Max_East = max_east;
            //set the maximum distance the rover can travel north
            this.Max_North = max_north;
        }

        /// <summary>This method takes user input and attempts to move the
        /// rover.  It takes one or many commands in the form or 'L', 'R', and 'M'
        /// which represent Left, Right and Move (1 unit) respectivly</summary>
        /// <param><c>instructions</c>This is user input commands for the rover</param>
        public void Drive(string instructions)
        {
            foreach (char instruction in instructions.ToCharArray())
            {
                switch (instruction)
                {
                    case 'R':
                        RotateRight();
                        break;
                    case 'L':
                        RotateLeft();
                        break;
                    case 'M':
                        Move(1); break;
                    default:
                        throw new InvalidOperationException(string.Format("The request to Drive failed to complete because the instructiion '{0}' was invalid.", instruction));
                }
            }
        }

        /// <summary>Rotate the rover 90 Deg to the Left</summary>
        public void RotateRight()
        {
                switch (this.Current_Heading)
                {
                    case 'N':
                        this.Current_Heading = 'E';
                        break;
                    case 'E':
                        this.Current_Heading = 'S';
                        break;
                    case 'S':
                        this.Current_Heading = 'W';
                        break;
                    case 'W':
                        this.Current_Heading = 'N';
                        break;
                }
        }

        /// <summary>Rotate the rover 90 Deg to the right</summary>
        public void RotateLeft()
        {
            switch (this.Current_Heading)
            {
                case 'N':
                    this.Current_Heading = 'W';
                    break;
                case 'E':
                    this.Current_Heading = 'N';
                    break;
                case 'S':
                    this.Current_Heading = 'E';
                    break;
                case 'W':
                    this.Current_Heading = 'S';
                    break;
            }
        }

        /// <summary>Move the rover x units forward based on the current heading
        /// the units should alwasy be 1, but for future version this .
        /// will allow the rover to accept more</summary>
        /// <param><c>units</c>In general this will always = 1 but it is not a rule</param>
        public void Move(int units)
        {
            switch (this.Current_Heading)
            {

                //this is all basic math to move around a grid while checking on its bounds;
                case 'N':
                    if (this.Current_Y + units <= this.Max_North)
                    {
                        this.Current_Y += units;
                    }
                    else
                    {
                        throw new InvalidOperationException("The request to MOVE 'North' failed because the coordinates are outside the max bounds.");
                    }
                    break;

                case'E':
                    if (this.Current_X + units <= this.Max_East)
                    {
                        this.Current_X += units;
                    }
                    else
                    {
                        throw new InvalidOperationException("The request to MOVE 'East' failed because the coordinates are outside the max bounds.");
                    }
                    break;

                case 'S':
                    if (this.Current_Y > 0)
                    {
                        this.Current_Y -= units;
                    }
                    else
                    {
                        throw new InvalidOperationException("The request to MOVE 'South' failed because the coordinates are outside the max bounds.");
                    }
                    break;

                case'W':
                    if (this.Current_X > 0)
                    {
                        this.Current_X -= units;
                    }
                    else
                    {
                        throw new InvalidOperationException("The request to MOVE 'West' failed because the coordinates are outside the max bounds.");
                    }
                    break;
            }
            
        }

        /// <summary>The method is used to retreive the rovers current position</summary>
        /// <returns>string</returns>
        public string GetPosition()
        {
            return string.Format("{0} {1} {2}", this.Current_X, this.Current_Y, this.Current_Heading);
        }

    }

}