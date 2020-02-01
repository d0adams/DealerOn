using System;
using System.Linq;
using MarsRover.App;
using MarsRover.App.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MarsRover
{

    /// <summary>
    /// 
    /// Author: David Adams
    /// Date: 1/31/2020
    /// 
    /// The goal of the MarsRover app is to demonstrate object-oriented programming skills
    /// </summary>
    /// <remarks>
    ///  The actual logic of this code test is elementary which actually makes it 
    ///  harder to showcase advanced programming skills.  There is an art to taking a 
    ///  complex program and simplifying it...this is the opposite.
    ///  
    /// To begin with I decided to use a simple console app with data read in from
    /// the console. In the real world I hope nasa is using something we a better GUI
    /// than my ASCII Intro banner.
    /// 
    /// The test specs don't spend much time talking about how to implement the input and output
    /// just that it matches.  I assume here that I can prompt the user and give feedback to them
    /// without that being considered "Output".  A tester can paste in the full test and it will 
    /// give correct output but they will just need to read through whats happening to see the results.
    /// I didn't spend much time on the actual user input (Console.Read() isn't very sophisticated)
    ///  but I did spend the bulk of my time writing robust input validation and error handling for that input.
    /// 
    /// Im using an custom Interface IInputValidator to separate the validation concerns from the main driver.
    /// doing this also allowed me to wire up the dotnet dependency injector to use in the application 
    /// Main.  
    /// 
    /// I also wired up some Unit Testing via a separate MSTest based project to test both the validation
    /// tool results, but also the actual Rover.  I could have documented the test a little better, and
    /// tested more cases, but I hit a least one of all the major methods as a demonstration.
    /// 
    /// Further assumptions:
    /// 
    /// 1) The rover must land in the grid plane.  The user will be prompted to re-enter if not.
    /// 2) The rover will move as far as it can until it reaches the borders.  Then the rover will
    /// throw an InvalidOperationException with a little feedback.  Then the rover displays its last postion
    /// and essentially dies.
    /// 3) The specs talk alot about multiple rovers, but the only clue it gives about how to implement them is that 
    /// they each get 2 strings to control them.  I assume this to mean that after one set of commands, the rover
    /// is done and we "land" a new one to use for the next input
    /// 4) I didn't over complicate the business logic of moving and rotating the rovers.  I ASSUME the test
    /// isn't about being able to check bounds and turn...but with that said some further improvements I considered
    /// were adding enums for the cardinal headings, or simply using an degree system to allow for turning any
    /// direction.  Also the grid could have (maybe should have) been made up of a 2D array so I could later store
    /// info about the points on the map...maybe pictures taken at the spot...again...I think the logic wasn't to point
    /// here.  Hope Im not under thinking it.
    /// 
    /// Overall I am pleased with how complicated I could make this little app.  It was fun!
    /// 
    /// Thanks for taking a look!
    /// 
    /// David
    /// 
    /// </remarks>
    class Program
    {
        private static IServiceProvider _serviceProvider;
        private static IInputValidator _inputValidator;

        static void Main(string[] args)
        {
            //register our service provider for DI
            RegisterServices();
            _inputValidator = _serviceProvider.GetService<IInputValidator>();
            _inputValidator.ValidateBoundaries("5 5");

            //Show instruction
            PrintInstructions();

            //get validated grid bounds
            var grid = GetGridBoundaries();
            
            //we know the data is valid so set the grid data
            var gridAry = grid.Trim().ToUpper().Split(' ');
            int max_east = int.Parse(gridAry[0]) ;
            int max_north = int.Parse(gridAry[1]) ;
            
            //while (exitLoop.ToUpper() != "EXIT")
            while (true)
            {
                //get validated Landing position and heading
                string landingPosition = GetLandingPosition(max_east,max_north);
                
                //split the input into by space and assign vars
                var landingAry = landingPosition.Trim().ToUpper().Split(' ');

                int landing_X = int.Parse(landingAry[0]);
                int landing_y = int.Parse(landingAry[1]);
                char landing_z = landingAry[2][0]; //converting from string to char here

                //get the Drive Instruction
                string instructions = GetDriveInstructions();
                
                //init the first rover
                IRover rover = new Rover(landing_X, landing_y, landing_z, max_east, max_north);

               
                try
                { 
                    //try to drive it
                    rover.Drive(instructions.ToUpper());
                }
                catch (InvalidOperationException e)
                {
                    Console.WriteLine(e.Message);
                }

                Console.WriteLine();
                Console.WriteLine("*** ROVER'S FINAL POSITION ***");
                Console.WriteLine(rover.GetPosition());

                Console.WriteLine();
                Console.WriteLine("INITIALIZING A NEW ROVER");
            }
        }

        /// <summary>This is used to setup our initial dependency injection
        /// this is a striped down version of the new DI built-in
        /// to any dotnet core MVC apps</summary>
        private static void RegisterServices()
        {
            var collection = new ServiceCollection();
            collection.AddSingleton<IInputValidator, InputValidator>();
            _serviceProvider = collection.BuildServiceProvider();
        }

        /// <summary>
        /// Used to get grid boundaries from user
        /// </summary>
        /// <returns>string</returns>
        static string GetGridBoundaries()
        {
            Console.Write(Environment.NewLine + "ENTER THE EAST(X) AND NORTH(Y) GRID EXPLORATION BOUNDARIES: ");
            var grid = Console.ReadLine();

            while (!_inputValidator.ValidateBoundaries(grid))
            {
                Console.WriteLine("INVALID. Please Try Again");
                Console.Write(Environment.NewLine + "ENTER THE EAST(X) AND NORTH(Y) GRID EXPLORATION BOUNDARIES: ");
                grid = Console.ReadLine();
            }
            return grid;
        }

        /// <summary>
        /// Used to get grid landing position from user
        /// </summary>
        /// <returns>string</returns>
        static string GetLandingPosition(int grid_x, int grix_y)
        {
            string cmd = "ENTER THE KNOWN LANDING POSITION OF ROVER: ";
            Console.Write(Environment.NewLine + cmd);
            string landingPosition =  Console.ReadLine();

            while (!_inputValidator.ValidateLandingPosition(landingPosition, grid_x, grix_y))
            {
                Console.WriteLine("INVALID Entry. Please Try Again");
                Console.Write(Environment.NewLine + cmd);
                landingPosition = Console.ReadLine();
            }
            return landingPosition;
        }
        
        /// <summary>
        /// Used to get drive commands from user
        /// </summary>
        /// <returns>string</returns>
        static string GetDriveInstructions()
        {
            string cmd = "ENTER DRIVE INSTRUCTIONS: ";
            Console.Write(Environment.NewLine + cmd );
            var instructions =  Console.ReadLine();

            while (!_inputValidator.ValidateInstructions(instructions))
            {
                Console.WriteLine("INVALID Entry. Please Try Again");
                Console.Write(Environment.NewLine + cmd);
                instructions = Console.ReadLine();
            }
            return instructions;
        }

        /// <summary>
        /// Displays the instruction
        /// </summary>
        /// <returns>string</returns>
        static void PrintInstructions()
        {
            Console.WriteLine("######################################################################");
            Console.WriteLine("#                    MARS ROVER CONTROL PROGRAM                      #");
            Console.WriteLine("######################################################################");
            Console.WriteLine("#                                                                    #");
            Console.WriteLine("#                            INSTRUCTIONS                            #");
            Console.WriteLine("#                                                                    #");
            Console.WriteLine("# 1. ENTER THE EAST(X) AND NORTH(Y) GRID EXPLORATION BOUNDARIES      #");
            Console.WriteLine("#    FORMAT: Coordinates must be Ints and be separtated by spaces.   #");
            Console.WriteLine("#    EXAMPLE INPUT: 5 5                                              #");
            Console.WriteLine("#                                                                    #");
            Console.WriteLine("# 2. COMMAND THE ROVER (In loop for each rover)                      #");
            Console.WriteLine("#  a. ENTER THE KNOWN LANDING POSITION OF ROVER: (X Y Z)             #");
            Console.WriteLine("#     FORMAT: X AND Y Coordinates must be Integers. Z Coordinate     #");
            Console.WriteLine("#             must be 'N', 'E', 'S', or 'W'. Separtated by spaces.   #");
            Console.WriteLine("#     EXAMPLE INPUT: 1 2 N                                           #");
            Console.WriteLine("#                                                                    #");
            Console.WriteLine("#  b. ENTER DRIVE INSTRUCTIONS                                       #");
            Console.WriteLine("#     FORMAT: Any combination of 'L', 'R', or 'M'. No spaces.        #");
            Console.WriteLine("#     EXAMPLE INPUT: LMLMLMLMM                                       #");
            Console.WriteLine("#                                                                    #");
            Console.WriteLine("######################################################################");
            Console.WriteLine();
        }
    }
}
