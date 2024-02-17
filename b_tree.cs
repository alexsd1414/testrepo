using System;
using System.Collections.Generic;

public class BTree<T> where T : IComparable<T>
{
    private readonly int _order;
    private Node _root;

    public BTree(int order)
    {
        _order = order;
        _root = new Node();
    }

    public void Insert(T value)
    {
        // Tìm vị trí để chèn
        Node node = _root;
        while (!node.IsLeaf)
        {
            int index = FindChildIndex(node.Keys, value);
            node = node.Children[index];
        }

        // Chèn giá trị vào node lá
        node.Keys.Add(value);
        if (node.Keys.Count > _order - 1)
        {
            SplitNode(node);
        }
    }

    public bool Search(T value)
    {
        Node node = _root;
        while (node != null)
        {
            int index = FindChildIndex(node.Keys, value);
            if (index < node.Keys.Count && node.Keys[index].Equals(value))
            {
                return true;
            }
            else
            {
                node = node.Children[index];
            }
        }

        return false;
    }

    public void Delete(T value)
    {
        // Tìm node chứa giá trị cần xóa
        Node node = FindNode(value);
        if (node == null)
        {
            return;
        }

        // Xóa giá trị khỏi node
        int index = node.Keys.IndexOf(value);
        node.Keys.RemoveAt(index);

        // Nếu node lá có ít hơn `_order / 2` giá trị, cần cân bằng lại cây
        if (node.IsLeaf && node.Keys.Count < _order / 2)
        {
            BalanceTree(node);
        }
    }

    private Node FindNode(T value)
    {
        Node node = _root;
        while (node != null)
        {
            int index = FindChildIndex(node.Keys, value);
            if (index < node.Keys.Count && node.Keys[index].Equals(value))
            {
                return node;
            }
            else
            {
                node = node.Children[index];
            }
        }

        return null;
    }

    private int FindChildIndex(List<T> keys, T value)
    {
        int low = 0;
        int high = keys.Count - 1;

        while (low <= high)
        {
            int mid = (low + high) / 2;
            int comparison = value.CompareTo(keys[mid]);

            if (comparison < 0)
            {
                high = mid - 1;
            }
            else if (comparison > 0)
            {
                low = mid + 1;
            }
            else
            {
                return mid;
            }
        }

        return low;
    }

    private void SplitNode(Node node)
    {
        // Tạo node mới để chứa một nửa số key của node hiện tại
        Node newNode = new Node();
        newNode.IsLeaf = node.IsLeaf;

        // Di chuyển một nửa số key sang node mới
        int mid = (node.Keys.Count - 1) / 2;
        for (int i = mid + 1; i < node.Keys.Count; i++)
        {
            newNode.Keys.Add(node.Keys[i]);
        }

        // Cập nhật node hiện tại
        node.Keys.RemoveRange(mid + 1, node.Keys.Count - mid - 1);

        // Thêm node mới vào danh sách con của node cha
        int index = node.Parent.Children.IndexOf(node);
        node.Parent.Children.Insert(index + 1, newNode);

        // Cập nhật parent của node mới
        newNode.Parent = node.Parent;

        // Nếu node cha đã đầy, cần chia đôi node cha
        if (node.Parent.Keys.Count > _order - 1)
        {
            SplitNode(node.Parent);
        }
    }
}

public class Example
{
    public static void Main(string[] args)
    {
        // Tạo B-Tree với order là 5
        BTree<string> bTree = new BTree<string>(5);

        // Thêm tên vào B-Tree
        bTree.Insert("Alice");
        bTree.Insert("Bob");
        bTree.Insert("Carol");
        bTree.Insert("Dave");
        bTree.Insert("Eve");

        // Tìm kiếm tên trong B-Tree
        bool isFound = bTree.Search("Bob");
        Console.WriteLine("Bob được tìm thấy trong B-Tree: " + isFound);

        // Xóa tên khỏi B-Tree
        bTree.Delete("Carol");

        // Duyệt qua B-Tree
        foreach (string name in bTree)
        {
            Console.WriteLine(name);
        }
    }
}
