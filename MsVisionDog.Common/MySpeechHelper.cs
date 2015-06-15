using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MsVisionDog.Common
{
    public class MySpeechHelper
    {
        private Stream _audioStream;

        public MySpeechHelper()
        {
            _audioStream = null;
        }

        public void Speak(string subscriptionKey, string text)
        {
            var guid = Guid.NewGuid().ToString("N");
            Authentication auth = new Authentication(guid, subscriptionKey);
            
            AccessTokenInfo token;
            try
            {
                token = auth.GetAccessToken();
            }
            catch (Exception ex)
            {
                return;
            }

            string requestUri = "https://speech.platform.bing.com/synthesize";

            var cortana = new Synthesize(new Synthesize.InputOptions()
            {
                RequestUri = new Uri(requestUri),
                Text = text,
                VoiceType = Gender.Female,
                Locale = "en-US",
                VoiceName = "Microsoft Server Speech Text to Speech Voice (en-US, ZiraRUS)",
                OutputFormat = AudioOutputFormat.Riff16Khz16BitMonoPcm,
                AuthorizationToken = "Bearer " + token.access_token,
            });

            cortana.OnAudioAvailable += cortana_OnAudioAvailable;
            cortana.OnError += cortana_OnError;
            cortana.Speak(CancellationToken.None).Wait();
        }

        public Stream GetAudioStream()
        {
            return _audioStream;
        }

        private void cortana_OnAudioAvailable(object sender, GenericEventArgs<System.IO.Stream> e)
        {
            _audioStream = e.EventData;
        }

        private void cortana_OnError(object sender, GenericEventArgs<Exception> e)
        {
            //
        }
    }
}
