using System.Collections;
using System.Reflection;
using System.Text.Json.Serialization;

namespace mano
{
    public abstract class ManoObject
    {
        public virtual List<ManoTrait> Traits { get; set; } = [];

        [JsonIgnore]
        public ManoObject? Parent { get; internal set; }

        private static readonly HashSet<(ManoObject, string)> ProcessedEvents = new();

        public void Down(string eventId)
        {
            if (!ProcessedEvents.Add((this, eventId))) return;

            try
            {
                ExecuteTraits(eventId);

                var members = GetType().GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
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
                        childObj.Parent = this;
                        childObj.Down(eventId);
                    }
                    else if (value is IEnumerable childList && value is not string)
                    {
                        foreach (var item in childList)
                        {
                            if (item is ManoObject childItem)
                            {
                                childItem.Parent = this;
                                childItem.Down(eventId);
                            }
                        }
                    }
                }
            }
            finally
            {
                if (Parent == null)
                {
                    ProcessedEvents.Clear();
                }
            }
        }

        public void Down(string[] events)
        {
            foreach (var e in events) Down(e);
        }

        public void Up(string eventId)
        {
            if (Parent == null) return;
            if (!ProcessedEvents.Add((Parent, eventId))) return;

            Parent.ExecuteTraits(eventId);
        }

        public void Up(string[] events)
        {
            foreach (var e in events) Up(e);
        }

        private void ExecuteTraits(string eventId)
        {
            foreach (var trait in Traits)
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