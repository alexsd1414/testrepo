#include <iostream>
#include <vector>

using namespace std;

class SegmentTree {
private:
    vector<int> tree;
    vector<int> nums;
    int n;

    void buildTree(int node, int start, int end) {
        if (start == end) {
            tree[node] = nums[start];
            return;
        }
        int mid = start + (end - start) / 2;
        buildTree(2 * node + 1, start, mid);
        buildTree(2 * node + 2, mid + 1, end);
        tree[node] = tree[2 * node + 1] + tree[2 * node + 2];
    }

    int queryHelper(int node, int start, int end, int left, int right) {
        if (start > right || end < left)
            return 0;
        if (left <= start && right >= end)
            return tree[node];
        int mid = start + (end - start) / 2;
        return queryHelper(2 * node + 1, start, mid, left, right) +
               queryHelper(2 * node + 2, mid + 1, end, left, right);
    }

public:
    SegmentTree(const vector<int>& nums) {
        this->nums = nums;
        n = nums.size();
        tree.resize(4 * n);
        buildTree(0, 0, n - 1);
    }

    int query(int left, int right) {
        return queryHelper(0, 0, n - 1, left, right);
    }

    void update(int index, int value) {
        int diff = value - nums[index];
        nums[index] = value;
        updateHelper(0, 0, n - 1, index, diff);
    }

    void updateHelper(int node, int start, int end, int index, int diff) {
        if (index < start || index > end)
            return;
        tree[node] += diff;
        if (start != end) {
            int mid = start + (end - start) / 2;
            updateHelper(2 * node + 1, start, mid, index, diff);
            updateHelper(2 * node + 2, mid + 1, end, index, diff);
        }
    }
};

int main() {
    vector<int> nums = {1, 3, 5, 7, 9, 11};
    SegmentTree segTree(nums);

    cout << "Sum of elements from index 1 to 3: " << segTree.query(1, 3) << endl;

    segTree.update(2, 10);

    cout << "Updated sum of elements from index 1 to 3: " << segTree.query(1, 3) << endl;

    return 0;
}
