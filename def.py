class DisjointSet:
    def __init__(self, n):
        self.parent = [i for i in range(n)]
        self.rank = [0] * n

    def find(self, u):
        if self.parent[u] != u:
            self.parent[u] = self.find(self.parent[u])  # Path compression
        return self.parent[u]

    def union(self, u, v):
        root_u = self.find(u)
        root_v = self.find(v)

        if root_u == root_v:
            return

        if self.rank[root_u] < self.rank[root_v]:
            self.parent[root_u] = root_v
        elif self.rank[root_u] > self.rank[root_v]:
            self.parent[root_v] = root_u
        else:
            self.parent[root_v] = root_u
            self.rank[root_u] += 1

def main():
    # Example usage
    n = 5  # Number of elements
    ds = DisjointSet(n)

    ds.union(0, 1)
    ds.union(1, 2)
    ds.union(3, 4)

    print("Parent array after union operations:", ds.parent)
    print("Rank array after union operations:", ds.rank)

    print("Are 0 and 2 in the same set?", ds.find(0) == ds.find(2))
    print("Are 0 and 3 in the same set?", ds.find(0) == ds.find(3))

if __name__ == "__main__":
    main()
