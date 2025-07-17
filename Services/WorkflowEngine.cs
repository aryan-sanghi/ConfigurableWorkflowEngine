using InfoneticaWorkflowEngine.Models;

namespace InfoneticaWorkflowEngine.Services;

public class WorkflowEngine {
    private Dictionary<string, WorkflowDefinition> definitions = new();
    private Dictionary<string, WorkflowInstance> instances = new();

    public string CreateDefinition(WorkflowDefinition def) {
        if (definitions.ContainsKey(def.Id))
            throw new Exception("Definition ID already exists.");

        if (def.States.Values.Count(s => s.IsInitial) != 1)
            throw new Exception("Exactly one initial state is required.");

        foreach (var action in def.Actions.Values) {
            if (!def.States.ContainsKey(action.ToState))
                throw new Exception($"Action {action.Id} points to undefined ToState.");
            foreach (var from in action.FromStates) {
                if (!def.States.ContainsKey(from))
                    throw new Exception($"Action {action.Id} has undefined FromState {from}.");
            }
        }

        definitions[def.Id] = def;
        return def.Id;
    }

    public string StartInstance(string defId) {
        if (!definitions.TryGetValue(defId, out var def))
            throw new Exception("Definition not found.");

        var initial = def.States.Values.FirstOrDefault(s => s.IsInitial);
        if (initial == null) throw new Exception("Initial state not found.");

        var instanceId = Guid.NewGuid().ToString();
        instances[instanceId] = new WorkflowInstance {
            Id = instanceId,
            DefinitionId = defId,
            CurrentStateId = initial.Id,
        };
        return instanceId;
    }

    public void ExecuteAction(string instanceId, string actionId) {
        if (!instances.TryGetValue(instanceId, out var instance))
            throw new Exception("Instance not found.");

        var def = definitions[instance.DefinitionId];
        if (!def.Actions.TryGetValue(actionId, out var action))
            throw new Exception("Action not found.");

        if (!action.Enabled) throw new Exception("Action is disabled.");
        if (!action.FromStates.Contains(instance.CurrentStateId))
            throw new Exception("Invalid state transition.");
        if (!def.States.ContainsKey(action.ToState))
            throw new Exception("ToState is invalid.");
        if (def.States[instance.CurrentStateId].IsFinal)
            throw new Exception("Cannot act from final state.");

        instance.CurrentStateId = action.ToState;
        instance.History.Add((actionId, DateTime.UtcNow));
    }

    public WorkflowDefinition GetDefinition(string id) => definitions[id];
    public WorkflowInstance GetInstance(string id) => instances[id];
}
