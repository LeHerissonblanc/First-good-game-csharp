using System.Numerics;
using Raylib_cs;

public class Player : Entity
{
    public Vector2 Direction { get; private set; }
    public float TempSpeed { get; private set; }

    public Player(
        float x,
        float y,
        float speed,
        Color color,
        Vector2? size = null,
        Shape shape = Shape.Circle)
        : base(
            x: x,
            y: y,
            size: size ?? new Vector2(35, 35),
            speed: speed,
            color: color,
            state: State.Dynamic,
            shape: shape)
    {
        Direction = Vector2.Zero;
        TempSpeed = speed;
    }

    public override void Update(float dt)
    {
        var direction = Vector2.Zero;
        TempSpeed = Speed;

        if (Raylib.IsKeyDown(KeyboardKey.D)) direction.X += 1;
        if (Raylib.IsKeyDown(KeyboardKey.A)) direction.X -= 1;
        if (Raylib.IsKeyDown(KeyboardKey.S)) direction.Y += 1;
        if (Raylib.IsKeyDown(KeyboardKey.W)) direction.Y -= 1;
        if (Raylib.IsKeyDown(KeyboardKey.LeftShift)) TempSpeed *= 1.5f;

        if (direction.Length() > 0)
            direction = Vector2.Normalize(direction);

        Direction = direction;
        Velocity = direction * TempSpeed;
    }
}
