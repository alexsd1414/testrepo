class Node {
  int value;
  int both; // XOR của địa chỉ của node trước và sau

  Node(this.value, this.both);
}

class XORLinkedList {
  Node? head;
  Node? tail;

  XORLinkedList();

  void add(int value) {
    Node newNode = Node(value, 0);
    if (head == null) {
      head = newNode;
      tail = newNode;
    } else {
      newNode.both = getAddress(tail);
      tail!.both ^= getAddress(newNode);
      tail = newNode;
    }
  }

  int getAddress(Node? node) {
    return node.hashCode;
  }

  Node? dereference(int address) {
    return address == getAddress(head) ? head : address == getAddress(tail) ? tail : null;
  }

  Node? getNext(Node? node, Node? prev) {
    return dereference(node!.both ^ getAddress(prev));
  }

  void traverse() {
    Node? prev = null;
    Node? curr = head;

    while (curr != null) {
      print(curr.value);

      Node? next = getNext(curr, prev);
      prev = curr;
      curr = next;
    }
  }
}

void main() {
  XORLinkedList list = XORLinkedList();
  list.add(1);
  list.add(2);
  list.add(3);
  list.add(4);

  list.traverse();
}
