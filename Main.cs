using Raylib_cs;

const int ScreenWidth = 800;
const int ScreenHeight = 600;
Color bgColor = Color.RayWhite;

Raylib.SetConfigFlags(ConfigFlags.Msaa4xHint | ConfigFlags.VSyncHint);
Raylib.InitWindow(ScreenWidth, ScreenHeight, "Game?");
// Raylib.SetTargetFPS(Fps);  // VSync enabled

var game = new Game(ScreenWidth, ScreenHeight, cellSize: 50);

while (!Raylib.WindowShouldClose())
{
    float dt = Raylib.GetFrameTime();
    game.Update(dt);
    game.Draw(dt, bgColor);
}

Raylib.CloseWindow();
