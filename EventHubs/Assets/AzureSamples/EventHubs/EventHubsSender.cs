using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Net;
using System.Net.Security;
using UnityEngine.UI;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.EventHubs.Amqp;

public class EventHubsSender : BaseEventHubs
{
    private static EventHubClient eventHubClient;
    
    [HideInInspector]
    public Text DebugText;

    // Use this for initialization
    public async void TestEventHubsSender()
    {
        try
        {
            var connectionStringBuilder = new EventHubsConnectionStringBuilder(EhConnectionString)
            {
                EntityPath = EhEntityPath
            };

            eventHubClient = EventHubClient.CreateFromConnectionString(connectionStringBuilder.ToString());
            await SendMessagesToEventHub(10);

        }
        catch (Exception ex)
        {
            WriteLine(ex.Message);
        }
        finally
        {
            await eventHubClient.CloseAsync();
        }
    }

    private async Task SendMessagesToEventHub(int numMessagesToSend)
    {
        for (var i = 0; i < numMessagesToSend; i++)
        {
            try
            {
                var message = $"Custom message from Unity {i} at {DateTime.Now}";
                WriteLine($"Sending message: {message}");
                await eventHubClient.SendAsync(new EventData(Encoding.UTF8.GetBytes(message)));
            }
            catch (Exception exception)
            {
                WriteLine($"{DateTime.Now} > Exception: {exception}");
                Debug.Log(exception);
                return;
            }

            await Task.Delay(10);
        }

        WriteLine($"{numMessagesToSend} messages sent.");
    }
}
