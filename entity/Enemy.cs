using System.Numerics;
using Raylib_cs;

public class Enemy : Entity
{
    private readonly Entity target;
    private readonly float stopMargin = 50;

    public Enemy(
        float x,
        float y,
        float speed,
        Color color,
        Entity target,
        State state = State.Dynamic,
        Shape shape = Shape.Circle)
        : base(
            x: x,
            y: y,
            size: new Vector2(35, 35),
            speed: speed,
            color: color,
            state: state,
            shape: shape)
    {
        this.target = target;
    }

    public override void Update(float dt)
    {
        var direction = target.Position - Position;
        float length = direction.Length();

        float surfaceDistance = length - target.Size.X / 2;
        float stopDistance = Size.X / 2 + stopMargin;

        if (length > 0 && surfaceDistance > stopDistance)
        {
            direction = Vector2.Normalize(direction);
            Velocity = direction * Speed;
        }
        else
        {
            Velocity = Vector2.Zero;
        }
    }
}
