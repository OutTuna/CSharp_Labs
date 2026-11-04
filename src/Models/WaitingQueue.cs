namespace ClinicApp.Models;

public class WaitingQueue<T> {
    private readonly Queue<T> _items = new Queue<T>();

    public int Count => _items.Count;
    public bool IsEmpty => Count == 0;

    public void Enqueue(T item) {
        _items.Enqueue(item);
    }

    public T Dequeue() {
        if (IsEmpty) {
            throw new InvalidOperationException("Черга порожня.");
        }

        return _items.Dequeue();
    }

    public T Peek() {
        if (IsEmpty) {
            throw new InvalidOperationException("Черга порожня.");
        }

        return _items.Peek();
    }

    public T[] ToArray() {
        return _items.ToArray();
    }
}
