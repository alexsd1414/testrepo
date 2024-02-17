package main

import "fmt"

// Định nghĩa cấu trúc Node đại diện cho mỗi phần tử trong danh sách liên kết
type Node struct {
	data int
	next *Node
}

// Định nghĩa cấu trúc LinkedList đại diện cho danh sách liên kết
type LinkedList struct {
	head *Node
}

// Phương thức thêm một phần tử vào cuối danh sách liên kết
func (ll *LinkedList) append(data int) {
	newNode := &Node{data: data}

	if ll.head == nil {
		ll.head = newNode
		return
	}

	lastNode := ll.head
	for lastNode.next != nil {
		lastNode = lastNode.next
	}
	lastNode.next = newNode
}

// Phương thức xoá một phần tử khỏi danh sách liên kết
func (ll *LinkedList) delete(data int) {
	if ll.head == nil {
		return
	}

	if ll.head.data == data {
		ll.head = ll.head.next
		return
	}

	prevNode := ll.head
	for prevNode.next != nil {
		if prevNode.next.data == data {
			prevNode.next = prevNode.next.next
			return
		}
		prevNode = prevNode.next
	}
}

// Phương thức hiển thị tất cả các phần tử trong danh sách liên kết
func (ll *LinkedList) display() {
	currentNode := ll.head
	for currentNode != nil {
		fmt.Printf("%d -> ", currentNode.data)
		currentNode = currentNode.next
	}
	fmt.Println("nil")
}

func main() {
	// Khởi tạo một danh sách liên kết mới
	list := LinkedList{}

	// Thêm các phần tử vào danh sách liên kết
	list.append(1)
	list.append(2)
	list.append(3)
	list.append(4)
	list.append(5)

	fmt.Println("Danh sách liên kết sau khi thêm:")
	list.display()

	// Xoá một phần tử khỏi danh sách liên kết
	list.delete(3)

	fmt.Println("Danh sách liên kết sau khi xoá phần tử có giá trị 3:")
	list.display()
}
