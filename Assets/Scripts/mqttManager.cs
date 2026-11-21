using System.Collections.Generic;
using UnityEngine;
using M2MqttUnity;
using uPLibrary.Networking.M2Mqtt.Messages;

public class mqttObj
{
    public string msg;
    public string topic;
}

public class mqttManager : M2MqttUnityClient
{
    [Header("MQTT Configuration")]
    public List<string> topicSubscribe = new List<string> { "Kipas/Suhu" };

    public event OnMessageArrivedDelegate OnMessageArrived;
    public delegate void OnMessageArrivedDelegate(mqttObj mqttObject);

    private mqttObj lastMessage = new mqttObj();

    protected override void SubscribeTopics()
    {
        foreach (string topic in topicSubscribe)
        {
            client.Subscribe(new string[] { topic }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        }
    }

    protected override void UnsubscribeTopics()
    {
        foreach (string topic in topicSubscribe)
        {
            client.Unsubscribe(new string[] { topic });
        }
    }

    protected override void DecodeMessage(string topic, byte[] message)
    {
        string msg = System.Text.Encoding.UTF8.GetString(message);
        lastMessage.msg = msg;
        lastMessage.topic = topic;

        Debug.Log("MQTT Received: " + msg + " from " + topic);

        if (OnMessageArrived != null)
        {
            OnMessageArrived(lastMessage);
        }
    }

    private void OnDestroy()
    {
        Disconnect();
    }
}