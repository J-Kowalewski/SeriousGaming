using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingPopUpData : MonoBehaviour
{
    public string loseName;
    [TextArea(4, 6)]
    public string loseText;
    public Sprite loseImage;

    public string winName;
    [TextArea(4, 6)]
    public string winText;
    public Sprite winImage;

    public string failsafeName;
    [TextArea(4, 6)]
    public string failsafeText;
    public Sprite failsafeImage;
}
