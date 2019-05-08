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

/**
 * <h2>Models a node of a graph G = (V, E)</h2>
 * <p>This class holds all the necessary information to store in memory a node w, w ∈ V</p>
 *
 * @version 1.0
 * @since 8/5/2019
 */
class Node {

    private Character label;
    private int cost;
    private int heuristicValue;

    private int f;

    /**
     * <p>Constructor method used to initialize all the fields of this class</p>
     * <p>f: a field that holds the value of the evaluation function of this node. Note: Heuristic function f(x) = h(x) + g(x) where h(x) the heuristic value of the node and g(x) the total cost from this node up to the root node<b>Default value: 0</b></p>
     *
     * @param label          this field stores a character, denoting the name of the node.
     * @param cost           a field that holds the cost of travelling from a parent node to this node. <b>Default value: 0</b>
     * @param heuristicValue As the name suggests it is the heuristic value of the node. Note: Heuristic value is an estimation of the actual cost when we traverse from a parent node to this one.
     */
    Node(Character label, int cost, int heuristicValue) {
        this.label = label;
        this.cost = cost;
        this.heuristicValue = heuristicValue;
        this.f = 0;
    }

    /**
     * <p>Getter method for the "label" field</p>
     *
     * @return returns the value of "label" field
     */
    Character getLabel() {
        return label;
    }

    /**
     * <p>Getter method for the "cost" field</p>
     *
     * @return returns the value of "cost" field
     */
    int getCost() {
        return cost;
    }

    /**
     * <p>Getter method for the "heuristicValue" field</p>
     *
     * @return returns the value of "heuristicValue" field
     */
    int getHeuristicValue() {
        return heuristicValue;
    }

    /**
     * <p>Getter method for the "heuristicValue" field</p>
     *
     * @return returns the value of "heuristicValue" field
     */
    int getF() {
        return f;
    }

    /**
     * <p>Setter method for the "f" field</p>
     *
     * @param f The value of f(x) = g(x) + h(x)
     */
    void setF(int f) {
        this.f = f;
    }
}
