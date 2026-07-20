namespace mano.Engine
{
    public struct Vector2
    {
        public float X { get; set; }
        public float Y { get; set; }

        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }
    }

    public struct Tick
    {
        public long Frame { get; set; }
        public float DeltaTime { get; set; }
        public DateTime DateTime { get; set; }
    }
}