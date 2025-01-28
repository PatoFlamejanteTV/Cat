Imports System.Drawing
Imports System.Threading
Imports System.Runtime.InteropServices


Public Class Form2
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        TextBox1.Text = "Wrong :("
        MessageBox.Show("kaboom betch ;)")

        RainbowScreen.StartRainbow()
        WindowTitleChanger.StartChangingTitle()
    End Sub

    Public Class WindowTitleChanger

        ' Import necessary Win32 functions
        <DllImport("user32.dll", CharSet:=CharSet.Ansi)>
        Public Shared Function SetWindowTextA(hWnd As IntPtr, lpString As String) As Boolean
        End Function


        <DllImport("user32.dll")>
        Public Shared Function GetForegroundWindow() As IntPtr
        End Function

        Private Shared isRunning As Boolean = False
        Private Shared rnd As New Random()

        Public Shared Sub StartChangingTitle()
            If isRunning Then Return
            isRunning = True
            Dim t As New Thread(AddressOf ChangeTitleThread)
            t.Start()
        End Sub

        Public Shared Sub StopChangingTitle()
            isRunning = False
        End Sub

        Private Shared Sub ChangeTitleThread()
            Try
                'While isRunning
                Dim titleChars(70) As Char ' Equivalent to CHAR local_9[0x46]; (+1 for null terminator)
                    For i As Integer = 0 To 69 ' Loop from 0 to 69 (inclusive)
                        Dim uVar1 As Integer = Environment.TickCount
                        Dim uVar3 As UInteger = CUInt(uVar1 Xor (uVar1 << 13))
                        uVar3 = uVar3 Xor (uVar3 << 17)
                        titleChars(i) = ChrW(CUInt((uVar3 Xor uVar3 >> 5) Mod 71) + 48) ' Mod 0x47 (71), +48 to make it printable

                    Next
                    Dim title As String = New String(titleChars, 0, 70)
                    Dim hWnd As IntPtr = GetForegroundWindow()
                SetWindowTextA(hWnd, title)

                Thread.Sleep(10)
                'If Not isRunning Then Exit While
                'End While
            Catch ex As Exception
                'Handle exceptions such as the window closing.
            Finally
                'Any clean up if needed.
            End Try
        End Sub

        ' A more accurate replacement for rdtsc if needed (requires unsafe code)
        <DllImport("kernel32.dll")>
        Private Shared Function QueryPerformanceCounter(ByRef lpPerformanceCount As Long) As Boolean
        End Function

        <DllImport("kernel32.dll")>
        Private Shared Function QueryPerformanceFrequency(ByRef lpFrequency As Long) As Boolean
        End Function

        Private Shared Function GetPreciseTimestamp() As Long
            Dim counter As Long = 0
            QueryPerformanceCounter(counter)
            Return counter
        End Function
    End Class

    Public Class RainbowScreen

        ' Import necessary Win32 functions
        <DllImport("gdi32.dll")>
        Private Shared Function CreateSolidBrush(crColor As Integer) As IntPtr
        End Function

        <DllImport("gdi32.dll")>
        Private Shared Function SelectObject(hdc As IntPtr, hObject As IntPtr) As IntPtr
        End Function

        <DllImport("gdi32.dll")>
        Private Shared Function PatBlt(hdc As IntPtr, x As Integer, y As Integer, w As Integer, h As Integer, rop As UInteger) As Boolean
        End Function

        <DllImport("gdi32.dll")>
        Private Shared Function DeleteObject(hObject As IntPtr) As Boolean
        End Function

        <DllImport("user32.dll")>
        Private Shared Function GetDC(hWnd As IntPtr) As IntPtr
        End Function

        <DllImport("user32.dll")>
        Private Shared Function ReleaseDC(hWnd As IntPtr, hDC As IntPtr) As Integer
        End Function

        <DllImport("user32.dll")>
        Private Shared Function GetDesktopWindow() As IntPtr
        End Function


        Private Shared isRunning As Boolean = False
        Private Shared rnd As New Random()

        Public Shared Sub StartRainbow()
            If isRunning Then Return
            isRunning = True
            Dim t As New Thread(AddressOf RainbowThread)
            t.Start()
        End Sub

        Public Shared Sub StopRainbow()
            isRunning = False
        End Sub

        Private Shared Sub RainbowThread()
            Dim x As Integer = Screen.PrimaryScreen.Bounds.Width
            Dim y As Integer = Screen.PrimaryScreen.Bounds.Height

            Dim desktopHandle As IntPtr = GetDesktopWindow()
            Dim hdc As IntPtr = GetDC(desktopHandle)

            Try
                While isRunning
                    ' Generate random RGB color
                    Dim r As Integer = rnd.Next(256) ' 0-255
                    Dim g As Integer = rnd.Next(256)
                    Dim b As Integer = rnd.Next(256)

                    ' Create COLORREF value
                    Dim colorRef As Integer = b * 65536 + g * 256 + r

                    Dim brush As IntPtr = CreateSolidBrush(colorRef)

                    If brush <> IntPtr.Zero Then ' Check if brush creation was successful
                        Dim oldBrush As IntPtr = SelectObject(hdc, brush)

                        PatBlt(hdc, 0, 0, x, y, &H5A0049) ' PATINVERT

                        SelectObject(hdc, oldBrush) ' Restore the old brush
                        DeleteObject(brush) ' Delete the created brush
                    End If


                    Thread.Sleep(10)
                    If Not isRunning Then Exit While 'Check if the thread should stop after the sleep.
                End While

            Finally
                ReleaseDC(desktopHandle, hdc)
            End Try
        End Sub
    End Class



End Class