Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.IO

Class MainWindow

    Public WhichBox = Nothing

    Sub Save()
        'Pb1.Source = New BitmapImage(New Uri("C:\Users\Utku\source\repos\Picture Crop Helper\example.jpg"))


        'Pb2.Source = New CroppedBitmap(Pb1.Source, New Int32Rect(0, 0, Pb1.Source.Width, Pb1.Source.Height))


        'Using stream As FileStream = New FileStream("C:\Users\Utku\Desktop\test.png", FileMode.CreateNew)
        '    Dim encoder = New PngBitmapEncoder
        '    encoder.Frames.Add(BitmapFrame.Create(Pb1.Source))
        '    encoder.Save(stream)
        'End Using


        Dim src = New Bitmap("C:\Users\Utku\source\repos\Picture Crop Helper\example.jpg")
        Dim rect = New Rectangle(Ma.TranslatePoint(New Windows.Point(), Me).X, 0, 640, 360)
        Dim cropped As Bitmap = CropRotatedRect(src, rect, -Test1.Angle, True)

        Dim ms = New MemoryStream()
        CType(cropped, Bitmap).Save(ms, Imaging.ImageFormat.Bmp)

        Dim img = New BitmapImage

        img.BeginInit()
        ms.Seek(0, SeekOrigin.Begin)
        img.StreamSource = ms
        img.EndInit()

        Pb2.Source = img
    End Sub

    Sub LineUpdate()
        Line1.X1 = Box1.Margin.Left
        Line1.X2 = Box2.Margin.Left
        Line1.Y1 = Box1.Margin.Top + 5
        Line1.Y2 = Box2.Margin.Top + 5

        Line2.X1 = Box2.Margin.Left
        Line2.X2 = Box3.Margin.Left
        Line2.Y1 = Box2.Margin.Top + 5
        Line2.Y2 = Box3.Margin.Top + 5

        Line3.X1 = Box3.Margin.Left
        Line3.X2 = Box4.Margin.Left
        Line3.Y1 = Box3.Margin.Top + 5
        Line3.Y2 = Box4.Margin.Top + 5

        Line4.X1 = Box4.Margin.Left
        Line4.X2 = Box1.Margin.Left
        Line4.Y1 = Box4.Margin.Top + 5
        Line4.Y2 = Box1.Margin.Top + 5


        Me.Title = Box3.Margin.Top - Box3.Margin.Left - Box1.Margin.Top & "F"
    End Sub

    Sub ActiveControl(control As Canvas)
        AddHandler control.MouseDown, Sub() WhichBox = control
        AddHandler control.MouseDown, Sub() control.Background = Media.Brushes.SkyBlue
        AddHandler control.MouseUp, Sub() WhichBox = Nothing
        AddHandler control.MouseUp, Sub() control.Background = New BrushConverter().ConvertFrom("#FF139B63")
    End Sub

    Function CropRotatedRect(source As Bitmap, rect As Rectangle, angle As Double, HighQuality As Boolean)
        Dim offsets = {-1, 1, 0}
        Dim result = New Bitmap(rect.Width, rect.Height)

        Using g As Graphics = Graphics.FromImage(result)
            g.InterpolationMode = InterpolationMode.HighQualityBicubic
            g.SmoothingMode = SmoothingMode.HighQuality

            For Each x In offsets
                For Each y In offsets
                    Using mat As Matrix = New Matrix()
                        mat.Translate(-rect.Location.X - rect.Width * x, -rect.Location.Y - rect.Height * y)
                        mat.RotateAt(angle, rect.Location)
                        g.Transform = mat
                        g.DrawImage(source, New Point(0, 0))
                    End Using
                Next
            Next

        End Using

        result.Save("C:\Users\Utku\source\repos\Picture Crop Helper\aboo.jpg")

        Return result


        'Dim result = New Bitmap(rect.Width, rect.Height)
        'Using g As Graphics = Graphics.FromImage(result)

        '    g.InterpolationMode = InterpolationMode.HighQualityBicubic
        '    Using mat As Matrix = New Matrix()
        '        mat.Translate(-rect.Location.X, -rect.Location.Y)
        '        mat.RotateAt(angle, rect.Location)
        '        g.Transform = mat
        '        g.DrawImage(source, New Point(0, 0))
        '    End Using
        'End Using
        'Return result
    End Function

    Private Sub MainWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        LineUpdate()
        ActiveControl(Box1)
        ActiveControl(Box2)
        ActiveControl(Box3)
        ActiveControl(Box4)
    End Sub

    Private Sub MainWindow_MouseMove(sender As Object, e As MouseEventArgs) Handles Me.MouseMove
        If Not WhichBox Is Nothing Then
            If Mouse.LeftButton Then
                WhichBox.Margin = New Thickness(e.GetPosition(Me).X, e.GetPosition(Me).Y, 0, 0)
            End If
            LineUpdate()
        End If
    End Sub

    Private Sub Sl1_ValueChanged(sender As Object, e As RoutedPropertyChangedEventArgs(Of Double)) Handles Sl1.ValueChanged
        'Pb2.RenderTransform = New RotateTransform(Sl1.Value)

        'Dim xx As New TransformedBitmap()
        'xx.BeginInit()
        'xx.Source = Pb2.Source
        'xx.Transform = New RotateTransform(Sl1.Value)
        'xx.EndInit()
        'Pb2.Source = xx
    End Sub
End Class
