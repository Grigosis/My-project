using System.Collections.Generic;

namespace Assets.Scripts.Slime.Core.Algorythms
{
    public interface IFutureChangeable
    {
        List<IFuture> GetPossibleFutures();
    }

    public interface IFuture
    {
        float Chance { get; }
    }
    
    
}