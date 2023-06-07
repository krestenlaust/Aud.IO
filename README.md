# Aud.IO
Audio file handling library. Can read and write wave-files, but can be expanded to support other audio formats as well.

## Background
This project was originally the secondary subproject of an exam project. The original project used this library to read audio files to perform audio processing with fourier transformation ([krestenlaust/Fourier-transformation](https://github.com/krestenlaust/Fourier-transformation)) on them. Because this project originally was part of another project, it once shared commit history with that project. This repository has been made by splitting said history in two (this project, and [krestenlaust/AudioAnalyzer](https://github.com/krestenlaust/AudioAnalyzer)). The effect of this, is that some of the original commits don't make much sense in the scope of this project.

## Usage
The API should be straightforward. You can also refer to one of the demo projects to better understand, how to use the API.

## Repository structure
The projects included in this repository are given a short description each.

### Aud.IO (main project)
This is the main project, which compiles into the Aud.IO nuget package. It contains the implementations of supported formats, and provides the API for reading and modifying audio files.

### Aud.IO.Tests (unit-test project)
Contains the unit-tests of the project. If you plan on contributing, it would be awesome to have more unit-tests, to ensure stability on new nuget package releases.

### AudioMetadataViewer (demo project)
This project shows basic usage of the Wavefile API, displaying all available metadata for a given file. The tool is currently in Danish.

### BasicAudioPlayer (demo project)
This project was an atempt at playing audio right in the console simply using console beep. This didn't succeed, since beep has a very narrow frequency range, and it isn't possible to play multiple beeps concurrently. I came up with the idea to spawn multiple console windows, but the project had depreciated in learning value, as a mere demo project.

## Contributing
Feel free to contribute however you like, any input/issue is welcome. If I don't respond at first, try tagging me as well.
