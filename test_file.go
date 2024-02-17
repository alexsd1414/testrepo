package main

import "fmt"

// Hàm sắp xếp mảng sử dụng Bubble Sort
func bubbleSort(arr []int) {
    n := len(arr)
    for i := 0; i < n-1; i++ {
        for j := 0; j < n-i-1; j++ {
            if arr[j] > arr[j+1] {
                // Hoán đổi giá trị nếu phần tử hiện tại lớn hơn phần tử kế tiếp
                arr[j], arr[j+1] = arr[j+1], arr[j]
            }
        }
    }
}

func main() {
    arr := []int{64, 34, 25, 12, 22, 11, 90}

    fmt.Println("Mảng trước khi sắp xếp:", arr)

    // Sắp xếp mảng sử dụng Bubble Sort
    bubbleSort(arr)

    fmt.Println("Mảng sau khi sắp xếp:", arr)
}
