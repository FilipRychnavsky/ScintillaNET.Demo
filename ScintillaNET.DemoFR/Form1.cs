﻿using System;
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

		/// <summary> 
		/// Grund für den Hintergrund der Zeilen ; Verwendet in Remarks von Line
		/// </summary>
		///<remarks>fr20200811 SI 415057</remarks>
		private enum ELineMarkersPurpose
		{
			ReadOnly = 0,
			CompilationStatus = 1
		}

		/// <summary> 
		/// Benutzt für die Auswahl der Farbe des Hintergrund der Zeilen 
		/// </summary>
		/// <remarks>fr20200811 SI 415057</remarks>
		public enum ECompilationStatus
		{
			OK = 0,
			Warning = 1,
			Error = 2
		}

		public enum ELineMarkersIDs
		{
			ReadWrite = 0,
			ReadOnly = 1,
			Error = 2,
			Warning = 3
		}

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
			//SetIndicatorForURL();
			m_rScintilla_TextArea.Lexer = Lexer.Cpp;
			InitStyles();
			SetCSharpKeyWords();
			SetDotNetKeywords();
			SetVbKeywords();
			SetDotNetKeywords();
			Colorize();

			m_rScintilla_TextArea.CharAdded += OnCharAdded;
			m_rScintilla_TextArea.InsertCheck += OnInsertCheck;
			m_rScintilla_TextArea.AutoCSelection += OnScintilla_AutoCSelection;
			//m_rScintilla_TextArea.AutoCCurrent
			m_rScintilla_TextArea.AutoCIgnoreCase = true;
			m_rScintilla_TextArea.AutoCCompleted += OnScintilla_AutoCCompleted;
			//use tab and not as three spaces
			m_rScintilla_TextArea.UseTabs = true;
			m_rScintilla_TextArea.UpdateUI += OnScintilla_UpdateUI;
			this.LostFocus += OnForm_LostFocus;
			m_rScintilla_TextArea.LostFocus += OnScintilla_TextArea_LostFocus;
			this.m_rPanel.Leave += OnPanel_Leave;
			this.Deactivate += OnForm_Deactivate;
			DefineLineMarkers();
			UpdateCheckBoxReadOnly();
		}

		private void OnForm_Deactivate(object sender, EventArgs e)
		{
			
		}

		private void OnPanel_Leave(object sender, EventArgs e)
		{
			//Debug.WriteLine("OnPanel_Leave");
		}

		// LostFocus kann ich bei Scintilla anlaufen
		private void OnScintilla_TextArea_LostFocus(object sender, EventArgs e)
		{
			//Debug.WriteLine("OnScintilla_TextArea_LostFocus");
		}

		// Verlust von Focus von OnForm konnte nicht angelaufen werden.
		private void OnForm_LostFocus(object sender, EventArgs e)
		{
			//Debug.WriteLine("OnForm_LostFocus");
		}

		private void DefineLineMarkers()
		{
			m_rScintilla_TextArea.Markers[(int)ELineMarkersIDs.ReadOnly].Symbol = MarkerSymbol.Background;
			m_rScintilla_TextArea.Markers[(int)ELineMarkersIDs.ReadOnly].SetBackColor(System.Drawing.Color.LightGray);
			m_rScintilla_TextArea.Markers[(int)ELineMarkersIDs.Error].Symbol = MarkerSymbol.Arrow;
			m_rScintilla_TextArea.Markers[(int)ELineMarkersIDs.Error].SetBackColor(System.Drawing.Color.Red);
			m_rScintilla_TextArea.Markers[(int)ELineMarkersIDs.Warning].Symbol = MarkerSymbol.Arrow;
			m_rScintilla_TextArea.Markers[(int)ELineMarkersIDs.Warning].SetBackColor(System.Drawing.Color.Yellow);
		}

		private void OnScintilla_UpdateUI(object sender, UpdateUIEventArgs e)
		{
			//System.Diagnostics.Debug.WriteLine(System.String.Format("on UpdateUI: {0}", e.ToString()));
		}

		private void SetDotNetKeywords()
		{
			m_rScintilla_TextArea.SetKeywords(2, "Control ");
		}

		private void Colorize()
		{
			m_rScintilla_TextArea.Colorize(0, m_rScintilla_TextArea.Text.Length);
		}

		/// <summary>
		/// <remark>https://github.com/jacobslusser/ScintillaNET/wiki/Automatic-Syntax-Highlighting</remark>
		/// </summary>
		private void InitStyles()
		{
			m_rScintilla_TextArea.StyleResetDefault();
			m_rScintilla_TextArea.Styles[Style.Default].Font = "Consolas";
			m_rScintilla_TextArea.Styles[Style.Default].Size = 13;
			m_rScintilla_TextArea.StyleClearAll();
#if (false)
			m_rScintilla_TextArea.Styles[1].ForeColor = Color.Green; //Comment Also /* */ mehrzeilig in C#
			m_rScintilla_TextArea.Styles[2].ForeColor = Color.Green; //Comment Line  Also // in C# | ' und rem in VB
			m_rScintilla_TextArea.Styles[3].ForeColor = Color.Green; //Comment Block (VB Keywords 0) Also /* */ einzeilig in C#
			m_rScintilla_TextArea.Styles[4].ForeColor = Color.Magenta; //Zahlen
			m_rScintilla_TextArea.Styles[5].ForeColor = Color.Blue; //KeywordSet 0
			m_rScintilla_TextArea.Styles[6].ForeColor = Color.Magenta; //Strings
			m_rScintilla_TextArea.Styles[7].ForeColor = Color.Magenta; //Character (VB Default)
//FR 20190115 14:49:42 SI366246 Style für verbatim string literals
			m_rScintilla_TextArea.Styles[ScintillaNET.Style.Cpp.Verbatim].ForeColor = Color.Magenta;
			//FR 20200131 SI 376537 Farbenanpassungen
			m_rScintilla_TextArea.Styles[ScintillaNET.Style.Cpp.Preprocessor].ForeColor = Color.FromArgb(128, 64, 0);
			m_rScintilla_TextArea.Styles[10].ForeColor = Color.Black; // Operator (VB Keywords 1)
			m_rScintilla_TextArea.Styles[11].ForeColor = Color.Black; // Identifier (VB Keywords 2)
			m_rScintilla_TextArea.Styles[16].ForeColor = Color.Blue; //Keywordset 1
			m_rScintilla_TextArea.Styles[17].ForeColor = Color.Red;
			m_rScintilla_TextArea.Styles[18].ForeColor = Color.Blue; //Keywordset 1
			m_rScintilla_TextArea.Styles[ScintillaNET.Style.Cpp.GlobalClass].ForeColor = Color.FromArgb(128,0,255);
#else
			// Beispiel von der Webseite für Syntax Highlighting
			// Configure the CPP (C#) lexer styles
			m_rScintilla_TextArea.Styles[Style.Cpp.Default].ForeColor = Color.Silver;
			m_rScintilla_TextArea.Styles[Style.Cpp.Comment].ForeColor = Color.FromArgb(0, 128, 0); // Green
			m_rScintilla_TextArea.Styles[Style.Cpp.CommentLine].ForeColor = Color.FromArgb(0, 128, 0); // Green
			m_rScintilla_TextArea.Styles[Style.Cpp.CommentLineDoc].ForeColor = Color.FromArgb(128, 128, 128); // Gray
			m_rScintilla_TextArea.Styles[Style.Cpp.Number].ForeColor = Color.Olive;
			m_rScintilla_TextArea.Styles[Style.Cpp.Word].ForeColor = Color.Blue;
			m_rScintilla_TextArea.Styles[Style.Cpp.Word2].ForeColor = Color.Blue;
			m_rScintilla_TextArea.Styles[Style.Cpp.String].ForeColor = Color.FromArgb(163, 21, 21); // Red
			m_rScintilla_TextArea.Styles[Style.Cpp.Character].ForeColor = Color.FromArgb(163, 21, 21); // Red
			m_rScintilla_TextArea.Styles[Style.Cpp.Verbatim].ForeColor = Color.FromArgb(163, 21, 21); // Red
			m_rScintilla_TextArea.Styles[Style.Cpp.StringEol].BackColor = Color.Pink;
			m_rScintilla_TextArea.Styles[Style.Cpp.Operator].ForeColor = Color.Purple;
			m_rScintilla_TextArea.Styles[Style.Cpp.Preprocessor].ForeColor = Color.Maroon;
#endif
		}

		private static string CreateStringWithGlobalKeywords()
		{
			return "Global TheView APEMail APWord APExcel APSupportInfos APTermine Dialog GlobalObject PDFDriver Query StdWaehrungsKuerzel TheFrame ";
		}

		private string CreateStringWithDotNetKeywords()
		{
			return "Control ";
		}

		/// <summary>
		/// Intro into Automatic Syntax Highlighting https://github.com/jacobslusser/ScintillaNET/wiki/Automatic-Syntax-Highlighting ;
		/// Übersicht der Keyword sets: string sKeywordSets = m_rScintilla_TextArea.DescribeKeywordSets();
		/// </summary>
		private void SetCSharpKeyWords()
		{
			// Primary keywords and identifiers
			m_rScintilla_TextArea.SetKeywords(0,
				"abstract as base break case catch checked continue default delegate do else event explicit extern false finally fixed for foreach goto if implicit in interface internal is lock namespace new null object operator out override params private protected public readonly ref return sealed sizeof stackalloc switch this throw true try typeof unchecked unsafe using virtual while");
			// Secondary keywords and identifiers
			m_rScintilla_TextArea.SetKeywords(1,
				"control bool byte char class const decimal double enum float int long partial sbyte short static string struct uint ulong ushort void");
			// Global classes and typedefs
			//FR20181023 15:58:04 SI 352281 LE 23.10.2018 14:16:10
			m_rScintilla_TextArea.SetKeywords(3, System.String.Concat(CreateStringWithDotNetKeywords() + CreateStringWithGlobalKeywords()));
		}

		private void SetVbKeywords()
		{
			m_rScintilla_TextArea.SetKeywords(0,
				"addhandler addressof alias and andalso as by byref byval call case catch class const continue declare default delegate dim directcast do each else elseif end endif erase error exit false finally for friend function get gettype global gosub goto handles if implements imports in inherits interface is isnot let lib like loop me mod module mustinherit mustoverride mybase myclass namespace narrowing new next not nothing notinheritable notoverridable of on operator option optional or orelse out overloads overridable overrides paramarray partial private property protected public raiseevent readonly redim rem removehandler resume return select set shadows shared static step stop strict structure sub synclock then throw to true try trycast typeof using variant wend when while widening with withevents writeonly xor attribute begin currency implement load lset rset type unload aggregate ansi assembly async auto await binary compare custom distinct equals explicit from group into isfalse istrue iterator join key mid off order preserve skip take text unicode until where yield");
			m_rScintilla_TextArea.SetKeywords(1,
				"boolean byte cbool cbyte cchar cdate cdbl cdec char cint clng cobj csbyte cshort csng cstr ctype cuint culng cushort date decimal double enum event integer long object sbyte short string uinteger ulong ushort");
			//FR20181023 15:58:04 SI 352281 LE 23.10.2018 14:16:10
			m_rScintilla_TextArea.SetKeywords(3,
				System.String.Concat(CreateStringWithDotNetKeywords().ToLower() + CreateStringWithGlobalKeywords().ToLower()));
		}


		private void SelectSomeText()
		{
			m_rScintilla_TextArea.SelectionStart = 50;
			m_rScintilla_TextArea.SelectionEnd = 55;
		}

		private void SetIndicatorForURL()
		{
			//ToolTip in AutoCompletion m_rScintilla_TextArea.AutoCShow(nLengthEntered, sAutoCompletionList);
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
			if (e.Char == ' ' && ((ModifierKeys & Keys.Control) == Keys.Control)) {
				ShowAutoCompletion();
			}
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

		string GetAutocompletionListFromParsedText()
		{
			string sResult = "";
			//find all words of the text
			// Einen Dictionary kann man verwenden, um Duplizitäten zu vermeiden
			var mWords = new System.Collections.Generic.Dictionary<string, string>();
			foreach (Match m in Regex.Matches(m_rScintilla_TextArea.Text, @"\b\w+\b")) {
				mWords[m.Value] = m.Value;
			}
			foreach (var word in mWords.Keys)
				sResult += string.Format(" {0}", word.ToString());
			return sResult;
		}

		private void ShowAutoCompletion()
		{
			// Find the word start
			int nCurrentPosition = m_rScintilla_TextArea.CurrentPosition;
			int nWordStartPosition = m_rScintilla_TextArea.WordStartPosition(nCurrentPosition, true);
			// Display the autocompletion list
			int nLengthEntered = nCurrentPosition - nWordStartPosition;
			//string sAutoCompletionList = "aaa bbb ccc dddd eee";
			// Idee - wenn das vorherige wort nicht alphabetisch vorher ist, kann autocompletion es nicht finden
			string sAutoCompletionList = "ab?2 ac?2 a1e?2 a2e?2 ae?2 ad?2 af?2 fff?2 aaa?2 bbb?2 ddd?2 ccc?2 eee?2 fff?2 ggg?2 hhh?2 iii?2 jjj?2 kkk?2 lll?2 mmm?2 nnn?2 ooo?2 ppp?2 qqq?2 rrr?2 sss?2 ttt?2 uuu?2 vvv?2 wwww?2 xxx?2 yyy?2 zzz?2";
			//string sAutoCompletionList = GetAutocompletionListFromParsedText();
			//SI 392112 Underscore debug values for Autocompletion
			sAutoCompletionList += " S_KopfLagerUmplanung?2 S_KopfLagerZubuchung?2 S_KopfLagerZubuchungExtra?2 S_PositionenLagerAbbuchung?2 S_PositionenLagerAbbuchungSchwund?2 S_PositionenLagerUmplanung?2 S_ProduktionenLagerRefresh?2 SetKopfArtikelEP?2 SetKopfLagerID?2 SetPositionenLagerID?2";
			sAutoCompletionList = sAutoCompletionList.Trim();
			// #CodeEditor SI383424 Setzte die Eingabetaste und "(" als FillUps. Sie dienen der Übernahme der Auswahl aus einer Autocompletion. 
			// https://www.scintilla.org/ScintillaDoc.html#Autocompletion
			m_rScintilla_TextArea.AutoCSetFillUps("\n(");
			//TODO_FR #try to parse list of autocompletion  https://github.com/jacobslusser/ScintillaNET/wiki/Custom-Autocompletion
			//TODO_FR #CodeEditor Kann AutoCOrder oddities in Autocompletion verursachen, wenn die Items nicht in der alphabetischen Reihenfolge sind?
			// m_rScintilla_TextArea.AutoCOrder = Order.Presorted;
			// Wenn man PerformSort ausübt, funktioniert die Navigation mit Anfangsbuchstaben in Autocompletion.
			//https://stackoverflow.com/questions/18564284/scintillanet-auto-completion-list-issue
			//Scintilla likes the list in sorted order.	
			m_rScintilla_TextArea.AutoCOrder = Order.PerformSort;
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
			m_rScintilla_TextArea.DwellStart += m_rScintilla_TextArea_DwellStart;
			m_rScintilla_TextArea.DwellEnd += m_rScintilla_TextArea_DwellEnd;
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
#if false
			#region MyRegion
			var url = GetUrlAtPosition(e.Position);
			if (url != null) {
				var callTip = string.Format("{0}\nCTRL + click to follow link", url);
				callTip += "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";
				callTip = WrapMessageForToolTip(callTip);
				m_rScintilla_TextArea.CallTipShow(e.Position, callTip);
			}
			#endregion
#endif
			m_rScintilla_TextArea.CallTipShow(e.Position, "Demo ToolTip");
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
			/*
						m_rScintilla_TextArea.Text = "http://www.google.com";
						m_rScintilla_TextArea.CurrentPosition = 21;
						m_rScintilla_TextArea.AppendText("\nhttp://www.izurnal.cz");
						string sDebug_FistLine = m_rScintilla_TextArea.Lines[1].Text;
			*/
			//m_rScintilla_TextArea.AddText("\r\nhttp://www.izurnal.cz");
			/*
						m_rScintilla_TextArea.Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";
						m_rScintilla_TextArea.GotoPosition(m_rScintilla_TextArea.Text.Length);
						m_rScintilla_TextArea.AddText("\nTheView");
						m_rScintilla_TextArea.GotoPosition(m_rScintilla_TextArea.Text.Length);
			*/
			/*
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
		*/
			m_rScintilla_TextArea.AppendText("void abstract MyFunction()");
			m_rScintilla_TextArea.AppendText("\n{");
			m_rScintilla_TextArea.AppendText("\n\tint n = 0;");
			m_rScintilla_TextArea.AppendText("\n\tGlobal.Query();");
			m_rScintilla_TextArea.AppendText("\n\tstring sMyString;");
			m_rScintilla_TextArea.AppendText("\n\tsMyString = \"hello world\"");
			m_rScintilla_TextArea.AppendText("\n\tTheView ;");
			m_rScintilla_TextArea.AppendText("\n\tQuery()");
			m_rScintilla_TextArea.AppendText("\n\t// my comment");
			m_rScintilla_TextArea.AppendText("\n\t <> = - + ");
			m_rScintilla_TextArea.AppendText("\n\t abstract as base control Control break case catch checked continue default delegate do else event explicit extern fa");
			m_rScintilla_TextArea.AppendText("\n\tGlobal.UIMessageBox( "); // Klammer und ein Leerzeichen
			m_rScintilla_TextArea.AppendText("\n}");
			m_rScintilla_TextArea.AppendText("//Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.");
			m_rScintilla_TextArea.AppendText("\npublic void Button_KlickAktion() {");
			m_rScintilla_TextArea.AppendText("\n\t\t// Kommentarzeile 01");
			m_rScintilla_TextArea.AppendText("\n\t\t// Kommentarzeile 02");
			m_rScintilla_TextArea.AppendText("\n\t\t// Kommentarzeile 03");
			m_rScintilla_TextArea.AppendText("\n\t\t// Kommentarzeile 04");
			m_rScintilla_TextArea.AppendText("\n\t\t// Kommentarzeile 05");
			m_rScintilla_TextArea.AppendText("\n\t\t// Kommentarzeile 06");
			m_rScintilla_TextArea.AppendText("\n\t\t// Kommentarzeile 07");
			m_rScintilla_TextArea.AppendText("\n\t\t// Kommentarzeile 08");
			m_rScintilla_TextArea.AppendText("\n\t\t// Kommentarzeile 09");
			m_rScintilla_TextArea.AppendText("\n\t\t// Kommentarzeile 10");
			m_rScintilla_TextArea.AppendText("\n\t\t// Kommentarzeile 11");
			m_rScintilla_TextArea.AppendText("\n\t\t// Kommentarzeile 12");
			m_rScintilla_TextArea.AppendText("\n\t\t// Kommentarzeile 13");
			m_rScintilla_TextArea.AppendText("\n\t\t// Kommentarzeile 14");
			m_rScintilla_TextArea.AppendText("\n\t\t// Kommentarzeile 15");
			m_rScintilla_TextArea.AppendText("\n\t\t// Kommentarzeile 16");
			m_rScintilla_TextArea.AppendText("\n\t\t// Kommentarzeile 17");
			m_rScintilla_TextArea.AppendText("\n\t\t// Kommentarzeile 18");
			m_rScintilla_TextArea.AppendText("\n\t\t// Kommentarzeile 19");
			m_rScintilla_TextArea.AppendText("\n\t\t// Kommentarzeile 20");
			m_rScintilla_TextArea.AppendText("\n}");
			m_rScintilla_TextArea.AppendText("\n");
			m_rScintilla_TextArea.AppendText("\n");
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
		private void SetBackgroundColor(Color oColorBackground)
		{
			//Styles durchiterieren und BackColor setzen
			// Why is BackColor not supported?  https://github.com/jacobslusser/ScintillaNET/issues/63
			foreach (Style rStyle in m_rScintilla_TextArea.Styles) {
				rStyle.BackColor = oColorBackground;
			}
		}

		private void SetScintillaReadOnly(bool bSetReadOnly)
		{
			m_rScintilla_TextArea.ReadOnly = bSetReadOnly;
			Color oColorBackground = System.Drawing.Color.White;
			if (bSetReadOnly)
				oColorBackground = System.Drawing.SystemColors.Control;

			SetBackgroundColor(oColorBackground);
			if (!bSetReadOnly) {
				m_rScintilla_TextArea.Styles[Style.CallTip].ForeColor = SystemColors.InfoText;
				m_rScintilla_TextArea.Styles[Style.CallTip].BackColor = SystemColors.Info;
			}

			UpdateCheckBoxReadOnly();
			// gc. Es gibt aber kein Unterschied zwischen user r/o und user-ad-programmatic r/o. https://github.com/jacobslusser/ScintillaNET/issues/360
			string sVorher = m_rScintilla_TextArea.Text;
			m_rScintilla_TextArea.Text += "Test während ro";
			if (sVorher.Equals(m_rScintilla_TextArea.Text)) {
				DebugAnnotations();
				Debug.WriteLine("der status ist ro, per code text wurde nicht erweitert");
			}
		}

		private void UpdateCheckBoxReadOnly()
		{
			m_rCheckBoxReadOnly.Checked = m_rScintilla_TextArea.ReadOnly;
		}

		private void m_rButtonSetReadWrite_Click(object sender, EventArgs e)
		{
			SetScintillaReadOnly(false);
		}


		private void m_rCheckBoxReadOnly_CheckedChanged(object sender, EventArgs e)
		{

		}

		private void m_rButtonCopyIntoClipboard_Click(object sender, EventArgs e)
		{
			Clipboard.SetDataObject("test");
		}

		private void m_rButtonReadFromClipboard_Click(object sender, EventArgs e)
		{
			string sFoundClipboardText = "Could not retrieve data off the clipboard.";
			// Declares an IDataObject to hold the data returned from the clipboard.
			// Retrieves the data from the clipboard.
			IDataObject iData = Clipboard.GetDataObject();
			// Determines whether the data is in a format you can use.
			if (iData.GetDataPresent(DataFormats.Text)) {
				// Yes it is, so display it in a text box.
				sFoundClipboardText = (String)iData.GetData(DataFormats.Text);
				bool bContainsRtf = Clipboard.ContainsText(TextDataFormat.Rtf);
				sFoundClipboardText = System.String.Format("contains TextDataFormat.Rtf: {0}\n{1}", bContainsRtf, sFoundClipboardText);
			}
			if (iData.GetDataPresent(DataFormats.Rtf)) {
				// Yes it is, so display it in a text box.
				sFoundClipboardText = (String)iData.GetData(DataFormats.Rtf);
				bool bContainsRtf = Clipboard.ContainsText(TextDataFormat.Rtf);
				sFoundClipboardText = System.String.Format("contains TextDataFormat.Rtf: {0}\n{1}", bContainsRtf, sFoundClipboardText);
			}
			System.Windows.Forms.MessageBox.Show(sFoundClipboardText);
		}

		private void m_rButtonCopyRTF_Click(object sender, EventArgs e)
		{
			m_rScintilla_TextArea.Copy(CopyFormat.Rtf);
		}

		private void m_rButton_GoToLine_Click(object sender, EventArgs e)
		{
			Debug.WriteLine("Going to line 3");
			m_rScintilla_TextArea.Lines[3 - 1].Goto();
			m_rScintilla_TextArea.Focus();
		}

		private void m_rButtonTestDeleteRange_Click(object sender, EventArgs e)
		{
			const int nLengthToDelete = 102;
			m_rScintilla_TextArea.DeleteRange(0, nLengthToDelete);
		}


		private void ClearAllLineMarkers()
		{
			foreach (var rLine in m_rScintilla_TextArea.Lines) {
				if (rLine.MarkerGet() > 0) {
					// Es gibt was zu löschen ; wir haben bisher 4 Marker
					for (int nMarkerID = 0; nMarkerID < 4; nMarkerID++) {
						rLine.MarkerDelete(nMarkerID);
					}
				} else {
					// Do nothing
				}
			}
		}

		private void m_rButtonSetBackgroundForSomeLines_Click(object sender, EventArgs e)
		{
			ClearAllLineMarkers();
			m_rScintilla_TextArea.Lines[1].MarkerAdd((int)ELineMarkersIDs.ReadOnly);
			m_rScintilla_TextArea.Lines[2].MarkerAdd((int)ELineMarkersIDs.Error);
			m_rScintilla_TextArea.Lines[3].MarkerAdd((int)ELineMarkersIDs.Warning);
		}

		/// <summary>
		/// Test aus dem ALPHAPLAN
		/// </summary>
		private void GetWordEntered()
		{
			string sWordEntered = string.Empty;
			int nWordStartPosition = m_rScintilla_TextArea.CurrentPosition;
			// folgende Zeile kann über OnKeyDown angelaufen werden, wenn man Strg + Leerzeichen in der Mitte eines Wortes betätigt
			if (m_rScintilla_TextArea.GetCharAt(nWordStartPosition - 1) != '.') {
				nWordStartPosition = m_rScintilla_TextArea.WordStartPosition(m_rScintilla_TextArea.CurrentPosition, true);
			}
			/*WordStartPosition wird auch mit dem onlyWordCharacters true Parameter gestartet.
				Wenn man "if(**." eingibt, dann findet WordStartPosition(nPos, false) die Klammer "(" - Anfang der non-word boundary 
				(die Suche wurde für "." - non-word gestartet) . 
				WordStartPosition(nPos, true) findet "." (erstes non-word Zeichen)*/
			int nWordStartPosition_OnlyWordCharacters = nWordStartPosition;
			// Suche nach dem Wort vor dem ersten "."
			while (m_rScintilla_TextArea.GetCharAt(nWordStartPosition - 1) == '.') {
				nWordStartPosition = m_rScintilla_TextArea.WordStartPosition(nWordStartPosition - 1, false);
			}
			//if(m_rScintilla_TextArea.GetCharAt(nWordStartPosition) == '.') {
			//  sWordEntered = m_rScintilla_TextArea.GetWordFromPosition(nWordStartPosition - 1);
			//}
			while (m_rScintilla_TextArea.GetCharAt(nWordStartPosition_OnlyWordCharacters - 1) == '.') {
				// Der zweite Parameter von WordStartPosition ist onlyWordCharacters - die Suche endet bei erstem Zeichen, das nicht zu einem Wort gehört.
				// Egal, ob die Suche auf einem nicht-Wort Zeichen - z.B. ein Punkt - gestartet wurde.
				nWordStartPosition_OnlyWordCharacters = m_rScintilla_TextArea.WordStartPosition(nWordStartPosition_OnlyWordCharacters - 1, true);
			}

			// IsRangeWord liefert mir false, obwohl das Wort existiert
			// bool bIsWordEntered_IsRangeWord = m_rScintilla_TextArea.IsRangeWord(nWordStartPosition, m_rScintilla_TextArea.CurrentPosition - nWordStartPosition - 1);

			sWordEntered = m_rScintilla_TextArea.GetTextRange(nWordStartPosition, m_rScintilla_TextArea.CurrentPosition - nWordStartPosition - 1);
			// Wenn der Punkt nicht der Anfang der non-word boundary ist, dann gibt es vor dem Punkt kein word.
			bool bIsWordEntered = nWordStartPosition == nWordStartPosition_OnlyWordCharacters;
			if (!bIsWordEntered)
				if (sWordEntered == ")") {
					// #CodeEditor SI 380949 Autocompletion für Klammern nach einer Methode anbieten
					string rsPatternSymbols = @"\S*[(][\s\S]*?[)]";
					RegexOptions rRegexOptions = RegexOptions.Multiline;
					// Nimm die aktuelle Zeile, aber nur bis die aktuelle Position
					// range: <Start der Zeile - Welche Position, Aktuelle Position>
					int nStartOfTheLine = m_rScintilla_TextArea.Lines[m_rScintilla_TextArea.LineFromPosition(nWordStartPosition_OnlyWordCharacters)].Position;
					string sInput = m_rScintilla_TextArea.GetTextRange(nStartOfTheLine, nWordStartPosition_OnlyWordCharacters - nStartOfTheLine + 1);
					foreach (Match rMatch in Regex.Matches(sInput, rsPatternSymbols, rRegexOptions)) {
						Console.WriteLine("'{0}' found at index {1}.", rMatch.Value, rMatch.Index);
						sWordEntered = rMatch.Value;
					}
				} else {
					sWordEntered = "";
				}
			// Beispiel System.DateTime.Now.AddDays(1). - System.DateTime Methoden sollen angeboten werden.
			Debug.WriteLine(string.Format("WordEntered: {0}", sWordEntered));
		}

		private void DebugWordsAroundCurrentPosition()
		{
			var pos = m_rScintilla_TextArea.CurrentPosition;

			var wordStart = m_rScintilla_TextArea.WordStartPosition(pos, true);
			var wordEnd = m_rScintilla_TextArea.WordEndPosition(pos, true);
			var word = m_rScintilla_TextArea.GetTextRange(wordStart, wordEnd - wordStart);

			var prevWordStart = m_rScintilla_TextArea.WordStartPosition(wordStart - 1, true);
			var prevWordEnd = m_rScintilla_TextArea.WordEndPosition(prevWordStart, true);
			var prevWord = m_rScintilla_TextArea.GetTextRange(prevWordStart, prevWordEnd - prevWordStart);

			var nextWordStart = m_rScintilla_TextArea.WordStartPosition(wordEnd + 1, true);
			var nextWordEnd = m_rScintilla_TextArea.WordEndPosition(nextWordStart, true);
			var nextWord = m_rScintilla_TextArea.GetTextRange(nextWordStart, nextWordEnd - nextWordStart);
			Debug.WriteLine(string.Format("prevWord, word, nextWord: {0}, {1}, {2}", prevWord, word, nextWord));
			Debug.WriteLine(string.Format("m_rScintilla_TextArea.GetWordFromPosition(pos): {0}", m_rScintilla_TextArea.GetWordFromPosition(pos)));
			//TODO_FR Debug GetWordEntered Logik von ALPHAPLAN
		}
		private void m_rButtonAutocompletionTest_Click(object sender, EventArgs e)
		{
			m_rScintilla_TextArea.ClearAll();
			// Navigate auf die (2).System.DateTime.Now.AddDays(2).
			m_rScintilla_TextArea.Text = "public void ButtonFR1_KlickAktion() {" + "\r" +
																		"\tvar This = ButtonFR1;" + "\r" +
																		"\tvar rAnotherButton = ButtonFR2;" + "\r" +
																		"\t// Kommentar 1" + "\r" +
																		"\t// Kommentar 2" + "\r" +
																		"\t//System.DateTime.Now." + "\r" +
																		"\tSystem.DateTime.Now.AddDays(2)." + "\r" +
																		"\tSystem.DateTime.Now.AddDays(2)." + " bla bla System.DateTime.Now.AddDays(1).AddHours(1)" +  "\r" +
																		"\tSystem.DateTime.Now." + "\r" +
																		"\tif(**." + "\r" +
																		"\tDemoFilip();" + "\r" +
																	"}" +
																	"\r\r";
			// Ermittle das Wort
			// Autocompletion
			m_rScintilla_TextArea.CurrentPosition = 182;
			//ShowAutoCompletion();
			DebugWordsAroundCurrentPosition();
		}

		///<summary>Deletes Text. It means replace Text with an empty string.</summary>
		private void DeleteText()
		{
			// Postion of comment: 126 and 127
			string sComment = @"//";
			int nLineStartingPosition = 126;
			m_rScintilla_TextArea.TargetStart = nLineStartingPosition;
			m_rScintilla_TextArea.TargetEnd = nLineStartingPosition + sComment.Length;
			// Der Grund für die Assertion ist, dass man einen String // durch einen leeren String ersetzt. 
			// In der Methode SCI_REPLACETARGET des SciLexer Editor.css Zeile 5989 wird überprüft, ob der Parameter nicht leer ist, was hier der Fall ist.
			if (m_rScintilla_TextArea.SearchInTarget(sComment) != -1)
				m_rScintilla_TextArea.ReplaceTarget("");
		}

		void ScrollDemo()
		{
			//ScrollRangeDemo Scroll to position 267 - nach dem Wort extern, Ln. 12
			//m_rScintilla_TextArea.ScrollRange(267, 268);
			// Scroll to some high number to get some whitespace down.
			// It does not work. - Bcause of clamping
			//m_rScintilla_TextArea.ScrollRange(2000, 3000);
			//SI416766 Scroll VISIBLE position to the top; f.e. the position  297 vor dem "lorem ipsum" ; SCI_LINESCROLL
			//m_rScintilla_TextArea.ScrollRange(297, 1317);
			int nPositionToNavigate = 297;
			// Ich mache zwei Operationen zugleich. Ganz nach oben zu scrollen und dann zu der Position navigieren.
			int nDeltaLinesUpwards = m_rScintilla_TextArea.LineFromPosition(nPositionToNavigate) - m_rScintilla_TextArea.FirstVisibleLine;
			m_rScintilla_TextArea.LineScroll(nDeltaLinesUpwards, 0);
		}

		private void DebugAnnotations()
		{
			string rsDebug = "Line | Annotations | Text\n";
			for (int i = 0; i < m_rScintilla_TextArea.Lines.Count; i++) {
				rsDebug +=System.String.Format("{0}|{1}|{2}", i, m_rScintilla_TextArea.Lines[i].AnnotationText, m_rScintilla_TextArea.Lines[i].Text);
			}
			Debug.WriteLine(rsDebug);
		}
		private void SetAnnotationOnEachLine()
		{
			for (int i = 0; i < m_rScintilla_TextArea.Lines.Count; i++) {
				m_rScintilla_TextArea.Lines[i].AnnotationText = System.String.Format("This is an Annotation for Line {0}.", i);
			}
		}

		private void InsertTextDemo()
		{
			// Documenting inserting text with and without single quote. Influence on Annotations
			SetAnnotationOnEachLine();
#if false
			DebugAnnotations();

			// This insert new Text on the end of the line 2.
			Debug.WriteLine("\nInserting Text in the middle; End of Line 2");
			m_rScintilla_TextArea.InsertText(m_rScintilla_TextArea.Lines[2].EndPosition, "\rDemoTextToInsertWithoutQuote");
			Debug.WriteLine("The Annotations of the Line 2 are OK:");
			DebugAnnotations();
			SetAnnotationOnEachLine();

			// Insert a Text with single Quote on line 11
			Debug.WriteLine("\nInserting Text in the middle; End of Line 11");
			m_rScintilla_TextArea.InsertText(m_rScintilla_TextArea.Lines[11].EndPosition, "\r'DemoTextToInsertWITHQuote");
			Debug.Write("The Annotations after Inserting text with quote");
			DebugAnnotations();
			SetAnnotationOnEachLine();

			Debug.WriteLine("\nInserting Text in the middle; One position before End of Line 11");
			m_rScintilla_TextArea.InsertText(m_rScintilla_TextArea.Lines[11].EndPosition - 1, "\r'DemoTextToInsertWITHQuote");
			Debug.Write("The Annotations after Inserting text with quote");
			DebugAnnotations();
			SetAnnotationOnEachLine();

			Debug.WriteLine("\nInserting Text on the end - with WITHOUT	Quote");
			m_rScintilla_TextArea.InsertText(m_rScintilla_TextArea.TextLength, "\rDemoTextToInsertWithoutQuote");
			DebugAnnotations();
			SetAnnotationOnEachLine();

			Debug.WriteLine("\nInserting Text on the end - with WITH Quote");
			m_rScintilla_TextArea.InsertText(m_rScintilla_TextArea.TextLength, "\r'DemoTextToInsertWITHQuote");
			Debug.Write("The Annotations after Inserting text with quote");
			/* Inserted text is on the end of the lines*/
			DebugAnnotations();
			SetAnnotationOnEachLine();
#endif

#if false
			Debug.WriteLine("\nInserting Text on the end after two empty lines - with WITHOUT	Quote");
			SetAnnotationOnEachLine();
			DebugAnnotations();
			int nPositionToInsert = m_rScintilla_TextArea.TextLength;
			bool bIsAddingOnEndOfTheLastEmptyLine = (nPositionToInsert == m_rScintilla_TextArea.TextLength) &&
																							m_rScintilla_TextArea.Lines[m_rScintilla_TextArea.LineFromPosition(nPositionToInsert)].Length == 0;
			//m_rScintilla_TextArea.InsertText(nPositionToInsert, "\rDemoText1\rDemoText2\rDemoText3");
			m_rScintilla_TextArea.InsertText(nPositionToInsert, System.String.Format("{0}{1}{0}{2}{0}{3}", System.Environment.NewLine,"DemoText1", "DemoText2", "DemoText3"));
			DebugAnnotations();
#endif

#if false
			Debug.WriteLine("\nInserting Text in the middle of Text on an filled lines. Position 845 - Ende der Kommentarzeile 03");
			SetAnnotationOnEachLine();
			DebugAnnotations();
			int nPositionToInsert = 845;
			m_rScintilla_TextArea.InsertText(nPositionToInsert, "\rDemoText1\rDemoText2");
			DebugAnnotations();
#endif

			//m_rScintilla_TextArea.AppendText("\n");
			//TODO am Ende - leere Zeile
#if false
			Debug.WriteLine("\nInserting Text am Ende. Nicht leere Zeile");
			SetAnnotationOnEachLine();
			DebugAnnotations();
#endif

			Debug.WriteLine("\n");
			m_rScintilla_TextArea.ClearAll();
			m_rScintilla_TextArea.InsertText(0, "abc\r\r\rdef");
			SetAnnotationOnEachLine();
			DebugAnnotations();
			Debug.WriteLine("Inserting line break with in the middle of a text (end of second line) on an empty line. Annotation will be moved into new line");
			m_rScintilla_TextArea.InsertText(4, "\rnew test");
			DebugAnnotations();

			Debug.WriteLine("\n");
			m_rScintilla_TextArea.ClearAll();
			m_rScintilla_TextArea.InsertText(0, "abc\rSOMETEXT\r\rdef");
			SetAnnotationOnEachLine();
			DebugAnnotations();
			Debug.WriteLine("Inserting line break with in the middle of a text (end of second line) on a non empty line. Annotation will stay on the line of position of insertion.");
			m_rScintilla_TextArea.InsertText(12, "\rnew test");
			DebugAnnotations();

			Debug.WriteLine("\n");
			m_rScintilla_TextArea.ClearAll();
			m_rScintilla_TextArea.InsertText(0, "abc\r\r\r");
			SetAnnotationOnEachLine();
			DebugAnnotations();
			Debug.WriteLine("Inserting line break in the end of a text. Last line ist empty. Annotation will be moved into new line");
			m_rScintilla_TextArea.InsertText(6, "\rnew test");
			DebugAnnotations();

			Debug.WriteLine("\n");
			m_rScintilla_TextArea.ClearAll();
			m_rScintilla_TextArea.InsertText(0, "abc\r\r\rd");
			SetAnnotationOnEachLine();
			DebugAnnotations();
			Debug.WriteLine("Inserting line break in the end of a text. Last line has some text. Annotation will be moved into new line");
			m_rScintilla_TextArea.InsertText(7, "\rnew test");
			DebugAnnotations();
		}



		private void m_rButtonBackDoor_Click(object sender, EventArgs e)
		{
			//DebugWordsAroundCurrentPosition();
			//GetWordEntered();
			//DeleteText();
			//ScrollDemo();
			InsertTextDemo();
			/*
			//TODO_FR Lines for negative indices
			Debug.WriteLine(m_rScintilla_TextArea.Lines[-1].Text);
			Debug.WriteLine(m_rScintilla_TextArea.Lines[m_rScintilla_TextArea.Lines.Count].Text);
			*/
			m_rScintilla_TextArea.Focus();
		}
	}
}
