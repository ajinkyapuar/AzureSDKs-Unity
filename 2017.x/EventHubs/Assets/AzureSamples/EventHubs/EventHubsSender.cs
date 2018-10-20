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
#if !UNITY_WSA
using System.Security.Cryptography.X509Certificates;
#endif

public class EventHubsSender : BaseEventHubs
{

    private static EventHubClient eventHubClient;
    
    [HideInInspector]
    public Text DebugText;

#if !UNITY_WSA
    private class CustomCertificatePolicy : ICertificatePolicy
    {
        public bool CheckValidationResult(ServicePoint sp,
            X509Certificate certificate, WebRequest request, int error)
        {
            return true;
        }
    }
#endif

    // Use this for initialization
    public async void TestEventHubsSender()
    {
        try
        {
#if !UNITY_WSA
            //Unity will complain that the following statement is deprecated
            //however, it's working :)
            ServicePointManager.CertificatePolicy = new CustomCertificatePolicy();
            
            //this 'workaround' seems to work for the .NET Storage SDK, but not here. Leaving this for clarity
            //ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
#endif

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
                WriteLine($"{DateTime.Now} > Exception: {exception.Message}");
                //something happened so exit the loop
                break;
            }

            await Task.Delay(10);
        }

        WriteLine($"{numMessagesToSend} messages sent.");
    }
}
