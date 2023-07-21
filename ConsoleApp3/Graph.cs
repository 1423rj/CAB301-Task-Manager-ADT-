using System;
using System.Collections.Generic;
using System.Text;
public class Edge //Basically a class that handles the connected between Nodes/Verticies
{
    public string? Destination { get; set; } //Destination node
    public int Weight { get; set; } //Weight will be the "Time" in this case.
    public Edge(string? destination, int weight)
    {
        Destination = destination;
        Weight = weight;
    }
}

public class Graph //Handles the graphing or 'creates a graph'
{
    private int vertices;
    private Dictionary<string, List<Edge>> connectedList; //List of all connections, otherwise known as an adjacencyList
    public Graph()
    {
        
        connectedList = new Dictionary<string, List<Edge>>();
    }
    public void AddNode(string node,  int weight, params string[] destination)
    {
        if (connectedList.ContainsKey(node) == false)
        {
            connectedList[node] = new List<Edge>();
            vertices++;
            
            
        }
        if (destination.Length == 0)
        {
            connectedList[node].Add(new Edge(null, weight));
        }


        for (int i = 0; i < destination.Length; i++)
        {

            if (connectedList.ContainsKey(destination[i]) == false)
            {
                connectedList[destination[i]] = new List<Edge>();
                vertices++;
                
            }
     
            connectedList[node].Add(new Edge(destination[i], weight));
        }
        
        
        

    }
   
    public Dictionary<string, List<Edge>> ToList()
    {
        return connectedList;
    }



    public void RemoveNode(string node)
    {
        if (!connectedList.ContainsKey(node))
        {
            Console.WriteLine("Node does not exist in the graph.");
            return;
        }

        // Remove dependencies on the specified node
        foreach (var edges in connectedList.Values)
        {
            foreach (var edge in edges)
            {
                if (edge.Destination == node)
                {
                    if (edges.Count == 1)
                    {
                        edge.Destination = null;
                    }
                    else
                    {
                        edges.Remove(edge);
                    }
                    break;
                }
            }
        }

        connectedList.Remove(node);
        vertices--;

    }

    public void ChangeTime(string node, int newTime)
    {
        if (!connectedList.ContainsKey(node))
        {
            Console.WriteLine("Node does not exist in the graph.");
            return;
        }
        foreach (Edge edge in connectedList[node])
        {
            edge.Weight = newTime;
            
        }


    }
    
    /// <summary>
    /// Topological Sorting (Depth-First search) 
    /// </summary>
    /// <remarks>https://en.wikipedia.org/wiki/Topological_sorting#Depth-first_search</remarks>
    /// <remarks>https://www.geeksforgeeks.org/topological-sorting/</remarks>
    /// <typeparam name="List"></typeparam>
    /// <returns>Sorted nodes in topological order in a list.</returns>
    public List<string> TopologicalSort()
    {
        List<string> list = new List<string>();
        Dictionary<string, bool> visited = new Dictionary<string, bool>(vertices);

        //Mark each vertex / node as unvisited
        foreach (string node in connectedList.Keys)
        {
            visited[node] = false;
        }

        // If node is unchecked send to helper
        foreach(string node in connectedList.Keys)
        {
            if (visited[node] == false)
            {
                TopologicalSortHelper(node, visited, list);
            }
        }

        
        //  Generate and Write to Sequence.txt  //
        //  In correct topological Order        //
        
        string file = Directory.GetParent(Directory.GetCurrentDirectory()) + "\\" + "Sequence.txt";

        if (File.Exists(file))
        {
            File.Delete(file);
            
        }

        using (StreamWriter sw = File.CreateText(file)) //Creates Sequence.txt and writes output
        {
            foreach(string text in list)
            {
                sw.WriteLine(text);
            }
        }
        return list;       //Return topologically sorted list
    }

    /// <summary>
    /// Topological Sorting Helper 
    /// </summary>
    /// <param name="node">Node in a DAC</param>
    /// <param name="visited">Boolean of each nodes status as marked</param>
    /// <param name="list">List to be sorted / sorted List</param>
    /// <returns>Sorted node in topological order.</returns>
    private void TopologicalSortHelper(string node, Dictionary<string, bool> visited, List<string> list)
    {
        //Mark the node as visited
        visited[node] = true;

        if (connectedList.ContainsKey(node))
        {
            foreach (Edge edge in connectedList[node])
            {
                if (edge.Destination != null)
                {
                    if (!visited[edge.Destination])
                    {
                        TopologicalSortHelper(edge.Destination, visited, list);
                    }
                }

            }
        }

        list.Add(node);
    }
    public Dictionary<string, int> CommencementTime()
    {
        //Essentially a list of nodes that have their Key (name) followed by their
        //earliest (max) commecement time

        Dictionary<string, int> commencementTimes = new Dictionary<string, int>(); 

        List<string> topologicalOrder = TopologicalSort(); //Topologically sorted list of Nodes

        foreach (string node in topologicalOrder)
        {
            int maxCommencementTime = 0; //Initialise Max CommencementTime
            
            if (connectedList.ContainsKey(node))
            {
                
                foreach (Edge edge in connectedList[node])
                {
                    if (edge.Destination != null) //Ensure the node has directed connections
                    {
                        //For each node connected to the base node, find the maxCommencementTime
                        foreach (Edge edgeSpecific in connectedList[edge.Destination]) 
                        {
                            maxCommencementTime = Math.Max(maxCommencementTime, commencementTimes[edge.Destination] + edgeSpecific.Weight);
                        }
                    }
                }
            }
                commencementTimes[node] = maxCommencementTime; //Finalise that nodes commencementTime
        }

        Console.WriteLine();
        //Display KeyValuePairs
        foreach (var key in commencementTimes)
        {
            Console.WriteLine(key.Key + ", " + key.Value);
        }

        //  Generate and Write to EarliestTimes.txt  //
        //  With correctly handled commcement times  //

        string file = Directory.GetParent(Directory.GetCurrentDirectory()) + "\\" + "EarliestTimes.txt";
        if (File.Exists(file))
        {
            File.Delete(file);

        }
        using (StreamWriter sw = File.CreateText(file))
        {
            try
            {
                var sortByTasks = commencementTimes.OrderBy(key => int.Parse(key.Key.Substring(1))); //This will attempt to order the Tasks by T1, T2 etc

                foreach (var key in sortByTasks)
                {
                    sw.WriteLine(key.Key + ", " + key.Value);
                }
            }

            catch
            {
                foreach (var key in commencementTimes)
                {
                    sw.WriteLine(key.Key + ", " + key.Value); //If there is a task that is not T{int} itll just print based on the topological order
                }
            }

        }

        return commencementTimes; //return Dictionary
    }

    /// <summary>
    /// Graph Collection Sorter
    /// </summary>
    /// <param name="lines">Each line from a file stored as an array</param>
    /// <param name="graph">Graph to be stored too</param>
    /// <returns>Graph after sorting a textFile</returns>
    public Graph CollectionSorter(string[] lines)
    {
        //Rebuild Graph
        Graph graph = new Graph();

        for (int i = 0; i < lines.Length; i++)
        {
            if (!string.IsNullOrEmpty(lines[i]))
            {

                string[] array = lines[i].Split(','); //Seperate commas into lines
                int z = 0;

                foreach (string line in array)
                {
                    array[z] = array[z].Replace(" ", ""); //Ensure no spacing
                    z++;
                }
                if (array.Length == 2)
                {
                    graph.AddNode(array[0], int.Parse(array[1])); //If Node has no dependencies   
                }
                else //If node has dependencies                                  
                {
                    for (int j = 2; j < array.Length; j++)
                    {
                        graph.AddNode(array[0], int.Parse(array[1]), array[j]);
                    }
                }
            }
            
            
            
        }
        return graph;
    }
    
    public string[] PrintConnectedList() //Turns the connectList into an array, Good for printing to console or to textFiles.
    {
        string[] Array = new string[connectedList.Count];
        int z = 0;

        foreach (var node in connectedList)
        {
            
            string key = node.Key;
            if (node.Value.Count > 0)
            {
                key = key + ", " + (node.Value[0].Weight);
            
                if(node.Value[0].Destination != null)
                {
                    key = key + ", " + node.Value[0].Destination;
                    for (int i = 1; i < node.Value.Count; i++)
                    {
                        if (node.Value[i].Destination != null)
                        {
                            key = key + ", " + node.Value[i].Destination;
                        }
                    }
                }
                
            }
            Array[z] = key;
            z++;
        }
        return Array;
    }

}

    

