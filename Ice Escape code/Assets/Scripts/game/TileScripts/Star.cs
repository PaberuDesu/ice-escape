using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : Tile
{
    public static int receivedStarCounter;

    private void Awake() {
        receivedStarCounter = 0;
    }

    public void GetStar() {
        receivedStarCounter++;
        animator.SetBool("_isDisappearing", true);
        Invoke("destroy", 1);
    }

    private void destroy() {
        Destroy(this);
    }

    public static bool GetStarToLevelResult() {
        if (receivedStarCounter > 0) {
            receivedStarCounter--;
            return true;
        }
        return false;
    }
}