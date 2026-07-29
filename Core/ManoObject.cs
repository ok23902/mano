using System.Collections;
using System.Reflection;
using System.Text.Json.Serialization;

namespace mano
{
    public abstract class ManoObject
    {
        private readonly List<ManoTrait> _traits = [];

        [JsonIgnore]
        public virtual IReadOnlyList<ManoTrait> Traits => _traits;

        [JsonIgnore]
        public IEnumerable<string> TraitIds => _traits.Select(t => t.GetType().Name);

        public void AttachTrait(ManoTrait trait)
        {
            var targetType = GetTraitTargetType(trait.GetType());
            if (targetType != null && !targetType.IsInstanceOfType(this))
            {
                throw new InvalidOperationException(
                    $"Trait '{trait.GetType().Name}' can only be attached to '{targetType.Name}', " +
                    $"but was attached to '{GetType().Name}'.");
            }
            _traits.Add(trait);
        }

        public void DetachTrait(ManoTrait trait) => _traits.Remove(trait);

        private static Type? GetTraitTargetType(Type traitType)
        {
            var t = traitType.BaseType;
            while (t != null)
            {
                if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(ManoTrait<>))
                    return t.GetGenericArguments()[0];
                t = t.BaseType;
            }
            return null;
        }

        public event Action<ManoObject, string>? Changed;

        public void NotifyChanged(string fieldName) => Changed?.Invoke(this, fieldName);

        public virtual void Down(string eventId) => RunEvent(eventId, cascade: true);

        public void Down(string[] events)
        {
            foreach (var e in events) Down(e);
        }

        protected void ExecuteOwnTraitsOnly(string eventId) => RunEvent(eventId, cascade: false);

        private void RunEvent(string eventId, bool cascade)
        {
            var runtime = Runtime.Instance;
            if (!runtime.TryEnterEvent(this, eventId)) return;

            runtime.PushCall(this);
            try
            {
                ExecuteTraits(eventId);
                if (cascade) DownToChildren(eventId);
            }
            finally
            {
                runtime.PopCall();
                runtime.ExitEvent(this, eventId);
            }
        }

        private void DownToChildren(string eventId)
        {
            var members = GetType().GetMembers(BindingFlags.Public | BindingFlags.Instance);
            foreach (var member in members)
            {
                object? value = member switch
                {
                    FieldInfo f => f.GetValue(this),
                    PropertyInfo p => p.CanRead ? p.GetValue(this) : null,
                    _ => null
                };

                if (value is ManoObject childObj)
                {
                    childObj.Down(eventId);
                }
                else if (value is IEnumerable childList && value is not string)
                {
                    foreach (var item in childList)
                    {
                        if (item is ManoObject childItem)
                        {
                            childItem.Down(eventId);
                        }
                    }
                }
            }
        }

        public void Up(string eventId)
        {
            var parent = Runtime.Instance.GetCallerOf(this);
            if (parent == null) return;

            if (!Runtime.Instance.TryEnterEvent(parent, eventId)) return;
            try
            {
                parent.ExecuteTraits(eventId);
            }
            finally
            {
                Runtime.Instance.ExitEvent(parent, eventId);
            }
        }

        public void Up(string[] events)
        {
            foreach (var e in events) Up(e);
        }

        private void ExecuteTraits(string eventId)
        {
            foreach (var trait in _traits)
            {
                var method = trait.GetType().GetMethod(
                    eventId,
                    BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic
                );

                method?.Invoke(trait, [this]);
            }
        }
    }
}