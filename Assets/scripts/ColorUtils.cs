using UnityEngine;

public static class ColorUtils {

    public static Color GREY = new Color(0.3f, 0.3f, 0.3f, 1.0f);

    public static Color getRandomColor() {
        return new Color(Random.Range(0, 100) / 100.0f, Random.Range(0, 100) / 100.0f, Random.Range(0, 100) / 100.0f, 1.0f);
    }

    public static Color invertColor(Color color) {
        return new Color(1 - color.r, 1 - color.g, 1 - color.b);
    }

    public static Color dullColor(Color color) {
        return new Color(color.r * 0.4f, color.g * 0.4f, color.b * 0.4f);
    }

    public static Color fadeColor(Color color) {
        return new Color(color.r, color.g, color.b, 0.3f);
    }

}
