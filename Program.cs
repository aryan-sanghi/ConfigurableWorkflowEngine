using InfoneticaWorkflowEngine.Models;
using InfoneticaWorkflowEngine.Services;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
var engine = new WorkflowEngine();

app.MapPost("/workflows", (WorkflowDefinition def) => {
    return Results.Ok(engine.CreateDefinition(def));
});

app.MapGet("/workflows/{id}", (string id) => {
    return Results.Ok(engine.GetDefinition(id));
});

app.MapPost("/instances", (string defId) => {
    return Results.Ok(engine.StartInstance(defId));
});

app.MapPost("/instances/{id}/actions/{actionId}", (string id, string actionId) => {
    engine.ExecuteAction(id, actionId);
    return Results.Ok("Action executed");
});

app.MapGet("/instances/{id}", (string id) => {
    return Results.Ok(engine.GetInstance(id));
});

app.Run();