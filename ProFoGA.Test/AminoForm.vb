Imports System.Xml
Imports System.Xml.Serialization

Public Class AminoForm
	Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

	Public Sub New()
		MyBase.New()

		'This call is required by the Windows Form Designer.
		InitializeComponent()

		'Add any initialization after the InitializeComponent() call

	End Sub

	'Form overrides dispose to clean up the component list.
	Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
		If disposing Then
			If Not (components Is Nothing) Then
				components.Dispose()
			End If
		End If
		MyBase.Dispose(disposing)
	End Sub

	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer

	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.  
	'Do not modify it using the code editor.
	Friend WithEvents fdlgOpenAmino As System.Windows.Forms.OpenFileDialog
	Friend WithEvents tbMain As System.Windows.Forms.ToolBar
	Friend WithEvents tbOpen1 As System.Windows.Forms.ToolBarButton
	Friend WithEvents tbOpen2 As System.Windows.Forms.ToolBarButton
	Friend WithEvents ilToolbarButtons As System.Windows.Forms.ImageList
	Friend WithEvents pnlMain As System.Windows.Forms.Panel
	Friend WithEvents pbDisplay As System.Windows.Forms.PictureBox
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container
		Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(AminoForm))
		Me.fdlgOpenAmino = New System.Windows.Forms.OpenFileDialog
		Me.tbMain = New System.Windows.Forms.ToolBar
		Me.tbOpen1 = New System.Windows.Forms.ToolBarButton
		Me.tbOpen2 = New System.Windows.Forms.ToolBarButton
		Me.ilToolbarButtons = New System.Windows.Forms.ImageList(Me.components)
		Me.pnlMain = New System.Windows.Forms.Panel
		Me.pbDisplay = New System.Windows.Forms.PictureBox
		Me.pnlMain.SuspendLayout()
		Me.SuspendLayout()
		'
		'fdlgOpenAmino
		'
		Me.fdlgOpenAmino.Filter = "Aminoacid Files|*.amn"
		'
		'tbMain
		'
		Me.tbMain.Buttons.AddRange(New System.Windows.Forms.ToolBarButton() {Me.tbOpen1, Me.tbOpen2})
		Me.tbMain.DropDownArrows = True
		Me.tbMain.ImageList = Me.ilToolbarButtons
		Me.tbMain.Location = New System.Drawing.Point(0, 0)
		Me.tbMain.Name = "tbMain"
		Me.tbMain.ShowToolTips = True
		Me.tbMain.Size = New System.Drawing.Size(292, 28)
		Me.tbMain.TabIndex = 1
		Me.tbMain.TextAlign = System.Windows.Forms.ToolBarTextAlign.Right
		'
		'tbOpen1
		'
		Me.tbOpen1.ImageIndex = 0
		Me.tbOpen1.Tag = "open1"
		Me.tbOpen1.Text = "Open &1"
		'
		'tbOpen2
		'
		Me.tbOpen2.ImageIndex = 0
		Me.tbOpen2.Tag = "open2"
		Me.tbOpen2.Text = "Open &2"
		'
		'ilToolbarButtons
		'
		Me.ilToolbarButtons.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit
		Me.ilToolbarButtons.ImageSize = New System.Drawing.Size(16, 16)
		Me.ilToolbarButtons.ImageStream = CType(resources.GetObject("ilToolbarButtons.ImageStream"), System.Windows.Forms.ImageListStreamer)
		Me.ilToolbarButtons.TransparentColor = System.Drawing.Color.Magenta
		'
		'pnlMain
		'
		Me.pnlMain.Controls.Add(Me.pbDisplay)
		Me.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill
		Me.pnlMain.Location = New System.Drawing.Point(0, 28)
		Me.pnlMain.Name = "pnlMain"
		Me.pnlMain.Size = New System.Drawing.Size(292, 238)
		Me.pnlMain.TabIndex = 2
		'
		'pbDisplay
		'
		Me.pbDisplay.BackColor = System.Drawing.Color.White
		Me.pbDisplay.Dock = System.Windows.Forms.DockStyle.Fill
		Me.pbDisplay.Location = New System.Drawing.Point(0, 0)
		Me.pbDisplay.Name = "pbDisplay"
		Me.pbDisplay.Size = New System.Drawing.Size(292, 238)
		Me.pbDisplay.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
		Me.pbDisplay.TabIndex = 0
		Me.pbDisplay.TabStop = False
		'
		'AminoForm
		'
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.ClientSize = New System.Drawing.Size(292, 266)
		Me.Controls.Add(Me.pnlMain)
		Me.Controls.Add(Me.tbMain)
		Me.Name = "AminoForm"
		Me.Text = "AminoForm"
		Me.pnlMain.ResumeLayout(False)
		Me.ResumeLayout(False)

	End Sub

#End Region
    Private amino1 As New ProFoGA.Main.Amino
    Private amino2 As New ProFoGA.Main.Amino

	Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim amino As New ProFoGA.Main.Amino

	End Sub

	Private Sub tbMain_ButtonClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles tbMain.ButtonClick
		If e.Button.Tag = "open1" Then LoadAndPaintAmino(amino1)
		If e.Button.Tag = "open2" Then LoadAndPaintAmino(amino2)
	End Sub

    Private Sub LoadAndPaintAmino(ByRef oneAmino As ProFoGA.Main.Amino)
        If fdlgOpenAmino.ShowDialog = Windows.Forms.DialogResult.OK Then
            oneAmino.Open(fdlgOpenAmino.FileName)
            pbDisplay.Image = New Bitmap(800, 600)
            For x As Integer = 0 To 799 : For y As Integer = 0 To 599
                    CType(pbDisplay.Image, Bitmap).SetPixel(x, y, Drawing.Color.Coral)
            Next y, x
            oneAmino.DisplayOptions = ProFoGA.Main.Amino.AtomDisplayOptions.Atoms
            oneAmino.Display(System.Drawing.Graphics.FromImage(pbDisplay.Image))
        End If

        Dim txtOut As New System.Text.StringBuilder

        Dim s As New XmlSerializer(GetType(ProFoGA.Main.Amino))
        Dim t As New System.IO.StringWriter(txtOut)

        s.Serialize(t, oneAmino)
        t.Close()

        'MessageBox.Show(txtOut.ToString)
        'Clipboard.SetDataObject(txtOut.ToString)

    End Sub
End Class
