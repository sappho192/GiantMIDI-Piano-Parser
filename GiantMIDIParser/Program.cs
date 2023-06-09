using CsvHelper;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.Globalization;

namespace GiantMIDIParser
{
	internal class Program
	{
		static void Main(string[] args)
		{
			string giantMIDIPath = $"{args[0]}";
			if (Directory.Exists(giantMIDIPath))
			{
				//ReadParseSave(@"D:\DATASET\surname_checked_midis");
				ReadParseSave(giantMIDIPath);
			}
			else
			{
				Console.WriteLine("Path is invalid");
			}
		}

		private static void ReadParseSave(string GIANT_MIDI_ROOT)
		{
			Console.WriteLine("Parser started");

			var allMidiLocation = Directory.GetFiles(GIANT_MIDI_ROOT, "*.mid", SearchOption.AllDirectories);
			var rng = new Random();
			rng.Shuffle(allMidiLocation);
			int[] ratios = { 8, 1, 1 }; // train, valid, test
			int[] sizes = new int[ratios.Length];
			int totalSize = allMidiLocation.Length;

			for (int i = 0; i < ratios.Length; i++)
			{
				sizes[i] = totalSize * ratios[i] / ratios.Sum();
			}

			string[][] midiDataset = new string[ratios.Length][];
			int startIndex = 0;
			for (int i = 0; i < ratios.Length; i++)
			{
				midiDataset[i] = new string[sizes[i]];
				Array.Copy(allMidiLocation, startIndex, midiDataset[i], 0, sizes[i]);
				startIndex += sizes[i];
			}

			string resultRoot = "results";
			string resultTrain = $"{resultRoot}\\train";
			string resultValidation = $"{resultRoot}\\validation";
			string resultTest = $"{resultRoot}\\test";
			Directory.CreateDirectory(resultRoot);
			Directory.CreateDirectory(resultTrain);
			Directory.CreateDirectory(resultValidation);
			Directory.CreateDirectory(resultTest);
			Console.WriteLine("Reading MIDI file");

			midi2csv(midiDataset[2], resultTest);
			midi2csv(midiDataset[1], resultValidation);
			midi2csv(midiDataset[0], resultTrain);

			Console.WriteLine("Finished");
			//var midiFile = MidiFile.Read(@"C:\DATASET\maestro-v3.0.0\2018\MIDI-Unprocessed_Chamber2_MID--AUDIO_09_R3_2018_wav--1.midi");
		}

		private static void midi2csv(string[] midiFilePaths, string resultPath)
		{
			foreach (var midiFilePath in midiFilePaths) // train
			{
				var midiFile = MidiFile.Read(midiFilePath);
				var midiNotes = midiFile.GetNotes().ToArray();
				var csv = new MIDItoCSV();
				for (int i = 0; i < midiNotes.Length; i++)
				{
					var note = midiNotes[i];
					if (i == 0)
					{
						var label = new GiantMIDILabels
						{
							time = (int)note.Time,
							time_diff = 0,
							length = (int)note.Length,
							note_num = note.NoteNumber,
							note_num_diff = 0,
							low_octave = note.NoteNumber < 72 ? 1 : 0,
							velocity = note.Velocity
						};
						csv.Add(label);
					}
					else
					{
						var prevNote = midiNotes[i - 1];
						int timeDiff = (int)(note.Time - prevNote.Time);
						int noteNumDiff = note.NoteNumber - prevNote.NoteNumber;
						var label = new GiantMIDILabels
						{
							time = (int)note.Time,
							time_diff = timeDiff,
							length = (int)note.Length,
							note_num = note.NoteNumber,
							note_num_diff = noteNumDiff,
							low_octave = note.NoteNumber < 72 ? 1 : 0,
							velocity = note.Velocity
						};
						csv.Add(label);
					}
				}

				var filename = Path.GetFileNameWithoutExtension(midiFilePath);
				//Console.WriteLine($"Saving {filename}.csv");
				csv.Save($"{resultPath}\\{filename}");
				midiFile.Write($"{resultPath}\\{filename}.midi");
				Console.Write(".");
			}
		}
	}
}