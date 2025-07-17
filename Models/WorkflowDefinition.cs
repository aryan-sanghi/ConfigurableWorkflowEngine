namespace InfoneticaWorkflowEngine.Models;

public class WorkflowDefinition {
    public string Id { get; set; }
    public Dictionary<string, State> States { get; set; } = new();
    public Dictionary<string, Action> Actions { get; set; } = new();
}
