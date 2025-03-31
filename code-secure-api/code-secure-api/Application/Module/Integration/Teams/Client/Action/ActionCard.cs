namespace CodeSecure.Application.Module.Integration.Teams.Client.Action
{
    public class ActionCard(string name) : IAction(ActionType.ActionCard, name)
    {
        public IList<IAction> Actions { get; set; } = new List<IAction>();
        public IList<Input.IInput> Inputs { get; set; } = new List<Input.IInput>();
        
    }
}