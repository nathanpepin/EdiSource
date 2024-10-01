namespace EdiSource.Domain.Elements;

public partial class Element
{
    public override bool Equals(object? obj)
    {
        if (obj is not IList<string> compositeElements) return false;
        if (compositeElements.Count != _compositeElements.Count) return false;

        foreach (var (e, x) in compositeElements.Zip(_compositeElements, (s, s1) => (s, s1)))
        {
            if (!e.Equals(x)) return false;
        }

        return true;
    }

    public bool Equals(Element other)
    {
        return _compositeElements.Equals(other._compositeElements);
    }

    public static bool operator ==(Element left, Element right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Element element1, Element element2)
    {
        return !(element1 == element2);
    }

    public override int GetHashCode()
    {
        return _compositeElements.GetHashCode();
    }
}