using UnityEngine;

namespace CodeBase.Map
{
    public class WorldTile : MonoBehaviour
    {
        public  MyTile MyTile;

        public void Construct(MyTile myTile)
        {
            MyTile = myTile;
        }
    }
}