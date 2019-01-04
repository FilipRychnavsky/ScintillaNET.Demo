using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ScintillaNET;
using System.Diagnostics;

namespace ScintillaNET.DemoFR
{
	public partial class Form1 : Form
	{
		ScintillaNET.Scintilla m_rScintilla_TextArea;

		public Form1()
		{
			InitializeComponent();
			m_rScintilla_TextArea = new ScintillaNET.Scintilla();
			m_rPanel.Controls.Add(m_rScintilla_TextArea);
			InitText();
			InitDwelling();
			SetIndicatorForURL();
			m_rScintilla_TextArea.CharAdded += OnCharAdded;
			m_rScintilla_TextArea.AutoCSelection += OnScintilla_AutoCSelection;
//m_rScintilla_TextArea.AutoCCurrent
			m_rScintilla_TextArea.AutoCIgnoreCase = true;
			m_rScintilla_TextArea.AutoCCompleted += OnScintilla_AutoCCompleted;
//m_rScintilla_TextArea.autoc
		}

		private void SetIndicatorForURL()
		{
			//TODO_FR 299 ToolTip in AutoCompletion m_rScintilla_CodeEditor.AutoCShow(nLengthEntered, sAutoCompletionList);
			//https://github.com/jacobslusser/ScintillaNET/issues/111
			// Define an indicator for marking URLs and apply it to a range.
			// How you determine a particular range is a URL and how often
			// you want to scan the text for them is up to you.
			m_rScintilla_TextArea.Indicators[0].Style = IndicatorStyle.Plain;
			m_rScintilla_TextArea.Indicators[0].ForeColor = Color.Blue;
			m_rScintilla_TextArea.IndicatorCurrent = 0;
			m_rScintilla_TextArea.IndicatorFillRange(0, 21); // Use your own logic

			// An indicator can only change the appearance of text in one way. So
			// to get underlining AND a different foreground color we have to use two indicators.
			m_rScintilla_TextArea.Indicators[1].Style = IndicatorStyle.TextFore;
			m_rScintilla_TextArea.Indicators[1].ForeColor = Color.Blue;
			m_rScintilla_TextArea.IndicatorCurrent = 1;
			m_rScintilla_TextArea.IndicatorFillRange(0, 21); // Use your own logic


/*
			// Indicator 10 - out of Lexer indicators - Filip
			m_rScintilla_TextArea.Indicators[10].Style = IndicatorStyle.TextFore;
			m_rScintilla_TextArea.Indicators[10].ForeColor = Color.LightBlue;
			m_rScintilla_TextArea.IndicatorCurrent = 10;
			string sDemoTextRange = m_rScintilla_TextArea.GetTextRange(25, 30);
			int nText_Length = m_rScintilla_TextArea.Text.Length;
			int nTextLength = m_rScintilla_TextArea.TextLength; // before updating Cache - old size - https://github.com/jacobslusser/ScintillaNET/issues/223
			m_rScintilla_TextArea.IndicatorFillRange(25,30);
*/
			m_rScintilla_TextArea.Styles[Style.CallTip].SizeF = 8.25F;
			m_rScintilla_TextArea.Styles[Style.CallTip].ForeColor = SystemColors.InfoText;
			m_rScintilla_TextArea.Styles[Style.CallTip].BackColor = SystemColors.Info;
		}

		private void OnScintilla_AutoCCompleted(object sender, AutoCSelectionEventArgs e)
		{
		}

		private void OnCharAdded(object sender, CharAddedEventArgs e)
		{
			Debug.WriteLine("Length of Text after OnCharAdded: {0}", m_rScintilla_TextArea.Text.Length);
			if (e.Char == '.') {
				ShowAutoCompletion();
			}
		}

		private void ShowAutoCompletion()
		{
			//TODO_FR #CodeEditor ? AutoCActive
			// Find the word start
			int nCurrentPosition = m_rScintilla_TextArea.CurrentPosition;
			int nWordStartPosition = m_rScintilla_TextArea.WordStartPosition(nCurrentPosition, true);
			// Display the autocompletion list
			int nLengthEntered = nCurrentPosition - nWordStartPosition;
			string sAutoCompletionList = "aaa bbb ccc dddd eee";			
			m_rScintilla_TextArea.AutoCShow(nLengthEntered, sAutoCompletionList);
		}

		private void OnScintilla_AutoCSelection(object sender, AutoCSelectionEventArgs rAutoCSelectionEventArgs)
		{
			string sDebug = System.String.Format("after selection:\nselected Text: {0}; position: {1}", rAutoCSelectionEventArgs.Text, rAutoCSelectionEventArgs.Position);
			sDebug += System.String.Format("\nnew lentgh: {0}", m_rScintilla_TextArea.Text.Length);
			sDebug += System.String.Format("\nWordFromPosition: {0}", m_rScintilla_TextArea.GetWordFromPosition(rAutoCSelectionEventArgs.Position));
			// Minus 1 wegen dem Punkt
			sDebug += System.String.Format("\nWordFromPosition - 1: {0}", m_rScintilla_TextArea.GetWordFromPosition(rAutoCSelectionEventArgs.Position - 1));
			Debug.WriteLine(sDebug);
		}

		private void InitDwelling()
		{
			//FRs Anmeldung von Dwell
			// BONUS: Configure call tips for the Dwell events
			m_rScintilla_TextArea.MouseDwellTime = 400;
			m_rScintilla_TextArea.DwellStart	+= m_rScintilla_TextArea_DwellStart;
			m_rScintilla_TextArea.DwellEnd		+= m_rScintilla_TextArea_DwellEnd;
		}

		private string GetUrlAtPosition(int position)
		{
			// Determine whether the specified position is on our 'URL indicator'
			// and if so whether it is a valid URL.

			var urlIndicator = m_rScintilla_TextArea.Indicators[0];
			var bitmapFlag = (1 << urlIndicator.Index);
			var bitmap = m_rScintilla_TextArea.IndicatorAllOnFor(position);
			var hasUrlIndicator = ((bitmapFlag & bitmap) == bitmapFlag);

			if (hasUrlIndicator) {
				var startPos = urlIndicator.Start(position);
				var endPos = urlIndicator.End(position);

				var text = m_rScintilla_TextArea.GetTextRange(startPos, endPos - startPos).Trim();
				if (Uri.IsWellFormedUriString(text, UriKind.Absolute))
					return text;
			}

			return null;
		}


		private void m_rScintilla_TextArea_DwellStart(object sender, DwellEventArgs e)
		{
			var url = GetUrlAtPosition(e.Position);
			if (url != null) {
				var callTip = string.Format("{0}\nCTRL + click to follow link", url);
				callTip += "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";
				callTip = WrapMessageForToolTip(callTip);
				m_rScintilla_TextArea.CallTipShow(e.Position, callTip);
			}
		}

		private string WrapMessageForToolTip(string callTip)
		{
			//TODO_FR 199 Teste Linebreaks in dem ToolTip
			//Implement
			string sResult = callTip;
			return sResult;
		}

		private void m_rScintilla_TextArea_DwellEnd(object sender, DwellEventArgs e)
		{
			m_rScintilla_TextArea.CallTipCancel();
		}


		private void InitText()
		{
			// InitText
			m_rScintilla_TextArea.Text = "http://www.google.com";
			m_rScintilla_TextArea.CurrentPosition = 21;
			m_rScintilla_TextArea.AppendText("\nhttp://www.izurnal.cz");
			string sDebug_FistLine = m_rScintilla_TextArea.Lines[1].Text;
			//m_rScintilla_TextArea.AddText("\r\nhttp://www.izurnal.cz");
/*
			m_rScintilla_TextArea.Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";
			m_rScintilla_TextArea.GotoPosition(m_rScintilla_TextArea.Text.Length);
 			m_rScintilla_TextArea.AddText("\nTheView");
			m_rScintilla_TextArea.GotoPosition(m_rScintilla_TextArea.Text.Length);
*/
		}

	}
}
