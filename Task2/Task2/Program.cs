using System;
using System.Collections.Generic;
using System.Linq;

namespace Task2
{
    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();
            Console.WriteLine("Hello World!");
            int numberOfDestinations = p.InsertPositiveNumber("numberOfDestinations");
            int selectedStrategy = p.SelectStrategy();
            int numberOfConsecutiveLoads = p.InsertPositiveNumber("numberOfConsecutiveLoads");
            int probabilityOfFailure = p.InsertPercentageOfFailure();
            int numberOfLoads = p.InsertPositiveNumber("numberOfLoads");
            Dictionary<int, int> destinations;
            destinations = p.startConveyer(numberOfDestinations, numberOfConsecutiveLoads, probabilityOfFailure, numberOfLoads, selectedStrategy);

            Console.WriteLine("Results: ");
            Console.WriteLine("Destination - How many packages reached the destination");

            foreach (var item in destinations.OrderBy(item=>item.Key))
            {
                int percentagePart = (int)Math.Round((double)(100 * item.Value) / numberOfLoads);
                Console.WriteLine(item.Key.ToString() + " - " + percentagePart.ToString() + "% " + item.Value);
            }
            Console.ReadKey();
        }


        #region |Input|

        public int InsertPositiveNumber(string inputName)
        {
            bool valid = false;
            int input = 0;
            string messageBeforeInput="Enter the number";
            string messageAfterInput="Entered number - ";
            switch (inputName) 
            {
                case "numberOfDestinations":
                    messageBeforeInput = "Enter the number of available destinations:";
                    messageAfterInput = "Number of destinations - ";
                    break;
                case "numberOfConsecutiveLoads":
                    messageBeforeInput = "Enter the number of consecutive loads:";
                    messageAfterInput = "Number of consecutive loads - ";
                    break;
                case "numberOfLoads":
                    messageBeforeInput = "Enter the number of loads:";
                    messageAfterInput = "Number of loads - ";
                    break;
            }

            while (!valid)
            {
                Console.WriteLine(messageBeforeInput);
                valid = Int32.TryParse(Console.ReadLine(), out input) && input > 0;
                if (!valid)
                {
                    Console.WriteLine("The input must be a positive number");
                }
                Console.WriteLine(messageAfterInput + input);
                Console.WriteLine();
            }

            return input;
        }


        public int SelectStrategy()
        {
            bool valid = false;
            int selectedStrategy = 0;

            while (!valid)
            {
                Console.WriteLine("Enter the number of destination selection strategy:");
                Console.WriteLine("1 - Round robin (1,2,3,..n, 1,2,3,…,n , … )");
                Console.WriteLine("2 - Random (select a destination randomly with the same probability weight for destination)");

                valid = Int32.TryParse(Console.ReadLine(), out selectedStrategy) && selectedStrategy > 0 && selectedStrategy <= 2;
                if (!valid)
                {
                    Console.WriteLine("The input must be 1 or 2");
                }
            }

            Console.WriteLine("Selected strategy - " + selectedStrategy);
            Console.WriteLine();

            return selectedStrategy;
        }


        public int InsertPercentageOfFailure()
        {
            bool valid = false;
            int percentageOfFailure = 0;

            while (!valid)
            {
                Console.WriteLine("Enter a percentage of failure for load to be diverted into its destination:");
                valid = Int32.TryParse(Console.ReadLine(), out percentageOfFailure)
                    && percentageOfFailure >= 0 && percentageOfFailure <= 100;
                if (!valid)
                {
                    Console.WriteLine("The input must be a number between 0 and 100");
                }
                Console.WriteLine("Percentage of a failure - " + percentageOfFailure);
                Console.WriteLine();
            }

            return percentageOfFailure;
        }


        #endregion |Input|

        public Dictionary<int, int> startConveyer(int numberOfDestinations, int numberOfConsecutiveLoads, int percentageOfFailure, int numberOfLoads,int selectedStrategy)
        {
            Dictionary<int, int> destinations = new Dictionary<int, int>();
            int currentDestination = 1;
            int currentConsecutiveDelivers = 0;
            for (int i = 0; i < numberOfLoads; i++)
            {
                if (selectedStrategy == 1)
                {
                    assignDestinationUsingStrategy1(ref currentDestination, numberOfDestinations, ref currentConsecutiveDelivers, numberOfConsecutiveLoads);
                }
                else {
                    assignDestinationUsingStrategy2(ref currentDestination, numberOfDestinations, ref currentConsecutiveDelivers, numberOfConsecutiveLoads);
                }

                bool reachedDestination = isDerived(percentageOfFailure);
                if (reachedDestination == false)
                {
                    addPackageToDestination(destinations, 0);
                }
                else 
                {
                    addPackageToDestination(destinations,currentDestination);
                }
            }
            return destinations;
        }


        public void addPackageToDestination(Dictionary<int,int> destinations, int destination) 
        {
            int reachedPackages;
            if (destinations.TryGetValue(destination, out reachedPackages))    //Checks if any packages already reached that destination
            {
                destinations[destination] = reachedPackages + 1;
            }
            else   
            {
                destinations.Add(destination, 1);
            }
        }


        public void assignDestinationUsingStrategy1(ref int currentDestination, int numberOfDestinations, ref int currentConsecutiveDelivers, int numberOfConsecutiveLoads)
        {
            if (currentConsecutiveDelivers == numberOfConsecutiveLoads)  //Checks if needs to change destination
            {
                if (currentDestination == numberOfDestinations)
                {
                    currentDestination = 1;
                }
                else
                {
                    currentDestination++;
                }
                currentConsecutiveDelivers = 1;
            }
            else   //Didn't need to change destination, load sent to the same destination as previous one.
            {
                currentConsecutiveDelivers++;
            }
        }


        public void assignDestinationUsingStrategy2(ref int currentDestination, int numberOfDestinations, ref int currentConsecutiveDelivers, int numberOfConsecutiveLoads) 
        {
            if (currentConsecutiveDelivers == numberOfConsecutiveLoads)  //Checks if needs to change destination
            {
                Random rand = new Random();
                currentDestination = rand.Next(1,numberOfDestinations);
                currentConsecutiveDelivers = 1;
            }
            else  //Didn't need to change destination, load sent to the same destination as previous one.
            {
                currentConsecutiveDelivers++;
            }
        }


        public bool isDerived(int probabilityOfFailure)
        {
            Random rand = new Random();
            if (rand.Next(1, 100) <= probabilityOfFailure)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

    }
}
