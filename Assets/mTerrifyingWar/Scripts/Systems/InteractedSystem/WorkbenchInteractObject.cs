using Zenject;

public class WorkbenchInteractObject : InteractObject
{
    [Inject] private readonly UIWindowService _windowService;
    
    protected override void OnInteract()
    {
        _windowService.OpenWindow(WindowType.Workbench);
    }
}