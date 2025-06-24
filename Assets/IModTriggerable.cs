public interface IModTriggerable
{
    string ActionName { get; }       // Label for the button
    void Trigger();                  // Called when the button is clicked
}
