using Microsoft.VisualStudio.TestTools.UnitTesting;
using MarsRover;
using MarsRover.App.Interfaces;
using MarsRover.App;
using System;

namespace RoverTests
{
    [TestClass]
    public class RoverTests
    {
        [TestMethod]
        public void GetPosition_WithValidData()
        {
            //arrange the test data
            int grid_x = 5;
            int grid_y = 5;
            int landing_x = 2;
            int landing_y = 2;
            char landing_z = 'S';

            string expected = "2 2 S";
            IRover rover = new Rover(landing_x, landing_y, landing_z, grid_x, grid_y);
            
            //act
            string actual = rover.GetPosition();

            //assert
            Assert.AreEqual(expected, actual, "Rover didn't land at the expected location");

        }

        [TestMethod]
        public void Drive_WithValidData_InsideGrid()
        {
            //arrange the test data
            int grid_x = 5;
            int grid_y = 5;
            int landing_x = 1;
            int landing_y = 2;
            char landing_z = 'N';
            string instructions = "LMLMLMLMM";

            string expected = "1 3 N";
            IRover rover = new Rover(landing_x, landing_y, landing_z, grid_x, grid_y);


            //act on the test data
            rover.Drive(instructions);

            string actual = rover.GetPosition();
            Assert.AreEqual(expected,actual,"Rover didn't move to the expected location");

            
        }

        [TestMethod]
        public void Drive_OutsideGrid_ThrowsException()
        {
            //arrange the test data
            int grid_x = 3;
            int grid_y = 3;
            int landing_x = 0;
            int landing_y = 0;
            char landing_z = 'N';
            string instructions = "MMMM";

            Exception expected = null;
            IRover rover = new Rover(landing_x, landing_y, landing_z, grid_x, grid_y);

            try
            {
                //act on the test data
                rover.Drive(instructions);
            }
            catch (Exception ex)
            {
                expected = ex;
            }
            
            //assert
            Assert.IsNotNull(expected);
            
        }


    }
}
