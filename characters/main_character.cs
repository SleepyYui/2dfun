using Godot;

public partial class main_character : CharacterBody2D
{
    [Export] private AnimationNodeStateMachine _animationState;

    [Export] private AnimationTree _animationTree;

    [Export] private float _moveSpeed = 100f;

    [Export] private Vector2 _startingPosition = new(0, 1);

    // create function to set animation state
    private void SetAnimationState(Vector2 move_input)
    {
        GD.Print(move_input.ToString());
        if (move_input != Vector2.Zero)
        {
            _animationTree.Set("parameters/Idle/blend_position", move_input);
            _animationTree.Set("parameters/Walk/blend_position", move_input);
        }
    }

    private void PickNewState(Vector2 move_input)
    {
        var animationStateMachine = (AnimationNodeStateMachinePlayback)_animationTree.Get("parameters/playback");
        if (move_input != Vector2.Zero)
            animationStateMachine.Travel("Walk");
        else
            animationStateMachine.Travel("Idle");
    }


    public override void _Ready()
    {
        base._Ready();
        // Called every time the node is added to the scene.
        // Initialization here

        SetAnimationState(_startingPosition);

        GD.Print("main_character ready");
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        // Called every frame. Delta is time since last frame.
        // Update physics here.

        // get input direction
        var inputDirection = new Vector2(
            Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left"),
            Input.GetActionStrength("move_down") - Input.GetActionStrength("move_up")
        );
        //GD.Print(inputDirection.ToString());

        SetAnimationState(inputDirection);
        
        var velocity = inputDirection * _moveSpeed;
        Velocity = velocity;
        
        MoveAndSlide();
        PickNewState(inputDirection);
    }
}