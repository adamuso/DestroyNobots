namespace DestroyNobots.Engine.Input
{
    public interface IInputElement
    {
        void OnKeyDown(KeyboardEventArgs e);
        void OnKeyUp(KeyboardEventArgs e);
    }
}
