using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyButton
{
    public bool isPressing = false;
    public bool onPressed = false;
    public bool onReleased = false;
    public bool isExtending = false;
    public bool isDelaying = false;

    public float entendingDuration = 0.20f;
    public float delayingDuration = 0.20f;//输入延时 

    private bool currentState = false;
    private bool lastState = false;

    private MyTimer extendingTimer = new MyTimer();
    private MyTimer delayingTimer = new MyTimer();

    public void Tick(bool input) {

        extendingTimer.TimerTick();
        delayingTimer.TimerTick();

        currentState = input;

        isPressing = currentState;

        onPressed = false;
        onReleased = false;
        isExtending = false;
        isDelaying = false;

        if (currentState != lastState) {
            if (currentState == true) {
                onPressed = true;
                StartTimer(delayingTimer, delayingDuration);
            }
            else {
                onReleased = true;
                StartTimer(extendingTimer, entendingDuration);
            }
        }
        lastState = currentState;

        if (extendingTimer.state == MyTimer.STATE.RUN) {
            isExtending = true;
        }

        if(delayingTimer.state == MyTimer.STATE.RUN) {
            isDelaying = true;
        }
    }
    private void StartTimer(MyTimer timer, float duratime) {
        //if (button.isPressing) {
            timer.duration = duratime;
            timer.GoTimer();
        //}
    }
}
