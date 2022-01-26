namespace CrudeObservatory.Abstractions.Interfaces
{
    public interface IIntervalOutput
    {
        /// <summary>
        /// In Unix msec
        /// </summary>
        public long NominalTime { get; set; }
    }
}
