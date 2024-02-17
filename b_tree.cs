using System;

public class BTreeNode {
    private int[] keys;
    private BTreeNode[] C;
    private int n;
    private bool leaf;
    private int t;

    public BTreeNode(int _t, bool _leaf) {
        t = _t;
        leaf = _leaf;

        keys = new int[2 * t - 1];
        C = new BTreeNode[2 * t];
        n = 0;
    }

    public int FindKey(int k) {
        int idx = 0;
        while (idx < n && keys[idx] < k)
            idx++;
        return idx;
    }

    public void Remove(int k) {
        int idx = FindKey(k);

        if (idx < n && keys[idx] == k) {
            if (leaf)
                RemoveFromLeaf(idx);
            else
                RemoveFromNonLeaf(idx);
        } else {
            if (leaf) {
                Console.WriteLine("The key " + k + " does not exist in the tree");
                return;
            }
            bool flag = (idx == n) ? true : false;
            if (C[idx].n < t)
                Fill(idx);
            if (flag && idx > n)
                C[idx - 1].Remove(k);
            else
                C[idx].Remove(k);
        }
        return;
    }

    private void RemoveFromLeaf(int idx) {
        for (int i = idx + 1; i < n; ++i)
            keys[i - 1] = keys[i];
        n--;
        return;
    }

    private void RemoveFromNonLeaf(int idx) {
        int k = keys[idx];
        if (C[idx].n >= t) {
            int pred = GetPred(idx);
            keys[idx] = pred;
            C[idx].Remove(pred);
        } else if (C[idx + 1].n >= t) {
            int succ = GetSucc(idx);
            keys[idx] = succ;
            C[idx + 1].Remove(succ);
        } else {
            Merge(idx);
            C[idx].Remove(k);
        }
        return;
    }

    private int GetPred(int idx) {
        BTreeNode cur = C[idx];
        while (!cur.leaf)
            cur = cur.C[cur.n];
        return cur.keys[cur.n - 1];
    }

    private int GetSucc(int idx) {
        BTreeNode cur = C[idx + 1];
        while (!cur.leaf)
            cur = cur.C[0];
        return cur.keys[0];
    }

    private void Fill(int idx) {
        if (idx != 0 && C[idx - 1].n >= t)
            BorrowFromPrev(idx);
        else if (idx != n && C[idx + 1].n >= t)
            BorrowFromNext(idx);
        else {
            if (idx != n)
                Merge(idx);
            else
                Merge(idx - 1);
        }
        return;
    }

    private void BorrowFromPrev(int idx) {
        BTreeNode child = C[idx];
        BTreeNode sibling = C[idx - 1];
        for (int i = child.n - 1; i >= 0; --i)
            child.keys[i + 1] = child.keys[i];
        if (!child.leaf) {
            for (int i = child.n; i >= 0; --i)
                child.C[i + 1] = child.C[i];
        }
        child.keys[0] = keys[idx - 1];
        if (!child.leaf)
            child.C[0] = sibling.C[sibling.n];
        keys[idx - 1] = sibling.keys[sibling.n - 1];
        child.n += 1;
        sibling.n -= 1;
        return;
    }

    private void BorrowFromNext(int idx) {
        BTreeNode child = C[idx];
        BTreeNode sibling = C[idx + 1];
        child.keys[(child.n)] = keys[idx];
        if (!(child.leaf))
            child.C[(child.n) + 1] = sibling.C[0];
        keys[idx] = sibling.keys[0];
        for (int i = 1; i < sibling.n; ++i)
            sibling.keys[i - 1] = sibling.keys[i];
        if (!sibling.leaf) {
            for (int i = 1; i <= sibling.n; ++i)
                sibling.C[i - 1] = sibling.C[i];
        }
        child.n += 1;
        sibling.n -= 1;
        return;
    }

    private void Merge(int idx) {
        BTreeNode child = C[idx];
        BTreeNode sibling = C[idx + 1];
        child.keys[t - 1] = keys[idx];
        for (int i = 0; i < sibling.n; ++i)
            child.keys[i + t] = sibling.keys[i];
        if (!child.leaf) {
            for (int i = 0; i <= sibling.n; ++i)
                child.C[i + t] = sibling.C[i];
        }
        for (int i = idx + 1; i < n; ++i)
            keys[i - 1] = keys[i];
        for (int i = idx + 2; i <= n; ++i)
            C[i - 1] = C[i];
        child.n += sibling.n + 1;
        n--;
        return;
    }

    public void Traverse() {
        int i;
        for (i = 0; i < n; i++) {
            if (leaf == false)
                C[i].Traverse();
            Console.Write(" " + keys[i]);
        }
        if (leaf == false)
            C[i].Traverse();
    }

    public BTreeNode Search(int k) {
        int i = 0;
        while (i < n && k > keys[i])
            i++;
        if (keys[i] == k)
            return this;
        if (leaf == true)
            return null;
        return C[i].Search(k);
    }
}

public class BTree {
    private BTreeNode root;
    private int t;

    public BTree(int _t) {
        root = null;
        t = _t;
    }

    public void Traverse() {
        if (root != null)
            root.Traverse();
    }

    public BTreeNode Search(int k) {
        return (root == null) ? null : root.Search(k);
    }

    public void Insert(int k) {
        if (root == null) {
            root = new BTreeNode(t, true);
            root.keys[0] = k;
            root.n = 1;
        } else {
            if (root.n == 2 * t - 1) {
                BTreeNode s = new BTreeNode(t, false);
                s.C[0] = root;
                s.SplitChild(0, root);
                int i = 0;
                if (s.keys[0] < k)
                    i++;
                s.C[i].InsertNonFull(k);
                root = s;
            } else
                root.InsertNonFull(k);
        }
    }

    public void Remove(int k) {
        if (root == null) {
            Console.WriteLine("The tree is empty");
            return;
        }
        root.Remove(k);
        if (root.n == 0) {
            BTreeNode tmp = root;
            if (root.leaf)
                root = null;
            else
                root = root.C[0];
            tmp = null;
        }
        return;
    }
}

public class MainClass {
    public static void Main(string[] args) {
        BTree tree = new BTree(3);

        tree.Insert(10);
        tree.Insert(20);
        tree.Insert(5);
        tree.Insert(6);
        tree.Insert(12);
        tree.Insert(30);
        tree.Insert(7);
        tree.Insert(17);

        Console.WriteLine("Traversal of the constucted tree is ");
        tree.Traverse();

        tree.Remove(6);
        Console.WriteLine("\nTraversal of the tree after removing 6");
        tree.Traverse();
    }
}
