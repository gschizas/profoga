<Serializable(), Xml.Serialization.XmlRoot(ElementName:="aminoacid")> _
Public Class Amino
  Public Sub New()

  End Sub
  Public Sub DehydrateC()
    Atom(DehydC1).Name = "*"
    Atom(DehydC2).Name = "*"

    For i As Byte = 1 To 6
      Atom(DehydC1).link(i).atom = -1
    Next

    For i As Byte = 1 To 6
      If Atom(DehydC2).link(i).atom = DehydC1 Then Atom(DehydC2).link(i).atom = -1
    Next

  End Sub
  Public Sub DehydrateN()
    Atom(Me.DehydN - 1).Name = "* "
  End Sub
  Public Sub Display(ByVal graphSurface As System.Drawing.Graphics)
    Const siz As Integer = 15

    Dim i, j As Byte
    Dim z, cx, cy, sx, sy, scx, scy, tx, ty As Integer
    Dim s, sz As Double
    Dim c As String
    Dim gPen As System.Drawing.Pen
    Dim gBrush As System.Drawing.Brush
    Dim gFont As System.Drawing.Font
    Dim pnt1, pnt2 As System.Drawing.PointF
    Dim rect As System.Drawing.RectangleF
    Dim sizF As System.Drawing.SizeF

    sx = graphSurface.ClipBounds.Width
    sy = graphSurface.ClipBounds.Height

    cx = graphSurface.ClipBounds.Width \ 2     ' sx \ 2 + g.ClipBounds.Left
    cy = graphSurface.ClipBounds.Height \ 2        'sy \ 2 + g.ClipBounds.Top

    scx = sx \ 10
    scy = sy \ 10

    gPen = New System.Drawing.Pen(System.Drawing.Color.Black)
    Dim rect1 As New RectangleF(cx, cy, 100, 100)

    graphSurface.DrawRectangle(gPen, rect1.X, rect1.Y, rect1.Width, rect1.Height)

    Exit Sub

    For z = 100 To -100 Step -1
      For i = 0 To Atom.Length - 1
        'If (Atom(i).Z >= (z / 10)) And (Atom(i).Z < ((z + 1) / 10)) Then

        ' Draw Bonds

        For j = 0 To 5
          If Atom(i).link(j).Atom <> -1 Then
            If (Atom(Atom(i).link(j).Atom).Z < ((z + 1) / 10)) Then
              gPen.Width = IIf(Atom(i).link(j).Bond = BondType.Single, 1, 4)

              pnt1 = New PointF
              sz = siz / (siz + Atom(i).Z)
              pnt1.X = CType(cx + (sx * Atom(i).X * sz), Single)
              pnt1.Y = CType(cy + (scy * Atom(i).Y * sz), Single)

              pnt2 = New PointF
              sz = siz / (siz + Atom(Atom(i).link(j).Atom).Z)
              pnt2.X = CType(cx + (scx * Atom(Atom(i).link(j).Atom).X * sz), Single)
              pnt2.Y = CType(cy + (scy * Atom(Atom(i).link(j).Atom).Y * sz), Single)
              graphSurface.DrawLine(gPen, pnt1, pnt2)
            End If
          End If
        Next

        s = siz
        sz = siz / (siz + Atom(i).Z)
        If Atom(i).Name = "H" Then s = siz \ 2

        Select Case Atom(i).Name
          Case "C"
            gBrush = New System.Drawing.SolidBrush(Drawing.Color.Gray)
          Case "H"
            gBrush = New System.Drawing.SolidBrush(Drawing.Color.Aqua)
          Case "O"
            gBrush = New System.Drawing.SolidBrush(Drawing.Color.Red)
          Case "N"
            gBrush = New System.Drawing.SolidBrush(Drawing.Color.Navy)
          Case "S"
            gBrush = New System.Drawing.SolidBrush(Drawing.Color.Yellow)
          Case Else
            gBrush = New System.Drawing.SolidBrush(Drawing.Color.White)
        End Select


        gPen.Width = 1

        If Me.DisplayOptions.Atoms Then
          pnt1 = New PointF
          sz = siz / (siz + Atom(i).Z)
          pnt1.X = CType(cx + (s * sz) + (sz * scx * (Atom(i).X)), Single)
          pnt1.Y = CType(cy + (s * sz) + (sz * scy * (Atom(i).Y)), Single)

          pnt2 = New PointF
          'sz = siz / (siz + Atom(Atom(i).link(j).atom).Z)
          pnt2.X = CType(cx - (s * sz) + (sz * scx * (Atom(i).X)), Single)
          pnt2.Y = CType(cy - (s * sz) + (sz * scy * (Atom(i).Y)), Single)

          sizF = New SizeF(pnt2.X - pnt1.X, pnt2.Y - pnt1.Y)

          rect = New RectangleF(pnt1, sizF)

          graphSurface.DrawEllipse(gPen, rect)

        End If

        If Me.DisplayOptions.Numbers And Atom(i).Name <> "*" Then
          If Atom(i).Charge = 1 Then c = "+"
          If Atom(i).Charge = 0 Then c = ""
          If Atom(i).Charge = -1 Then c = "-"

          'c = i.ToString()					'Diagnostics

          gFont = New System.Drawing.Font("Tahoma", 8)
          gBrush = New System.Drawing.SolidBrush(System.Drawing.Color.Black)
          If Atom(i).Name = "N" Then gBrush = New System.Drawing.SolidBrush(System.Drawing.Color.White)
          sizF = graphSurface.MeasureString(c, gFont)
          tx = sizF.Width
          ty = sizF.Height
          pnt1 = New PointF
          pnt1.X = cx - tx \ 2 + (sz * scx * (Atom(i).X))
          pnt1.Y = cy - ty \ 2 + (sz * scy * (Atom(i).Y))
          graphSurface.DrawString(c, gFont, gBrush, pnt1)
        End If
        'End If
        '{				ACanvas.Brush.Color:=clWhite;
        '				ACanvas.Font.Color:=clBlack;
        '				if Atom[i].name='N ' then
        '					ACanvas.Font.Color:=clWhite;
        '				ACanvas.Brush.Style:=bsClear;
        '
        '				if Atom[i].Charge=1 then
        '				begin
        '					ACanvas.Brush.Style:=bsSolid;
        '					ACanvas.Brush.Color:=clYellow;
        '					ACanvas.Font.Color:=clBlack;
        '				end;
        '				if Atom[i].Charge=-1 then
        '				begin
        '					ACanvas.Brush.Style:=bsSolid;
        '					ACanvas.Brush.Color:=clGreen;
        '					ACanvas.Font.Color:=clWhite;
        '				end;
        '
        '        if Atom[i].Name='* ' then
        '        	ACanvas.Font.Style:=[fsItalic,fsStrikeOut]
        '        else
        '        	ACanvas.Font.Style:=[];
        '
        '				ACanvas.Font.Name:='Tahoma';
        '				ACanvas.Font.Size:=9;
        '
        '
        '				tx:=ACanvas.TextWidth(IntToStr(i));
        '				ty:=ACanvas.TextHeight(IntToStr(i));
        '				ACanvas.TextOut(cx-tx div 2+Trunc(sz*scx*(Atom[i].X)),
        '					Trunc(cy-ty div 2+sz*scy*(Atom[i].Y)),
        '					IntToStr(i));
        '
        '          }
      Next
    Next


  End Sub
  Public Sub Move(ByVal dx As Double, ByVal dy As Double, ByVal dz As Double)
    For i As Byte = 0 To Atom.Length - 1
      Atom(i).X += dx
      Atom(i).Y += dy
      Atom(i).Z += dz
    Next
  End Sub
  Public Sub Open(ByVal fname As String)
    Me.Name = System.IO.Path.GetFileNameWithoutExtension(fname).ToUpper
    Dim i, j, n, a1, a2 As Byte
    Dim c As Short
    Dim line As String
    Dim amn As New System.IO.StreamReader(fname)
    n = amn.ReadLine()
    Atom = Array.CreateInstance(GetType(Atom), n)
    For i = 0 To n - 1          'Read Atoms
      With Atom(i)
        .Name = amn.ReadLine.Trim
        line = System.Text.RegularExpressions.Regex.Replace(amn.ReadLine.Trim, "  +", " ").Replace(".", ",")
        .X = Double.Parse(line.Split()(0))
        .Y = Double.Parse(line.Split()(1))
        .Z = Double.Parse(line.Split()(2))
        line = System.Text.RegularExpressions.Regex.Replace(amn.ReadLine.Trim, "  +", " ")
        .link = Array.CreateInstance(GetType(BondLink), 6)
        For j = 0 To 5
          .link(j).atom = line.Split()(j) - 1
          .link(j).bond = BondType.Single
        Next
        .Charge = 0
      End With
    Next
    line = System.Text.RegularExpressions.Regex.Replace(amn.ReadLine.Trim, "  +", " ")
    DehydN = line.Split()(0) - 1
    DehydC1 = line.Split()(1) - 1
    DehydC2 = line.Split()(2) - 1
    n = amn.ReadLine
    For i = 1 To n
      line = System.Text.RegularExpressions.Regex.Replace(amn.ReadLine.Trim, "  +", " ")
      a1 = line.Split()(0) - 1
      a2 = line.Split()(1) - 1
      For j = 0 To 5
        If Atom(a1).link(j).atom = a2 Then Atom(a1).link(j).bond = BondType.Double
        If Atom(a2).link(j).atom = a1 Then Atom(a2).link(j).bond = BondType.Double
      Next
    Next

    n = amn.ReadLine
    For i = 1 To n
      c = amn.ReadLine - 1
      Atom(Math.Abs(c)).Charge = Math.Sign(c)
    Next

    If Atom(DehydC1).Name = "O" Then
      i = DehydC1
      DehydC1 = DehydC2
      DehydC2 = i
    End If
    amn.Close()

  End Sub

  Public Sub Rotate(ByVal x1 As Double, ByVal y1 As Double, ByVal z1 As Double, ByVal x2 As Double, ByVal y2 As Double, ByVal z2 As Double, ByVal th As Double)
    Dim i As Byte
    Dim s, s2 As Double
    Dim tx, ty, tz, tl As Double
    Dim a, b, c As Double
    Dim x, y, z As Double

    If th < 0 Then th = th + 2 * Math.PI
    If th = 0 Then Exit Sub
    s = Math.Cos(th / 2)
    s2 = Math.Sin(th / 2)
    tx = x2 - x1
    ty = y2 - y1
    tz = z2 - z1
    tl = Math.Sqrt(tx * tx + ty * ty + tz * tz)
    If tl = 0 Then Exit Sub
    a = s2 * (tx / tl)
    b = s2 * (ty / tl)
    c = s2 * (tz / tl)
    Move(-x1, -y1, -z1)
    For i = 0 To Atom.Length - 1
      x = Atom(i).X
      y = Atom(i).Y
      z = Atom(i).Z
      Atom(i).X = (1 - 2 * b * b - 2 * c * c) * x + (2 * a * b - 2 * s * c) * y + (2 * a * c + 2 * s * b) * z
      Atom(i).Y = (2 * a * b + 2 * s * c) * x + (1 - 2 * a * a - 2 * c * c) * y + (2 * b * c - 2 * s * a) * z
      Atom(i).Z = (2 * a * c - 2 * s * b) * x + (2 * b * c + 2 * s * a) * y + (1 - 2 * a * a - 2 * b * b) * z
    Next
    Move(x1, y1, z1)
  End Sub
  Public Sub RotateAngC(ByVal th As Double)

  End Sub
  Public Sub RotateAngN(ByVal th As Double)

  End Sub

  Private _atom() As Atom

  <Xml.Serialization.XmlArray("atoms"), Xml.Serialization.XmlArrayItem("atom")> _
  Public Property Atom() As Atom()
    Get
      Return _atom
    End Get
    Set(ByVal value As Atom())
      _atom = value
    End Set
  End Property

  Public DehydC1 As Integer
  Public DehydC2 As Integer
  Public DehydN As Integer

  <Xml.Serialization.XmlIgnore()> _
  Public DisplayOptions As AtomDisplayOptions
  'Public num As Byte

  <Xml.Serialization.XmlAttributeAttribute("name")> _
  Public Name As String

  <Flags()> _
  Public Enum AtomDisplayOptions
    Numbers = 1
    Atoms = 2
  End Enum

  Public Sub Join(ByRef OneAmino As Amino, ByRef TwoAmino As Amino)

    Dim i As Integer
    Dim bond1, bond2, bond3, bond4 As Integer
    Dim A, B, C, D As Double
    Dim A1, B1, C1, D1 As Double
    Dim A2, B2, C2, D2 As Double
    Dim x1, y1, z1, x2, y2, z2 As Double
    Dim x3, y3, z3, x4, y4, z4 As Double
    Dim l1, l2 As Double
    Dim co, si, th As Double

    bond1 = Atom(OneAmino.DehydC2).link(0).atom
    bond2 = Atom(TwoAmino.DehydN).link(0).atom

    '	Move to lock 1st atom
    TwoAmino.Move( _
     Atom(bond1).X - Atom(TwoAmino.DehydN).X, _
     Atom(bond1).Y - Atom(TwoAmino.DehydN).Y, _
     Atom(bond1).Z - Atom(TwoAmino.DehydN).Z)


    '	Rotate to lock 2nd atom
    x1 = Atom(OneAmino.DehydC2).X
    y1 = Atom(OneAmino.DehydC2).Y
    z1 = Atom(OneAmino.DehydC2).Z

    x2 = Atom(bond1).X
    y2 = Atom(bond1).Y
    z2 = Atom(bond1).Z

    x3 = Atom(bond2).X
    y3 = Atom(bond2).Y
    z3 = Atom(bond2).Z


    A = y1 * (z2 - z3) + y2 * (z3 - z1) + y3 * (z1 - z2)
    B = z1 * (x2 - x3) + z2 * (x3 - x1) + z3 * (x1 - x2)
    C = x1 * (y2 - y3) + x2 * (y3 - y1) + x3 * (y1 - y2)
    D = -x1 * (y2 * z3 - y3 * z2) - x2 * (y3 * z1 - y1 * z3) - x3 * (y1 * z2 - y2 * z1)

    l1 = Math.Sqrt((x1 - x2) ^ 2 + (y1 - y2) ^ 2 + (z1 - z2) ^ 2)
    l2 = Math.Sqrt((x3 - x2) ^ 2 + (y3 - y2) ^ 2 + (z3 - z2) ^ 2)


    co = ((x1 - x2) * (x3 - x2) + (y1 - y2) * (y3 - y2) + (z1 - z2) * (z3 - z2)) / (l1 * l2)
    si = Math.Sqrt(Math.Abs(1 - co ^ 2))

    If co = 0 Then th = Math.PI / 2 Else th = Math.Atan(si / co)

    If (x1 - x2) * x3 + (y1 - y2) * y3 + (z1 - z2) * z3 < (x1 - x2) * x2 + (y1 - y2) * y2 + (z1 - z2) * z2 Then th = Math.PI + th

    TwoAmino.Rotate(x2, y2, z2, x2 + A, y2 + B, z2 + C, th)


    '	Find O and H of CO=NH Bond
    bond3 = -1
    bond4 = -1

    For i = 0 To 5
      If Atom(Atom(bond1).link(i).atom).Name = "O" Then bond3 = Atom(bond1).link(i).atom
      If Atom(Atom(bond2).link(i).atom).Name = "H" Then bond4 = Atom(bond2).link(i).atom
    Next

    ' Proline - special case, no H bonded to N

    If bond4 = -1 Then
      bond4 = 10              ' Really Dirty Hack, but I suppose it will work
    End If

    ' Rotate to make bond flat

    x1 = Atom(bond1).X
    y1 = Atom(bond1).Y
    z1 = Atom(bond1).Z

    x2 = Atom(bond2).X
    y2 = Atom(bond2).Y
    z2 = Atom(bond2).Z

    x3 = Atom(bond3).X
    y3 = Atom(bond3).Y
    z3 = Atom(bond3).Z

    x4 = Atom(bond4).X
    y4 = Atom(bond4).Y
    z4 = Atom(bond4).Z

    '	find equation of plane O,C,N (3,1,2)
    A1 = y3 * (z1 - z2) + y1 * (z2 - z3) + y2 * (z3 - z1)
    B1 = z3 * (x1 - x2) + z1 * (x2 - x3) + z2 * (x3 - x1)
    C1 = x3 * (y1 - y2) + x1 * (y2 - y3) + x2 * (y3 - y1)
    D1 = -x3 * (y1 * z2 - y2 * z1) - x1 * (y2 * z3 - y3 * z2) - x2 * (y3 * z1 - y1 * z3)

    ' find equation of plane H,N,C (4,2,1)
    A2 = y4 * (z2 - z1) + y2 * (z1 - z4) + y1 * (z4 - z2)
    B2 = z4 * (x2 - x1) + z2 * (x1 - x4) + z1 * (x4 - x2)
    C2 = x4 * (y2 - y1) + x2 * (y1 - y4) + x1 * (y4 - y2)
    D2 = -x4 * (y2 * z1 - y1 * z2) - x2 * (y1 * z4 - y4 * z1) - x1 * (y4 * z2 - y2 * z4)

    '	l1=Sqrt(Sqr(x1-x3)+Sqr(y1-y3)+Sqr(z1-z3))
    '	l2=Sqrt(Sqr(x2-x4)+Sqr(y2-y4)+Sqr(z2-z4))
    '	co=((x1-x3)*(x2-x4)+(y1-y3)*(y2-y4)+(z1-z3)*(z2-z4))/(l1*l2)

    co = Math.Abs((A1 * A2 + B1 * B2 + C1 * C2) / (Math.Sqrt(A1 * A1 + B1 * B1 + C1 * C1) * Math.Sqrt(A2 * A2 + B2 * B2 + C2 * C2)))

    If co > 1 Then co = 1
    If co < -1 Then co = -1

    si = Math.Sqrt(1 - co ^ 2)

    If co = 0 Then th = Math.PI / 2 Else th = Math.Atan(si / co)

    If A1 * x4 + B1 * y4 + C1 * z4 + D1 > 0 Then th = -th

    TwoAmino.Rotate(x1, y1, z1, x2, y2, z2, -th)


  End Sub


End Class