class LRUCache:
    def __init__(self, capacity: int):
        self.capacity = capacity
        self.cache = {}
        self.order = []

    def get(self, key: int) -> int:
        if key in self.cache:
            # Nếu key đã tồn tại trong cache, di chuyển nó lên đầu danh sách order
            self.order.remove(key)
            self.order.insert(0, key)
            return self.cache[key]
        else:
            return -1

    def put(self, key: int, value: int) -> None:
        if key in self.cache:
            # Nếu key đã tồn tại trong cache, cập nhật giá trị và di chuyển nó lên đầu danh sách order
            self.cache[key] = value
            self.order.remove(key)
            self.order.insert(0, key)
        else:
            # Nếu key chưa tồn tại trong cache
            if len(self.cache) >= self.capacity:
                # Nếu cache đã đầy, loại bỏ phần tử LRU (cuối danh sách order)
                lru_key = self.order.pop()
                del self.cache[lru_key]
            # Thêm phần tử mới vào cache và đưa nó lên đầu danh sách order
            self.cache[key] = value
            self.order.insert(0, key)


# Test ví dụ cho LRU Cache
if __name__ == "__main__":
    # Khởi tạo một cache có dung lượng là 3
    cache = LRUCache(3)

    # Thêm các cặp key-value vào cache
    cache.put(1, 1)
    cache.put(2, 2)
    cache.put(3, 3)

    # Hiển thị cache sau khi thêm các cặp key-value
    print("Cache sau khi thêm các cặp key-value:")
    print("Cache:", cache.cache)
    print("Order:", cache.order)

    # Truy cập một key trong cache
    print("\nTruy cập key 2:", cache.get(2))

    # Hiển thị cache sau khi truy cập key 2
    print("\nCache sau khi truy cập key 2:")
    print("Cache:", cache.cache)
    print("Order:", cache.order)

    # Thêm một cặp key-value mới vào cache, vượt quá dung lượng
    cache.put(4, 4)

    # Hiển thị cache sau khi thêm phần tử mới vượt quá dung lượng
    print("\nCache sau khi thêm phần tử mới vượt quá dung lượng:")
    print("Cache:", cache.cache)
    print("Order:", cache.order)

    # Truy cập một key không tồn tại trong cache
    print("\nTruy cập key 1:", cache.get(1))

    # Hiển thị cache sau khi truy cập key 1
    print("\nCache sau khi truy cập key 1:")
    print("Cache:", cache.cache)
    print("Order:", cache.order)
