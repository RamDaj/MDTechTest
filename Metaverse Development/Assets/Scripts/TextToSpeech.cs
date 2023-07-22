using Amazon;
using Amazon.Polly;
using Amazon.Polly.Model;
using Amazon.Runtime;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class TextToSpeech : MonoBehaviour
{

    public AudioSource audioSource;
    [SerializeField] TalkingGirl girl;

    private void Start()
    {
        girl = gameObject.transform.parent.GetComponent<TalkingGirl>();
    }

    public async void createAudioClip(string textToConvert)
    {	
		#TODO: Replace credentials with your own.
        var credentials = new BasicAWSCredentials("Key", "Secret Key");
        var client = new AmazonPollyClient(credentials, RegionEndpoint.USEast1);

        //Create request to Amazon Polly
        var request = new SynthesizeSpeechRequest()
        {
            Text = textToConvert,
            Engine = Engine.Neural,
            VoiceId = VoiceId.Kendra,
            OutputFormat = OutputFormat.Mp3
        };

        var response = await client.SynthesizeSpeechAsync(request);

        //Writes the response into an audio file
        WriteIntoFile(response.AudioStream);

        //Use the previously obtained file
        using (var www = UnityWebRequestMultimedia.GetAudioClip($"{Application.persistentDataPath}/audio.mp3", AudioType.MPEG))
        {
            var op = www.SendWebRequest();

            while (!op.isDone) await Task.Yield();

            var clip = DownloadHandlerAudioClip.GetContent(www);

            audioSource.clip = clip;
            playAudio();
        }
    }

    public void playAudio()
    {
        audioSource.Play();
        StartCoroutine(girl.waitForSound());
    }

    private void WriteIntoFile(Stream stream)
    {
        using (var fileStream = new FileStream($"{Application.persistentDataPath}/audio.mp3", FileMode.Create))
        {
            byte[] buffer = new byte[4 * 1024];
            int bytesRead;
            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                fileStream.Write(buffer, 0, bytesRead);
            }
        }
    }
}
