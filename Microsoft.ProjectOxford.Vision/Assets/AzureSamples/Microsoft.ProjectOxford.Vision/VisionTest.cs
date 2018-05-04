using System;
using System.IO;
using System.Linq;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using UnityEngine;

#if WINDOWS_UWP && ENABLE_DOTNET
using Windows.Storage;
using Windows.Storage.Streams;
#endif

public class VisionTest : BaseVision
{
	private static VisionServiceClient visionClient;

	public string ImageToUpload = "islands.png";

	// Use this for initialization
	public async void TestVision()
	{
		try
		{
			VisionServiceClient client = new VisionServiceClient(VisionKey, VisionEndpoint);
			AnalysisResult result = await client.DescribeAsync("https://upload.wikimedia.org/wikipedia/commons/7/76/Las_Vegas_strip.jpg");
			WriteOutput(result);
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

	public async void TestVisionFile()
	{
		Stream s;

		try
		{
#if WINDOWS_UWP && ENABLE_DOTNET
			StorageFolder storageFolder = await StorageFolder.GetFolderFromPathAsync(Application.streamingAssetsPath.Replace('/', '\\'));
			StorageFile sf = await storageFolder.GetFileAsync(ImageToUpload);
			IRandomAccessStreamWithContentType ras = await sf.OpenReadAsync();
			s = ras.AsStreamForRead();
#else
			s = new FileStream(Path.Combine(Application.streamingAssetsPath, ImageToUpload), FileMode.Open);
#endif

			VisionServiceClient client = new VisionServiceClient(VisionKey, VisionEndpoint);
			AnalysisResult result = await client.DescribeAsync(s);
			WriteOutput(result);
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

	private void WriteOutput(AnalysisResult result)
	{
		WriteLine(result.Description.Captions[0].Text);
		string tags = result.Description.Tags.Aggregate((x, y) => x + ", " + y);
		WriteLine(tags);
	}
}
