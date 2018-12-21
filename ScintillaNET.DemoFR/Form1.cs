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
		}

		private void OnCharAdded(object sender, CharAddedEventArgs e)
		{
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

		private void SetIndicatorForURL()
		{
			//TODO_FR 199 Implement ToolTip between DwellStart und DwellEnd events
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

		private void InitDwelling()
		{
			//FRs Anmeldung von Dwell
			// BONUS: Configure call tips for the Dwell events
			m_rScintilla_TextArea.MouseDwellTime = 400;
			m_rScintilla_TextArea.DwellStart	+= m_rScintilla_TextArea_DwellStart;
			m_rScintilla_TextArea.DwellEnd		+= m_rScintilla_TextArea_DwellEnd;
		}

		//TODO_FR #CodeEditor OnCharAdded

		private void m_rScintilla_TextArea_DwellStart(object sender, DwellEventArgs e)
		{
//TODO_FR #CodeEditor ShowAutoCompletion
			//ShowAutoCompletion();
			var callTip = string.Format("nIntellisense");
			m_rScintilla_TextArea.CallTipShow(e.Position, callTip);
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
		}

	}
}
