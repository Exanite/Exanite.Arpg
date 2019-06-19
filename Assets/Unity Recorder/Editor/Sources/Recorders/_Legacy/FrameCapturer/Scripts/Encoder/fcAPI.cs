using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace UnityEditor.Recorder.FrameCapturer
{
#pragma warning disable CS0649
#pragma warning disable IDE1006 // Naming Styles
    static class fcAPI
#pragma warning restore IDE1006 // Naming Styles
    {
        // -------------------------------------------------------------
        // Foundation
        // -------------------------------------------------------------

#pragma warning disable IDE1006 // Naming Styles
        internal enum fcPixelFormat
#pragma warning restore IDE1006 // Naming Styles
        {
            Unknown = 0,

            ChannelMask = 0xF,
            TypeMask = 0xF << 4,
            Type_f16 = 0x1 << 4,
            Type_f32 = 0x2 << 4,
            Type_u8  = 0x3 << 4,
            Type_i16 = 0x4 << 4,
            Type_i32 = 0x5 << 4,

            Rf16     = Type_f16 | 1,
            RGf16    = Type_f16 | 2,
            RGBf16   = Type_f16 | 3,
            RGBAf16  = Type_f16 | 4,
            Rf32     = Type_f32 | 1,
            RGf32    = Type_f32 | 2,
            RGBf32   = Type_f32 | 3,
            RGBAf32  = Type_f32 | 4,
            Ru8      = Type_u8  | 1,
            RGu8     = Type_u8  | 2,
            RGBu8    = Type_u8  | 3,
            RGBAu8   = Type_u8  | 4,
            Ri16     = Type_i16 | 1,
            RGi16    = Type_i16 | 2,
            RGBi16   = Type_i16 | 3,
            RGBAi16  = Type_i16 | 4,
            Ri32     = Type_i32 | 1,
            RGi32    = Type_i32 | 2,
            RGBi32   = Type_i32 | 3,
            RGBAi32  = Type_i32 | 4,
        };

#pragma warning disable IDE1006 // Naming Styles
        internal enum fcBitrateMode
#pragma warning restore IDE1006 // Naming Styles
        {
            CBR,
            VBR,
        }

#pragma warning disable IDE1006 // Naming Styles
        internal enum fcAudioBitsPerSample
#pragma warning restore IDE1006 // Naming Styles
        {
            _8Bits = 8,
            _16Bits = 16,
            _24Bits = 24,
        }

#pragma warning disable IDE1006 // Naming Styles
        internal struct fcDeferredCall
#pragma warning restore IDE1006 // Naming Styles
        {
            public int handle;
            public void Release() { fcReleaseDeferredCall(this); handle = 0; }
            public static implicit operator int(fcDeferredCall v) { return v.handle; }
        }

#pragma warning disable IDE1006 // Naming Styles
        internal struct fcStream
#pragma warning restore IDE1006 // Naming Styles
        {
            public IntPtr ptr;
            public void Release() { fcReleaseStream(this); ptr = IntPtr.Zero; }
            public static implicit operator bool(fcStream v) { return v.ptr != IntPtr.Zero; }
        }
#pragma warning disable IDE1006 // Naming Styles
        [DllImport ("fccore")] internal static extern fcStream     fcCreateFileStream(string path);
#pragma warning restore IDE1006 // Naming Styles
#pragma warning disable IDE1006 // Naming Styles
        [DllImport ("fccore")] static extern void        fcReleaseStream(fcStream s);
#pragma warning restore IDE1006 // Naming Styles
        
#pragma warning disable IDE1006 // Naming Styles
        [DllImport ("fccore")] static extern void         fcGuardBegin();
#pragma warning restore IDE1006 // Naming Styles
#pragma warning disable IDE1006 // Naming Styles
        [DllImport ("fccore")] static extern void         fcGuardEnd();
#pragma warning restore IDE1006 // Naming Styles
#pragma warning disable IDE1006 // Naming Styles
        [DllImport ("fccore")] static extern void        fcReleaseDeferredCall(fcDeferredCall dc);
#pragma warning restore IDE1006 // Naming Styles

#pragma warning disable IDE1006 // Naming Styles
        internal static fcPixelFormat fcGetPixelFormat(TextureFormat v)
#pragma warning restore IDE1006 // Naming Styles
        {
            switch (v)
            {
                case TextureFormat.Alpha8:      return fcPixelFormat.Ru8;
                case TextureFormat.RGB24:       return fcPixelFormat.RGBu8;
                case TextureFormat.RGBA32:      return fcPixelFormat.RGBAu8;
                case TextureFormat.ARGB32:      return fcPixelFormat.RGBAu8;
                case TextureFormat.RGBAHalf:    return fcPixelFormat.RGBAf16;
                case TextureFormat.RGHalf:      return fcPixelFormat.RGf16;
                case TextureFormat.RHalf:       return fcPixelFormat.Rf16;
                case TextureFormat.RGBAFloat:   return fcPixelFormat.RGBAf32;
                case TextureFormat.RGFloat:     return fcPixelFormat.RGf32;
                case TextureFormat.RFloat:      return fcPixelFormat.Rf32;
            }
            return fcPixelFormat.Unknown;
        }

#pragma warning disable IDE1006 // Naming Styles
        internal static int fcGetNumAudioChannels()
#pragma warning restore IDE1006 // Naming Styles
        {
            switch (AudioSettings.speakerMode)
            {
                case AudioSpeakerMode.Mono:         return 1;
                case AudioSpeakerMode.Stereo:       return 2;
                case AudioSpeakerMode.Quad:         return 4;
                case AudioSpeakerMode.Surround:     return 5;
                case AudioSpeakerMode.Mode5point1:  return 6;
                case AudioSpeakerMode.Mode7point1:  return 8;
                case AudioSpeakerMode.Prologic:     return 6;
            }
            return 0;
        }


#pragma warning disable IDE1006 // Naming Styles
        [DllImport ("fccore")] internal static extern void fcWaitAsyncDelete();
#pragma warning restore IDE1006 // Naming Styles
#pragma warning disable IDE1006 // Naming Styles
        [DllImport ("fccore")] internal static extern void fcReleaseContext(IntPtr ctx);
#pragma warning restore IDE1006 // Naming Styles


        // -------------------------------------------------------------
        // PNG Exporter
        // -------------------------------------------------------------

#pragma warning disable IDE1006 // Naming Styles
        internal enum fcPngPixelFormat
#pragma warning restore IDE1006 // Naming Styles
        {
            Auto, // select optimal one for input data
            UInt8,
            UInt16,
        };

        [Serializable]
#pragma warning disable IDE1006 // Naming Styles
        internal struct fcPngConfig
#pragma warning restore IDE1006 // Naming Styles
        {
            public fcPngPixelFormat pixelFormat;
            [Range(1, 32)] public int maxTasks;
            // C# ext
            [HideInInspector] public int width;
            [HideInInspector] public int height;
            [HideInInspector] public int channels;

#pragma warning disable IDE1006 // Naming Styles
            internal static fcPngConfig default_value
#pragma warning restore IDE1006 // Naming Styles
            {
                get
                {
                    return new fcPngConfig
                    {
                        pixelFormat = fcPngPixelFormat.Auto,
                        maxTasks = 2,
                    };
                }
            }
        };

#pragma warning disable IDE1006 // Naming Styles
        internal struct fcPngContext
#pragma warning restore IDE1006 // Naming Styles
        {
            public IntPtr ptr;
            public void Release() { fcReleaseContext(ptr); ptr = IntPtr.Zero; }
            public static implicit operator bool(fcPngContext v) { return v.ptr != IntPtr.Zero; }
        }

#pragma warning disable IDE1006 // Naming Styles
        [DllImport ("fccore")] internal static extern Bool         fcPngIsSupported();
#pragma warning restore IDE1006 // Naming Styles
#pragma warning disable IDE1006 // Naming Styles
        [DllImport ("fccore")] internal static extern fcPngContext fcPngCreateContext(ref fcPngConfig conf);
#pragma warning restore IDE1006 // Naming Styles
#pragma warning disable IDE1006 // Naming Styles
        [DllImport ("fccore")] internal static extern Bool         fcPngExportPixels(fcPngContext ctx, string path, byte[] pixels, int width, int height, fcPixelFormat fmt, int num_channels);
#pragma warning restore IDE1006 // Naming Styles


        // -------------------------------------------------------------
        // EXR Exporter
        // -------------------------------------------------------------

#pragma warning disable IDE1006 // Naming Styles
        internal enum fcExrPixelFormat
#pragma warning restore IDE1006 // Naming Styles
        {
            Auto, // select optimal one for input data
            Half,
            Float,
            Int,
        };

#pragma warning disable IDE1006 // Naming Styles
        internal enum fcExrCompression
#pragma warning restore IDE1006 // Naming Styles
        {
            None,
            RLE,
            ZipS, // par-line
            Zip,  // block
            PIZ,
        };

        [Serializable]
#pragma warning disable IDE1006 // Naming Styles
        internal struct fcExrConfig
#pragma warning restore IDE1006 // Naming Styles
        {
            public fcExrPixelFormat pixelFormat;
            public fcExrCompression compression;
            [Range(1, 32)] public int maxTasks;
            // C# ext
            [HideInInspector] public int width;
            [HideInInspector] public int height;
            [HideInInspector] public int channels;

#pragma warning disable IDE1006 // Naming Styles
            internal static fcExrConfig default_value
#pragma warning restore IDE1006 // Naming Styles
            {
                get
                {
                    return new fcExrConfig
                    {
                        pixelFormat = fcExrPixelFormat.Auto,
                        compression = fcExrCompression.Zip,
                        maxTasks = 2,
                    };
                }
            }
        };

#pragma warning disable IDE1006 // Naming Styles
        internal struct fcExrContext
#pragma warning restore IDE1006 // Naming Styles
        {
            public IntPtr ptr;
            public void Release() { fcReleaseContext(ptr); ptr = IntPtr.Zero; }
            public static implicit operator bool(fcExrContext v) { return v.ptr != IntPtr.Zero; }
        }

#pragma warning disable IDE1006 // Naming Styles
        [DllImport ("fccore")] internal static extern Bool         fcExrIsSupported();
#pragma warning restore IDE1006 // Naming Styles
#pragma warning disable IDE1006 // Naming Styles
        [DllImport ("fccore")] internal static extern fcExrContext fcExrCreateContext(ref fcExrConfig conf);
#pragma warning restore IDE1006 // Naming Styles
#pragma warning disable IDE1006 // Naming Styles
        [DllImport ("fccore")] internal static extern Bool         fcExrBeginImage(fcExrContext ctx, string path, int width, int height);
#pragma warning restore IDE1006 // Naming Styles
#pragma warning disable IDE1006 // Naming Styles
        [DllImport ("fccore")] internal static extern Bool         fcExrAddLayerPixels(fcExrContext ctx, byte[] pixels, fcPixelFormat fmt, int ch, string name);
#pragma warning restore IDE1006 // Naming Styles
#pragma warning disable IDE1006 // Naming Styles
        [DllImport ("fccore")] internal static extern Bool         fcExrEndImage(fcExrContext ctx);
#pragma warning restore IDE1006 // Naming Styles


        // -------------------------------------------------------------
        // GIF Exporter
        // -------------------------------------------------------------

        [Serializable]
#pragma warning disable IDE1006 // Naming Styles
        internal struct fcGifConfig
#pragma warning restore IDE1006 // Naming Styles
        {
            [HideInInspector] public int width;
            [HideInInspector] public int height;
            [Range(1, 256)] public int numColors;
            [Range(1, 120)] public int keyframeInterval;
            [Range(1, 32)] public int maxTasks;

#pragma warning disable IDE1006 // Naming Styles
            internal static fcGifConfig default_value
#pragma warning restore IDE1006 // Naming Styles
            {
                get
                {
                    return new fcGifConfig
                    {
                        numColors = 256,
                        maxTasks = 8,
                        keyframeInterval = 30,
                    };
                }
            }
        };
#pragma warning disable IDE1006 // Naming Styles
        internal struct fcGifContext
#pragma warning restore IDE1006 // Naming Styles
        {
            public IntPtr ptr;
            public void Release() { fcReleaseContext(ptr); ptr = IntPtr.Zero; }
            public static implicit operator bool(fcGifContext v) { return v.ptr != IntPtr.Zero; }
        }

#pragma warning disable IDE1006 // Naming Styles
        [DllImport ("fccore")] internal static extern Bool         fcGifIsSupported();
#pragma warning restore IDE1006 // Naming Styles
#pragma warning disable IDE1006 // Naming Styles
        [DllImport ("fccore")] internal static extern fcGifContext fcGifCreateContext(ref fcGifConfig conf);
#pragma warning restore IDE1006 // Naming Styles
#pragma warning disable IDE1006 // Naming Styles
        [DllImport ("fccore")] internal static extern void         fcGifAddOutputStream(fcGifContext ctx, fcStream stream);
#pragma warning restore IDE1006 // Naming Styles
#pragma warning disable IDE1006 // Naming Styles
        [DllImport ("fccore")] internal static extern Bool         fcGifAddFramePixels(fcGifContext ctx, byte[] pixels, fcPixelFormat fmt, double timestamp = -1.0);
#pragma warning restore IDE1006 // Naming Styles


        // -------------------------------------------------------------
        // MP4 Exporter
        // -------------------------------------------------------------

#pragma warning disable IDE1006 // Naming Styles
        internal enum fcMP4VideoFlags
#pragma warning restore IDE1006 // Naming Styles
        {
            H264NVIDIA  = 1 << 1,
            H264AMD     = 1 << 2,
            H264IntelHW = 1 << 3,
            H264IntelSW = 1 << 4,
            H264OpenH264= 1 << 5,
            H264Mask = H264NVIDIA | H264AMD | H264IntelHW | H264IntelSW | H264OpenH264,
        };

#pragma warning disable IDE1006 // Naming Styles
        internal enum fcMP4AudioFlags
#pragma warning restore IDE1006 // Naming Styles
        {
            AACIntel = 1 << 1,
            AACFAAC  = 1 << 2,
            AACMask = AACIntel | AACFAAC,
        };

        [Serializable]
#pragma warning disable IDE1006 // Naming Styles
        internal struct fcMP4Config
#pragma warning restore IDE1006 // Naming Styles
        {
            [HideInInspector] public Bool video;
            [HideInInspector] public int videoWidth;
            [HideInInspector] public int videoHeight;
            [HideInInspector] public int videoTargetFramerate;
            public fcBitrateMode videoBitrateMode;
            public int videoTargetBitrate;
            [HideInInspector] public int videoFlags;
            [Range(1, 32)] public int videoMaxTasks;

            [HideInInspector] public Bool audio;
            [HideInInspector] public int audioSampleRate;
            [HideInInspector] public int audioNumChannels;
            public fcBitrateMode audioBitrateMode;
            public int audioTargetBitrate;
            [HideInInspector] public int audioFlags;
            [Range(1, 32)] public int audioMaxTasks;

#pragma warning disable IDE1006 // Naming Styles
            internal static fcMP4Config default_value
#pragma warning restore IDE1006 // Naming Styles
            {
                get
                {
                    return new fcMP4Config
                    {
                        video = true,
                        videoBitrateMode = fcBitrateMode.VBR,
                        videoTargetBitrate = 1024 * 1000,
                        videoTargetFramerate = 30,
                        videoFlags = (int)fcMP4VideoFlags.H264Mask,
                        videoMaxTasks = 4,

                        audio = true,
                        audioSampleRate = 48000,
                        audioNumChannels = 2,
                        audioBitrateMode = fcBitrateMode.VBR,
                        audioTargetBitrate = 128 * 1000,
                        audioFlags = (int)fcMP4AudioFlags.AACMask,
                        audioMaxTasks = 4,
                    };
                }
            }
        };
#pragma warning disable IDE1006 // Naming Styles
        internal struct fcMP4Context
#pragma warning restore IDE1006 // Naming Styles
        {
            public IntPtr ptr;
            public void Release() { fcReleaseContext(ptr); ptr = IntPtr.Zero; }
            public static implicit operator bool(fcMP4Context v) { return v.ptr != IntPtr.Zero; }
        }

#pragma warning disable IDE1006 // Naming Styles
        [DllImport ("fccore")] internal static extern Bool             fcMP4OSIsSupported();
#pragma warning restore IDE1006 // Naming Styles
#pragma warning disable IDE1006 // Naming Styles
        [DllImport ("fccore")] internal static extern fcMP4Context     fcMP4OSCreateContext(ref fcMP4Config conf, string path);
#pragma warning restore IDE1006 // Naming Styles
#pragma warning disable IDE1006 // Naming Styles
        [DllImport ("fccore")] internal static extern Bool             fcMP4AddVideoFramePixels(fcMP4Context ctx, byte[] pixels, fcPixelFormat fmt, double timestamp = -1.0);
#pragma warning restore IDE1006 // Naming Styles
#pragma warning disable IDE1006 // Naming Styles
        [DllImport ("fccore")] internal static extern Bool             fcMP4AddAudioSamples(fcMP4Context ctx, float[] samples, int num_samples);
#pragma warning restore IDE1006 // Naming Styles


        // -------------------------------------------------------------
        // WebM Exporter
        // -------------------------------------------------------------

#pragma warning disable IDE1006 // Naming Styles
        internal struct fcWebMContext
#pragma warning restore IDE1006 // Naming Styles
        {
            public IntPtr ptr;
            public void Release() { fcReleaseContext(ptr); ptr = IntPtr.Zero; }
            public static implicit operator bool(fcWebMContext v) { return v.ptr != IntPtr.Zero; }
        }

#pragma warning disable IDE1006 // Naming Styles
        internal enum fcWebMVideoEncoder
#pragma warning restore IDE1006 // Naming Styles
        {
            VP8,
            VP9,
            VP9LossLess,
        };
#pragma warning disable IDE1006 // Naming Styles
        internal enum fcWebMAudioEncoder
#pragma warning restore IDE1006 // Naming Styles
        {
            Vorbis,
            Opus,
        };

        [Serializable]
#pragma warning disable IDE1006 // Naming Styles
        internal struct fcWebMConfig
#pragma warning restore IDE1006 // Naming Styles
        {
            [HideInInspector] public Bool video;
            public fcWebMVideoEncoder videoEncoder;
            [HideInInspector] public int videoWidth;
            [HideInInspector] public int videoHeight;
            [HideInInspector] public int videoTargetFramerate;
            public fcBitrateMode videoBitrateMode;
            public int videoTargetBitrate;
            [Range(1, 32)] public int videoMaxTasks;

            [HideInInspector] public Bool audio;
            public fcWebMAudioEncoder audioEncoder;
            [HideInInspector] public int audioSampleRate;
            [HideInInspector] public int audioNumChannels;
            public fcBitrateMode audioBitrateMode;
            public int audioTargetBitrate;
            [Range(1, 32)] public int audioMaxTasks;

#pragma warning disable IDE1006 // Naming Styles
            internal static fcWebMConfig default_value
#pragma warning restore IDE1006 // Naming Styles
            {
                get
                {
                    return new fcWebMConfig
                    {
                        video = true,
                        videoEncoder = fcWebMVideoEncoder.VP8,
                        videoTargetFramerate = 60,
                        videoBitrateMode = fcBitrateMode.VBR,
                        videoTargetBitrate = 1024 * 1000,
                        videoMaxTasks = 4,

                        audio = true,
                        audioEncoder = fcWebMAudioEncoder.Vorbis,
                        audioSampleRate = 48000,
                        audioNumChannels = 2,
                        audioBitrateMode = fcBitrateMode.VBR,
                        audioTargetBitrate = 128 * 1000,
                        audioMaxTasks = 4,
                    };
                }
            }
        }

#pragma warning disable IDE1006 // Naming Styles
        [DllImport ("fccore")] internal static extern Bool fcWebMIsSupported();
#pragma warning restore IDE1006 // Naming Styles
#pragma warning disable IDE1006 // Naming Styles
        [DllImport ("fccore")] internal static extern fcWebMContext fcWebMCreateContext(ref fcWebMConfig conf);
#pragma warning restore IDE1006 // Naming Styles
#pragma warning disable IDE1006 // Naming Styles
        [DllImport ("fccore")] internal static extern void fcWebMAddOutputStream(fcWebMContext ctx, fcStream stream);
#pragma warning restore IDE1006 // Naming Styles
        // timestamp=-1 is treated as current time.
#pragma warning disable IDE1006 // Naming Styles
        [DllImport ("fccore")] internal static extern Bool fcWebMAddVideoFramePixels(fcWebMContext ctx, byte[] pixels, fcPixelFormat fmt, double timestamp = -1.0);
#pragma warning restore IDE1006 // Naming Styles
        // timestamp=-1 is treated as current time.
#pragma warning disable IDE1006 // Naming Styles
        [DllImport ("fccore")] internal static extern Bool fcWebMAddAudioSamples(fcWebMContext ctx, float[] samples, int num_samples);
#pragma warning restore IDE1006 // Naming Styles


        // -------------------------------------------------------------
        // Wave Exporter
        // -------------------------------------------------------------

        [Serializable]
#pragma warning disable IDE1006 // Naming Styles
        internal struct fcWaveConfig
#pragma warning restore IDE1006 // Naming Styles
        {
            [HideInInspector] public int sampleRate;
            [HideInInspector] public int numChannels;
            public fcAudioBitsPerSample bitsPerSample;
            [Range(1, 32)] public int maxTasks;

#pragma warning disable IDE1006 // Naming Styles
            internal static fcWaveConfig default_value
#pragma warning restore IDE1006 // Naming Styles
            {
                get
                {
                    return new fcWaveConfig
                    {
                        sampleRate = 48000,
                        numChannels = 2,
                        bitsPerSample = fcAudioBitsPerSample._16Bits,
                        maxTasks = 2,
                    };
                }
            }
        }       

        // -------------------------------------------------------------
        // Ogg Exporter
        // -------------------------------------------------------------

        [Serializable]
#pragma warning disable IDE1006 // Naming Styles
        internal struct fcOggConfig
#pragma warning restore IDE1006 // Naming Styles
        {
            [HideInInspector] public int sampleRate;
            [HideInInspector] public int numChannels;
            public fcBitrateMode bitrateMode;
            public int targetBitrate;
            [Range(1, 32)] public int maxTasks;

#pragma warning disable IDE1006 // Naming Styles
            internal static fcOggConfig default_value
#pragma warning restore IDE1006 // Naming Styles
            {
                get
                {
                    return new fcOggConfig
                    {
                        sampleRate = 48000,
                        numChannels = 2,
                        bitrateMode = fcBitrateMode.VBR,
                        targetBitrate = 128 * 1000,
                        maxTasks = 2,
                    };
                }
            }
        }

        // -------------------------------------------------------------
        // Flac Exporter
        // -------------------------------------------------------------

        [Serializable]
#pragma warning disable IDE1006 // Naming Styles
        internal struct fcFlacConfig
#pragma warning restore IDE1006 // Naming Styles
        {
            [HideInInspector] public int sampleRate;
            [HideInInspector] public int numChannels;
            public fcAudioBitsPerSample bitsPerSample;
            [Range(0,9)] public int compressionLevel;
            public int blockSize;
            [HideInInspector] public Bool verify;
            [Range(1, 32)] public int maxTasks;
        }

#pragma warning disable IDE1006 // Naming Styles
        internal static void fcLock(RenderTexture src, TextureFormat dstfmt, Action<byte[], fcPixelFormat> body)
#pragma warning restore IDE1006 // Naming Styles
        {
            var tex = new Texture2D(src.width, src.height, dstfmt, false);
            RenderTexture.active = src;
            tex.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0, false);
            tex.Apply();
            body(tex.GetRawTextureData(), fcGetPixelFormat(tex.format));
            UnityEngine.Object.Destroy(tex);
        }

#pragma warning disable IDE1006 // Naming Styles
        internal static void fcLock(RenderTexture src, Action<byte[], fcPixelFormat> body)
#pragma warning restore IDE1006 // Naming Styles
        {
            TextureFormat dstfmt = TextureFormat.RGBA32;
            switch (src.format)
            {
                case RenderTextureFormat.DefaultHDR:
                case RenderTextureFormat.ARGB2101010:
                case RenderTextureFormat.RGB111110Float:
                case RenderTextureFormat.ARGBHalf:
                case RenderTextureFormat.RGHalf:
                case RenderTextureFormat.Depth:
                case RenderTextureFormat.Shadowmap:
                case RenderTextureFormat.RHalf: dstfmt = TextureFormat.RGBAHalf; break;
                case RenderTextureFormat.ARGBFloat:
                case RenderTextureFormat.RGFloat:
                case RenderTextureFormat.RFloat: dstfmt = TextureFormat.RGBAFloat; break;
            }
            fcLock(src, dstfmt, body);
        }
    }
#pragma warning restore CS0649
}
