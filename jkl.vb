Imports System

' Định nghĩa lớp Node cho AVL Tree
Public Class AVLNode
    Public Key As Integer
    Public Value As String
    Public Height As Integer
    Public Left As AVLNode
    Public Right As AVLNode

    Public Sub New(key As Integer, value As String)
        Me.Key = key
        Me.Value = value
        Me.Height = 1
        Me.Left = Nothing
        Me.Right = Nothing
    End Sub
End Class

' Định nghĩa lớp AVL Tree
Public Class AVLTree
    Private root As AVLNode

    Public Sub New()
        Me.root = Nothing
    End Sub

    ' Lấy chiều cao của một nút
    Private Function GetHeight(node As AVLNode) As Integer
        If node Is Nothing Then
            Return 0
        Else
            Return node.Height
        End If
    End Function

    ' Lấy hiệu cân bằng của một nút
    Private Function GetBalance(node As AVLNode) As Integer
        If node Is Nothing Then
            Return 0
        Else
            Return GetHeight(node.Left) - GetHeight(node.Right)
        End If
    End Function

    ' Cập nhật chiều cao của một nút
    Private Sub UpdateHeight(node As AVLNode)
        If node IsNot Nothing Then
            node.Height = Math.Max(GetHeight(node.Left), GetHeight(node.Right)) + 1
        End If
    End Sub

    ' Xoay phải tại nút x
    Private Function RotateRight(y As AVLNode) As AVLNode
        Dim x As AVLNode = y.Left
        Dim T2 As AVLNode = x.Right

        ' Thực hiện xoay
        x.Right = y
        y.Left = T2

        ' Cập nhật chiều cao
        UpdateHeight(y)
        UpdateHeight(x)

        Return x
    End Function

    ' Xoay trái tại nút y
    Private Function RotateLeft(x As AVLNode) As AVLNode
        Dim y As AVLNode = x.Right
        Dim T2 As AVLNode = y.Left

        ' Thực hiện xoay
        y.Left = x
        x.Right = T2

        ' Cập nhật chiều cao
        UpdateHeight(x)
        UpdateHeight(y)

        Return y
    End Function

    ' Thêm một khóa mới vào cây
    Public Sub Insert(key As Integer, value As String)
        Me.root = InsertRecursive(Me.root, key, value)
    End Sub

    ' Thêm một khóa mới vào cây (hàm đệ quy)
    Private Function InsertRecursive(node As AVLNode, key As Integer, value As String) As AVLNode
        ' Nếu nút là null, thêm nút mới vào
        If node Is Nothing Then
            Return New AVLNode(key, value)
        End If

        ' Nếu khóa nhỏ hơn khóa của nút hiện tại, thêm vào cây con trái
        If key < node.Key Then
            node.Left = InsertRecursive(node.Left, key, value)
        ElseIf key > node.Key Then ' Nếu khóa lớn hơn khóa của nút hiện tại, thêm vào cây con phải
            node.Right = InsertRecursive(node.Right, key, value)
        Else ' Trường hợp khóa trùng lặp, thêm vào cây con phải
            node.Right = InsertRecursive(node.Right, key, value)
        End If

        ' Cập nhật chiều cao của nút hiện tại
        UpdateHeight(node)

        ' Cân bằng lại cây nếu cần
        Return Balance(node)
    End Function

    ' Cân bằng cây AVL
    Private Function Balance(node As AVLNode) As AVLNode
        ' Cập nhật lại chiều cao của nút
        UpdateHeight(node)

        ' Lấy hiệu cân bằng của nút
        Dim balance As Integer = GetBalance(node)

        ' Trường hợp mất cân bằng, xử lý tùy thuộc vào trường hợp con trái, con phải của nút
        If balance > 1 Then
            If GetBalance(node.Left) >= 0 Then
                ' Trường hợp Left-Left
                Return RotateRight(node)
            Else
                ' Trường hợp Left-Right
                node.Left = RotateLeft(node.Left)
                Return RotateRight(node)
            End If
        ElseIf balance < -1 Then
            If GetBalance(node.Right) <= 0 Then
                ' Trường hợp Right-Right
                Return RotateLeft(node)
            Else
                ' Trường hợp Right-Left
                node.Right = RotateRight(node.Right)
                Return RotateLeft(node)
            End If
        End If

        ' Trường hợp cân bằng, không cần thay đổi
        Return node
    End Function

    ' Inorder traversal để in ra cây theo thứ tự tăng dần của khóa
    Public Sub InOrderTraversal(node As AVLNode)
        If node IsNot Nothing Then
            InOrderTraversal(node.Left)
            Console.WriteLine("Key: " & node.Key & ", Value: " & node.Value)
            InOrderTraversal(node.Right)
        End If
    End Sub
End Class

' Lớp chính để thử nghiệm AVL Tree với khóa trùng lặp
Public Class MainClass
    Public Shared Sub Main(args As String())
        Dim tree As New AVLTree()

        ' Thêm các khóa mới vào cây
        tree.Insert(10, "A")
        tree.Insert(20, "B")
        tree.Insert(30, "C")
        tree.Insert(20, "D") ' Khóa trùng lặp
        tree.Insert(40, "E")

        ' In ra cây sau khi thêm
        Console.WriteLine("AVL Tree sau khi thêm các khóa:")
        tree.InOrderTraversal(tree.root)
    End Sub
End Class
