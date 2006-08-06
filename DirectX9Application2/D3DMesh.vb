Imports System
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.DirectX
Imports Microsoft.DirectX.Direct3D
Imports Direct3D=Microsoft.DirectX.Direct3D


Public Class GraphicsClass
    Inherits GraphicsSample
    Private target As System.Windows.Forms.PictureBox = nothing
    Private groupBox1 As System.Windows.Forms.GroupBox = nothing
    Private drawingFont As GraphicsFont = nothing
    Private x As Single = 0
    Private y As Single = 0
    Private teapot As Mesh = Nothing
    Private lastTick As Integer = Environment.TickCount
    Private Elapsed As Integer = Environment.TickCount
    Private destination As Point = New Point(0, 0)

    Public Sub New()
        Me.MinimumSize = New Size(200,100)
        Me.Text = "DirectX9Application2"
        AddHandler Me.KeyDown, AddressOf Me.OnPrivateKeyDown
        AddHandler Me.KeyUp, AddressOf Me.OnPrivateKeyUp
        drawingFont = New GraphicsFont("Arial", System.Drawing.FontStyle.Bold)

        target = New System.Windows.Forms.PictureBox()
        groupBox1 = New System.Windows.Forms.GroupBox()
        target.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        target.Location = New System.Drawing.Point(155, 20)
        target.Name = "pictureBox1"
        target.Size = New System.Drawing.Size(220, 260)
        target.TabIndex = 0
        target.TabStop = False
        target.Anchor = (AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right)
        groupBox1.Anchor = (AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left)
        groupBox1.Location = New System.Drawing.Point(8, 20)
        groupBox1.Name = "groupBox1"
        groupBox1.Size = New System.Drawing.Size(110, 260)
        groupBox1.TabIndex = 1
        groupBox1.TabStop = False
        groupBox1.Text = "Insert Your UI"

        Me.Controls.AddRange(New System.Windows.Forms.Control() {Me.groupBox1, Me.target})
        Me.RenderTarget = target
    End Sub

    '/ <summary>
    '/ Event Handler for windows messages
    '/ </summary>
    Private Sub OnPrivateKeyDown(sender As Object, e As KeyEventArgs)
        
        Select Case (e.KeyCode)
            Case Keys.Up:
                destination.X = 1
            Case Keys.Down:
                destination.X = -1
            Case Keys.Left:
                destination.Y = 1
            Case Keys.Right:
                destination.Y = -1
        End Select
    End Sub
    Private Sub OnPrivateKeyUp(sender As object , e As KeyEventArgs)

        Select Case (e.KeyCode)
                Case Keys.Up:
                    destination.X = 0
                Case Keys.Down:
                    destination.X = 0
                Case Keys.Left:
                    destination.Y = 0
                Case Keys.Right:
                    destination.Y = 0
        End Select
    End Sub

    '/ <summary>
    '/ Called once per frame, the call is the entry point for 3d rendering. This 
    '/ function sets up render states, clears the viewport, and renders the scene.
    '/ </summary>
    Protected Overrides Sub Render()
        'Clear the backbuffer to a Blue color 
        device.Clear(ClearFlags.Target Or ClearFlags.ZBuffer, Color.Blue, 1.0F, 0)
        'Begin the scene
        device.BeginScene()

        device.Lights(0).Enabled = True

        ' Setup the world, view, and projection matrices
        Dim m As New Matrix()

        If (destination.Y <> 0) Then
            y += DXUtil.Timer(DirectXTimer.GetElapsedTime) * (destination.Y * 25)
        End If
        If (destination.X <> 0) Then
            x += DXUtil.Timer(DirectXTimer.GetElapsedTime) * (destination.X * 25)
        End If
        m = Matrix.RotationY(y)
        m = Matrix.Multiply(m, Matrix.RotationX(x))

        device.Transform.World = m
        device.Transform.View = Matrix.LookAtLH(New Vector3(0.0F, 3.0F, -5.0F), New Vector3(0.0F, 0.0F, 0.0F), New Vector3(0.0F, 1.0F, 0.0F))
        device.Transform.Projection = Matrix.PerspectiveFovLH(Math.PI / 4, 1.0F, 1.0F, 100.0F)

        ' Render the teapot.
        teapot.DrawSubset(0)

        device.EndScene()
    End Sub

    '/ <summary>
    '/ Called when a device needs to be restored.
    '/ </summary>
    Protected Overrides Sub RestoreDeviceObjects(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim mtrl As Material = GraphicsUtility.InitMaterial(Color.White)
        device.Material = mtrl

        device.Lights(0).Type = LightType.Directional
        device.Lights(0).Direction = New Vector3(0.3F, -0.5F, 0.2F)
        device.Lights(0).Diffuse = Color.White

        device.RenderState.Lighting = True
    End Sub

    '/ <summary>
    '/ Initialize scene objects.
    '/ </summary>
    Protected Overrides Sub InitializeDeviceObjects()
        drawingFont.InitializeDeviceObjects(device)
        teapot = Mesh.Teapot(device)
    End Sub
End Class