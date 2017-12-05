using UnityEngine;
using UnityEngine.UI;

public class BaseEventHubs : MonoBehaviour
{
    public string EhConnectionString;
    public string EhEntityPath;

    private Text _myText;

	// Use this for initialization
	void Start ()
	{
		_myText = GameObject.Find("DebugText").GetComponent<Text>();
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
