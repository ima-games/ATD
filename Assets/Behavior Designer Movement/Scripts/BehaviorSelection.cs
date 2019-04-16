using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using BehaviorDesigner.Runtime;

public class BehaviorSelection : MonoBehaviour
{
    public GameObject marker;
    public GameObject mainBot;
    public GameObject flockGroup;
    public GameObject followGroup;
    public GameObject queueGroup;
    public GameObject[] waypoints;
    public GameObject[] waypointsA;
    public GUISkin descriptionGUISkin;

    private Vector3[] flockGroupPosition;
    private Vector3[] followGroupPosition;
    private Vector3[] queueGroupPosition;
    private Quaternion[] flockGroupRotation;
    private Quaternion[] followGroupRotation; 
    private Quaternion[] queueGroupRotation;

    private Dictionary<int, BehaviorTree> behaviorTreeGroup = new Dictionary<int, BehaviorTree>();

    private enum BehaviorSelectionType { MoveTowards, RotateTowards, Seek, Flee, Pursue, Evade, Patrol, Cover, Wander, Search, WithinDistance, CanSeeObject, CanHearObject, Flock, LeaderFollow, Queue, Last }
    private BehaviorSelectionType selectionType = BehaviorSelectionType.MoveTowards;
    private BehaviorSelectionType prevSelectionType = BehaviorSelectionType.MoveTowards;

    public void Start()
    {
        var behaviorTrees = mainBot.GetComponents<BehaviorTree>();
        for (int i = 0; i < behaviorTrees.Length; ++i) {
            behaviorTreeGroup.Add(behaviorTrees[i].Group, behaviorTrees[i]);
        }
        behaviorTrees = Camera.main.GetComponents<BehaviorTree>();
        for (int i = 0; i < behaviorTrees.Length; ++i) {
            behaviorTreeGroup.Add(behaviorTrees[i].Group, behaviorTrees[i]);
        }

        flockGroupPosition = new Vector3[flockGroup.transform.childCount];
        flockGroupRotation = new Quaternion[flockGroup.transform.childCount];
        for (int i = 0; i < flockGroup.transform.childCount; ++i) {
            flockGroup.transform.GetChild(i).gameObject.SetActive(false);
            flockGroupPosition[i] = flockGroup.transform.GetChild(i).transform.position;
            flockGroupRotation[i] = flockGroup.transform.GetChild(i).transform.rotation;
        }
        followGroupPosition = new Vector3[followGroup.transform.childCount];
        followGroupRotation = new Quaternion[followGroup.transform.childCount];
        for (int i = 0; i < followGroup.transform.childCount; ++i) {
            followGroup.transform.GetChild(i).gameObject.SetActive(false);
            followGroupPosition[i] = followGroup.transform.GetChild(i).transform.position;
            followGroupRotation[i] = followGroup.transform.GetChild(i).transform.rotation;
        }
        queueGroupPosition = new Vector3[queueGroup.transform.childCount];
        queueGroupRotation = new Quaternion[queueGroup.transform.childCount];
        for (int i = 0; i < queueGroup.transform.childCount; ++i) {
            queueGroup.transform.GetChild(i).gameObject.SetActive(false);
            queueGroupPosition[i] = queueGroup.transform.GetChild(i).transform.position;
            queueGroupRotation[i] = queueGroup.transform.GetChild(i).transform.rotation;
        }

        SelectionChanged();
    }

    public void OnGUI()
    {
        GUILayout.BeginVertical(GUILayout.Width(300));
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("<-")) {
            prevSelectionType = selectionType;
            selectionType = (BehaviorSelectionType)(((int)selectionType - 1) % (int)BehaviorSelectionType.Last);
            if ((int)selectionType < 0) selectionType = BehaviorSelectionType.Queue;
            SelectionChanged();
        }
        GUILayout.Box(SplitCamelCase(selectionType.ToString()), GUILayout.Width(220));
        if (GUILayout.Button("->")) {
            prevSelectionType = selectionType;
            selectionType = (BehaviorSelectionType)(((int)selectionType + 1) % (int)BehaviorSelectionType.Last);
            SelectionChanged();
        }
        GUILayout.EndHorizontal();
        GUILayout.Box(Description(), descriptionGUISkin.box);
        if (selectionType == BehaviorSelectionType.CanHearObject) {
            if (GUILayout.Button("Play Sound")) {
                marker.GetComponent<AudioSource>().Play();
            }
        }
        GUILayout.EndVertical();
    }

	private string Description()
    {
        string desc = "";
        switch (selectionType) {
            case BehaviorSelectionType.MoveTowards:
                desc = "The Move Towards task will move the agent towards the target (without pathfinding). In this example the green agent is moving towards the red dot.";
                break;
            case BehaviorSelectionType.RotateTowards:
                desc = "The Rotate Towards task rotate the agent towards the target. In this example the green agent is rotating towards the red dot.";
                break;
            case BehaviorSelectionType.Seek:
                desc = "The Seek task will move the agent towards the target with pathfinding. In this example the green agent is seeking the red dot (which moves).";
                break;
            case BehaviorSelectionType.Flee:
                desc = "The Flee task will move the agent away from the target with pathfinding. In this example the green agent is fleeing from red dot (which moves).";
                break;
            case BehaviorSelectionType.Pursue:
                desc = "The Pursue task is similar to the Seek task except the Pursue task predicts where the target is going to be in the future. This allows the agent to arrive at the target earlier than it would have with the Seek task.";
                break;
            case BehaviorSelectionType.Evade:
                desc = "The Evade task is similar to the Flee task except the Evade task predicts where the target is going to be in the future. This allows the agent to flee from the target earlier than it would have with the Flee task.";
                break;
            case BehaviorSelectionType.Patrol:
                desc = "The Patrol task moves from waypoint to waypint. In this example the green agent is patrolling with four different waypoints (the white dots).";
                break;
            case BehaviorSelectionType.Cover:
                desc = "The Cover task will move the agent into cover from its current position. In this example the agent sees cover in front of it so takes cover behind the wall.";
                break;
            case BehaviorSelectionType.Wander:
                desc = "The Wander task moves the agent randomly throughout the map with pathfinding.";
                break;
            case BehaviorSelectionType.Search:
                desc = "The Search task will search the map by wandering until it finds the target. It can find the target by seeing or hearing the target. In this example the Search task is looking for the red dot.";
                break;
            case BehaviorSelectionType.WithinDistance:
                desc = "The Within Distance task is a conditional task that returns success when another object comes within distance of the current agent. In this example the Within Distance task is paired with the Seek task so the agent will move towards the target as soon as the target within distance.";
                break;
            case BehaviorSelectionType.CanSeeObject:
                desc = "The Can See Object task is a conditional task that returns success when it sees an object in front of the current agent. In this example the Can See Object task is paired with the Seek task so the agent will move towards the target as soon as the target is seen.";
                break;
            case BehaviorSelectionType.CanHearObject:
                desc = "The Can Hear Object task is a conditional task that returns success when it hears another object. Press the \"Play\" button to emit a sound from the red dot. If the red dot is within range of the agent when the sound is played then the agent will move towards the red dot with the Seek task.";
                break;
            case BehaviorSelectionType.Flock:
                desc = "The Flock task moves a group of objects together in a pattern (which can be adjusted). In this example the Flock task is controlling all 30 objects. There are no colliders on the objects - the Flock task is completing managing the position of all of the objects";
                break;
            case BehaviorSelectionType.LeaderFollow:
                desc = "The Leader Follow task moves a group of objects behind a leader object. There are two behavior trees running in this example - one for the leader (who is patrolling the area) and one for the group of objects. Again, there is are no colliders on the objects.";
                break;
            case BehaviorSelectionType.Queue:
                desc = "The Queue task will move a group of objects through a small space in an organized way. In this example the Queue task is controlling all of the objects. Another way to move all of the objects through the doorway is with the seek task, however with this approach the objects would group up at the doorway.";
                break;
        }
        return desc;
    }

    private static string SplitCamelCase(string s)
    {
        var r = new Regex(@"(?<=[A-Z])(?=[A-Z][a-z])|(?<=[^A-Z])(?=[A-Z])|(?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace);
        s = r.Replace(s, " ");
        return (char.ToUpper(s[0]) + s.Substring(1)).Trim();
    }

    private void SelectionChanged()
    {
        DisableAll();
        switch (selectionType) {
            case BehaviorSelectionType.MoveTowards:
                marker.transform.position = new Vector3(20, 1, -20);
                marker.SetActive(true);
                mainBot.transform.position = new Vector3(-20, 1, -20);
                mainBot.transform.eulerAngles = new Vector3(0, 180, 0);
                mainBot.SetActive(true);
                break;
            case BehaviorSelectionType.RotateTowards:
                marker.transform.position = new Vector3(20, 1, 10);
                marker.SetActive(true);
                mainBot.transform.position = new Vector3(0, 1, -20);
                mainBot.transform.eulerAngles = new Vector3(0, 180, 0);
                mainBot.SetActive(true);
                break;
            case BehaviorSelectionType.Seek:
                marker.transform.position = new Vector3(20, 1, 20);
                marker.SetActive(true);
                marker.GetComponent<Animation>()["MarkerSeek"].time = 0;
                marker.GetComponent<Animation>()["MarkerSeek"].speed = 1;
                marker.GetComponent<Animation>().Play("MarkerSeek");
                mainBot.transform.position = new Vector3(-20, 1, -20);
                mainBot.transform.eulerAngles = new Vector3(0, 180, 0);
                mainBot.SetActive(true);
                break;
            case BehaviorSelectionType.Flee:
                marker.transform.position = new Vector3(20, 1, 20);
                marker.SetActive(true);
                marker.GetComponent<Animation>()["MarkerFlee"].time = 0;
                marker.GetComponent<Animation>()["MarkerFlee"].speed = 1;
                marker.GetComponent<Animation>().Play("MarkerFlee");
                mainBot.transform.position = new Vector3(10, 1, 18);
                mainBot.transform.eulerAngles = new Vector3(0, 180, 0);
                mainBot.SetActive(true);
                break;
            case BehaviorSelectionType.Pursue:
                marker.transform.position = new Vector3(20, 1, 20);
                marker.SetActive(true);
                marker.GetComponent<Animation>()["MarkerPersue"].time = 0;
                marker.GetComponent<Animation>()["MarkerPersue"].speed = 1;
                marker.GetComponent<Animation>().Play("MarkerPersue");
                mainBot.transform.position = new Vector3(-20, 1, 0);
                mainBot.transform.eulerAngles = new Vector3(0, 90, 0);
                mainBot.SetActive(true);
                break;
            case BehaviorSelectionType.Evade:
                marker.transform.position = new Vector3(20, 1, 20);
                marker.SetActive(true);
                marker.GetComponent<Animation>()["MarkerEvade"].time = 0;
                marker.GetComponent<Animation>()["MarkerEvade"].speed = 1;
                marker.GetComponent<Animation>().Play("MarkerEvade");
                mainBot.transform.position = new Vector3(0, 1, 18);
                mainBot.transform.eulerAngles = new Vector3(0, 180, 0);
                mainBot.SetActive(true);
                break;
            case BehaviorSelectionType.Patrol:
                for (int i = 0; i < waypoints.Length; ++i) {
                    waypoints[i].SetActive(true);
                }
                mainBot.transform.position = new Vector3(-20, 1, 20);
                mainBot.transform.eulerAngles = new Vector3(0, 180, 0);
                mainBot.SetActive(true);
                break;
            case BehaviorSelectionType.Cover:
                mainBot.transform.position = new Vector3(-5, 1, -10);
                mainBot.transform.eulerAngles = new Vector3(0, 0, 0);
                mainBot.SetActive(true);
                break;
            case BehaviorSelectionType.Wander:
                mainBot.transform.position = new Vector3(-20, 1, -20);
                mainBot.transform.eulerAngles = new Vector3(0, 0, 0);
                mainBot.SetActive(true);
                break;
            case BehaviorSelectionType.Search:
                marker.transform.position = new Vector3(20, 1, 20);
                marker.SetActive(true);
                mainBot.transform.position = new Vector3(-20, 1, -20);
                mainBot.transform.eulerAngles = new Vector3(0, 0, 0);
                mainBot.SetActive(true);
                break;
            case BehaviorSelectionType.WithinDistance:
                marker.transform.position = new Vector3(20, 1, 20);
                marker.GetComponent<Animation>()["MarkerPersue"].time = 0;
                marker.GetComponent<Animation>()["MarkerPersue"].speed = 1;
                marker.GetComponent<Animation>().Play("MarkerPersue");
                marker.SetActive(true);
                mainBot.transform.position = new Vector3(-15, 1, 2);
                mainBot.transform.eulerAngles = new Vector3(0, 0, 0);
                mainBot.SetActive(true);
                break;
            case BehaviorSelectionType.CanSeeObject:
                marker.transform.position = new Vector3(20, 1, 20);
                marker.GetComponent<Animation>()["MarkerPersue"].time = 0;
                marker.GetComponent<Animation>()["MarkerPersue"].speed = 1;
                marker.GetComponent<Animation>().Play("MarkerPersue");
                marker.SetActive(true);
                mainBot.transform.position = new Vector3(-15, 1, -10);
                mainBot.transform.eulerAngles = new Vector3(0, 0, 0);
                mainBot.SetActive(true);
                break;
            case BehaviorSelectionType.CanHearObject:
                marker.transform.position = new Vector3(20, 1, 20);
                marker.GetComponent<Animation>()["MarkerPersue"].time = 0;
                marker.GetComponent<Animation>()["MarkerPersue"].speed = 1;
                marker.GetComponent<Animation>().Play("MarkerPersue");
                marker.SetActive(true);
                mainBot.transform.position = new Vector3(-15, 1, -10);
                mainBot.transform.eulerAngles = new Vector3(0, 0, 0);
                mainBot.SetActive(true);
                break;
            case BehaviorSelectionType.Flock:
                Camera.main.transform.position = new Vector3(0, 90, -150);
                for (int i = 0; i < flockGroup.transform.childCount; ++i) {
                    flockGroup.transform.GetChild(i).gameObject.SetActive(true);
                }
                break;
            case BehaviorSelectionType.LeaderFollow:
                for (int i = 0; i < waypointsA.Length; ++i) {
                    waypointsA[i].SetActive(true);
                }
                mainBot.transform.position = new Vector3(0, 1, -130);
                mainBot.SetActive(true);
                Camera.main.transform.position = new Vector3(0, 90, -150);
                for (int i = 0; i < followGroup.transform.childCount; ++i) {
                    followGroup.transform.GetChild(i).gameObject.SetActive(true);
                }
                break;
            case BehaviorSelectionType.Queue:
                marker.transform.position = new Vector3(45, 1, 0);
                for (int i = 0; i < queueGroup.transform.childCount; ++i) {
                    queueGroup.transform.GetChild(i).gameObject.SetActive(true);
                }
                break;
        }
        StartCoroutine("EnableBehavior");
    }

    private void DisableAll()
    {
        StopCoroutine("EnableBehavior");
        behaviorTreeGroup[(int)prevSelectionType].DisableBehavior();
        // enable the leader as well
        if (prevSelectionType == BehaviorSelectionType.LeaderFollow) {
            behaviorTreeGroup[(int)BehaviorSelectionType.Last].DisableBehavior();
        }
        marker.GetComponent<Animation>().Stop();
        marker.SetActive(false);
        mainBot.SetActive(false);
        Camera.main.transform.position = new Vector3(0, 90, 0);
        for (int i = 0; i < flockGroup.transform.childCount; ++i) {
            flockGroup.transform.GetChild(i).gameObject.SetActive(false);
            flockGroup.transform.GetChild(i).transform.position = flockGroupPosition[i];
            flockGroup.transform.GetChild(i).transform.rotation = flockGroupRotation[i];
        }
        for (int i = 0; i < followGroup.transform.childCount; ++i) {
            followGroup.transform.GetChild(i).gameObject.SetActive(false);
            followGroup.transform.GetChild(i).transform.position = followGroupPosition[i];
            followGroup.transform.GetChild(i).transform.rotation = followGroupRotation[i];
        }
        for (int i = 0; i < queueGroup.transform.childCount; ++i) {
            queueGroup.transform.GetChild(i).gameObject.SetActive(false);
            queueGroup.transform.GetChild(i).transform.position = queueGroupPosition[i];
            queueGroup.transform.GetChild(i).transform.rotation = queueGroupRotation[i];
        }
        for (int i = 0; i < waypoints.Length; ++i) {
            waypoints[i].SetActive(false);
        }
        for (int i = 0; i < waypointsA.Length; ++i) {
            waypointsA[i].SetActive(false);
        }
    }

    private IEnumerator EnableBehavior()
    {
        yield return new WaitForSeconds(0.5f);

        behaviorTreeGroup[(int)selectionType].EnableBehavior();

        // enable the leader as well
        if (selectionType == BehaviorSelectionType.LeaderFollow) {
            behaviorTreeGroup[(int)BehaviorSelectionType.Last].EnableBehavior();
        }
    }
}
