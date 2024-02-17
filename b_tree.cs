using System;

class BTreeNode
{
    public int[] keys;
    public int t; // Minimum degree (defines the range for number of keys)
    public BTreeNode[] C;
    public int n; // Current number of keys
    public bool leaf;

    public BTreeNode(int t1, bool leaf1)
    {
        t = t1;
        leaf = leaf1;

        // Allocate memory for maximum number of possible keys
        // and child pointers
        keys = new int[2 * t - 1];
        C = new BTreeNode[2 * t];

        // Initialize the number of keys as 0
        n = 0;
    }

    // Function to traverse all nodes in a subtree rooted with this node
    public void traverse()
    {
        // There are n keys and n+1 children, traverse through n keys
        // and first n children
        int i;
        for (i = 0; i < n; i++)
        {
            // If this is not leaf, then before printing key[i],
            // traverse the subtree rooted with child C[i].
            if (leaf == false)
            {
                C[i].traverse();
            }
            Console.Write(" " + keys[i]);
        }

        // Print the subtree rooted with last child
        if (leaf == false)
            C[i].traverse();
    }
}

class BTree
{
    public BTreeNode root; // Pointer to root node
    public int t; // Minimum degree

    // Constructor (Initializes tree as empty)
    public BTree(int t1)
    {
        root = null;
        t = t1;
    }

    // function to traverse the tree
    public void traverse()
    {
        if (root != null) root.traverse();
    }
}

public class Program
{
    public static void Main()
    {
        BTree t = new BTree(3); // A B-Tree with minimum degree 3
        t.root = new BTreeNode(3, true);
        t.root.keys[0] = 1;
        t.root.keys[1] = 3;
        t.root.keys[2] = 7;
        t.root.n = 3;

        t.root.C[0] = new BTreeNode(3, true);
        t.root.C[0].keys[0] = 10;
        t.root.C[0].keys[1] = 20;
        t.root.C[0].keys[2] = 30;
        t.root.C[0].n = 3;

        t.root.C[1] = new BTreeNode(3, true);
        t.root.C[1].keys[0] = 40;
        t.root.C[1].keys[1] = 50;
        t.root.C[1].keys[2] = 60;
        t.root.C[1].n = 3;

        t.root.C[2] = new BTreeNode(3, true);
        t.root.C[2].keys[0] = 70;
        t.root.C[2].keys[1] = 80;
        t.root.C[2].keys[2] = 90;
        t.root.C[2].n = 3;

        Console.WriteLine("Traversal of tree constructed is");
        t.traverse();
    }
}
