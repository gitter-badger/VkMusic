using System;

namespace VkMusicSync
{
    public interface IProgressable
    {
        event EventHandler<ProgressStatus> ProgressChanged;

        event EventHandler Completed;

        bool Cancel();

        bool Cancelable { get; }
    }
}
