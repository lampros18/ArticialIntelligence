package gr.hua.dit;

class Node {

    private Character label;
    private int cost;
    private int heuristicValue;

    Node(Character label, int cost, int heuristicValue) {
        this.label = label;
        this.cost = cost;
        this.heuristicValue = heuristicValue;
    }

    Character getLabel() {
        return label;
    }


    int getCost() {
        return cost;
    }


    int getHeuristicValue() {
        return heuristicValue;
    }


}
