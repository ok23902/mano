using System.Collections;
using System.Reflection;
using System.Text.Json;

namespace mano
{
    public class Runtime
    {
        public T Load<T>(string json) where T : ManoObject
        {
            var obj = JsonSerializer.Deserialize<T>(json);
            if (obj == null)
            {
                throw new InvalidOperationException($"JSON から {typeof(T).Name} の生成に失敗しました。");
            }
            return obj;
        }

       public void Set(object target, string memberName, object? value)
        {
            var type = target.GetType();
            var flags = BindingFlags.Public | BindingFlags.Instance;

            var prop = type.GetProperty(memberName, flags);
            if (prop != null && prop.CanWrite)
            {
                prop.SetValue(target, value);
                AutoUpdateParent(target, value);
                return;
            }

            var field = type.GetField(memberName, flags);
            if (field != null)
            {
                field.SetValue(target, value);
                AutoUpdateParent(target, value);
                return;
            }

            throw new ArgumentException($"'{type.Name}' に書き込み可能な '{memberName}' が見つかりません。");
        }

        private void AutoUpdateParent(object parentCandidate, object? value)
        {
            if (value == null || parentCandidate is not ManoObject parentObj) return;

            if (value is ManoObject childObj)
            {
                childObj.Parent = parentObj;
            }
            else if (value is IEnumerable collection)
            {
                foreach (var item in collection)
                {
                    if (item is ManoObject elementChild)
                    {
                        elementChild.Parent = parentObj;
                    }
                }
            }
        }

        public void Move(ManoObject child, ManoObject newParent, string newMemberName)
        {
            if (child.Parent != null)
            {
                DetachDynamically(child.Parent, child);
            }

            var type = newParent.GetType();
            var flags = BindingFlags.Public | BindingFlags.Instance;

            var prop = type.GetProperty(newMemberName, flags);
            if (prop != null)
            {
                AssignToMember(newParent, prop, child);
            }
            else
            {
                var field = type.GetField(newMemberName, flags);
                if (field != null)
                {
                    AssignToMember(newParent, field, child);
                }
                else
                {
                    throw new ArgumentException($"'{type.Name}' に '{newMemberName}' が見つかりません。");
                }
            }

            child.Parent = newParent;
        }

        private void AssignToMember(ManoObject parent, MemberInfo member, ManoObject child)
        {
            var value = member switch
            {
                PropertyInfo p => p.GetValue(parent),
                FieldInfo f => f.GetValue(parent),
                _ => null
            };

            if (value is IList list)
            {
                list.Add(child);
            }
            else
            {
                if (member is PropertyInfo p && p.CanWrite) p.SetValue(parent, child);
                if (member is FieldInfo f) f.SetValue(parent, child);
            }
        }

        private void DetachDynamically(ManoObject oldParent, ManoObject child)
        {
            var type = oldParent.GetType();
            var flags = BindingFlags.Public | BindingFlags.Instance;

            foreach (var prop in type.GetProperties(flags))
            {
                if (!prop.CanRead) continue;
                
                var val = prop.GetValue(oldParent);
                if (val is IList list && list.Contains(child))
                {
                    list.Remove(child);
                    return;
                }
                else if (val == child && prop.CanWrite)
                {
                    prop.SetValue(oldParent, null);
                    return;
                }
            }

            foreach (var field in type.GetFields(flags))
            {
                var val = field.GetValue(oldParent);
                if (val is IList list && list.Contains(child))
                {
                    list.Remove(child);
                    return;
                }
                else if (val == child)
                {
                    field.SetValue(oldParent, null);
                    return;
                }
            }
        }

        public void Broadcast(ManoObject root, string eventId)
        {
            root.Down(eventId);
        }
    }
}