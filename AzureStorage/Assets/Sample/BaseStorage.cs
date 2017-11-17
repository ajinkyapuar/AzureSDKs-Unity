using Microsoft.WindowsAzure.Storage;
using UnityEngine;
using UnityEngine.UI;
using System.Net;

public class BaseStorage : MonoBehaviour
{
	public string ConnectionString = string.Empty;

	protected CloudStorageAccount StorageAccount;
	private Text _myText;

	// Use this for initialization
	void Start ()
	{
#if UNITY_ANDROID || UNITY_IOS || UNITY_STANDALONE_OSX
		ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
#endif

		_myText = GameObject.Find("DebugText").GetComponent<Text>();
		StorageAccount = CloudStorageAccount.Parse(ConnectionString);
	}

	public void ClearOutput()
	{
		_myText.text = string.Empty;
	}

	public void WriteLine(string s)
	{
		if(_myText.text.Length > 20000)
			_myText.text = string.Empty + "-- TEXT OVERFLOW --";

		_myText.text += s + "\r\n";
	}
}
