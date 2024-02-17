class DisjointSet:
    def __init__(self, vertices):
        self.vertices = vertices
        self.parent = {}
        for v in vertices:
            self.parent[v] = v
        self.rank = dict.fromkeys(vertices, 0)

    def find(self, item):
        if self.parent[item] == item:
            return item
        else:
            return self.find(self.parent[item])

    def union(self, x, y):
        x_root = self.find(x)
        y_root = self.find(y)
        if self.rank[x_root] < self.rank[y_root]:
            self.parent[x_root] = y_root
        elif self.rank[x_root] > self.rank[y_root]:
            self.parent[y_root] = x_root
        else:
            self.parent[y_root] = x_root
            self.rank[x_root] += 1

# Tạo một Disjoint Set với các đỉnh từ 1 đến 5
vertices = [1, 2, 3, 4, 5]
ds = DisjointSet(vertices)

# Kết hợp các tập hợp
ds.union(1, 2)
ds.union(3, 4)
ds.union(4, 5)

# Kiểm tra xem các đỉnh có cùng tập hợp hay không
for i in range(1, 6):
    for j in range(i+1, 6):
        if ds.find(i) == ds.find(j):
            print(f"Vertex {i} and vertex {j} are in the same set.")
        else:
            print(f"Vertex {i} and vertex {j} are in different sets.")
