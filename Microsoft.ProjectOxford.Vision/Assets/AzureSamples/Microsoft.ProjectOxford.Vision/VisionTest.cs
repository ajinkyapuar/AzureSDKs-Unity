using System;
using System.Linq;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;

public class VisionTest : BaseVision
{
    private static VisionServiceClient visionClient;
    
    // Use this for initialization
    public async void TestVision()
    {
        try
        {
			VisionServiceClient client = new VisionServiceClient(VisionKey, VisionEndpoint);
			AnalysisResult result = await client.DescribeAsync("https://upload.wikimedia.org/wikipedia/commons/f/f3/ISS-52_Aurora_australis_above_Antarctica.jpg");
			WriteLine(result.Description.Captions[0].Text);
			string tags = result.Description.Tags.Aggregate((x, y) => x + ", " + y);
			WriteLine(tags);
        }
        catch (ClientException ex)
        {
            WriteLine(ex.Error.Message);
        }
        catch (Exception ex)
        {
            WriteLine(ex.ToString());
        }
    }
}
