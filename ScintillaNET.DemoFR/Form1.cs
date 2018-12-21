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
			m_rScintilla_TextArea.CharAdded += OnCharAdded;
			m_rScintilla_TextArea.AutoCSelection += OnScintilla_AutoCSelection;
//m_rScintilla_TextArea.AutoCCurrent
			m_rScintilla_TextArea.AutoCIgnoreCase = true;
//m_rScintilla_TextArea.autoc
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

		private void OnScintilla_AutoCSelection(object sender, AutoCSelectionEventArgs rAutoCSelectionEventArgs)
		{
			string sDebug = rAutoCSelectionEventArgs.Text;
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

		private void m_rScintilla_TextArea_DwellStart(object sender, DwellEventArgs rDwellEventArgs)
		{
			//TODO_FR #CodeEditor react an Dwelling on Autocompletion List Items
			int nPosition = rDwellEventArgs.Position;
			var callTip = string.Format("Tooltip for Position {0}", nPosition);
			m_rScintilla_TextArea.CallTipShow(rDwellEventArgs.Position, callTip);
		}

		private void m_rScintilla_TextArea_DwellEnd(object sender, DwellEventArgs e)
		{
			m_rScintilla_TextArea.CallTipCancel();
		}


		private void InitText()
		{
			m_rScintilla_TextArea.Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum. \nabc";
			m_rScintilla_TextArea.GotoPosition(m_rScintilla_TextArea.TextLength);
		}

	}
}
