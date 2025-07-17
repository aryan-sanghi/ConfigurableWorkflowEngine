# Infonetica Workflow Engine

A minimal backend for managing configurable state-machine workflows.

## 🏃 How to Run

```bash
dotnet run
```
Requires .NET 8 SDK.

🛠 Features
- Define workflows with states and actions
- Create instances from definitions
- Execute actions with validation
- View instance status and history

✅ Assumptions
- All data is stored in-memory only
- Workflow definition is immutable after creation
- States and actions use string IDs

❗ Known Limitations
- No persistence across runs
- No authentication or concurrency protection

### 🧪 Optional: Add `InfoneticaWorkflowEngine.csproj`


You can create the project with:
```bash
dotnet new web -n InfoneticaWorkflowEngine
```