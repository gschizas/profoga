Imports System
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.DirectX
Imports Microsoft.DirectX.Direct3D
Imports Direct3D = Microsoft.DirectX.Direct3D

Public Class GraphicsClass
	Inherits GraphicsSample


#Region " Windows Form Designer generated code "

	Public Sub New()
		MyBase.New()

		'This call is required by the Windows Form Designer.
		InitializeComponent()

		'Add any initialization after the InitializeComponent() call
		Me.RenderTarget = target
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
	Friend WithEvents target As System.Windows.Forms.PictureBox
	Friend WithEvents groupBox1 As System.Windows.Forms.GroupBox

	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Me.target = New System.Windows.Forms.PictureBox
		Me.groupBox1 = New System.Windows.Forms.GroupBox
		Me.SuspendLayout()
		'
		'target
		'
		Me.target.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
								Or System.Windows.Forms.AnchorStyles.Left) _
								Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.target.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.target.Location = New System.Drawing.Point(128, 8)
		Me.target.Name = "target"
		Me.target.Size = New System.Drawing.Size(392, 344)
		Me.target.TabIndex = 0
		Me.target.TabStop = False
		'
		'groupBox1
		'
		Me.groupBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
								Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
		Me.groupBox1.Location = New System.Drawing.Point(8, 0)
		Me.groupBox1.Name = "groupBox1"
		Me.groupBox1.Size = New System.Drawing.Size(110, 352)
		Me.groupBox1.TabIndex = 1
		Me.groupBox1.TabStop = False
		Me.groupBox1.Text = "Insert Your UI"
		'
		'ourRenderTarget
		'
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.ClientSize = New System.Drawing.Size(528, 358)
		Me.Controls.Add(Me.groupBox1)
		Me.Controls.Add(Me.target)
		Me.MinimumSize = New System.Drawing.Size(200, 100)
		Me.Name = "ourRenderTarget"
		Me.Text = "DirectX9Application1"
		Me.ResumeLayout(False)

	End Sub

#End Region


	Private drawingFont As GraphicsFont = New GraphicsFont("Arial", System.Drawing.FontStyle.Bold)
	Private x As Single = 0
	Private y As Single = 0
	Private teapot As Mesh = Nothing
	Private lastTick As Integer = Environment.TickCount
	Private Elapsed As Integer = Environment.TickCount
	Private destination As Point = New Point(0, 0)

	'
	'Event Handler for windows messages
	'
	Private Sub OnPrivateKeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles MyBase.KeyDown
		Dim mult As Integer = 1
		If e.Shift Then mult *= 2
		If e.Control Then mult *= 4
		If e.Alt Then mult *= 8

		Select Case (e.KeyCode)
			Case Keys.Up
				destination.X = mult
			Case Keys.Down
				destination.X = -mult
			Case Keys.Left
				destination.Y = mult
			Case Keys.Right
				destination.Y = -mult
		End Select
	End Sub
	Private Sub OnPrivateKeyUp(ByVal sender As Object, ByVal e As KeyEventArgs) Handles MyBase.KeyUp
		Select Case (e.KeyCode)
			Case Keys.Up
				destination.X = 0
			Case Keys.Down
				destination.X = 0
			Case Keys.Left
				destination.Y = 0
			Case Keys.Right
				destination.Y = 0
		End Select
	End Sub

	''' <summary>
	''' Called once per frame, the call is the entry point for 3d rendering. This 
	''' function sets up render states, clears the viewport, and renders the scene.
	''' </summary>
	Protected Overrides Sub Render()
		'Clear the backbuffer to a Blue color 
		Device.Clear(ClearFlags.Target Or ClearFlags.ZBuffer, Color.Blue, 1.0F, 0)
		'Begin the scene
		Device.BeginScene()

		Device.Lights(0).Enabled = True

		' Setup the world, view, and projection matrices
		Dim m As New Matrix

		If (destination.Y <> 0) Then
			y += DXUtil.Timer(DirectXTimer.GetElapsedTime) * (destination.Y * 25)
		End If
		If (destination.X <> 0) Then
			x += DXUtil.Timer(DirectXTimer.GetElapsedTime) * (destination.X * 25)
		End If
		m = Matrix.RotationY(y)
		m = Matrix.Multiply(m, Matrix.RotationX(x))

		Device.Transform.World = m
		Device.Transform.View = Matrix.LookAtLH(New Vector3(0.0F, 3.0F, -5.0F), New Vector3(0.0F, 0.0F, 0.0F), New Vector3(0.0F, 1.0F, 0.0F))
		Device.Transform.Projection = Matrix.PerspectiveFovLH(Math.PI / 4, 1.0F, 1.0F, 100.0F)

		' Render the teapot.
		teapot.DrawSubset(0)

		Device.EndScene()
	End Sub

	''' <summary>
	''' Called when a device needs to be restored.
	''' </summary>
	Protected Overrides Sub RestoreDeviceObjects(ByVal sender As System.Object, ByVal e As System.EventArgs)
		Dim mtrl As Material = GraphicsUtility.InitMaterial(Color.White)
		Device.Material = mtrl

		Device.Lights(0).Type = LightType.Directional
		Device.Lights(0).Direction = New Vector3(0.3F, -0.5F, 0.2F)
		Device.Lights(0).Diffuse = Color.White

		Device.RenderState.Lighting = True
	End Sub

	''' <summary>
	''' Initialize scene objects.
	''' </summary>
	Protected Overrides Sub InitializeDeviceObjects()
		drawingFont.InitializeDeviceObjects(Device)
		'teapot = Mesh.Cylinder(Device, 0.5F, 0.2F, 0.6F, 10, 15)
		'teapot = Mesh.Sphere(Device, 0.5F, 10, 5)		'.Cylinder(Device, 0.5F, 0.2F, 0.6F, 10, 15)
		teapot = Mesh.Teapot(Device)	 'Sphere(Device, 0.5F, 10, 5)		'.Cylinder(Device, 0.5F, 0.2F, 0.6F, 10, 15)
	End Sub

End Class