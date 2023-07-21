# CAB301-Task-Manager(ADT)
 This C# program implements a task sorting algorithm using a graph data structure. The tasks are represented as nodes in the graph, and dependencies between tasks are represented as edges. The program provides functionality to read tasks from a text file, add new tasks, remove tasks, change task times, perform topological sorting, and find the earliest possible commencement times for each task.

## Table of Contents
- [Graph.cs](#graphcs)
- [Program.cs](#programcs)

## Graph.cs

This file contains the implementation of the graph data structure and the algorithms for managing tasks and their dependencies.

### Class: `Edge`

- `string? Destination`: The destination node of the edge (the task that depends on another task).
- `int Weight`: The weight of the edge (the time required to complete the task).

### Class: `Graph`

- `private int vertices`: The number of vertices (tasks) in the graph.
- `private Dictionary<string, List<Edge>> connectedList`: A dictionary that represents the adjacency list for the graph. It stores the connections between tasks as edges.

#### Constructor: `public Graph()`

- Initializes a new instance of the `Graph` class.

#### Method: `public void AddNode(string node, int weight, params string[] destination)`

- Adds a new node (task) to the graph along with its connections (dependencies) to other tasks.
- If the node doesn't exist in the graph, it is added to the graph.
- If the node has no dependencies (destination.Length == 0), a self-loop edge is added to represent the task.
- If the node has dependencies, edges from the node to each destination are added to represent the dependencies.

#### Method: `public void RemoveNode(string node)`

- Removes a node (task) from the graph along with its dependencies.
- If the node doesn't exist in the graph, a message is displayed, and no action is taken.
- Removes all edges connected to the specified node (both incoming and outgoing).

#### Method: `public void ChangeTime(string node, int newTime)`

- Changes the time (weight) of a specific node (task) in the graph.
- If the node doesn't exist in the graph, a message is displayed, and no action is taken.
- Updates the weight property of all outgoing edges from the specified node to the newTime value.

#### Method: `public List<string> TopologicalSort()`

- Performs a topological sort on the graph using depth-first search (DFS) algorithm.
- Returns a list of nodes (tasks) in topological order.
- Writes the sorted nodes to a file named "Sequence.txt" in the bin/debug directory.

#### Method: `private void TopologicalSortHelper(string node, Dictionary<string, bool> visited, List<string> list)`

- Helper function for the topological sort.
- Recursively explores the graph using DFS and adds nodes to the list in the correct order.

#### Method: `public Dictionary<string, int> CommencementTime()`

- Calculates the earliest possible commencement time for each task in the graph.
- Uses the topological order to calculate the earliest commencement time for each task.
- Writes the earliest commencement times to a file named "EarliestTimes.txt" in the bin/debug directory.
- Returns a dictionary with each task name as the key and its earliest commencement time as the value.

#### Method: `public Graph CollectionSorter(string[] lines)`

- Rebuilds the graph using the provided array of lines (tasks) from a text file.
- Parses each line to extract the task name, time, and dependencies (if any) and adds them to the graph.
- Returns the updated graph.

#### Method: `public string[] PrintConnectedList()`

- Converts the graph's adjacency list (connectedList) into an array of strings.
- Good for printing the graph's data to the console or to text files.

## Program.cs

This file contains the main program that interacts with the user and utilizes the `Graph` class to manage tasks and perform sorting.

### Class: `Program`

#### Method: `public static void Main(string[] args)`

- The entry point of the program.
- Implements a command-line interface to interact with the task sorting functionalities.
- Reads tasks from a text file, adds new tasks, removes tasks, changes task times, performs topological sorting, saves the graph to a text file, and finds the earliest possible commencement times for each task.
- All text files (Sequence.txt, EarliestTimes.txt, and Tasks.txt) are stored in the bin/debug directory.

