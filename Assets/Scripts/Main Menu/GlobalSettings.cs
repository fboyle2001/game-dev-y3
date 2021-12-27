public class GlobalSettings {

    public static string primaryName = "Primary";
    public static string secondaryName = "Secondary";
    public static int difficulty = 1;

    public static float horizontalMouseSensitivity = 0.5f;
    public static float verticalMouseSensitivity = 0.5f;

    public static float GetEnemyHealthScalar() {
        return difficulty == 3 ? 1.3f : 1.0f;
    }

    public static float GetRewardScalar() {
        return difficulty == 3 ? 0.7f : 1.0f;
    }

    public static void GiveBonusStats(PlayerStats stats) {
        if(difficulty == 3) {
            stats.AddDamageMultiplier(-0.2f);
        }

        if(difficulty >= 2) return;
        stats.AddArmour(difficulty == 0 ? 20 : 5);
        stats.AddDamageMultiplier(difficulty == 0 ? 0.4f : 0.2f);
        stats.AddMaxHealthMultiplier(difficulty == 0 ? 1.0f : 0.0f);
        stats.AddRegenPerSecond(difficulty == 0 ? 0.5f : 0.2f);
    }

}