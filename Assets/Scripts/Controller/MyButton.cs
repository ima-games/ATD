using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyButton
{
    public bool isPressing = false;
    public bool onPress = false;
    public bool onReleased = false;

    private bool currentState = false;
    private bool lastState = false;

    public void Tick(bool input) {
        currentState = input;
        isPressing = currentState;

        onPress = false;
        onReleased = false;
        if (currentState != lastState) {
            if (currentState == true) {
                onPress = true;
            }
            else {
                onReleased = true;
            }
        }
        lastState = currentState;
    }
}
