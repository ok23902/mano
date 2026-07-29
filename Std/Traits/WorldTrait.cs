namespace mano
{
    public class WorldTrait : ManoTrait<WorldObject>
    {
        public void OnInit(WorldObject world)
        {
            Console.WriteLine("WorldObject Initialized");
        }

        public void OnUpdate(WorldObject world)
        {
            world.TimeOfDay++;

            if (world.TimeOfDay >= 24)
            {
                world.TimeOfDay = 0;
                world.DayCount++;
                //Console.WriteLine($"[World] 日付が変わりました。{world.DayCount}日目。");
            }
        }
    }
}