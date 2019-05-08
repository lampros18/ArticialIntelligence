/*
 * Copyright (c) 2008, 2019, Harokopeio University of Athens Department of Informatics and Telematics and/or its affiliates. All rights reserved.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 *
 * This code is distributed in the hope that it will be useful, but WITHOUT
 * ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
 * FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License
 * version 2 for more details (a copy is included in the LICENSE file that
 * accompanied this code).
 *
 * You should have received a copy of the GNU General Public License version
 * 2 along with this work; if not, write to the Free Software Foundation,
 * Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
 *
 * Please contact Harokopeio University, Informatics and Telematics Department, Tavros, Omirou 9 17778
 * or visit https://www.dit.hua.gr/index.php/en/ if you need additional information or have any
 * questions.
 */
package gr.hua.dit;

import org.jetbrains.annotations.NotNull;

import java.util.*;

/**
 * <h2>This class consists exclusively of graph search algorithms.</h2>
 * <ol>
 * <li>Blind Search Algorithms</li>
 * <ul>
 * <li>Depth-First-Search - DFS</li>
 * </ul>
 * <li>Greedy Algorithms</li>
 * <ul>
 * <li>Best-First-Search</li>
 * </ul>
 * <li>Heuristic Search Algorithms</li>
 * <ul>
 * <li>A*</li>
 * </ul>
 * </ol>
 * A list containing the order with which the algorithm visited the vertices of the graph is being stored in a LinkedHashSet data structure.
 * Note: There are two versions of the Best-First-Search algorithm. The first one always chooses to visit the node with the minimum cost, whilst the other chooses to visit the node with the minimum heuristic value.
 */
class GraphSearchUtils {

    private LinkedHashSet<Node> searchTree;

    /**
     * Default constructor overridden in order to initialise the searchTree.
     * Note: The search tree field contains all the vertices in an order specified by the algorithm that we used to search the graph. In practice the search algorithm uses this field in order to keep track of the vertices visited.
     */
    GraphSearchUtils() {
        searchTree = new LinkedHashSet<>();
    }

    /**
     * @param v A specified node that we want to obtain all its successors(adjacent nodes)
     * @param g A graph
     * @return The all the successors of the specified node
     */
    private LinkedList<Node> retrieveSuccessors(@NotNull Node v, @NotNull Graph g) {
        return g.getAdjacencyMap().get(v.getLabel());
    }


    /**
     * Method that uses the Depth-First-Search algorithm.
     *
     * @param start  The starting point of the graph. From this node the algorithm will start the search.
     * @param target The target node that we want to discover. When the algorithm visits this node the method stop searching.
     * @param g      A graph
     */
    void DFS(Graph g, Node start, Node target) {
        HashMap<Character, Boolean> visited = new HashMap<>();
        DFSUtil(start, target, visited, g);
    }

    /**
     * A helper method.
     *
     * @param start   The starting point of the graph. From this node the algorithm will start the search.
     * @param target  The target node that we want to discover. When the algorithm visits this node the method stop searching.
     * @param visited A map data structure that marks a node when we visit it. We keep track of the nodes we visited so as to avoid visiting them again.
     * @param g       A graph
     */
    private void DFSUtil(@NotNull Node start, Node target, @NotNull HashMap<Character, Boolean> visited, Graph g) {

        visited.put(start.getLabel(), true);
        searchTree.add(start);

        for (Node w : retrieveSuccessors(start, g)) {
            if (visited.get(target.getLabel()) != null)
                return;
            if (visited.get(w.getLabel()) == null)
                DFSUtil(w, target, visited, g);

        }
    }

    /**
     * This method searches a graph by using a greedy tactic. Specifically, it visits the node with the smallest heuristic value. Best-First-Search Implementation.
     * local variable: visited A map data structure that marks a node when we visit it. We keep track of the nodes we visited so as to avoid visiting them again.
     *
     * @param start  The starting point of the graph. From this node the algorithm will start the search.
     * @param target The target node that we want to discover. When the algorithm visits this node the method stop searching.
     * @param g      A graph
     *               Note: A comparator object is created in order to impose an order to the node objects. A priority queue data structure then always has in the first slot the node with the smallest heuristic value.
     */
    void bestFirstSearchHeuristicVersion(Graph g, Node start, Node target) {

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
                for (Node w : retrieveSuccessors(v, g)) {
                    if (visited.get(w.getLabel()) == null) {
                        visited.put(w.getLabel(), true);
                        pq.add(w);
                    }
                }
                visited.put(v.getLabel(), true);
            }
        }
    }

    /**
     * This method searches a graph by using a greedy tactic. Specifically, it visits the node with the smallest cost. Best-First-Search Implementation.
     * local variable: visited A map data structure that marks a node when we visit it. We keep track of the nodes we visited so as to avoid visiting them again.
     *
     * @param start  The starting point of the graph. From this node the algorithm will start the search.
     * @param target The target node that we want to discover. When the algorithm visits this node the method stop searching.
     * @param g      A graph
     *               Note: A comparator object is created in order to impose an order to the node objects. A priority queue data structure then always has in the first slot the node with the smallest cost.
     */
    void bestFirstSearchCostVersion(Graph g, Node start, Node target) {

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
                for (Node w : retrieveSuccessors(v, g)) {
                    if (visited.get(w.getLabel()) == null) {
                        visited.put(w.getLabel(), true);
                        pq.add(w);
                    }
                }
                visited.put(v.getLabel(), true);
            }
        }
    }

    /**
     * This method searches a graph by using an A* implementation.
     * local variable: visited A map data structure that marks a node when we visit it. We keep track of the nodes we visited so as to avoid visiting them again.
     *
     * @param start  The starting point of the graph. From this node the algorithm will start the search.
     * @param target The target node that we want to discover. When the algorithm visits this node the method stop searching.
     * @param g      A graph
     *               Note: A comparator object is created in order to impose an order to the node objects. A priority queue data structure then always has in the first slot the node with the smallest value returned from the evaluation function.
     */
    void astar(Graph g, Node start, Node target) {

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
                for (Node w : retrieveSuccessors(v, g)) {
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

    /**
     * @param w      A specific node.
     * @param parent A map data structure containing the all the parents from node w.
     * @return return the total cost from the root to the specifed node w.
     */
    private int calculateTotalCost(Node w, HashMap<Node, Node> parent) {
        int totalCost = 0;
        Node iteratorNode = w;

        while (iteratorNode != null) {
            totalCost = totalCost + iteratorNode.getCost();
            iteratorNode = parent.get(iteratorNode);
        }
        return totalCost;
    }

    /**
     * @return returns the search tree produces by the search algorithm.
     */
    LinkedHashSet<Node> getSearchTree() {
        return searchTree;
    }

    /**
     * This method purges the search tree.
     */
    void clearSearchTree() {
        searchTree.clear();
    }
}
