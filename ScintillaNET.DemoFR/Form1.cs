using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Design;
using System.Text.RegularExpressions;

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
			m_rScintilla_TextArea.Size = m_rPanel.Size;
			//BeginUndoAction und EndUndoAction- beeinflusst nur den Umfang von "a single undo action"
			m_rScintilla_TextArea.BeginUndoAction();
			InitText();
			SelectSomeText();
			m_rScintilla_TextArea.EndUndoAction();
			// EmptyUndoBuffer - Grenze, bis wohin UNDO etwas zurücknehmen kann.
			m_rScintilla_TextArea.EmptyUndoBuffer();
			InitDwelling();
			SetIndicatorForURL();
			m_rScintilla_TextArea.CharAdded += OnCharAdded;
			m_rScintilla_TextArea.InsertCheck += OnInsertCheck;
			m_rScintilla_TextArea.AutoCSelection += OnScintilla_AutoCSelection;
			//m_rScintilla_TextArea.AutoCCurrent
			m_rScintilla_TextArea.AutoCIgnoreCase = true;
			m_rScintilla_TextArea.AutoCCompleted += OnScintilla_AutoCCompleted;
			//use tab and not as three spaces
			m_rScintilla_TextArea.UseTabs = true;
			UpdateCheckBoxReadOnly();
		}


		private void SelectSomeText()
		{
			//TODO_FR #CodeEditor select some text
			m_rScintilla_TextArea.SelectionStart = 50;
			m_rScintilla_TextArea.SelectionEnd = 55;
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

// https://gist.github.com/Ahmad45123/f2910192987a73a52ab4
		private void OnInsertCheck(object sender, InsertCheckEventArgs e)
		{
			if ((e.Text.EndsWith("" + "\r") || e.Text.EndsWith("" + "\n"))) {
				int nStartPos = m_rScintilla_TextArea.Lines[m_rScintilla_TextArea.LineFromPosition(m_rScintilla_TextArea.CurrentPosition)].Position;
				int nEndPos = e.Position;
				string sCurLineText = m_rScintilla_TextArea.GetTextRange(nStartPos, (nEndPos - nStartPos)); //Text until the caret so that the whitespace is always equal in every line.

				Match indent = Regex.Match(sCurLineText, "^[ \\t]*");
				e.Text = (e.Text + indent.Value);
				if (Regex.IsMatch(sCurLineText, "{\\s*$")) {
					e.Text = (e.Text + "\t");
				}
			}
		}

		//Codes for the handling the Indention of the lines.
		//They are manually added here until they get officially added to the Scintilla control.
		#region "CodeIndent Handlers"
			const int SCI_SETLINEINDENTATION = 2126;
			const int SCI_GETLINEINDENTATION = 2127;
			private void SetIndent(ScintillaNET.Scintilla scin, int line, int indent)
			{
				scin.DirectMessage(SCI_SETLINEINDENTATION, new IntPtr(line), new IntPtr(indent));
			}
			private int GetIndent(ScintillaNET.Scintilla scin, int line)
			{
				int nResult = 0;
				IntPtr rIntPtrIndent = scin.DirectMessage(SCI_GETLINEINDENTATION, new IntPtr(line), IntPtr.Zero);
				nResult = rIntPtrIndent.ToInt32();
				return nResult;
			}
		#endregion

		private void OnCharAdded(object sender, CharAddedEventArgs e)
		{
			Debug.WriteLine("Length of Text after OnCharAdded: {0}", m_rScintilla_TextArea.Text.Length);
			if (e.Char == '.') {
				ShowAutoCompletion();
			}
			//The '}' char.
			if (e.Char == 125) {
				int nCurLine = m_rScintilla_TextArea.LineFromPosition(m_rScintilla_TextArea.CurrentPosition);
		
				if (m_rScintilla_TextArea.Lines[nCurLine].Text.Trim() == "}") { //Check whether the bracket is the only thing on the line.. For cases like "if() { }".
					SetIndent(m_rScintilla_TextArea, nCurLine, GetIndent(m_rScintilla_TextArea, nCurLine) - 4);
				}
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
// https://stackoverflow.com/questions/22368434/best-way-to-split-string-into-lines-with-maximum-length-without-breaking-words
			string sResult = callTip;
			int nMaximumLineLength = 80;
			sResult = System.Text.RegularExpressions.Regex.Replace(callTip, @"(.{1," + nMaximumLineLength + @"})(?:\s|$)", "$1\n");
			sResult = sResult.TrimEnd('\n');
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
	
			m_rScintilla_TextArea.AppendText("\nSub ButtonFR_KlickAktion()");
			m_rScintilla_TextArea.AppendText("\nDim This : Set This = ButtonFR");
			m_rScintilla_TextArea.AppendText("\n\tGlobal.MsgBox(\"Hello world!\")");
			m_rScintilla_TextArea.AppendText("\n\trem Global.MsgBox(\"Hello world!\")");
			m_rScintilla_TextArea.AppendText("\n\tremGlobal.MsgBox(\"Hello world!\")");
			m_rScintilla_TextArea.AppendText("{");
			m_rScintilla_TextArea.AppendText("\naaa");
			m_rScintilla_TextArea.AppendText("\nbbb}sofort_C");
			m_rScintilla_TextArea.AppendText("\nEnd Sub");
			SetScintillaReadOnly(true);
			m_rScintilla_TextArea.AppendText("\nText nach dem ich ReadOnly auf true gesetzt habe - AppendText");
			m_rScintilla_TextArea.InsertText(m_rScintilla_TextArea.TextLength, "\nInserting Text");
			SetScintillaReadOnly(false);
			m_rScintilla_TextArea.AppendText("\nText nach dem ich ReadOnly auf FALSE gesetzt habe - AppendText");
			m_rScintilla_TextArea.InsertText(m_rScintilla_TextArea.TextLength, "\nInserting Text");
		}

		private void m_rButtonSearch_Click(object sender, EventArgs e)
		{
			//http wird zwischen 0 und 3 nicht gefunden
			string sSearchFor = "http";
			m_rScintilla_TextArea.TargetStart = 0;
			m_rScintilla_TextArea.TargetEnd = 3;
			int nPositionFound = m_rScintilla_TextArea.SearchInTarget(sSearchFor);
			Debug.WriteLine(System.String.Format("nPositionFound: {0}_", nPositionFound));
			m_rScintilla_TextArea.TargetEnd = 4;
			nPositionFound = m_rScintilla_TextArea.SearchInTarget(sSearchFor);
			Debug.WriteLine(System.String.Format("TargetEnd 4: nPositionFound: {0}", nPositionFound));
			m_rScintilla_TextArea.TargetEnd = 100;
			nPositionFound = m_rScintilla_TextArea.SearchInTarget(sSearchFor);
			Debug.WriteLine(System.String.Format("TargetEnd 100: nPositionFound: {0}; m_rScintilla_TextArea.TargetEnd aktualisiert: {1}", nPositionFound, m_rScintilla_TextArea.TargetEnd));
		}

		private void m_rButtonSetReadOnly_Click(object sender, EventArgs e)
		{
			SetScintillaReadOnly(true);
		}

		private void SetScintillaReadOnly(bool bSetReadOnly)
		{
			m_rScintilla_TextArea.ReadOnly = bSetReadOnly;
			Color oColorBackground = System.Drawing.Color.White;
			if (bSetReadOnly)
				oColorBackground = System.Drawing.SystemColors.Control;

			//Styles durchiterieren und BackColor setzen
			// Why is BackColor not supported?  https://github.com/jacobslusser/ScintillaNET/issues/63
			foreach (Style rStyle in m_rScintilla_TextArea.Styles) {
				rStyle.BackColor = oColorBackground;
			}
			if (!bSetReadOnly) {
				m_rScintilla_TextArea.Styles[Style.CallTip].ForeColor = SystemColors.InfoText;
				m_rScintilla_TextArea.Styles[Style.CallTip].BackColor = SystemColors.Info;
			}

			UpdateCheckBoxReadOnly();
		}

		private void UpdateCheckBoxReadOnly()
		{
			m_rCheckBoxReadOnly.Checked = m_rScintilla_TextArea.ReadOnly;
		}

		private void m_rButtonSetReadWrite_Click(object sender, EventArgs e)
		{
			SetScintillaReadOnly(false);
		}
	}
}
