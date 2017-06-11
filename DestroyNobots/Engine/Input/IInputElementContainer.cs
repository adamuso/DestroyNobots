using System.Collections.Generic;

namespace DestroyNobots.Engine.Input
{
    public interface IInputElementContainer : IInputElement
    {
        IEnumerable<IInputElement> Children { get; }
    }
}
