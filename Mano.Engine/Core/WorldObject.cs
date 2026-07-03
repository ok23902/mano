using System.Collections.Generic;

namespace mano.Engine
{
    public class WorldObject
    {
        public DataContainer Static { get; } = new DataContainer();
        public DataContainer Dynamic { get; } = new DataContainer();

        // 4つの辞書を持つだけのシンプルなコンテナ
        public class DataContainer
        {
            public Dictionary<string, Object> Character { get; } = new Dictionary<string, Object>();
            public Dictionary<string, Object> Item { get; } = new Dictionary<string, Object>();
            public Dictionary<string, Object> Room { get; } = new Dictionary<string, Object>();
            public Dictionary<string, Object> Rule { get; } = new Dictionary<string, Object>();
            
            public void Clear()
            {
                Character.Clear();
                Item.Clear();
                Room.Clear();
                Rule.Clear();
            }
        }
    }
}