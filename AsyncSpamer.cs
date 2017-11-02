using System;
using System.Threading;
using System.Collections.Generic;
using WindowsInput;

namespace ShadeSpamer
{
	public class AsyncSpamer
	{
		private Thread TheradSpamer;
		private InputSimulator Simulator = new InputSimulator();
		public List<string> TextToSpam = new List<string>();
		public bool UseEnter = false;
		public int LagTime = 0;
		public int StartTime = 0;
		public int SendCount = 0;

		// Rozpoczęcie Spamowania
		public delegate void StartSpamDelegata();
		public event StartSpamDelegata SpamerStarted;

		// Wysłanie Pojedyńczej Wiadomości
		public delegate void SendTextDelegata(string text);
		public event SendTextDelegata SpamerSendText;

		private void SendText(string text)
		{
			Simulator.Keyboard.TextEntry(text);
			SpamerSendText(text);
			if (UseEnter) {
				Simulator.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.RETURN);
			}
		}

		private void ThreadSpamer_Main()
		{
			Thread.Sleep(StartTime);
			SpamerStarted();

			while (TextToSpam.Count > 0)
			{
				Thread.Sleep(LagTime);
				SendText(TextToSpam[0]);
				TextToSpam.RemoveAt(0);
				SendCount++;
			}
		}

		public void Start()
		{
			TheradSpamer = new Thread(new ThreadStart(ThreadSpamer_Main));
			TheradSpamer.IsBackground = true;
			TheradSpamer.Start();
		}
	}
}
