using Unity.MLAgents.SideChannels;
using System;

[Serializable]
public class DQNValueSideChannel : SideChannel
{
    public float LatestValue = 0f;

    public DQNValueSideChannel()
    {
        ChannelId = new Guid("11111111-1111-1111-1111-111111111111");
    }

    protected override void OnMessageReceived(IncomingMessage msg)
    {
        LatestValue = msg.ReadFloat32();
    }

    public void SendValue(float value)
    {
        using (var msg = new OutgoingMessage())
        {
            msg.WriteFloat32(value);
            QueueMessageToSend(msg);
        }
    }
}
