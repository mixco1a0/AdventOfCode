using System.Collections.Generic;
using System.Linq;
using AoC.Solid.Utils.Math;

namespace AoC.Solid.Utils.Math2D;

public enum Dir
{
    None = -1,
    North,
    NorthEast,
    East,
    SouthEast,
    South,
    SouthWest,
    West,
    NorthWest,
    Max = NorthWest
}

public static class Map
{
    /// <summary>
    /// NorthWest (-1, -1) | North     ( 0, -1) | NorthEast ( 1, -1)
    /// West      (-1,  0) | None      ( 0,  0) | East      ( 1,  0)
    /// SouthWest (-1,  1) | South     ( 0,  1) | SouthEast ( 1,  1)
    /// </summary>
    private record Node(Dir Dir, Vec2 Vec2, char Arrow, char SimpleArrow);

    private static readonly char _emptySimpleArrow = '_';

    private static readonly Dictionary<Dir, Node> _cache = new()
    {
        { Dir.None,         new(Dir.None,       new( 0,  0), '.', '.') },
        { Dir.North,        new(Dir.North,      new( 0, -1), '↑', '^') },
        { Dir.NorthEast,    new(Dir.NorthEast,  new( 1, -1), '↗', _emptySimpleArrow) },
        { Dir.East,         new(Dir.East,       new( 1,  0), '→', '>') },
        { Dir.SouthEast,    new(Dir.SouthEast,  new( 1,  1), '↘', _emptySimpleArrow) },
        { Dir.South,        new(Dir.South,      new( 0,  1), '↓', 'v') },
        { Dir.SouthWest,    new(Dir.SouthWest,  new(-1,  1), '↙', _emptySimpleArrow) },
        { Dir.West,         new(Dir.West,       new(-1,  0), '←', '<') },
        { Dir.NorthWest,    new(Dir.NorthWest,  new(-1, -1), '↖', _emptySimpleArrow) }
    };

    public static readonly Dictionary<Dir, Vec2> Neighbor = _cache.ToDictionary(p => p.Key, p => p.Value.Vec2);
    public static readonly Dictionary<Dir, char> Arrow = _cache.ToDictionary(p => p.Key, p => p.Value.Arrow);
    public static readonly Dictionary<Dir, char> SimpleArrow = _cache.Where(p => p.Value.SimpleArrow != _emptySimpleArrow)
                                                                .ToDictionary(p => p.Key, p => p.Value.SimpleArrow);
    public static readonly Dictionary<char, Dir> SimpleArrowFlipped = _cache.Where(p => p.Value.SimpleArrow != _emptySimpleArrow)
                                                                        .ToDictionary(p => p.Value.SimpleArrow, p => p.Key);
    public static readonly Dictionary<Dir, Dir> RotateCW45 = _cache.ToDictionary(p => p.Key, p => (Dir)Int.Mod((int)p.Key + 1, (int)Dir.Max));
    public static readonly Dictionary<Dir, Dir> RotateCW90 = _cache.ToDictionary(p => p.Key, p => (Dir)Int.Mod((int)p.Key + 2, (int)Dir.Max));
    public static readonly Dictionary<Dir, Dir> RotateCCW45 = _cache.ToDictionary(p => p.Key, p => (Dir)Int.Mod((int)p.Key - 1, (int)Dir.Max));
    public static readonly Dictionary<Dir, Dir> RotateCCW90 = _cache.ToDictionary(p => p.Key, p => (Dir)Int.Mod((int)p.Key - 2, (int)Dir.Max));
}