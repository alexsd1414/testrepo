package main

import (
	"fmt"
	"sort"
)

// SuffixArray là một slice chứa các chỉ số của các hậu tố của chuỗi
type SuffixArray []int

// Tạo một SuffixArray mới từ một chuỗi
func NewSuffixArray(s string) SuffixArray {
	n := len(s)
	suffixes := make([][2]int, n)

	for i := 0; i < n; i++ {
		suffixes[i] = [2]int{i, int(s[i])}
	}

	sort.Slice(suffixes, func(i, j int) bool {
		return suffixes[i][1] < suffixes[j][1] || (suffixes[i][1] == suffixes[j][1] && suffixes[i][0] < suffixes[j][0])
	})

	sa := make([]int, n)
	for i := 0; i < n; i++ {
		sa[i] = suffixes[i][0]
	}

	return sa
}

// Tìm kiếm nhị phân trên SuffixArray để tìm vị trí đầu tiên của một chuỗi trong chuỗi
func (sa SuffixArray) search(s string) int {
	low, high := 0, len(sa)-1
	for low <= high {
		mid := low + (high-low)/2
		index := sa[mid]
		if s == sa.index(index) {
			return index
		} else if s < sa.index(index) {
			high = mid - 1
		} else {
			low = mid + 1
		}
	}
	return -1
}

// Trả về hậu tố tại vị trí chỉ mục trong chuỗi
func (sa SuffixArray) index(i int) string {
	return string(sa[i:])
}

// SuffixTree là một nút trong cây hậu tố
type SuffixTree struct {
	children map[byte]*SuffixTree
}

// Tạo một SuffixTree mới từ một chuỗi
func NewSuffixTree(s string) *SuffixTree {
	root := &SuffixTree{children: make(map[byte]*SuffixTree)}
	for i := 0; i < len(s); i++ {
		curr := root
		for j := i; j < len(s); j++ {
			if _, exists := curr.children[s[j]]; !exists {
				curr.children[s[j]] = &SuffixTree{children: make(map[byte]*SuffixTree)}
			}
			curr = curr.children[s[j]]
		}
	}
	return root
}

// Tìm kiếm một chuỗi trong cây hậu tố
func (st *SuffixTree) search(s string) bool {
	curr := st
	for i := 0; i < len(s); i++ {
		if _, exists := curr.children[s[i]]; !exists {
			return false
		}
		curr = curr.children[s[i]]
	}
	return true
}

func main() {
	// Test SuffixArray
	text := "banana"
	sa := NewSuffixArray(text)
	fmt.Println("Suffix Array for 'banana':", sa)

	index := sa.search("ana")
	if index != -1 {
		fmt.Println("Found 'ana' at index:", index)
	} else {
		fmt.Println("'ana' not found in the suffix array")
	}

	// Test SuffixTree
	st := NewSuffixTree(text)
	fmt.Println("\nSuffix Tree for 'banana':", st)

	if st.search("nan") {
		fmt.Println("Found 'nan' in the suffix tree")
	} else {
		fmt.Println("'nan' not found in the suffix tree")
	}
}
