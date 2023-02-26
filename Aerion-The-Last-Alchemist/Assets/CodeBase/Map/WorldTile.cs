using UnityEngine;

namespace CodeBase.Map
{
    public class WorldTile : MonoBehaviour
    {
        public  MyTile MyTile;
        private bool _isSelected;

        public void Construct(MyTile myTile)
        {
            MyTile = myTile;
        }

        private void SelectTile()
        {
            _isSelected = true;
        }
    }
}