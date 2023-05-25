using System.Collections;
using UnityEngine;


public class Action : MonoBehaviour
{

    public string name;
    public double difficulty = 50;
    public int factor = 5;
    
    [TextArea(4,6)]
    public string goodFlavorText;

    public Sprite goodImage;
    public int goodResult;
    public Color goodColor = Color.green;

    [TextArea(4,6)]
    public string badFlavorText;

    public Sprite badImage;
    public int badResult;
    public Color badColor = Color.red;

    public bool selected = false;
}
