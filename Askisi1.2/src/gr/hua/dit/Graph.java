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
 * <h2>Models a directed graph G = (V, E)</h2>
 *
 * <p>A key-value pair combination represents the connection between two nodes(the edge). The key will be the name of the parent node,
 * whilst the value will be a Linked List containing all the nodes adjacent to parent.</p>
 * <p>While in fact an adjacency list holds the successors of a specified node the name adjacencyMap is used, because a HashMap data structure
 * holds in its key the label of a node(name) and the value, holds the adjacency list containing the successors vertices.</p>
 * <p>In practice, our graph is modeled in computer's memory by the use of a HashMap data structure.</p>
 *
 * @version 1.0
 * @since 8/5/2019
 */
class Graph {

    private HashMap<Character, LinkedList<Node>> adjacencyMap;

    /**
     * Default constructor overridden so as to initialize the adjacency map of the graph.
     */
    Graph() {
        adjacencyMap = new HashMap<>();
    }

    /**
     * This method is responsible for inserting a node into the graph.
     * Specifically, the name(label) of the node will become the key of the Map, while an empty LinkedList(Adjacency List) will become the value
     * (Note: An empty Adjacency List denotes that the node hasn't any nodes adjacent to it).
     *
     * @param w Contains a Node object
     */
    void addNode(@NotNull Node w) {
        adjacencyMap.put(w.getLabel(), new LinkedList<>());
    }

    /**
     * <p>This method is responsible for connecting two vertices with an edge.</p>
     * <p>First of all it creates a new node called temp to create a new object(of type node) containing the cost of traversing from node w to node v.</p>
     * The reason for this object creation is for two main reasons:
     * <ul>
     *  <li>Java uses call by reference when passing objects between the caller and the called method.</li>
     *  <li>A node v may be adjacent(connected) to more than one vertices.</li>
     * </ul>
     * <p>Conclusion, if we do not create a new instance of the <b>v</b> node every time, we may change its cost unintentionally
     * due to the fact that the original node w(node that we want to add adjacent vertices) with default cost 0 has a specific address to memory
     * and each time we pass the node to the addEdge method we pass the same reference value(address).
     * Last but not least, the adjacency list of the specified node w is obtained,and then an insertion to the adjacency list is taking place.</p>
     *
     * @param w    The node that we want to add an adjacent node to its adjacency list.
     * @param v    An adjacent node.
     * @param cost The cost of traversing from node w to v.
     */
    void addEdge(@NotNull Node w, @NotNull Node v, int cost) {
        Node temp = new Node(v.getLabel(), cost, v.getHeuristicValue());
        adjacencyMap.get(w.getLabel()).add(temp);
    }

    /**
     * This method returns the adjacency map of the graph.
     *
     * @return Returns a map containing for each and every node its adjacent vertices.
     */
    HashMap<Character, LinkedList<Node>> getAdjacencyMap() {
        return adjacencyMap;
    }
}
