using System;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

namespace helloworld
{
    class Program
    {
        static void Main(string[] args)
        {
            CustomizedSpeechRecognizer recognizer = new CustomizedSpeechRecognizer("bc86772390394a8eb56b564baa98de62", "westus");
            while (true)
            {
                Console.WriteLine("please select 1. short speech 2. continous speech 3. audio input");
                Console.WriteLine("4. audio input with pull stream 5. chinese short speech");
                var option = Console.ReadLine();
                if (option.StartsWith("1"))
                {
                    recognizer.RecognizeShortSpeechAsync().Wait();
                }
                else if (option.StartsWith("2"))
                {
                    recognizer.SpeechContinuousRecognitionAsync().Wait();
                }
                else if (option.StartsWith("3"))
                {
                    recognizer.ShortSpeechRecognitionWithFile("whatstheweatherlike.wav").Wait();
                }
                else if (option.StartsWith("4"))
                {
                    recognizer.ShortSpeechRecognitionWithPullStream("whatstheweatherlike.wav").Wait();
                }
                else if (option.StartsWith("5"))
                {
                    recognizer.RecognizeShortSpeechAsync("zh-CN").Wait();
                }
                else
                {
                    break;
                }
                //Console.WriteLine("Please press a key to continue.");
                //Console.ReadLine();
            }

            recognizer.Close();
        }
    }
}
