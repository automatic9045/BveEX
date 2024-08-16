using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AtsEx.PluginHost.Sound;
using AtsEx.PluginHost.Sound.Native;

namespace AtsEx.Sound
{
    internal sealed class AtsSound : IAtsSound
    {
        private readonly AtsSoundCommandQueue CommandQueue = new AtsSoundCommandQueue();

        public PlayState PlayState { get; private set; }

        public AtsSound()
        {
        }

        public void Dispose()
        {
        }

        public int Tick() => CommandQueue.Tick().SerializedValue;

        public void Play()
        {
            CommandQueue.SetNextCommand(AtsSoundCommand.Once);
            PlayState = PlayState.Playing;
        }

        public void PlayLoop()
        {
            CommandQueue.SetNextCommand(AtsSoundCommand.PlayLoop(0));
            PlayState = PlayState.PlayingLoop;
        }

        public void PlayLoopMillibel(int volumeMillibel)
        {
            CommandQueue.SetNextCommand(AtsSoundCommand.PlayLoop(volumeMillibel));
            PlayState = PlayState.PlayingLoop;
        }

        public void PlayLoop(double volumeDecibel)
            => PlayLoopMillibel((int)(volumeDecibel / 100));

        public void PlayLoopRatio(double volumeRatio)
            => PlayLoopMillibel((int)(2000 * Math.Log10(volumeRatio)));

        public void Stop()
        {
            if (PlayState == PlayState.Stop) return;

            CommandQueue.SetNextCommand(AtsSoundCommand.Stop);
            PlayState = PlayState.Stop;
        }

        public void StopAndPlay()
        {
            CommandQueue.SetNextCommand(AtsSoundCommand.Stop, AtsSoundCommand.Once);
            PlayState = PlayState.Playing;
        }
    }
}
