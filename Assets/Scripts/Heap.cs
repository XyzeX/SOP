using System;
using UnityEngine;

// Heap class works like a heap, where each parent has 0, 1 or 2 children
// The heap should always be sorted
// This means the first item will always be the most efficient vertex to search through
public class Heap<T> where T : IHeapItem<T>
{
    // Array of items in heap
    T[] items;
    int currentItemCount;

    // Constructor
    public Heap(int capacity)
    {
        // Create array of given max capacity
        items = new T[capacity];
    }

    // Count is a public getter for currentItemCount
    public int Count
    {
        get
        {
            return currentItemCount;
        }
    }

    // UpdateItem updates an items placement in the heap
    public void UpdateItem(T item)
    {
        SortUp(item);
    }

    // Add adds an item into the heap at the correct location
    public void Add(T item)
    {
        // Set new items index at the end
        item.HeapIndex = currentItemCount;

        // Put item into list
        items[currentItemCount] = item;

        // Sort if from the bottom of the heap upwards
        SortUp(item);

        currentItemCount++;
    }

    // RemoveFirstItem takes the item at the top of the heap, removes it and returns it
    public T RemoveFirstItem()
    {
        // Get the first item
        T firstItem = items[0];
        currentItemCount--;

        // Replace the first item with the last item
        items[0] = items[currentItemCount];

        // Update the previously last items index
        items[0].HeapIndex = 0;

        // Sort item from the top of the heap and downwards
        SortDown(items[0]);

        return firstItem;
    }

    // Contains checks if heap contains a given item
    public bool Contains(T item)
    {
        return Equals(items[item.HeapIndex], item);
    }

    // SortUp sorts an item in the heap from the bottom up
    private void SortUp(T item)
    {
        while (true)
        {
            // Get the item index just above it in the heap
            int parentIndex = (item.HeapIndex - 1) / 2;

            // Get parent item
            T parentItem = items[parentIndex];

            // Check if items should be swapped
            if (item.CompareTo(parentItem) > 0)
            {
                Swap(item, parentItem);
            }
            // If they should not be swapped, sorting is done -> break
            else
            {
                break;
            }
        }
    }

    // SortDown sorts an item in the heap from the top down
    private void SortDown(T item)
    {
        while (true)
        {
            // Since we are going from the top down, we need to compare to both children
            int childIndexLeft = item.HeapIndex * 2 + 1;
            int childIndexRight = item.HeapIndex * 2 + 2;

            // Check if child even exists
            if (childIndexLeft < currentItemCount)
            {
                // We start with the left child since:
                // The left child can exist without the right child,
                // but the right child cannot exist without the left child
                int swapIndex = childIndexLeft;

                // If the right child also exists, then which child to swap with
                if (childIndexRight < currentItemCount)
                {
                    // Change swapIndex to right child if left has lower priority
                    if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0)
                    {
                        swapIndex = childIndexRight;
                    }
                }

                // Now swapIndex has been found, check if swap is necessary
                if (item.CompareTo(items[swapIndex]) < 0)
                {
                    Swap(item, items[swapIndex]);
                }
                // If no swap was necessary, we are done sorting
                else
                {
                    return;
                }
            }
            // If the left child doesn't exist, the right child won't either
            // Therfore we are done sorting
            else
            {
                return;
            }
        }
    }

    // Swap swaps to item positions in the heap
    private void Swap(T item1, T item2)
    {
        // Swap the items in the heap
        items[item1.HeapIndex] = item2;
        items[item2.HeapIndex] = item1;

        // Keep a temporary index for one of the items
        int tempItem1HeapIndex = item1.HeapIndex;

        // Swap the item's heap indexes
        item1.HeapIndex = item2.HeapIndex;
        item2.HeapIndex = tempItem1HeapIndex;
    }
}

// This interface is implemented by Vertex
public interface IHeapItem<T> : IComparable<T>
{
    // HeapIndex can be get and set
    int HeapIndex
    {
        get;
        set;
    }
}
