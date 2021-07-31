namespace ControlsLibrary.Model
{
    public interface IFaComponent
    {
        public ITransitionFilter DefaultFilter { get; }
        public ITransitionSideEffect DefaultSideEffect { get; }
        public ITransitionFilter CurrentFilter { get; }
        public bool IsReadyToTerminate { get; }
        public bool RequiresTermination { get; }
        public void TakeTransition(TransitionComponent transition);
        public IFaComponent Copy();
    }
}