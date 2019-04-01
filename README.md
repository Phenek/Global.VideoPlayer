# Global.VideoPlayer
A simple VideoPlayer for Xamarin.forms based on [Video Player Demos](https://developer.xamarin.com/samples/xamarin-forms/customrenderers/VideoPlayerDemos/)

## Setup
* Available on NuGet: [Global.VideoPlayer](https://www.nuget.org/packages/Global.VideoPlayer) [![NuGet](https://img.shields.io/nuget/v/Global.VideoPlayer.svg?label=NuGet)](https://www.nuget.org/packages/Global.VideoPlayer)
* Add nuget package to your Xamarin.Forms .netStandard/PCL project and to your platform-specific projects

|Platform|Version|
| ------------------- | ------------------- |
|Xamarin.iOS|8.0+|
|Xamarin.Android|15+|

## Preview
<img src="https://media.giphy.com/media/pIQjIu1KJVrRS/giphy.gif" width="460" height="375"> <img src="https://docs.microsoft.com/en-us/xamarin/xamarin-forms/app-fundamentals/custom-renderer/video-player/web-videos-images/playwebvideo-large.png" width="400" height="375">

## Player Properties

Common video player operations are described here.

| Property            | Description                                                                                                                                   |
|---------------------|-----------------------------------------------------------------------------------------------------------------------------------------------|
| Aspect              | Defines how the video content is displayed. Default value is AspectFit. Use AspectFill for Background LoginSCreen                             |
| Loop                | Specifies that the video will restart playing at the end. Default value is False.                                                             |
| AutoPlay            | Specifies that the video will start playing as soon as it is ready. Default value is True.                                                    |
| NativeControls      | Specifies that native controls should be displayed. Default value is True.                                                                    |
| Source              | A local file path or remote URL to a video file.                                                                                              |
| Position            | A read-only bindable playback time for the current video.                                                                                     |
| Duration            | A read-only Field that provide the duration of the video                                                                                      |
| TimeToEnd           | A read-only Field that provide the time until the end of the video.                                                                           |
| State         	  | A read-only bindable property indicating the current state of the video player (NotReady, Playing, Pause)                                     |

## Samples
The sample you can find here https://github.com/Phenek/Global.VideoPlayer/tree/master/Sample

## Video Player Initialization
After installing the NuGet package, the following initialization code is required in each application project:

* iOS - AppDelegate.cs file, in the FinishedLaunching method.
```c#
Global.VideoPlayer.iOS.VideoPlayer.Init();
```

 * Android - MainActivity.cs file, in the OnCreate method.
```c#
Global.VideoPlayer.Droid.VideoPlayer.Init(this, bundle);
```

This calls should be made after the `Xamarin.Forms.Forms.Init()` method call. It is recommended to place this calls in the following files for each platform:
Once the NuGet package has been added and the initialization method called inside each application, the VideoPlayer APIs can be used in the common PCL or Shared Project code.

## License
The MIT License (MIT) see [License file](LICENSE)

## Contribution
Feel free to do it for UWP! I think it's not a big deal, but got no time and no need for now :()

Many Thanks to [@charlespetzold](https://github.com/charlespetzold) for it's contribution ever!