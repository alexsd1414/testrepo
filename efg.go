package main

import "fmt"

type Node struct {
	key    int
	left   *Node
	right  *Node
	parent *Node
}

type SplayTree struct {
	root *Node
}

// RotateLeft thực hiện xoay trái tại nút x
func (t *SplayTree) RotateLeft(x *Node) {
	y := x.right
	x.right = y.left
	if y.left != nil {
		y.left.parent = x
	}
	y.parent = x.parent
	if x.parent == nil {
		t.root = y
	} else if x == x.parent.left {
		x.parent.left = y
	} else {
		x.parent.right = y
	}
	y.left = x
	x.parent = y
}

// RotateRight thực hiện xoay phải tại nút x
func (t *SplayTree) RotateRight(x *Node) {
	y := x.left
	x.left = y.right
	if y.right != nil {
		y.right.parent = x
	}
	y.parent = x.parent
	if x.parent == nil {
		t.root = y
	} else if x == x.parent.right {
		x.parent.right = y
	} else {
		x.parent.left = y
	}
	y.right = x
	x.parent = y
}

// Splay thực hiện thao tác Splay tại nút x
func (t *SplayTree) Splay(x *Node) {
	for x.parent != nil {
		if x.parent.parent == nil {
			if x == x.parent.left {
				t.RotateRight(x.parent)
			} else {
				t.RotateLeft(x.parent)
			}
		} else if x == x.parent.left && x.parent == x.parent.parent.left {
			t.RotateRight(x.parent.parent)
			t.RotateRight(x.parent)
		} else if x == x.parent.right && x.parent == x.parent.parent.right {
			t.RotateLeft(x.parent.parent)
			t.RotateLeft(x.parent)
		} else if x == x.parent.right && x.parent == x.parent.parent.left {
			t.RotateLeft(x.parent)
			t.RotateRight(x.parent)
		} else {
			t.RotateRight(x.parent)
			t.RotateLeft(x.parent)
		}
	}
}

// Insert chèn một khóa mới vào Splay Tree
func (t *SplayTree) Insert(key int) {
	if t.root == nil {
		t.root = &Node{key: key}
		return
	}

	var prev *Node
	curr := t.root
	for curr != nil {
		prev = curr
		if key < curr.key {
			curr = curr.left
		} else if key > curr.key {
			curr = curr.right
		} else {
			return // Khóa đã tồn tại trong cây
		}
	}

	newNode := &Node{key: key, parent: prev}
	if key < prev.key {
		prev.left = newNode
	} else {
		prev.right = newNode
	}

	t.Splay(newNode)
}

// Search tìm kiếm một khóa trong Splay Tree và splay nút chứa khóa lên trên
func (t *SplayTree) Search(key int) *Node {
	curr := t.root
	for curr != nil {
		if key < curr.key {
			curr = curr.left
		} else if key > curr.key {
			curr = curr.right
		} else {
			t.Splay(curr)
			return curr
		}
	}
	return nil
}

// InOrderTraversal thực hiện duyệt cây theo thứ tự In-order
func InOrderTraversal(root *Node) {
	if root != nil {
		InOrderTraversal(root.left)
		fmt.Print(root.key, " ")
		InOrderTraversal(root.right)
	}
}

func main() {
	st := SplayTree{}

	// Chèn các khóa vào Splay Tree
	keys := []int{10, 5, 15, 3, 7, 12, 17}
	for _, key := range keys {
		st.Insert(key)
	}

	fmt.Println("Splay Tree sau khi chèn các khóa:")
	InOrderTraversal(st.root)
	fmt.Println()

	// Tìm kiếm và splay nút chứa khóa 7
	foundNode := st.Search(7)
	if foundNode != nil {
		fmt.Println("Khóa 7 được tìm thấy và splayed lên trên:")
		InOrderTraversal(st.root)
		fmt.Println()
	} else {
		fmt.Println("Khóa 7 không tồn tại trong Splay Tree")
	}
}
