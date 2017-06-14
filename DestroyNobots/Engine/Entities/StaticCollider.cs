namespace DestroyNobots.Engine.Entities
{
    public class StaticCollider : Collider
    {
        public override bool CanMove { get { return false; } }

        protected override void MakeDirty() { }

        protected override void MakeClean() { }
    }
}
