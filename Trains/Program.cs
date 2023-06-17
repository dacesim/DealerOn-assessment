using System;
using System.Collections.Generic;
using System.Linq;

namespace Trains;

public class Trains
{
  private Dictionary<char, Dictionary<char, int>> graph;

  public Trains(List<string> routes)
  {
    graph = new Dictionary<char, Dictionary<char, int>>();

    foreach (string route in routes)
    {
      char start = route[0];
      char end = route[1];
      Console.WriteLine(route);
      int distance = int.Parse(route.Substring(2));

      // If the starting town is not already in the graph, add it
      if (!graph.ContainsKey(start))
          graph[start] = new Dictionary<char, int>();

      // Add the route to the graph, with the ending town as the key and the distance as the value
      graph[start][end] = distance;
    }
  }

  public int CalculateDistance(string route)
  {
    int distance = 0;
    for (int i = 0; i < route.Length - 1; i++)
    {
      char start = route[i];
      char end = route[i + 1];

      // Check if there is a direct route between the current town and the next town
      if (graph.ContainsKey(start) && graph[start].ContainsKey(end))
        distance += graph[start][end];
      else
        return -1; // No such route
    }

    return distance;
  }

  //Call the recursive DFS method to count routes with maximum stops
  public int CountRoutes(char start, char end, int limit, string criteria)
  {
    int count = 0;
    CountRoutesDFS(start, end, 0, 0, limit, criteria, ref count);
    return count;
  }

  // calculates the number of routes between the start and end towns within a maximum number of stops
  private void CountRoutesDFS(char current, char end, int stops, int distance, int limit, string criteria, ref int count)
  {
    if (criteria == "Stops" && stops > limit)
      return;

    if (criteria == "Distance" && distance > limit)
      return;

    if (current == end && stops > 0 && ((criteria == "Stops" && stops <= limit) || (criteria == "Distance" &&  distance < limit)))
      count++;

    if (!graph.ContainsKey(current))
      return;

    foreach (var neighbor in graph[current])
      CountRoutesDFS(neighbor.Key, end, stops + 1, distance + neighbor.Value, limit, criteria, ref count);
  }

  // Calculates the number of routes bewteen the start and end towns that have an exact number of stops
  public int CountRoutesWithExactStops(char start, char end, int exactStops)
  {
    int count = 0;
    CountRoutesWithExactStopsDFS(start, end, 0, exactStops, ref count);
    return count;
  }

  // DFS traversal of the graph to count the routes with an exact number of trips
  private void CountRoutesWithExactStopsDFS(char current, char end, int stops, int exactStops, ref int count)
  {
    if (stops == exactStops && current == end)
    {
      count++;
      return;
    }

    if (stops >= exactStops)
      return;

    if (!graph.ContainsKey(current))
      return;

    foreach (var neighbor in graph[current])
      CountRoutesWithExactStopsDFS(neighbor.Key, end, stops + 1, exactStops, ref count);
  }

  // find the shortest route distance between two vertices using Dijkstra
  public int ShortestRouteDistance(char start, char end)
  {
    Dictionary<char, int> distances = new Dictionary<char, int>();
    HashSet<char> visited = new HashSet<char>();
    PriorityQueue<char, int> queue = new PriorityQueue<char, int>();

    // Set initial distances for all vertices except the start vertex
    foreach (char vertex in graph.Keys)
      distances[vertex] = vertex == start ? 0 : int.MaxValue;

    // Enqueue the start vertex with distance 0
    distances[start] = 0;
    queue.Enqueue(start, 0);

    while (queue.Count > 0)
    {
      char current = queue.Dequeue();

      if (current == end && distances[current] != 0)
        break;

      if (visited.Contains(current))
        continue;

      visited.Add(current);

      if (!graph.ContainsKey(current))
        continue;
      
      // Explore neighbors and update distances
      foreach (var neighbor in graph[current])
      {
        int newDistance = distances[current] + neighbor.Value;
        // Update the distance and enqueue the neighbor if it provides a shorter path
        if (newDistance < distances[neighbor.Key] || distances[neighbor.Key] == 0)
        {
          distances[neighbor.Key] = newDistance;
          queue.Enqueue(neighbor.Key, newDistance);
        }
      }
    }

    // Return the distance of the end vertex or -1 if it is unreachable
    return distances[end] == int.MaxValue ? -1 : distances[end];
  }
}

public class Program
{
  public static void Main(string[] args)
  {
    List<string> routes = new List<string>();

    Console.WriteLine("Enter train routes (e.g., AB5, BC4, CD8):");

    string trainRoutesInput = Console.ReadLine();
    string[] trainRoutes = trainRoutesInput.Replace(" ", string.Empty).Split(',');

    foreach (var route in trainRoutes)
    {
      routes.Add(route);
    }

    Trains railroad = new Trains(routes);

    // The distance of the route A-B-C.
    int distanceABC = railroad.CalculateDistance("ABC");
    Console.WriteLine("1. The distance of the route A-B-C: " + (distanceABC != -1 ? distanceABC.ToString() : "NO SUCH ROUTE"));

    // The distance of the route A-D.
    int distanceAD = railroad.CalculateDistance("AD");
    Console.WriteLine("2. The distance of the route A-D: " + (distanceAD != -1 ? distanceAD.ToString() : "NO SUCH ROUTE"));

    // The distance of the route A-D-C.
    int distanceADC = railroad.CalculateDistance("ADC");
    Console.WriteLine("3. The distance of the route A-D-C: " + (distanceADC != -1 ? distanceADC.ToString() : "NO SUCH ROUTE"));

    // The distance of the route A-E-B-C-D.
    int distanceAEBCD = railroad.CalculateDistance("AEBCD");
    Console.WriteLine("4. The distance of the route A-E-B-C-D: " + (distanceAEBCD != -1 ? distanceAEBCD.ToString() : "NO SUCH ROUTE"));

    // The distance of the route A-E-D.
    int distanceAED = railroad.CalculateDistance("AED");
    Console.WriteLine("5. The distance of the route A-E-D: " + (distanceAED != -1 ? distanceAED.ToString() : "NO SUCH ROUTE"));

    // The number of trips starting at C and ending at C with a maximum of 3 stops.
    int tripsCCMaxStops3 = railroad.CountRoutes('C', 'C', 3, "Stops");
    Console.WriteLine("6. The number of trips starting at C and ending at C with a maximum of 3 stops: " + tripsCCMaxStops3);

    // The number of trips starting at A and ending at C with exactly 4 stops.
    int tripsACExactStops4 = railroad.CountRoutesWithExactStops('A', 'C', 4);
    Console.WriteLine("7. The number of trips starting at A and ending at C with exactly 4 stops: " + tripsACExactStops4);

    // The length of the shortest route from A to C.
    int shortestRouteAC = railroad.ShortestRouteDistance('A', 'C');
    Console.WriteLine("8. The length of the shortest route from A to C: " + shortestRouteAC);

    // The length of the shortest route from B to B.
    int shortestRouteBB = railroad.ShortestRouteDistance('B', 'B');
    Console.WriteLine("9. The length of the shortest route from B to B: " + shortestRouteBB);

    // The number of different routes from C to C with a distance of less than 30.
    int routesCCMaxDistance30 = railroad.CountRoutes('C', 'C', 30, "Distance");
    Console.WriteLine("10. The number of different routes from C to C with a distance of less than 30: " + routesCCMaxDistance30);
  }
}
