using System; // Don't use anything else than System and only use C-core functionality; read the specs!

/// <summary>
/// Implement a binary search tree 
/// 
/// Notes
/// 1) Don't rename any of the method names in this file or change their arguments or return types or their order in the file.
/// 2) If you want to add methods do this in the space indicated at the top of the Program.
/// 3) You can add fields to the structures Tree, Node, DataEntry, if you find this necessary or useful.
/// 4) Some of the method stubs have return statements that you may need to change (the code wouldn't run without return statements).
/// 
///    You can ignore most warnings - many of them result from requirements of Object-Orientated Programming or other constraints
///    unimportant for COMP1003.
///    
/// </summary>
/// <summary>
/// Declare what sort of data we store in the tree.
/// 
/// We use simple integers for convenience, but this could be anything sortable in general.
/// </summary>
class DataEntry
{
    public int data;
}


/// <summary>
/// A single node in the tree;
/// </summary>
class Node
{
    public DataEntry data;
    public Node right;
    public Node left;
    public int height;
}


/// <summary>
/// The top-level tree structure
/// </summary>
class Tree
{
    public Node root;
}


class Program
{
    /// THIS LINE: If you want to add methods add them between THIS LINE and THAT LINE
    /// Your methods go here  .... (and nowhere else)

    //  ========================================= Tree Functions =========================================
    /// <summary>
    /// Returns the number of elements in a tree with root "node".
    /// </summary>
    static int SizeNode(Node node)
    {
        if (node == null)
        {
            return 0;
        }

        // Recursively count the number of elements in the left and right subtrees
        return SizeNode(node.left) + 1 + SizeNode(node.right);
    }

    /// <summary>
    /// Returns the parent of a Node in a tree.
    /// </summary>
    /// <param name="root">The root of the tree</param>
    /// <param name="node">The Node</param>
    /// <returns>The parent of the Node in the tree, or null if the Node has no parent.</returns>
    static Node ParentNode(Node root, Node node)
    {
        // If the root is null, the node has no parent
        if (root == null)
        {
            return null;
        }

        // If the left or right child of the root is the node, then the root is the parent
        if (root.left == node || root.right == node)
        {
            return root;
        }

        // Recursively search for the parent in the left and right subtrees
        Node parent = ParentNode(root.left, node);
        if (parent != null)
        {
            return parent;
        }

        return ParentNode(root.right, node);
    }

    /// <summary>
    /// Deletes the Node with the smallest value in a tree.
    /// </summary>
    static Node DeleteMinNode(Node root)
    {
        if (root.left == null)
        {
            return root.right;
        }

        root.left = DeleteMinNode(root.left);
        return root;
    }

    /// <summary>
    /// Deletes the given Node from the tree.
    /// </summary>
    static void DeleteItemNode(ref Node root, Node item)
    {
        if (root == null)
        {
            return;
        }

        // If the root is the item to delete
        if (IsEqual(root, item))
        {
            if (root.left == null)
            {
                root = root.right;
            }
            else if (root.right == null)
            {
                root = root.left;
            }
            else
            {
                // Find the maximum value in the left subtree
                Node max = FindMax(root.left);
                root.data = max.data;

                // Delete the maximum value from the left subtree
                DeleteItemNode(ref root.left, max);
            }
        }
        else if (IsSmaller(item, root)) // If the item is smaller than the root
        {
            // Recursively delete the item from the left subtree
            DeleteItemNode(ref root.left, item);
        }
        else
        {
            // Recursively delete the item from the right subtree
            DeleteItemNode(ref root.right, item);
        }


        // ====== AVL changes ======

        // If the tree had only one node then return
        if (root == null)
        {
            return;
        }


        // Update height of the current node
        root.height = Math.Max(Height(root.left), Height(root.right)) + 1;

        // Get the balance factor
        int balance = GetBalance(root);

        // If this node becomes unbalanced, then there are 4 cases

        // Left Left Case
        if (balance > 1 && GetBalance(root.left) >= 0)
            root = RightRotate(root);

        // Left Right Case
        if (balance > 1 && GetBalance(root.left) < 0)
        {
            root.left = LeftRotate(root.left);
            root = RightRotate(root);
        }

        // Right Right Case
        if (balance < -1 && GetBalance(root.right) <= 0)
            root = LeftRotate(root);

        // Right Left Case
        if (balance < -1 && GetBalance(root.right) > 0)
        {
            root.right = RightRotate(root.right);
            root = LeftRotate(root);
        }
    }


    //  ========================================= Set Functions =========================================
    /// <summary>
    /// Inserts the values of one tree into another tree.
    /// </summary>
    /// <param name="node"></param>
    /// <param name="tree"></param>
    static void InorderInsert(Node node, Tree tree)
    {
        if (node == null)
        {
            return;
        }

        // Traverse the left subtree
        InorderInsert(node.left, tree);

        Node newNode = new Node();
        newNode.data = node.data;
        InsertTree(tree, newNode);

        // Traverse the right subtree
        InorderInsert(node.right, tree);
    }

    /// <summary>
    /// Inserts the values of one tree into another tree, but only if the value is not already present in the tree.
    /// </summary>
    /// <param name="node"></param>
    /// <param name="tree"></param>
    static void InorderInsertUnique(Node node, Tree tree)
    {
        if (node == null)
        {
            return;
        }

        // Traverse the left subtree
        InorderInsertUnique(node.left, tree);

        // If the value is not already in the tree, insert it
        if (!SearchTree(tree.root, node.data))
        {
            Node newNode = new Node();
            newNode.data = node.data;
            InsertTree(tree, newNode);
        }

        // Traverse the right subtree
        InorderInsertUnique(node.right, tree);
    }

    /// <summary>
    /// Inserts the values that are in both trees into a new tree.
    /// </summary>
    /// <param name="node"></param>
    /// <param name="tree2"></param>
    /// <param name="intersectionTree"></param>
    static void InorderIntersection(Node node, Tree tree2, Tree intersectionTree)
    {
        if (node == null)
        {
            return;
        }

        // Traverse the left subtree
        InorderIntersection(node.left, tree2, intersectionTree);

        // If the value is in both trees, insert it into the intersection tree
        if (SearchTree(tree2.root, node.data))
        {
            Node newNode = new Node();
            newNode.data = node.data;
            InsertTree(intersectionTree, newNode);
        }

        // Traverse the right subtree
        InorderIntersection(node.right, tree2, intersectionTree);
    }

    /// <summary>
    /// Deletes the values that are in the second tree from the first tree.
    /// </summary>
    /// <param name="node"></param>
    /// <param name="tree"></param>
    static void InorderDeleteIfExists(Node node, Tree tree)
    {
        if (node == null)
        {
            return;
        }

        // Traverse the left subtree
        InorderDeleteIfExists(node.left, tree);

        // If the value is in the tree, delete it
        if (SearchTree(tree.root, node.data))
        {
            Node nodeToDelete = new Node();
            nodeToDelete.data = node.data;
            DeleteItem(tree, node);
        }

        // Traverse the right subtree
        InorderDeleteIfExists(node.right, tree);
    }

    //  ========================================= AVL Functions =========================================

    static int Height(Node N)
    {
        if (N == null)
            return 0;
        return N.height;
    }


    static int GetBalance(Node N)
    {
        if (N == null)
            return 0;
        return Height(N.left) - Height(N.right);
    }

    static Node RightRotate(Node y)
    {
        Node x = y.left;
        Node T2 = x.right;

        // Perform rotation
        x.right = y;
        y.left = T2;

        // Update heights
        y.height = Math.Max(Height(y.left), Height(y.right)) + 1;
        x.height = Math.Max(Height(x.left), Height(x.right)) + 1;

        // Return new root
        return x;
    }

    static Node LeftRotate(Node x)
    {
        Node y = x.right;
        Node T2 = y.left;

        // Perform rotation
        y.left = x;
        x.right = T2;

        // Update heights
        x.height = Math.Max(Height(x.left), Height(x.right)) + 1;
        y.height = Math.Max(Height(y.left), Height(y.right)) + 1;

        // Return new root
        return y;
    }


    /// THAT LINE: If you want to add methods add them between THIS LINE and THAT LINE
    /// <summary>
    /// Recursively traverse a tree depth-first printing data in in-fix order.
    /// 
    /// Note that we expect the root Node as argument, not a Tree structure.
    /// Otherwise we would need an auxiliary function as we do recursion over Nodes.
    /// 
    /// In fact, the method below can print any non-empty sub-tree.
    /// 
    /// </summary>
    /// <param name="subtree">The *root node* of the tree to traverse and print</param>
    static void PrintTree(Node tree)
    {
        if (tree.left != null)
            PrintTree(tree.left);

        Console.Write(tree.data.data + "  ");

        if (tree.right != null)
            PrintTree(tree.right);
    }


    /// <summary>
    /// Compare whether the data in one Node is smaller than data in another Node. 
    /// 
    /// The data held in Nodes could be different from integers, but it must be sortable.
    /// This function/method defines when the data in Node item1 is smaller than in item2.
    /// As we assume Integers for convenience, the comparison is just the usual "smaller than".
    /// </summary>
    /// <param name="item1">First Node</param>
    /// <param name="item2">Second Node</param>
    /// <returns>True if the data in item1 is smaller than the data in item2, and false otherwise.</returns>
    static bool IsSmaller(Node item1, Node item2)
    {
        return item1.data.data < item2.data.data;
    }


    /// <summary>
    /// Predicate that checks if two Nodes hold the same value. 
    /// 
    /// As we assume Integers for convenience, the comparison is just the usual "equality" on integers.
    /// Equality could be more complicated for other sorts of data.
    /// </summary>
    /// <param name="item1">First Node</param>
    /// <param name="item2">Second Node</param>
    /// <returns>True if two Nodes have the same value, false otherwise.</returns>
    static bool IsEqual(Node item1, Node item2)
    {
        // if either item is null, they are not equal
        if (item1 == null || item2 == null)
        {
            return false;
        }

        return item1.data.data == item2.data.data;
    }


    /// <summary>
    /// Insert a Node into a Tree
    /// 
    /// Note that the root node has to be provided, not the Tree reference, because we do recursion over the Nodes.
    /// The function makes use of IsSmaller and would work for other sorts of data than Integers.
    /// </summary>
    /// <param name="tree">The *root node* of the tree</param>
    /// <param name="item">The item to insert</param>
    static void InsertItem(ref Node tree, Node item)
    {
        if (tree == null) // if tree Node is empty, make item the tree's Node
        {
            tree = item;
            tree.height = 1;
            return;
        }

        if (IsSmaller(item, tree)) // if item data is smaller than tree's data
        {
            InsertItem(ref tree.left, item); //     recursively insert into the left subtree
        }
        else if (IsSmaller(tree, item)) // if item data is larger than tree's data
        {
            InsertItem(ref tree.right, item); //     recursively insert into the right subtree
        }

        // otherwise the item data is already in the tree and we discard it 

        /* 2. Update height of this ancestor node */
        tree.height = 1 + Math.Max(Height(tree.left), Height(tree.right));

        /* 3. Get the balance factor of this ancestor node to check whether
           this node became unbalanced */
        int balance = GetBalance(tree);

        // If this node becomes unbalanced, then there are 4 cases

        // Left Left Case
        if (balance > 1 && IsSmaller(item, tree.left))
            tree = RightRotate(tree);

        // Right Right Case
        if (balance < -1 && IsSmaller(tree.right, item))
            tree = LeftRotate(tree);

        // Left Right Case
        if (balance > 1 && IsSmaller(tree.left, item))
        {
            tree.left = LeftRotate(tree.left);
            tree = RightRotate(tree);
        }

        // Right Left Case
        if (balance < -1 && IsSmaller(item, tree.right))
        {
            tree.right = RightRotate(tree.right);
            tree = LeftRotate(tree);
        }
    }


    /// <summary>
    /// Insert a Node into a Tree
    /// 
    /// This is an auxiliary function that expects a Tree structure, in contrast to the previous method. 
    /// It always inserts on the toplevel and is not recursive. It's just a wrapper.
    /// </summary>
    /// <param name="tree">The Tree (not a Node as in InsertItem())</param>
    /// <param name="item">The Node to insert</param>
    static void InsertTree(Tree tree, Node item)
    {
        InsertItem(ref tree.root, item);
    }


    /// <summary>
    /// Find a value in a tree.
    /// 
    /// This requires the IsEqual() predicate defined above for general data.
    /// </summary>
    /// <param name="tree">The root node of the Tree.</param>
    /// <param name="value">The Data to find</param>
    /// <returns>True if the value is found and false otherwise.</returns>
    static bool SearchTree(Node tree, DataEntry value)
    {
        // we have to search for the value in the tree with binary search
        // if the tree is empty, the value is not in the tree
        if (tree == null)
        {
            return false;
        }

        // if the value is smaller than the current node's value, search in the left subtree
        if (value.data < tree.data.data)
        {
            return SearchTree(tree.left, value);
        }

        // if the value is larger than the current node's value, search in the right subtree
        if (value.data > tree.data.data)
        {
            return SearchTree(tree.right, value);
        }

        // if the value is equal to the current node's value, the value is in the tree
        return true;
    }


    /// <summary>
    /// Find a Node in a tree
    /// 
    /// This compares Node references not data values.
    /// </summary>
    /// <param name="tree">The root node of the tree.</param>
    /// <param name="item">The Node (reference) to be found.</param>
    /// <returns>True if the Node is found, false otherwise.</returns>
    static bool SearchTreeItem(Node tree, Node item)
    {
        if (tree == null)
        {
            return false;
        }

        if (IsEqual(tree, item))
        {
            return true;
        }

        return SearchTreeItem(tree.left, item) || SearchTreeItem(tree.right, item);
    }


    /// <summary>
    /// Delete a Node from a tree
    /// </summary>
    /// <param name="tree">The root of the tree</param>
    /// <param name="item">The Node to remove</param>
    static void DeleteItem(Tree tree, Node item)
    {
        if (tree.root == null)
        {
            return;
        }

        if (IsEqual(tree.root, item))
        {
            tree.root = null;
            return;
        }

        DeleteItemNode(ref tree.root, item);
    }


    /// <summary>
    /// Returns how many elements are in a Tree
    /// </summary>
    /// <param name="tree">The Tree.</param>
    /// <returns>The number of items in the tree.</returns>
    static int Size(Tree tree)
    {
        return SizeNode(tree.root);
    }


    /// <summary>
    /// Returns the depth of a tree with root "tree"
    /// 
    /// Note that this function should work for any non-empty subtree
    /// </summary>
    /// <param name="tree">The root of the tree</param>
    /// <returns>The depth of the tree.</returns>
    static int Depth(Node tree)
    {
        if (tree == null)
        {
            return 0;
        }
        else
        {
            int lDepth = Depth(tree.left);
            int rDepth = Depth(tree.right);
            if (lDepth > rDepth)
            {
                return (lDepth + 1);
            }
            else
            {
                return (rDepth + 1);
            }
        }
    }


    /// <summary>
    /// Find the parent of Node node in Tree tree.
    /// </summary>
    /// <param name="tree">The Tree</param>
    /// <param name="node">The Node</param>
    /// <returns>The parent of node in the tree, or null if node has no parent.</returns>
    static Node Parent(Tree tree, Node node)
    {
        // since this method only accept Tree, if I wanted to call this method recursively,
        // I would need to wrap the Node in a Tree first then call the method. since it's not an efficient way to do it,
        // I will create and use the ParentNode method instead.
        return ParentNode(tree.root, node);
    }


    /// <summary>
    /// Find the Node with maximum value in a (sub-)tree, given the IsSmaller predicate.
    /// </summary>
    /// <param name="tree">The root node of the tree to search.</param>
    /// <returns>The Node that contains the largest value in the sub-tree provided.</returns>
    static Node FindMax(Node tree)
    {
        // The maximum value in a binary search tree is on the rightmost leaf,
        // so we traverse the right subtree until we reach the last node.
        Node current = tree;
        while (current.right != null)
        {
            current = current.right;
        }

        return current;
    }


    /// <summary>
    /// Delete the Node with the smallest value from the Tree. 
    /// </summary>
    /// <param name="tree">The Tree to process.</param>
    static void DeleteMin(Tree tree)
    {
        tree.root = DeleteMinNode(tree.root);
    }


    /// SET FUNCTIONS 
    /// <summary>
    /// Merge the items of one tree with another one.
    /// Note that duplicate data entries are prohibited.
    /// </summary>
    /// <param name="tree1">A tree</param>
    /// <param name="tree2">Another tree</param>
    /// <returns>A new tree with all the values from tree1 and tree2.</returns>
    static Tree Union(Tree tree1, Tree tree2)
    {
        Tree newTree = new Tree();

        // Traverse tree1 and insert all values of tree1 to newTree
        InorderInsert(tree1.root, newTree);

        // Traverse tree2 and insert all values of tree2 to newTree
        // If a value is already present in newTree, then skip the insertion
        InorderInsertUnique(tree2.root, newTree);

        return newTree;
    }


    /// <summary>
    /// Find all values that are in tree1 AND in tree2 and copy them into a new Tree.
    /// </summary>
    /// <param name="tree1">The first Tree</param>
    /// <param name="tree2">The second Tree</param>
    /// <returns>A new Tree with all values in tree1 and tree2.</returns>
    static Tree Intersection(Tree tree1, Tree tree2)
    {
        // Create a new tree to store the intersection
        Tree intersectionTree = new Tree();

        // Traverse tree1 and if a value is present in tree2, insert it into intersectionTree
        InorderIntersection(tree1.root, tree2, intersectionTree);

        return intersectionTree;
    }


    /// <summary>
    /// Compute the set difference of the values of two Trees (interpreted as Sets).
    /// </summary>
    /// <param name="tree1">Tree one</param>
    /// <param name="tree2">Tree two</param>
    /// <returns>The values of the set difference tree1/tree2 in a new Tree.</returns>
    static Tree Difference(Tree tree1, Node tree2)
    {
        // Create a new tree to store the difference
        Tree differenceTree = new Tree();

        // Traverse tree1 and insert all values of tree1 to differenceTree
        InorderInsert(tree1.root, differenceTree);

        // Traverse differenceTree and if a value is present in tree2, delete it
        InorderDeleteIfExists(tree2, differenceTree);

        return differenceTree;
    }


    /// <summary>
    /// Compute the symmetric difference of the values of two Trees (interpreted as Sets).
    /// </summary>
    /// <param name="tree1">Tree one</param>
    /// <param name="tree2">Tree two</param>
    /// <returns>The values of the symmetric difference tree1/tree2 in a new Tree.</returns>
    static Tree SymmetricDifference(Node tree1, Tree tree2)
    {
        // create a new tree with the root of tree1
        Tree treeOneWithRoot = new Tree();
        treeOneWithRoot.root = tree1;

        // Compute the union of tree1 and tree2
        Tree unionTree = Union(treeOneWithRoot, tree2);

        // Compute the intersection of tree1 and tree2
        Tree intersectionTree = Intersection(treeOneWithRoot, tree2);

        // Compute the difference between the union and the intersection
        Tree symmetricDifferenceTree = Difference(unionTree, intersectionTree.root);

        return symmetricDifferenceTree;
    }


    /*
     *  TESTING
     */


    /// <summary>
    /// Testing of the Tree methods that does some reasonable checks.
    /// It does not have to be exhaustive but sufficient to suggest the code is correct.
    /// </summary>
    static void TreeTests() // some tests
    {
        Console.WriteLine("\n===== Tree tests =======================================\n");

        Tree tree = new Tree();
        Random r = new Random();
        DataEntry data;

        // Test InsertTree
        /*
         * Since we are going to use data and nodes to test the other functions,
         * we will need them to be declared manually instead of randomly.
         * for example, to test parent, we need to know the parent of a node. or FindMax, we need to know the max value.
         */
        Console.WriteLine("### Test InsertTree:");
        Node node1 = new Node { data = new DataEntry { data = 5 } };
        Node node2 = new Node { data = new DataEntry { data = 16 } };
        Node node3 = new Node { data = new DataEntry { data = 7 } };
        Node node4 = new Node { data = new DataEntry { data = 27 } };
        Node node5 = new Node { data = new DataEntry { data = 44 } };
        Node node6 = new Node { data = new DataEntry { data = 33 } };
        Node node7 = new Node { data = new DataEntry { data = 3 } };
        Node node8 = new Node { data = new DataEntry { data = 26 } };
        Node node9 = new Node { data = new DataEntry { data = 11 } };
        Node node10 = new Node { data = new DataEntry { data = 39 } };

        InsertTree(tree, node1);
        InsertTree(tree, node2);
        InsertTree(tree, node3);
        InsertTree(tree, node4);
        InsertTree(tree, node5);
        InsertTree(tree, node6);
        InsertTree(tree, node7);
        InsertTree(tree, node8);
        InsertTree(tree, node9);
        InsertTree(tree, node10);

        // print out the new tree
        Console.WriteLine("\nTree: ");
        PrintTree(tree.root);
        Console.WriteLine("\n");

        // Test Size
        Console.WriteLine("### Testing Size: " + Size(tree)); // Should print 10
        Console.WriteLine();

        // Test Depth
        Console.WriteLine("### Test Depth: " + Depth(tree.root)); // Should print 2
        Console.WriteLine();

        // Test SearchTree
        Console.WriteLine("### Test SearchTree:");
        data = new DataEntry { data = 5 };
        Console.WriteLine("Searching for " + data.data + " in the tree: " +
                          SearchTree(tree.root, data)); // Should print true
        Console.WriteLine();

        data = new DataEntry { data = 50 };
        Console.WriteLine("Searching for " + data.data + " in the tree: " +
                          SearchTree(tree.root, data)); // Should print false
        Console.WriteLine();

        // Test SearchTreeItem
        Console.WriteLine("### Test SearchTreeItem:");
        Console.WriteLine("Search node5: " + SearchTreeItem(tree.root, node5)); // Should print True
        Console.WriteLine("Search new node: " +
                          SearchTreeItem(tree.root,
                              new Node { data = new DataEntry { data = 50 } })); // Should print False
        Console.WriteLine("\n");

        // Test SearchTree
        Console.WriteLine("### Test SearchTree:");
        Console.WriteLine("Search for 5: " +
                          SearchTree(tree.root, new DataEntry { data = 5 })); // Should print True
        Console.WriteLine("Search for 50: " +
                          SearchTree(tree.root, new DataEntry { data = 50 })); // Should print False
        Console.WriteLine("\n");

        // Test Parent
        Console.WriteLine("### Test Parent:");
        Console.WriteLine("Parent of node2: " + Parent(tree, node2).data.data); // Should print 7
        Console.WriteLine("\n");

        // Test isEqual
        Console.WriteLine("### Test IsEqual:");
        Console.WriteLine("IsEqual node1 and node2: " + IsEqual(node1, node2)); // Should print False
        Console.WriteLine("IsEqual node1 and node1: " + IsEqual(node1, node1)); // Should print True
        Console.WriteLine("\n");

        // Test FindMax
        Console.WriteLine("### Test FindMax:");
        Console.WriteLine("Max: " + FindMax(tree.root).data.data); // Should print 44
        Console.WriteLine();

        // Test DeleteMin
        Console.WriteLine("### Test DeleteMin:");
        DeleteMin(tree);
        Console.WriteLine("Size after DeleteMin: " + Size(tree)); // Should print 9
        Console.Write("New tree: ");
        PrintTree(tree.root);
        Console.WriteLine("\n");

        // Test DeleteItem
        DeleteItem(tree, node4);
        Console.WriteLine("Size after DeleteItem: " + Size(tree)); // Should print 8
        Console.Write("New tree: ");
        PrintTree(tree.root);
        Console.WriteLine("\n");
    }


    /// <summary>
    /// Testing of the Set methods that does some reasonable checks.
    /// It does not have to be exhaustive but sufficient to suggest the code is correct.
    /// </summary>
    static void SetTests()
    {
        Console.WriteLine("\n===== Set tests =======================================\n");

        // Create two trees
        Tree tree1 = new Tree();
        Tree tree2 = new Tree();

        // Insert some nodes into tree1
        InsertTree(tree1, new Node { data = new DataEntry { data = 1 } });
        InsertTree(tree1, new Node { data = new DataEntry { data = 45 } });
        InsertTree(tree1, new Node { data = new DataEntry { data = 19 } });
        InsertTree(tree1, new Node { data = new DataEntry { data = 37 } });

        // Insert some nodes into tree2
        InsertTree(tree2, new Node { data = new DataEntry { data = 13 } });
        InsertTree(tree2, new Node { data = new DataEntry { data = 1 } });
        InsertTree(tree2, new Node { data = new DataEntry { data = 8 } });
        InsertTree(tree2, new Node { data = new DataEntry { data = 18 } });

        Console.WriteLine("Tree 1: ");
        PrintTree(tree1.root);
        Console.WriteLine("\n");

        Console.WriteLine("Tree 2: ");
        PrintTree(tree2.root);
        Console.WriteLine("\n");

        // Test Union function
        Console.WriteLine("### Test Union function:");
        Tree unionTree = Union(tree1, tree2);

        // Print the union tree
        Console.Write("Union of Tree 1 and Tree 2: ");
        PrintTree(unionTree.root);
        Console.WriteLine("\n");

        // Test Intersection function
        Console.WriteLine("### Test Intersection function:");

        Tree intersectionTree = Intersection(tree1, tree2);

        // Print the intersection tree
        Console.Write("Intersection of Tree 1 and Tree 2: ");
        PrintTree(intersectionTree.root);
        Console.WriteLine("\n");

        // Test Difference function
        Console.WriteLine("### Test Difference function:");

        Tree differenceTree = Difference(tree1, tree2.root);


        // Print the difference tree
        Console.Write("Difference of Tree 1 and Tree 2: " + Size(differenceTree) + " elements: ");

        // prevent null exception
        if (differenceTree.root != null)
        {
            PrintTree(differenceTree.root);
        }
        else
        {
            Console.WriteLine("Empty");
        }

        Console.WriteLine("\n");


        // Test SymmetricDifference function
        Console.WriteLine("### Test SymmetricDifference function:");

        Tree symmetricDifferenceTree = SymmetricDifference(tree1.root, tree2);

        // Print the symmetric difference tree
        Console.Write("Symmetric difference of Tree 1 and Tree 2: ");
        PrintTree(symmetricDifferenceTree.root);
        Console.WriteLine("\n");
    }


    /// <summary>
    /// The Main entry point into the code. Don't change anythhing here. 
    /// </summary>
    static void Main()
    {
        TreeTests();
        SetTests();
    }
}