using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NodeVisuals : MonoBehaviour
{
    [SerializeField] private SquareNode node;

    //In a normal situation, it should be only one, but I'm had no time to find or create the sprite
    [SerializeField] private SpriteRenderer[] sprites;

    void Start()
    {
        sprites = GetComponentsInChildren<SpriteRenderer>();
        node = GetComponent<SquareNode>();
        node.OnChangeCharge += Node_OnChangeCharge;
    }

    private void Node_OnChangeCharge(bool newValue)
    {
        var from = CircuitColors.UnchargedColor;
        var to = CircuitColors.ChargedColor;

        if (newValue == false)
        {
            from = CircuitColors.ChargedColor;
            to = CircuitColors.UnchargedColor;
        }

        StartCoroutine(ChangeColor_Routine(from, to));
    }

    private IEnumerator ChangeColor_Routine(Color from, Color to)
    {
        float lerpValue = 0;

        while (lerpValue < 1)
        {
            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i].color = Color.Lerp(from, to, lerpValue);
            }

            lerpValue += Time.deltaTime * 4; //duration equals 0.25f seconds
            yield return null;
        }
    }
}

public class CircuitColors
{
    public static readonly Color UnchargedColor = new Color(0.5f, 0.5f, 0.5f, 1);
    public static readonly Color ChargedColor = Color.white;
}