package gr.hua.dit;

import org.jetbrains.annotations.NotNull;

import java.util.*;

class Graph {

    private HashMap<Character, LinkedList<Node>> adjacencyMap;

    private LinkedHashSet<Node> searchTree = new LinkedHashSet<>();

    Graph() {
        adjacencyMap = new HashMap<>();
    }

    void addNode(@NotNull Node w) {
        adjacencyMap.put(w.getLabel(), new LinkedList<>());
    }

    void addEdge(@NotNull Node w, @NotNull Node v, int cost) {
        Node temp = new Node(v.getLabel(), cost, v.getHeuristicValue());
        adjacencyMap.get(w.getLabel()).add(temp);
    }

    private LinkedList<Node> retrieveSuccessors(@NotNull Node v) {
        return adjacencyMap.get(v.getLabel());
    }


    private void DFSUtil(@NotNull Node start, Node target, @NotNull HashMap<Character, Boolean> visited) {
        // Mark the current node as visited and print it
        visited.put(start.getLabel(), true);
        searchTree.add(start);

        for (Node w : retrieveSuccessors(start)) {
            if (visited.get(target.getLabel()) != null)
                return;
            if (visited.get(w.getLabel()) == null)
                DFSUtil(w, target, visited);

        }
    }

    // The function to do DFS traversal. It uses recursive DFSUtil()
    void DFS(Node start, Node target) {
        HashMap<Character, Boolean> visited = new HashMap<>();
        // Call the recursive helper function to create the Search Tree
        DFSUtil(start, target, visited);
    }


    void bestFirstSearchHeuristicVersion(Node start, Node target) {

        PriorityQueue<Node> pq = new PriorityQueue<>((o1, o2) -> {
            if (o1.getHeuristicValue() == o2.getHeuristicValue())
                return o1.getLabel().compareTo(o2.getLabel());
            else
                return Integer.compare(o1.getHeuristicValue(), o2.getHeuristicValue());

        });
        HashMap<Character, Boolean> visited = new HashMap<>();
        pq.add(start);

        while (!pq.isEmpty()) {
            Node v = pq.poll();
            searchTree.add(v);
            if (v.getLabel() == target.getLabel())
                return;
            else {
                for (Node w : retrieveSuccessors(v)) {
                    if (visited.get(w.getLabel()) == null) {
                        visited.put(w.getLabel(), true);
                        pq.add(w);
                    }
                }
                visited.put(v.getLabel(), true);
            }
        }
    }

    void bestFirstSearchCostVersion(Node start, Node target) {

        PriorityQueue<Node> pq = new PriorityQueue<>((o1, o2) -> {
            if (o1.getCost() == o2.getCost())
                return o1.getLabel().compareTo(o2.getLabel());
            else
                return Integer.compare(o1.getCost(), o2.getCost());

        });
        HashMap<Character, Boolean> visited = new HashMap<>();
        pq.add(start);

        while (!pq.isEmpty()) {
            Node v = pq.poll();
            searchTree.add(v);
            if (v.getLabel() == target.getLabel())
                return;
            else {
                for (Node w : retrieveSuccessors(v)) {
                    if (visited.get(w.getLabel()) == null) {
                        visited.put(w.getLabel(), true);
                        pq.add(w);
                    }
                }
                visited.put(v.getLabel(), true);
            }
        }
    }

    void astar(Node start, Node target) {

        PriorityQueue<Node> pq = new PriorityQueue<>((o1, o2) -> {
            if (o1.getF() == o2.getF())
                return o1.getLabel().compareTo(o2.getLabel());
            else
                return Integer.compare(o1.getF(), o2.getF());

        });

        HashMap<Character, Boolean> visited = new HashMap<>();
        HashMap<Node, Node> parent = new HashMap<>();
        pq.add(start);
        parent.put(start, null);

        while (!pq.isEmpty()) {
            Node v = pq.poll();
            searchTree.add(v);
            if (v.getLabel() == target.getLabel())
                return;
            else {
                for (Node w : retrieveSuccessors(v)) {
                    if (visited.get(w.getLabel()) == null) {
                        parent.put(w, v);
                        w.setF(w.getHeuristicValue() + calculateTotalCost(w, parent));
                        visited.put(w.getLabel(), true);
                        pq.add(w);
                    }
                }
                visited.put(v.getLabel(), true);
            }
        }
    }

    private int calculateTotalCost(Node w, HashMap<Node, Node> parent) {
        int totalCost = 0;
        Node iteratorNode = w;

        while (iteratorNode != null) {
            totalCost = totalCost + iteratorNode.getCost();
            iteratorNode = parent.get(iteratorNode);
        }
        return totalCost;
    }

    LinkedHashSet<Node> getSearchTree() {
        return searchTree;
    }
}
