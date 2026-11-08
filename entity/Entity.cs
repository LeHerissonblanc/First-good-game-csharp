using System.Numerics;
using Raylib_cs;

public enum State
{
    Static,
    Dynamic
}

public enum Shape
{
    Circle,
    Rect
}

public class Entity
{
    public Vector2 Position { get; set; }
    public Vector2 Velocity { get; set; }
    public Vector2 Size { get; set; }
    public float Speed { get; set; }
    public State State { get; set; }
    public Shape Shape { get; set; }
    public Color Color { get; set; }

    public Entity(
        float x = 0,
        float y = 0,
        Vector2? size = null,
        float speed = 0,
        Color? color = null,
        State state = State.Static,
        Shape shape = Shape.Rect)
    {
        Position = new Vector2(x, y);
        Velocity = Vector2.Zero;
        Size = size ?? new Vector2(30, 30);
        Speed = speed;
        State = state;
        Shape = shape;
        Color = color ?? Color.Red;
    }

    public virtual void Update(float dt)
    {
    }

    public void Move(float dt, List<Entity> solids)
    {
        if (State != State.Dynamic)
            return;

        // move each axis separately so the entity slides along walls
        float newX = Position.X + Velocity.X * dt;
        if (!HitsAny(new Vector2(newX, Position.Y), solids))
            Position = new Vector2(newX, Position.Y);

        float newY = Position.Y + Velocity.Y * dt;
        if (!HitsAny(new Vector2(Position.X, newY), solids))
            Position = new Vector2(Position.X, newY);
    }

    public bool HitsAny(Vector2 pos, List<Entity> solids)
    {
        return solids.Any(s => s != this && CollidesAt(pos, s));
    }

    public Rectangle GetRect(Vector2? pos = null)
    {
        var p = pos ?? Position;
        return new Rectangle(p.X, p.Y, Size.X, Size.Y);
    }

    public bool CollidesAt(Vector2 pos, Entity other)
    {
        if (Shape == Shape.Circle)
        {
            float radius = Size.X / 2;
            if (other.Shape == Shape.Circle)
                return Raylib.CheckCollisionCircles(pos, radius, other.Position, other.Size.X / 2);
            return Raylib.CheckCollisionCircleRec(pos, radius, other.GetRect());
        }

        if (other.Shape == Shape.Circle)
            return Raylib.CheckCollisionCircleRec(other.Position, other.Size.X / 2, GetRect(pos));
        return Raylib.CheckCollisionRecs(GetRect(pos), other.GetRect());
    }

    public void Draw()
    {
        switch (Shape)
        {
            case Shape.Circle:
                if (Size.X != Size.Y)
                    throw new InvalidOperationException("size X and size Y must be equal for the circles");

                float radius = Size.X / 2;
                Raylib.DrawCircle((int)Position.X, (int)Position.Y, radius, Color);
                Raylib.DrawRing(Position, radius - 3, radius, 0, 360, 48, Color.Black);
                break;

            case Shape.Rect:
                var rect = GetRect();
                Raylib.DrawRectangleRec(rect, Color);
                Raylib.DrawRectangleLinesEx(rect, 3, Color.Black);
                break;

            default:
                throw new InvalidOperationException($"{this} has no shape");
        }
    }
}
