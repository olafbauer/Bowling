using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bowling
{
	public class Game
	{
		int TotalRounds;
		int Round;
		Stack<Frame> History;

		public Game ()
		{
			this.TotalRounds = 10;
			this.Round = 1;
			this.History = new Stack<Frame> ();
		}

		public void AddRoll (int pins)
		{
			Frame f = (History.Count < this.Round) ? new Frame () : History.Pop ();
			f.Roll (pins);
			if (History.Count > 0) {
				History.Peek ().Update (f);
			}
			History.Push (f);

			if (History.Count < 10) {
				if (f.PinsRolled.Length == 2 || f.IsStrike ()) {
					this.Round++;
				}
			} else if (f.PinsRolled.Length == 3 || (f.PinsRolled.Length == 2 && !f.IsSpare ())) {
				this.Round++;
			}
		}

		Frame[] Frames ()
		{
			return this.History.ToArray ();
		}

		public int TotalScore ()
		{
			return History.Aggregate (0, (acc, f) => acc + f.Score);
		}

		public bool Over ()
		{
			return this.Round > this.TotalRounds;
		}

		public void Start ()
		{
			int[] n = new int[] { 1, 4, 4, 5, 6, 4, 5, 5, 10, 0, 1, 7, 3, 6, 4, 10, 2, 8, 6 };

			int i = 0;
			while (!this.Over ()) {
				this.AddRoll (n [i]);
				Console.WriteLine (this.TotalScore ());
				i++;
			}
		}

		public void PrintFrames ()
		{
			foreach (Frame f in History.Reverse()) {
				Console.Write ("([");
				foreach (int pins in f.PinsRolled) {
					Console.Write (pins + " ");
				}
				Console.Write ("] ");
				Console.Write (f.Score);
				Console.Write (") ");
			}
			Console.WriteLine (this.TotalScore ());
			Console.WriteLine ("-------------------------------------------");
			Console.ReadLine ();
		}
	}

	public class Frame
	{
		public int[] PinsRolled;
		public int Score;

		public Frame ()
		{
			this.Score = 0;
			this.PinsRolled = new int[] { };
		}

		public Frame (int score, int[] pins)
		{
			this.PinsRolled = pins;
			this.Score = score;
		}

		public bool IsEmpty ()
		{
			return this.PinsRolled.Length == 0;
		}

		public int CountRolls ()
		{
			return this.PinsRolled.Length;
		}

		public bool IsSpare ()
		{
			return this.Score == 10;
		}

		public bool IsStrike ()
		{
			return (!this.IsEmpty () && this.PinsRolled [0] == 10);
		}

		public void Roll (int pins)
		{
			int[] p = new int[this.PinsRolled.Length + 1];
			this.PinsRolled.CopyTo (p, 0);
			p [p.Length - 1] = pins;
			this.PinsRolled = p;

			this.Score += pins;
		}

		public void Update (Frame f)
		{
			if ((this.IsSpare () && f.PinsRolled.Length == 1) || (f.PinsRolled.Length == 2 && this.IsStrike ())) {
				this.Score += f.PinsRolled.Last ();
			}
		}

		public override bool Equals (Object o)
		{
			if (o == null)
				return false;
			Frame f = o as Frame;
			if (this.Score != f.Score || this.PinsRolled.Length != f.PinsRolled.Length) {
				return false;
			}
			for (int i = 0; i < this.PinsRolled.Length; i++) {
				if (this.PinsRolled [i] != f.PinsRolled [i]) {
					return false;
				}
			}
			return true;
		}

		public override int GetHashCode ()
		{
			return base.GetHashCode ();
		}
	}
}
