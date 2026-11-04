using ClinicApp.Interfaces;

namespace ClinicApp.Managers;

public class Repository<T> where T : IIdentifiable {
    private readonly List<T> _items = new List<T>();

    public int Count => _items.Count;

    public void Add(T item) {
        _items.Add(item);
    }

    public T GetById(int id) {
        for (int i = 0; i < Count; i++) {
            if (_items[i].Id == id) {
                return _items[i];
            }
        }

        return default!;
    }

    public T[] GetAll() {
        return _items.ToArray();
    }

    public bool Remove(int id) {
        for (int i = 0; i < Count; i++) {
            if (_items[i].Id == id) {
                _items.RemoveAt(i);
                return true;
            }
        }

        return false;
    }
}
