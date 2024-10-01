namespace EdiSource.Domain.Elements;

public partial class Element
{
    public IEnumerator<string> GetEnumerator()
    {
        return _compositeElements.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Add(string item)
    {
        _compositeElements.Add(item);
    }

    public void Clear()
    {
        _compositeElements.Clear();
    }

    public bool Contains(string item)
    {
        return _compositeElements.Contains(item);
    }

    public void CopyTo(string[] array, int arrayIndex)
    {
        _compositeElements.CopyTo(array, arrayIndex);
    }

    public bool Remove(string item)
    {
        return _compositeElements.Remove(item);
    }

    public int Count => _compositeElements.Count;
    public bool IsReadOnly => false;

    public int IndexOf(string item)
    {
        return _compositeElements.IndexOf(item);
    }

    public void Insert(int index, string item)
    {
        _compositeElements.Insert(index, item);
    }

    public void RemoveAt(int index)
    {
        _compositeElements.RemoveAt(index);
    }

    public string this[int index]
    {
        get => _compositeElements[index];
        set => _compositeElements[index] = value;
    }
}