using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FFmpeg_EasyConvert.FFmpeg;

namespace FFmpeg_EasyConvert
{
    public static class FFmpeg
    {
        public static class Codec
        {
            public static string H264 { get; } = "libx264";
            public static string H265 { get; } = "libx265";
            public static string AAC { get; } = "aac";
        }
        
        public class Converter
        {
            public int? AspectRatio { get; set; }
            public int? AudioBitrate { get; set; }
            public string? AudioCodec { get; set; }
            public bool DisableAudio { get; set; }
            public bool DisableVideo { get; set; }
            public int? Framerate { get; set; }
            public int? SamplingRate { get; set; }
            public int? VideoBitrate { get; set; }
            public string? VideoCodec { get; set; }

            private static Process Convert_Internal(string[] arguments)
            {
                Process p = new();
                {
                    p.StartInfo.FileName = "ffmpeg.exe";
                    p.StartInfo.Arguments = String.Join(' ', arguments);
                    p.StartInfo.CreateNoWindow = true;
                    p.Start();
                }
                p.WaitForExit();
                return p;
            }

            public void Start(string input, string output)
            {
                Console.WriteLine($"Starting conversion from {Path.GetFullPath(input)} to {Path.GetFullPath(output)}");
                string[] args =  [
                $"-i \"{input}\"",
                VideoCodec != null ? $"-c:v {VideoCodec}" : "",
                AudioCodec != null ? $"-c:a {AudioCodec}" : "",
                VideoBitrate.HasValue ? $"-b:v {VideoBitrate}" : "",
                AudioBitrate.HasValue ? $"-b:a {AudioBitrate}" : "",
                SamplingRate.HasValue ? $"-ar {SamplingRate}" : "",
                Framerate.HasValue ? $"-r {Framerate}" : "",
                AspectRatio.HasValue ? $"-aspect {AspectRatio}" : "",
                DisableVideo == true ? "-vn" : "",
                DisableVideo == true ? "-an" : "",
                $"\"{output}\""
                ];

                Process p = Convert_Internal(args);
                string originalExtension = Path.GetExtension(input);
                string newExtension = Path.GetExtension(output);
                Console.WriteLine(
                $"Completed {output} ({originalExtension} to {newExtension})" +
                $" in {Math.Round((p.ExitTime - p.StartTime).TotalMicroseconds / 1000000, 6)} seconds." +
                $" ({p.ExitCode})"
                );
            }

            public void StartMultiple(string[] inputs, string[] outputs)
            {
                if (inputs.Length != outputs.Length)
                    throw new Exception();

                List<Task> conversionTasks = [];
                foreach (var (@in, @out) in inputs.Zip(outputs))
                {
                    Task task = new(() => Start(@in, @out));
                    task.Start();
                    conversionTasks.Add(task);
                }

                Task.WaitAll([.. conversionTasks]);
                Console.WriteLine($"{inputs.Length} conversions complete.");
            }
        }

        
    }
}
