public static class DebugFlags {
    public static bool DebugDontLoadGame { get; } = false;
    public static bool DebugDontShowIntroductions { get; } = false;
    public static bool DebugDontShowTutorial { get; } = false;

    public static bool SkipTitleScreen { get; } = false;

    public static bool StartWith4DayStreak { get; } = false;

    public static bool OnlyMoveCameraWhenHoldingLeftMouse { get; } = false;
    
    public static bool ShowDebugText { get; } = false;

    public const bool FastEnvironmentTransitions = false;
}