
using Unity.Entities;
using UnityEngine;

public partial class InputSystem : SystemBase
{
    private Controls controls;
    protected override void OnCreate()
    {
        if(!SystemAPI.TryGetSingleton<InputComponent>(out InputComponent input))
        {
            EntityManager.CreateEntity(typeof(InputComponent));
        }
        controls = new Controls();
        controls.Enable();
        base.OnCreate();
    }
    protected override void OnUpdate()
    {
        Vector2 movementDirection=Vector2.zero;
        switch (PlayerTouchInputManager.Instance.inputType)
        {
            case InputType.WASD:
                movementDirection = controls.ActionMap.Movement.ReadValue<Vector2>();
                break;
            case InputType.Joystick:
                movementDirection = PlayerTouchInputManager.Instance.MovementAmount;
                break;
            default:
                break;
        }
        
      
        SystemAPI.SetSingleton(new InputComponent { MovementDirection = movementDirection });
    }
}
