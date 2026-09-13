# ManoEngine

A tree-structured, event-driven framework for building AI-agent simulations.

ManoEngine is the substrate for worlds where many autonomous agents live,
talk, act, and react — without a central scheduler dictating what each one
does. Behavior emerges from events propagating through a tree.

## Concept

ManoEngine separates four things:

| Concept     | Where it lives                       |
|-------------|--------------------------------------|
| Data        | Fields on `ManoObject`               |
| Behavior    | Methods on `ManoTrait`               |
| Structure   | Parent / child edges                 |
| Communication | `Up` / `Down` event propagation    |

Everything else is composition.

## Core rules

1. **Fields hold state. Traits hold behavior.**
   A trait method reads only `self`'s fields. It never receives a payload.
   What it does is determined entirely by which event was called and what
   state `self` is in.

2. **Communication flows along the tree — nowhere else.**
   Siblings cannot talk. To reach a sibling, go `Up` to a common ancestor,
   then `Down`. This makes causality traceable and prevents hidden
   coupling.

3. **The parent is the aggregator.**
   Children write to their own fields. Parents read them via
   `Collect<T>()`. There is no return value from events.

## Quick example

```csharp
using mano;

TraitRegistry.Load();

var world = new WorldObject { Id = "world", Name = "World" };
world.Attach(TraitRegistry.Get<WorldTrait>());

var room = new RoomObject { Id = "room1", Name = "Living Room" };
world.Add(room);

var characters = Mano.Load<CharacterObject>(File.ReadAllText("character.json"));
foreach (var c in characters)
    room.Add(c);

await room.Down("Init");
await room.Up("Finished");
```
## API
`ManoTrait`
```csharp
// structure
void Add(ManoObject child)
void Rem(ManoObject child)

// search / aggregation
T? Find<T>(string id)
IEnumerable<T> Collect<T>()
IEnumerable<T> Collect<T>(Func<T, bool> predicate)
IEnumerable<T> Direct<T>()

// traits
void Attach(ManoTrait trait)
void Detach(ManoTrait trait)

// events
Task Exec(string eventId)   // self only
Task Down(string eventId)   // self, then descendants (preorder)
Task Up(string eventId)     // parent only, one level
```
A trait declares its target type via the generic parameter. Attaching it
to the wrong object type throws.

`ManoTrait`
```csharp
public class WorldTrait : ManoTrait<WorldObject>
{
    public void Init(WorldObject self) { /* ... */ }
    public async Task Update(WorldObject self) { /* ... */ }
}
```

`Mano.Load`
```csharp
ManoObject[] Load(string json)
T[] Load<T>(string json) where T : ManoObject
```

JSON entries need a `type` field naming the target class. Optional
`traits` array names `traits` to attach by class name.

## Invariants
- A node has at most one parent.

- Ids are unique within a tree. `Add` enforces this.

- Cycles are rejected. `Add` walks the ancestor chain.

- `Collect<T>` is lazily evaluated. Do not mutate the tree while
iterating.

## What ManoEngine is not
- Not an ECS. There is no component storage, no system scheduler.

- Not a message bus. Communication is structural, not addressed.

- Not opinionated about time, rendering, or persistence.

- Those belong to the layer above.

## Status
Pre-1.0. The core is stable; the standard library is under construction.