# GiantMIDI-Piano-Parser

GiantMIDI-Piano dataset parser written in C#

## Dataset

Dataset can be acquired from [https://github.com/bytedance/GiantMIDI-Piano](https://github.com/bytedance/GiantMIDI-Piano)

## How to use

This program parses information from the MIDI file and translates into following types of data:

| label       | type    | range              | source                                                 |
| ----------- | ------- | ------------------ | ------------------------------------------------------ |
| Time        | Integer | 0 < n              | raw time                                               |
| TimeDiff    | Integer | 0 ≤ n             | Time - Previous Time                                   |
| Length      | Integer | 0 < n              | Note Off - Note On                                     |
| NoteNum     | Integer | 0 ≤ n ≤ 127      |                                                        |
| NoteNumDiff | Integer | -127 ≤ n ≤ 127 | NoteNum - Previous NoteNum                             |
| LowOctave   | Integer | 0 or 1             | 1 when NoteNum < 72 (C5)<br />0 when NoteNum > 71 (B4) |
| Velocity    | Integer | 0 ≤ n ≤ 127      |                                                        |

### Execution

In terminal, just pass the directory path of the midi dataset of GiantMIDI-Piano (`midis` or `surname_checked_midis`)

```bash
dotnet GiantMIDIParser D:\DATASET\surname_checked_midis
```

Result will be saved in `result` folder.  

Data will be automatically shuffled and split into train/validation/test folder. Split ratio is 8:1:1.
