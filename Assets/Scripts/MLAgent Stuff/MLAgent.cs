using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine.UI;
using System.IO;

/// <summary>
/// This class is used to implement the MLAgent IA in the game
/// </summary>
public class MLAgent : Agent
{
    private Text _rewardText;
    private Environment _currentEnvironment;
    /// <summary>
    /// The current environment of the agent to use <see cref="Environment.Reset"/>
    /// </summary>
    public Environment CurrentEnvironment { get => _currentEnvironment; set => _currentEnvironment = value; }

    private Mario _currentMario;
    /// <summary>
    /// The script that control mario and the custom physic, to use <see cref="Mario.MLMoveMario"/>
    /// </summary>
    public Mario CurrentMario { get => _currentMario; set => _currentMario = value; }

    public override void OnEpisodeBegin()
    {
        CurrentEnvironment.Reset();
    }


    private void Start()
    {
        _rewardText = GameObject.Find("Reward").GetComponent<Text>();
    }

    /// <summary>
    /// Use the <see cref="Mario.MLMoveMario"/> function to control <see cref="Mario"/> based on <paramref name="actions"/>
    /// </summary>
    /// <param name="actions"></param>
    public override void OnActionReceived(ActionBuffers actions)
    {
        int dir = actions.DiscreteActions[0]; //direction
        if (dir == 2)
            dir = -1;

        int run = actions.DiscreteActions[1]; //run

        int jump = actions.DiscreteActions[2]; //jump

        CurrentMario.MLMoveMario(dir, run, jump);

        AddReward(-0.0025f);
        if (GetComponent<Rigidbody2D>().velocity.x > 0)
        {
            if (_currentMario.CurrentVelocityX == Mario.VelocityX.course)
                AddReward(0.002f);
            else
                AddReward(0.0015f);
        }
    }

    /// <summary>
    /// This heuristic method is used to control the agent when he is driven by the IA
    /// </summary>
    /// <param name="actionsOut"></param>
    public override void Heuristic(in ActionBuffers actionsOut) //Control the player with the keyboard
    {
        ActionSegment<int> discreteAction = actionsOut.DiscreteActions;
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)) //Marche
        {
            if (Input.GetKey(KeyCode.LeftArrow)) //Direction
                discreteAction[0] = 2;
            else
                discreteAction[0] = 1;
        }
        else
            discreteAction[0] = 0;

        if (Input.GetKey(KeyCode.Space)) //run
            discreteAction[1] = 1;
        else
            discreteAction[1] = 0;

        if (Input.GetKey(KeyCode.UpArrow)) //jump
            discreteAction[2] = 1;
        else
            discreteAction[2] = 0;
    }

    private void FixedUpdate()
    {
        _rewardText.text = GetCumulativeReward().ToString();
    }

    public void CustomDeath()
    {
        AddReward(-10f);
        CustomEndEpisode();
        _currentEnvironment.Reset();
    }

    public void GetReward(float amount)
    {
        AddReward(amount);
    }

    public void FlagTouch(float position)
    {
        AddReward(5f + position);
        CustomEndEpisode();
    }

    private void CustomEndEpisode()
    {
        var statsRecorder = Academy.Instance.StatsRecorder;
        statsRecorder.Add("Mario/" + _currentEnvironment.name + "/Distance_reached", transform.position.x, StatAggregationMethod.Histogram);
        EndEpisode();
    }
}
