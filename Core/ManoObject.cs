using System.Reflection;
using System.Text.Json.Serialization;

namespace mano;

public abstract class ManoObject
{
    private readonly List<ManoObject> _children = new();
    private readonly List<ManoTrait> _traits = new();

    public string Id { get; set; } = "";
    public string Name { get; set; } = "";

    [JsonIgnore] public string Type => GetType().Name;
    [JsonIgnore] public ManoObject? Parent { get; private set; }
    [JsonIgnore] public IReadOnlyList<ManoObject> Children => _children;
    [JsonIgnore] public IReadOnlyList<ManoTrait> Traits => _traits;

    public virtual void Add(ManoObject child)
    {
        if (ReferenceEquals(child, this))
            throw new InvalidOperationException("Cannot add self as a child.");
        if (child.Parent != null)
            throw new InvalidOperationException($"'{child.Id}' already has a parent.");
        if (IsAncestorOf(child))
            throw new InvalidOperationException($"Cannot add ancestor '{child.Id}' (cycle).");

        var incoming = new HashSet<string>();
        CollectIds(child, incoming);
        foreach (var id in incoming)
            if (FindInternal(id) != null)
                throw new InvalidOperationException($"Id '{id}' already exists in this tree.");

        child.Parent = this;
        _children.Add(child);
    }

    public virtual void Rem(ManoObject child)
    {
        if (!ReferenceEquals(child.Parent, this)) return;
        child.Parent = null;
        _children.Remove(child);
    }

    public T? Find<T>(string id) where T : ManoObject => FindInternal(id) as T;

    private ManoObject? FindInternal(string id)
    {
        if (Id == id) return this;
        foreach (var c in _children)
        {
            var hit = c.FindInternal(id);
            if (hit != null) return hit;
        }
        return null;
    }

    public IEnumerable<T> Collect<T>() where T : ManoObject
    {
        foreach (var c in _children)
        {
            if (c is T t) yield return t;
            foreach (var d in c.Collect<T>()) yield return d;
        }
    }

    public IEnumerable<T> Collect<T>(Func<T, bool> pred) where T : ManoObject
        => Collect<T>().Where(pred);

    public IEnumerable<T> Direct<T>() where T : ManoObject
    {
        foreach (var c in _children)
            if (c is T t) yield return t;
    }

    public void Attach(ManoTrait trait)
    {
        var target = ManoTrait.GetTargetType(trait.GetType());
        if (target != null && !target.IsInstanceOfType(this))
            throw new InvalidOperationException(
                $"Trait '{trait.GetType().Name}' requires '{target.Name}', " +
                $"but was attached to '{GetType().Name}'.");
        _traits.Add(trait);
    }

    public void Detach(ManoTrait trait) => _traits.Remove(trait);

    public async Task Exec(string eventId)
    {
        foreach (var t in _traits)
        {
            var m = t.GetType().GetMethod(
                eventId,
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
            if (m == null) continue;

            var result = m.Invoke(t, new object[] { this });
            switch (result)
            {
                case Task task: await task; break;
                case ValueTask vt: await vt; break;
            }
        }
    }

    public async Task Down(string eventId)
    {
        await Exec(eventId);
        foreach (var c in _children)
            await c.Down(eventId);
    }

    public async Task Up(string eventId)
    {
        if (Parent != null) await Parent.Exec(eventId);
    }

    private bool IsAncestorOf(ManoObject other)
    {
        var p = other.Parent;
        while (p != null)
        {
            if (ReferenceEquals(p, this)) return true;
            p = p.Parent;
        }
        return false;
    }

    private static void CollectIds(ManoObject node, HashSet<string> set)
    {
        if (!set.Add(node.Id))
            throw new InvalidOperationException($"Duplicate Id '{node.Id}' in subtree.");
        foreach (var c in node._children) CollectIds(c, set);
    }
}