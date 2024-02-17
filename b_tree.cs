using System;
using System.Collections.Generic;

public class BTreeNode
{
    public int[] keys;
    public int t;
    public BTreeNode[] child;
    public int n;
    public bool leaf;

    public BTreeNode(int t, bool leaf)
    {
        this.t = t;
        this.leaf = leaf;
        keys = new int[2 * t - 1];
        child = new BTreeNode[2 * t];
        n = 0;
    }

    public void traverse()
    {
        int i = 0;
        for (i = 0; i < n; i++)
        {
            if (leaf == false)
                child[i].traverse();
            Console.Write(keys[i] + " ");
        }
        if (leaf == false)
            child[i].traverse();
    }

    public BTreeNode search(int k)
    {
        int i = 0;
        while (i < n && k > keys[i])
            i++;
        if (keys[i] == k)
            return this;
        if (leaf == true)
            return null;
        return child[i].search(k);
    }

    public void insertNonFull(int k)
    {
        int i = n - 1;
        if (leaf == true)
        {
            while (i >= 0 && keys[i] > k)
            {
                keys[i + 1] = keys[i];
                i--;
            }
            keys[i + 1] = k;
            n = n + 1;
        }
        else
        {
            while (i >= 0 && keys[i] > k)
                i--;
            if (child[i + 1].n == 2 * t - 1)
            {
                splitChild(i + 1, child[i + 1]);
                if (keys[i + 1] < k)
                    i++;
            }
            child[i + 1].insertNonFull(k);
        }
    }

    public void splitChild(int i, BTreeNode y)
    {
        BTreeNode z = new BTreeNode(y.t, y.leaf);
        z.n = t - 1;
        for (int j = 0; j < t - 1; j++)
            z.keys[j] = y.keys[j + t];
        if (y.leaf == false)
        {
            for (int j = 0; j < t; j++)
                z.child[j] = y.child[j + t];
        }
        y.n = t - 1;
        for (int j = n; j >= i + 1; j--)
            child[j + 1] = child[j];
        child[i + 1] = z;
        for (int j = n - 1; j >= i; j--)
            keys[j + 1] = keys[j];
        keys[i] = y.keys[t - 1];
        n = n + 1;
    }
}

public class BTree
{
    public BTreeNode root;
    public int t;

    public BTree(int t)
    {
        this.t = t;
        root = null;
    }

    public void traverse()
    {
        if (root != null)
            root.traverse();
    }

    public BTreeNode search(int k)
    {
        if (root == null)
            return null;
        else
            return root.search(k);
    }

    public void insert(int k)
    {
        if (root == null)
        {
            root = new BTreeNode(t, true);
            root.keys[0] = k;
            root.n = 1;
        }
        else
        {
            if (root.n == 2 * t - 1)
            {
                BTreeNode temp = new BTreeNode(t, false);
                temp.child[0] = root;
                temp.splitChild(0, root);
                int i = 0;
                if (temp.keys[0] < k)
                    i++;
                temp.child[i].insertNonFull(k);
                root = temp;
            }
            else
                root.insertNonFull(k);
        }
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        BTree b = new BTree(3);
        b.insert(10);
        b.insert(20);
        b.insert(5);
        b.insert(6);
        b.insert(12);
        b.insert(30);
        b.insert(7);
        b.insert(17);

        Console.WriteLine("Traversal of the constucted tree is");
        b.traverse();
    }
}
