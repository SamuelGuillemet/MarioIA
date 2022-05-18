using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    /// <summary>
    /// The maximum number of steps before <see cref="Agent.EpisodeInterrupted"/> is called
    /// </summary>
    private float _maxStep;

    public override void OnEpisodeBegin()
    {
        CurrentEnvironment.Reset();
    }


    private void Start()
    {
        _rewardText = GameObject.Find("Reward").GetComponent<Text>();

        foreach (Checkpoint item in CurrentEnvironment.CheckpointList)
        {
            Physics2D.IgnoreCollision(item.GetComponent<Collider2D>(), gameObject.GetComponent<BoxCollider2D>());
        }

        _maxStep = 25 * CurrentEnvironment.FlagTransform.localPosition.x;
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


        if (GetComponent<Rigidbody2D>().velocity.x > 0.1f)
        {
            if (_currentMario.CurrentVelocityX == Mario.VelocityX.course)
                AddReward(-0.00015f);
            else
                AddReward(-0.001f);
        }
        else
        {
            AddReward(-0.003f);
        }

        if (jump == 1)
        {
            AddReward(0.001f);
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
        if (StepCount > 500 && (CurrentEnvironment.gameObject.name.Contains("Initializer") || SceneManager.GetActiveScene().name.Contains("Training")) || StepCount > _maxStep)
        {
            if ((Mathf.Abs(transform.localPosition.x - CurrentEnvironment.MarioInitPositionX) < 2f) && !CurrentEnvironment.gameObject.name.Contains("Initializer"))
                AddReward(-5f);
            SaveDataToTensorboard();
            EpisodeInterrupted();
        }
    }

    /// <summary>
    /// This is called when we want to make the IA die after he has to
    /// </summary>
    public void CustomDeath()
    {
        AddReward(-5f);
        SaveDataToTensorboard();
        EndEpisode();
        _currentEnvironment.Reset();
    }

    /// <summary>
    /// This function gives reward to the IA
    /// </summary>
    /// <param name="amount">Amount of reward to give to the IA</param>
    public void GetReward(float amount)
    {
        AddReward(amount);
    }

    /// <summary>
    /// This fucntion is called when <see cref="Mario"/> touches the flag
    /// </summary>
    /// <param name="position">Position of the collision to increase the value of the reward</param>
    public void FlagTouch(float position)
    {
        AddReward(5f + position);
        SaveDataToTensorboard();
        EndEpisode();
    }

    /// <summary>
    /// This function is used to add data to the tensorboard to monitor the position of the IA
    /// </summary>
    private void SaveDataToTensorboard()
    {
        var statsRecorder = Academy.Instance.StatsRecorder;
        statsRecorder.Add("MarioDistance/" + _currentEnvironment.name + "/Distance_reached", ((int)Mathf.Floor(transform.localPosition.x)), StatAggregationMethod.Histogram);
        statsRecorder.Add("MarioReward/" + _currentEnvironment.name + "/Collected_Reward", ((int)Mathf.Floor(GetCumulativeReward())), StatAggregationMethod.Histogram);
        //Debug.Log(((int)Mathf.Floor(transform.localPosition.x)) + " : " + ((int)Mathf.Floor(GetCumulativeReward())));
    }

    /// <summary>
    /// This fucntion is called essentially when <see cref="MLAgent"/> enter a <see cref="Checkpoint"/>
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Checkpoint")
            other.gameObject.GetComponent<Checkpoint>().EnterCheckpoint();
    }
}
