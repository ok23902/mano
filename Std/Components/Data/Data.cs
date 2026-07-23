namespace mano
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

        public static Vector2 Zero => new(0f, 0f);

        public static Vector2 One => new(1f, 1f);

        public override readonly string ToString() => $"({X}, {Y})";
    }

    public struct Tick
    {
        public long Frame { get; set; }
        public float DeltaTime { get; set; }
        public DateTime DateTime { get; set; }
    }
}