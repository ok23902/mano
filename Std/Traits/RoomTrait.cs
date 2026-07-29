namespace mano
{
    public class RoomTrait : ManoTrait<RoomObject>
    {
        public void OnInit(RoomObject room)
        {
            Console.WriteLine("RoomObject Initialized");
        }
        public void OnUpCasted(RoomObject room)
        {
            Console.WriteLine("Up Casted");
            room.Down("OnDownCast");
        }
    }
}