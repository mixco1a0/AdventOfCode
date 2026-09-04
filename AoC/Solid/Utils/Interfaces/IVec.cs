using System.Numerics;

namespace AoC.Solid.Utils.Interfaces;

public interface IVec<VecT, T> :
    IAdditionOperators<VecT, VecT, VecT>,
    ISubtractionOperators<VecT, VecT, VecT>,
    IMultiplyOperators<VecT, T, VecT>,
    IDivisionOperators<VecT, T, VecT>,
    IModulusOperators<VecT, T, VecT>
    where VecT : IVec<VecT, T>
{
    static abstract VecT Zero {get;}
    static abstract VecT One {get;}
    
    public abstract T Manhattan(VecT other);
    public static abstract VecT Parse(string input, string delimeters);
}