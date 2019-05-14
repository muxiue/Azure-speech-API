using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace helloworld
{
    public class CustomizedSpeechRecognizer
    {
        private SpeechConfig m_config;

        public CustomizedSpeechRecognizer(string subscriptionKey, string region)
        {
            m_config = SpeechConfig.FromSubscription(subscriptionKey, region);
        }

        public async Task RecognizeShortSpeechAsync()
        {
            Console.WriteLine("-------------------------Short Speech Start------------------------");
            Console.WriteLine("Say something...");

            // silence for 15 seconds.
            using (var recognizer = new SpeechRecognizer(m_config))
            {
                var result = await recognizer.RecognizeOnceAsync();

                if (result.Reason == ResultReason.RecognizedSpeech)
                {
                    Console.WriteLine($"We recognized: {result.Text}");
                }
                else if (result.Reason == ResultReason.NoMatch)
                {
                    Console.WriteLine($"NOMATCH: Speech could not be recognized.");
                }
                else if (result.Reason == ResultReason.Canceled)
                {
                    var cancellation = CancellationDetails.FromResult(result);
                    Console.WriteLine($"CANCELED: Reason={cancellation.Reason}");

                    if (cancellation.Reason == CancellationReason.Error)
                    {
                        Console.WriteLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
                        Console.WriteLine($"CANCELED: ErrorDetails={cancellation.ErrorDetails}");
                        Console.WriteLine($"CANCELED: Did you update the subscription info?");
                    }
                }
            }
            Console.WriteLine("-------------------------Short Speech End--------------------------");
        }

        public async Task RecognizeShortSpeechAsync(string language)
        {
            Console.WriteLine($"-------------------------{language} Short Speech Start------------------------");
            Console.WriteLine("Say something...");

            var config = SpeechConfig.FromSubscription(m_config.SubscriptionKey, m_config.Region);
            config.SpeechRecognitionLanguage = language;
            config.OutputFormat = OutputFormat.Detailed;
            using (var recognizer = new SpeechRecognizer(config))
            {
                // silence for 15 seconds.
                var result = await recognizer.RecognizeOnceAsync();

                if (result.Reason == ResultReason.RecognizedSpeech)
                {
                    Console.WriteLine($"We recognized: {result.Text}");
                }
                else if (result.Reason == ResultReason.NoMatch)
                {
                    Console.WriteLine($"NOMATCH: Speech could not be recognized.");
                }
                else if (result.Reason == ResultReason.Canceled)
                {
                    var cancellation = CancellationDetails.FromResult(result);
                    Console.WriteLine($"CANCELED: Reason={cancellation.Reason}");

                    if (cancellation.Reason == CancellationReason.Error)
                    {
                        Console.WriteLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
                        Console.WriteLine($"CANCELED: ErrorDetails={cancellation.ErrorDetails}");
                        Console.WriteLine($"CANCELED: Did you update the subscription info?");
                    }
                }
            }
            Console.WriteLine($"-------------------------{language} Short Speech End--------------------------");
            
        }

        public async Task SpeechContinuousRecognitionAsync()
        {
            Console.WriteLine("-------------------------Continous Speech Start------------------------");
            //var stopRecognition = new TaskCompletionSource<int>();

            using (var recognizer = new SpeechRecognizer(m_config))
            {
                recognizer.Recognizing += (s, e) =>
                {
                    Console.WriteLine($"RECOGNIZING: Text={e.Result.Text}");
                };

                recognizer.Recognized += (s, e) =>
                {
                    var result = e.Result;
                    Console.WriteLine($"Reason: {result.Reason.ToString()}");
                    if (result.Reason == ResultReason.RecognizedSpeech)
                    {
                        Console.WriteLine($"Final result: Text: {result.Text}.");
                    }
                };

                recognizer.Canceled += (s, e) =>
                {
                    Console.WriteLine($"Recognition Canceled. Reason: {e.Reason.ToString()}, CanceledReason: {e.Reason}");
                    //stopRecognition.TrySetResult(0);
                };

                recognizer.SessionStarted += (s, e) =>
                {
                    Console.WriteLine("Session started event.");
                };

                recognizer.SessionStopped += (s, e) =>
                {
                    Console.WriteLine("Session stopped event.");
                    //stopRecognition.TrySetResult(0);
                    Console.WriteLine("-------------------------Continous Speech End--------------------------");
                };

                // Starts continuous recognition. Uses StopContinuousRecognitionAsync() to stop recognition.
                await recognizer.StartContinuousRecognitionAsync().ConfigureAwait(false);

                do
                {
                    Console.WriteLine("Press Enter to stop");
                } while (Console.ReadKey().Key != ConsoleKey.Enter);

                //Task.WaitAny(new[] { stopRecognition.Task });

                // Stops recognition.
                await recognizer.StopContinuousRecognitionAsync().ConfigureAwait(false);
            }
        }

        //public async Task CodecCompressedAudioRecognition(string fileName)
        //{
        //    var audioFormat = AudioStreamFormat.GetCompressedFormat(AudioStreamContainerFormat.OGG_OPUS);
        //    var audioConfig = AudioConfig.FromStreamInput(myPushStream, audioFormat);
            

        //    using (m_audioRecognizer = new SpeechRecognizer(m_config, audioConfig))
        //    {
        //        var result = await m_recognizer.RecognizeOnceAsync();

        //        if (result.Reason == ResultReason.RecognizingSpeech)
        //        {
        //            Console.WriteLine($"Recognized: {result.Text}");
        //        }
        //        else
        //        {
        //            Console.WriteLine($"Faile to recognize, the reason is {result.Reason.ToString()}");
        //        }
        //    }
        //}


        public async Task ShortSpeechRecognitionWithFile(string fileName)
        {
            Console.WriteLine("----------------Short Speech With File Start------------------------");
            var audioInput = AudioConfig.FromWavFileInput(fileName);
            using (var audioRecognizer = new SpeechRecognizer(m_config, audioInput))
            {
                var result = await audioRecognizer.RecognizeOnceAsync();

                if (result.Reason == ResultReason.RecognizedSpeech)
                {
                    Console.WriteLine($"Recognized: {result.Text}");
                }
                else
                {
                    Console.WriteLine($"Faile to recognize, the reason is {result.Reason.ToString()}");
                }
            }
            Console.WriteLine("-----------------Short Speech With File End--------------------------");
        }

        public async Task ShortSpeechRecognitionWithPullStream(string fileName)
        {
            Console.WriteLine("-------------------Short Speech With PullStream Start---------------------");
            using (var audioConfig = Helper.OpenWavFile(fileName))
            {
                using (var audioRecognizer = new SpeechRecognizer(m_config, audioConfig))
                {
                    var result = await audioRecognizer.RecognizeOnceAsync();

                    if (result.Reason == ResultReason.RecognizedSpeech)
                    {
                        Console.WriteLine($"Recognized: {result.Text}");
                    }
                    else
                    {
                        Console.WriteLine($"Failed to recognize, the reason is {result.Reason.ToString()}");
                    }
                }
            }
            Console.WriteLine("-------------------Short Speech With PullStream End-----------------------");
        }

        public void Close()
        {
            //m_recognizer.Dispose();
        }
    }
}
