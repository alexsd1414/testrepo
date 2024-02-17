class Graph:
    def __init__(self):
        self.graph = {}

    def add_edge(self, u, v):
        if u not in self.graph:
            self.graph[u] = []
        self.graph[u].append(v)

    def remove_edge(self, u, v):
        if u in self.graph and v in self.graph[u]:
            self.graph[u].remove(v)

    def print_graph(self):
        for vertex in self.graph:
            print(vertex, "->", " -> ".join(map(str, self.graph[vertex])))

def main():
    # Tạo một đồ thị vô hướng
    graph = Graph()

    # Thêm các cạnh vào đồ thị
    graph.add_edge(1, 2)
    graph.add_edge(1, 3)
    graph.add_edge(2, 3)
    graph.add_edge(3, 4)
    graph.add_edge(4, 5)

    # In đồ thị
    print("Đồ thị sau khi thêm các cạnh:")
    graph.print_graph()

    # Xóa một cạnh khỏi đồ thị
    graph.remove_edge(3, 4)

    # In đồ thị sau khi xóa cạnh
    print("\nĐồ thị sau khi xóa cạnh (3, 4):")
    graph.print_graph()

if __name__ == "__main__":
    main()
