
//Please note files are expected to be in bin/debug
public class Program
{
    public static void Main(string[] args)
    {
        Graph graph = new Graph(); //Create a graph

        //Initialise Variables
        string[] lines = { "" };
        string? path = "";
        string? FileName = "Tasks.txt";
        int End = 0;

        //Interface
        while (End == 0)
        {

            //List of options for the user to pick.
            Console.WriteLine("");
            Console.WriteLine("1) Read a Text File");   // Implemented
            Console.WriteLine("2) Add a new task");     // Implemented
            Console.WriteLine("3) Remove a Task");      // Implemented
            Console.WriteLine("4) Change the Time");    // Implemented
            Console.WriteLine("5) Sort Tasks");         // Implemented
            Console.WriteLine("6) Save to TextFile");   // Implemented 
            Console.WriteLine("7) Earliest Possible Commencement Time"); // Implemented
            Console.WriteLine("8) Exit");               // Implemented
            Console.WriteLine("");
            string? option = Console.ReadLine();
            Console.WriteLine("");


            switch (option)
            {
                //CASE 1 - READ A TEXT FILE
                case "1":

                    Console.WriteLine("Please input a Text File Name with \nextension example: Tasks.txt" + "\n");

                    try
                    {
                        path = Console.ReadLine();
                        FileName = path;
                        path = (Directory.GetParent(Directory.GetCurrentDirectory()) + "\\" + path);
                        lines = File.ReadAllLines(path);
                    }
                    catch (System.IO.FileNotFoundException)
                    {
                        Console.WriteLine("Path not found" + "\n");
                        path = "";
                    }
                    catch (System.IO.DirectoryNotFoundException)
                    {
                        Console.WriteLine("Directory cannot be null" + "\n");
                        path = "";
                    }
                    graph = graph.CollectionSorter(lines);
                    break;

                //CASE 2 - ADD A TASK
                case "2":

                    //Ensure the input exists and that the input is not empty
                    if (path == "")
                    {
                        Console.WriteLine("Please Input a Text File");
                        break;
                    }
                    Console.WriteLine("Please Input a Task of type Tn \nFor versatility you may use other task names also\n");
                    string? newTask = Console.ReadLine();
                    if (string.IsNullOrEmpty(newTask))
                    {
                        Console.WriteLine("Task Cannot be empty");
                        break;
                    }

                    // Ensure the Task isnt in use
                    StringComparison comp = StringComparison.OrdinalIgnoreCase;
                    bool ContainsTask = false;
                    foreach (string line in lines)
                    {
                        if (line.Contains(newTask, comp))
                        {
                            Console.WriteLine("\nCannot have a repeat Task or Task name too similar to alternate tasks \nPlease make sure to type T{integer}");
                            ContainsTask = true;
                            break;
                        }
                    }
                    if (ContainsTask == true)
                    {
                        break;
                    }

                    //Ensures a Valid Time
                    Console.WriteLine("Please Input Time");
                    int number;
                    string? newTaskTime = Console.ReadLine();
                    if (!Int32.TryParse(newTaskTime, out number))
                    {
                        Console.WriteLine("Time must be an integer");
                        break;
                    }
                    else
                    {
                        number = int.Parse(newTaskTime);
                    }

                    //Ask for Dependencies, Will be created if they do not exist
                    Console.WriteLine("Please Input Dependencies as follows: dependency 1, dependency 2, ... dependency \nOr enter None to enter no dependencies");
                    string? newDependency = Console.ReadLine();
                    string[]? array;
                    if (!string.IsNullOrEmpty(newDependency))
                    {
                       array = newDependency.Split(',');
                       int z = 0;
                       foreach (string line in array)
                       {
                           array[z] = array[z].Replace(" ", ""); //Ensure no spacing
                           z++;
                       }
                       for (int i = 0; i<array.Length; i++)
                       {
                           graph.AddNode(newTask, number, array[i]); 
                       }
                    }
                    else
                    {
                        graph.AddNode(newTask, number);
                    }
                    break;

                //CASE 3 - REMOVE A TASK
                case "3":
                    if (path == "")
                    {
                        Console.WriteLine("Please Input a Text File\n");
                        break;
                    }
                    Console.WriteLine("Please Input a Task of type Tn\n");
                    string? removeTask = Console.ReadLine();
                    if (string.IsNullOrEmpty(removeTask))
                    {
                        Console.WriteLine("Task Cannot be empty\n");
                        break;
                    }
                    graph.RemoveNode(removeTask);
                    Console.WriteLine("Revised List:\n");
                    for (int i = 0; i < graph.PrintConnectedList().Length; i++)
                    {
                        Console.WriteLine(graph.PrintConnectedList()[i]);
                    }

                    break;

                //CASE 4 - CHANGE A TIME
                case "4":
                    if (path == "")
                    {
                        Console.WriteLine("Please Input a Text File");
                        break;
                    }
                    Console.WriteLine("Please Input Node\n");
                    string? node = Console.ReadLine();
                    Console.WriteLine("Please Input New Time\n");
                    string? newTime = Console.ReadLine();
                    int NewTimeNumber = 0;
                    if (string.IsNullOrEmpty(node) || string.IsNullOrEmpty(newTime))
                    {
                        Console.WriteLine("Task or Time Cannot be empty");
                        break;
                    }
                    if (!Int32.TryParse(newTime, out NewTimeNumber))
                    {
                        Console.WriteLine("Time must be an integer");
                        break;
                    }
                    else
                    {
                        NewTimeNumber = int.Parse(newTime);
                    }
                    graph.ChangeTime(node, NewTimeNumber);
                    Console.WriteLine("Revised List:\n");
                    for (int i = 0; i < graph.PrintConnectedList().Length; i++)
                    {
                        Console.WriteLine(graph.PrintConnectedList()[i]);
                    }
                    break;

                //CASE 5 - TOPOLOGICALLY SORT SEQUENCE
                case "5":
                    if (path == "")
                    {
                        Console.WriteLine("Please Input a Text File");
                        break;
                    }
                    Console.WriteLine("Printing to Sequence.txt");
                    List<string> list = graph.TopologicalSort();
                    string[] a = list.ToArray();
                    for (int i = 0; i < a.Length; i++)
                    {
                        Console.WriteLine(a[i]);
                    }

                    break;

                //CASE 6 - SAVE TO ORIGINAL FILE
                case "6":
                    if (path == "")
                    {
                        Console.WriteLine("Please Input a Text File");
                        break;
                    }
                    Console.WriteLine("Printing to " + FileName + "\n");
                    for (int i = 0; i < graph.PrintConnectedList().Length; i++)
                    {
                        Console.WriteLine(graph.PrintConnectedList()[i]);
                    }
                    string file = Directory.GetParent(Directory.GetCurrentDirectory()) + "\\" + FileName;
                    if (File.Exists(file))
                    {
                        File.Delete(file);

                    }
                    using (StreamWriter sw = File.CreateText(file))
                    {
                        foreach (string line in graph.PrintConnectedList())
                        {
                            sw.WriteLine(line);
                        }
                    }
                    break;

                //CASE 7 - FIND EARLIEST COMMENCEMENT TIMES
                case "7":
                    if (path == "")
                    {
                        Console.WriteLine("Please Input a Text File");
                        break;
                    }
                    Console.WriteLine("Printing to EarliestTimes.txt");

                    graph.CommencementTime();
                    break;


                //CASE 8 - END CONSOLE
                case "8":
                    Console.WriteLine("Exiting. All txt files can be found in bin/debug\n");
                    End = 1;
                    break;

                //ELSE
                default:
                    Console.WriteLine("Option Not Available ");
                    break;
            }
        }
    }
}