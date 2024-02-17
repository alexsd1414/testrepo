use rand::{Rng, thread_rng};
use std::cmp::Ordering;
use std::ptr;

// Định nghĩa cấu trúc nút của Skip List
struct SkipListNode<T> {
    value: T,
    next: Vec<Option<*mut SkipListNode<T>>>,
}

// Định nghĩa cấu trúc Skip List
pub struct SkipList<T> {
    head: SkipListNode<T>,
    max_height: usize,
    length: usize,
}

impl<T: Ord> SkipList<T> {
    // Tạo một Skip List mới
    pub fn new(max_height: usize) -> Self {
        let head = SkipListNode {
            value: Default::default(),
            next: vec![None; max_height],
        };
        SkipList {
            head,
            max_height,
            length: 0,
        }
    }

    // Thêm một giá trị vào Skip List
    pub fn insert(&mut self, value: T) {
        let mut update = vec![ptr::null_mut(); self.max_height];
        let mut node = &mut self.head;

        // Tìm vị trí để chèn giá trị mới
        for i in (0..self.max_height).rev() {
            while let Some(next_node) = node.next[i] {
                let next_value = unsafe { &*next_node }.value;
                match next_value.cmp(&value) {
                    Ordering::Less => node = unsafe { &mut *next_node },
                    Ordering::Equal => return, // Giá trị đã tồn tại
                    Ordering::Greater => break,
                }
            }
            update[i] = node;
        }

        // Tạo một nút mới và cập nhật liên kết
        let height = self.random_height();
        let new_node = Box::new(SkipListNode {
            value,
            next: vec![None; height],
        });

        for i in 0..height {
            unsafe {
                new_node.next[i] = update[i].as_mut().unwrap().next[i];
                update[i].as_mut().unwrap().next[i] = Some(Box::into_raw(new_node) as *mut SkipListNode<T>);
            }
        }

        self.length += 1;
    }

    // Tìm kiếm một giá trị trong Skip List
    pub fn contains(&self, value: &T) -> bool {
        let mut node = &self.head;

        for i in (0..self.max_height).rev() {
            while let Some(next_node) = node.next[i] {
                let next_value = unsafe { &*next_node }.value;
                match next_value.cmp(&value) {
                    Ordering::Less => node = unsafe { &*next_node },
                    Ordering::Equal => return true,
                    Ordering::Greater => break,
                }
            }
        }

        false
    }

    // Xoá một giá trị khỏi Skip List
    pub fn delete(&mut self, value: &T) -> bool {
        let mut update = vec![ptr::null_mut(); self.max_height];
        let mut node = &mut self.head;

        // Tìm vị trí của giá trị cần xoá
        for i in (0..self.max_height).rev() {
            while let Some(next_node) = node.next[i] {
                let next_value = unsafe { &*next_node }.value;
                match next_value.cmp(&value) {
                    Ordering::Less => node = unsafe { &mut *next_node },
                    Ordering::Equal => {
                        update[i] = node;
                        break;
                    }
                    Ordering::Greater => break,
                }
            }
        }

        // Nếu giá trị cần xoá không tồn tại
        if update[0].is_null() || unsafe { &*update[0] }.value != *value {
            return false;
        }

        let deleted_node = unsafe { &mut *update[0] };
        let height = deleted_node.next.len();

        // Cập nhật liên kết
        for i in 0..height {
            if update[i].is_null() {
                self.head.next[i] = deleted_node.next[i];
            } else {
                update[i].as_mut().unwrap().next[i] = deleted_node.next[i];
            }
        }

        self.length -= 1;
        true
    }

    // Random số ngẫu nhiên cho chiều cao của nút
    fn random_height(&self) -> usize {
        let mut rng = thread_rng();
        let mut height = 1;
        while rng.gen::<bool>() && height < self.max_height {
            height += 1;
        }
        height
    }
}

fn main() {
    let mut list = SkipList::new(4); // Chiều cao tối đa của Skip List là 4

    // Thêm các giá trị vào Skip List
    list.insert(10);
    list.insert(20);
    list.insert(30);
    list.insert(40);
    list.insert(50);
    list.insert(60);

    // Kiểm tra xem các giá trị có tồn tại trong Skip List không
    println!("Skip List contains 30: {}", list.contains(&30));
    println!("Skip List contains 35: {}", list.contains(&35));

    // Xoá một giá trị khỏi Skip List và kiểm tra lại
    list.delete(&30);
    println!("After deleting 30, Skip List contains 30: {}", list.contains(&30));
}
