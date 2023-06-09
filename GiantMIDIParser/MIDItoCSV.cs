using System.Text;

namespace GiantMIDIParser
{
	public class MIDItoCSV
	{
		public readonly string fields;
		private List<GiantMIDILabels> noteInfos;

		public MIDItoCSV()
		{
			fields = $"{nameof(GiantMIDILabels.time)},{nameof(GiantMIDILabels.time_diff)},{nameof(GiantMIDILabels.note_num)},{nameof(GiantMIDILabels.note_num_diff)},{nameof(GiantMIDILabels.low_octave)},{nameof(GiantMIDILabels.length)},{nameof(GiantMIDILabels.velocity)}\n";
			noteInfos = new List<GiantMIDILabels>();
		}

		public void Add(GiantMIDILabels result)
		{
			noteInfos.Add(result);
		}

		public bool Save(string filename)
		{
			StringBuilder sb = new();
			sb.Append(fields);
			foreach (var item in noteInfos)
			{
				sb.Append($"{item.time},{item.time_diff},{item.note_num},{item.note_num_diff},{item.low_octave},{item.length},{item.velocity}\n");
			}

			File.WriteAllText($"{filename}.csv", sb.ToString());

			return true;
		}
	}
}
