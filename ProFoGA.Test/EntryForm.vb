Imports System.Xml
Imports System.Xml.Serialization

Public Class EntryForm
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
	Friend WithEvents btnAmino As System.Windows.Forms.Button
	Friend WithEvents btnCritter As System.Windows.Forms.Button
	Friend WithEvents btnExit As System.Windows.Forms.Button
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Me.btnAmino = New System.Windows.Forms.Button
		Me.btnCritter = New System.Windows.Forms.Button
		Me.btnExit = New System.Windows.Forms.Button
		Me.SuspendLayout()
		'
		'btnAmino
		'
		Me.btnAmino.Location = New System.Drawing.Point(8, 8)
		Me.btnAmino.Name = "btnAmino"
		Me.btnAmino.Size = New System.Drawing.Size(152, 48)
		Me.btnAmino.TabIndex = 0
		Me.btnAmino.Text = "&Aminoacid"
		'
		'btnCritter
		'
		Me.btnCritter.Location = New System.Drawing.Point(8, 64)
		Me.btnCritter.Name = "btnCritter"
		Me.btnCritter.Size = New System.Drawing.Size(152, 48)
		Me.btnCritter.TabIndex = 1
		Me.btnCritter.Text = "&Critters"
		'
		'btnExit
		'
		Me.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel
		Me.btnExit.Location = New System.Drawing.Point(8, 160)
		Me.btnExit.Name = "btnExit"
		Me.btnExit.Size = New System.Drawing.Size(152, 48)
		Me.btnExit.TabIndex = 2
		Me.btnExit.Text = "E&xit"
		'
		'EntryForm
		'
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.CancelButton = Me.btnExit
		Me.ClientSize = New System.Drawing.Size(168, 214)
		Me.Controls.Add(Me.btnExit)
		Me.Controls.Add(Me.btnCritter)
		Me.Controls.Add(Me.btnAmino)
		Me.Name = "EntryForm"
		Me.Text = "EntryForm"
		Me.ResumeLayout(False)

	End Sub

#End Region

	Private Sub btnAmino_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAmino.Click
		Dim frmAmino As New AminoForm
		frmAmino.ShowDialog()
	End Sub

	Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
		Me.Close()
	End Sub

	Private Sub btnCritter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCritter.Click
		Dim frmCritter As New CritterForm
		frmCritter.ShowDialog()
	End Sub
End Class
