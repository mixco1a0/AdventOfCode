namespace AoC.Solid.Core.Interfaces;

public interface IWriter<T>
{
    void Write(T t);
}