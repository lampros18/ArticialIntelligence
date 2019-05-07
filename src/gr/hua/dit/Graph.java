package gr.hua.dit;

import org.jetbrains.annotations.NotNull;

import java.util.HashMap;
import java.util.LinkedHashSet;
import java.util.LinkedList;

class Graph {

    private HashMap<Character, LinkedList<Node>> adjacencyMap;

    private LinkedHashSet<Node> searchTree = new LinkedHashSet<>();

    Graph()
    {
        adjacencyMap = new HashMap<>();
    }

    void addNode(@NotNull Node w){
        adjacencyMap.put(w.getLabel(), new LinkedList<>());
    }

    void addEdge(@NotNull Node w, @NotNull Node v, int cost){
        Node temp = new Node(v.getLabel(), cost, v.getHeuristicValue());
        adjacencyMap.get(w.getLabel()).add(temp);
    }

    private LinkedList<Node> retrieveSuccessors(@NotNull Node v){
        return adjacencyMap.get(v.getLabel());
    }





    private void DFSUtil(@NotNull Node start, Node target, @NotNull HashMap<Character, Boolean> visited)
    {
        // Mark the current node as visited and print it
        visited.put(start.getLabel(), true);
        searchTree.add(start);

        for(Node w : retrieveSuccessors(start))
        {
            if(visited.get(target.getLabel()) != null)
                return;
            if ( visited.get(w.getLabel()) == null )
                DFSUtil(w, target, visited);

        }
    }

    // The function to do DFS traversal. It uses recursive DFSUtil()
    void DFS(Node start, Node target)
    {
        HashMap<Character, Boolean> visited = new HashMap<>();
        // Call the recursive helper function to create the Search Tree
        DFSUtil(start, target, visited);
    }

    LinkedHashSet<Node> getSearchTree() {
        return searchTree;
    }
}
