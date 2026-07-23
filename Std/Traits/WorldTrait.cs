namespace mano
{
    public class WorldTrait : ManoTrait<WorldObject>
    {
        public void OnInit(WorldObject world)
        {
            Console.WriteLine($"[World] 世界を生成しました。時刻は {world.TimeOfDay} 時です。");
        }

        public void OnUpdate(WorldObject world)
        {
            world.TimeOfDay++;

            if (world.TimeOfDay >= 24)
            {
                world.TimeOfDay = 0;
                world.DayCount++;
                Console.WriteLine($"[World] 日付が変わりました。{world.DayCount}日目。");
            }
        }
    }
}