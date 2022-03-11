using System;
public class Constants
{
    public enum SCENES
    {
        MAIN_MENU = 0,
        INTRO,
        DEMOGRAPHICS,
        ENCODING_INSTRUCTIONS,
        ENCODING,
        INTERFERENCE_INSTRUCTIONS,
        INTERFERENCE,
        GOT_IT_WRONG_ALERT,
        RECALL_INSTRUCTIONS,
        RECALL,
        VISUO_INSTRUCTIONS,
        VISIO_SPATIAL,
        FINAL_SCORE,
        FINAL_SCORE_VISUO
    }

    public const int RECALL_TOTAL_ITERATIONS = 30;
    public const int INTERFERENCE_TOTAL_ITERATIONS = 10;//100;
    public const int VISUOSPATIAL_TOTAL_ITERATIONS = 25;//25;

    public static bool SKIP_VISUOSPATIAL_LEVEL = false;
    public static bool INSERT_SCORE_IN_DATABASE = false;
}
