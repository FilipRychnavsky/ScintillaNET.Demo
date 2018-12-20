// MainForm.cs
// Filip,20180809 10:13:04
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ScintillaNET;
using ScintillaNET.Demo.Utils;

namespace ScintillaNET.Demo {
	public partial class MainForm : Form {
		public MainForm() {
			InitializeComponent();
		}

		ScintillaNET.Scintilla m_rScintilla_TextArea;

		private void MainForm_Load(object sender, EventArgs e) {

			// CREATE CONTROL
			m_rScintilla_TextArea = new ScintillaNET.Scintilla();
			TextPanel.Controls.Add(m_rScintilla_TextArea);

			// BASIC CONFIG
			m_rScintilla_TextArea.Dock = System.Windows.Forms.DockStyle.Fill;
			m_rScintilla_TextArea.TextChanged += (this.OnTextChanged);

			// INITIAL VIEW CONFIG
			m_rScintilla_TextArea.WrapMode = WrapMode.None;
			m_rScintilla_TextArea.IndentationGuides = IndentView.LookBoth;
			//TODO_FR 199 Implement ToolTip between DwellStart und DwellEnd events
			//TODO_FR 299 ToolTip in AutoCompletion m_rScintilla_CodeEditor.AutoCShow(nLengthEntered, sAutoCompletionList);
			//https://github.com/jacobslusser/ScintillaNET/issues/111
			m_rScintilla_TextArea.MouseDwellTime = 400;
			m_rScintilla_TextArea.Styles[Style.CallTip].SizeF = 8.25F;
			m_rScintilla_TextArea.Styles[Style.CallTip].ForeColor = SystemColors.InfoText;
			m_rScintilla_TextArea.Styles[Style.CallTip].BackColor = SystemColors.Info;


			// STYLING
			InitColors();
			InitSyntaxColoring();

			// NUMBER MARGIN
			InitNumberMargin();

			// BOOKMARK MARGIN
			InitBookmarkMargin();

			// CODE FOLDING MARGIN
			InitCodeFolding();

			// DRAG DROP
			InitDragDropFile();

			// DEFAULT FILE
			//LoadDataFromFile("../../MainForm.cs");
			LoadDataFromFile("c:/Users/FRychnavsky/source/repos/Filip/Scintilla/Adressen.Adressen.Lokale CSharp Aktionen.cs");
			// INIT HOTKEYS
			InitHotkeys();

		}


private string GetUrlAtPosition(int position)
{
    // Determine whether the specified position is on our 'URL indicator'
    // and if so whether it is a valid URL.

    var urlIndicator = m_rScintilla_TextArea.Indicators[0];
    var bitmapFlag = (1 << urlIndicator.Index);
    var bitmap = m_rScintilla_TextArea.IndicatorAllOnFor(position);
    var hasUrlIndicator = ((bitmapFlag & bitmap) == bitmapFlag);

    if (hasUrlIndicator)
    {
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
    if (url != null)
    {
        var callTip = string.Format("{0}\nCTRL + click to follow link", url);
        m_rScintilla_TextArea.CallTipShow(e.Position, callTip);
    }
}

private void m_rScintilla_TextArea_DwellEnd(object sender, DwellEventArgs e)
{
    m_rScintilla_TextArea.CallTipCancel();
}

		private void InitColors() {

			m_rScintilla_TextArea.SetSelectionBackColor(true, IntToColor(0x114D9C));

		}

		private void InitHotkeys() {

			// register the hotkeys with the form
			HotKeyManager.AddHotKey(this, OpenSearch, Keys.F, true);
			HotKeyManager.AddHotKey(this, OpenFindDialog, Keys.F, true, false, true);
			HotKeyManager.AddHotKey(this, OpenReplaceDialog, Keys.R, true);
			HotKeyManager.AddHotKey(this, OpenReplaceDialog, Keys.H, true);
			HotKeyManager.AddHotKey(this, Uppercase, Keys.U, true);
			HotKeyManager.AddHotKey(this, Lowercase, Keys.L, true);
			HotKeyManager.AddHotKey(this, ZoomIn, Keys.Oemplus, true);
			HotKeyManager.AddHotKey(this, ZoomOut, Keys.OemMinus, true);
			HotKeyManager.AddHotKey(this, ZoomDefault, Keys.D0, true);
			HotKeyManager.AddHotKey(this, CloseSearch, Keys.Escape);

			// remove conflicting hotkeys from scintilla
			m_rScintilla_TextArea.ClearCmdKey(Keys.Control | Keys.F);
			m_rScintilla_TextArea.ClearCmdKey(Keys.Control | Keys.R);
			m_rScintilla_TextArea.ClearCmdKey(Keys.Control | Keys.H);
			m_rScintilla_TextArea.ClearCmdKey(Keys.Control | Keys.L);
			m_rScintilla_TextArea.ClearCmdKey(Keys.Control | Keys.U);

		}

		private void InitSyntaxColoring() {

			// Configure the default style
			m_rScintilla_TextArea.StyleResetDefault();
			m_rScintilla_TextArea.Styles[Style.Default].Font = "Consolas";
			m_rScintilla_TextArea.Styles[Style.Default].Size = 10;
			m_rScintilla_TextArea.Styles[Style.Default].BackColor = IntToColor(0x212121);
			m_rScintilla_TextArea.Styles[Style.Default].ForeColor = IntToColor(0xFFFFFF);
			m_rScintilla_TextArea.StyleClearAll();

			// Configure the CPP (C#) lexer styles
			m_rScintilla_TextArea.Styles[Style.Cpp.Identifier].ForeColor = IntToColor(0xD0DAE2);
			m_rScintilla_TextArea.Styles[Style.Cpp.Comment].ForeColor = IntToColor(0xBD758B);
			m_rScintilla_TextArea.Styles[Style.Cpp.CommentLine].ForeColor = IntToColor(0x40BF57);
			m_rScintilla_TextArea.Styles[Style.Cpp.CommentDoc].ForeColor = IntToColor(0x2FAE35);
			m_rScintilla_TextArea.Styles[Style.Cpp.Number].ForeColor = IntToColor(0xFFFF00);
			m_rScintilla_TextArea.Styles[Style.Cpp.String].ForeColor = IntToColor(0xFFFF00);
			m_rScintilla_TextArea.Styles[Style.Cpp.Character].ForeColor = IntToColor(0xE95454);
			m_rScintilla_TextArea.Styles[Style.Cpp.Preprocessor].ForeColor = IntToColor(0x8AAFEE);
			m_rScintilla_TextArea.Styles[Style.Cpp.Operator].ForeColor = IntToColor(0xE0E0E0);
			m_rScintilla_TextArea.Styles[Style.Cpp.Regex].ForeColor = IntToColor(0xff00ff);
			m_rScintilla_TextArea.Styles[Style.Cpp.CommentLineDoc].ForeColor = IntToColor(0x77A7DB);
			m_rScintilla_TextArea.Styles[Style.Cpp.Word].ForeColor = IntToColor(0x48A8EE);
			m_rScintilla_TextArea.Styles[Style.Cpp.Word2].ForeColor = IntToColor(0xF98906);
			m_rScintilla_TextArea.Styles[Style.Cpp.CommentDocKeyword].ForeColor = IntToColor(0xB3D991);
			m_rScintilla_TextArea.Styles[Style.Cpp.CommentDocKeywordError].ForeColor = IntToColor(0xFF0000);
			m_rScintilla_TextArea.Styles[Style.Cpp.GlobalClass].ForeColor = IntToColor(0x48A8EE);

			m_rScintilla_TextArea.Lexer = Lexer.Cpp;

			m_rScintilla_TextArea.SetKeywords(0, "class extends implements import interface new case do while else if for in switch throw get set function var try catch finally while with default break continue delete return each const namespace package include use is as instanceof typeof author copy default deprecated eventType example exampleText exception haxe inheritDoc internal link mtasc mxmlc param private return see serial serialData serialField since throws usage version langversion playerversion productversion dynamic private public partial static intrinsic internal native override protected AS3 final super this arguments null Infinity NaN undefined true false abstract as base bool break by byte case catch char checked class const continue decimal default delegate do double descending explicit event extern else enum false finally fixed float for foreach from goto group if implicit in int interface internal into is lock long new null namespace object operator out override orderby params private protected public readonly ref return switch struct sbyte sealed short sizeof stackalloc static string select this throw true try typeof uint ulong unchecked unsafe ushort using var virtual volatile void while where yield");
			m_rScintilla_TextArea.SetKeywords(1, "void Null ArgumentError arguments Array Boolean Class Date DefinitionError Error EvalError Function int Math Namespace Number Object RangeError ReferenceError RegExp SecurityError String SyntaxError TypeError uint XML XMLList Boolean Byte Char DateTime Decimal Double Int16 Int32 Int64 IntPtr SByte Single UInt16 UInt32 UInt64 UIntPtr Void Path File System Windows Forms ScintillaNET");

		}

		private void OnTextChanged(object sender, EventArgs e) {

		}
		

		#region Numbers, Bookmarks, Code Folding

		/// <summary>
		/// the background color of the text area
		/// </summary>
		private const int BACK_COLOR = 0x2A211C;

		/// <summary>
		/// default text color of the text area
		/// </summary>
		private const int FORE_COLOR = 0xB7B7B7;

		/// <summary>
		/// change this to whatever margin you want the line numbers to show in
		/// </summary>
		private const int NUMBER_MARGIN = 1;

		/// <summary>
		/// change this to whatever margin you want the bookmarks/breakpoints to show in
		/// </summary>
		private const int BOOKMARK_MARGIN = 2;
		private const int BOOKMARK_MARKER = 2;

		/// <summary>
		/// change this to whatever margin you want the code folding tree (+/-) to show in
		/// </summary>
		private const int FOLDING_MARGIN = 3;

		/// <summary>
		/// set this true to show circular buttons for code folding (the [+] and [-] buttons on the margin)
		/// </summary>
		private const bool CODEFOLDING_CIRCULAR = true;

		private void InitNumberMargin() {

			m_rScintilla_TextArea.Styles[Style.LineNumber].BackColor = IntToColor(BACK_COLOR);
			m_rScintilla_TextArea.Styles[Style.LineNumber].ForeColor = IntToColor(FORE_COLOR);
			m_rScintilla_TextArea.Styles[Style.IndentGuide].ForeColor = IntToColor(FORE_COLOR);
			m_rScintilla_TextArea.Styles[Style.IndentGuide].BackColor = IntToColor(BACK_COLOR);

			var nums = m_rScintilla_TextArea.Margins[NUMBER_MARGIN];
			nums.Width = 30;
			nums.Type = MarginType.Number;
			nums.Sensitive = true;
			nums.Mask = 0;

			m_rScintilla_TextArea.MarginClick += TextArea_MarginClick;
		}

		private void InitBookmarkMargin() {

			//m_rScintilla_TextArea.SetFoldMarginColor(true, IntToColor(BACK_COLOR));

			var margin = m_rScintilla_TextArea.Margins[BOOKMARK_MARGIN];
			margin.Width = 20;
			margin.Sensitive = true;
			margin.Type = MarginType.Symbol;
			margin.Mask = (1 << BOOKMARK_MARKER);
			//margin.Cursor = MarginCursor.Arrow;

			var marker = m_rScintilla_TextArea.Markers[BOOKMARK_MARKER];
			marker.Symbol = MarkerSymbol.Circle;
			marker.SetBackColor(IntToColor(0xFF003B));
			marker.SetForeColor(IntToColor(0x000000));
			marker.SetAlpha(100);

		}

		private void InitCodeFolding() {

			m_rScintilla_TextArea.SetFoldMarginColor(true, IntToColor(BACK_COLOR));
			m_rScintilla_TextArea.SetFoldMarginHighlightColor(true, IntToColor(BACK_COLOR));

			// Enable code folding
			m_rScintilla_TextArea.SetProperty("fold", "1");
			m_rScintilla_TextArea.SetProperty("fold.compact", "1");

			// Configure a margin to display folding symbols
			m_rScintilla_TextArea.Margins[FOLDING_MARGIN].Type = MarginType.Symbol;
			m_rScintilla_TextArea.Margins[FOLDING_MARGIN].Mask = Marker.MaskFolders;
			m_rScintilla_TextArea.Margins[FOLDING_MARGIN].Sensitive = true;
			m_rScintilla_TextArea.Margins[FOLDING_MARGIN].Width = 20;

			// Set colors for all folding markers
			for (int i = 25; i <= 31; i++) {
				m_rScintilla_TextArea.Markers[i].SetForeColor(IntToColor(BACK_COLOR)); // styles for [+] and [-]
				m_rScintilla_TextArea.Markers[i].SetBackColor(IntToColor(FORE_COLOR)); // styles for [+] and [-]
			}

			// Configure folding markers with respective symbols
			m_rScintilla_TextArea.Markers[Marker.Folder].Symbol = CODEFOLDING_CIRCULAR ? MarkerSymbol.CirclePlus : MarkerSymbol.BoxPlus;
			m_rScintilla_TextArea.Markers[Marker.FolderOpen].Symbol = CODEFOLDING_CIRCULAR ? MarkerSymbol.CircleMinus : MarkerSymbol.BoxMinus;
			m_rScintilla_TextArea.Markers[Marker.FolderEnd].Symbol = CODEFOLDING_CIRCULAR ? MarkerSymbol.CirclePlusConnected : MarkerSymbol.BoxPlusConnected;
			m_rScintilla_TextArea.Markers[Marker.FolderMidTail].Symbol = MarkerSymbol.TCorner;
			m_rScintilla_TextArea.Markers[Marker.FolderOpenMid].Symbol = CODEFOLDING_CIRCULAR ? MarkerSymbol.CircleMinusConnected : MarkerSymbol.BoxMinusConnected;
			m_rScintilla_TextArea.Markers[Marker.FolderSub].Symbol = MarkerSymbol.VLine;
			m_rScintilla_TextArea.Markers[Marker.FolderTail].Symbol = MarkerSymbol.LCorner;

			// Enable automatic folding
			m_rScintilla_TextArea.AutomaticFold = (AutomaticFold.Show | AutomaticFold.Click | AutomaticFold.Change);

		}

		private void TextArea_MarginClick(object sender, MarginClickEventArgs e) {
			if (e.Margin == BOOKMARK_MARGIN) {
				// Do we have a marker for this line?
				const uint mask = (1 << BOOKMARK_MARKER);
				var line = m_rScintilla_TextArea.Lines[m_rScintilla_TextArea.LineFromPosition(e.Position)];
				if ((line.MarkerGet() & mask) > 0) {
					// Remove existing bookmark
					line.MarkerDelete(BOOKMARK_MARKER);
				} else {
					// Add bookmark
					line.MarkerAdd(BOOKMARK_MARKER);
				}
			}
		}

		#endregion

		#region Drag & Drop File

		public void InitDragDropFile() {

			m_rScintilla_TextArea.AllowDrop = true;
			m_rScintilla_TextArea.DragEnter += delegate(object sender, DragEventArgs e) {
				if (e.Data.GetDataPresent(DataFormats.FileDrop))
					e.Effect = DragDropEffects.Copy;
				else
					e.Effect = DragDropEffects.None;
			};
			m_rScintilla_TextArea.DragDrop += delegate(object sender, DragEventArgs e) {

				// get file drop
				if (e.Data.GetDataPresent(DataFormats.FileDrop)) {

					Array a = (Array)e.Data.GetData(DataFormats.FileDrop);
					if (a != null) {

						string path = a.GetValue(0).ToString();

						LoadDataFromFile(path);

					}
				}
			};

		}

		private void LoadDataFromFile(string path) {
			if (File.Exists(path)) {
				FileName.Text = Path.GetFileName(path);
				m_rScintilla_TextArea.Text = File.ReadAllText(path);
			}
		}

		#endregion

		#region Main Menu Commands

		private void openToolStripMenuItem_Click(object sender, EventArgs e) {
			if (openFileDialog.ShowDialog() == DialogResult.OK) {
				LoadDataFromFile(openFileDialog.FileName);
			}
		}

		private void findToolStripMenuItem_Click(object sender, EventArgs e) {
			OpenSearch();
		}

		private void findDialogToolStripMenuItem_Click(object sender, EventArgs e) {
			OpenFindDialog();
		}

		private void findAndReplaceToolStripMenuItem_Click(object sender, EventArgs e) {
			OpenReplaceDialog();
		}

		private void cutToolStripMenuItem_Click(object sender, EventArgs e) {
			m_rScintilla_TextArea.Cut();
		}

		private void copyToolStripMenuItem_Click(object sender, EventArgs e) {
			m_rScintilla_TextArea.Copy();
		}

		private void pasteToolStripMenuItem_Click(object sender, EventArgs e) {
			m_rScintilla_TextArea.Paste();
		}

		private void selectAllToolStripMenuItem_Click(object sender, EventArgs e) {
			m_rScintilla_TextArea.SelectAll();
		}

		private void selectLineToolStripMenuItem_Click(object sender, EventArgs e) {
			Line line = m_rScintilla_TextArea.Lines[m_rScintilla_TextArea.CurrentLine];
			m_rScintilla_TextArea.SetSelection(line.Position + line.Length, line.Position);
		}

		private void clearSelectionToolStripMenuItem_Click(object sender, EventArgs e) {
			m_rScintilla_TextArea.SetEmptySelection(0);
		}

		private void indentSelectionToolStripMenuItem_Click(object sender, EventArgs e) {
			Indent();
		}

		private void outdentSelectionToolStripMenuItem_Click(object sender, EventArgs e) {
			Outdent();
		}

		private void uppercaseSelectionToolStripMenuItem_Click(object sender, EventArgs e) {
			Uppercase();
		}

		private void lowercaseSelectionToolStripMenuItem_Click(object sender, EventArgs e) {
			Lowercase();
		}

		private void wordWrapToolStripMenuItem1_Click(object sender, EventArgs e) {

			// toggle word wrap
			wordWrapItem.Checked = !wordWrapItem.Checked;
			m_rScintilla_TextArea.WrapMode = wordWrapItem.Checked ? WrapMode.Word : WrapMode.None;
		}
		
		private void indentGuidesToolStripMenuItem_Click(object sender, EventArgs e) {

			// toggle indent guides
			indentGuidesItem.Checked = !indentGuidesItem.Checked;
			m_rScintilla_TextArea.IndentationGuides = indentGuidesItem.Checked ? IndentView.LookBoth : IndentView.None;
		}

		private void hiddenCharactersToolStripMenuItem_Click(object sender, EventArgs e) {

			// toggle view whitespace
			hiddenCharactersItem.Checked = !hiddenCharactersItem.Checked;
			m_rScintilla_TextArea.ViewWhitespace = hiddenCharactersItem.Checked ? WhitespaceMode.VisibleAlways : WhitespaceMode.Invisible;
		}

		private void zoomInToolStripMenuItem_Click(object sender, EventArgs e) {
			ZoomIn();
		}

		private void zoomOutToolStripMenuItem_Click(object sender, EventArgs e) {
			ZoomOut();
		}

		private void zoom100ToolStripMenuItem_Click(object sender, EventArgs e) {
			ZoomDefault();
		}

		private void collapseAllToolStripMenuItem_Click(object sender, EventArgs e) {
			m_rScintilla_TextArea.FoldAll(FoldAction.Contract);
		}

		private void expandAllToolStripMenuItem_Click(object sender, EventArgs e) {
			m_rScintilla_TextArea.FoldAll(FoldAction.Expand);
		}
		

		#endregion

		#region Uppercase / Lowercase

		private void Lowercase() {

			// save the selection
			int start = m_rScintilla_TextArea.SelectionStart;
			int end = m_rScintilla_TextArea.SelectionEnd;

			// modify the selected text
			m_rScintilla_TextArea.ReplaceSelection(m_rScintilla_TextArea.GetTextRange(start, end - start).ToLower());

			// preserve the original selection
			m_rScintilla_TextArea.SetSelection(start, end);
		}

		private void Uppercase() {

			// save the selection
			int start = m_rScintilla_TextArea.SelectionStart;
			int end = m_rScintilla_TextArea.SelectionEnd;

			// modify the selected text
			m_rScintilla_TextArea.ReplaceSelection(m_rScintilla_TextArea.GetTextRange(start, end - start).ToUpper());

			// preserve the original selection
			m_rScintilla_TextArea.SetSelection(start, end);
		}

		#endregion

		#region Indent / Outdent

		private void Indent() {
			// we use this hack to send "Shift+Tab" to scintilla, since there is no known API to indent,
			// although the indentation function exists. Pressing TAB with the editor focused confirms this.
			GenerateKeystrokes("{TAB}");
		}

		private void Outdent() {
			// we use this hack to send "Shift+Tab" to scintilla, since there is no known API to outdent,
			// although the indentation function exists. Pressing Shift+Tab with the editor focused confirms this.
			GenerateKeystrokes("+{TAB}");
		}

		private void GenerateKeystrokes(string keys) {
			HotKeyManager.Enable = false;
			m_rScintilla_TextArea.Focus();
			SendKeys.Send(keys);
			HotKeyManager.Enable = true;
		}

		#endregion

		#region Zoom

		private void ZoomIn() {
			m_rScintilla_TextArea.ZoomIn();
		}

		private void ZoomOut() {
			m_rScintilla_TextArea.ZoomOut();
		}

		private void ZoomDefault() {
			m_rScintilla_TextArea.Zoom = 0;
		}


		#endregion

		#region Quick Search Bar

		bool SearchIsOpen = false;

		private void OpenSearch() {

			SearchManager.SearchBox = TxtSearch;
			SearchManager.TextArea = m_rScintilla_TextArea;

			if (!SearchIsOpen) {
				SearchIsOpen = true;
				InvokeIfNeeded(delegate() {
					PanelSearch.Visible = true;
					TxtSearch.Text = SearchManager.LastSearch;
					TxtSearch.Focus();
					TxtSearch.SelectAll();
				});
			} else {
				InvokeIfNeeded(delegate() {
					TxtSearch.Focus();
					TxtSearch.SelectAll();
				});
			}
		}
		private void CloseSearch() {
			if (SearchIsOpen) {
				SearchIsOpen = false;
				InvokeIfNeeded(delegate() {
					PanelSearch.Visible = false;
					//CurBrowser.GetBrowser().StopFinding(true);
				});
			}
		}

		private void BtnClearSearch_Click(object sender, EventArgs e) {
			CloseSearch();
		}

		private void BtnPrevSearch_Click(object sender, EventArgs e) {
			SearchManager.Find(false, false);
		}
		private void BtnNextSearch_Click(object sender, EventArgs e) {
			SearchManager.Find(true, false);
		}
		private void TxtSearch_TextChanged(object sender, EventArgs e) {
			SearchManager.Find(true, true);
		}

		private void TxtSearch_KeyDown(object sender, KeyEventArgs e) {
			if (HotKeyManager.IsHotkey(e, Keys.Enter)) {
				SearchManager.Find(true, false);
			}
			if (HotKeyManager.IsHotkey(e, Keys.Enter, true) || HotKeyManager.IsHotkey(e, Keys.Enter, false, true)) {
				SearchManager.Find(false, false);
			}
		}

		#endregion

		#region Find & Replace Dialog

		private void OpenFindDialog() {

		}
		private void OpenReplaceDialog() {


		}

		#endregion

		#region Utils

		public static Color IntToColor(int rgb) {
			return Color.FromArgb(255, (byte)(rgb >> 16), (byte)(rgb >> 8), (byte)rgb);
		}

		public void InvokeIfNeeded(Action action) {
			if (this.InvokeRequired) {
				this.BeginInvoke(action);
			} else {
				action.Invoke();
			}
		}

		#endregion




	}
}
