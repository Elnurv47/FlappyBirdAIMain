using UnityEngine;

public class MultiAgentAcademy : MonoBehaviour
{
    public BirdAgent[] birdAgents;

    void Start()
    {
        birdAgents = FindObjectsOfType<BirdAgent>();
    }

    public void AddGroupReward(float reward)
    {
        foreach (BirdAgent agent in birdAgents)
        {
            agent.AddReward(reward);
        }
    }
}
