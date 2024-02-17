class LFUCache:
    def __init__(self, capacity: int):
        self.capacity = capacity
        self.cache = {}
        self.freq = {}
        self.min_freq = 0

    def get(self, key: int) -> int:
        if key not in self.cache:
            return -1

        self.freq[key] += 1
        self.update_min_freq(key)
        return self.cache[key]

    def put(self, key: int, value: int) -> None:
        if self.capacity == 0:
            return

        if key in self.cache:
            self.cache[key] = value
            self.freq[key] += 1
        else:
            if len(self.cache) >= self.capacity:
                self.remove_min_freq()

            self.cache[key] = value
            self.freq[key] = 1
            self.min_freq = 1

    def update_min_freq(self, key: int) -> None:
        freq = self.freq[key]
        if freq == self.min_freq and len([k for k, v in self.freq.items() if v == freq]) == 0:
            self.min_freq += 1

    def remove_min_freq(self) -> None:
        keys = [k for k, v in self.freq.items() if v == self.min_freq]
        key_to_remove = keys[0]
        del self.cache[key_to_remove]
        del self.freq[key_to_remove]


def main():
    # Test LFU Cache
    cache = LFUCache(2)

    cache.put(1, 1)
    cache.put(2, 2)
    print(cache.get(1))  # returns 1
    cache.put(3, 3)       # evicts key 2
    print(cache.get(2))  # returns -1 (not found)
    print(cache.get(3))  # returns 3.
    cache.put(4, 4)       # evicts key 1.
    print(cache.get(1))  # returns -1 (not found)
    print(cache.get(3))  # returns 3
    print(cache.get(4))  # returns 4


if __name__ == "__main__":
    main()
