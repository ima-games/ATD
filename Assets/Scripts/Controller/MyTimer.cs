using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTimer
{
    public enum STATE
    {
        IDLE,
        RUN,
        FINISHED
    }

    public STATE state;

    public float duration = 1.0f;

    private float elapsedTime = 0;

    public void TimerTick() {

        if (state == STATE.IDLE) {

        }
        else if (state == STATE.RUN) {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= duration) {
                state = STATE.FINISHED;
            }
        }
        else if (state == STATE.FINISHED) {

        }
        else {
            Debug.Log("MYTIMEERROR");
        }
    }
    public void GoTimer() {
        elapsedTime = 0;
        state = STATE.RUN;
    }
}
