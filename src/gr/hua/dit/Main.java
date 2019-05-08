package gr.hua.dit;

import java.util.Set;

/**
 * This class has a main method to test all the search algorithms given the graph of our assignment.
 */
public class Main {

    public static void main(String[] args) {

        Graph g = new Graph();

        Node S = new Node('S', 0, 4);
        Node A = new Node('A', 0, 2);
        Node B = new Node('B', 0, 3);
        Node C = new Node('C', 0, 4);
        Node D = new Node('D', 0, 5);
        Node E = new Node('E', 0, 6);
        Node F = new Node('F', 0, 4);
        Node G = new Node('G', 0, 0);
        Node H = new Node('H', 0, 2);
        Node I = new Node('I', 0, 2);
        Node J = new Node('J', 0, 1);
        Node K = new Node('K', 0, 5);
        Node L = new Node('L', 0, 6);


        g.addNode(S);
        g.addNode(A);
        g.addNode(B);
        g.addNode(C);
        g.addNode(D);
        g.addNode(E);
        g.addNode(F);
        g.addNode(G);
        g.addNode(H);
        g.addNode(I);
        g.addNode(J);
        g.addNode(K);
        g.addNode(L);

        g.addEdge(S, A, 2);
        g.addEdge(S, F, 1);
        g.addEdge(S, K, 2);
        g.addEdge(A, B, 2);
        g.addEdge(B, C, 2);
        g.addEdge(C, D, 2);
        g.addEdge(D, E, 1);
        g.addEdge(D, G, 5);
        g.addEdge(K, L, 1);
        g.addEdge(F, H, 1);
        g.addEdge(H, I, 1);
        g.addEdge(I, J, 1);
        g.addEdge(J, G, 1);

        GraphSearchUtils gsu = new GraphSearchUtils();

        System.out.println();
        System.out.println("----------------------------------------------------------");
        System.out.println("Running DFS: ");
        gsu.DFS(g, S, G);

        Set<Node> searchTreeDFS = gsu.getSearchTree();

        for (Node node : searchTreeDFS) {
            if (node.getLabel() == G.getLabel())
                System.out.println(node.getLabel());
            else
                System.out.print(node.getLabel() + "-");
        }

        int sum = 0;
        for (Node node :searchTreeDFS) {
            sum += node.getCost();
        }
        System.out.println("DFS search cost : " + sum);
        System.out.println("----------------------------------------------------------");


        System.out.println();
        System.out.println("----------------------------------------------------------");
        System.out.println("Running Best-First-Search cost edition: ");

        gsu.clearSearchTree();

        gsu.bestFirstSearchCostVersion(g, S, G);

        Set<Node> searchTreeBFSCost = gsu.getSearchTree();

        for (Node node : searchTreeBFSCost) {
            if (node.getLabel() == G.getLabel())
                System.out.println(node.getLabel());
            else
                System.out.print(node.getLabel() + "-");
        }

        sum = 0;
        for (Node node :searchTreeBFSCost) {
            sum += node.getCost();
        }
        System.out.println("Best-First-Search cost edition : " + sum);

        System.out.println("----------------------------------------------------------");


        System.out.println();
        System.out.println("----------------------------------------------------------");
        System.out.println("Running Best-First-Search heuristic edition: ");

        gsu.clearSearchTree();

        gsu.bestFirstSearchHeuristicVersion(g, S, G);

        Set<Node> searchTreeBFSHeuristic = gsu.getSearchTree();

        for (Node node : searchTreeBFSHeuristic) {
            if (node.getLabel() == G.getLabel())
                System.out.println(node.getLabel());
            else
                System.out.print(node.getLabel() + "-");
        }

        sum = 0;
        for (Node node :searchTreeBFSHeuristic) {
            sum += node.getCost();
        }
        System.out.println("Best-First-Search heuristic edition : " + sum);

        System.out.println("----------------------------------------------------------");



        System.out.println();
        System.out.println("----------------------------------------------------------");
        System.out.println("Running A*: ");

        gsu.clearSearchTree();

        gsu.astar(g, S, G);

        Set<Node> searchTreeAstar = gsu.getSearchTree();

        for (Node node : searchTreeAstar) {
            if (node.getLabel() == G.getLabel())
                System.out.println(node.getLabel());
            else
                System.out.print(node.getLabel() + "-");
        }

        sum = 0;
        for (Node node :searchTreeAstar) {
            sum += node.getCost();
        }
        System.out.println("A* cost : " + sum);

        System.out.println("----------------------------------------------------------");

    }
}
