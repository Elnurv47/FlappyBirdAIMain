using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Unity.MLAgents;
using System;

public class BirdManager : MonoBehaviour
{
    public static BirdManager Instance;
    private List<BirdAgent> allBirds = new List<BirdAgent>();
    private int birdsFallen = 0;

    private SimpleMultiAgentGroup agentGroup;
    public SimpleMultiAgentGroup AgentGroup { get => agentGroup; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        agentGroup = new SimpleMultiAgentGroup();
    }

    public void RegisterBird(BirdAgent bird)
    {
        if (!allBirds.Contains(bird))
        {
            allBirds.Add(bird);
        }
        agentGroup.RegisterAgent(bird);
    }

    public void BirdFailed(BirdAgent fallenBird)
    {
        birdsFallen++;
        fallenBird.gameObject.SetActive(false);

        // Check if all birds have fallen and episode hasn't been ended yet
        if (birdsFallen >= allBirds.Count)
        {
            agentGroup.EndGroupEpisode();  // End the episode for all agents
            birdsFallen = 0;  // Reset the fallen birds count
            PipeSpawner.DestroyAllPipes();
        }
    }

    public void SetActive(GameObject obj)
    {
        obj.SetActive(true);
    }
}
