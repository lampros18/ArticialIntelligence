package gr.hua.dit;

public class Main {

    public static void main(String[] args) {

        Graph g = new Graph();

        Node S = new Node('S', 0, 4 );
        Node A = new Node('A', 0, 2 );
        Node B = new Node('B', 0, 3 );
        Node C = new Node('C', 0, 4 );
        Node D = new Node('D', 0, 5 );
        Node E = new Node('E', 0, 6 );
        Node F = new Node('F', 0, 4 );
        Node G = new Node('G', 0, 0 );
        Node H = new Node('H', 0, 2 );
        Node I = new Node('I', 0, 2 );
        Node J = new Node('J', 0, 1 );
        Node K = new Node('K', 0, 5 );
        Node L = new Node('L', 0, 6 );


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

//        g.DFS(S, G);
        g.bestFirstSearch(S, G);

        System.out.println();
        System.out.println("----------------------------------------------------------");

        for(Node node : g.getSearchTree()){
            if(node.getLabel() == G.getLabel())
                System.out.print(node.getLabel());
            else
                System.out.print(node.getLabel()+"-");
        }

        System.out.println();
        System.out.println("----------------------------------------------------------");

        int sum = 0;
        for(Node node : g.getSearchTree()){
            sum+= node.getCost();
        }
        System.out.println("Best First search cost : " + sum);

    }
}
