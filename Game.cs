using System.Numerics;
using Raylib_cs;

public class Game
{
    private readonly int screenWidth;
    private readonly int screenHeight;
    private readonly int cellSize;
    private int yGuiOffset;

    private readonly List<Entity> entities = new();
    private readonly Player player;
    private readonly Enemy enemy;
    private readonly Entity box;
    private Camera2D camera;

    public Game(int screenWidth, int screenHeight, int cellSize = 50)
    {
        this.screenWidth = screenWidth;
        this.screenHeight = screenHeight;
        this.cellSize = cellSize;
        yGuiOffset = 0;

        // init all entities
        player = new Player(
            x: 0,
            y: 0,
            size: new Vector2(35, 35),
            color: Color.SkyBlue,
            speed: 200,
            shape: Shape.Circle
        );

        enemy = new Enemy(
            x: -200,
            y: 200,
            speed: 100,
            color: Color.Red,
            target: player,
            shape: Shape.Rect
        );

        box = new Entity(
            x: cellSize * 2,
            y: cellSize * 3,
            size: new Vector2(cellSize * 10, cellSize * 5),
            color: Color.DarkGray,
            shape: Shape.Rect,
            state: State.Static
        );

        entities.Add(player);
        entities.Add(enemy);
        entities.Add(box);

        // camera
        camera = new Camera2D
        {
            Target = player.Position,
            Offset = new Vector2(screenWidth / 2f, screenHeight / 2f),
            Rotation = 0,
            Zoom = 1
        };
    }

    private void DisplayInfo(string text, object? value = null, Color? color = null)
    {
        yGuiOffset += 20;
        Raylib.DrawText($"{text} {value}", 10, yGuiOffset, 20, color ?? Color.Red);
    }

    private void DrawGui(float dt)
    {
        yGuiOffset = 0;
        DisplayInfo("Game?");
        DisplayInfo("player x:", $"{player.Position.X:F2}");
        DisplayInfo("player y:", $"{player.Position.Y:F2}");
        DisplayInfo("player speed:", player.Speed);
        DisplayInfo("player temp speed:", player.TempSpeed);
        DisplayInfo("enemy x:", $"{enemy.Position.X:F2}");
        DisplayInfo("enemy y:", $"{enemy.Position.Y:F2}");
        DisplayInfo("dt:", dt);
        DisplayInfo("camera x:", $"{camera.Target.X + camera.Offset.X:F2}");
        DisplayInfo("camera y:", $"{camera.Target.Y + camera.Offset.Y:F2}");
    }

    public void Update(float dt)
    {
        foreach (var entity in entities)
            entity.Update(dt);

        var solids = entities.Where(e => e.State == State.Static).ToList();
        foreach (var entity in entities)
            entity.Move(dt, solids);

        camera.Target = player.Position;
    }

    public void Draw(float dt, Color bgColor)
    {
        Raylib.BeginDrawing();

        Raylib.ClearBackground(bgColor);

        Raylib.BeginMode2D(camera);

        // World grid
        for (int x = -5000; x <= 5000; x += cellSize)
            Raylib.DrawLine(x, -5000, x, 5000, Color.LightGray);
        for (int y = -5000; y <= 5000; y += cellSize)
            Raylib.DrawLine(-5000, y, 5000, y, Color.LightGray);
        Raylib.DrawRectangleLines(-5000, -5000, 10_000, 10_000, Color.Red);

        foreach (var entity in entities)
            entity.Draw();

        Raylib.EndMode2D();

        DrawGui(dt);

        Raylib.EndDrawing();
    }
}
