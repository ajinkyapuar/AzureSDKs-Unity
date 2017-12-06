using Microsoft.ProjectOxford.Common.Contract;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

#if !UNITY_UWP
using System.Security.Cryptography.X509Certificates;
#endif



public class CognitiveFaceDemo : MonoBehaviour
{
    public string ImageToUpload = "Satya.jpg";
    public string FaceApiKey = string.Empty;
    public string FaceApiUri = "https://westcentralus.api.cognitive.microsoft.com/face/v1.0";
    private IFaceServiceClient faceServiceClient;

    void Awake()
    {
        Debug.Log("Ensure your Face API Key was generated for the region of your Face API URI or it might not work!!");
        faceServiceClient = new FaceServiceClient(FaceApiKey, FaceApiUri);
#if !UNITY_WSA
        //This works, and one of these two options are required as Unity's TLS (SSL) support doesn't currently work like .NET
        //ServicePointManager.CertificatePolicy = new CustomCertificatePolicy();

        //This 'workaround' seems to work for the .NET Storage SDK, but not event hubs. 
        //If you need all of it (ex Storage, event hubs,and app insights) then consider using the above.
        //If you don't want to check an SSL certificate coming back, simply use the return true delegate below.
        //Also it may help to use non-ssl URIs if you have the ability to, until Unity fixes the issue (which may be fixed by the time you read this)
        ServicePointManager.ServerCertificateValidationCallback = CheckValidCertificateCallback; //delegate { return true; };
#endif
    }

    public bool CheckValidCertificateCallback(System.Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        bool valid = true;

        // If there are errors in the certificate chain, look at each error to determine the cause.
        if (sslPolicyErrors != SslPolicyErrors.None)
        {
            for (int i = 0; i < chain.ChainStatus.Length; i++)
            {
                if (chain.ChainStatus[i].Status != X509ChainStatusFlags.RevocationStatusUnknown)
                {
                    chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
                    chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
                    chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 1, 0);
                    chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
                    bool chainIsValid = chain.Build((X509Certificate2)certificate);
                    if (!chainIsValid)
                    {
                        valid = false;
                    }
                }
            }
        }
        return valid;
    }



    // Uploads the image file and calls Detect Faces.
    private async Task<Face[]> UploadAndDetectFaces(string imageFilePath)
    {
        // The list of Face attributes to return.
        IEnumerable<FaceAttributeType> faceAttributes =
            new FaceAttributeType[] { FaceAttributeType.Gender, FaceAttributeType.Age, FaceAttributeType.Smile, FaceAttributeType.Emotion, FaceAttributeType.Glasses, FaceAttributeType.Hair };

        // Call the Face API.
        try
        {
            using (Stream imageFileStream = File.OpenRead(imageFilePath))
            {
                Face[] faces = await faceServiceClient.DetectAsync(imageFileStream, returnFaceId: true, returnFaceLandmarks: false, returnFaceAttributes: faceAttributes);
                return faces;
            }
        }
        // Catch and display Face API errors.
        catch (FaceAPIException f)
        {
            Debug.LogError($"Error Message:{f.ErrorMessage} ErrorCode:{f.ErrorCode}");
            return null;
        }
        // Catch and display all other errors.
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
            return null;
        }
    }

    public async void AnalyzeImage()
    {
        // Detect any faces in the image.
        Debug.Log("Detecting...");
        var faces = await UploadAndDetectFaces(Path.Combine(Application.streamingAssetsPath, ImageToUpload));
        var title = String.Format("Detection Finished. {0} face(s) detected", faces.Length);

        if (faces.Length > 0)
        {
            for (int i = 0; i < faces.Length; ++i)
            {
                Face face = faces[i];

                // Draw a rectangle on the face.
                Debug.Log(string.Format("Face: L:{0} T:{1} W:{2} H:{3}",
                        face.FaceRectangle.Left,
                        face.FaceRectangle.Top,
                        face.FaceRectangle.Width,
                        face.FaceRectangle.Height)
                );

                // Store the face description.
                Debug.Log(FaceDescription(face));
            }

        }
    }
    private string FaceDescription(Face face)
    {
        var sb = new StringBuilder();

        sb.Append("Face: ");

        // Add the gender, age, and smile.
        sb.Append(face.FaceAttributes.Gender);
        sb.Append(", ");
        sb.Append(face.FaceAttributes.Age);
        sb.Append(", ");
        sb.Append(String.Format("smile {0:F1}%, ", face.FaceAttributes.Smile * 100));

        // Add the emotions. Display all emotions over 10%.
        sb.Append("Emotion: ");
        EmotionScores emotionScores = face.FaceAttributes.Emotion;
        if (emotionScores.Anger >= 0.1f) sb.Append(String.Format("anger {0:F1}%, ", emotionScores.Anger * 100));
        if (emotionScores.Contempt >= 0.1f) sb.Append(String.Format("contempt {0:F1}%, ", emotionScores.Contempt * 100));
        if (emotionScores.Disgust >= 0.1f) sb.Append(String.Format("disgust {0:F1}%, ", emotionScores.Disgust * 100));
        if (emotionScores.Fear >= 0.1f) sb.Append(String.Format("fear {0:F1}%, ", emotionScores.Fear * 100));
        if (emotionScores.Happiness >= 0.1f) sb.Append(String.Format("happiness {0:F1}%, ", emotionScores.Happiness * 100));
        if (emotionScores.Neutral >= 0.1f) sb.Append(String.Format("neutral {0:F1}%, ", emotionScores.Neutral * 100));
        if (emotionScores.Sadness >= 0.1f) sb.Append(String.Format("sadness {0:F1}%, ", emotionScores.Sadness * 100));
        if (emotionScores.Surprise >= 0.1f) sb.Append(String.Format("surprise {0:F1}%, ", emotionScores.Surprise * 100));

        // Add glasses.
        sb.Append(face.FaceAttributes.Glasses);
        sb.Append(", ");

        // Add hair.
        sb.Append("Hair: ");

        // Display baldness confidence if over 1%.
        if (face.FaceAttributes.Hair.Bald >= 0.01f)
            sb.Append(String.Format("bald {0:F1}% ", face.FaceAttributes.Hair.Bald * 100));

        // Display all hair color attributes over 10%.
        HairColor[] hairColors = face.FaceAttributes.Hair.HairColor;
        foreach (HairColor hairColor in hairColors)
        {
            if (hairColor.Confidence >= 0.1f)
            {
                sb.Append(hairColor.Color.ToString());
                sb.Append(String.Format(" {0:F1}% ", hairColor.Confidence * 100));
            }
        }


        // Return the built string.
        return sb.ToString();
    }

}
