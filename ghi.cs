using System;

public class KDNode
{
    public int[] point;
    public KDNode left, right;

    public KDNode(int[] point)
    {
        this.point = point;
        this.left = null;
        this.right = null;
    }
}

public class KDTree
{
    private KDNode root;
    private int k; // số chiều

    public KDTree(int k)
    {
        this.k = k;
        this.root = null;
    }

    // Hàm xây dựng cây
    private KDNode BuildKDTree(int[][] points, int depth)
    {
        int n = points.Length;
        if (n == 0) return null;

        int axis = depth % k;
        Array.Sort(points, (a, b) => a[axis].CompareTo(b[axis]));

        int mid = n / 2;
        KDNode node = new KDNode(points[mid]);

        node.left = BuildKDTree(SubArray(points, 0, mid), depth + 1);
        node.right = BuildKDTree(SubArray(points, mid + 1, n - mid - 1), depth + 1);

        return node;
    }

    // Hàm tạo mảng con từ mảng cha
    private int[][] SubArray(int[][] array, int start, int length)
    {
        int[][] subArray = new int[length][];
        Array.Copy(array, start, subArray, 0, length);
        return subArray;
    }

    // Hàm thêm một điểm vào cây
    public void Add(int[] point)
    {
        if (root == null)
            root = new KDNode(point);
        else
            AddRecursive(root, point, 0);
    }

    // Hàm đệ quy thêm một điểm vào cây
    private void AddRecursive(KDNode node, int[] point, int depth)
    {
        int axis = depth % k;

        if (point[axis] < node.point[axis])
        {
            if (node.left == null)
                node.left = new KDNode(point);
            else
                AddRecursive(node.left, point, depth + 1);
        }
        else
        {
            if (node.right == null)
                node.right = new KDNode(point);
            else
                AddRecursive(node.right, point, depth + 1);
        }
    }

    // Hàm tìm kiếm điểm gần nhất
    public int[] FindNearestNeighbor(int[] target)
    {
        if (root == null)
            return null;

        KDNode nearest = FindNearestNeighbor(root, target, 0, root);
        return nearest.point;
    }

    // Hàm đệ quy tìm kiếm điểm gần nhất
    private KDNode FindNearestNeighbor(KDNode node, int[] target, int depth, KDNode nearest)
    {
        if (node == null)
            return nearest;

        int axis = depth % k;
        KDNode nextBranch = null;
        KDNode otherBranch = null;

        if (target[axis] < node.point[axis])
        {
            nextBranch = node.left;
            otherBranch = node.right;
        }
        else
        {
            nextBranch = node.right;
            otherBranch = node.left;
        }

        nearest = FindNearestNeighbor(nextBranch, target, depth + 1, nearest);

        if (DistanceSquared(node.point, target) < DistanceSquared(nearest.point, target))
            nearest = node;

        if (Math.Abs(target[axis] - node.point[axis]) < Math.Sqrt(DistanceSquared(nearest.point, target)))
            nearest = FindNearestNeighbor(otherBranch, target, depth + 1, nearest);

        return nearest;
    }

    // Hàm tính bình phương khoảng cách giữa hai điểm
    private int DistanceSquared(int[] p1, int[] p2)
    {
        int distance = 0;
        for (int i = 0; i < k; i++)
        {
            distance += (p1[i] - p2[i]) * (p1[i] - p2[i]);
        }
        return distance;
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Test KD-Tree
        int[][] points = new int[][]
        {
            new int[] {2, 3},
            new int[] {5, 4},
            new int[] {9, 6},
            new int[] {4, 7},
            new int[] {8, 1},
            new int[] {7, 2}
        };

        KDTree kdTree = new KDTree(2); // 2 chiều

        foreach (var point in points)
        {
            kdTree.Add(point);
        }

        int[] target = new int[] { 9, 2 };
        int[] nearest = kdTree.FindNearestNeighbor(target);
        Console.WriteLine("Nearest neighbor to {0},{1} is {2},{3}", target[0], target[1], nearest[0], nearest[1]);
    }
}
