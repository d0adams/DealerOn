using Microsoft.VisualStudio.TestTools.UnitTesting;
using MarsRover.App.Interfaces;
using MarsRover.App;
using System;

namespace RoverTests
{
    [TestClass]
    public class InputValidatorTests
    {
        
        [TestMethod]
        public void Validate_Grid_ValidData_1()
        {
            //arrange the test data
            string input = "5 5";
            var expected = true;

            //try
            IInputValidator validator = new InputValidator();
                
            //assert
            Assert.AreEqual(expected, validator.ValidateBoundaries(input), "The Validator failed  on VALID data ");
        }

        [TestMethod]
        public void Validate_Grid_ValidData_2()
        {
            //arrange the test data
            string input = " 20 1 ";
            var expected = true;

            //try
            IInputValidator validator = new InputValidator();

            //assert
            Assert.AreEqual(expected, validator.ValidateBoundaries(input), "The Validator failed on VALID data ");
        }

        [TestMethod]
        public void Validate_Grid_BadData_1()
        {
            //arrange the test data
            string input = "20 0 ";
            var expected = false;

            //try
            IInputValidator validator = new InputValidator();

            //assert
            Assert.AreEqual(expected, validator.ValidateBoundaries(input), "The Validator passed on invalid data ");
        }
        
        [TestMethod]
        public void Validate_Landing_GoodData_1()
        {
            //arrange the test data
            string input = "1 2 N";
            int grid_x = 5;
            int grid_y = 5;
            var expected = true;

            //try
            IInputValidator validator = new InputValidator();

            //assert
            Assert.AreEqual(expected, validator.ValidateLandingPosition(input, grid_x, grid_y), "The Validator failed on valid data ");
        }

        [TestMethod]
        public void Validate_Landing_BadData_1()
        {
            //arrange the test data
            int grid_x = 5;
            int grid_y = 5;
            string input = "1 3 z";
            var expected = false;

            //try
            IInputValidator validator = new InputValidator();

            //assert
            Assert.AreEqual(expected, validator.ValidateLandingPosition(input, grid_x, grid_y), "The Validator passed on invalid data ");
        }

        [TestMethod]
        public void Validate_Landing_BadData_2()
        {
            //arrange the test data
            int grid_x = 5;
            int grid_y = 5;
            string input = "1 8 N";
            var expected = false;

            //try
            IInputValidator validator = new InputValidator();

            //assert
            Assert.AreEqual(expected, validator.ValidateLandingPosition(input, grid_x, grid_y), "The Validator passed on invalid data ");
        }


        [TestMethod]
        public void Validate_Instructions_GoodData_1()
        {
            //arrange the test data
            string input = "LMRLMRLR";
            var expected = true;

            //try
            IInputValidator validator = new InputValidator();

            //assert
            Assert.AreEqual(expected, validator.ValidateInstructions(input), "The Validator failed on valid data ");
        }

        [TestMethod]
        public void Validate_Instructions_BadData_1()
        {
            //arrange the test data
            string input = "LM1N1";
            var expected = false;

            //try
            IInputValidator validator = new InputValidator();

            //assert
            Assert.AreEqual(expected, validator.ValidateInstructions(input), "The Validator passed on invalid data ");
        }
    }
}
