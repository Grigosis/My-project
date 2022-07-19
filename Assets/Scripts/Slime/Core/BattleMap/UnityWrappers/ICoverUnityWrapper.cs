using System.Collections.Generic;

namespace Assets.Scripts.Slime.Core.BattleMap
{
    public interface ICoverUnityWrapper
    {
        public void GetCellCovers(List<MapCellCover> list);
    }
}